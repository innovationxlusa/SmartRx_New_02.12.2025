import React from "react";
import "./StepIndicator.css";
import { IoCheckmarkSharp } from "react-icons/io5";

const StepIndicator = ({ steps: customSteps }) => {
    const defaultSteps = [
        { label: "Rx Profile", isActive: true },
        { label: "Online Profile", isActive: true },
        { label: "Doctor Fee", isActive: true },
        { label: "Doctor Waiting", isActive: false },
        { label: "Doctor Rating", isActive: false },
    ];

    const steps = customSteps || defaultSteps;

    return (
        <div className="main-step-container">
            <div className="indicator-container">
                {steps.map((step, index) => (
                    <div key={index} className="step-container">
                        <div className={`step-indicator ${step.isActive ? "active-step" : ""}`}>{step.isActive ? <IoCheckmarkSharp /> : <div className="inactive-circle"></div>}</div>
                        <span className={`step-text ${step.isActive ? "active-step-text" : ""}`}>{step.label}</span>
                        {index < steps.length - 1 && <div className={`line ${steps[index + 1].isActive ? "active-line" : ""}`} />}
                    </div>
                ))}
            </div>
        </div>
    );
};

export default StepIndicator;
