// EnhancementComparisonModal.tsx

import React, { useState } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

interface EnhancementComparisonModalProps {
  show: boolean;
  onHide: () => void;
  originalContent: {
    doneThisWeekContent: string;
    planForNextWeekContent: string;
    blockersContent: string;
  };
  enhancedContent: {
    doneThisWeekContent: string;
    planForNextWeekContent: string;
    blockersContent: string;
  };
  onApply: (selections: {
    useEnhancedDoneThisWeekContent: boolean;
    useEnhancedPlanForNextWeekContent: boolean;
    useEnhancedBlockersContent: boolean;
  }) => void;
}

const EnhancementComparisonModal: React.FC<EnhancementComparisonModalProps> = ({
  show,
  onHide,
  originalContent,
  enhancedContent,
  onApply,
}) => {
  const [useEnhancedDoneThisWeekContent, setUseEnhancedDoneThisWeekContent] =
    useState<boolean>(true);
  const [useEnhancedPlanForNextWeekContent, setUseEnhancedPlanForNextWeekContent] =
    useState<boolean>(true);
  const [useEnhancedBlockersContent, setUseEnhancedBlockersContent] =
    useState<boolean>(true);

  const handleApply = () => {
    onApply({
      useEnhancedDoneThisWeekContent,
      useEnhancedPlanForNextWeekContent,
      useEnhancedBlockersContent,
    });
  };

  return (
    <Modal show={show} onHide={onHide} size="lg">
      <Modal.Header closeButton>
        <Modal.Title>Compare Original and Enhanced Content</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {/* Done This Week Section */}
        <h5>What was done this week:</h5>
        <Form.Check
          type="switch"
          id="switch-doneThisWeek"
          label="Use Enhanced Content"
          checked={useEnhancedDoneThisWeekContent}
          onChange={(e) => setUseEnhancedDoneThisWeekContent(e.target.checked)}
        />
        <div className="d-flex">
          <div className="flex-fill mr-2">
            <h6>Original Content</h6>
            <ReactQuill
              value={originalContent.doneThisWeekContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
          <div className="flex-fill ml-2">
            <h6>Enhanced Content</h6>
            <ReactQuill
              value={enhancedContent.doneThisWeekContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
        </div>
        <hr />
        {/* Plan For Next Week Section */}
        <h5>Plan for next week:</h5>
        <Form.Check
          type="switch"
          id="switch-planForNextWeek"
          label="Use Enhanced Content"
          checked={useEnhancedPlanForNextWeekContent}
          onChange={(e) => setUseEnhancedPlanForNextWeekContent(e.target.checked)}
        />
        <div className="d-flex">
          <div className="flex-fill mr-2">
            <h6>Original Content</h6>
            <ReactQuill
              value={originalContent.planForNextWeekContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
          <div className="flex-fill ml-2">
            <h6>Enhanced Content</h6>
            <ReactQuill
              value={enhancedContent.planForNextWeekContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
        </div>
        <hr />
        {/* Blockers Section */}
        <h5>Blockers:</h5>
        <Form.Check
          type="switch"
          id="switch-blockers"
          label="Use Enhanced Content"
          checked={useEnhancedBlockersContent}
          onChange={(e) => setUseEnhancedBlockersContent(e.target.checked)}
        />
        <div className="d-flex">
          <div className="flex-fill mr-2">
            <h6>Original Content</h6>
            <ReactQuill
              value={originalContent.blockersContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
          <div className="flex-fill ml-2">
            <h6>Enhanced Content</h6>
            <ReactQuill
              value={enhancedContent.blockersContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
        </div>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Cancel
        </Button>
        <Button variant="primary" onClick={handleApply}>
          Apply
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default EnhancementComparisonModal;
