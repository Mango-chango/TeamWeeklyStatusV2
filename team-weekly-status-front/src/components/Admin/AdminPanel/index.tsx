import React, { useEffect } from "react";
import { Button, Alert } from "react-bootstrap";
import { userStore } from "../../../store";
import { useNavigate } from "react-router-dom";


const AdminPanel: React.FC = () => {
  const { isAdmin } = userStore();
  const navigate = useNavigate();

  useEffect(() => {
    if (!isAdmin) {
      navigate("/"); // Redirect non-admins to the homepage
    }
  }, [isAdmin, navigate]);

  const handleMembersMgmt = () => {
    navigate("/members-mgmt");
  };

  const handleTeamsMgmt = () => {
    navigate("/teams-mgmt");
  };



  return isAdmin ? (
    <div className="admin-panel">
      <h2>Admin Panel</h2>
      <p>Welcome, admin!</p>
      <Button
        variant="primary"
        onClick={handleMembersMgmt}
        className="form__btn"
      >
        Members Management
      </Button>
      <Button
        variant="primary"
        onClick={handleTeamsMgmt}
        className="form__btn"
      >
        Teams Management
      </Button>

    </div>
  ) : (
    <Alert variant="danger">You do not have access to this page</Alert>
  );
};

export default AdminPanel;
