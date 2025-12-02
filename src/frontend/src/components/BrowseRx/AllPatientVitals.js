import React, { useEffect, useState, useMemo, useRef } from "react";
import { FaSort, FaSortUp, FaSortDown, FaChevronLeft, FaChevronRight } from "react-icons/fa";
import PageTitle from "../static/PageTitle/PageTitle";
import useApiClients from "../../services/useApiClients";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import CustomButton from "../static/Commons/CustomButton";
import { useNavigate, useLocation } from "react-router-dom";
import "./AllPatientVital.css";
import { Table } from "react-bootstrap";
import plusIcon from "../../assets/img/PlusIconPurple.png";
import TemparatureImage from "../../assets/img/VitalTemaparature.svg";
import RespiratoryImage from "../../assets/img/VitalRespiratoryRate.svg";
import SaturationImage from "../../assets/img/VitalSaturation.svg";
import WeightImage from "../../assets/img/VitalWeight.svg";
import GlucoseImage from "../../assets/img/VitalGlucose.svg";
import BmiImage from "../../assets/img/VitalBMI.svg";
import HeightImage from "../../assets/img/VitalHeightNew.svg";
import OxygenImage from "../../assets/img/BloodOxygenIcon.svg";
import BPImage from "../../assets/img/BloodPressureIcon.svg";
import PulseRateImage from "../../assets/img/PulseRateIcon.svg";
import CustomSelect from "../static/Dropdown/CustomSelect";
import CustomInput from "../static/Commons/CustomInput";
import { ReactComponent as SearchIcon } from "../../assets/img/SearchNew.svg";
import DateField from "../static/Commons/DateField";
import VitalManagementModal from "../SmartRxInsider/Vitals/VitalManagementModal";
import CustomModal from "../static/CustomModal/CustomModal";

