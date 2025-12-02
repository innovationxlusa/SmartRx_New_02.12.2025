import React from "react";

const PatientHistory = ({ histories = [] }) => {
    if (!histories.length) {
        return (
            <div className="ptHistory pt-3 pb-3">
                <div className="ptHistory-row">
                    <span className="ptHistory-value">No patient history recorded.</span>
                </div>
            </div>
        );
    }

    return (
        <div className="ptHistory pt-3 pb-3">
            {histories.map((history, index) => {
                const historyText =
                    [history.title, history.description].filter(Boolean).join(" ").trim();

                return (
                    <div key={history.id ?? index} className="ptHistory-row">
                        <span className="ptHistory-value">
                            {historyText || "N/A"}
                        </span>
                    </div>
                );
            })}
        </div>
    );
};

export default PatientHistory;
