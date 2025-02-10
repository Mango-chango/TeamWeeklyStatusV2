// WeeklyStatusRichText/index.tsx

import React, { useEffect, useState } from "react";
import { Button, Form, Alert, Container, Spinner } from "react-bootstrap";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

import moment from "moment";
import { userStore } from "../../store";
import { WeeklyStatusRichTextData } from "../../types/WeeklyStatus.types";
import { makeApiRequest } from "../../services/apiHelper";
import { useNavigate } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./styles.css";
import ReportPreview from "../ReportPreview/index";
import StaticModal from "../UI/StaticModal";
import ContentModal from "../ContentModal";

import EnhancementComparisonModal from "./EnhancementComparisonModal";

const WeeklyStatusRichText: React.FC = () => {
  const {
    teamId,
    teamName,
    memberName,
    memberId,
    memberActiveTeams,
    isAdmin,
    isTeamLead,
    isCurrentWeekReporter,
    featureFlags,
  } = userStore();

  const [localMemberId, setLocalMemberId] = useState(memberId);
  const [localTeamId, setLocalTeamId] = useState(teamId);
  const [existingWeeklyStatus, setExistingWeeklyStatus] =
    useState<WeeklyStatusRichTextData | null>(null);

  const [doneThisWeekContent, setDoneThisWeekContent] = useState<string>("");
  const [planForNextWeekContent, setPlanForNextWeekContent] =
    useState<string>("");

  const [blockersContent, setBlockersContent] = useState<string>(""); // Updated

  const [upcomingPTO, setUpcomingPTO] = useState<string[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);

  const initialStartDate = moment().startOf("week").toDate();
  const [startDate] = useState(initialStartDate);
  const isWeekday = (date: Date) => {
    const day = date.getDay();
    return day !== 0 && day !== 6;
  };

  const [selectedDates, setSelectedDates] = useState<Date[]>([]);

  const [showModal, setShowModal] = useState(false);

  const endDate = moment().endOf("week").toDate();
  const nextWeekStart = moment().add(1, "weeks").startOf("isoWeek");
  const inTwoMonths = moment().add(2, "months").endOf("isoWeek");

  const navigate = useNavigate();

  const [localIsAdmin] = useState<boolean>(isAdmin);
  const [localIsTeamLead] = useState<boolean>(isTeamLead);
  const [localIsCurrentWeekReporter] = useState<boolean>(isCurrentWeekReporter);

  const [showContentModal, setShowContentModal] = useState<boolean>(false);

  const [isEnhancing, setIsEnhancing] = useState<boolean>(false); // Updated

  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );

  const [originalContent, setOriginalContent] = useState<{
    doneThisWeekContent: string;
    planForNextWeekContent: string;
    blockersContent: string;
  }>({
    doneThisWeekContent: "",
    planForNextWeekContent: "",
    blockersContent: "",
  });

  const [enhancedContent, setEnhancedContent] = useState<{
    doneThisWeekContent: string;
    planForNextWeekContent: string;
    blockersContent: string;
  }>({
    doneThisWeekContent: "",
    planForNextWeekContent: "",
    blockersContent: "",
  });

  const [showComparisonModal, setShowComparisonModal] =
    useState<boolean>(false);

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
  }, [localMemberId]);

  useEffect(() => {
    // Subscribe to teamId changes
    const unsubscribe = userStore.subscribe((state) => {
      if (state.teamId !== localTeamId) {
        setLocalTeamId(state.teamId);
      }
    });

    // Cleanup subscription on component unmount
    return () => unsubscribe();
  }, [localTeamId]);

  useEffect(() => {
    const fetchExistingStatus = async () => {
      const requestData = {
        memberId: memberId,
        teamId: userStore.getState().teamId,
        weekStartDate: startDate.toISOString(),
      };
      const response: WeeklyStatusRichTextData = await makeApiRequest(
        "/v2.0/WeeklyStatus/GetByMemberIdAndStartDate",
        "POST",
        requestData
      );

      if (response) {
        setExistingWeeklyStatus(response);
        setDoneThisWeekContent(response.doneThisWeekContent || "");
        setPlanForNextWeekContent(response.planForNextWeekContent || "");
        setBlockersContent(response.blockers || ""); // Updated
        setUpcomingPTO(
          response.upcomingPTO.map((date) => moment(date).format("YYYY-MM-DD"))
        );
        setSelectedDates(
          response.upcomingPTO.map((date) => moment(date).toDate())
        );
      }
    };

    fetchExistingStatus();
  }, [localMemberId, startDate]);

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

  const handleEnhanceContent = async () => {
    setIsEnhancing(true);
    try {
      const payload = {
        teamId,
        doneThisWeekContent,
        planForNextWeekContent,
        blockersContent,
      };
      const response = await makeApiRequest(
        `/v2.0/WeeklyStatus/GetAIEnhancedContent`,
        "POST",
        payload
      );

      const enhancedResponse = response as any;

      // Store the original content
      setOriginalContent({
        doneThisWeekContent,
        planForNextWeekContent,
        blockersContent,
      });

      // Store the enhanced content
      setEnhancedContent({
        doneThisWeekContent: enhancedResponse.enhancedDoneThisWeekContent,
        planForNextWeekContent: enhancedResponse.enhancedPlanForNextWeekContent,
        blockersContent: enhancedResponse.enhancedBlockersContent,
      });

      setShowComparisonModal(true);
    } catch (error) {
      console.error("Error enhancing content:", error);
      displayErrorMessage("An error occurred while enhancing your content.");
    } finally {
      setIsEnhancing(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const dataToSubmit: WeeklyStatusRichTextData = {
      id: existingWeeklyStatus?.id || 0,
      weekStartDate: startDate,
      doneThisWeekContent,
      planForNextWeekContent,
      blockers: blockersContent, // Updated
      upcomingPTO,
      memberId,
      teamId,
    };

    try {
      const endpoint = existingWeeklyStatus
        ? "/v2.0/WeeklyStatus/Edit"
        : "/v2.0/WeeklyStatus/Add";
      const method = existingWeeklyStatus ? "PUT" : "POST";

      const response = await makeApiRequest<
        WeeklyStatusRichTextData | { success: boolean }
      >(endpoint, method, dataToSubmit);
      setExistingWeeklyStatus(response as WeeklyStatusRichTextData);
      displaySuccessMessage();
    } catch (err) {
      displayErrorMessage("An error occurred while saving your weekly status.");
    }
  };

  const displaySuccessMessage = () => {
    setAlertMessage("Your weekly status has been saved!");
    setAlertVariant("success");
    setShowAlert(true);

    setTimeout(() => {
      setShowAlert(false);
    }, 5000);
  };

  const displayErrorMessage = (message: string) => {
    setAlertMessage(message);
    setAlertVariant("danger");
    setShowAlert(true);

    setTimeout(() => {
      setShowAlert(false);
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

  const handleApplyEnhancedContent = (selections: {
    useEnhancedDoneThisWeekContent: boolean;
    useEnhancedPlanForNextWeekContent: boolean;
    useEnhancedBlockersContent: boolean;
  }) => {
    if (selections.useEnhancedDoneThisWeekContent) {
      setDoneThisWeekContent(enhancedContent.doneThisWeekContent);
    }
    if (selections.useEnhancedPlanForNextWeekContent) {
      setPlanForNextWeekContent(enhancedContent.planForNextWeekContent);
    }
    if (selections.useEnhancedBlockersContent) {
      setBlockersContent(enhancedContent.blockersContent);
    }
    setShowComparisonModal(false);
  };

  return (
    <Container fluid className="container-main">
      <Form onSubmit={handleSubmit}>
        <h1>Team {teamName}</h1>
        <h2>Welcome {memberName}!</h2>
        <h2>
          Weekly Status: {moment(startDate).format("MMM DD")} -{" "}
          {moment(endDate).format("MMM DD")}
        </h2>
        {showAlert && (
          <Alert
            variant={alertVariant}
            className={`alert-fixed alert-custom-${alertVariant}`}
            onClose={() => setShowAlert(false)}
            dismissible
          >
            {alertMessage}
          </Alert>
        )}

        {/* What was done this week */}
        <Form.Group controlId="doneThisWeek" className="form__group">
          <Form.Label className="form__label">
            What was done this week:
          </Form.Label>
          <ReactQuill
            value={doneThisWeekContent}
            onChange={setDoneThisWeekContent}
            modules={quillModules}
            formats={quillFormats}
          />
        </Form.Group>

        {/* Plan for Next Week */}
        <Form.Group controlId="planForNextWeek" className="form__group">
          <Form.Label className="form__label">Plan for Next Week</Form.Label>
          <ReactQuill
            value={planForNextWeekContent}
            onChange={setPlanForNextWeekContent}
            modules={quillModules}
            formats={quillFormats}
          />
        </Form.Group>

        {/* Blockers */}
        <Form.Group controlId="blockers" className="form__group">
          <Form.Label className="form__label">Blockers</Form.Label>
          <ReactQuill
            value={blockersContent}
            onChange={setBlockersContent}
            modules={quillModules}
            formats={quillFormats}
          />
        </Form.Group>

        {/* Improve Grammar Button */}
        <Button
          variant="secondary"
          onClick={handleEnhanceContent}
          disabled={isEnhancing}
          className="mt-2"
        >
          {isEnhancing ? (
            <>
              <Spinner
                as="span"
                animation="border"
                size="sm"
                role="status"
                aria-hidden="true"
              />{" "}
              Enhancing...
            </>
          ) : (
            "Improve Grammar"
          )}
        </Button>

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
          <Form.Label className="form__label">Selected dates: </Form.Label>
          {upcomingPTO
            .map((dateStr) => moment(dateStr).format("MMM DD"))
            .join(", ")}
        </div>

        <Form.Group controlId="buttons" className="form__btngroup flex-wrap">
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
      <EnhancementComparisonModal
        show={showComparisonModal}
        onHide={() => setShowComparisonModal(false)}
        originalContent={originalContent}
        enhancedContent={enhancedContent}
        onApply={handleApplyEnhancedContent}
      />
    </Container>
  );
};

const quillColors = [
  "purple",
  "#785412",
  "#452632",
  "#856325",
  "#963254",
  "#254563",
  "white",
];

const quillModules = {
  toolbar: [
    [{ header: [1, 2, 3, 4, 5, 6, false] }],
    ["bold", "italic", "underline", "strike", "blockquote"],
    [{ align: ["right", "center", "justify"] }],
    [{ list: "ordered" }, { list: "bullet" }],
    ["link", "image"],
    [{ color: quillColors }],
    [{ background: quillColors }],
    ["clean"],
  ],
};

const quillFormats = [
  'header',
  'font',
  'size',
  'bold',
  'italic',
  'underline',
  'strike',
  'blockquote',
  'list',
  'bullet',
  'indent',
  'link',
  'image',
  'color',
  "background",
  "align"
];

export default WeeklyStatusRichText;
