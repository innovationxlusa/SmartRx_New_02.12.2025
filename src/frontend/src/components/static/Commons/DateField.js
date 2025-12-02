import React, { useState, useEffect } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../PatientProfile/ProfileDetails.css";

const DateField = ({ styles, placeholderText, onChange, value }) => {
    const [date, setDate] = useState(null);

    useEffect(() => {
        setDate(value || null);
    }, [value]);

    const handleDateChange = (date) => {
        setDate(date);
        if (onChange) onChange(date);
    };

    const getFormattedDate = (date) => {
        if (!date) return "";
        const day = String(date.getDate()).padStart(2, "0");
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
    };

    return (
        <div>
            <DatePicker
                selected={date}
                onChange={handleDateChange}
                dateFormat="dd-MM-yyyy"
                className="form-control testDate"
                style={styles}
                placeholderText={placeholderText}
                showMonthDropdown
                showYearDropdown
                dropdownMode="select"
                portalId="root-portal"
                popperPlacement="bottom-start"
                popperClassName="datepicker-popper"
            />
        </div>
    );
};

export default DateField;
