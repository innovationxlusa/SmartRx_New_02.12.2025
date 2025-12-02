import React from "react";
import "./LoadingSpinner.css"; // Optional styling

const LoadingSpinner = () => (
    <div className="loading-spinner-overlay">
        <div className="loading-spinner">
            <div></div>
            <div></div>
            <div></div>
            <div></div>
        </div>
    </div>
);

export default LoadingSpinner;
