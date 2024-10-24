import React from "react";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import "./styles.css";

interface StaticModalProps {
  show: boolean;
  onHide: () => void;
  onClose: () => void;
  children: React.ReactNode;
}

const StaticModal: React.FC<StaticModalProps> = ({
  show,
  onHide,
  onClose,
  children,
}) => {
  return (
    <Modal show={show} onHide={onHide} backdrop="static" keyboard={false} centered dialogClassName="90-w">
      <Modal.Header closeButton>
        <Modal.Title>Report Preview</Modal.Title>
      </Modal.Header>
      <Modal.Body>{children}</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default StaticModal;
