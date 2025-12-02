import React from "react";
import { FaChevronDown } from "react-icons/fa"; // Import a dropdown icon
import "../../Profile/ProfileDetails.css";

const SelectField = ({ customStyles, Placeholder, options, onChange, name, value, icon }) => {
    return (
        <div className="select-container" style={customStyles?.container}>
            {icon && <span className="select-icon">{icon}</span>}
            <select className="custom-select" style={customStyles?.select} name={name} value={value} placeholder={Placeholder} onChange={onChange}>
                {options?.map((option, index) => (
                    <option key={index} value={option.value} style={customStyles?.option}>
                        {option.label}
                    </option>
                ))}
            </select>
        </div>
    );
};

export default SelectField;
