import React, { useState, useEffect } from 'react'
import { BLOOD_GROUPS, GENDER } from "../../../constants/constants";
import FieldProgress from "../../static/Commons/FieldProgress";
import { useNavigate } from "react-router-dom";

const PatientInformation = ({ smartRxInsiderData, profile, getColorForName, fetchPatientData }) => {
    const [imageError, setImageError] = useState(false);
    const hasValidPicture = profile?.picture && !imageError;
    const navigate = useNavigate();
    
    useEffect(() => {
        setImageError(false);
    }, [profile?.picture]);

    const ptInfo = [
        [
            {
                name: "Patient",
                value: (d) => `${d?.firstName} ${d?.lastName}` ?? "N/A",
            },
            { name: "Gender", value: (d) => GENDER[d?.gender] ?? "N/A" },
            { name: "Age", value: (d) => `${d?.age} years` ?? "N/A" },
        ],
    ];

    const colorForDefaultName = getColorForName(profile?.fullName);
    const coloredName = profile?.nameInitials.substring(0, 2).toUpperCase(); // Default to Indigo if no name


    return (
        <>
            <div className="col-8 ptInfo">
                <div className="col-10 pt-3 pb-3">
                    {ptInfo.map((entry, i) => (
                        <div key={i} className="ptInfo-row">
                            {entry.map((item) => (
                                <div key={item.name} className="ptInfo-item">
                                    <span className="data-name">
                                        {item.name}
                                    </span>
                                    <span className="data-mid">:</span>
                                    <span className="data-value">
                                        {item.value(
                                            smartRxInsiderData?.patientInfo,
                                        )}
                                    </span>
                                </div>
                            ))}
                        </div>
                    ))}
                </div>

                <div className="col-2">
                    <div className="profile-container">
                        <div className="circle-wrapper">
                            <div className="profile-inner">
                                {hasValidPicture ? (
                                    <img
                                        key={profile?.picture}
                                        src={`${profile?.picture?.startsWith('http') ? profile?.picture : `${process.env.REACT_APP_IMAGE_URL}/${profile?.picture}`}?t=${Date.now()}`}
                                        alt="Profile"
                                        onError={() => {
                                            setImageError(true);
                                        }}
                                        onLoad={() => {
                                        }}
                                    />
                                ) : (
                                    <div
                                        className="circle-inner"
                                        style={{ color: colorForDefaultName }}
                                    >
                                        {coloredName && (
                                            <span
                                                className="initials"
                                                style={{
                                                    color: colorForDefaultName,
                                                }}
                                            >
                                                {coloredName}
                                            </span>
                                        )}
                                    </div>
                                )}
                            </div>
                        </div>

                        <button
                            className="profile-button"
                            onClick={async () => {
                                const patientData = await fetchPatientData({
                                    PatientId: smartRxInsiderData?.patientId,
                                });

                                if (patientData) {
                                    const fullName = `${smartRxInsiderData?.patientInfo?.firstName || ""} ${smartRxInsiderData?.patientInfo?.lastName || ""}`;
                                    const initials = fullName
                                        .split(" ")
                                        .map((n) => n[0] || "")
                                        .join("")
                                        .substring(0, 2)
                                        .toUpperCase();

                                    const colorForInitials =
                                        getColorForName(fullName);

                                    navigate("/profileDetails", {
                                        state: {
                                            patientData,
                                            coloredName: initials,
                                            colorForDefaultName:
                                                colorForInitials,
                                        },
                                        refetch: patientData,
                                    });
                                }
                            }}
                        >
                            View Profile
                        </button>
                    </div>
                </div>
            </div>
            <div
                className="pt-info-progress-bar-wrapper"
                style={{
                    marginTop: "4px",
                    marginBottom: "2px",
                }}
            >
                <span
                    style={{
                        fontSize: "14px",
                        color: "#65636e",
                        fontWeight: "bold",
                    }}
                >
                    Completed
                </span>
                <span>
                    <FieldProgress
                        customStyles={{
                            background:
                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                            borderRadius: "2px",
                            height: "5%",
                            width: "100%",
                            pointerEvents: "none",
                        }}
                        profile=""
                        value={`${profile?.profileProgress || 0}`}
                        min="0"
                        maxValue="100"
                        onValueChange={null}
                    />
                </span>
            </div>
        </>
    );
}

export default PatientInformation
