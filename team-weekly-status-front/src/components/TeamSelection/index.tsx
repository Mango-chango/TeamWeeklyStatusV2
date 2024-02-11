import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Form, ListGroup, Alert } from "react-bootstrap";
import { userStore } from "../../store";
import "./styles.css";

const TeamSelection: React.FC = () => {
  const memberActiveTeams = userStore((state) => state.memberActiveTeams);
  const [selectedTeamId, setSelectedTeamId] = useState<number>(0);
  const [showAlert, setShowAlert] = useState<boolean>(false);
  console.table(memberActiveTeams);
  const navigate = useNavigate();

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (selectedTeamId && selectedTeamId !== 0)  {
      const teamName = memberActiveTeams?.find(team => team.teamId === selectedTeamId)?.teamName ?? '';
      // Set the selected team ID using zustand store
      userStore.getState().setTeamId(selectedTeamId);
      // Set the selected team name using zustand store
      userStore.getState().setTeamName(teamName);
      // Navigate to the weekly status page or the appropriate screen
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
            memberActiveTeams.map((memberTeam) => (
              <ListGroup.Item
                key={memberTeam.teamId}
                action={false}
                active={selectedTeamId === memberTeam.teamId}
                onClick={(event) => {
                  setSelectedTeamId(memberTeam.teamId);
                }}
              >
                {memberTeam.teamName}
              </ListGroup.Item>
            ))}
        </ListGroup>
        <Button type="submit" className="mt-3" variant="primary" size="lg">
          Continue
        </Button>
      </Form>
    </div>
  );
};

export default TeamSelection;
