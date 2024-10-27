import React from 'react';
import { Modal } from 'react-bootstrap';
import './styles.css'; 

interface ContentModalProps {
  show: boolean;
  onHide: () => void;
}

const ContentModal: React.FC<ContentModalProps> = ({ show, onHide }) => {
  return (
    <Modal
      show={show}
      onHide={onHide}
      centered
      dialogClassName="content-modal d-flex align-items-center justify-content-center"
      backdrop="static"
      keyboard={false}
    >
      <Modal.Body
        style={{ padding: 0, cursor: 'pointer' }}
      >
        <img
          src="/chango-es-friday.webp"
          alt="Es Friday Chango!!!"
          style={{ display: 'block', cursor: 'pointer' }}
          onClick={onHide} // Close modal on image click
        />
      </Modal.Body>
    </Modal>
  );
};

export default ContentModal;
