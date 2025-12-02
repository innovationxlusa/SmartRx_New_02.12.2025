import React from "react";
import "./VitalRangeGraph.css";

const VitalRangeGraph = ({ valueTop, valueBottom, min = 35, max = 240, unit = "bpm", gradientColors = "linear-gradient(to right, #a5e85b, #ffcc33, #8a2be2)", step = 10 }) => {
    const getLeftPercent = (val) => `${((val - min) / (max - min)) * 100}%`;

    const rangeTicks = Array.from({ length: Math.floor((max - min) / step) + 1 }, (_, i) => min + i * step);

    return (
        <div className="vital-graph-wrapper">
            {/* Top value label and vertical line */}
            <div className="value-indicator" style={{ left: getLeftPercent(valueTop) }}>
                <div className="value-text">
                    {valueTop} {unit}
                </div>
                <div className="indicator-line" />
            </div>

            {/* Gradient bar and ticks */}
            <div className="vital-bar">
                <div className="gradient" style={{ background: gradientColors }} />

                <div className="tick-container top">
                    {rangeTicks.map((val) => (
                        <div key={`top-${val}`} className="tick-top" style={{ left: getLeftPercent(val) }}>
                            <span>{val}</span>
                        </div>
                    ))}
                </div>

                <div className="tick-container bottom">
                    {rangeTicks.map((val) => (
                        <div key={`bottom-${val}`} className="tick-bottom" style={{ left: getLeftPercent(val) }}>
                            {val}
                        </div>
                    ))}
                </div>
            </div>

            {/* Bottom value label and vertical line */}
            <div className="value-indicator bottom" style={{ left: getLeftPercent(valueBottom) }}>
                <div className="indicator-line" />
                <div className="value-text">
                    {valueBottom} {unit}
                </div>
            </div>
        </div>
    );
};

export default VitalRangeGraph;
