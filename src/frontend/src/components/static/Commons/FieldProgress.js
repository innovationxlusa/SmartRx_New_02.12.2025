import React, { useState, useEffect } from "react";
import "../../PatientProfile/ProfileDetails.css";

const RangeSlider = React.forwardRef(
    (
        {
            customStyles,
            onValueChange,
            value: initialValue = 0,
            maxValue = 300,
            profile,
            readOnly = false,
        },
        ref,
    ) => {
        const [sliderValue, setSliderValue] = useState(initialValue);
        const min = 0;
        const max = maxValue;

        useEffect(() => {
            setSliderValue(initialValue);
        }, [initialValue]);

        const handleChange = (event) => {
            const newValue = parseInt(event.target.value);
            setSliderValue(newValue);
            if (onValueChange) {
                onValueChange(newValue);
            }
        };

        const filledPercentage = ((sliderValue - min) / (max - min)) * 100;

        return (
            <div className="range-container">
                <div className="slider-wrapper">
                    <div
                        className="slider-value"
                        style={{
                            left: `${filledPercentage}%`,
                            transform: "translateX(-50%) translateY(-70%)",
                        }}
                    >
                        {sliderValue}
                    </div>

                    <input
                        ref={ref}
                        type="range"
                        min={min}
                        max={max}
                        value={sliderValue}
                        onChange={handleChange}
                        step="1"
                        className="custom-range"
                        disabled={readOnly}
                        style={{
                            background: `linear-gradient(90deg, rgb(74, 39, 119) ${filledPercentage}%, rgb(213, 216, 216) ${filledPercentage}%)`,
                            ...customStyles,
                        }}
                    />
                </div>
                <div className="range-labels">
                    <span
                        className="left-label"
                        style={{ marginBottom: "10px" }}
                    >
                        {min}
                    </span>
                    <span
                        className="right-label"
                        style={{ marginBottom: "10px" }}
                    >
                        {max}
                    </span>
                </div>
            </div>
        );
    },
);

RangeSlider.displayName = 'FieldProgress';

export default RangeSlider;