import React, { useEffect, useMemo, useState } from "react";
import { userStore } from "../../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../../services/apiHelper";
import { UserMember } from "../../../types/WeeklyStatus.types";
import { Alert, Button, Form, Table } from "react-bootstrap";
import AddEditMemberModal from "./AddEditMemberModal";

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

    console.log(response);

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

  return (
    <div>
      <h2>Changos Management</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <div className="search-container">
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
            <th>Chango ID</th>
            <th>Full Name</th>
            <th>Email</th>
            <th>Administrator</th>
          </tr>
        </thead>
        <tbody>
          {filteredUsersMembersData.map((userMember: UserMember) => (
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
                >
                  Edit
                </Button>{" "}
                <Button
                  variant="danger"
                  onClick={() => handleDelete(userMember.id)}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Button variant="primary" onClick={handleAddNew}>
        New
      </Button>

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
