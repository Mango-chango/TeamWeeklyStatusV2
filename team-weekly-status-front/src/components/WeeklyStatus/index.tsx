import React, { useState } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import moment from "moment";

interface WeeklyStatusProps {
  role: "TeamLead" | "CurrentWeekReporter" | "Normal";
}

const WeeklyStatus: React.FC<WeeklyStatusProps> = ({ role }) => {
  const [doneThisWeek, setDoneThisWeek] = useState<string[]>([""]);
  const [planForNextWeek, setPlanForNextWeek] = useState<string[]>([""]);
  const [blockers, setBlockers] = useState<string>("");
  const [upcomingPTO, setUpcomingPTO] = useState<string[]>([]);
  const [selectedDate, setSelectedDate] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const startDate = moment().startOf("week").toDate();
  const endDate = moment().endOf("week").toDate();
  const nextWeekStart = moment().add(1, 'weeks').startOf('isoWeek');
  const nextWeekEnd = moment().add(1, 'weeks').endOf('isoWeek');

  const handleTaskChange = (
    index: number,
    value: string,
    setFunction: React.Dispatch<React.SetStateAction<string[]>>
  ) => {
    const newTasks = [...doneThisWeek];
    newTasks[index] = value;
    setFunction(newTasks);
  };

  const handlePlanChange = (index: number, value: string) => {
    const newPlans = [...planForNextWeek];
    newPlans[index] = value;
    setPlanForNextWeek(newPlans);
  };

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const dateStr = e.target.value;

    const index = upcomingPTO.indexOf(dateStr);

    if (index !== -1) {
      // Date already exists, remove it
      setUpcomingPTO((prev) => prev.filter((d) => d !== dateStr));
    } else {
      setUpcomingPTO((prev) => [...prev, dateStr]);
    }

    // Clear the selected date to allow reselection
    setSelectedDate(null);
  };

  const addTask = (
    setFunction: React.Dispatch<React.SetStateAction<string[]>>
  ) => {
    setFunction((prev) => [...prev, ""]);
  };

  const removeTask = (
    index: number,
    setFunction: React.Dispatch<React.SetStateAction<string[]>>
  ) => {
    setFunction((prev) => prev.filter((_, idx) => idx !== index));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const dataToSubmit = {
      doneThisWeek,
      planForNextWeek,
      upcomingPTO,
      blockers,
    };

    // TODO: Submit dataToSubmit to the backend
  };

  return (
    <div className="d-flex flex-column align-items-center mt-5">
      <Form onSubmit={handleSubmit} style={{ width: "500px" }}>
        <h3>
          Weekly Status: {moment(startDate).format("MMM DD")} -{" "}
          {moment(endDate).format("MMM DD")}
        </h3>
        {/* Done This Week */}
        <Form.Group controlId="doneThisWeek">
          <Form.Label>Done This Week</Form.Label>
          {doneThisWeek.map((task, index) => (
            <div key={index} className="mb-2">
              <Row>
                <Col>
                  <Form.Control
                    type="text"
                    placeholder={`Task ${index + 1}`}
                    value={task}
                    onChange={(e) =>
                      handleTaskChange(index, e.target.value, setDoneThisWeek)
                    }
                  />
                </Col>
                <Col xs="auto">
                  <Button
                    variant="danger"
                    onClick={() => removeTask(index, setDoneThisWeek)}
                  >
                    Remove
                  </Button>
                </Col>
              </Row>
            </div>
          ))}
          <Button variant="secondary" onClick={() => addTask(setDoneThisWeek)}>
            Add Task
          </Button>
        </Form.Group>
        {/* Plan for Next Week */}
        <Form.Group controlId="planForNextWeek">
          <Form.Label>Plan for Next Week</Form.Label>
          {planForNextWeek.map((plan, index) => (
            <div key={index} className="mb-2">
              <Row>
                <Col>
                  <Form.Control
                    type="text"
                    placeholder={`Plan ${index + 1}`}
                    value={plan}
                    onChange={(e) => handlePlanChange(index, e.target.value)}
                  />
                </Col>
                <Col xs="auto">
                  <Button
                    variant="danger"
                    onClick={() => removeTask(index, setPlanForNextWeek)}
                  >
                    Remove
                  </Button>
                </Col>
              </Row>
            </div>
          ))}
          <Button
            variant="secondary"
            onClick={() => addTask(setPlanForNextWeek)}
          >
            Add Plan
          </Button>
        </Form.Group>
        {/* Upcoming PTO */}
      <Form.Group controlId="upcomingPTO">
        <Form.Label>Upcoming PTO</Form.Label>
        <Form.Control
          type="date"
          value={selectedDate || ""}
          min={nextWeekStart.format('YYYY-MM-DD')}
          max={nextWeekEnd.format('YYYY-MM-DD')}
          onChange={handleDateChange}
        />
      </Form.Group>

        <div className="mt-2">
          Selected dates:{" "}
          {upcomingPTO
            .map((dateStr) => moment(dateStr).format("MMM DD"))
            .join(", ")}
        </div>

        {/* Blockers */}
        <Form.Group controlId="blockers">
          <Form.Label>Blockers</Form.Label>
          <Form.Control
            as="textarea"
            rows={3}
            value={blockers}
            onChange={(e) => setBlockers(e.target.value)}
          />
        </Form.Group>
        <Button variant="primary" type="submit" className="w-100 mt-3">
          Submit
        </Button>
      </Form>

      {error && (
        <Alert variant="danger" className="mt-3">
          {error}
        </Alert>
      )}
    </div>
  );
};

export default WeeklyStatus;