const AllPatientVitals = ({ onClick }) => {
    const location = useLocation();
    const patient = location.state?.patient || null;

    const [sortConfig, setSortConfig] = useState({
        key: null,
        direction: null,
    });
    const [modalType, setModalType] = useState(null);
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    const fetchSmartRxVitalData = async ({ VitalName }) => {
        try {
            const response = await api.getVitalsByVitalName({ VitalName });
            return response;
        } catch (err) {
            console.error("API call failed:", err);
            return null;
        }
    };

    const handleSort = (key, direction) => {
        setSortConfig({ key, direction });
    };

    const [formData, setFormData] = useState({
        vitalName: "",
        fromDate: "",
        toDate: "",
        search: "",
    });

    const [filteredVitals, setFilteredVitals] = useState(null);
    const [fieldErrors, setFieldErrors] = useState({ vitalName: null });
    const [isLoading, setIsLoading] = useState(false);
    const [tooltipVisible, setTooltipVisible] = useState(null);
    const tooltipTimerRef = useRef(null);

    const handleVitalChange = (e) => {
        const { value } = e.target;
        const selectedOption = vitalOptions.find((opt) => opt.value === value);

        setFormData((prev) => ({
            ...prev,
            vitalName: selectedOption?.label || "",
        }));

        setFieldErrors((prev) => ({
            ...prev,
            vitalName: "",
        }));
    };

    const vitalOptions = [
        { value: "bp", label: "Blood Pressure" },
        { value: "temp", label: "Body Temperature" },
        { value: "pr", label: "Pulse Rate" },
        { value: "resp", label: "Respiratory Rate" },
        { value: "bo", label: "Blood Oxygen" },
        { value: "weight", label: "Weight" },
        { value: "glucose", label: "Blood Glucose" },
        { value: "height", label: "Height" },
    ];

    const [activeTab, setActiveTab] = useState("lastView");
    const [advancedSearchModalOpen, setAdvancedSearchModalOpen] = useState(false);
    const navigate = useNavigate();
    const [currentPage, setCurrentPage] = useState(1);
    const [sortBy, setSortBy] = useState("name");
    const [sortDirection, setSortDirection] = useState("asc");
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [lastVitalCurrentPage, setLastVitalCurrentPage] = useState(1);
    const [allVitalCurrentPage, setAllVitalCurrentPage] = useState(1);
    const itemsPerPageForVitals = 5;
    const { api } = useApiClients();
    const userId = useCurrentUserId();

    const renderSortIcons = (key) => (
        <span
            className="sort-icons"
            style={{ display: "flex", flexDirection: "column" }}
        >
            <FaSortUp
                className={`sort-icon ${sortConfig.key === key && sortConfig.direction === "asc" ? "active" : "inactive"}`}
                onClick={(e) => {
                    e.stopPropagation();
                    handleSort(key, "asc");
                }}
            />
            <FaSortDown
                className={`sort-icon ${sortConfig.key === key && sortConfig.direction === "desc" ? "active" : "inactive"}`}
                onClick={(e) => {
                    e.stopPropagation();
                    handleSort(key, "desc");
                }}
            />
        </span>
    );

    const formatDate = (isoDate) => {
        if (!isoDate) return "";
        const date = new Date(isoDate);
        const day = String(date.getDate()).padStart(2, "0");
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const year = String(date.getFullYear()).slice(-2);
        return `${day}-${month}-${year}`;
    };

    const vitalImages = {
        "Body Temperature": TemparatureImage,
        "Respiratory Rate": RespiratoryImage,
        "Blood Oxygen": SaturationImage,
        Weight: WeightImage,
        Height: HeightImage,
        "Blood Glucose": GlucoseImage,
        BMI: BmiImage,
        "Blood Pressure": BPImage,
        "Pulse Rate": PulseRateImage,
    };

    const vitalInfo = {
        bp: {
            mmHg: {
                range: "Normal Range: 120/80 mmHg",
                example: "120/80",
            },
        },
        temp: {
            "°F": { range: "Normal Range: 98.6°F to 100.4°F", example: "98.8" },
        },
        pr: {
            bpm: { range: "Normal Range: 60 to 100 bpm", example: "75" },
        },
        resp: {
            "breaths/min": {
                range: "Normal Range:\nAdults: 12 to 16\nKids: 18 to 30\nInfants: 30 to 60",
                example: "16",
            },
            rpm: {
                range: "Normal Range:\nAdults: 12 to 16\nKids: 18 to 30\nInfants: 30 to 60",
                example: "16"
            },
        },
        bo: {
            "%": { range: "Normal Range: 95% to 100%", example: "98" },
        },
        height: {
            ftin: {
                range: "Normal Range: 4 to 7 Feet-inches (varies)",
                example: "5.5",
            },
        },
        weight: {
            kg: {
                range: "Normal Range: 50 kg to 100 kg (varies)",
                example: "70",
            },
            g: { range: "Normal Range: 50000 g to 100000 g", example: "70000" },
            lb: { range: "Normal Range: 110 lb to 220 lb", example: "154" },
        },
        glucose: {
            "mg/dL": { range: "Normal Range: 70 to 140 mg/dL", example: "90" },
            "mmol/L": {
                range: "Normal Range: 3.9 to 7.8 mmol/L",
                example: "5.0",
            },
        },
        bmi: {
            "kg/m²": {
                range: "Normal Range: 18.5 to 24.9 kg/m²",
                example: "22.5"
            },
        },
    };

    const vitalNameToKey = {
        "Blood Pressure": "bp",
        "Body Temperature": "temp",
        "Pulse Rate": "pr",
        "Respiratory Rate": "resp",
        "Blood Oxygen": "bo",
        "Height": "height",
        "Weight": "weight",
        "Blood Glucose": "glucose",
        "BMI": "bmi",
    };

    // Get normal range text for a vital
    const getVitalNormalRange = (vitalName, vitalUnit) => {
        const vitalKey = vitalNameToKey[vitalName];
        if (!vitalKey || !vitalInfo[vitalKey]) return null;

        const vitalData = vitalInfo[vitalKey];

        // If unit is provided and exists, use it
        if (vitalUnit && vitalData[vitalUnit]) {
            return vitalData[vitalUnit].range;
        }

        // Otherwise, get the first available unit's range
        const firstUnit = Object.keys(vitalData)[0];
        return vitalData[firstUnit]?.range || null;
    };

    // Handle tooltip click
    const handleTooltipClick = (vitalId) => {
        // Clear any existing timer
        if (tooltipTimerRef.current) {
            clearTimeout(tooltipTimerRef.current);
        }

        // Toggle tooltip visibility
        if (tooltipVisible === vitalId) {
            setTooltipVisible(null);
        } else {
            setTooltipVisible(vitalId);

            // Auto-close after 5 seconds
            tooltipTimerRef.current = setTimeout(() => {
                setTooltipVisible(null);
            }, 5000);
        }
    };

    // Clean up timer on unmount
    useEffect(() => {
        return () => {
            if (tooltipTimerRef.current) {
                clearTimeout(tooltipTimerRef.current);
            }
        };
    }, []);

    const handleClear = () => {
        setFormData({
            vitalName: "",
            fromDate: "",
            toDate: "",
            search: "",
        });

        setFieldErrors({
            vitalName: null,
            fromDate: null,
            toDate: null,
            search: null,
        });

        setFilteredVitals(null);
    };

    // Autocomplete state
    const [suggestions, setSuggestions] = useState([]);
    const [isSuggesting, setIsSuggesting] = useState(false);
    const suggestAbortRef = useRef(null);

    // Debounced autocomplete fetch from API based on search keyword
    useEffect(() => {
        const keyword = (formData.search || "").trim();
        if (!keyword) {
            if (suggestions.length) setSuggestions([]);
            return;
        }

        const timer = setTimeout(async () => {
            try {
                setIsSuggesting(true);
                if (suggestAbortRef.current) {
                    try { suggestAbortRef.current.abort(); } catch (_) { }
                }
                suggestAbortRef.current = new AbortController();


                const payload = {
                    UserId: userId,
                    PatientId: patient?.patientId || patient?.id || null,
                    VitalName: formData.vitalName || null,
                    SearchKeyword: keyword,
                    SearchColumn: null,
                    FromDate: formatDateForAPI(formData.fromDate),
                    ToDate: formatDateForAPI(formData.toDate),
                    PagingSorting: {
                        PageNumber: currentPage,
                        PageSize: itemsPerPage,
                        SortBy: getSortField(sortBy),
                        SortDirection: getSortDirection(sortDirection),
                    },
                };

                const resp = await api.getPatientVitalListFilterById(
                    suggestAbortRef.current.signal,
                    payload,
                );

                const items = Array.isArray(resp?.response?.data)
                    ? resp.response.data
                    : Array.isArray(resp?.data)
                        ? resp.data
                        : Array.isArray(resp)
                            ? resp
                            : [];

                // Flatten vitals arrays and collect unique text suggestions
                const collected = new Set();
                items.forEach((p) => {
                    if (Array.isArray(p?.vitals)) {
                        p.vitals.forEach((v) => {
                            const name = (v?.vitalName || "").trim();
                            const val = (v?.vitalValue?.toString?.() || "").trim();
                            const status = (v?.vitalStatus || "").trim();
                            if (name && name.toLowerCase().includes(keyword.toLowerCase())) collected.add(name);
                            if (val && val.toLowerCase().includes(keyword.toLowerCase())) collected.add(val);
                            if (status && status.toLowerCase().includes(keyword.toLowerCase())) collected.add(status);
                        });
                    }
                });

                setSuggestions(Array.from(collected).slice(0, 10));
            } catch (e) {
                // ignore abort errors
            } finally {
                setIsSuggesting(false);
            }
        }, 300);

        return () => clearTimeout(timer);

    }, [formData.search, userId, patient?.patientId, patient?.id, formData.fromDate, formData.toDate]);

    const formatDateForAPI = (date) => {
        if (!date) return null;
        const d = new Date(date);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, "0");
        const day = String(d.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    };

    // Function to group blood pressure readings by date and pair systolic/diastolic
    const groupBloodPressureReadings = (vitals) => {
        if (!vitals || !Array.isArray(vitals)) return [];

        const bpGroups = {};

        vitals.forEach((vital) => {
            if (vital.vitalName === "Blood Pressure") {
                const dateKey = new Date(vital.createdDate)
                    .toISOString()
                    .split("T")[0];

                if (!bpGroups[dateKey]) {
                    bpGroups[dateKey] = {
                        systolic: null,
                        diastolic: null,
                        createdDate: vital.createdDate,
                        vitalStatus: "",
                    };
                }

                if (vital.vitalId === 65) {
                    // Systolic
                    bpGroups[dateKey].systolic = vital;
                } else if (vital.vitalId === 64) {
                    // Diastolic
                    bpGroups[dateKey].diastolic = vital;
                }
            }
        });

        // Convert grouped BP readings to array format and set status
        const bpReadings = Object.values(bpGroups).map((group) => {
            // ALWAYS use vitalId 65 (systolic) status if available
            let finalStatus = "";
            if (group.systolic && group.systolic.vitalStatus) {
                finalStatus = group.systolic.vitalStatus;
            } else if (group.diastolic && group.diastolic.vitalStatus) {
                finalStatus = group.diastolic.vitalStatus;
            }

            return {
                id:
                    group.systolic?.id ||
                    group.diastolic?.id ||
                    `bp-${group.createdDate}`,
                vitalName: "Blood Pressure",
                createdDate: group.createdDate,
                systolic: group.systolic,
                diastolic: group.diastolic,
                displayValue:
                    group.systolic && group.diastolic
                        ? `${group.systolic.vitalValue} mmHg / ${group.diastolic.vitalValue} mmHg`
                        : group.systolic
                            ? `${group.systolic.vitalValue} mmHg`
                            : group.diastolic
                                ? `${group.diastolic.vitalValue} mmHg`
                                : "",
                vitalStatus: finalStatus,
                isBloodPressure: true,
            };
        });

        return bpReadings;
    };

    const handleSubmitSearch = async () => {
        setIsLoading(true);

        try {
            const basePayload = {
                UserId: userId,
                PatientId: patient?.patientId || patient?.id || null,
                VitalName: formData.vitalName || null,
                SearchKeyword: formData.search || null,
                SearchColumn: null,
                FromDate: formatDateForAPI(formData.fromDate),
                ToDate: formatDateForAPI(formData.toDate),
                PagingSorting: {
                    PageNumber: currentPage,
                    PageSize: itemsPerPage,
                    SortBy: getSortField(sortBy),
                    SortDirection: getSortDirection(sortDirection),
                },
            };
            
            const shouldMultiColumnSearch = (basePayload.SearchKeyword || "").trim() && basePayload.SearchColumn == null;

            const fetchForColumn = async (column) => {

                const payload = { ...basePayload, SearchColumn: null };
                const response = await api.getPatientVitalListFilterById(undefined, payload);
                const data = Array.isArray(response?.response?.data)
                    ? response.response.data
                    : Array.isArray(response?.data)
                        ? response.data
                        : Array.isArray(response)
                            ? response
                            : [];
                const vitals = [];

                data.forEach((patientData) => {
                    if (Array.isArray(patientData?.vitals)) {
                        vitals.push(...patientData.vitals);
                    }
                });
                return vitals;
            };

            let extractedVitals = [];
            if (shouldMultiColumnSearch) {

                const columnsToTry = ["VitalName", "VitalValue", "VitalStatus"];
                const results = await Promise.all(columnsToTry.map(fetchForColumn));
                const merged = results.flat();
                const seen = new Set();
                extractedVitals = merged.filter((v) => {
                    const key = v?.id ?? `${v?.vitalName}|${v?.vitalValue}|${v?.vitalStatus}|${v?.createdDate}`;
                    if (seen.has(key)) return false;
                    seen.add(key);
                    return true;
                });
            } else {
                const singleResponse = await api.getPatientVitalListFilterById(undefined, basePayload);
                const data = Array.isArray(singleResponse?.response?.data)
                    ? singleResponse.response.data
                    : Array.isArray(singleResponse?.data)
                        ? singleResponse.data
                        : Array.isArray(singleResponse)
                            ? singleResponse
                            : [];
                data.forEach((patientData) => {
                    if (Array.isArray(patientData?.vitals)) {
                        extractedVitals.push(...patientData.vitals);
                    }
                });
            }

            setFilteredVitals(extractedVitals);
        } catch (error) {
            console.error("Search error:", error);
            setFilteredVitals([]);
        } finally {
            setIsLoading(false);
        }
    };

    const getSortField = (sortBy) => {
        if (sortBy === "lowToHigh" || sortBy === "highToLow") return "age";
        if (sortBy === "yearAsc" || sortBy === "yearDesc") return "createdDate";
        if (sortBy === "alphabeticAsc" || sortBy === "alphabeticDesc")
            return "patientcode";
        if (sortBy === "name" || sortBy === "name") return "name";
        return "patientcode";
    };

    const getSortDirection = (sortBy) => {
        if (
            sortBy === "lowToHigh" ||
            sortBy === "yearAsc" ||
            sortBy === "alphabeticAsc" ||
            sortBy === "name"
        )
            return "asc";
        if (
            sortBy === "highToLow" ||
            sortBy === "yearDesc" ||
            sortBy === "alphabeticDesc"
        )
            return "desc";
        return "asc";
    };

    // Function to process vitals and group blood pressure readings
    const processVitalsForDisplay = (vitals) => {
        if (!vitals || vitals.length === 0) return [];

        // Separate BP and non-BP vitals
        const bpVitals = vitals.filter((v) => v.vitalName === "Blood Pressure");
        const nonBpVitals = vitals.filter(
            (v) => v.vitalName !== "Blood Pressure",
        );

        // Group BP readings by date
        const groupedBP = groupBloodPressureReadings(bpVitals);

        // Add display properties to non-BP vitals
        const processedNonBpVitals = nonBpVitals.map((vital) => {
            let displayValue = vital.vitalValue;

            if (
                vital.vitalName === "Height" &&
                vital.heightFeet != null &&
                vital.heightInches != null
            ) {
                displayValue = `${vital.heightFeet} Feet ${vital.heightInches} Inches`;
            } else if (vital.vitalUnit) {
                displayValue = `${vital.vitalValue} ${vital.vitalUnit}`;
            }

            return {
                ...vital,
                displayValue,
                isBloodPressure: false,
            };
        });

        // Combine grouped BP with other vitals
        const allProcessedVitals = [...groupedBP, ...processedNonBpVitals];

        // Sort by date (newest first)
        return allProcessedVitals.sort(
            (a, b) => new Date(b.createdDate) - new Date(a.createdDate),
        );
    };

    // Function to render table rows for vitals
    const renderVitalRows = (vitals) => {
        if (!vitals || vitals.length === 0) {
            return (
                <tr>
                    <td colSpan={4} className="text-center">
                        No vitals available
                    </td>
                </tr>
            );
        }

        return vitals.map((vital, index) => {
            let displayValue = vital.displayValue || vital.vitalValue;
            let statusText = vital.vitalStatus || "";
            let statusColor = "";

            // Determine status color
            if (statusText) {
                const statusLower = statusText.toLowerCase();
                if (statusLower.includes("high")) {
                    statusColor = "red";
                } else if (statusLower.includes("low")) {
                    statusColor = "#fdc34b";
                } else if (statusLower.includes("normal")) {
                    statusColor = "green";
                } else {
                    statusColor = "black";
                }
            }

            const vitalId = vital.id || `vital-${index}`;
            const normalRangeText = getVitalNormalRange(vital.vitalName, vital.vitalUnit);

            return (

                <tr key={vitalId} className="table-body-row">
                    <td>
                        {vitalImages[vital.vitalName] && (
                            <img
                                src={vitalImages[vital.vitalName]}
                                alt={vital.vitalName}
                                style={{
                                    width: "24px",
                                    height: "24px",
                                    display: "block",
                                    margin: "0 auto 4px",
                                }}
                            />
                        )}
                        {vital.vitalName}
                    </td>
                    <td>{formatDate(vital.createdDate)}</td>
                    <td>
                        {displayValue}
                        <br />
                        {statusText && (
                            <span
                                style={{
                                    color: statusColor,
                                    fontWeight: "bold",
                                }}
                            >
                                {statusText}
                            </span>
                        )}
                    </td>
                    <td className="std-details-cell" style={{ position: "relative" }}>
                        <span
                            className="view-text"
                            style={{ cursor: "pointer", textDecoration: "underline" }}
                            onClick={() => handleTooltipClick(vitalId)}
                        >
                            View
                        </span>
                        {tooltipVisible === vitalId && normalRangeText && (
                            <span className="vital-tooltip-text">
                                <div style={{ fontSize: "9px", color: "#65636e", textAlign: "left", whiteSpace: "pre-line" }}>
                                    {normalRangeText}
                                </div>
                            </span>
                        )}
                    </td>
                </tr>
            );
        });
    };

    // Get latest vitals for the "Last View" tab
    const getLatestVitals = (vitals) => {
        if (!vitals || vitals.length === 0) return [];

        // First process all vitals to group blood pressure readings
        const processedVitals = processVitalsForDisplay(vitals);
        const latestVitalsMap = new Map();

        processedVitals.forEach((vital) => {
            const existing = latestVitalsMap.get(vital.vitalName);
            if (
                !existing ||
                new Date(vital.createdDate) > new Date(existing.createdDate)
            ) {
                latestVitalsMap.set(vital.vitalName, vital);
            }
        });

        return Array.from(latestVitalsMap.values());
    };

    // Get paginated vitals
    const getPaginatedVitals = (vitals, currentPage) => {
        if (!vitals || vitals.length === 0) return [];

        const startIndex = (currentPage - 1) * itemsPerPageForVitals;
        const endIndex = startIndex + itemsPerPageForVitals;
        return vitals.slice(startIndex, endIndex);
    };

    // Calculate total pages - ensure at least 1 page
    const getTotalPages = (vitals) => {
        if (!vitals || vitals.length === 0) return 0;
        return Math.max(1, Math.ceil(vitals.length / itemsPerPageForVitals));
    };

    // Pagination helper function - same as PatientVitalList.js
    const getPageNumbers = (current, total) => {
        const delta = 2;
        const range = [];
        const rangeWithDots = [];
        let l;

        for (let i = 1; i <= total; i++) {
            if (
                i === 1 ||
                i === total ||
                (i >= current - delta && i <= current + delta)
            ) {
                range.push(i);
            }
        }

        for (let i of range) {
            if (l) {
                if (i - l === 2) rangeWithDots.push(l + 1);
                else if (i - l !== 1) rangeWithDots.push(null);
            }
            rangeWithDots.push(i);
            l = i;
        }
        return rangeWithDots;
    };

    // Render pagination component
    const renderPagination = (totalPages, currentPage, setCurrentPage) => {
        if (totalPages <= 1) return null;

        return (
            <div className="d-flex justify-content-center gap-2 flex-wrap mt-4 align-items-center">
                <button
                    className="btn btn-light btn-sm"
                    disabled={currentPage === 1}
                    style={{ cursor: "pointer", fontFamily: "Georama" }}
                    onClick={() => setCurrentPage((p) => p - 1)}
                >
                    <FaChevronLeft className="me-1" />
                    Prev
                </button>

                <span
                    style={{
                        fontFamily: "Georama",
                        fontSize: "10px",
                        fontWeight: "500",
                        padding: "0 12px",
                        color: "#fff",
                        backgroundColor: "#4b3b8b",
                        borderRadius: "50%",
                        width: "24px",
                        height: "24px",
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        
                    }}
                >
                    {currentPage}
                </span>

                <button
                    className="btn btn-light btn-sm"
                    disabled={currentPage === totalPages}
                    style={{ cursor: "pointer", fontFamily: "Georama" }}
                    onClick={() => setCurrentPage((p) => p + 1)}
                >
                    Next
                    <FaChevronRight className="ms-1" />
                </button>
            </div>
        );
    };

    // Reset pagination when switching tabs or when filtered vitals change
    useEffect(() => {
        if (activeTab === "lastView") {
            setLastVitalCurrentPage(1);
        } else if (activeTab === "allView") {
            setAllVitalCurrentPage(1);
        }
    }, [activeTab, filteredVitals]);

    return (
        <>
            <div className="content-container">
                <div className="rx-folder-container row px-3 px-md-5">
                    <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                        <PageTitle
                            pageName={`${patient?.firstName}'s Vitals`}
                            smallText={true}
                        />

                        <div
                            className="add-vital-button-container"
                            style={{
                                display: "flex",
                                justifyContent: "flex-end",
                                marginBottom: "4px",
                            }}
                        >
                            <CustomButton
                                type="button"
                                label={
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                            fontFamily: "Georama",
                                            fontWeight: "600",
                                            gap: "8px",
                                            verticalAlign: "middle",
                                        }}
                                    >
                                        <span
                                            style={{
                                                display: "flex",
                                                justifyContent: "center",
                                                alignItems: "center",
                                                width: "20px",
                                                height: "20px",
                                                borderRadius: "50%",
                                                backgroundColor: "#4b3b8b",
                                                color: "#fff",
                                            }}
                                        >
                                            <img
                                                src={plusIcon}
                                                alt="Add New Vital"
                                                style={{
                                                    width: "20px",
                                                    height: "20px",
                                                }}
                                            />
                                        </span>
                                        Add New Vital
                                    </div>
                                }
                                className="add-vital-action-btn"
                                width="auto"
                                textColor="var(--theme-font-color)"
                                backgroundColor="#FAF8FA"
                                borderRadius="4px"
                                shape="Square"
                                borderColor="1px solid var(--theme-font-color)"
                                labelStyle={{
                                    fontSize: "10px",
                                    fontWeight: "500",
                                    textTransform: "capitalize",
                                }}
                                hoverEffect="theme"
                                onClick={() => openModal("add")}
                            />
                        </div>

                        <div className="tabbed-vital">
                            <div className="vital-tabs">
                                <button
                                    className={`tab ${activeTab === "lastView" ? "active" : ""}`}
                                    onClick={() => setActiveTab("lastView")}
                                >
                                    All Last Vital
                                </button>
                                <button
                                    className={`tab ${activeTab === "allView" ? "active" : ""}`}
                                    onClick={() => setActiveTab("allView")}
                                >
                                    All Vital View
                                </button>
                            </div>

                            {activeTab === "lastView" && (
                                <div className="content-table">
                                    <div
                                        style={{
                                            overflowX: "auto",
                                            overflowY: "auto",
                                        }}
                                    >
                                        <Table
                                            bordered
                                            hover
                                            size="sm"
                                            className="custom-table"
                                        >
                                            <thead className="table-header-row">
                                                <tr>
                                                    <th>
                                                        <span className="th-content">
                                                            Vitals Name
                                                            {renderSortIcons(
                                                                "vitalName",
                                                            )}
                                                        </span>
                                                    </th>
                                                    <th>
                                                        <span className="th-content two-line">
                                                            {"Date\nREF"}
                                                            {renderSortIcons(
                                                                "dateRef",
                                                            )}
                                                        </span>
                                                    </th>
                                                    <th>
                                                        <span className="th-content">
                                                            Value
                                                            {renderSortIcons(
                                                                "value",
                                                            )}
                                                        </span>
                                                    </th>
                                                    <th>
                                                        <span className="th-content two-line">
                                                            {"Std\nDetails"}
                                                        </span>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {renderVitalRows(
                                                    getPaginatedVitals(
                                                        getLatestVitals(patient?.vitals),
                                                        lastVitalCurrentPage
                                                    )
                                                )}
                                            </tbody>
                                        </Table>
                                    </div>

                                    {/* Pagination for Last Vital View */}
                                    {(() => {
                                        const latestVitals = getLatestVitals(patient?.vitals);
                                        const totalPages = getTotalPages(latestVitals);
                                        return renderPagination(totalPages, lastVitalCurrentPage, setLastVitalCurrentPage);
                                    })()}
                                </div>
                            )}

                            {activeTab === "allView" && (
                                <div>
                                    <div
                                        className="pt-vital-action-buttons"
                                        style={{
                                            display: "flex",
                                            gap: "5px",
                                            marginBottom: "5px",
                                            marginTop: "5px",
                                        }}
                                    >
                                        <div
                                            className="pt-vital-search-button"
                                            style={{ width: "100%" }}
                                        >
                                            <CustomButton
                                                type="button"
                                                label={
                                                    <div
                                                        style={{
                                                            display: "flex",
                                                            alignItems: "center",
                                                            fontFamily: "Georama",
                                                            fontWeight: "600",
                                                            gap: "8px",
                                                            verticalAlign: "middle",
                                                            justifyContent: "center",
                                                        }}
                                                    >
                                                        Advanced Search
                                                    </div>
                                                }
                                                className="vital-search-action-btn"
                                                width="100%"
                                                textColor="var(--theme-font-color)"
                                                backgroundColor="#FAF8FA"
                                                borderRadius="4px"
                                                shape="Square"
                                                borderColor="1px solid var(--theme-font-color)"
                                                labelStyle={{
                                                    fontSize: "16px",
                                                    fontWeight: "500",
                                                    textTransform: "capitalize",
                                                }}
                                                hoverEffect="theme"
                                                onClick={() => setAdvancedSearchModalOpen(true)}
                                            />
                                        </div>
                                    </div>

                                    <div className="content-table">
                                        <div
                                            style={{
                                                overflowX: "auto",
                                                overflowY: "auto",
                                            }}
                                        >
                                            <Table
                                                bordered
                                                hover
                                                size="sm"
                                                className="custom-table"
                                            >
                                                <thead className="table-header-row">
                                                    <tr>
                                                        <th>
                                                            <span className="th-content">
                                                                Vitals Name
                                                                {renderSortIcons(
                                                                    "vitalName",
                                                                )}
                                                            </span>
                                                        </th>
                                                        <th>
                                                            <span className="th-content two-line">
                                                                {"Date\nREF"}
                                                                {renderSortIcons(
                                                                    "dateRef",
                                                                )}
                                                            </span>
                                                        </th>
                                                        <th>
                                                            <span className="th-content">
                                                                Value
                                                                {renderSortIcons(
                                                                    "value",
                                                                )}
                                                            </span>
                                                        </th>
                                                        <th>
                                                            <span className="th-content two-line">
                                                                {"Std\nDetails"}
                                                            </span>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {isLoading ? (
                                                        <tr>
                                                            <td
                                                                colSpan={4}
                                                                className="text-center"
                                                            >
                                                                Searching...
                                                            </td>
                                                        </tr>
                                                    ) : (
                                                        renderVitalRows(
                                                            getPaginatedVitals(
                                                                filteredVitals !== null
                                                                    ? processVitalsForDisplay(filteredVitals)
                                                                    : processVitalsForDisplay(patient?.vitals || []),
                                                                allVitalCurrentPage
                                                            )
                                                        )
                                                    )}
                                                </tbody>
                                            </Table>
                                        </div>
                                    </div>

                                    {/* Pagination for All Vital View */}
                                    {!isLoading && (() => {
                                        const allVitals = filteredVitals !== null
                                            ? processVitalsForDisplay(filteredVitals)
                                            : processVitalsForDisplay(patient?.vitals || []);
                                        const totalPages = getTotalPages(allVitals);
                                        return renderPagination(totalPages, allVitalCurrentPage, setAllVitalCurrentPage);
                                    })()}
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
            {/* Add Vital Modal */}
            <VitalManagementModal
                modalType={modalType}
                isOpen={modalType === "add"}
                folderData={null}
                vitalData={null}
                onClose={closeModal}
                fetchSmartRxVitalData={fetchSmartRxVitalData}
                smartRxInsiderGenderData={patient?.gender}
                smartRxInsiderAgeData={patient?.age}
                anotherButton={"true"}
                smartRxMasterId={patient?.smartRxId}
                prescriptionId={patient?.prescriptionId}
                refetch={() => { }}
                smartRxInsiderVitalData={patient?.vitals}
                patientFullName={`${patient?.firstName || ""} ${patient?.lastName || ""} ${patient?.nickName || ""}`.trim()}
                patientId= {patient?.patientId || patient?.id || null}
            />

            {/* Advanced Search Modal */}
            <CustomModal
                isOpen={advancedSearchModalOpen}
                close={() => setAdvancedSearchModalOpen(false)}
                modalName="Vitals Advanced Search"
                closeOnOverlayClick={true}
                modalSize="medium"
                modalNameStyle={{
                    fontFamily: "Georama",
                    color: "#65636e",
                }}
            >
                <div style={{ padding: "30px 0 10px 0" }}>
                    {/* Date Fields */}
                    <div className="pt-vital-select-date" style={{ display: "flex", gap: "10px", marginBottom: "15px" }}>
                        <div className="date-container" style={{ width: "50%" }}>
                            <DateField
                                className="fromDate"
                                placeholderText="From date"
                                type="text"
                                value={formData.fromDate}
                                onChange={(selectedDateObj) => {
                                    setFormData((prev) => ({
                                        ...prev,
                                        fromDate: selectedDateObj,
                                    }));
                                }}
                                customStyles={{ height: "36px" }}
                            />
                        </div>
                        <div className="date-container" style={{ width: "50%" }}>
                            <DateField
                                className="toDate"
                                placeholderText="To date"
                                type="text"
                                value={formData.toDate}
                                onChange={(selectedDateObj) => {
                                    setFormData((prev) => ({
                                        ...prev,
                                        toDate: selectedDateObj,
                                    }));
                                }}
                                styles={{ height: "36px" }}
                            />
                        </div>
                    </div>

                    {/* Select Vital and Search Keyword */}
                    <div style={{ display: "flex", gap: "10px", marginBottom: "15px" }}>
                        <div style={{ width: "50%" }}>
                            <CustomSelect
                                placeholder="Select Vital"
                                name="vitalName"
                                value={
                                    vitalOptions.find(
                                        (opt) => opt.label === formData.vitalName,
                                    )?.value || ""
                                }
                                onChange={handleVitalChange}
                                error={fieldErrors.vitalName}
                                options={vitalOptions}
                                showOverModal={true}
                            />
                        </div>
                        <div style={{ width: "50%", position: "relative" }}>
                            <CustomInput
                                name="search"
                                type="text"
                                placeholder="Search with keyword..."
                                value={formData.search}
                                onChange={(e) =>
                                    setFormData((prev) => ({
                                        ...prev,
                                        [e.target.name]: e.target.value,
                                    }))
                                }
                                onKeyDown={(e) => {
                                    if (e.key === "Enter") {
                                        e.preventDefault();
                                        handleSubmitSearch();
                                        setAdvancedSearchModalOpen(false);
                                        if (suggestions.length) setSuggestions([]);
                                    }
                                }}
                                minHeight="0px"
                            />
                            {Array.isArray(suggestions) && suggestions.length > 0 && (
                                <ul
                                    style={{
                                        position: "absolute",
                                        top: "100%",
                                        left: 0,
                                        right: 0,
                                        zIndex: 10,
                                        maxHeight: "200px",
                                        overflowY: "auto",
                                        background: "#fff",
                                        border: "1px solid #ddd",
                                        borderRadius: "4px",
                                        margin: 0,
                                        padding: "4px 0",
                                        listStyle: "none",
                                    }}
                                >
                                    {suggestions.map((s, idx) => (
                                        <li
                                            key={`${s}-${idx}`}
                                            onClick={() => {
                                                setFormData((prev) => ({
                                                    ...prev,
                                                    search: s,
                                                }));
                                                setSuggestions([]);
                                            }}
                                            style={{
                                                padding: "6px 10px",
                                                cursor: "pointer",
                                                whiteSpace: "nowrap",
                                                overflow: "hidden",
                                                textOverflow: "ellipsis",
                                            }}
                                        >
                                            {s}
                                        </li>
                                    ))}
                                </ul>
                            )}
                        </div>
                    </div>

                    {/* Action Buttons */}
                    <div style={{ display: "flex", gap: "10px", marginTop: "40px" }}>
                        <div style={{ width: "50%" }}>
                            <CustomButton
                                type="button"
                                label="Clear"
                                className="vital-cancel-action-btn"
                                width="100%"
                                textColor="var(--theme-font-color)"
                                backgroundColor="#FAF8FA"
                                borderRadius="4px"
                                shape="Square"
                                borderColor="1px solid var(--theme-font-color)"
                                labelStyle={{
                                    fontSize: "16px",
                                    fontWeight: "500",
                                    textTransform: "capitalize",
                                }}
                                hoverEffect="theme"
                                onClick={handleClear}
                            />
                        </div>
                        <div style={{ width: "50%" }}>
                            <CustomButton
                                type="button"
                                label={
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                            fontFamily: "Georama",
                                            fontWeight: "600",
                                            gap: "8px",
                                            verticalAlign: "middle",
                                            justifyContent: "center",
                                        }}
                                    >
                                        <span className="search-img">
                                            <SearchIcon
                                                style={{
                                                    height: "22px",
                                                    width: "22px",
                                                    color: "#65636e",
                                                    fontFamily: "Georama"
                                                }}
                                            />
                                        </span>
                                        {isLoading ? "Searching..." : "Search"}
                                    </div>
                                }
                                className="vital-search-action-btn"
                                width="100%"
                                textColor="#FAF8FA"
                                backgroundColor="var(--theme-font-color)"
                                borderRadius="4px"
                                shape="Square"
                                borderColor="1px solid FAF8FA"
                                labelStyle={{
                                    fontSize: "16px",
                                    fontWeight: "500",
                                    textTransform: "capitalize",
                                }}
                                hoverEffect="theme"
                                onClick={() => {
                                    handleSubmitSearch();
                                    setAdvancedSearchModalOpen(false);
                                }}
                                disabled={isLoading}
                            />
                        </div>
                    </div>
                </div>
            </CustomModal>
        </>
    );
};

export default AllPatientVitals;
