import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const DateField = () => {
    const [date, setDate] = useState(null);

    return (
        <div>
            <DatePicker
                selected={date}
                onChange={(newDate) => setDate(newDate)}
                dateFormat="dd-MM-yyyy"
                className="custom-input"
                placeholderText={"DD-MM-YYYY"}
                showMonthDropdown
                showYearDropdown
                dropdownMode="select"
                isClearable={true}
                showIcon
            />
        </div>
    );
};

export default DateField;
