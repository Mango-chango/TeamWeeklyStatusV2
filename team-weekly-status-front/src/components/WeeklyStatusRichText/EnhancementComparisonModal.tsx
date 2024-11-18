import React from "react";
import { Modal, Button } from "react-bootstrap";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

interface EnhancementComparisonModalProps {
  show: boolean;
  onHide: () => void;
  originalContent: string;
  enhancedContent: string;
  onAccept: () => void;
  onReject: () => void;
}

const EnhancementComparisonModal: React.FC<EnhancementComparisonModalProps> = ({
  show,
  onHide,
  originalContent,
  enhancedContent,
  onAccept,
  onReject,
}) => {
  console.log("Original Content in Modal:", originalContent);
  console.log("Enhanced Content in Modal:", enhancedContent);

  return (
    <Modal show={show} onHide={onHide} size="lg">
      <Modal.Header closeButton>
        <Modal.Title>Compare Original and Enhanced Content</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <div className="d-flex">
          <div className="flex-fill mr-2">
            <h5>Original Content</h5>
            <ReactQuill
              value={originalContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
          <div className="flex-fill ml-2">
            <h5>Enhanced Content</h5>
            <ReactQuill
              value={enhancedContent}
              readOnly={true}
              theme="bubble"
            />
          </div>
        </div>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onReject}>
          Keep Original
        </Button>
        <Button variant="primary" onClick={onAccept}>
          Accept Enhanced
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default EnhancementComparisonModal;
