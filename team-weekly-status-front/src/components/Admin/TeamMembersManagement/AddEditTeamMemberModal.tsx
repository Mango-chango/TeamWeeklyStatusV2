import React, { useState, useEffect } from "react";
import { Modal, Button, Form, Alert } from "react-bootstrap";
import { makeApiRequest } from "../../../services/apiHelper";
import { TeamMember } from "../../../types/WeeklyStatus.types";
import Select from "react-select";

interface AddEditTeamMemberModalProps {
  show: boolean;
  onHide: () => void;
  teamId: number;
  teamMember?: TeamMember | null;
  onSave: () => void;
  existingMemberIds: number[];
}

const AddEditTeamMemberModal: React.FC<AddEditTeamMemberModalProps> = ({
  show,
  onHide,
  teamId,
  teamMember,
  onSave,
  existingMemberIds,
}) => {
  const [memberId, setMemberId] = useState<number | null>(null);
  const [isTeamLead, setIsTeamLead] = useState<boolean>(false);
  const [startActiveDate, setStartActiveDate] = useState<string>("");
  const [endActiveDate, setEndActiveDate] = useState<string>("");
  const [error, setError] = useState<string>("");

  // State to hold all available members
  const [allMembers, setAllMembers] = useState<{ id: number; name: string }[]>(
    []
  );
  const [selectedMemberOption, setSelectedMemberOption] = useState<{
    value: number;
    label: string;
  } | null>(null);
  const memberOptions = allMembers
    .filter((member) => !existingMemberIds.includes(member.id))
    .map((member) => ({
      value: member.id,
      label: member.name,
    }));

  useEffect(() => {
    // Fetch all members to populate the dropdown
    const fetchMembers = async () => {
      try {
        const response: { id: number; name: string }[] = await makeApiRequest(
          "/Member/GetAll",
          "GET"
        );
        if (response) {
          setAllMembers(response);
        }
      } catch (error) {
        console.error(error);
        setError("An error occurred while fetching members.");
      }
    };

    fetchMembers();
  }, []);

  useEffect(() => {
    if (teamMember) {
      setMemberId(teamMember.memberId);
      setIsTeamLead(teamMember.isTeamLead);
      setStartActiveDate(teamMember.startActiveDate.slice(0, 10)); // Extract date part
      setEndActiveDate(
        teamMember.endActiveDate ? teamMember.endActiveDate.slice(0, 10) : ""
      );
    } else {
      // Reset form
      setMemberId(null);
      setIsTeamLead(false);
      setStartActiveDate("");
      setEndActiveDate("");
    }
  }, [teamMember]);

  const handleSubmit = async () => {
    if (!selectedMemberOption || !startActiveDate) {
      setError("Please fill in all required fields.");
      return;
    }

    const memberId = selectedMemberOption.value;

    const payload = {
      teamId,
      memberId,
      isTeamLead,
      startActiveDate,
      endActiveDate: endActiveDate || null,
    };

    try {
      if (teamMember) {
        // Edit existing team member
        await makeApiRequest(
          `/TeamMember/Update/${teamId}/${memberId}`,
          "PUT",
          payload
        );
      } else {
        // Add new team member
        await makeApiRequest("/TeamMember/Add", "POST", payload);
      }
      onSave();
      onHide();
    } catch (error) {
      console.error(error);
      setError("An error occurred while saving the team member.");
    }
  };

  useEffect(() => {
    if (teamMember) {
      setSelectedMemberOption({
        value: teamMember.memberId,
        label: teamMember.memberName,
      });
      setIsTeamLead(teamMember.isTeamLead);
      setStartActiveDate(teamMember.startActiveDate.slice(0, 10)); // Extract date part
      setEndActiveDate(
        teamMember.endActiveDate ? teamMember.endActiveDate.slice(0, 10) : ""
      );
    } else {
      // Reset form
      setSelectedMemberOption(null);
      setIsTeamLead(false);
      setStartActiveDate("");
      setEndActiveDate("");
    }
  }, [teamMember]);

  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>{teamMember ? "Edit" : "Add"} Team Member</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {error && <Alert variant="danger">{error}</Alert>}
        <Form>
          {!teamMember && (
            <Form.Group controlId="memberSelect">
              <Form.Label>Member</Form.Label>
              <Select
                options={memberOptions}
                value={selectedMemberOption}
                onChange={(option) => setSelectedMemberOption(option)}
                isClearable
                placeholder="Select a member..."
                styles={{
                  control: (base) => ({
                    ...base,
                    borderColor: "#ced4da",
                    minHeight: "38px",
                  }),
                }}
              />
            </Form.Group>
          )}
          {teamMember && (
            <Form.Group controlId="memberName">
              <Form.Label>Member</Form.Label>
              <Form.Control
                type="text"
                value={teamMember.memberName}
                readOnly
              />
            </Form.Group>
          )}

          <Form.Group controlId="isTeamLeadCheckbox">
            <Form.Check
              type="checkbox"
              label="Team Lead"
              checked={isTeamLead}
              onChange={(e) => setIsTeamLead(e.target.checked)}
            />
          </Form.Group>
          <Form.Group controlId="startActiveDate">
            <Form.Label>Start Active Date</Form.Label>
            <Form.Control
              type="date"
              value={startActiveDate}
              onChange={(e) => setStartActiveDate(e.target.value)}
            />
          </Form.Group>
          <Form.Group controlId="endActiveDate">
            <Form.Label>End Active Date</Form.Label>
            <Form.Control
              type="date"
              value={endActiveDate}
              onChange={(e) => setEndActiveDate(e.target.value)}
            />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Cancel
        </Button>
        <Button variant="primary" onClick={handleSubmit}>
          {teamMember ? "Save Changes" : "Add Member"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default AddEditTeamMemberModal;
