import React, { useEffect, useMemo, useState } from "react";
import { makeApiRequest } from "../../../services/apiHelper";
import { TeamMember } from "../../../types/WeeklyStatus.types";
import { Alert, Button, Table } from "react-bootstrap";
import AddEditTeamMemberModal from "./AddEditTeamMemberModal";
import { PaginationControls } from "../../Common";
import { formatDate } from '../../../utils/dateUtils';
import "./styles.css"

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
        const response: TeamMember[] = await makeApiRequest(endpoint, method, body);
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
      !window.confirm("Are you sure you want to remove this member from the team?")
    ) {
      return;
    }

    const endpoint = `/TeamMember/Delete?teamId=${teamId}&memberId=${memberId}`
    const method = 'DELETE'

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
      <Table striped bordered hover>
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
                  variant="warning"
                  onClick={() => handleEditTeamMember(teamMember)}
                >
                  Edit
                </Button>{" "}
                <Button
                  variant="danger"
                  onClick={() => handleDeleteTeamMember(teamMember.memberId)}
                >
                  Remove
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

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
        existingMemberIds={teamMembersData ? teamMembersData.map((tm) => tm.memberId) : []}
      />
    </div>
  );
};

export default TeamMembersManagement;
