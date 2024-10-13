import React, { useEffect, useMemo, useState } from "react";
import { userStore } from "../../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../../services/apiHelper";
import { Team } from "../../../types/WeeklyStatus.types";
import { Alert, Button, Table } from "react-bootstrap";
import AddEditTeamModal from "./AddEditTeamModal";
import TeamMembersManagement from "../TeamMembersManagement";
import { PaginationControls } from "../../Common";

const TeamsManagement: React.FC = () => {
  const { isAdmin } = userStore();
  const navigate = useNavigate();

  const [showModal, setShowModal] = useState(false);
  const [selectedTeam, setSelectedTeam] = useState<Team | null>(null);
  const [error, setError] = useState<string>("");

  const [teamsData, setTeamsData] = useState<Team[] | null>(null);

  const fetchTeams = async () => {
    const response: Team[] = await makeApiRequest("/Team/GetAll", "GET");

    if (response) {
      setTeamsData(response);
    }
  };

  useEffect(() => {
    fetchTeams();
  }, []);

  const handleEdit = (team: Team) => {
    setSelectedTeam(team);
    setShowModal(true);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this team?")) {
      return;
    }

    try {
      await makeApiRequest(`/Team/Delete/${id}`, "DELETE");
      fetchTeams();
    } catch (error) {
      // console.log(error);
    }
  };

  const handleAddNew = () => {
    setSelectedTeam(null);
    setShowModal(true);
  };

  const [nameSearch, setNameSearch] = useState("");

  const filteredTeamsData = useMemo(() => {
    if (!teamsData) return [];

    return teamsData.filter((team) => {
      const matchesName = team.name
        .toLowerCase()
        .includes(nameSearch.toLowerCase());
      return matchesName;
    });
  }, [nameSearch, teamsData]);

  const [sortConfig, setSortConfig] = useState<{
    key: string;
    direction: "ascending" | "descending";
  } | null>(null);

  const requestSort = (key: string) => {
    let direction: "ascending" | "descending" = "ascending";
    if (sortConfig && sortConfig.key === key && sortConfig.direction === "ascending") {
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

  const sortedFilteredTeamsData = useMemo(() => {
    let sortableData = [...filteredTeamsData];

    if (sortConfig !== null) {
      sortableData.sort((a, b) => {
        const aValue = a[sortConfig.key as keyof Team];
        const bValue = b[sortConfig.key as keyof Team];

        if (typeof aValue === "string" && typeof bValue === "string") {
          return sortConfig.direction === "ascending"
            ? aValue.localeCompare(bValue)
            : bValue.localeCompare(aValue);
        } else {
          return sortConfig.direction === "ascending"
            ? (aValue as number) - (bValue as number)
            : (bValue as number) - (aValue as number);
        }
      });
    }

    return sortableData;
  }, [filteredTeamsData, sortConfig]);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  const totalPages = Math.ceil(sortedFilteredTeamsData.length / itemsPerPage);

  const currentPageData = useMemo(() => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return sortedFilteredTeamsData.slice(startIndex, endIndex);
  }, [sortedFilteredTeamsData, currentPage, itemsPerPage]);

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(pageNumber);
  };

  useEffect(() => {
    setCurrentPage(1);
  }, [nameSearch, sortConfig]);

  // State for selected team
  const [selectedTeamId, setSelectedTeamId] = useState<number | null>(null);
  const [selectedTeamName, setSelectedTeamName] = useState<string>("");

  return (
    <div>
      <h2>Teams Management</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <div className="search-container">
        <input
          type="text"
          placeholder="Search by Name"
          value={nameSearch}
          onChange={(e) => setNameSearch(e.target.value)}
        />
      </div>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th onClick={() => requestSort("id")} style={{ cursor: "pointer" }}>
              Team ID {renderSortIcon("id")}
            </th>
            <th onClick={() => requestSort("name")} style={{ cursor: "pointer" }}>
              Name {renderSortIcon("name")}
            </th>
            <th onClick={() => requestSort("description")} style={{ cursor: "pointer" }}>
              Description {renderSortIcon("description")}
            </th>
            <th>Email Notifications</th>
            <th>Slack Notifications</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentPageData.map((team: Team) => (
            <tr
              key={team.id}
              onClick={() => {
                setSelectedTeamId(team.id);
                setSelectedTeamName(team.name);
              }}
              style={{
                cursor: "pointer",
                backgroundColor:
                  selectedTeamId === team.id ? "#e0f7fa" : "transparent",
              }}
            >
              <td>{team.id}</td>
              <td>{team.name}</td>
              <td>{team.description? team.description : ""}</td>
              <td style={{ textAlign: "center" }}>
                <input
                  type="checkbox"
                  checked={team.emailNotificationsEnabled}
                  onChange={(e) => e.preventDefault()}
                  aria-readonly="true"
                  className="readonly-checkbox"
                />
              </td>
              <td style={{ textAlign: "center" }}>
                <input
                  type="checkbox"
                  checked={team.slackNotificationsEnabled}
                  onChange={(e) => e.preventDefault()}
                  aria-readonly="true"
                  className="readonly-checkbox"
                />
              </td>
              <td>
                <Button
                  variant="warning"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleEdit(team);
                  }}
                >
                  Edit
                </Button>{" "}
                <Button
                  variant="danger"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleDelete(team.id);
                  }}
                >
                  Delete
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

      <Button variant="primary" onClick={handleAddNew}>
        New
      </Button>

      <AddEditTeamModal
        show={showModal}
        onHide={() => {
          setShowModal(false);
          fetchTeams();
        }}
        team={selectedTeam}
        onSave={fetchTeams}
      />

      {selectedTeamId && (
        <TeamMembersManagement teamId={selectedTeamId} teamName={selectedTeamName} />
      )}
    </div>
  );
};

export default TeamsManagement;
