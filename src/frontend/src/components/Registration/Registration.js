import React from "react";
import "./Registration.css";

const Registration = ({ onClose }) => {
  return (
    <div className="modal-backdrop">
      <div className="modal-content">
        <h2>Register</h2>
        <form>
          <input type="text" placeholder="Name" className="form-control mb-3" />
          <input
            type="email"
            placeholder="Email"
            className="form-control mb-3"
          />
          <input
            type="password"
            placeholder="Password"
            className="form-control mb-3"
          />
          <button type="submit" className="btn btn-primary w-100 mb-2">
            Submit
          </button>
        </form>
        <button className="btn btn-secondary w-100" onClick={onClose}>
          Close
        </button>
      </div>
    </div>
  );
};

export default Registration;
