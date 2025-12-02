import React, { useRef, useState, useEffect, useMemo } from "react";
import AccordionShadow from "../static/AccordionShadow/AccordionShadow";
import StepIndicator from "../static/StepIndicator/StepIndicator";
import HorizontalScrollMenu from "../static/Menu/HorizontalScrollMenu";
import "./SmartRxInsider.css";
import Vitals from "./Vitals/Vitals";
import { classNames } from "@react-pdf-viewer/core";
import PageTitle from "../static/PageTitle/PageTitle";
import Medicine from "./Medicine/Medicine";
import useApiClients from "../../services/useApiClients";
import { useFetchData } from "../../hooks/useFetchData";
import { BLOOD_GROUPS, GENDER, getColorForName } from "../../constants/constants";
import { formatDateDDMMYYYY } from "../../utils/dateTimeFormatterUtils";
import Investigation from "./Investigation/Investigation";
import { useLocation, useNavigate } from "react-router-dom";
import SmartRxInsiderShimmer from "./SmartRxInsiderShimmer";
import Advice from "./Advice/Advice";
import ProfilePicture from "../PatientProfile/ProfilePicture";
import { get } from "react-scroll/modules/mixins/scroller";
import Favorites from "./Favorites/Favorites";
import { IMAGE_HOST } from "../../config/config";
import DoctorInformation from "./DoctorInformation/DoctorInformation";
import PatientInformation from "./PatientInformation/PatientInformation";
import ChiefComplaint from "./ChiefComplaint/ChiefComplaint";
import Overview from "./Overview/Overview";
import PatientHistory from "./PatientHistory/PatientHistory";
import CustomAccordion from "../static/CustomAccordion/CustomAccordion";
import StarRating from "../static/StarRating/StarRating";
import { BASE_URL } from "../../constants/apiEndpoints";

const IMAGE_URL = IMAGE_HOST;

