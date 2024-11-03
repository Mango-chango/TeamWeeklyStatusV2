import React, { useEffect, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import {
  TeamMemberWeeklyStatusData,
  TeamWeeklyStatusData,
} from "../../types/WeeklyStatus.types";
import moment from "moment";
import "./styles.css";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { Button, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { generateHTML, generateMarkdown, generatePDF } from "./reportService";
import { console } from "inspector";

const StatusReporting: React.FC = () => {
  const { teamId, teamName } = userStore();
  const [localTeamName, setLocalTeamName] = useState(teamName);
  const [teamWeeklyStatusData, setTeamWeeklyStatusData] =
    useState<TeamWeeklyStatusData | null>(null);
  const [unreportedMembers, setUnreportedMembers] = useState<
    TeamMemberWeeklyStatusData[]
  >([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  console.log("teamWeeklyStatusData", teamWeeklyStatusData);

  const initialStartDate = moment()
    .startOf("week")
    .toDate();
  const [startDate] = useState(initialStartDate);
  const endDate = moment().endOf("week").toDate();

  const navigate = useNavigate();

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
      const response: TeamWeeklyStatusData = await makeApiRequest(
        "/WeeklyStatus/GetAllWeeklyStatusesByStartDate",
        "POST",
        requestData
      );

      if (response) {
        setTeamWeeklyStatusData(response);
        const membersWhoDidNotReport = response.filter(
          (member) => !member.weeklyStatus
        );
        setUnreportedMembers(membersWhoDidNotReport);
      }
      setIsLoading(false);
    };

    fetchTeamWeeklyStatus();
  }, [localTeamName, startDate, teamId]);

  const editorData = generateHTML(
    localTeamName || "",
    startDate,
    endDate,
    teamWeeklyStatusData || []
  );

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

  return (
    <div className="status-reporting-container">
      <h5 className="status-reporting-header">
        This is a readonly view. The changes done here are not persisted in the
        database.
      </h5>

      <div className="status-reporting-buttons">
        <Button variant="secondary" onClick={handleBack} className="mt-3">
          Back
        </Button>
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
      </div>

      {isLoading ? (
        <div className="status-reporting-loading">
          <Spinner animation="border" variant="primary" />
        </div>
      ) : (
        <div className="status-reporting-editor">
          <CKEditor
            editor={ClassicEditor}
            data={editorData}
            config={{
              toolbar: ['heading', '|', 'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote', '|', 'selectAll', 'undo', 'redo'],
            }}
          />
        </div>
      )}

      <div className="status-reporting-unreported">
        <span className="unreported-title">
          Changos who haven't reported yet:
        </span>{" "}
        {unreportedMembers.map((member) => member.memberName).join(", ")}
      </div>
    </div>
  );
};

export default StatusReporting;
