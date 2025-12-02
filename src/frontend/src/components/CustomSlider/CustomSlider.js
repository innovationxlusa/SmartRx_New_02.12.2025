import React from "react";
import "./CustomSlider.css";

const CustomSlider = ({ value, min, max, step, onChange }) => {
    const percentage = ((value - min) / (max - min)) * 100;

    // Generate tick marks
    const ticks = [];
    for (let i = min; i <= max; i += step) {
        ticks.push(i);
    }

    return (
        <div className="custom-slider-container">
            {/* Value popup */}
            <div className="slider-value" style={{ left: `${percentage}%` }}>
                {value}°
            </div>

            {/* Track with ticks */}
            <div className="slider-track">
                {ticks.map((tick, idx) => (
                    <div key={idx} className="tick" />
                ))}
                {/* Green indicator line */}
                <div className="indicator" style={{ left: `${percentage}%` }} />
            </div>

            {/* Hidden range input for interaction */}
            <input type="range" min={min} max={max} step={step} value={value} onChange={(e) => onChange(parseInt(e.target.value))} className="hidden-range" />
        </div>
    );
};

export default CustomSlider;
