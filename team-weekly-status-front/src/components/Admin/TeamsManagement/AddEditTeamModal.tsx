import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import { Team } from "../../../types/WeeklyStatus.types";
import { makeApiRequest } from "../../../services/apiHelper";

interface AddEditTeamModalProps {
  show: boolean;
  onHide: () => void;
  team: Team | null;
  onSave: () => void;
}

const AddEditTeamModal: React.FC<AddEditTeamModalProps> = ({
  show,
  onHide,
  team,
  onSave,
}) => {
  const [formData, setFormData] = useState<Team>({
    id: 0,
    name: "",
  });

  useEffect(() => {
    if (team) {
      setFormData(team);
    } else {
      setFormData({
        id: 0,
        name: "",
      });
    }
  }, [team]);

  const handleSave = async () => {
    const endpoint = team ? "/Team/Update" : "/Team/Add";
    const method = team ? "PUT" : "POST";

    if (!team) {
      formData.id = 0;
    }

    const response = await makeApiRequest<Team | { success: boolean }>(
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
        <Modal.Title>{team ? "Edit Team" : "Add New Team"}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group controlId="formName">
            <Form.Label>First Name</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter the team's name"
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
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
          {team ? "Update" : "Add"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default AddEditTeamModal;
