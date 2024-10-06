import React, { useEffect, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import { WeeklyStatusData } from "../../types/WeeklyStatus.types";
import moment from "moment";
import "./styles.css";
import { CKEditor } from "@ckeditor/ckeditor5-react";

import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const ReportPreview: React.FC = () => {
  const { teamId, teamName, memberName, memberId } = userStore();
  const [localMemberId, setLocalMemberId] = useState(memberId);
  const [localTeamName, setLocalTeamName] = useState(teamName);
  const [existingWeeklyStatus, setExistingWeeklyStatus] =
    useState<WeeklyStatusData | null>(null);

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
    const fetchExistingStatus = async () => {
      const requestData = {
        memberId: memberId,
        teamId: teamId,
        weekStartDate: startDate.toISOString(),
      };
      const response: WeeklyStatusData = await makeApiRequest(
        "/WeeklyStatus/GetByMemberIdAndStartDate",
        "POST",
        requestData
      );

      if (response) {
        setExistingWeeklyStatus(response);
      }
    };

    fetchExistingStatus();
  }, [localMemberId, startDate]);

  const generateHTML = () => {
    if (!existingWeeklyStatus) return "";

    let htmlContent = ``;
    htmlContent += `<h4>What was done this week:</h4>`;
    htmlContent += `<ul>`;
    existingWeeklyStatus?.doneThisWeek?.forEach((taskWithSubtasks) => {
      htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
      if (taskWithSubtasks.subtasks && taskWithSubtasks.subtasks.length > 0) {
        htmlContent += `<ul>`;
        taskWithSubtasks.subtasks.forEach((subtask) => {
          htmlContent += `<li>${subtask.subtaskDescription}</li>`;
        });
        htmlContent += `</ul>`;
      }
      htmlContent += `</li>`;
    });
    htmlContent += `</ul>`;
    htmlContent += `<h4>Plan for next week:</h4>`;
    htmlContent += `<ul>`;
    existingWeeklyStatus?.planForNextWeek?.forEach((taskWithSubtasks) => {
      htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
      if (taskWithSubtasks.subtasks && taskWithSubtasks.subtasks.length > 0) {
        htmlContent += `<ul>`;
        taskWithSubtasks.subtasks.forEach((subtask) => {
          htmlContent += `<li>${subtask.subtaskDescription}</li>`;
        });
        htmlContent += `</ul>`;
      }
      htmlContent += `</li>`;
    });
    htmlContent += `</ul>`;
    htmlContent += `<h4>Blockers:</h4>`;
    htmlContent += `<p>${existingWeeklyStatus?.blockers ?? "None"}</p>`;
    htmlContent += `<h4>Upcoming Time Off:</h4>`;
    const datesList = existingWeeklyStatus?.upcomingPTO?.map((date) =>
      moment(date).format("MMM DD")
    );
    if (datesList?.length) {
      htmlContent += datesList.join(", ");
    }

    return htmlContent;
  };

  const editorData = generateHTML();

  return (
    <>
      <h5 style={{ textAlign: "center", paddingTop: "20px" }}>This is a readonly view. The changes done here are not persisted in the database.</h5>
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
    </>
  );
};

export default ReportPreview;
