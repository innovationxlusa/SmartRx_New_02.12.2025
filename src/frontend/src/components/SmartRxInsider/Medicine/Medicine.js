import "./Medicine.css";
import { useState } from "react";
import ReadMoreCard from "../../static/ReadMoreCard.js";
import FaTablets from "../../../assets/img/Tablet.svg";
import skfLogo from "../../../assets/img/SkfLogo.png";
import noonIcon from "../../../assets/img/Summer.png";
import nightIcon from "../../../assets/img/Night.png";
import eveningIcon from "../../../assets/img/EveningIcon.svg";
import morningIcon from "../../../assets/img/ClearSky.png";
import RiCapsuleFill from "../../../assets/img/Capsule.svg";
import TbMedicineSyrup from "../../../assets/img/Syrup.svg";
import squareLogo from "../../../assets/img/SquareLogo.png";
import BeximcoLogo from "../../../assets/img/BeximcoLogo.png";
import InceptaLogo from "../../../assets/img/InceptaLogo.png";
import { useFetchData } from "../../../hooks/useFetchData.js";
import useApiClients from "../../../services/useApiClients.js";
import warningIcon from "../../../assets/img/RxWarningIcon.svg";
import { HiMiniArrowTopRightOnSquare } from "react-icons/hi2";
import MedicineManagementModal from "./MedicineManagementModal.js";
import { ReactComponent as Check } from "../../../assets/img/Check.svg";
import CustomAccordion from "../../static/CustomAccordion/CustomAccordion.js";
import { FaEdit, FaTrash, FaCopy } from "react-icons/fa";
import { ChartNoAxesColumnDecreasingIcon } from "lucide-react";
import Injection from "../../../assets/img/Injection.svg";
import ScalpSolution from "../../../assets/img/ScalpSolution.svg";
import OralPowder from "../../../assets/img/OralPowder.svg";
import OralSuspension from "../../../assets/img/OralSuspension.svg";
import EffervescentPowder from "../../../assets/img/EffervescentPowder.svg";
import EffervescentGranules from "../../../assets/img/EffervescentGranules.svg";
import Liquid from "../../../assets/img/Liquid.svg";
import MuscleRub from "../../../assets/img/MuscleRub.svg";
import Cream from "../../../assets/img/Cream.svg";
import DialysisSolution from "../../../assets/img/DialysisSolution.svg";
import DispersibleTablet from "../../../assets/img/DispersibleTablet.svg";
import EffervescentTablet from "../../../assets/img/EffervescentTablet.svg";
import IVInfusion from "../../../assets/img/IVInfusion.svg";
import OphthalmicOintment from "../../../assets/img/OphthalmicOintment.svg";
import SCInjection from "../../../assets/img/SCInjection.svg";
import Gel from "../../../assets/img/Gel.svg";
import TopicalGel from "../../../assets/img/TopicalGel.svg";
import RetardTablet from "../../../assets/img/RetardTablet.svg";
import TabletExtendedRelease from "../../../assets/img/TabletExtendedRelease.svg";
import TopicalSolution from "../../../assets/img/TopicalSolution.svg";
import TopicalSpray from "../../../assets/img/TopicalSpray.svg";
import PediatricDrops from "../../../assets/img/PediatricDrops.svg";
import IM_IVInjection from "../../../assets/img/IM_IVInjection.svg";
import OralPaste from "../../../assets/img/OralPaste.svg";
import NailLacquer from "../../../assets/img/NailLacquer.svg";
import OphthalmicSolution from "../../../assets/img/OphthalmicSolution.svg";
import NebuliserSolution from "../../../assets/img/NebuliserSolution.svg";
import NasalSpray from "../../../assets/img/NasalSpray.svg";
import PowderSuspense from "../../../assets/img/PowderSuspense.svg";

