import React from 'react';
import './PatientViewList.css';
import { getRxColor, getRxText } from '../../utils/utils';
import { useParams, useNavigate } from "react-router-dom";

const PatientVeiwList = ({ patient }) => {
    const navigate = useNavigate();

    return (
        <div className="patient-list-item">
            <div className="patient-avatar">
                <img
                    src={patient.isWaiting ? '/img/PatientViewDefaultAvatar.svg' : patient.isUncategorized ? "/img/PatientViewDefaultAvatar2.svg" : ('/img/profilepic.png')}
                    alt={`${patient.name || patient.patientName || 'Patient'}`}
                    className="avatar-image"
                />
            </div>

            <div className="patient-details">
                {
                    patient.isWaitingBlank ? <span className="patient-row-title">Waiting For SmartRx</span> : patient.isUncategorizeeBlank ? <span className="patient-row-title">Un-Categorized</span> :

                        (
                            <>
                                <div className="patient-info">
                                    <span>Name:</span> <span>{patient.name || patient.patientName || 'Unknown'}</span>
                                </div>
                                <div className="patient-info">
                                    <span>Age:</span> <span>{patient.age || patient.patientAge || 'N/A'}</span>
                                </div>
                                <div className="patient-info">
                                    <span>Gender:</span> <span>{patient.gender || patient.patientGender || 'N/A'}</span>
                                </div>
                            </>
                        )
                }
            </div>

            <div className="patient-status">
                <div className="status-badge-container">
                    <div
                        style={{
                            borderRadius: "3px",
                            backgroundColor: getRxColor(patient.prescriptions.length > 0 ? patient.prescriptions[0] : patient),
                            display: "inline-block",
                            padding: "8px",
                            color: "white",
                            fontWeight: "700",
                            fontSize: "10px",
                            lineHeight: "96.3%",
                            width: "100%",
                            textAlign: "center",
                            letterSpacing: "0.5px",
                            textTransform: "uppercase",
                            minWidth: "70.25px"
                        }}
                    >
                        {getRxText(patient.prescriptions.length > 0 ? patient.prescriptions[0] : patient)}
                    </div>
                    <div role="button" className="total-count" onClick={() => {
                        
                        const rxText = getRxText(patient.prescriptions.length > 0 ? patient.prescriptions[0] : patient);
                        let type, prescriptionType;

                        if (rxText === "Smart RX") { type = "smart-prescription-list"; prescriptionType = "smartrx"; }
                        else if (rxText === "Waiting"||rxText==="Pending") { type = "pending-prescription-list"; prescriptionType = "waiting"; }
                        else { type = "rx-only-prescription-list"; prescriptionType = "uncategorized"; }

                        navigate(`/browse-rx/${type}`, { state: { patientId: patient.patientId, prescriptionType: prescriptionType, prescriptionOwner: patient.name || patient.patientName }, });
                    }}>
                        {patient.totalPrescriptions || patient.totalCount || 0} Total
                    </div>
                </div>

            </div>
        </div>
    );
};

export default PatientVeiwList;
