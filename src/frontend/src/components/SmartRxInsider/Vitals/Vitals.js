import "./Vitals.css";
import { useState } from "react";
import VitalsCard from "./VitalsCard.js";
import { VitalsData } from "./VitalsConfig.js";
import { getGraphRange, convertDecimalHeightToFeetInches} from "../../../utils/utils.js";
import { useFetchData } from "../../../hooks/useFetchData.js";
import useApiClients from "../../../services/useApiClients.js";
import CustomButton from "../../static/Commons/CustomButton.js";
import VitalManagementModal from "./VitalManagementModal.js";
import { useUserContext } from "../../../contexts/UserContext.js";
import PulseRateGraph from "../../static/Graph/PulseRateGraph.js";
import { ReactComponent as Age } from "../../../assets/img/Age.svg";
import { ReactComponent as Gender } from "../../../assets/img/Gender.svg";
import { ReactComponent as Height } from "../../../assets/img/Height.svg";
import { ReactComponent as Weight } from "../../../assets/img/Weight.svg";
import { FaEdit } from "react-icons/fa";
import { GENDER, VITALS_OBSERVATION } from "../../../constants/constants.js";
import CustomAccordion from "../../static/CustomAccordion/CustomAccordion.js";
import {
    formatDateYYYYMMDD,
    formatTime12Hour,
} from "../../../utils/dateTimeFormatterUtils.js";
import VitalFaqModal from "./VitalFaqModal.js";
import EditNewModal from "./EditNewModal.js";
import HorizontalRangeSlider from "../../static/HorizontalSlider/HorizontalRangeSlider.js";
import CustomModal from "../../static/CustomModal/CustomModal.js";
import CustomInput from "../../static/Commons/CustomInput.js";

