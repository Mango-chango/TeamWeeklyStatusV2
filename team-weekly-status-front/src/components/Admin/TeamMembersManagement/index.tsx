import React, { useEffect, useMemo, useState } from "react";
import { makeApiRequest } from "../../../services/apiHelper";
import { TeamMember } from "../../../types/WeeklyStatus.types";
import { Alert, Button, Table, Form, Row, Col } from "react-bootstrap";
import AddEditTeamMemberModal from "./AddEditTeamMemberModal";
import { PaginationControls } from "../../Common";
import { formatDate } from "../../../utils/dateUtils";
import "../styles.css";

interface TeamMembersManagementProps {
  teamId: number;
  teamName: string;
}

const TeamMembersManagement: React.FC<TeamMembersManagementProps> = ({
  teamId,
  teamName,
}) => {
  const [error, setError] = useState<string>("");

  const [teamMembersData, setTeamMembersData] = useState<TeamMember[] | null>(
    null
  );

  const fetchTeamMembers = async () => {
    try {
      const endpoint = `/TeamMember/GetAll`;
      const method = "POST";
      const body = { teamId };
      const response: TeamMember[] = await makeApiRequest(
        endpoint,
        method,
        body
      );
      if (response) {
        setTeamMembersData(response);
      }
    } catch (error) {
      console.error(error);
      setError("An error occurred while fetching team members.");
    }
  };

  useEffect(() => {
    fetchTeamMembers();
  }, [teamId]);

  const [isMobile, setIsMobile] = useState(false);
  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768); // Adjust breakpoint as needed
    };

    window.addEventListener("resize", handleResize);
    handleResize(); // Initial check

    return () => window.removeEventListener("resize", handleResize);
  }, []);

  // Sorting and pagination state
  const [sortConfig, setSortConfig] = useState<{
    key: string;
    direction: "ascending" | "descending";
  } | null>(null);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  const requestSort = (key: string) => {
    let direction: "ascending" | "descending" = "ascending";
    if (
      sortConfig &&
      sortConfig.key === key &&
      sortConfig.direction === "ascending"
    ) {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const renderSortIcon = (columnKey: string) => {
    if (!sortConfig || sortConfig.key !== columnKey) {
      return null;
    }
    return sortConfig.direction === "ascending" ? " ▲" : " ▼";
  };

  const sortedTeamMembersData = useMemo(() => {
    if (!teamMembersData) return [];

    let sortableData = [...teamMembersData];

    if (sortConfig !== null) {
      sortableData.sort((a, b) => {
        const aValue = a[sortConfig.key as keyof TeamMember];
        const bValue = b[sortConfig.key as keyof TeamMember];

        if (typeof aValue === "string" && typeof bValue === "string") {
          return sortConfig.direction === "ascending"
            ? aValue.localeCompare(bValue)
            : bValue.localeCompare(aValue);
        } else {
          return sortConfig.direction === "ascending"
            ? Number(aValue) - Number(bValue)
            : Number(bValue) - Number(aValue);
        }
      });
    }

    return sortableData;
  }, [teamMembersData, sortConfig]);

  const totalPages = Math.ceil(sortedTeamMembersData.length / itemsPerPage);

  const currentPageData = useMemo(() => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return sortedTeamMembersData.slice(startIndex, endIndex);
  }, [sortedTeamMembersData, currentPage, itemsPerPage]);

  useEffect(() => {
    setCurrentPage(1);
  }, [teamMembersData, sortConfig]);

  // State for controlling the TeamMember modal
  const [showMemberModal, setShowMemberModal] = useState(false);
  const [selectedTeamMember, setSelectedTeamMember] =
    useState<TeamMember | null>(null);

  const handleAddTeamMember = () => {
    setSelectedTeamMember(null);
    setShowMemberModal(true);
  };

  const handleEditTeamMember = (teamMember: TeamMember) => {
    setSelectedTeamMember(teamMember);
    setShowMemberModal(true);
  };

  const handleDeleteTeamMember = async (memberId: number) => {
    if (
      !window.confirm(
        "Are you sure you want to remove this member from the team?"
      )
    ) {
      return;
    }

    const endpoint = `/TeamMember/Delete?teamId=${teamId}&memberId=${memberId}`;
    const method = "DELETE";

    try {
      await makeApiRequest(endpoint, method);
      fetchTeamMembers();
    } catch (error) {
      console.error(error);
      setError("An error occurred while deleting the team member.");
    }
  };

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(pageNumber);
  };

  return (
    <div>
      <h3>Members of {teamName}</h3>
      {error && <Alert variant="danger">{error}</Alert>}

      {/* Conditionally render based on isMobile */}
      {isMobile ? (
        /* Render cards */
        <div className="card-container">
          {currentPageData.map((teamMember: TeamMember) => (
            <div key={teamMember.memberId} className="member-card mb-3">
              <div className="card-body">
                <h5 className="card-title">{teamMember.memberName}</h5>
                <p className="card-text">
                  <strong>Team Lead:</strong>{" "}
                  {teamMember.isTeamLead ? "Yes" : "No"}
                </p>
                <p className="card-text">
                  <strong>Current Week Reporter:</strong>{" "}
                  {teamMember.isCurrentWeekReporter ? "Yes" : "No"}
                </p>
                <p className="card-text">
                  <strong>Start Active Date:</strong>{" "}
                  {formatDate(teamMember.startActiveDate)}
                </p>
                <p className="card-text">
                  <strong>End Active Date:</strong>{" "}
                  {formatDate(teamMember.endActiveDate)}
                </p>
                <div className="d-flex flex-wrap">
                  <Button
                    variant="warning"
                    onClick={() => handleEditTeamMember(teamMember)}
                    className="btn-icon btn-sm mr-2 mb-2"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-pencil"
                      viewBox="0 0 16 16"
                    >
                      <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325" />
                    </svg>
                  </Button>
                  <Button
                    variant="danger"
                    onClick={() => handleDeleteTeamMember(teamMember.memberId)}
                    className="btn-icon btn-sm mr-2 mb-2"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-person-dash"
                      viewBox="0 0 16 16"
                    >
                      <path d="M12.5 16a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7M11 12h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1 0-1m0-7a3 3 0 1 1-6 0 3 3 0 0 1 6 0M8 7a2 2 0 1 0 0-4 2 2 0 0 0 0 4" />
                      <path d="M8.256 14a4.5 4.5 0 0 1-.229-1.004H3c.001-.246.154-.986.832-1.664C4.484 10.68 5.711 10 8 10q.39 0 .74.025c.226-.341.496-.65.804-.918Q8.844 9.002 8 9c-5 0-6 3-6 4s1 1 1 1z" />
                    </svg>
                  </Button>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th
                onClick={() => requestSort("memberName")}
                style={{ cursor: "pointer" }}
              >
                Member Name {renderSortIcon("memberName")}
              </th>
              <th
                onClick={() => requestSort("isTeamLead")}
                style={{ cursor: "pointer", textAlign: "center" }}
              >
                Team Lead {renderSortIcon("isTeamLead")}
              </th>
              <th
                onClick={() => requestSort("isCurrentWeekReporter")}
                style={{ cursor: "pointer", textAlign: "center" }}
              >
                Current Week Reporter {renderSortIcon("isCurrentWeekReporter")}
              </th>
              <th
                onClick={() => requestSort("startActiveDate")}
                style={{ cursor: "pointer" }}
              >
                Start Active Date {renderSortIcon("startActiveDate")}
              </th>
              <th
                onClick={() => requestSort("endActiveDate")}
                style={{ cursor: "pointer" }}
              >
                End Active Date {renderSortIcon("endActiveDate")}
              </th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {currentPageData.map((teamMember: TeamMember) => (
              <tr key={teamMember.memberId}>
                <td>{teamMember.memberName}</td>
                <td style={{ textAlign: "center" }}>
                  <input
                    type="checkbox"
                    checked={teamMember.isTeamLead}
                    onChange={(e) => e.preventDefault()}
                    aria-readonly="true"
                    className="readonly-checkbox"
                  />
                </td>
                <td style={{ textAlign: "center" }}>
                  <input
                    type="checkbox"
                    checked={teamMember.isCurrentWeekReporter}
                    onChange={(e) => e.preventDefault()}
                    aria-readonly="true"
                    className="readonly-checkbox"
                  />
                </td>
                <td>{formatDate(teamMember.startActiveDate)}</td>
                <td>{formatDate(teamMember.endActiveDate)}</td>
                <td>
                  <Button
                    variant="primary"
                    onClick={() => handleEditTeamMember(teamMember)}
                    className="btn-icon"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-pencil"
                      viewBox="0 0 16 16"
                    >
                      <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325" />
                    </svg>
                  </Button>{" "}
                  <Button
                    variant="danger"
                    onClick={() => handleDeleteTeamMember(teamMember.memberId)}
                    className="btn-icon"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-person-dash"
                      viewBox="0 0 16 16"
                    >
                      <path d="M12.5 16a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7M11 12h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1 0-1m0-7a3 3 0 1 1-6 0 3 3 0 0 1 6 0M8 7a2 2 0 1 0 0-4 2 2 0 0 0 0 4" />
                      <path d="M8.256 14a4.5 4.5 0 0 1-.229-1.004H3c.001-.246.154-.986.832-1.664C4.484 10.68 5.711 10 8 10q.39 0 .74.025c.226-.341.496-.65.804-.918Q8.844 9.002 8 9c-5 0-6 3-6 4s1 1 1 1z" />
                    </svg>{" "}
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}

      {totalPages > 1 && (
        <PaginationControls
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={handlePageChange}
        />
      )}

      <Button variant="primary" onClick={handleAddTeamMember}>
        Add Member
      </Button>

      <AddEditTeamMemberModal
        show={showMemberModal}
        onHide={() => setShowMemberModal(false)}
        teamId={teamId}
        teamMember={selectedTeamMember}
        onSave={() => {
          setShowMemberModal(false);
          fetchTeamMembers();
        }}
        existingMemberIds={
          teamMembersData ? teamMembersData.map((tm) => tm.memberId) : []
        }
      />
    </div>
  );
};

export default TeamMembersManagement;
