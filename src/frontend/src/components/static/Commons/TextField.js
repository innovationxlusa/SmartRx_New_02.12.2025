import React from "react";
import "../../PatientProfile/ProfileDetails.css";

const TextField = ({
    customStyles,
    Placeholder,
    type,
    onChange,
    onKeyDown,
    value,
}) => {
    return (
        <div>
            <input
                type={type}
                className="form-control"
                style={customStyles?.input}
                placeholder={Placeholder}
                onChange={onChange}
                onKeyDown={onKeyDown}
                value={value}
            />
        </div>
    );
};

export default TextField;
