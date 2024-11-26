import { TeamWeeklyRichTextStatusData } from "../../types/WeeklyStatus.types";
import moment from "moment";
import jsPDF from "jspdf";
import TurndownService from "turndown";
//import WeeklyStatus from '../WeeklyStatus/index';

// Helper function to strip HTML tags
const stripHtml = (html: string): string => {
  const tempDiv = document.createElement("div");
  tempDiv.innerHTML = html;
  return tempDiv.textContent || tempDiv.innerText || "";
};

export const generateHTML = (
  teamName: string,
  startDate: Date,
  endDate: Date,
  teamWeeklyStatusData: TeamWeeklyRichTextStatusData
): string => {
  let htmlContent = `<h2>${teamName} Weekly Status Report</h2>`;
  htmlContent += `<h3>${startDate.toDateString()} - ${endDate.toDateString()}</h3>`;

  teamWeeklyStatusData
    .filter(({ weeklyStatus }) => weeklyStatus !== null)
    .forEach(({ memberName, weeklyStatus }) => {
      if (
        weeklyStatus?.doneThisWeekContent !== null &&
        weeklyStatus?.doneThisWeekContent !== undefined &&
        weeklyStatus?.planForNextWeekContent !== null &&
        weeklyStatus?.planForNextWeekContent !== undefined
      ) {
        htmlContent += `<h3>${memberName}</h3>`;
        htmlContent += `<h4>What was done this week:</h4>`;
        htmlContent += weeklyStatus?.doneThisWeekContent || "<p>None</p>";

        htmlContent += `<h4>Plan for next week:</h4>`;
        htmlContent += weeklyStatus?.planForNextWeekContent || "<p>None</p>";

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
      }
    });

  return htmlContent;
};

export const generateMarkdown = (
  teamName: string,
  startDate: Date,
  endDate: Date,
  teamWeeklyStatusData: TeamWeeklyRichTextStatusData
): string => {
  const turndownService = new TurndownService();

  let markdownContent = `# ${teamName} Weekly Status Report\n\n`;
  markdownContent += `## ${startDate.toDateString()} - ${endDate.toDateString()}\n\n`;

  teamWeeklyStatusData
    .filter(({ weeklyStatus }) => weeklyStatus !== null)
    .forEach(({ memberName, weeklyStatus }) => {
      if (
        weeklyStatus?.doneThisWeekContent !== null &&
        weeklyStatus?.doneThisWeekContent !== undefined &&
        weeklyStatus?.planForNextWeekContent !== null &&
        weeklyStatus?.planForNextWeekContent !== undefined
      ) {
        markdownContent += `### ${memberName}\n\n`;
        markdownContent += `#### What was done this week:\n`;
        const doneThisWeekMarkdown = weeklyStatus?.doneThisWeekContent
          ? turndownService.turndown(weeklyStatus?.doneThisWeekContent)
          : "None";
        markdownContent += `${doneThisWeekMarkdown}\n\n`;

        markdownContent += `#### Plan for next week:\n`;
        const planForNextWeekMarkdown = weeklyStatus?.planForNextWeekContent
          ? turndownService.turndown(weeklyStatus?.planForNextWeekContent)
          : "None";
        markdownContent += `${planForNextWeekMarkdown}\n\n`;

        markdownContent += `#### Blockers:\n`;
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
      }
    });

  return markdownContent;
};