const SmartRxInsider = () => {
    // Destructuring API service methods
    const { api } = useApiClients();
    const { state } = useLocation();
    const navigate = useNavigate();
    const fileId = state?.fileId;
    const smartRxMasterId = state?.smartRxMasterId;

    const {
        data: smartRxInsiderData = {},
        isLoading,
        error,
        refetch,
    } = useFetchData(
        api.getSmartRxInsiderByUserId,
        0, // page
        0, // rowsPerPage
        null, // sortField
        null, // sortOrder
        { SmartRxMasterId: smartRxMasterId }, // fileId custom param for payload
    );
    const fetchSmartRxVitalData = async ({ VitalName }) => {
        try {
            const response = await api.getVitalsByVitalName({ VitalName });
            return response; // Pass response back to modal
        } catch (err) {
            console.error("API call failed:", err);
            return null;
        }
    };

    // {Patient Profile API call}
    const patientId = smartRxInsiderData?.patientId;
    const fetchPatientData = async () => {
        try {
            const responsePatient = await api.getPatientDataById({
                patientId: patientId,
            }); // ✅ lowercase 'p'
            return responsePatient;
        } catch (err) {
            console.error("Patient Data API call failed:", err);
            return null;
        }
    };

    const patientInfoRows = [
        [
            {
                name: "Patient:",
                value: (d) => `${d?.firstName} ${d?.lastName}` ?? "N/A",
            },
            { name: "Age:", value: (d) => `${d?.age} years` ?? "N/A" },
        ],
        [
            { name: "Gender:", value: (d) => GENDER[d?.gender] ?? "N/A" },
            {
                name: "Blood Group:",
                value: (d) => BLOOD_GROUPS[d?.bloodGroup] ?? "N/A",
            },
        ],
        [
            {
                name: "Height:",
                value: (d) =>
                    d?.heightFeet != null || d?.heightInches != null
                        ? `${Number(d?.heightFeet || 0)} ft ${Number(d?.heightInches || 0)} in`
                        : "N/A",
            },

            {
                name: "Weight:",
                value: (d) => {
                    const w = Number(d?.weight);
                    return isNaN(w) || w <= 0 ? "N/A" : `${w} kg`;
                },
            },
        ],
    ];

    const doctorInfoItems = [
        {
            name: (
                <>
                    <span>Rx by </span>
                    <span style={{ color: "#65636e" }}>
                        {smartRxInsiderData?.prescriptions?.[0]?.doctor
                            ?.patientDoctor?.doctorFirstName +
                            " " +
                            smartRxInsiderData?.prescriptions?.[0]?.doctor
                                ?.patientDoctor?.doctorLastName}
                    </span>
                </>
            ),
            className: "rx",
        },

        {
            name: "Dated - ",
            value:
                smartRxInsiderData &&
                smartRxInsiderData.prescriptions &&
                smartRxInsiderData.prescriptions[0]?.completedDate
                    ? formatDateDDMMYYYY(
                        smartRxInsiderData.prescriptions[0].completedDate,
                    )
                    : "N/A",
            className: "dated",
        },
    ];

    const sectionRefs = {
        Overview: useRef(null),
        PatientInformation: useRef(null),
        ChiefComplaint: useRef(null),
        PatientHistory: useRef(null),
        DoctorsInformation: useRef(null),
        Vitals: useRef(null),
        Medicine: useRef(null),
        Investigation: useRef(null),
        Advice: useRef(null),
        Favorites: useRef(null),
    };

    const patientInfo = smartRxInsiderData?.patientInfo;

    const profile = patientInfo
        ? {
            ...patientInfo,
            picture: patientInfo?.profilePhotoPath
                ? `${IMAGE_URL}/${patientInfo?.profilePhotoPath?.replace(
                    /\\/g,
                    "/",
                )}`
                : null,
            profileProgress: patientInfo?.profileProgress || 0,
            fullName: `${patientInfo?.firstName} ${patientInfo?.lastName} ${patientInfo?.nickName || ""}`.trim(),
            nameInitials: [
                patientInfo?.FirstName?.[0] || patientInfo?.firstName?.[0],
                patientInfo?.LastName?.[0] || patientInfo?.lastName?.[0],
                patientInfo?.NickName?.[0] || patientInfo?.nickName?.[0],
            ]
                .filter(Boolean)
                .join("")
                .toUpperCase(),
        }
        : null;

    const colorForDefaultName = getColorForName(profile?.fullName);
    const coloredName = profile?.nameInitials.substring(0, 2).toUpperCase(); // Default to Indigo if no name
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
            fullName: `${docInfo?.doctorFirstName} ${docInfo?.doctorLastName}`.trim(),
            nameInitials: [
                docInfo?.DoctorFirstName?.[0] || docInfo?.doctorFirstName?.[0],
                docInfo?.DoctorLastName?.[0] || docInfo?.doctorLastName?.[0]
            ]
                .filter(Boolean)
                .join("")
                .toUpperCase(),
        }
        : null;
    const getInitialsDoctor = (fullName) =>
        fullName
            .replace(/\b(Dr\.?|Prof\.?)\s*/gi, "")
            .split(/\s+/)
            .map((w) => w[0])
            .join("")
            .toUpperCase();
    const coloredDoctorName = getInitialsDoctor(doctorProfile?.fullName).substring(0, 2).toUpperCase();
    const colorForDefaultDoctorName = getColorForName(doctorProfile?.fullName);

    const patientHistories = useMemo(() => {
        const histories = smartRxInsiderData?.prescriptions?.[0]?.histories;
        if (!Array.isArray(histories)) {
            return [];
        }

        return histories
            .filter((history) => history && (history.title || history.description || history.note))
            .map((history, index) => ({
                id: history?.id ?? history?.historyId ?? `history-${index}`,
                title: history?.title ?? "",
                description: history?.description ?? history?.note ?? "",
            }));
    }, [smartRxInsiderData]);





    const [imageError, setImageError] = useState(false);
    const hasValidPicture = profile?.picture && !imageError;

    const scrollToSection = (key) => {
        const target = sectionRefs[key]?.current;
        if (target) {
            target.scrollIntoView({
                behavior: "smooth",
                block: "start",
                inline: "nearest",
            });
        }
    };



    return (
        <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto px-md-4 pb-4">
            {isLoading ? (
                <SmartRxInsiderShimmer />
            ) : (
                <div>
                    <div className="content-container">
                        <div className="smartRx-header">
                            {/* Keep same Bootstrap column layout inside */}
                            <div className="row px-2 px-sm-5 px-md-5 px-lg-5 px-xl-0">
                                <div className="page-title">
                                    <PageTitle switchButton={false} pageName={"Smart Rx - Insider"} />
                                </div>
                                <div className="docGeneral mt-1 mb-1">
                                    {doctorInfoItems?.map((item, index) => (
                                        <div key={item.name} className="docGeneralInfo-item">
                                            <span
                                                className={`docGeneralData-name ${item.className} `}
                                            >
                                                <b>{item.name}</b>
                                            </span>
                                            <span
                                                className={`docGeneralData-value ${index === 0
                                                    ? "rx rxval"
                                                    : "text-[14px] fw-normal"
                                                    }`}
                                            >
                                                {item.value}
                                            </span>
                                        </div>
                                    ))}
                                </div>
                                <div className="horizontal-menu mt-1" style={{ boxShadow: "0 3px 4px -3px rgba(0, 0, 0, 0.1)" }}>
                                    <HorizontalScrollMenu data={sectionRefs} scrollToSection={scrollToSection} />
                                </div>
                            </div>
                        </div>
                        {/* Spacer to offset fixed header */}
                        <div style={{ height: "160px" }} />
                        <div className="generalInfo mt-1">
                            {patientInfoRows.map((row, rowIndex) => (
                                <div key={rowIndex} className="generalInfo-row">
                                    {row.map((item) => (
                                        <div key={item.name} className="generalInfo-item">
                                            <span className="generalData-name">
                                                <b>{item.name}</b>
                                            </span>
                                            <span className="generalData-value">
                                                {item.value(
                                                    smartRxInsiderData?.patientInfo,
                                                )}
                                            </span>
                                        </div>
                                    ))}
                                </div>
                            ))}
                        </div>
                        <div ref={sectionRefs.Overview} className="mt-2" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Overview"}>
                                <Overview smartRxInsiderData={smartRxInsiderData} fileId={fileId} refetch={refetch} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.PatientInformation} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Patient Information"}>
                                <PatientInformation smartRxInsiderData={smartRxInsiderData} getColorForName={getColorForName} hasValidPicture={hasValidPicture} profile={profile} fetchPatientData={fetchPatientData} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.ChiefComplaint} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Chief Complaint"}>
                                <ChiefComplaint smartRxInsiderData={smartRxInsiderData} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.PatientHistory} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Patient History"}>
                                <PatientHistory histories={patientHistories} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.DoctorsInformation} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Doctor's Information"}>
                                <DoctorInformation smartRxInsiderData={smartRxInsiderData} getColorForName={getColorForName} hasValidPicture={hasValidPicture} fileId={fileId} refetch={refetch} profile={profile} fetchPatientData={fetchPatientData} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.Vitals} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Vitals"}>
                                <Vitals smartRxInsiderData={smartRxInsiderData} fetchSmartRxVitalData={fetchSmartRxVitalData} smartRxMasterId={smartRxMasterId} refetch={refetch} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.Medicine} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Medicine"}>
                                <Medicine smartRxInsiderData={smartRxInsiderData} smartRxInsiderDataRefetch={refetch} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.Investigation} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Investigation"}>
                                <Investigation smartRxInsiderData={smartRxInsiderData} smartRxInsiderDataRefetch={refetch} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.Advice} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Advice"}>
                                <Advice smartRxInsiderData={smartRxInsiderData} />
                            </CustomAccordion>
                        </div>
                        <div ref={sectionRefs.Favorites} className="mt-3" style={{ scrollMarginTop: "250px" }}>
                            <CustomAccordion defaultOpen={true} accordionHeader={"Favorites"}>
                                <Favorites smartRxInsiderData={smartRxInsiderData} />
                            </CustomAccordion>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
};

export default SmartRxInsider;