const Medicine = ({ smartRxInsiderData, smartRxInsiderDataRefetch }) => {
    /*  ───── Local state ─────  */
    const [modalType, setModalType] = useState(null);
    const [selectedMedicineId, setSelectedMedicineId] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [sortBy, setSortBy] = useState("lowToHigh");
    const [itemsPerPage, setItemsPerPage] = useState(3);
    const [totalDosageQty, setTotalDosageQty] = useState("");

    const { api } = useApiClients();

    // For Comparison Modal (when modalType is "add")
    const getSortField = (sortBy) => {
        if (sortBy === "lowToHigh" || sortBy === "highToLow") return "price";
        if (sortBy === "alphabeticAsc" || sortBy === "alphabeticDesc")
            return "name";
        return "price"; // fallback
    };

    const getSortDirection = (sortBy) => {
        if (sortBy === "lowToHigh" || sortBy === "alphabeticAsc") return "asc";
        if (sortBy === "highToLow" || sortBy === "alphabeticDesc")
            return "desc";
        return "asc"; // fallback
    };

    /* ---------- server fetch ---------- */
    const {
        data: medicineListToCompareData = {},
        isLoading,
        error,
        refetch: medicineCompareRefetch,
    } = useFetchData(
        modalType === "add" ? api.getMedicineListToCompare : null,
        modalType === "add" && selectedMedicineId ? currentPage - 1 : null,
        modalType === "add" && selectedMedicineId ? itemsPerPage : null,
        modalType === "add" && selectedMedicineId ? "unitPriceValue" : null,
        modalType === "add" && selectedMedicineId
            ? getSortDirection(sortBy)
            : null,
        modalType === "add" && selectedMedicineId
            ? {
                  MedicineId: selectedMedicineId,
                  PrescriptionId:
                      smartRxInsiderData?.prescriptions?.[0]?.prescriptionId,
                  PatientMedicineId: 0,
                  SmartRxMasterId: smartRxInsiderData?.smartRxId,
                  PagingSorting: {
                      PageNumber: currentPage,
                      PageSize: itemsPerPage,
                      SortBy: getSortField(sortBy),
                      SortDirection: getSortDirection(sortBy),
                  },
              }
            : null,
    );

    /* For FAQ Modal (when modalType is "dragInfo") */
    const {
        data: faqData,
        isLoading: isFaqLoading,
        error: faqError,
        refetch,
    } = useFetchData(
        modalType === "dragInfo"
            ? api.getSmartRxInsiderMedicineFAQByMedicineId
            : null,
        modalType === "dragInfo" && selectedMedicineId ? 0 : null,
        modalType === "dragInfo" && selectedMedicineId ? 0 : null,
        null,
        null,
        modalType === "dragInfo" ? selectedMedicineId : null
    );

    const medicineIcon = {
        Tab: <img src={FaTablets} className="medicine-icon" />, 
        Cap: <img src={RiCapsuleFill} className="medicine-icon" />, 
        Syr: <img src={TbMedicineSyrup} className="medicine-icon" />,
        Inj: <img src={Injection} className="medicine-icon" />,
        "Tab SR": <img src={FaTablets} className="medicine-icon" />, 
        "Sc Sol": <img src={ScalpSolution} className="medicine-icon" />,
        "Tab Chew": <img src={FaTablets} className="medicine-icon" />,
        "Oral Pow": <img src={OralPowder} className="medicine-icon" />,
        "Oral Susp": <img src={OralSuspension} className="medicine-icon" />,
        "Oint": <img src={OphthalmicOintment} className="medicine-icon" />,
        "Eff Pow": <img src={EffervescentPowder} className="medicine-icon" />,
        "Pow Susp": <img src={PowderSuspense} className="medicine-icon" />,
        "Eff Gran": <img src={EffervescentGranules} className="medicine-icon" />,
        Liq: <img src={Liquid} className="medicine-icon" />,
        "Mus Rub": <img src={MuscleRub} className="medicine-icon" />,
        Crm: <img src={Cream} className="medicine-icon" />,
        "Dial Sol": <img src={DialysisSolution} className="medicine-icon" />,
        "Tab Disp": <img src={DispersibleTablet} className="medicine-icon" />,
        "Tab Eff": <img src={EffervescentTablet} className="medicine-icon" />,
        "IV Inf": <img src={IVInfusion} className="medicine-icon" />,
        "Oph Oint": <img src={OphthalmicOintment} className="medicine-icon" />,
        "SC Inj": <img src={SCInjection} className="medicine-icon" />,
        Gel: <img src={Gel} className="medicine-icon" />,
        "Top Gel": <img src={TopicalGel} className="medicine-icon" />,
        "Tab Ret": <img src={RetardTablet} className="medicine-icon" />,
        "Tab ER": <img src={TabletExtendedRelease} className="medicine-icon" />,
        "Top Sol": <img src={TopicalSolution} className="medicine-icon" />,
        "Top Spray": <img src={TopicalSpray} className="medicine-icon" />,
        "Ped Drops": <img src={PediatricDrops} className="medicine-icon" />,
        "IM/IV Inj": <img src={IM_IVInjection} className="medicine-icon" />,
        "Oral Paste": <img src={OralPaste} className="medicine-icon" />,
        "Nail Lacq": <img src={NailLacquer} className="medicine-icon" />,
        "IV Inj": <img src={IM_IVInjection} className="medicine-icon" />,
        "Oph Sol": <img src={OphthalmicSolution} className="medicine-icon" />,
        "Neb Sol": <img src={NebuliserSolution} className="medicine-icon" />,
        "Tab EC": <img src={TabletExtendedRelease} className="medicine-icon" />,
        "Cap ER": <img src={RiCapsuleFill} className="medicine-icon" />,
        "Nas Spray": <img src={NasalSpray} className="medicine-icon" />,

        // fallback for anything not listed
        default: <img src={FaTablets} className="medicine-icon" />
    };

    const getLogoForCompany = (company) => {
        const lower = company.toLowerCase();
        if (lower.includes("square"))
            return (
                <img src={squareLogo} alt="square" className="company-logo" />
            );
        if (lower.includes("beximco"))
            return (
                <img src={BeximcoLogo} alt="beximco" className="company-logo" />
            );
        if (lower.includes("eskayef") || lower.includes("skf"))
            return <img src={skfLogo} alt="skf" className="company-logo" />;
        if (lower.includes("incepta"))
            return <img src={InceptaLogo} alt="inc" className="company-logo" />;
        return null;
    };

    const getTimeIcon = (time) => {
        switch (time.toLowerCase()) {
            case "morning":
                return (
                    <img
                        src={morningIcon}
                        alt="morning"
                        className="time-icon"
                    />
                );
            case "noon":
                return <img src={noonIcon} alt="noon" className="time-icon" />;
            case "evening":
                return (
                    <img
                        src={eveningIcon}
                        alt="evening"
                        className="time-icon"
                    />
                );
            case "night":
                return (
                    <img src={nightIcon} alt="night" className="time-icon" />
                );
            default:
                return null;
        }
    };

    const getDoseString = (entry) => {
        const doses = [];
        let frequency = Math.max(3, Math.min(entry.frequencyInADay, 4));
        for (let i = 1; i <= frequency; i++) {
            const dose = entry[`dose${i}InADay`];
            doses.push(dose || "0");
        }
        return doses.join("+");
    };
    

    const getTimeSlots = (frequencyInADay) => {
        switch (frequencyInADay) {
            case 1:
                return ["Morning"];
            case 2:
                return ["Morning", "Night"];
            case 3:
                return ["Morning", "Noon", "Night"];
            case 4:
            default:
                return ["Morning", "Noon", "Evening", "Night"];
        }
    };

    const parseDosage = (dosageString, frequencyInADay) => {
        let frequency = Math.max(3, Math.min(frequencyInADay, 4));
        const slotOrder = getTimeSlots(frequency);
        const parts = dosageString.split("+").map((part) => part.trim());
        const result = {};
        for (let i = 0; i < slotOrder.length; i++) {
            const key = slotOrder[i].toLowerCase();
            result[key] = parts[i] === "0" ? "No Dosage" : parts[i] + " Dosage";
        }
        return result;
    };

    const buildDosageSchedule = (dosage, instruction, frequencyInADay) => {
        let frequency = frequencyInADay;
        if(frequencyInADay<3) frequency=3;
        const timeSlots = getTimeSlots(frequency);
        return timeSlots.map((time) => {
            const key = time.toLowerCase();
            return {
                time,
                dosage: dosage[key] || "No Dosage",
                instruction:
                    dosage[key] === "No Dosage"
                        ? "No Instruction Given"
                        : instruction,
            };
        });
    };

    const calculateTotalQty = (day, dosage) => {
        const dosesPerDay = dosage
            ?.split("+")
            .reduce((sum, val) => sum + parseInt(val || "0"), 0);
        return parseInt(day) * dosesPerDay;
    };

    /* ───────── Modal helpers ───────── */
    const openModal = (type) => setModalType(type);
    const closeModal = () => {
        setModalType(null);
        setSelectedMedicineId(null);
    };

    const handleOpenModal = (medicineId, type, dosageQty) => {
        setSelectedMedicineId(medicineId);
        openModal(type);

        setTotalDosageQty(dosageQty);
    };

    return (
        <div className="medicine-container">
            {smartRxInsiderData?.prescriptions?.[0]?.medicines?.map(
                (entry, index) => {
                    const doseStr = getDoseString(entry);
                    const dosage = parseDosage(doseStr, entry.frequencyInADay);
                    const instruction = entry?.notes || "No Instruction Given";
                    const dosageSchedule = buildDosageSchedule(
                        dosage,
                        instruction,
                        entry.frequencyInADay
                    );

                    return (
                        <div key={index} className="">
                            <CustomAccordion
                                className="mt-0"
                                background="#ffffff"
                                border="1px solid #D9D9D9"
                                borderRadius="4px"
                                shadow={false}
                                defaultOpen={false}
                                iconStyleOverride={{
                                    marginRight: "20px",
                                    marginTop: "-8px",
                                }}
                                accordionHeaderData={
                                    <div className="card-header">
                                        <div className="">
                                            <div className="d-flex justify-content-between align-items-center gap-2">
                                                <div>
                                                    <span>{medicineIcon[entry?.medicineShortForm]}</span>
                                                    <span className="card-header ms-2">{`${entry.medicineDosageFormName} ${entry.medicineName}`}</span>
                                                </div>
                                                <div className="card-header-side">
                                                    {entry.medicineStrength}
                                                </div>
                                            </div>
                                            <div className="card-header-sub d-flex gap-3 gap-md-4 ms-4 ps-1 ps-md-2">
                                                <span>
                                                    {`Days - ${entry.durationOfContinuationCount} `}
                                                </span>
                                                <span>
                                                    Dosage - {doseStr}
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                }
                            >
                                <div className="accordion-body-content">
                                    <div className="des-title">
                                        {entry?.medicineName}
                                    </div>
                                    <ReadMoreCard id="" description={entry?.medicineBrandDescription} />
                                    <div>
                                        <div className="des-company-wrapper">
                                            {getLogoForCompany(
                                                entry.medicineManufacturerName,
                                            )}
                                            <div className="des-company">{entry.medicineManufacturerName}</div>

                                            {/* medicineManufacturerUrl */}
                                            <div className="des-icon ms-1">
                                                {entry.medicineManufacturerUrl ? (
                                                    <a
                                                        href={
                                                            entry.medicineManufacturerUrl
                                                        }
                                                        target="_blank"
                                                        rel="noopener noreferrer"
                                                        className="text-decoration-none"
                                                    >
                                                        <HiMiniArrowTopRightOnSquare />
                                                    </a>
                                                ) : (
                                                    <HiMiniArrowTopRightOnSquare
                                                        style={{ opacity: 0.4 }}
                                                    />
                                                )}
                                            </div>
                                        </div>

                                        <div className="des-warning-wrapper">
                                            <img
                                                src={warningIcon}
                                                alt="warning"
                                                className="warning-logo"
                                            />
                                            <div className="des-warning">
                                                {
                                                    entry.medicinePrecautionsAndWarnings
                                                }
                                            </div>
                                        </div>

                                        <div className="dosage-schedule">
                                            <div className="timeline">
                                                {dosageSchedule.map(
                                                    (schedule, idx) => (
                                                        <div
                                                            key={idx}
                                                            className="timeline-item"
                                                        >
                                                            <div
                                                                className={`timeline-bullet bullet-${schedule.time.toLowerCase()}`}
                                                            />
                                                            <div className="timeline-content">
                                                                <div className="time-header">
                                                                    <div className="time-text">
                                                                        <span>
                                                                            {
                                                                                schedule.time
                                                                            }{" "}
                                                                            -{" "}
                                                                        </span>
                                                                        <span className="dosage-text">
                                                                            {
                                                                                schedule.dosage
                                                                            }
                                                                        </span>
                                                                    </div>
                                                                    <div className="time-icon">
                                                                        {getTimeIcon(
                                                                            schedule.time,
                                                                        )}
                                                                    </div>
                                                                </div>
                                                                <div className="instruction-text">
                                                                    {
                                                                        schedule.instruction
                                                                    }
                                                                </div>
                                                            </div>
                                                        </div>
                                                    ),
                                                )}
                                            </div>
                                        </div>

                                        <div className="price-box-main">
                                            <div className="price-box">
                                                <div className="price-header-container">
                                                    <div className="price-header-text">
                                                        Total Cost BDT
                                                    </div>
                                                    <div className="total-cost-corner">
                                                        <span className="arc-value">
                                                            {(
                                                                Number(
                                                                    entry.medicineUnitPrice,
                                                                ) *
                                                                calculateTotalQty(
                                                                    entry.durationOfContinuationCount,
                                                                    doseStr,
                                                                )
                                                            ).toFixed(2)}
                                                        </span>
                                                    </div>
                                                </div>
                                                <div className="price-below-part">
                                                    <div className="price-row-fix">
                                                        <div className="price-row row-unit-price">
                                                            <span>
                                                                <Check />
                                                            </span>
                                                            <span className="price-label">
                                                                Unit Price
                                                            </span>
                                                            <span className="price-divider">
                                                                --
                                                            </span>
                                                            <span className="price-value">
                                                                BDT{" "}
                                                                {Number(
                                                                    entry.medicineUnitPrice,
                                                                ).toFixed(2)}
                                                            </span>
                                                        </div>
                                                        <div className="price-row row-total-qty">
                                                            <span>
                                                                <Check />
                                                            </span>
                                                            <span className="price-label">
                                                                Total Qty
                                                            </span>
                                                            <span className="price-divider">
                                                                --
                                                            </span>
                                                            <span className="price-value">
                                                                {calculateTotalQty(
                                                                    entry.durationOfContinuationCount,
                                                                    doseStr,
                                                                )}{" "}
                                                                Pieces
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div className="text-compare">
                                                        <button
                                                            className="add-button"
                                                            onClick={(e) => {
                                                                e.stopPropagation();
                                                                e.nativeEvent.stopImmediatePropagation();
                                                                handleOpenModal(
                                                                    entry.medicineId,
                                                                    "add",
                                                                    calculateTotalQty(
                                                                        entry.durationOfContinuationCount,
                                                                        doseStr,
                                                                    ),
                                                                );
                                                            }}
                                                        >
                                                            Compare Drug
                                                        </button>
                                                    </div>
                                                    <div className="text-drug">
                                                        <button
                                                            className="add-button"
                                                            onClick={(e) => {
                                                                e.stopPropagation();
                                                                e.nativeEvent.stopImmediatePropagation();
                                                                handleOpenModal(
                                                                    entry.medicineId,
                                                                    "dragInfo",
                                                                    "",
                                                                );
                                                            }}
                                                        >
                                                            Drug Information
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </CustomAccordion>
                        </div>
                    );
                }
            )}

            {/* ───── Add / Edit / Delete modal ───── */}
            <MedicineManagementModal
                isOpen={!!modalType}
                modalType={modalType}
                onClose={closeModal}
                selectedMedicineId={selectedMedicineId}
                data={modalType === "add" ? medicineListToCompareData : faqData}
                isLoading={modalType === "add" ? isLoading : isFaqLoading}
                error={modalType === "add" ? error : faqError}
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
                sortBy={sortBy}
                setSortBy={setSortBy}
                itemsPerPage={itemsPerPage}
                setItemsPerPage={setItemsPerPage}
                totalDosageQty={totalDosageQty}
                smartRxInsiderData={smartRxInsiderData}
                refetch={medicineCompareRefetch}
                smartRxInsiderDataRefetch={smartRxInsiderDataRefetch}
            />
        </div>
    );
};

export default Medicine;
