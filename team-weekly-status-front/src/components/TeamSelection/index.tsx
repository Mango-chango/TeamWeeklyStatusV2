import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Form, ListGroup, Alert } from "react-bootstrap";
import { userStore } from "../../store";

const TeamSelection: React.FC = () => {
  const memberActiveTeams = userStore((state) => state.memberActiveTeams);
  const [selectedTeamId, setSelectedTeamId] = useState<number>(0);
  const [showAlert, setShowAlert] = useState<boolean>(false);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!selectedTeamId) {
      // Set the selected team ID using zustand store
      userStore.getState().setTeamId(selectedTeamId);
      // Navigate to the weekly status page or the appropriate screen
      const navigate = useNavigate();
      navigate("/weekly-status");
    } else {
      setShowAlert(true);
    }
  };

  return (
    <div className="team-selection-container">
      <Form onSubmit={handleSubmit}>
        <h2>Select Your Team</h2>
        {showAlert && (
          <Alert
            variant="danger"
            onClose={() => setShowAlert(false)}
            dismissible
          >
            Please select a team.
          </Alert>
        )}
        <ListGroup>
          {memberActiveTeams &&
            memberActiveTeams.map((team) => (
              <ListGroup.Item
                key={team.id}
                action
                active={selectedTeamId === team.id}
                onClick={() => setSelectedTeamId(team.id)}
              >
                {team.name}
              </ListGroup.Item>
            ))}
        </ListGroup>
        <Button type="submit" className="mt-3">
          Continue
        </Button>
      </Form>
    </div>
  );
};

export default TeamSelection;
