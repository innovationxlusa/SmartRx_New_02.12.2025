import React, { useId } from "react";
import "./NormalViewToggle.css";

const NormalViewToggle = ({ isPatientView, onToggle }) => {
    const toggleId = useId();
    
    return (
        <>
            <div className="toggle-wrapper">
                <input 
                    className="toggle-input" 
                    type="checkbox" 
                    id={toggleId}
                    checked={isPatientView}
                    onChange={onToggle}
                />
                <label className="toggle-container" htmlFor={toggleId}>
                    <div className="toggle-circle">
                        <div className="pin-icon">
                            <span className="pin-top"></span>
                            <span className="pin-middle"></span>
                            <span className="pin-bottom"></span>
                        </div>
                    </div>
                    <div className="d-flex justify-content-between label-container">
                        <div className="label-left">Patient View</div>
                        <div className="label-right">Normal View</div>
                    </div>
                </label>
            </div>
        </>
    );
};

export default NormalViewToggle;
