import React, { useEffect, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import {
  TeamMemberWeeklyStatusData,
  TeamWeeklyStatusData,
} from "../../types/WeeklyStatus.types";
import moment from "moment";
import "./reporting.css";
import { CKEditor } from "@ckeditor/ckeditor5-react";

import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const StatusReporting: React.FC = () => {
  const { role, teamName, memberName, memberId } = userStore();
  const [localTeamName, setLocalTeamName] = useState(teamName);
  const [teamWeeklyStatusData, setTeamWeeklyStatusData] =
    useState<TeamWeeklyStatusData | null>(null);
  const [unreportedMembers, setUnreportedMembers] = useState<
    TeamMemberWeeklyStatusData[]
  >([]);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate, setStartDate] = useState(initialStartDate);

  const endDate = moment().endOf("week").toDate();

  useEffect(() => {
    setLocalTeamName(teamName);

    const unsubscribe = userStore.subscribe((state) => {
      if (state.teamName !== localTeamName) {
        setLocalTeamName(state.teamName);
      }
    });

    return () => unsubscribe();
  }, []);

  useEffect(() => {
    const fetchTeamWeeklyStatus = async () => {
      const requestData = {
        memberId: null,
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
    };

    fetchTeamWeeklyStatus();
  }, [localTeamName, startDate]);

  const generateHTML = () => {
    if (!teamWeeklyStatusData) return "";

    let htmlContent = `<h2>${localTeamName} Weekly Status Report</h2>`;
    htmlContent += `<h3>${startDate.toDateString()} - ${endDate.toDateString()}</h3>`;

    teamWeeklyStatusData
      .filter(({ weeklyStatus }) => weeklyStatus !== null)
      .forEach(({ memberName, weeklyStatus }) => {
        htmlContent += `<h3>${memberName}</h3>`;
        htmlContent += `<h4>What was done this week:</h4>`;
        htmlContent += `<ul>`;
        weeklyStatus?.doneThisWeek?.forEach((task: string) => {
          htmlContent += `<li>${task}</li>`;
        });
        htmlContent += `</ul>`;
        htmlContent += `<h4>Plan for next week:</h4>`;
        htmlContent += `<ul>`;
        weeklyStatus?.planForNextWeek?.forEach((task: string) => {
          htmlContent += `<li>${task}</li>`;
        });
        htmlContent += `</ul>`;
        htmlContent += `<h4>Blockers:</h4>`;
        htmlContent += `<p>${weeklyStatus?.blockers}</p>`;
        htmlContent += `<h4>Upcoming PTO:</h4>`;
        htmlContent += `<ul>`;
        weeklyStatus?.upcomingPTO?.forEach((date) => {
          htmlContent += `<li>${moment(date).format("MMM DD")}</li>`;
        });
        htmlContent += `</ul>`;
      });

    return htmlContent;
  };

  const editorData = generateHTML();

  return (
    <>
      <div className="card mt-5 centered-div">
        <div className="card-body card-content">
          <CKEditor
            editor={ClassicEditor}
            data={editorData}
            config={{
              toolbar: [
                "heading",
                "|",
                "bold",
                "italic",
                "link",
                "bulletedList",
                "numberedList",
                "blockQuote",
                "insertTable",
                "undo",
                "redo",
                "selectAll",
                "copy",
              ],
            }}
          />

        </div>
      </div>

      <div className="d-flex flex-column mt-2 centered-left-aligned-content">
        <span style={{ fontWeight: "bolder" }}>
          Members still not reported:
        </span>{" "}
        {unreportedMembers.map((member) => member.memberName).join(", ")}
      </div>
    </>
  );
};

export default StatusReporting;
