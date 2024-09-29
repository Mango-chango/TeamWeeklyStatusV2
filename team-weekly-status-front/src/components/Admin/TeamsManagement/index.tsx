import React, { useEffect, useMemo, useState } from "react";
import { userStore } from "../../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../../services/apiHelper";
import { Team } from "../../../types/WeeklyStatus.types";
import { Alert, Button, Form, Table, Pagination } from "react-bootstrap";
import AddEditTeamModal from "./AddEditTeamModal";
import { PaginationControls } from "../../Common";

const TeamsManagement: React.FC = () => {
  const { isAdmin } = userStore();
  const navigate = useNavigate();

  const [showModal, setShowModal] = useState(false);
  const [selectedTeam, setSelectedTeam] =
    useState<Team | null>(null);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    if (!isAdmin) {
      navigate("/"); // Redirect non-admins to the homepage
    }
  }, [isAdmin, navigate]);

  type TeamData = Team[];
  const [teamsData, setTeamsData] =
    useState<TeamData | null>(null);

  const fetchTeams = async () => {
    const response: TeamData = await makeApiRequest(
      "/Team/GetAll",
      "GET"
    );

    console.log(response);

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
      console.log(error);
    }
  };

  const handleAddNew = () => {
    setSelectedTeam(null);
    setShowModal(true);
  };

  const [nameSearch, setNameSearch] = useState("");

  const filteredTeamsData = useMemo(() => {
    if (!teamsData) return [];

    return teamsData.filter((user) => {
      const matchesName = user.name
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
            : ((bValue as number | undefined) ?? 0) -
                ((aValue as number | undefined) ?? 0);
        }
      });
    }

    return sortableData;
  }, [filteredTeamsData, sortConfig]);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  const totalPages = Math.ceil(
    sortedFilteredTeamsData.length / itemsPerPage
  );

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
            <th
              onClick={() => requestSort("name")}
              style={{ cursor: "pointer" }}
            >
              Name {renderSortIcon("name")}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentPageData.map((team: Team) => (
            <tr key={team.id}>
              <td>{team.id}</td>
              <td>{team.name}</td>
              <td>
                <Button
                  variant="warning"
                  onClick={() => handleEdit(team)}
                >
                  Edit
                </Button>{" "}
                <Button
                  variant="danger"
                  onClick={() => handleDelete(team.id)}
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
    </div>
  );
};

export default TeamsManagement;
