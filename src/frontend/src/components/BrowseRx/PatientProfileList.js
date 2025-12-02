import React, { useEffect, useState, useMemo } from "react";
import { useLocation } from "react-router-dom";
import PageTitle from "../static/PageTitle/PageTitle";
import SearchIcon from "../../assets/img/SearchIcon.svg";
import CustomInput from "../static/Commons/CustomInput";
import useApiClients from "../../services/useApiClients";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import CustomButton from "../static/Commons/CustomButton";
import plusIcon from "../../assets/img/PlusIconPurple.png";
import { useFetchData } from "../../hooks/useFetchData";
import { GENDER, getColorForName } from "../../constants/constants";
import { useParams, useNavigate } from "react-router-dom";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import PatientView from "./PatientView";
import ProfileProgress from "../PatientProfile/ProfileProgress";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import "./PatientVitalList.css";

const PatientProfileList = () => {
    const [search, setSearch] = useState("");
    const [debouncedSearch, setDebouncedSearch] = useState("");
    const navigate = useNavigate();
    const [activeTab, setActiveTab] = useState("lastView");
    const [currentPage, setCurrentPage] = useState(1);
    const [sortBy, setSortBy] = useState("alphabeticAsc");
    const [sortDirection, setSortDirection] = useState("asc");
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const { smartNavigate } = useSmartNavigate({ scroll: "top" });

    const { state } = useLocation();
    const { api } = useApiClients();
    const loginUserId = useCurrentUserId();

    const getSortField = (sortBy) => {
        if (sortBy === "lowToHigh" || sortBy === "highToLow") return "age";
        if (sortBy === "yearAsc" || sortBy === "yearDesc") return "createdDate";
        if (sortBy === "alphabeticAsc" || sortBy === "alphabeticDesc")
            return "patientcode";
        return "patientcode";
    };

    const getSortDirection = (sortBy) => {
        if (
            sortBy === "lowToHigh" ||
            sortBy === "yearAsc" ||
            sortBy === "alphabeticAsc"
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

    const createPatientProfilePayload = useMemo(() => {
        if (!loginUserId) {
            return null;
        }

        return {
            userId: loginUserId,
            PatientId: null,
            RxType: "Smart Rx",
            searchKeyword: debouncedSearch || null,
            searchColumn: null,
            pagingSorting: {
                pageNumber: currentPage,
                pageSize: itemsPerPage,
                sortBy: getSortField(sortBy),
                sortDirection: getSortDirection(sortBy),
            },
        };
    }, [loginUserId, debouncedSearch, currentPage, itemsPerPage, sortBy]);

    // Fetch patient profiles
    const {
        data: profileListData,
        isLoading: isProfileListLoading,
        error: profileListError,
        refetch: profileListRefetch,
    } = useFetchData(
        api.getPatientProfileListById,
        currentPage - 1, // Convert to 0-based indexing
        itemsPerPage,
        getSortField(sortBy),
        getSortDirection(sortBy),
        createPatientProfilePayload,
        loginUserId,
    );

    const patientList = Array.isArray(profileListData?.data)
        ? profileListData?.data
        : [];

    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedSearch(search);
        }, 300);

        return () => clearTimeout(timer);
    }, [search]);

    const [expandedIndex, setExpandedIndex] = useState(null);
    const [goToPage, setGoToPage] = useState("");

    const totalRecords = profileListData?.totalRecords || 0;
    const totalPages = Math.max(1, Math.ceil((totalRecords || patientList.length) / itemsPerPage));

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

    const toggleExpand = (id) => {
        setExpandedIndex((prev) => (prev === id ? null : id));
    };

    const progress = null;

    return (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle pageName={"Patient Profile"} />

                    {/* Action row: Add New Patient (right aligned) */}
                    <div
                        className="d-flex justify-content-end align-items-center"
                        style={{
                            fontSize: "14px",
                            marginTop: "10px",
                            fontFamily: "Georama",
                            color: "#4b3b8b",
                            fontWeight: "bold",
                            gap: "10px",
                        }}
                    >
                        <CustomButton
                            type="button"
                            label={
                                <div
                                    style={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "8px",
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
                                            alt="Add"
                                            style={{
                                                width: "20px",
                                                height: "20px",
                                            }}
                                        />
                                    </span>
                                    Add New Patient
                                </div>
                            }
                            className="add-patient-action-btn"
                            width="auto"
                            height="clamp(36px, 2.3vw, 40px)"
                            textColor="var(--theme-font-color)"
                            backgroundColor="#FAF8FA"
                            borderRadius="4px"
                            shape="Square"
                            borderColor="1px solid var(--theme-font-color)"
                            labelStyle={{
                                fontSize: "14px",
                                fontWeight: "500",
                                textTransform: "capitalize",
                            }}
                            hoverEffect="theme"
                            onClick={() => smartNavigate("/addPatient")}
                        />
                    </div>

                    {/* Search + Sort row */}
                    <div className="d-flex align-items-center justify-content-between gap-2 mt-2">
                        <div className="flex-grow-1">
                            <CustomInput
                                className={"w-100"}
                                icon={SearchIcon}
                                iconPosition="left"
                                name="search"
                                type="text"
                                placeholder="Search"
                                value={search}
                                onChange={(e) => setSearch(e.target.value)}
                                minHeight="0px"
                                style={{
                                    border: "1px solid #E0E3E7",
                                    borderRadius: "12px",
                                    padding: "10px 14px 10px 40px",
                                    fontSize: "16px",
                                    color: "#4B5563",
                                    boxShadow: "0 1px 2px rgba(16,24,40,0.05)",
                                }}
                            />
                        </div>
                        <div className="d-flex align-items-center gap-2">
                            <label
                                className="form-label mb-0"
                                htmlFor="sort-by-select"
                                style={{
                                    whiteSpace: "nowrap",
                                    fontSize: "12px",
                                    color: "#65636e",
                                }}
                            >
                                Sort by:
                            </label>
                            <select
                                id="sort-by-select"
                                className="form-select form-select-sm"
                                style={{
                                    width: "160px",
                                    fontFamily: "Georama",
                                    color: "#65636e",
                                }}
                                value={sortBy}
                                onChange={(e) => {
                                    setSortBy(e.target.value);
                                    setCurrentPage(1);
                                }}
                            >
                                <option value="alphabeticAsc">
                                    Name: A to Z
                                </option>
                                <option value="alphabeticDesc">
                                    Name: Z to A
                                </option>
                                <option value="yearAsc">
                                    Date: Oldest First
                                </option>
                                <option value="yearDesc">
                                    Date: Newest First
                                </option>
                                <option value="lowToHigh">
                                    Age: Low to High
                                </option>
                                <option value="highToLow">
                                    Age: High to Low
                                </option>
                            </select>
                        </div>
                    </div>

                    {/* Former Add New Patient row removed (moved above) */}
                    <div className="menu-content">
                        {isProfileListLoading ? (
                            <p
                                style={{
                                    textAlign: "center",
                                    marginTop: "20px",
                                    fontFamily: "Georama",
                                    color: "#65636e"
                                }}
                            >
                                Loading patients...
                            </p>
                        ) : patientList.length === 0 ? (
                            <p
                                style={{
                                    textAlign: "center",
                                    marginTop: "20px",
                                    fontFamily: "Georama",
                                    color: "#65636e"
                                }}
                            >
                                No patients found
                            </p>
                        ) : (
                            patientList
                                .filter((p) =>
                                    `${p.firstName} ${p.lastName}`
                                        .toLowerCase()
                                        .includes(
                                            debouncedSearch.toLowerCase(),
                                        ),
                                )
                                .map((patient) => {
                                    const fullName =
                                        `${patient.firstName || ""} ${patient.lastName || ""}`.trim();
                                    const initials =
                                        fullName
                                            .split(" ")
                                            .map((n) => n[0] || "")
                                            .join("")
                                            .substring(0, 2)
                                            .toUpperCase() || "P";

                                    const backgroundColor =
                                        getColorForName(fullName);

                                    const profileData = {
                                        profilePhotoPath:
                                            patient?.profilePhotoPath || null,
                                        picture:
                                            patient?.profilePhotoPath || null,
                                        coloredName: initials,
                                        colorForDefaultName: backgroundColor,
                                    };

                                    const progressData =
                                        patient?.profileProgress || 0;
                                    const patientData = patient;

                                    return (
                                        <div
                                            key={patient.id}
                                            className={`patient-list-item mt-1 ${
                                                expandedIndex === patient.id
                                                    ? "expanded"
                                                    : ""
                                            }`}
                                            onClick={() =>
                                                toggleExpand(patient.id)
                                            }
                                        >
                                            {/* Avatar */}
                                            <div className="patient-avatar">
                                                {patient?.profilePhotoPath ? (
                                                    <img
                                                        key={
                                                            patient.profilePhotoPath
                                                        }
                                                        src={`${patient.profilePhotoPath?.startsWith("http") ? patient.profilePhotoPath : `${process.env.REACT_APP_IMAGE_URL}/${patient.profilePhotoPath}`}?t=${Date.now()}`}
                                                        alt={`${patient.firstName} ${patient.lastName}`}
                                                        className="avatar-image"
                                                        onError={(e) => {
                                                            console.error(
                                                                "Image failed to load:",
                                                                patient.profilePhotoPath,
                                                            );
                                                            e.target.style.display =
                                                                "none";
                                                            e.target.nextSibling.style.display =
                                                                "flex";
                                                        }}
                                                    />
                                                ) : null}
                                                <div
                                                    className="avatar-initials"
                                                    style={{
                                                        display:
                                                            patient?.profilePhotoPath
                                                                ? "none"
                                                                : "flex",
                                                        backgroundColor:
                                                            backgroundColor,
                                                        width: "48px",
                                                        height: "48px",
                                                        borderRadius: "50%",
                                                        alignItems: "center",
                                                        justifyContent:
                                                            "center",
                                                        color: "white",
                                                        fontSize: "20px",
                                                        fontWeight: "bold",
                                                        fontFamily: "Georama",
                                                        border: "2px solid #E5E5E5",
                                                    }}
                                                >
                                                    {initials}
                                                </div>
                                            </div>

                                            <div className="patient-details">
                                                <div className="patient-info">
                                                    <span>Name:</span>{" "}
                                                    <span>
                                                        {patient.firstName}{" "}
                                                        {patient.lastName}{" "}
                                                        {patient.nickName}
                                                    </span>
                                                </div>
                                                <div className="patient-info">
                                                    <span>Age:</span>{" "}
                                                    <span>
                                                        {patient.age || "N/A"}
                                                    </span>
                                                </div>
                                                <div className="patient-info">
                                                    <span>Gender:</span>{" "}
                                                    <span>
                                                        {GENDER[
                                                            patient.gender
                                                        ] || "Other"}
                                                    </span>
                                                </div>
                                            </div>

                                            <div className="patient-status">
                                                <div
                                                    className="status-badge-container"
                                                    role="button"
                                                >
                                                    <div
                                                        style={{
                                                            borderRadius: "3px",
                                                            backgroundColor:
                                                                "#008000",
                                                            display:
                                                                "inline-block",
                                                            padding: "8px",
                                                            color: "white",
                                                            fontWeight: "700",
                                                            fontSize: "10px",
                                                            width: "100%",
                                                            textAlign: "center",
                                                            textTransform:
                                                                "uppercase",
                                                        }}
                                                    >
                                                        Smart RX
                                                    </div>

                                                    <div
                                                        role="button"
                                                        className="total-count"
                                                    >
                                                        {patient?.totalPrescriptions !=
                                                            null &&
                                                        patient?.totalPrescriptions !==
                                                            ""
                                                            ? patient.totalPrescriptions
                                                            : 0}{" "}
                                                        Total
                                                    </div>
                                                </div>
                                            </div>

                                            <div
                                                className={`button-group-wrapper d-flex flex-column gap-2 w-100 px-3 ${
                                                    expandedIndex === patient.id
                                                        ? "show mt-2"
                                                        : "d-none"
                                                }`}
                                            >
                                                <div className="progress-bar-wrap-full-width mb-1">
                                                    <ProfileProgress
                                                        customStyles={{
                                                            background:
                                                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                                                            borderRadius: "2px",
                                                            height: "6px",
                                                            width: "100%",
                                                            pointerEvents:
                                                                "none",
                                                        }}
                                                        progress={progressData}
                                                    />
                                                </div>
                                                <div className="d-flex justify-content-between gap-2 w-100">
                                                    <CustomButton
                                                        type="button"
                                                        label="Profile View & Update"
                                                        onClick={() =>
                                                            smartNavigate(
                                                                "/profileDetails",
                                                                {
                                                                    state: {
                                                                        data: {
                                                                            data: patientData,
                                                                            progress:
                                                                                progressData,
                                                                            profilePhotoPath:
                                                                                profileData.profilePhotoPath,
                                                                            picture:
                                                                                profileData.picture,
                                                                        },
                                                                        coloredName:
                                                                            profileData.coloredName,
                                                                        colorForDefaultName:
                                                                            profileData.colorForDefaultName,
                                                                        currentProgress:
                                                                            progressData,
                                                                        source: "PageTitle",
                                                                    },
                                                                },
                                                            )
                                                        }
                                                        width="150px"
                                                        height="30px"
                                                        textColor="var(--theme-font-color)"
                                                        shape="roundedSquare"
                                                        borderColor="1px solid var(--theme-font-color)"
                                                        labelStyle={{
                                                            fontSize:
                                                                "clamp(12px, 2.5vw, 14px)",
                                                            fontWeight: "500",
                                                            fontFamily:
                                                                "Georama",
                                                            textTransform:
                                                                "capitalize",
                                                            whiteSpace:
                                                                "nowrap",
                                                        }}
                                                    />
                                                    <CustomButton
                                                        type="button"
                                                        label="Browse RX"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            navigate(
                                                                "/browse-rx/smart-prescription-list",
                                                                {
                                                                    state: {
                                                                        UserId: loginUserId ?? 0,
                                                                        patientId:
                                                                            patient.id,
                                                                        prescriptionType:
                                                                            "smartrx",
                                                                        prescriptionOwner: `${patient.firstName} ${patient.lastName}`,
                                                                    },
                                                                },
                                                            );
                                                        }}
                                                        width="150px"
                                                        height="30px"
                                                        textColor="var(--theme-font-color)"
                                                        shape="roundedSquare"
                                                        borderColor="1px solid var(--theme-font-color)"
                                                        labelStyle={{
                                                            fontSize: "13px",
                                                            fontWeight: "500",
                                                            fontFamily:
                                                                "Georama",
                                                            textTransform:
                                                                "capitalize",
                                                        }}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                    );
                                })
                        )}

                        {/* Pagination Controls */}
                        {patientList.length > 0 && (
                            <div className="mt-4">
                                <div className="d-flex justify-content-between align-items-center mb-2">
                                    <div
                                        className="text-muted"
                                        style={{
                                            fontFamily: "Georama",
                                            color: "#65636e",
                                            fontSize: "12px",
                                        }}
                                    >
                                        {`Showing ${(currentPage - 1) * itemsPerPage + 1} to ${Math.min(
                                            currentPage * itemsPerPage,
                                            profileListData?.totalRecords ||
                                                patientList.length,
                                        )} of ${profileListData?.totalRecords || patientList.length} patients`}
                                    </div>
                                    <div className="row-per-page d-flex gap-2 align-items-center">
                                        <span
                                            className="text-muted"
                                            style={{
                                                fontSize: "12px",
                                                fontFamily: "Georama",
                                            }}
                                        >
                                            Rows per page
                                        </span>
                                        <select
                                            className="form-select form-select-sm"
                                            style={{
                                                width: "auto",
                                                minWidth: "80px",
                                                height: "28px",
                                                padding: "2px 8px",
                                                lineHeight: "24px",
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                            }}
                                            value={itemsPerPage}
                                            onChange={(e) => {
                                                setItemsPerPage(
                                                    Number(e.target.value),
                                                );
                                                setCurrentPage(1);
                                            }}
                                        >
                                            <option value={10}>10</option>
                                            <option value={20}>20</option>
                                            <option value={50}>50</option>
                                            <option value={100}>100</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="d-flex justify-content-center gap-2 flex-wrap mt-4">
                                    <button
                                        className="btn btn-light btn-sm"
                                        disabled={currentPage === 1}
                                        style={{
                                            cursor: "pointer",
                                            fontFamily: "Georama",
                                        }}
                                        onClick={() =>
                                            setCurrentPage((p) => p - 1)
                                        }
                                    >
                                        <FaChevronLeft className="me-1" />
                                        Prev
                                    </button>

                                    {getPageNumbers(
                                        currentPage,
                                        totalPages,
                                    ).map((page, idx) =>
                                        page ? (
                                            <button
                                                key={page}
                                                className={`btn btn-sm rounded-circle ${
                                                    currentPage === page
                                                        ? "button-primary"
                                                        : "btn-light"
                                                }`}
                                                style={{
                                                    padding:
                                                        page > 9
                                                            ? "0rem 0.4rem"
                                                            : "",
                                                    cursor: "pointer",
                                                    width: "32px",
                                                    height: "32px",
                                                }}
                                                onClick={() =>
                                                    setCurrentPage(page)
                                                }
                                            >
                                                {page}
                                            </button>
                                        ) : (
                                            <span
                                                key={"dots-" + idx}
                                                className="px-1"
                                            >
                                                …
                                            </span>
                                        ),
                                    )}

                                    <button
                                        className="btn btn-light btn-sm"
                                        disabled={currentPage === totalPages}
                                        style={{
                                            cursor: "pointer",
                                            fontFamily: "Georama",
                                        }}
                                        onClick={() =>
                                            setCurrentPage((p) => p + 1)
                                        }
                                    >
                                        Next
                                        <FaChevronRight className="ms-1" />
                                    </button>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default PatientProfileList;
