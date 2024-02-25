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
import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const StatusReporting: React.FC = () => {
  const { role, teamId, teamName, memberName, memberId } = userStore();
  const [localTeamName, setLocalTeamName] = useState(teamName);
  const [teamWeeklyStatusData, setTeamWeeklyStatusData] =
    useState<TeamWeeklyStatusData | null>(null);
  const [unreportedMembers, setUnreportedMembers] = useState<
    TeamMemberWeeklyStatusData[]
  >([]);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate, setStartDate] = useState(initialStartDate);

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
  }, []);

  useEffect(() => {
    const fetchTeamWeeklyStatus = async () => {
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
        weeklyStatus?.doneThisWeek?.forEach(taskWithSubtasks => {
          htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
          if (taskWithSubtasks.subtasks && taskWithSubtasks.subtasks.length > 0) {
            htmlContent += `<ul>`;
            taskWithSubtasks.subtasks.forEach(subtask => {
              htmlContent += `<li>${subtask.subtaskDescription}</li>`;
            });
            htmlContent += `</ul>`;
          }
          htmlContent += `</li>`;
        });
        htmlContent += `</ul>`;
        htmlContent += `<h4>Plan for next week:</h4>`;
        htmlContent += `<ul>`;
        weeklyStatus?.planForNextWeek?.forEach((task: string) => {
          htmlContent += `<li>${task}</li>`;
        });
        htmlContent += `</ul>`;
        htmlContent += `<h4>Blockers:</h4>`;
        htmlContent += `<p>${weeklyStatus?.blockers ?? "None"}</p>`;
        htmlContent += `<h4>Upcoming PTO:</h4>`;
        const datesList = weeklyStatus?.upcomingPTO?.map((date) =>
          moment(date).format("MMM DD")
        );
        if (datesList?.length) {
          htmlContent += datesList.join(", ");
        }
      });

    return htmlContent;
  };

  const editorData = generateHTML();

  const handleBack = async () => {
    navigate("/weekly-status");
  };

  return (
    <>
      <div className="card mt-5 div__container">
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

      <div className="d-flex flex-column mt-2 div__secondary">
        <span className="div__secondary__content">
          Changos who haven't reported yet:
        </span>{" "}
        {unreportedMembers.map((member) => member.memberName).join(", ")}
      </div>

      <div className="form__buttons">
        <Button variant="secondary" onClick={handleBack} className="mt-3 ml-2">
          Back
        </Button>
      </div>
    </>
  );
};

export default StatusReporting;
