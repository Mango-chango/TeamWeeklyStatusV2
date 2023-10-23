import React, { useEffect, useRef, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import {
  TeamMemberWeeklyStatusData,
  TeamWeeklyStatusData,
} from "../../types/WeeklyStatus.types";
import moment from "moment";
import Markdown from "react-markdown";
import "./reporting.css";
import { Alert } from 'react-bootstrap';


const StatusReporting: React.FC = () => {
  const { role, teamName, memberName, memberId } = userStore();
  const [localTeamName, setLocalTeamName] = useState(teamName);
  const [teamWeeklyStatusData, setTeamWeeklyStatusData] =
    useState<TeamWeeklyStatusData | null>(null);
  const [unreportedMembers, setUnreportedMembers] = useState<
    TeamMemberWeeklyStatusData[]
  >([]);
  const [showToast, setShowToast] = useState(false);
  const [copySuccess, setCopySuccess] = useState<string | null>(null);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate, setStartDate] = useState(initialStartDate);

  const endDate = moment().endOf("week").toDate();
  const markdown = `# ${teamName} Weekly Status Report
    ## ${startDate.toDateString()} - ${endDate.toDateString()}
    ### ${memberName}
    `;

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

  const generateMarkdown = () => {
    if (!teamWeeklyStatusData) return "";

    let markdownContent = `## ${localTeamName} Weekly Status Report`;
    markdownContent += `\n### ${startDate.toDateString()} - ${endDate.toDateString()}\n`;
    teamWeeklyStatusData
      .filter(({ weeklyStatus }) => weeklyStatus !== null)
      .forEach(({ memberName, weeklyStatus }) => {
        markdownContent += `\n### ${memberName}\n`;
        markdownContent += `\n#### What was done this week:\n`;
        markdownContent += `${weeklyStatus?.doneThisWeek
          ?.map((task: string) => `* ${task}`)
          .join("\n")}`;
        markdownContent += `\n#### Plan for next week:\n`;
        markdownContent += `${weeklyStatus?.planForNextWeek
          ?.map((task: string) => `* ${task}`)
          .join("\n")}`;
        markdownContent += `\n#### Blockers:\n`;
        markdownContent += `${weeklyStatus?.blockers}\n`;
        markdownContent += `\n#### Upcoming PTO:\n`;
        markdownContent += `${weeklyStatus?.upcomingPTO
          ?.map((date) => moment(date).format("MMM DD"))
          .join("\n")}`;
      });

    return markdownContent;
  };

  const handleCopyToClipboard = async () => {
    const content = generateMarkdown();
    try {
      await navigator.clipboard.writeText(content);
      // Indicate to the user that the content has been copied
      setCopySuccess("Copied to clipboard!");

      // Optionally, you can set a timer to hide the success message after a few seconds
      setTimeout(() => {
        setCopySuccess(null);
      }, 2000); // 2 seconds delay
    } catch (error) {
      console.error("Failed to copy text: ", error);
    }
  };

//  const markdownRef = useRef(null);

//   const handleCopyToClipboardV2 = () => {
//     const range = document.createRange();
//     const selection = window.getSelection();
//     if (!markdownRef.current) return;

//     if (!selection) return;
//     range.selectNodeContents(markdownRef.current);
//     selection.removeAllRanges();
//     selection.addRange(range);

//     try {
//         document.execCommand('copy');

//         selection.removeAllRanges();

//         setCopySuccess("Copied to clipboard!");

//         setTimeout(() => {
//             setCopySuccess(null);
//         }, 2000);

//     } catch (error) {
//         console.error("Failed to copy text: ", error);
//     }
// };


  return (
    <>
      <div className="card mt-5 centered-div">
        <div className="card-body card-content">
          <Markdown>{generateMarkdown()}</Markdown>
        </div>
      </div>
      {copySuccess && <Alert variant="success">{copySuccess}</Alert>}

      <div className="d-flex flex-column mt-2 centered-button">
        <button
          onClick={handleCopyToClipboard}
          className="btn btn-primary mt-3"
        >
          Copy MD to Clipboard
        </button>
      </div>
      {/* <div className="d-flex flex-column mt-2 centered-button">
        <button
          onClick={handleCopyToClipboardV2}
          className="btn btn-primary mt-3"
        >
          Copy to Clipboard V2
        </button>
      </div> */}

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
