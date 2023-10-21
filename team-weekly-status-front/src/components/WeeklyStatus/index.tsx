import React, { useEffect, useState } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import moment from "moment";
import { userStore } from "../../store";
import { WeeklyStatusData } from "../../types/WeeklyStatus.types";
import { makeApiRequest } from "../../services/apiHelper";

interface WeeklyStatusProps {
  role: "TeamLead" | "CurrentWeekReporter" | "Normal";
  teamName: string;
  memberName: string;
  memberId: number;
}

const WeeklyStatus: React.FC = () => {
  const { role, teamName, memberName, memberId } = userStore();
  const [localMemberId, setLocalMemberId] = useState(memberId);
  const [existingWeeklyStatus, setExistingWeeklyStatus] =
    useState<WeeklyStatusData | null>(null);
  const [doneThisWeek, setDoneThisWeek] = useState<string[]>([""]);
  const [planForNextWeek, setPlanForNextWeek] = useState<string[]>([""]);
  const [blockers, setBlockers] = useState<string>("");
  const [upcomingPTO, setUpcomingPTO] = useState<string[]>([]);
  const [selectedDate, setSelectedDate] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate, setStartDate] = useState(initialStartDate);

  //const startDate = moment().startOf("week").toDate();
  const endDate = moment().endOf("week").toDate();
  const nextWeekStart = moment().add(1, "weeks").startOf("isoWeek");
  const nextWeekEnd = moment().add(1, "weeks").endOf("isoWeek");

  useEffect(() => {
    // Subscribe to memberId changes
    const unsubscribe = userStore.subscribe((state) => {
      if (state.memberId !== localMemberId) {
        setLocalMemberId(state.memberId);
      }
    });

    // Cleanup subscription on component unmount
    return () => unsubscribe();
  }, []);

  useEffect(() => {
    const fetchExistingStatus = async () => {
      const requestData = {
        memberId: memberId,
        weekStartDate: startDate.toISOString(),
      };
      const response: WeeklyStatusData = await makeApiRequest(
        "/WeeklyStatus/GetByMemberIdAndStartDate",
        "POST",
        requestData
      );

      if (response) {
        setExistingWeeklyStatus(response);
        setDoneThisWeek(response.doneThisWeek);
        setPlanForNextWeek(response.planForNextWeek);
        setUpcomingPTO(
          response.upcomingPTO.map((date) =>
            typeof date === "string" ? date : date.toISOString().split("T")[0]
          )
        );
        setBlockers(response.blockers);
      }
    };

    fetchExistingStatus();
  }, [localMemberId, startDate]);

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

    const dataToSubmit: WeeklyStatusData = {
      id: existingWeeklyStatus?.id || 0,
      weekStartDate: startDate,
      doneThisWeek,
      planForNextWeek,
      upcomingPTO,
      blockers,
      memberId,
    };

    console.log(dataToSubmit);

    try {
      const endpoint = existingWeeklyStatus
        ? "/WeeklyStatus/Edit"
        : "/WeeklyStatus/Add";
      const method = existingWeeklyStatus ? "PUT" : "POST";

      const response = await makeApiRequest<{ success: boolean }>(
        endpoint,
        method,
        dataToSubmit
      );
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
  

  return (
    <div className="d-flex flex-column align-items-center mt-5">
      <Form onSubmit={handleSubmit} style={{ width: "70%" }}>
        <h2>Team {teamName}</h2>
        <h3>Welcome {memberName}!</h3>
        <h3>
          Weekly Status: {moment(startDate).format("MMM DD")} -{" "}
          {moment(endDate).format("MMM DD")}
        </h3>
        {success && (
          <Alert variant="success" className="mt-3">
            Your weekly status has been saved!
          </Alert>
        )}
        {error && (
          <Alert variant="danger" className="mt-3">
            {error}
          </Alert>
        )}
        {/* What was done this week: */}
        <Form.Group controlId="doneThisWeek" style={{ paddingTop: "20px" }}>
          <Form.Label style={{ fontWeight: "bold" }}>
            What was done this week:
          </Form.Label>
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
        <Form.Group controlId="planForNextWeek" style={{ paddingTop: "20px" }}>
          <Form.Label style={{ fontWeight: "bold" }}>
            Plan for Next Week
          </Form.Label>
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
        <Form.Group controlId="upcomingPTO" style={{ paddingTop: "20px" }}>
          <Form.Label style={{ fontWeight: "bold" }}>Upcoming PTO</Form.Label>
          <Form.Control
            type="date"
            value={selectedDate || ""}
            min={nextWeekStart.format("YYYY-MM-DD")}
            max={nextWeekEnd.format("YYYY-MM-DD")}
            onChange={handleDateChange}
          />
        </Form.Group>

        <div className="mt-2">
          <span style={{ fontWeight: "bold" }}>Selected dates: </span>
          {upcomingPTO
            .map((dateStr) => moment(dateStr).format("MMM DD"))
            .join(", ")}
        </div>

        {/* Blockers */}
        <Form.Group controlId="blockers" style={{ paddingTop: "20px" }}>
          <Form.Label style={{ fontWeight: "bold" }}>Blockers</Form.Label>
          <Form.Control
            as="textarea"
            rows={3}
            value={blockers}
            onChange={(e) => setBlockers(e.target.value)}
          />
        </Form.Group>
        <Form.Group
          controlId="submitButton"
          style={{ width: "20%", marginLeft: "auto", marginRight: "auto" }}
        >
          <Button variant="primary" type="submit" className="w-100 mt-3">
            Save
          </Button>
        </Form.Group>
      </Form>
    </div>
  );
};

export default WeeklyStatus;
