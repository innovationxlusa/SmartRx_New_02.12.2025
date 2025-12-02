import React, { useState } from "react";
import Modal from "./CustomModal";

const ModalPage = () => {
  const [open, setOpen] = useState(false);

  return (
    <>
      <button className="btn btn-primary" onClick={() => setOpen(true)}>
        Open Modal
      </button>
      <Modal isOpen={open} close={() => setOpen(false)}>
        {/* Custom modal content */}
        <div>This is Props Data</div>
        <div className="modal-icon">
          <i className="fas fa-cubes"></i>
        </div>
        <h3 className="title">PPC Bee</h3>
        <p className="description">is currently in maintenance mode.</p>
      </Modal>
    </>
  );
};

export default ModalPage;
