import React, { useState, useEffect } from "react";
import { ListGroup, Button, Alert, Form } from "react-bootstrap";
import { makeApiRequest } from "../../services/apiHelper";
import { Member } from "../../types/WeeklyStatus.types";
import { useNavigate } from "react-router-dom";
import './styles.css';

const AssignReporter: React.FC = () => {
  const [members, setMembers] = useState<Member[]>([]);
  const [selectedMemberId, setSelectedMemberId] = useState<number | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    // Fetch the members but exclude the current reporter
    const fetchMembers = async () => {
      const response: Member[] = await makeApiRequest(
        "/TeamMember/WithoutCurrentReporter",
        "GET"
      );
      setMembers(response);
    };

    fetchMembers();
  }, []);

  const handleSave = async () => {
    try {
      if (selectedMemberId) {
        await makeApiRequest("/TeamMember/AssignReporter", "POST", {
          memberId: selectedMemberId,
        });
      }
      displaySuccessMessage();
    } catch (err) {
      setSuccess(false);
      setError("An error occurred. Please try again.");
    }
  };

  const displaySuccessMessage = () => {
    setSuccess(true);

    setTimeout(() => {
      setSuccess(false);
    }, 5000);
  };

  const handleBack = async () => {
    navigate("/weekly-status");
  };

  return (
    <div className="d-flex flex-column align-items-center mt-5">
      <h3>Assign Weekly Status Reporter</h3>
      {success && (
        <Alert variant="success" className="mt-3">
          Reporter for next week has been assigned!
        </Alert>
      )}
      {error && (
        <Alert variant="danger" className="mt-3">
          {error}
        </Alert>
      )}
      <ListGroup>
        {members.map((member) => (
          <ListGroup.Item
            key={member.id}
            active={selectedMemberId === member.id}
            onClick={() => setSelectedMemberId(member.id)}
          >
            {member.name}
          </ListGroup.Item>
        ))}
      </ListGroup>
      <Form.Group
        controlId="buttons"
        className="form__buttons"
      >
        <Button
          onClick={handleSave}
          disabled={!selectedMemberId}
          className="mt-3"
        >
          Save
        </Button>
        <Button variant="secondary" onClick={handleBack} className="mt-3 ml-2">
          Back
        </Button>
      </Form.Group>
    </div>
  );
};

export default AssignReporter;
