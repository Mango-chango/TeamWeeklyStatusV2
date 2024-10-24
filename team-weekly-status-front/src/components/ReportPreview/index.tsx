import React, { useEffect, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import { WeeklyStatusData } from "../../types/WeeklyStatus.types";
import moment from "moment";
import "./styles.css";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const ReportPreview: React.FC = () => {
  const { teamId, memberId } = userStore();
  const [existingWeeklyStatus, setExistingWeeklyStatus] = useState<WeeklyStatusData | null>(null);

  const startDate = moment().startOf("week").toDate();

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
  }, [memberId, teamId, startDate]);

  const generateHTML = () => {
    if (!existingWeeklyStatus) return "";

    let htmlContent = `<h4>What was done this week:</h4><ul>`;
    existingWeeklyStatus.doneThisWeek?.forEach((taskWithSubtasks) => {
      htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
      if (taskWithSubtasks.subtasks?.length) {
        htmlContent += `<ul>`;
        taskWithSubtasks.subtasks.forEach((subtask) => {
          htmlContent += `<li>${subtask.subtaskDescription}</li>`;
        });
        htmlContent += `</ul>`;
      }
      htmlContent += `</li>`;
    });
    htmlContent += `</ul>`;

    htmlContent += `<h4>Plan for next week:</h4><ul>`;
    existingWeeklyStatus.planForNextWeek?.forEach((taskWithSubtasks) => {
      htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
      if (taskWithSubtasks.subtasks?.length) {
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
    htmlContent += `<p>${existingWeeklyStatus.blockers || "None"}</p>`;

    htmlContent += `<h4>Upcoming Time Off:</h4>`;
    const datesList = existingWeeklyStatus.upcomingPTO?.map((date) =>
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
      <h5 className="readonly-message">
        This is a readonly view. Changes made here are not saved.
      </h5>
      <div className="div__container">
        <CKEditor
          editor={ClassicEditor}
          data={editorData}
          config={{
            toolbar: [
            ],
            // Remove 'shouldNotGroupWhenFull' as it's not supported
          }}
          disabled={true}
        />
      </div>
    </>
  );
};

export default ReportPreview;