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
  const [isCurrentWeekReporter, setIsCurrentWeekReporter] = useState<boolean>(false);
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

  // Prepare member options, excluding existing members (when adding)
  const memberOptions = allMembers
    .filter((member) => !existingMemberIds.includes(member.id))
    .map((member) => ({
      value: member.id,
      label: member.name,
    }));

  // Combined useEffect to handle state initialization
  useEffect(() => {
    if (teamMember) {
      // Editing mode
      setMemberId(teamMember.memberId);
      setSelectedMemberOption({
        value: teamMember.memberId,
        label: teamMember.memberName,
      });
      setIsTeamLead(teamMember.isTeamLead);
      setIsCurrentWeekReporter(teamMember.isCurrentWeekReporter);
      setStartActiveDate(teamMember.startActiveDate?.slice(0, 10) || "");
      setEndActiveDate(teamMember.endActiveDate?.slice(0, 10) || "");
    } else {
      // Adding mode or reset
      setMemberId(null);
      setSelectedMemberOption(null);
      setIsTeamLead(false);
      setIsCurrentWeekReporter(false);
      setStartActiveDate("");
      setEndActiveDate("");
    }
  }, [teamMember]);

  const handleSubmit = async () => {
    const currentMemberId = teamMember ? teamMember.memberId : selectedMemberOption?.value;

    if (!currentMemberId) {
      setError("Please select a member.");
      return;
    }

    const payload = {
      teamId,
      memberId: currentMemberId,
      isTeamLead,
      isCurrentWeekReporter,
      startActiveDate: startActiveDate || null,
      endActiveDate: endActiveDate || null,
    };

    try {
      const endpoint = teamMember ? "/TeamMember/Update" : "/TeamMember/Add";
      const method = teamMember ? "PUT" : "POST";

      await makeApiRequest(endpoint, method, payload);

      onSave();
      onHide();
    } catch (error) {
      console.error(error);
      setError("An error occurred while saving the team member.");
    }
  };

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
          <Form.Group controlId="isCurrentWeekReporterCheckbox">
            <Form.Check
              type="checkbox"
              label="Current Week Reporter"
              checked={isCurrentWeekReporter}
              onChange={(e) => setIsCurrentWeekReporter(e.target.checked)}
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
