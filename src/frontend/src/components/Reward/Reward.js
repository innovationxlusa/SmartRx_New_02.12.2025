import React from "react";
import "./Reward.css";

const Reward = ({ onClose }) => {
  return (
    <div className="modal-backdrop">
      <div className="modal-content">
        <h2>Reward Points</h2>
        <p>
          You have earned <strong>1200</strong> reward points. These points can
          be redeemed for various offers and discounts. Keep earning!
        </p>
        <button className="btn btn-secondary w-100" onClick={onClose}>
          Close
        </button>
      </div>
    </div>
  );
};

export default Reward;
