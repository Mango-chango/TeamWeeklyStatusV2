import React, { useEffect } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import { userStore } from "../../../store";
import { useNavigate } from "react-router-dom";
import MembersManagement from '../MembersManagement/index';

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

      {/* Add your admin functionality here */}
    </div>
  ) : (
    <Alert variant="danger">You do not have access to this page</Alert>
  );
};

export default AdminPanel;
