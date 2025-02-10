import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import { TeamRead } from "../../../types/WeeklyStatus.types";
import { makeApiRequest } from "../../../services/apiHelper";

interface AddEditTeamModalProps {
  show: boolean;
  onHide: () => void;
  team: TeamRead | null;
  onSave: () => void;
}

const AddEditTeamModal: React.FC<AddEditTeamModalProps> = ({
  show,
  onHide,
  team,
  onSave,
}) => {
  const [formData, setFormData] = useState<TeamRead>({
    id: 0,
    name: "",
    description: "",
    emailNotificationsEnabled: false,
    slackNotificationsEnabled: false,
    weekReporterAutomaticAssignment: false,
    isActive: false,
    aiConfiguration: {
      aiEngineName: "",
      apiUrl: "",
      apiKey: "",
      model: "",
    },
  });

  useEffect(() => {
    if (team) {
      setFormData({
        ...team,
        aiConfiguration: team.aiConfiguration || {
          aiEngineName: "",
          apiUrl: "",
          apiKey: "",
          model: "",
        },
      });
    } else {
      setFormData({
        id: 0,
        name: "",
        description: "",
        emailNotificationsEnabled: false,
        slackNotificationsEnabled: false,
        weekReporterAutomaticAssignment: false,
        isActive: false,
        aiConfiguration: {
          aiEngineName: "",
          apiUrl: "",
          apiKey: "",
          model: "",
        },
      });
    }
  }, [team]);

  const handleSave = async () => {
    const endpoint = team ? "/Team/Update" : "/Team/Add";
    const method = team ? "PUT" : "POST";

    if (!team) {
      formData.id = 0;
    }

    const response = await makeApiRequest<TeamRead | { success: boolean }>(
      endpoint,
      method,
      formData
    );
    onSave();
    onHide();
  };

  // AI Engine Options
  const aiEngineOptions = ["OpenAI", "Gemini"];
  //console.log("formData", formData);

  return (
    <Modal show={show} onHide={onHide} size="lg">
      <Modal.Header closeButton>
        <Modal.Title>{team ? "Edit Team" : "Add New Team"}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          {/* Team Information */}
          <Form.Group controlId="formName">
            <Form.Label>Team Name</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter the team's name"
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
            />
          </Form.Group>
          <Form.Group controlId="description">
            <Form.Label>Description</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter a description for the team (optional)"
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
            />
          </Form.Group>
          <Form.Group controlId="emailNotificationsEnabled">
            <Form.Check
              type="checkbox"
              label="Email Notifications Enabled"
              checked={formData.emailNotificationsEnabled}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  emailNotificationsEnabled: e.target.checked,
                })
              }
            />
          </Form.Group>
          <Form.Group controlId="weekReporterAutomaticAssignment">
            <Form.Check
              type="checkbox"
              label="Week Reporter Automatic Assignment"
              checked={formData.weekReporterAutomaticAssignment}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  weekReporterAutomaticAssignment: e.target.checked,
                })
              }
            />
          </Form.Group>
          <Form.Group controlId="isActive">
            <Form.Check
              type="checkbox"
              label="Active"
              checked={formData.isActive}
              onChange={(e) =>
                setFormData({ ...formData, isActive: e.target.checked })
              }
            />
          </Form.Group>

          {/* AI Configuration */}
          <hr />
          <h5>AI Configuration</h5>
          <Form.Group controlId="aiEngineName">
            <Form.Label>AI Engine Name</Form.Label>
            <Form.Control
              as="select"
              value={formData.aiConfiguration?.aiEngine?.aiEngineName || ""}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  aiConfiguration: {
                    ...formData.aiConfiguration,
                    aiEngineName: e.target.value,
                  },
                })
              }
            >
              <option value="">Select an AI Engine</option>
              {aiEngineOptions.map((option) => (
                <option key={option} value={option}>
                  {option}
                </option>
              ))}
            </Form.Control>
          </Form.Group>
          <Form.Group controlId="apiUrl">
            <Form.Label>API URL</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter the API URL"
              value={formData.aiConfiguration?.apiUrl || ""}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  aiConfiguration: {
                    ...formData.aiConfiguration,
                    apiUrl: e.target.value,
                  },
                })
              }
            />
          </Form.Group>
          <Form.Group controlId="apiKey">
            <Form.Label>API Key</Form.Label>
            <Form.Control
              type="password"
              placeholder="Enter the API Key"
              value={formData.aiConfiguration?.apiKey || ""}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  aiConfiguration: {
                    ...formData.aiConfiguration,
                    apiKey: e.target.value,
                  },
                })
              }
            />
          </Form.Group>
          <Form.Group controlId="model">
            <Form.Label>Model</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter the Model Name"
              value={formData.aiConfiguration?.model || ""}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  aiConfiguration: {
                    ...formData.aiConfiguration,
                    model: e.target.value,
                  },
                })
              }
            />
          </Form.Group>
          <Form.Group controlId="removeAIConfig">
            <Form.Check
              type="checkbox"
              label="Remove AI Configuration"
              checked={!formData.aiConfiguration}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  aiConfiguration: e.target.checked
                    ? undefined
                    : {
                        aiEngineName: "",
                        apiUrl: "",
                        apiKey: "",
                        model: "",
                      },
                })
              }
            />
          </Form.Group>
          {/* End AI Configuration */}
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
