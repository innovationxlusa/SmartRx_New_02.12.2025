import React from "react";
import "./CustomCard.css";

const CustomCard = ({ children }) => {
    return (
        <div className="custom-card">
            <div className="custom-card-wrapper">{children}</div>
        </div>
    );
};

export default CustomCard;