export const generatePDF = async (
  teamName: string,
  startDate: Date,
  endDate: Date,
  teamWeeklyStatusData: TeamWeeklyRichTextStatusData
): Promise<jsPDF> => {
  const doc = new jsPDF();

  // Define page dimensions and margins
  const pageHeight =
    doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
  const pageWidth =
    doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
  const marginLeft = 14;
  const marginRight = 14;
  const marginTop = 20;
  const marginBottom = 20;
  const maxYPosition = pageHeight - marginBottom;

  // Add content to the PDF
  doc.setFontSize(18);
  doc.setFont("helvetica", "bold");
  doc.text(`${teamName} Weekly Status Report`, marginLeft, marginTop);

  doc.setFontSize(14);
  doc.setFont("helvetica", "normal");
  doc.text(
    `${startDate.toDateString()} - ${endDate.toDateString()}`,
    marginLeft,
    marginTop + 10
  );

  let yPosition = marginTop + 20;

  teamWeeklyStatusData
    .filter(({ weeklyStatus }) => weeklyStatus !== null)
    .forEach(({ memberName, weeklyStatus }) => {
      if (
        weeklyStatus?.doneThisWeekContent !== null &&
        weeklyStatus?.doneThisWeekContent !== undefined &&
        weeklyStatus?.planForNextWeekContent !== null &&
        weeklyStatus?.planForNextWeekContent !== undefined
      ) {
        // Check if we need to add a new page before adding member name
        if (yPosition + 10 > maxYPosition) {
          doc.addPage();
          yPosition = marginTop;
        }

        // Member Name in Bold
        doc.setFontSize(16);
        doc.setFont("helvetica", "bold");
        doc.text(memberName, marginLeft, yPosition);
        yPosition += 8;

        // Section Titles in Bold
        doc.setFontSize(14);
        doc.setFont("helvetica", "bold");
        doc.text("What was done this week:", marginLeft, yPosition);
        yPosition += 6;

        // Reset font to normal for content
        doc.setFont("helvetica", "normal");

        // Content: What was done this week
        const doneThisWeekText = weeklyStatus?.doneThisWeekContent
          ? stripHtml(weeklyStatus?.doneThisWeekContent)
          : "None";
        yPosition = addTextToPDF(
          doc,
          doneThisWeekText,
          marginLeft + 4,
          yPosition,
          maxYPosition,
          pageWidth,
          marginTop
        );

        // Section Title: Plan for next week
        if (yPosition + 10 > maxYPosition) {
          doc.addPage();
          yPosition = marginTop;
        }
        doc.setFont("helvetica", "bold");
        doc.text("Plan for next week:", marginLeft, yPosition);
        yPosition += 6;
        doc.setFont("helvetica", "normal");

        // Content: Plan for next week
        const planForNextWeekText = weeklyStatus?.planForNextWeekContent
          ? stripHtml(weeklyStatus?.planForNextWeekContent)
          : "None";
        yPosition = addTextToPDF(
          doc,
          planForNextWeekText,
          marginLeft + 4,
          yPosition,
          maxYPosition,
          pageWidth,
          marginTop
        );

        // Section Title: Blockers
        if (yPosition + 10 > maxYPosition) {
          doc.addPage();
          yPosition = marginTop;
        }
        doc.setFont("helvetica", "bold");
        doc.text("Blockers:", marginLeft, yPosition);
        yPosition += 6;
        doc.setFont("helvetica", "normal");

        const blockers1 = weeklyStatus?.blockers || "None";
        yPosition = addTextToPDF(
          doc,
          blockers1,
          marginLeft + 4,
          yPosition,
          maxYPosition,
          pageWidth,
          marginTop
        );

        // Section Title: Upcoming Time Off
        if (yPosition + 10 > maxYPosition) {
          doc.addPage();
          yPosition = marginTop;
        }
        doc.setFont("helvetica", "bold");
        doc.text("Upcoming Time Off:", marginLeft, yPosition);
        yPosition += 6;
        doc.setFont("helvetica", "normal");

        const datesList = weeklyStatus?.upcomingPTO?.map((date) =>
          moment(date).format("MMM DD")
        );
        const ptoText = datesList?.length ? datesList.join(", ") : "None";
        yPosition = addTextToPDF(
          doc,
          ptoText,
          marginLeft + 4,
          yPosition,
          maxYPosition,
          pageWidth,
          marginTop
        );

        // Add extra space before next member
        yPosition += 10;
      }
    });

  return doc;
};

// Helper function to add plain text to PDF with wrapping
const addTextToPDF = (
  doc: jsPDF,
  text: string,
  marginLeft: number,
  yPosition: number,
  maxYPosition: number,
  pageWidth: number,
  marginTop: number
): number => {
  const textLines = doc.splitTextToSize(text, pageWidth - marginLeft - 20);
  textLines.forEach((line: string) => {
    if (yPosition + 10 > maxYPosition) {
      doc.addPage();
      yPosition = marginTop;
    }
    doc.text(line, marginLeft, yPosition);
    yPosition += 6;
  });
  return yPosition;
};
