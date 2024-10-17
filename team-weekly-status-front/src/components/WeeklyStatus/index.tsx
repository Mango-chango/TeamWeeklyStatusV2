import React, { useEffect, useState } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import moment from "moment";
import { userStore } from "../../store";
import {
  WeeklyStatusData,
  TaskWithSubtasks,
  Subtask,
} from "../../types/WeeklyStatus.types";
import { makeApiRequest } from "../../services/apiHelper";
import { useNavigate } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./styles.css";
import ReportPreview from "../ReportPreview/index";
import StaticModal from "../UI/StaticModal";
import ContentModal from "../ContentModal";

interface WeeklyStatusProps {
  teamName: string;
  memberName: string;
  memberId: number;
}

const WeeklyStatus: React.FC = () => {
  const {
    teamId,
    teamName,
    memberName,
    memberId,
    memberActiveTeams,
    isAdmin,
    isTeamLead,
    isCurrentWeekReporter,
    featureFlags
  } = userStore();
  const [localMemberId, setLocalMemberId] = useState(memberId);
  const [localTeamId, setLocalTeamId] = useState(teamId);
  const [existingWeeklyStatus, setExistingWeeklyStatus] =
    useState<WeeklyStatusData | null>(null);
  const [doneThisWeek, setDoneThisWeek] = useState<TaskWithSubtasks[]>([
    { taskDescription: "", subtasks: [{ subtaskDescription: "" }] }, // Ensuring subtasks are an array of Subtask objects
  ]);
  const [planForNextWeek, setPlanForNextWeek] = useState<TaskWithSubtasks[]>([
    { taskDescription: "", subtasks: [{ subtaskDescription: "" }] },
  ]);
  const [blockers, setBlockers] = useState<string>("");
  const [upcomingPTO, setUpcomingPTO] = useState<string[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [error, setError] = useState<boolean>(false);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate, setStartDate] = useState(initialStartDate);
  const isWeekday = (date) => {
    const day = date.getDay();
    return day !== 0 && day !== 6;
  };

  const [selectedDates, setSelectedDates] = useState<Date[]>([]);

  const [showModal, setShowModal] = useState(false);

  const endDate = moment().endOf("week").toDate();
  const nextWeekStart = moment().add(1, "weeks").startOf("isoWeek");
  const inTwoMonths = moment().add(2, "months").endOf("isoWeek");

  const navigate = useNavigate();

  const [localIsAdmin, setLocalIsAdmin] = useState<boolean>(isAdmin);
  const [localIsTeamLead, setLocalIsTeamLead] = useState<boolean>(isTeamLead);
  const [localIsCurrentWeekReporter, setLocalIsCurrentWeekReporter] =
    useState<boolean>(isCurrentWeekReporter);

  const [showContentModal, setShowContentModal] = useState<boolean>(false);

  useEffect(() => {
    // Check if the feature flag is enabled
    if (featureFlags.showContentModal) {
      setShowContentModal(true);
    }
  }, [featureFlags.showContentModal]);

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
    // Subscribe to teamId changes
    const unsubscribe = userStore.subscribe((state) => {
      if (state.teamId !== localTeamId) {
        setLocalTeamId(state.teamId);
      }
    });

    // Cleanup subscription on component unmount
    return () => unsubscribe();
  }, []);

  useEffect(() => {
    const fetchExistingStatus = async () => {
      const requestData = {
        memberId: memberId,
        teamId: userStore.getState().teamId,
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
          response.upcomingPTO.map((date) => moment(date).format("YYYY-MM-DD"))
        );
        setSelectedDates(
          response.upcomingPTO.map((date) => moment(date).toDate())
        );
        setBlockers(response.blockers);
      }
    };

    fetchExistingStatus();
  }, [localMemberId, startDate]);

  const handleSubtaskChange = (
    taskIndex: number,
    subtaskIndex: number,
    value: string,
    setFunction: React.Dispatch<React.SetStateAction<TaskWithSubtasks[]>>
  ) => {
    setFunction((currentTasks) => {
      const newTasks = [...currentTasks];
      newTasks[taskIndex].subtasks[subtaskIndex] = {
        subtaskDescription: value,
      };
      return newTasks;
    });
  };

  const addSubtask = (
    taskIndex: number,
    setFunction: React.Dispatch<React.SetStateAction<TaskWithSubtasks[]>>
  ) => {
    setFunction((currentTasks) => {
      const newTasks = [...currentTasks];
      newTasks[taskIndex].subtasks.push({ subtaskDescription: "" });
      return newTasks;
    });
  };

  const removeSubtask = (
    taskIndex: number,
    subtaskIndex: number,
    setFunction: React.Dispatch<React.SetStateAction<TaskWithSubtasks[]>>
  ) => {
    setFunction((currentTasks) => {
      const newTasks = [...currentTasks];
      newTasks[taskIndex].subtasks.splice(subtaskIndex, 1);
      return newTasks;
    });
  };

  const handleTaskChange = (
    index: number,
    value: string,
    setFunction: React.Dispatch<React.SetStateAction<TaskWithSubtasks[]>>
  ) => {
    setFunction((currentTasks) => {
      const newTasks = [...currentTasks];
      newTasks[index] = { ...newTasks[index], taskDescription: value };
      return newTasks;
    });
  };

  const handleDateChange = (date: Date | null) => {
    if (date) {
      const dateStr = moment(date).format("YYYY-MM-DD");
      setUpcomingPTO((prev) => {
        if (prev.includes(dateStr)) {
          return prev.filter((d) => d !== dateStr);
        } else {
          return [...prev, dateStr];
        }
      });

      setSelectedDates((prev) => {
        if (prev.some((d) => moment(d).isSame(date, "day"))) {
          return prev.filter((d) => !moment(d).isSame(date, "day"));
        } else {
          return [...prev, date];
        }
      });
    }
  };

  const highlightWithRanges = [
    {
      "custom-highlight": selectedDates,
    },
  ];

  const addTask = (
    setFunction: React.Dispatch<React.SetStateAction<TaskWithSubtasks[]>>
  ) => {
    setFunction((prev) => [...prev, { taskDescription: "", subtasks: [] }]);
  };

  const removeTask = (
    index: number,
    setFunction: React.Dispatch<React.SetStateAction<TaskWithSubtasks[]>>
  ) => {
    setFunction((prev) => prev.filter((_, idx) => idx !== index));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const dataToSubmit: WeeklyStatusData = {
      id: existingWeeklyStatus?.id || 0,
      weekStartDate: startDate,
      doneThisWeek: doneThisWeek.map((task) => ({
        taskDescription: task.taskDescription,
        subtasks: task.subtasks
          .filter((subtask) => subtask.subtaskDescription.trim() !== "")
          .map((subtask) => ({
            subtaskDescription: subtask.subtaskDescription,
          })),
      })),
      planForNextWeek: planForNextWeek.map((task) => ({
        taskDescription: task.taskDescription,
        subtasks: task.subtasks
          .filter((subtask) => subtask.subtaskDescription.trim() !== "")
          .map((subtask) => ({
            subtaskDescription: subtask.subtaskDescription,
          })),
      })),
      upcomingPTO,
      blockers,
      memberId,
      teamId,
    };

    // console.log(dataToSubmit);

    try {
      const endpoint = existingWeeklyStatus
        ? "/WeeklyStatus/Edit"
        : "/WeeklyStatus/Add";
      const method = existingWeeklyStatus ? "PUT" : "POST";

      const response = await makeApiRequest<
        WeeklyStatusData | { success: boolean }
      >(endpoint, method, dataToSubmit);
      setExistingWeeklyStatus(response as WeeklyStatusData);
      displaySuccessMessage();
    } catch (err) {
      setSuccess(false);
      displayErrorMessage();
    }
  };

  const displaySuccessMessage = () => {
    setSuccess(true);

    setTimeout(() => {
      setSuccess(false);
    }, 5000);
  };

  const displayErrorMessage = () => {
    setError(true);

    setTimeout(() => {
      setError(false);
    }, 8000);
  };

  const statusReporting = () => {
    navigate("/status-reporting");
  };

  const assignReporter = () => {
    navigate("/assign-reporter");
  };

  const handleShowModal = () => setShowModal(true);
  const handleCloseModal = () => setShowModal(false);

  const backTeamSelection = () => {
    navigate("/team-selection");
  };

  const handleAdminPanel = () => {
    navigate("/admin");
  };

  return (
    <div className="container-main">
      <Form onSubmit={handleSubmit} className="weekly-status__form__container">
        <h1>Team {teamName}</h1>
        <h2>Welcome {memberName}!</h2>
        <h2>
          Weekly Status: {moment(startDate).format("MMM DD")} -{" "}
          {moment(endDate).format("MMM DD")}
        </h2>
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
        <Form.Group controlId="doneThisWeek" className="form__group">
          <Form.Label className="form__label">
            What was done this week:
          </Form.Label>
          {doneThisWeek.map((taskWithSubtasks, taskIndex) => (
            <div key={taskIndex} className="mb-2">
              <Row>
                <Col>
                  <Form.Control
                    type="text"
                    placeholder={`Task ${taskIndex + 1}`}
                    value={taskWithSubtasks.taskDescription}
                    onChange={(e) =>
                      handleTaskChange(
                        taskIndex,
                        e.target.value,
                        setDoneThisWeek
                      )
                    }
                  />
                  {taskWithSubtasks.subtasks.map((subtask, subtaskIndex) => (
                    <div className="form__group__subtask" key={subtaskIndex}>
                      <Form.Control
                        key={subtaskIndex}
                        type="text"
                        placeholder={`Subtask ${subtaskIndex + 1}`}
                        value={subtask.subtaskDescription}
                        onChange={(e) =>
                          handleSubtaskChange(
                            taskIndex,
                            subtaskIndex,
                            e.target.value,
                            setDoneThisWeek
                          )
                        }
                      />
                      <Button
                        variant="danger"
                        onClick={() =>
                          removeSubtask(
                            taskIndex,
                            subtaskIndex,
                            setDoneThisWeek
                          )
                        }
                        className="btn-icon"
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          width="16"
                          height="16"
                          fill="currentColor"
                          className="bi bi-trash"
                          viewBox="0 0 16 16"
                        >
                          <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />
                          <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />
                        </svg>
                      </Button>
                    </div>
                  ))}
                  <div className="form__btn__subtask">
                    <Button
                      variant="secondary"
                      onClick={() => addSubtask(taskIndex, setDoneThisWeek)}
                    >
                      Add Subtask
                    </Button>
                  </div>
                </Col>
                <Col xs="auto">
                  <Button
                    variant="danger"
                    onClick={() => removeTask(taskIndex, setDoneThisWeek)}
                    className="btn-icon"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-trash"
                      viewBox="0 0 16 16"
                    >
                      <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />
                      <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />
                    </svg>
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
        <Form.Group controlId="planForNextWeek" className="form__group">
          <Form.Label className="form__label">Plan for Next Week</Form.Label>
          {planForNextWeek.map((taskWithSubtasks, taskIndex) => (
            <div key={taskIndex} className="mb-2">
              <Row>
                <Col>
                  <Form.Control
                    type="text"
                    placeholder={`Task ${taskIndex + 1}`}
                    value={taskWithSubtasks.taskDescription}
                    onChange={(e) =>
                      handleTaskChange(
                        taskIndex,
                        e.target.value,
                        setPlanForNextWeek
                      )
                    }
                  />
                  {taskWithSubtasks.subtasks.map((subtask, subtaskIndex) => (
                    <div className="form__group__subtask" key={subtaskIndex}>
                      <Form.Control
                        key={subtaskIndex}
                        type="text"
                        placeholder={`Subtask ${subtaskIndex + 1}`}
                        value={subtask.subtaskDescription}
                        onChange={(e) =>
                          handleSubtaskChange(
                            taskIndex,
                            subtaskIndex,
                            e.target.value,
                            setPlanForNextWeek
                          )
                        }
                      />
                      <Button
                        variant="danger"
                        onClick={() =>
                          removeSubtask(
                            taskIndex,
                            subtaskIndex,
                            setPlanForNextWeek
                          )
                        }
                        className="btn-icon"
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          width="16"
                          height="16"
                          fill="currentColor"
                          className="bi bi-trash"
                          viewBox="0 0 16 16"
                        >
                          <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />
                          <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />
                        </svg>
                      </Button>
                    </div>
                  ))}
                  <div className="form__btn__subtask">
                    <Button
                      variant="secondary"
                      onClick={() => addSubtask(taskIndex, setPlanForNextWeek)}
                    >
                      Add Subtask
                    </Button>
                  </div>
                </Col>
                <Col xs="auto">
                  <Button
                    variant="danger"
                    onClick={() => removeTask(taskIndex, setPlanForNextWeek)}
                    className="btn-icon"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-trash"
                      viewBox="0 0 16 16"
                    >
                      <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />
                      <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />
                    </svg>
                  </Button>
                </Col>
              </Row>
            </div>
          ))}
          <Button
            variant="secondary"
            onClick={() => addTask(setPlanForNextWeek)}
          >
            Add Task
          </Button>
        </Form.Group>

        {/* Upcoming PTO */}
        <Form.Group controlId="upcomingPTO" className="form__group">
          <Form.Label className="form__label">Upcoming Time Off</Form.Label>
          <DatePicker
            selected={selectedDate}
            onChange={handleDateChange}
            minDate={nextWeekStart.toDate()}
            maxDate={inTwoMonths.toDate()}
            dateFormat="yyyy-MM-dd"
            className="form-control"
            highlightDates={highlightWithRanges}
            showIcon
            toggleCalendarOnIconClick
            monthsShown={2}
            withPortal
            filterDate={isWeekday}
            placeholderText="Click to select a date"
          />
        </Form.Group>

        <div className="mt-2">
          <span className="form__label">Selected dates: </span>
          {upcomingPTO
            .map((dateStr) => moment(dateStr).format("MMM DD"))
            .join(", ")}
        </div>

        {/* Blockers */}
        <Form.Group controlId="blockers" className="form__group">
          <Form.Label className="form__label">Blockers</Form.Label>
          <Form.Control
            as="textarea"
            rows={3}
            value={blockers}
            onChange={(e) => setBlockers(e.target.value)}
          />
        </Form.Group>
        <Form.Group controlId="buttons" className="form__btngroup">
          <Button variant="primary" type="submit" className="form__btn">
            Save Weekly Status
          </Button>
          {localIsCurrentWeekReporter && (
            <Button
              variant="primary"
              onClick={statusReporting}
              className="form__btn"
            >
              Report
            </Button>
          )}

          {localIsTeamLead && (
            <Button
              variant="primary"
              onClick={assignReporter}
              className="form__btn"
            >
              Assign Reporter
            </Button>
          )}

          <Button
            onClick={handleShowModal}
            variant="primary"
            className="form__btn"
          >
            Preview Report
          </Button>

          {memberActiveTeams && memberActiveTeams.length > 1 && (
            <Button
              variant="primary"
              onClick={backTeamSelection}
              className="form__btn"
            >
              Team Selection
            </Button>
          )}

          {localIsAdmin && (
            <Button
              variant="primary"
              onClick={handleAdminPanel}
              className="form__btn"
            >
              Admin Panel
            </Button>
          )}

          <StaticModal
            show={showModal}
            onHide={handleCloseModal}
            onClose={handleCloseModal}
          >
            <ReportPreview />
          </StaticModal>
        </Form.Group>
      </Form>
      <ContentModal
        show={showContentModal}
        onHide={() => setShowContentModal(false)}
      />
    </div>
  );
};

export default WeeklyStatus;
