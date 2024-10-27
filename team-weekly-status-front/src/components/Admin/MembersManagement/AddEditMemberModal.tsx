import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import { UserMember } from "../../../types/WeeklyStatus.types";
import { makeApiRequest } from "../../../services/apiHelper";

interface AddEditMemberModalProps {
  show: boolean;
  onHide: () => void;
  user: UserMember | null;
  onSave: () => void;
}

const AddEditMemberModal: React.FC<AddEditMemberModalProps> = ({
  show,
  onHide,
  user,
  onSave,
}) => {
  const [formData, setFormData] = useState<UserMember>({
    id: 0,
    name: "",
    email: "",
    isAdmin: false,
  });

  useEffect(() => {
    if (user) {
      setFormData(user);
    } else {
      setFormData({
        id: 0,
        name: "",
        email: "",
        isAdmin: false,
      });
    }
  }, [user]);

  const handleSave = async () => {
    const endpoint = user ? "/Member/Update" : "/Member/Add";
    const method = user ? "PUT" : "POST";

    if (!user) {
      formData.id = 0;
    }

    const response = await makeApiRequest<UserMember | { success: boolean }>(
      endpoint,
      method,
      formData
    );
    onSave();
    onHide();
  };

  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>{user ? "Edit Chango" : "Add New Chango"}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group controlId="formName">
            <Form.Label>First Name</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter the Chango's full name"
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
            />
          </Form.Group>
          <Form.Group controlId="formEmail">
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              placeholder="Enter email"
              value={formData.email}
              onChange={(e) =>
                setFormData({ ...formData, email: e.target.value })
              }
            />
          </Form.Group>
          <Form.Group controlId="formIsAdmin">
            <Form.Check
              type="checkbox"
              label="Administrator"
              checked={formData.isAdmin}
              onChange={(e) =>
                setFormData({ ...formData, isAdmin: e.target.checked })
              }
            />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Close
        </Button>
        <Button variant="primary" onClick={handleSave}>
          {user ? "Update" : "Add"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default AddEditMemberModal;
