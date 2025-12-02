import React, { useState } from 'react'
import DoctorReviewModal from "./DoctorReviewModal";
import StarRating from "../Investigation/StarRating";
import CustomAccordion from '../../static/CustomAccordion/CustomAccordion';
import { BASE_URL } from "../../../constants/apiEndpoints";
import { useNavigate } from "react-router-dom";

const DoctorInformation = ({ smartRxInsiderData, getColorForName, hasValidPicture, fileId, refetch, profile, fetchPatientData }) => {
    const [modalType, setModalType] = useState(null);
    const [selectedFolder, setSelectedFolder] = useState(null);
    const smartRxMasterId = smartRxInsiderData?.smartRxId;
    const navigate = useNavigate();

    const getInitialsDoctor = (fullName) =>
        fullName
            .replace(/\b(Dr\.?|Prof\.?|Professor)\b\.?\s*/gi, "") // <-- added \.? after word
            .trim()
            .split(/\s+/)
            .map((w) => w[0])
            .join("")
            .toUpperCase();

    const smartRxDoctorInfo = smartRxInsiderData?.prescriptions?.[0]?.doctor || {};
    const docInfo = smartRxInsiderData?.prescriptions?.[0]?.doctor?.patientDoctor || {};
    const doctorChamber = smartRxInsiderData?.prescriptions?.[0].doctor?.chambers || [];
    const doctorEducations = smartRxInsiderData?.prescriptions?.[0].doctor?.doctorEducations || [];

    const doctorProfile = docInfo
        ? {
            ...docInfo,
            picture: docInfo?.profilePhotoPath
                ? `${BASE_URL}${docInfo?.profilePhotoPath?.replace(
                    /\\/g,
                    "/"
                )}`
                : null,
            fullName: `${docInfo?.doctorTitle} ${docInfo?.doctorFirstName} ${docInfo?.doctorLastName}`.trim(),
            nameInitials: [
                docInfo?.DoctorFirstName?.[0] || docInfo?.doctorFirstName?.[0],
                docInfo?.DoctorLastName?.[0] || docInfo?.doctorLastName?.[0]
            ]
                .filter(Boolean)
                .join("")
                .toUpperCase(),
        }
        : null;
    
    const coloredDoctorName = getInitialsDoctor(doctorProfile?.fullName).substring(0, 2).toUpperCase();
    const colorForDefaultDoctorName = getColorForName(doctorProfile?.fullName);
    
    /* ───────── Modal helpers ───────── */
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    return (
        <>
            <div className="col-12 ptInfo">
                <div className="col-8 pt-3 pb-3">
                    <span>
                        {docInfo?.doctorTitle} {docInfo.doctorFirstName}{" "}
                        {docInfo.doctorLastName}
                    </span>
                    <br></br>
                    <span>
                        {doctorEducations
                            ?.map((entry) => entry.educationDegreeName)
                            .filter(Boolean) // removes null/undefined
                            .join(", ")}
                    </span>
                    <span>
                        {doctorChamber
                            ?.filter((entry) => entry.isMainChamber)
                            .map(
                                (entry) =>
                                    `${entry.doctorSpecialization || ""} Specialist,${entry.doctorDesignationInChamber || ""}, Department of ${entry.departmentName || ""},${entry.hospitalName || ""},${entry.chamberCityName || ""}`,
                            )
                            .filter(Boolean)
                            .map((text, idx) => (
                                <div key={idx}>
                                    {text.split(",").map((part, i) => (
                                        <div key={i}>{part}</div>
                                    ))}
                                </div>
                            ))}
                    </span>
                    <span>
                        BMDC Reg No.: {docInfo.doctorBMDCRegNo}{" "}
                    </span>
                    <br></br>
                    <div className="col-12 d-flex align-items-center">
                        <div className="col-3">
                            <span>
                                {smartRxDoctorInfo.doctorRating}
                            </span>
                        </div>

                        <div className="col-9">
                            <StarRating
                                rating={smartRxDoctorInfo.doctorRating}
                                size={16}
                                readOnly={true}
                            />
                        </div>
                    </div>
                </div>

                <div className="col-4 d-flex justify-content-center align-items-center flex-column">
                    <div className="circle-wrapper">
                        <div className="profile-inner">
                            {doctorProfile?.picture ? (
                                <img
                                    src={doctorProfile?.picture}
                                    alt="Profile"
                                    className="profile-picture"
                                    onError={() => setImageError(true)}
                                />
                            ) : (
                                <div
                                    className="circle-inner"
                                    style={{
                                        color: colorForDefaultDoctorName,
                                    }}
                                >
                                    {coloredDoctorName && (
                                        <span
                                            className="initials"
                                            style={{
                                                color: colorForDefaultDoctorName,
                                            }}
                                        >
                                            {coloredDoctorName}
                                        </span>
                                    )}
                                </div>
                            )}
                        </div>
                    </div>

                    <button
                        className="profile-button"
                        onClick={() => {
                            navigate("/doctorProfile", {
                                state: { doctorId: smartRxInsiderData?.prescriptions?.[0]?.doctor?.doctorId },
                            });
                        }}
                    >
                        View Profile
                    </button>
                </div>
            </div>
            <div className="text-center">
                <button
                    className="add-button"
                    onClick={() => setModalType("add")}
                >
                    Add
                </button>
            </div>

            <div className="chamber-details-container mt-3">
                <CustomAccordion
                    className="doc-chamber-container"
                    accordionHeader={
                        <div
                            style={{
                                marginLeft: "8px",
                                fontSize: "20px",
                            }}
                        >
                            Chamber Details
                        </div>
                    }
                    background="#ffffff"
                    border="1px solid #D9D9D9"
                    borderRadius="4px"
                    shadow={false}
                    defaultOpen={true}
                    iconStyleOverride={{
                        marginRight: "20px",
                        marginTop: "-8px",
                    }}
                >
                    <div className="chamber-accordion-body-content">
                        {smartRxInsiderData?.prescriptions?.[0]?.doctor?.chambers
                            ?.filter((chamber) => chamber.isActive)
                            ?.sort(
                                (a, b) =>
                                    (b.isMainChamber ? 1 : 0) -
                                    (a.isMainChamber ? 1 : 0),
                            )
                            ?.map((chamber, index) => (
                                <div
                                    key={index}
                                    className="chamber-item mb-3"
                                >
                                    <div
                                        className="chamber-hospital"
                                        style={{
                                            fontWeight: "bold",
                                            fontSize: "16px",
                                        }}
                                    >
                                        {chamber.hospitalName}
                                    </div>
                                    <div className="chamber-specialization">
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                            }}
                                        >
                                            Specialization:
                                        </span>{" "}
                                        {chamber.doctorSpecialization ||
                                            "N/A"}
                                    </div>

                                    <div className="chamber-department">
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                            }}
                                        >
                                            Department:
                                        </span>{" "}
                                        {chamber.departmentName ||
                                            "N/A"}
                                    </div>
                                    <div className="chamber-address">
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                            }}
                                        >
                                            Address:
                                        </span>{" "}
                                        {chamber.chamberAddress}
                                    </div>
                                    <div className="chamber-booking-number">
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                            }}
                                        >
                                            For Serial:
                                        </span>{" "}
                                        {
                                            chamber.chamberDoctorBookingMobileNos
                                        }
                                    </div>
                                    <div className="chamber-days">
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                            }}
                                        >
                                            Visiting Days:
                                        </span>{" "}
                                        {chamber.doctorVisitingDaysInChamber
                                            ? chamber.doctorVisitingDaysInChamber
                                                .split(",")
                                                .join(", ")
                                            : "N/A"}
                                    </div>

                                    <div className="chamber-hour">
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                            }}
                                        >
                                            Visiting time:
                                        </span>{" "}
                                        {chamber.chamberVisitingHour}
                                    </div>
                                </div>
                            ))}
                    </div>
                </CustomAccordion>
            </div>
            <DoctorReviewModal
                modalType={modalType}
                isOpen={modalType === "add" || modalType === "edit"}
                folderData={selectedFolder}
                onClose={closeModal}
                anotherButton={"true"}
                smartRxMasterId={smartRxMasterId}
                prescriptionId={
                    smartRxInsiderData?.prescriptions?.[0]
                        ?.prescriptionId
                }
                doctorId={
                    smartRxInsiderData?.prescriptions?.[0]?.doctor
                        ?.doctorId
                }
                refetch={refetch}
                smartRxOverview={smartRxInsiderData?.smartRxOverview}
                doctorRating={smartRxDoctorInfo?.doctorRating}
                doctorComments={smartRxDoctorInfo?.doctorComments || smartRxDoctorInfo?.comments}
            />
        </>
    )
}

export default DoctorInformation
