import React from "react";
import { Link } from "react-router-dom";
import "./CustomCheck.css";

const CustomCheck = ({
    value,
    onChange,
    error,
    label = "I agree with the", // Default label text
    name = "",
    disabled = false,
    linkSection = "",
    linkText = "",
    onLinkClick = null,
    className = "",
    style = {},
}) => {
    return (
        <div className={`custom-check-wrapper ${error ? "has-error" : ""} ${className}`} style={style}>
            <label className={`custom-check-label ${disabled ? "disabled" : ""}`}>
                <input type="checkbox" id={name} name={name} checked={value} onChange={onChange} className="custom-checkbox" disabled={disabled} />
                <span className="custom-checkbox-box" />
                <span className="custom-check-text">
                    {label}
                    {linkSection ? (
                        <Link to="/privacypolicy" className="custom-check-link">
                            {linkSection}
                        </Link>
                    ) : (
                        ""
                    )}
                    {linkText && (
                        <span 
                            className="custom-check-link" 
                            style={{ cursor: "pointer" }}
                            onClick={onLinkClick}
                        >
                            {linkText}
                        </span>
                    )}
                </span>
            </label>
            {error && <div className="custom-check-error">{error}</div>}
        </div>
    );
};

export default CustomCheck;
