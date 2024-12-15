import React, { useEffect, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import {
  TeamMemberWeeklyStatusRichTextData,
  TeamWeeklyRichTextStatusData,
} from "../../types/WeeklyStatus.types";
import moment from "moment";
import "./styles.css";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import { Button, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { generateHTML, generateMarkdown, generatePDF } from "./reportService";

const StatusReporting: React.FC = () => {
  const { teamId, teamName } = userStore();
  const [localTeamName, setLocalTeamName] = useState(teamName);
  const [teamWeeklyStatusData, setTeamWeeklyStatusData] =
    useState<TeamWeeklyRichTextStatusData | null>(null);
  const [unreportedMembers, setUnreportedMembers] = useState<
    TeamMemberWeeklyStatusRichTextData[]
  >([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate] = useState(initialStartDate);
  const endDate = moment().endOf("week").toDate();

  const navigate = useNavigate();

  const [editorHtml, setEditorHtml] = useState("");

  useEffect(() => {
    setLocalTeamName(teamName);

    const unsubscribe = userStore.subscribe((state) => {
      if (state.teamName !== localTeamName) {
        setLocalTeamName(state.teamName);
      }
    });

    return () => unsubscribe();
  }, [localTeamName, teamName]);

  useEffect(() => {
    const fetchTeamWeeklyStatus = async () => {
      setIsLoading(true);
      const requestData = {
        memberId: null,
        teamId: teamId,
        weekStartDate: startDate.toISOString(),
      };
      const response: TeamWeeklyRichTextStatusData = await makeApiRequest(
        "/v2.0/WeeklyStatus/GetAllWeeklyStatusesByStartDate", // Updated endpoint
        "POST",
        requestData
      );

      if (response) {
        setTeamWeeklyStatusData(response);
        const membersWhoDidNotReport = response.filter(
          (member) => (!member.weeklyStatus?.doneThisWeekContent && !member.weeklyStatus?.planForNextWeekContent)
        );
        setUnreportedMembers(membersWhoDidNotReport);
      }
      setIsLoading(false);
    };

    fetchTeamWeeklyStatus();
  }, [localTeamName, startDate, teamId]);

  const editorData = generateHTML(
    localTeamName ?? "",
    startDate,
    endDate,
    teamWeeklyStatusData || []
  );
  useEffect(() => {
    setEditorHtml(editorData);
  }, [editorData]);

  const handleBack = () => {
    navigate("/weekly-status");
  };

  const handleDownloadMarkdown = () => {
    if (!teamWeeklyStatusData) return;

    const markdownContent = generateMarkdown(
      localTeamName,
      startDate,
      endDate,
      teamWeeklyStatusData
    );

    const blob = new Blob([markdownContent], {
      type: "text/markdown;charset=utf-8;",
    });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.setAttribute(
      "download",
      `${localTeamName}-Weekly-Status-${startDate.toDateString()}.md`
    );
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const handleDownloadPDF = async () => {
    if (!teamWeeklyStatusData) return;

    const doc = await generatePDF(
      localTeamName || "",
      startDate,
      endDate,
      teamWeeklyStatusData
    );

    doc.save(`${localTeamName}-Weekly-Status-${startDate.toDateString()}.pdf`);
  };

  const handleDownloadHTML = () => {
    if (!editorHtml) return;

    // Wrap the content in a basic HTML structure for better email compatibility
    const htmlContent = `<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8" />
<title>${localTeamName}-Weekly-Status</title>
<style> .ql-indent-1 { margin-left: 2em; list-style-type: disc; } </style>
<style> h3 { font-size: 16px !important; } </style>
</head>
<body style="font-family: Times New Roman, sans-serif; font-size: 14px !important;">
${editorHtml}
</body>
</html>`;

    const blob = new Blob([htmlContent], { type: "text/html" });
    const url = URL.createObjectURL(blob);

    const a = document.createElement("a");
    a.href = url;
    a.download = `${localTeamName}-Weekly-Status-${startDate.toDateString()}.html`;
    document.body.appendChild(a);
    a.click();

    // Cleanup
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  };

  return (
    <div className="status-reporting-container">
      <h5 className="status-reporting-header">
        This is a read-only view. The changes done here are not persisted in the
        database.
      </h5>

      <div className="status-reporting-buttons">
        <Button variant="secondary" onClick={handleBack} className="mt-3">
          Back
        </Button>
        {/*
        <Button
          variant="primary"
          onClick={handleDownloadPDF}
          className="mt-3 ml-2"
          disabled={!teamWeeklyStatusData || isLoading}
        >
          Download PDF
        </Button>
        <Button
          variant="primary"
          onClick={handleDownloadMarkdown}
          className="mt-3 ml-2"
          disabled={!teamWeeklyStatusData || isLoading}
        >
          Download Markdown
        </Button>
        */}
        <Button
          variant="primary"
          onClick={handleDownloadHTML}
          className="mt-3 ml-2"
          disabled={isLoading}
        >
          Download HTML
        </Button>
      </div>

      {isLoading ? (
        <div className="status-reporting-loading">
          <Spinner animation="border" variant="primary" />
        </div>
      ) : (
        <div className="status-reporting-editor">
          <ReactQuill
            value={editorHtml}
            theme="bubble" // Or "snow" based on your preference
            modules={{ toolbar: false }}
          />
        </div>
      )}

      <div className="status-reporting-unreported">
        <span className="unreported-title">
          Members who haven't reported yet:
        </span>{" "}
        {unreportedMembers.map((member) => member.memberName).join(", ")}
      </div>
    </div>
  );
};

export default StatusReporting;
