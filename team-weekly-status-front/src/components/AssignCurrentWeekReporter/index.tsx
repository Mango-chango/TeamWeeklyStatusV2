import React, { useState, useEffect } from "react";
import { ListGroup, Button, Alert, Form } from "react-bootstrap";
import { makeApiRequest } from "../../services/apiHelper";
import { Reporter } from "../../types/WeeklyStatus.types";
import { useNavigate } from "react-router-dom";
import "./styles.css";
import userStore from "../../store/userStore";

const AssignCurrentWeekReporter: React.FC = () => {
  const { teamId, memberId } = userStore();

  const [members, setMembers] = useState<Member[]>([]);
  const [selectedMemberId, setSelectedMemberId] = useState<number | null>(
    memberId
  );
  const [success, setSuccess] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    // Fetch the members but exclude the current reporter
    const fetchMembers = async () => {
      const body = { teamId };
      const response: Reporter[] = await makeApiRequest(
        "/TeamMember/GetAll",
        "POST",
        body
      );
      setMembers(response);
    };

    fetchMembers();
  }, []);

  const handleSave = async () => {
    try {
      if (selectedMemberId) {
        await makeApiRequest("/TeamMember/AssignCurrentWeekReporter", "POST", {
          teamId,
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
    <div className="d-flex flex-column align-items-center mt-1 assign-current-week-reporter__form__container">
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
      <div>
        <label>
          {" "}
          Select a team member to assign as the reporter for this week:
        </label>
      </div>
      <ListGroup className="assign-current-week-reporter__list__group">
        {members.map((member) => (
          <ListGroup.Item
            key={member.memberId}
            active={selectedMemberId === member.memberId}
            onClick={() => setSelectedMemberId(member.memberId)}
            className="assign-current-week-reporter__list__item"
            action
            as="button"
          >
            {member.memberName}
          </ListGroup.Item>
        ))}
      </ListGroup>

      <div className="assign-current-week-reporter__form__btngroup">
        <Button
          variant="primary"
          disabled={!selectedMemberId}
          onClick={handleSave}
          className="assign-current-week-reporter__form__btn"
        >
          Assign
        </Button>

        <Button variant="secondary" onClick={handleBack} className="assign-current-week-reporter__form__btn">
          Back
        </Button>
      </div>
    </div>
  );
};

export default AssignCurrentWeekReporter;