const Vitals = ({
    smartRxInsiderData,
    fetchSmartRxVitalData,
    smartRxMasterId,
    refetch,
    anotherButton,
}) => {
    /* ───────────────────────
       ───── Local state ─────
       ─────────────────────── */
    const [modalType, setModalType] = useState(null);
    const [selectedFolder, setSelectedFolder] = useState(null);
    const [selectedVitalData, setSelectedVitalData] = useState(null);
    const [selectedItem, setSelectedItem] = useState(null);

    const [toDeleteVitalId, setToDeleteVitalId] = useState(null);

    const [isLoading, setIsLoading] = useState(false);

    const { user } = useUserContext();
    const { api } = useApiClients(); // Destructure API service methods

    /* ───────── Modal helpers ───────── */
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    const handleOpenModal = (vitalData, type) => {
        setSelectedFolder(vitalData);
        setSelectedVitalData(vitalData);
        openModal(type);
    };

    const handleOpenModalEdit = (item, type) => {
        setSelectedItem(item);
        setModalType(type);
    };

    const patientInfoItems = [
        {
            label: "Gender",
            icon: <Gender alt="Gender" />,
            value: (d) => GENDER[d?.gender],
            measurementUnit: "",
        },
        {
            label: "Age",
            icon: <Age alt="Age" />,
            value: (d) => d?.age,
            measurementUnit: "Yrs",
        },
        {
            label: "Height",
            icon: <Height alt="Height" />,
            value: (d) => {
                const h = parseFloat(d?.height);
                const height = Number.isInteger(h)
                    ? h.toFixed(0)
                    : h.toFixed(2);
                if (isNaN(height)) return "N/A";
                const feetInch = convertDecimalHeightToFeetInches(height);
                const formattedHeight = `${d?.heightFeet}'${d?.heightInch}"`;

                return formattedHeight;
            },
            measurementUnit: "",
        },
        {
            label: "Weight",
            icon: <Weight alt="Weight" />,
            value: (d) => {
                const value = parseFloat(d?.weight);
                if (isNaN(value)) return "N/A";
                // Return integer values without decimal places, otherwise show 2 decimal places
                return Number.isInteger(value)
                    ? value.toFixed(0)
                    : value.toFixed(2);
            },
            measurementUnit: "Kg",
        },
    ];

    const parseStandardRange = (standard, name, entity) => {
        if (!standard || typeof standard !== "string") return {};

        if (name.toLowerCase() === "blood pressure") {
            // Only extract if it's systolic
            const [systolic, diastolic] = standard.split("/");
            if (entity === "Systolic") {
                return {
                    low: Number(diastolic),
                    high: Number(systolic),
                };
            } else {
                return {};
            }
        }

        // For other vitals:
        const [low, high] = standard.split("-").map(Number);
        return {
            low: !isNaN(low) ? low : undefined,
            high: !isNaN(high) ? high : undefined,
        };
    };

    /* ───────── Dynamic‑static merge helpers ───────── */
    const ALIAS = { bmi: "bodymassindex" }; // “BMI” → “Body Mass Index”
    const normalize = (str) => str.toLowerCase().replace(/[^a-z0-9]/g, "");

    /** Enrich dynamic vitals with icons / descriptions from the static config */
    const enrichVitals = (dynamicVitals = []) => {
        const staticMap = VitalsData.reduce((acc, item) => {
            acc[normalize(item.title)] = item;
            return acc;
        }, {});

        return dynamicVitals.map((dyn) => {
            if (!dyn || !dyn.name) return dyn; // Skip if no name

            const key = normalize(dyn.name);
            const matchedStatic =
                staticMap[key] || (ALIAS[key] && staticMap[ALIAS[key]]) || null;

            return matchedStatic
                ? {
                      ...dyn,
                      description2: matchedStatic.description,
                      icon: matchedStatic.icon,
                      rangeImage: matchedStatic.rangeImage,
                  }
                : dyn;
        });
    };

    const mergedVitalData = enrichVitals(
        smartRxInsiderData?.prescriptions?.[0]?.vitals ?? [],
    );

    const min = 0;
    const max = 200;
    const {
        data: vitalFAQsData,
        isLoading: isVitalFAQLoading,
        error: vitalFAQError,
        refetch: faqRefetch,
    } = useFetchData(
        modalType === "faq" ? api.getSmartRxInsiderVitalFAQByVitalId : null,
        modalType === "faq" && selectedVitalData ? 0 : null,
        modalType === "faq" && selectedVitalData ? 0 : null,
        null,
        null,
        modalType === "faq" ? selectedVitalData.vitalId : null,
    );

    /* ─────────────────────────────────────────
       ─────── JSX ────────────────────────────
       ───────────────────────────────────────── */
    return (
        <div className="vitals-container">
            {/* ───── Basic patient info ───── */}
            <div className="vital-basic-info">
                {patientInfoItems.map((item, index) => {
                    const patientValue = item.value(
                        smartRxInsiderData?.patientInfo,
                    );
                    if (!patientValue) return null;

                    const isBasicInline = [
                        "gender",
                        "age",
                        "height",
                        "weight",
                    ].includes(item.label.toLowerCase());

                    if (isBasicInline) {
                        return (
                            <div className="vital-info-item" key={`patient-info-${index}-${item.label}`}>
                                <div className="col-6 vitals-item">
                                    <span className="vital-info-item-icon">
                                        {item.icon}
                                    </span>
                                    <span className="vital-info-item-header">
                                        {item.label}
                                    </span>
                                </div>
                                <span className="vital-info-item-data col-3">
                                    {patientValue}&nbsp;
                                    {patientValue == null ||
                                    patientValue === "N/A"
                                        ? ""
                                        : item.measurementUnit}
                                </span>

                                <span
                                    className="col-3"
                                    style={{
                                        cursor: "pointer",
                                        display: "flex",
                                        textAlign: "left",
                                        alignItems: "left",
                                    }}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        e.nativeEvent.stopImmediatePropagation();
                                        handleOpenModalEdit(item, "editNew");
                                    }}
                                >
                                    <FaEdit />
                                </span>
                            </div>
                        );
                    }

                    // Optionally handle mergedVitalData outside this loop for performance.
                    return null;
                })}

                {/* Add‑vitals button */}

                <div className="mt-4 mb-5">
                    <CustomButton
                        type="button"
                        label="Add Vitals"
                        className="investigation-action-btn mt-2"
                        width="clamp(250px, 30vw, 450px)"
                        height="clamp(42px, 2.3vw, 40px)"
                        textColor="var(--theme-font-color)"
                        backgroundColor="#FAF8FA"
                        borderRadius="3px"
                        shape="Square"
                        borderColor="1px solid var(--theme-font-color)"
                        labelStyle={{
                            fontSize: "clamp(14px, 2vw, 16px)",
                            fontWeight: "100",
                            textTransform: "capitalize",
                        }}
                        hoverEffect="theme"
                        onClick={(e) => {
                            e.stopPropagation();
                            e.nativeEvent.stopImmediatePropagation();
                            handleOpenModal(null, "add");
                        }}
                    />
                </div>
            </div>

            {mergedVitalData
                .filter(
                    (v) =>
                        (v.title ?? v.name)?.toLowerCase() !==
                            "blood pressure" ||
                        v.applicableEntity === "Systolic",
                )
                .map((v, vitalIndex) => {
                    if (
                        !v ||
                        !v.status ||
                        v.name.toLowerCase() === "Height".toLowerCase() ||
                        v.name.toLowerCase() === "Weight".toLowerCase()
                    )
                        return null;

                    const isBloodPressure =
                        (v.title ?? v.name)?.toLowerCase() === "blood pressure";
                    const isSystolic = v.applicableEntity === "Systolic";

                    const diastolicEntry = isBloodPressure
                        ? mergedVitalData.find(
                              (d) =>
                                  (d.title ?? d.name)?.toLowerCase() ===
                                      "blood pressure" &&
                                  d.applicableEntity === "Diastolic" &&
                                  d.prescriptionId === v.prescriptionId,
                          )
                        : null;

                    const { min, max } = getGraphRange(v.name);
                    return (
                        <CustomAccordion
                            key={`vital-${vitalIndex}-${v.vitalId}-${v.prescriptionId}-${v.applicableEntity || 'default'}`}
                            background="#ffffff"
                            border="1px solid #D9D9D9"
                            borderRadius="4px"
                            shadow={false}
                            defaultOpen={false}
                            accordionHeaderData={
                                <div className="vital-card-header">
                                    <div className="d-flex justify-content-between align-items-start">
                                        {/* Left side */}
                                        <div className="d-flex justify-content-between align-items-start">
                                            <div className="me-2 pe-1">
                                                {v.icon}
                                            </div>
                                            <div>
                                                <div className="vital-card-header">
                                                    {v.name}
                                                </div>
                                                <div className="vital-card-header">
                                                    {v.vitalValueString}
                                                    {v.measurementUnit}
                                                </div>
                                            </div>
                                        </div>

                                        {/* Right side */}
                                        <div>
                                            <div className="vital-card-date">
                                                {v.date ??
                                                    `${formatDateYYYYMMDD(
                                                        smartRxInsiderData?.prescriptionDate,
                                                    )} at ${formatTime12Hour(
                                                        smartRxInsiderData?.prescriptionDate,
                                                    )}`}
                                            </div>
                                            <div className="d-flex justify-content-between align-items-start">
                                                {VITALS_OBSERVATION[v.status]}
                                                <div className="vital-card-header">
                                                    {v.status}
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            }
                        >
                            <VitalsCard
                                id={v.id}
                                value={v.vitalValueString}
                                date={v.detailedDate ?? v.date}
                                standard={
                                    v.standard ?? v.vitalValueStandardString
                                }
                                observation={v.observation ?? v.status}
                                description={
                                    v.description
                                        ? v.description
                                        : (v.description2 ?? "")
                                }
                                measurementUnit={v?.measurementUnit}
                                vitalData={v}
                                handleOpenModal={handleOpenModal}
                                // handleOpenModalEdit={handleOpenModalEdit}
                                rangeGraph={
                                    isBloodPressure ? (
                                        <PulseRateGraph
                                            bloodPressure={true}
                                            value={v.vitalValue}
                                            bloodPressureLowerValue={
                                                diastolicEntry?.vitalValue
                                            }
                                            {...parseStandardRange(
                                                v.vitalValueStandardString,
                                                v.name,
                                                v.applicableEntity,
                                            )}
                                            min={min}
                                            max={max}
                                        />
                                    ) : (v.title ?? v.name)?.toLowerCase() ===
                                      "bmi" ? (
                                        (() => {
                                            const standardRange =
                                                parseStandardRange(
                                                    v.vitalValueStandardString,
                                                    v.name,
                                                    v.applicableEntity,
                                                );
                                            const overWeightRange =
                                                parseStandardRange(
                                                    v.vitalValueOverWeightString,
                                                    v.name,
                                                    v.applicableEntity,
                                                );

                                            const {
                                                low: standardLow,
                                                high: standardHigh,
                                            } = standardRange;
                                            const {
                                                low: overWeightLow,
                                                high: overWeightHigh,
                                            } = overWeightRange;

                                            return (
                                                <PulseRateGraph
                                                    value={v.vitalValue}
                                                    low={standardLow}
                                                    high={standardHigh}
                                                    overWeightLow={
                                                        overWeightLow
                                                    }
                                                    overWeightHigh={
                                                        overWeightHigh
                                                    }
                                                    min={min}
                                                    max={max}
                                                    isBMI={true}
                                                    slightlyHigh={
                                                        v.vitalMidNextRange
                                                    }
                                                />
                                            );
                                        })()
                                    ) : (
                                        <PulseRateGraph
                                            value={v.vitalValue}
                                            {...parseStandardRange(
                                                v.vitalValueStandardString,
                                                v.name,
                                                v.applicableEntity,
                                            )}
                                            min={min}
                                            max={max}
                                        />
                                    )
                                }
                            />
                        </CustomAccordion>
                    );
                })}

            {/* ───── Add / Edit / Delete modal ───── */}

            <VitalManagementModal
                modalType={modalType}
                isOpen={
                    modalType === "add" ||
                    modalType === "edit" ||
                    modalType === "delete"
                }
                folderData={selectedFolder}
                vitalData={selectedVitalData}
                onClose={closeModal}
                fetchSmartRxVitalData={fetchSmartRxVitalData}
                smartRxInsiderGenderData={
                    smartRxInsiderData?.patientInfo?.gender
                }
                smartRxInsiderAgeData={smartRxInsiderData?.patientInfo?.age}
                anotherButton={"true"}
                smartRxMasterId={smartRxMasterId}
                prescriptionId={
                    smartRxInsiderData?.prescriptions?.[0]?.prescriptionId
                }
                refetch={refetch}
                smartRxInsiderVitalData={
                    smartRxInsiderData?.prescriptions?.[0]?.vitals
                }
                patientFullName={`${smartRxInsiderData?.patientInfo?.firstName || ""} ${smartRxInsiderData?.patientInfo?.lastName || ""} ${smartRxInsiderData?.patientInfo?.nickName || ""}`.trim()}
                patientId= {smartRxInsiderData?.patientId || null}

            />
            <VitalFaqModal
                modalType={modalType}
                isOpen={modalType === "faq"}
                vitalData={selectedVitalData}
                onClose={closeModal}
                fetchSmartRxVitalData={fetchSmartRxVitalData}
                anotherButton={"true"}
                smartRxMasterId={smartRxMasterId}
                prescriptionId={
                    smartRxInsiderData?.prescriptions?.[0]?.prescriptionId
                }
                refetch={refetch}
                vitalFAQsData={vitalFAQsData}
                isVitalFAQLoading={isVitalFAQLoading}
                vitalFAQError={vitalFAQError}
            />
            <EditNewModal
                modalType={modalType}
                isOpen={modalType === "editNew"}
                onClose={closeModal}
                selectedItem={selectedItem}
                patientId={smartRxInsiderData?.prescriptions?.[0]?.patientId}
                smartRxInsiderGenderData={
                    smartRxInsiderData?.patientInfo?.gender
                }
                smartRxInsiderAgeData={smartRxInsiderData?.patientInfo?.age}
                smartRxInsiderHeightData={
                    smartRxInsiderData?.patientInfo?.height
                }
                smartRxInsiderWeightData={
                    smartRxInsiderData?.patientInfo?.weight
                }
                anotherButton={"true"}
                smartRxMasterId={smartRxMasterId}
                prescriptionId={
                    smartRxInsiderData?.prescriptions?.[0]?.prescriptionId
                }
                refetch={refetch}
            />
        </div>
    );
};

export default Vitals;
