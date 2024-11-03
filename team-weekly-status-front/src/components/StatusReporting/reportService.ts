import {
  TeamMemberWeeklyStatusData,
  TeamWeeklyStatusData,
} from "../../types/WeeklyStatus.types";
import moment from "moment";
import jsPDF from "jspdf";

export const generateHTML = (
  teamName: string,
  startDate: Date,
  endDate: Date,
  teamWeeklyStatusData: TeamWeeklyStatusData
): string => {
  let htmlContent = `<h2>${teamName} Weekly Status Report</h2>`;
  htmlContent += `<h3>${startDate.toDateString()} - ${endDate.toDateString()}</h3>`;

  teamWeeklyStatusData
    .filter(({ weeklyStatus }) => weeklyStatus !== null)
    .forEach(({ memberName, weeklyStatus }) => {
      htmlContent += `<h3>${memberName}</h3>`;
      htmlContent += `<h4>What was done this week:</h4>`;
      htmlContent += `<ul>`;
      weeklyStatus?.doneThisWeek?.forEach((taskWithSubtasks) => {
        htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
        if (
          taskWithSubtasks.subtasks &&
          taskWithSubtasks.subtasks.length > 0
        ) {
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
      weeklyStatus?.planForNextWeek?.forEach((taskWithSubtasks) => {
        htmlContent += `<li>${taskWithSubtasks.taskDescription}`;
        if (
          taskWithSubtasks.subtasks &&
          taskWithSubtasks.subtasks.length > 0
        ) {
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
      htmlContent += `<p>${weeklyStatus?.blockers ?? "None"}</p>`;

      htmlContent += `<h4>Upcoming Time Off:</h4>`;
      const datesList = weeklyStatus?.upcomingPTO?.map((date) =>
        moment(date).format("MMM DD")
      );
      if (datesList?.length) {
        htmlContent += `<p>${datesList.join(", ")}</p>`;
      } else {
        htmlContent += `<p>None</p>`;
      }
    });

  return htmlContent;
};

export const generateMarkdown = (
  teamName: string,
  startDate: Date,
  endDate: Date,
  teamWeeklyStatusData: TeamWeeklyStatusData
): string => {
  let markdownContent = `# ${teamName} Weekly Status Report\n\n`;
  markdownContent += `## ${startDate.toDateString()} - ${endDate.toDateString()}\n\n`;

  teamWeeklyStatusData
    .filter(({ weeklyStatus }) => weeklyStatus !== null)
    .forEach(({ memberName, weeklyStatus }) => {
      markdownContent += `### ${memberName}\n\n`;
      markdownContent += `#### What was done this week:\n`;
      weeklyStatus?.doneThisWeek?.forEach((taskWithSubtasks) => {
        markdownContent += `- ${taskWithSubtasks.taskDescription}\n`;
        taskWithSubtasks.subtasks?.forEach((subtask) => {
          markdownContent += `  - ${subtask.subtaskDescription}\n`;
        });
      });
      markdownContent += `\n#### Plan for next week:\n`;
      weeklyStatus?.planForNextWeek?.forEach((taskWithSubtasks) => {
        markdownContent += `- ${taskWithSubtasks.taskDescription}\n`;
        taskWithSubtasks.subtasks?.forEach((subtask) => {
          markdownContent += `  - ${subtask.subtaskDescription}\n`;
        });
      });
      markdownContent += `\n#### Blockers:\n`;
      markdownContent += `${weeklyStatus?.blockers ?? "None"}\n\n`;
      markdownContent += `#### Upcoming Time Off:\n`;
      const datesList = weeklyStatus?.upcomingPTO?.map((date) =>
        moment(date).format("MMM DD")
      );
      if (datesList?.length) {
        markdownContent += `${datesList.join(", ")}\n\n`;
      } else {
        markdownContent += `None\n\n`;
      }
    });

  return markdownContent;
};

export const generatePDF = async (
  teamName: string,
  startDate: Date,
  endDate: Date,
  teamWeeklyStatusData: TeamWeeklyStatusData
): Promise<jsPDF> => {
  const doc = new jsPDF();

  // Add content to the PDF
  doc.setFontSize(18);
  doc.text(
    `${teamName}
    Weekly Status Report`,
    14,
    22
  );
  doc.setFontSize(14);
  doc.text(
    `${startDate.toDateString()}
    - ${endDate.toDateString()}`,
    14,
    32
  );

  let yPosition = 42;

  teamWeeklyStatusData
    .filter(({ weeklyStatus }) => weeklyStatus !== null)
    .forEach(({ memberName, weeklyStatus }) => {
      doc.setFontSize(16);
      doc.text(memberName, 14, yPosition);
      yPosition += 10;

      doc.setFontSize(14);
      doc.text("What was done this week:", 14, yPosition);
      yPosition += 8;

      weeklyStatus?.doneThisWeek?.forEach((taskWithSubtasks) => {
        doc.text(`- ${taskWithSubtasks.taskDescription}`, 18, yPosition);
        yPosition += 8;
        taskWithSubtasks.subtasks?.forEach((subtask) => {
          doc.text(`  - ${subtask.subtaskDescription}`, 22, yPosition);
          yPosition += 8;
        });
      });

      // Repeat for Plan for next week, Blockers, Upcoming Time Off
      weeklyStatus?.planForNextWeek?.forEach((taskWithSubtasks) => {
        doc.text(`- ${taskWithSubtasks.taskDescription}`, 18, yPosition);
        yPosition += 8;
        taskWithSubtasks.subtasks?.forEach((subtask) => {
          doc.text(`  - ${subtask.subtaskDescription}`, 22, yPosition);
          yPosition += 8;
        });
      });

      doc.text("Blockers:", 14, yPosition);
      yPosition += 8;
      doc.text(`${weeklyStatus?.blockers ?? "None"}`, 18, yPosition);
      yPosition += 8;

      doc.text("Upcoming Time Off:", 14, yPosition);
      yPosition += 8;
      const datesList = weeklyStatus?.upcomingPTO?.map((date) =>
        moment(date).format("MMM DD")
      );
      if (datesList?.length) {
        doc.text(datesList.join(", "), 18, yPosition);
        yPosition += 8;
      } else {
        doc.text("None", 18, yPosition);
        yPosition += 8;
      }


      // Check if yPosition exceeds page height
      if (yPosition > doc.internal.pageSize.height - 20) {
        doc.addPage();
        yPosition = 20;
      }
    });

  return doc;
};
