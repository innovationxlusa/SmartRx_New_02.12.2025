import React, { useState } from 'react'
import { FcInfo } from "react-icons/fc";

const ChiefComplaint = ({ smartRxInsiderData }) => {
    const [isTooltipVisible, setIsTooltipVisible] = useState(false);
    // Get complaint data from smartRxInsiderData or use default
    const getComplaintData = () => {
        // Check if smartRxInsiderData has chief complaints
        if (smartRxInsiderData?.prescriptions?.[0]?.chiefComplaints && Array.isArray(smartRxInsiderData?.prescriptions?.[0]?.chiefComplaints)) {
            return smartRxInsiderData?.prescriptions?.[0]?.chiefComplaints.map(complaint => ({
                name: "line",
                value: complaint.description
            }));
        }
        
        // Fallback to default data
        return [
            { name: "line", value: "No complaint found" },
        ];
    };
    const complaintData = getComplaintData();
    
    // Extract acronyms from smartRxInsiderData
    const getAcronymsFromData = () => {
        const allAcronyms = [];        
        // Check if smartRxInsiderData has chief complaints with acronyms
        if (smartRxInsiderData?.prescriptions?.[0]?.chiefComplaints && Array.isArray(smartRxInsiderData.prescriptions[0]?.chiefComplaints)) {
            smartRxInsiderData.prescriptions[0]?.chiefComplaints?.forEach(chiefComplaint => {                
                if (chiefComplaint.acronyms && Array.isArray(chiefComplaint.acronyms)) {
                    chiefComplaint.acronyms.forEach(acronym => {
                        if (acronym.abbreviation && acronym.elaboration) {
                            allAcronyms.push({
                                abbreviation: acronym.abbreviation,
                                elaboration: acronym.elaboration
                            });
                        }
                    });
                }
            }); 
        }
        
        // Remove duplicates based on abbreviation
        const uniqueAcronyms = allAcronyms.filter((acronym, index, self) => 
            index === self.findIndex(a => a.abbreviation === acronym.abbreviation)
        );
        
        return uniqueAcronyms;
    };

    const acronyms = getAcronymsFromData();

    // Event handlers for tooltip visibility
    const handleMouseEnter = () => {
        setIsTooltipVisible(true);
    };

    const handleMouseLeave = () => {
        setIsTooltipVisible(false);
    };

    const handleClick = () => {
        setIsTooltipVisible(!isTooltipVisible);
    };

    return (
        <div className="chComplaint pt-3 pb-3 gap-3 position-relative">
            {complaintData.map((entry, i) => (
                <div key={i} className="chComplaint-row">
                    <span className="chComplaint-value">
                        {entry.value}
                    </span>
                </div>
            ))}
            <div 
                className="info-box position-absolute" 
                tabIndex={0}
                //onMouseEnter={handleMouseEnter}
                //onMouseLeave={handleMouseLeave}
                onClick={handleClick}
                style={{ cursor: 'pointer' }}
            >
                <FcInfo />
                <span 
                    className="info-text"
                    style={{ cursor: 'pointer' }}
                >
                    Info
                </span>
                {isTooltipVisible && (
                    <span className="tooltip-text">
                        {acronyms.length > 0 ? (
                            <div>
                                <div className="fw-semibold mb-2">Elaboration Found</div>
                                {acronyms.map((acronym, index) => (
                                    <div key={index} className="mb-1">
                                        <span className="fw-bold text-primary">{acronym.abbreviation}:</span> {acronym.elaboration}
                                    </div>
                                ))}
                            </div>
                        ) : (
                            "No medical acronyms detected in chief complaints."
                        )}
                    </span>
                )}
            </div>

        </div>
    )
}

export default ChiefComplaint
