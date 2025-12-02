import React from "react";
import "./Instruction.css";

const Instruction = ({ onClose }) => {
  return (
    <div className="modal-backdrop">
      <div className="modal-content">
        <h2>Instructions</h2>
        <p>
          These are the instructions to help you navigate through the platform.
          Follow the steps to complete your tasks.
        </p>
        <ul>
          <li>Step 1: Do this...</li>
          <li>Step 2: Do that...</li>
          <li>Step 3: Follow these instructions...</li>
        </ul>
        <button className="btn btn-secondary w-100" onClick={onClose}>
          Close
        </button>
      </div>
    </div>
  );
};

export default Instruction;
