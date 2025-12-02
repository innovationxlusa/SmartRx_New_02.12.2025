import React from "react";

const Checkbox = ({ label, checked, onChange, customStyles, type }) => {
  return (
    <label className="custom-checkbox">
      <input
        type={type}
        checked={checked}
        onChange={onChange}
        className="checkbox-input"
        style={customStyles?.checkbox}
      />
      &nbsp; {label && <span style={{ fontSize: "14px" }}>{label}</span>}
    </label>
  );
};

export default Checkbox;
