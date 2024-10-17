import React, { useEffect, useMemo, useState } from "react";
import { userStore } from "../../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../../services/apiHelper";
import { UserMember } from "../../../types/WeeklyStatus.types";
import { Alert, Button, Form, Table, Pagination } from "react-bootstrap";
import AddEditMemberModal from "./AddEditMemberModal";
import { PaginationControls } from "../../Common";

const MembersManagement: React.FC = () => {
  const { isAdmin } = userStore();
  const navigate = useNavigate();

  //const [usersMembers, setUsersMembers] = useState<UserMember[]>([]);
  const [showModal, setShowModal] = useState(false);
  const [selectedUserMember, setSelectedUserMember] =
    useState<UserMember | null>(null);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    if (!isAdmin) {
      navigate("/"); // Redirect non-admins to the homepage
    }
  }, [isAdmin, navigate]);

  type UserMemberData = UserMember[];
  const [usersMembersData, setUsersMembersData] =
    useState<UserMemberData | null>(null);

  const fetchUserMembers = async () => {
    const response: UserMemberData = await makeApiRequest(
      "/Member/GetAll",
      "GET"
    );

    // console.log(response);

    if (response) {
      setUsersMembersData(response);
    }
  };

  useEffect(() => {
    fetchUserMembers();
  }, []);

  const handleEdit = (user: UserMember) => {
    setSelectedUserMember(user);
    setShowModal(true);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this user?")) {
      return;
    }

    try {
      await makeApiRequest(`/Member/Delete/${id}`, "DELETE");
      fetchUserMembers();
    } catch (error) {
      console.log(error);
    }
  };

  const handleAddNew = () => {
    setSelectedUserMember(null);
    setShowModal(true);
  };

  const [nameSearch, setNameSearch] = useState("");
  const [emailSearch, setEmailSearch] = useState("");

  const filteredUsersMembersData = useMemo(() => {
    if (!usersMembersData) return [];

    return usersMembersData.filter((user) => {
      const matchesName = user.name
        .toLowerCase()
        .includes(nameSearch.toLowerCase());
      const matchesEmail = user.email
        .toLowerCase()
        .includes(emailSearch.toLowerCase());
      return matchesName && matchesEmail;
    });
  }, [nameSearch, emailSearch, usersMembersData]);

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

  const sortedFilteredUsersMembersData = useMemo(() => {
    let sortableData = [...filteredUsersMembersData];

    if (sortConfig !== null) {
      sortableData.sort((a, b) => {
        const aValue = a[sortConfig.key as keyof UserMember];
        const bValue = b[sortConfig.key as keyof UserMember];

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
  }, [filteredUsersMembersData, sortConfig]);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  const totalPages = Math.ceil(
    sortedFilteredUsersMembersData.length / itemsPerPage
  );

  const currentPageData = useMemo(() => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return sortedFilteredUsersMembersData.slice(startIndex, endIndex);
  }, [sortedFilteredUsersMembersData, currentPage, itemsPerPage]);

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(pageNumber);
  };

  useEffect(() => {
    setCurrentPage(1);
  }, [nameSearch, emailSearch, sortConfig]);

  return (
    <div className="container-main-left-aligned">
      <h2>Changos Management</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <div className="limited-width-form">
        <input
          type="text"
          placeholder="Search by Name"
          value={nameSearch}
          onChange={(e) => setNameSearch(e.target.value)}
        />
        <input
          type="text"
          placeholder="Search by Email"
          value={emailSearch}
          onChange={(e) => setEmailSearch(e.target.value)}
        />
      </div>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th onClick={() => requestSort("id")} style={{ cursor: "pointer" }}>
              Chango ID {renderSortIcon("id")}
            </th>
            <th
              onClick={() => requestSort("name")}
              style={{ cursor: "pointer" }}
            >
              Full Name {renderSortIcon("name")}
            </th>
            <th
              onClick={() => requestSort("email")}
              style={{ cursor: "pointer" }}
            >
              Email {renderSortIcon("email")}
            </th>
            <th
              onClick={() => requestSort("isAdmin")}
              style={{ cursor: "pointer", textAlign: "center" }}
            >
              Administrator {renderSortIcon("isAdmin")}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentPageData.map((userMember: UserMember) => (
            <tr key={userMember.id}>
              <td>{userMember.id}</td>
              <td>{userMember.name}</td>
              <td>
                <a
                  href={`mailto:${encodeURIComponent(
                    `"${userMember.name}" <${userMember.email}>`
                  )}`}
                  className="text-primary"
                >
                  {userMember.email}
                </a>
              </td>
              <td style={{ textAlign: "center" }}>
                <Form.Check
                  type="switch"
                  checked={userMember.isAdmin}
                  readOnly
                  onClick={(e) => e.preventDefault()}
                  style={{ pointerEvents: "none" }}
                />
              </td>

              <td>
                <Button
                  variant="warning"
                  onClick={() => handleEdit(userMember)}
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
                  onClick={() => handleDelete(userMember.id)}
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
                  </svg>
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

      <div className="new-button-container">
        <Button
          variant="primary"
          onClick={handleAddNew}
          className="btn-primary"
        >
          New
        </Button>
      </div>

      <AddEditMemberModal
        show={showModal}
        onHide={() => {
          setShowModal(false);
          fetchUserMembers();
        }}
        user={selectedUserMember}
        onSave={fetchUserMembers}
      />
    </div>
  );
};

export default MembersManagement;
