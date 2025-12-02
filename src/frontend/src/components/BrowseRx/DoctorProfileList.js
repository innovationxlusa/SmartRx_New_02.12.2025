import React, { useEffect, useMemo, useState } from "react";
import useApiClients from "../../services/useApiClients";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import PageTitle from "../static/PageTitle/PageTitle";
import CustomInput from "../static/Commons/CustomInput";
import SearchIcon from "../../assets/img/SearchIcon.svg";
import { useFetchData } from "../../hooks/useFetchData";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import CustomButton from "../static/Commons/CustomButton";
import { getColorForName } from "../../constants/constants";
import "./PatientVitalList.css";

const DoctorProfileList = () => {
    const { api } = useApiClients();
    const userId = useCurrentUserId();
    const location = useLocation();
    const navigate = useNavigate();
    const [search, setSearch] = useState("");
    const [debouncedSearch, setDebouncedSearch] = useState("");
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [sortBy, setSortBy] = useState("alphabeticAsc");

    
    // Get patientId from navigation state (null for "All Patients" view)
    // IMPORTANT:
    // - If patientId is NULL: API returns ALL doctors for userId
    // - If patientId is PROVIDED: API returns only doctors who have treated THIS SPECIFIC patient (userId + patientId combination)
    const patientId = location.state?.patientId ?? null;
    const patientName = location.state?.patientName ?? null; // Get patient name from navigation state

    useEffect(() => {
        const t = setTimeout(() => setDebouncedSearch(search), 300);
        return () => clearTimeout(t);
    }, [search]);

    // Helper functions for sorting
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

    const payload = useMemo(() => {
        if (!userId) {
            return undefined;
        }

        return {
            userId,
            PatientId: patientId ?? null, // Use PatientId (capital P) to match backend API
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
    }, [userId, patientId, debouncedSearch, currentPage, itemsPerPage, sortBy]);
    const { data, isLoading, error, refetch } = useFetchData(
        api.getDoctorProfilesByUserId,
        payload,
    );

    const profiles = Array.isArray(data?.data) ? data.data : [];

    // Derived, sorted, paginated list (client-side)
    const getSortedProfiles = (list) => {
        const arr = [...list];
        if (sortBy === "alphabeticAsc" || sortBy === "alphabeticDesc") {
            arr.sort((a, b) => {
                const aName =
                    `${a?.doctorFirstName || ""} ${a?.doctorLastName || ""}`
                        .trim()
                        .toLowerCase();
                const bName =
                    `${b?.doctorFirstName || ""} ${b?.doctorLastName || ""}`
                        .trim()
                        .toLowerCase();
                if (aName < bName) return sortBy === "alphabeticAsc" ? -1 : 1;
                if (aName > bName) return sortBy === "alphabeticAsc" ? 1 : -1;
                return 0;
            });
        }
        return arr;
    };

    const filtered = profiles.filter((d) =>
        `${d?.doctorFirstName || ""} ${d?.doctorLastName || ""}`
            .toLowerCase()
            .includes((debouncedSearch || "").toLowerCase()),
    );
    const sorted = getSortedProfiles(filtered);
    const totalRecords = sorted.length;
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = Math.min(startIndex + itemsPerPage, totalRecords);
    const pageItems = sorted.slice(startIndex, endIndex);

    // Calculate total pages - ensure at least 1 page
    const totalPages = Math.max(1, Math.ceil(totalRecords / itemsPerPage));

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

    const [expandedIndex, setExpandedIndex] = useState(null);

    const toggleExpand = (id) => {
        setExpandedIndex((prev) => (prev === id ? null : id));
    };

    // Extract first two words to prevent overlap with Switch Patient toggle
    const getShortName = (fullName) => {
        if (!fullName) return null;
        // Get first two words
        const words = fullName.trim().split(/\s+/);
        const shortName = words.slice(0, 1).join(" ");
        return shortName;
    };

    // Dynamic page title based on view mode
    const pageTitle = patientId
        ? patientName
            ? `Doctors for ${getShortName(patientName)}`
            : "Patient's Doctors"
        : "All Doctors";

    return (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle pageName={pageTitle} smallText={true} />
                    <div className="d-flex align-items-center justify-content-between gap-2 mt-3 mb-2">
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
                            </select>
                        </div>
                    </div>

                    <div className="menu-content">
                        {isLoading ? (
                            <p style={{ textAlign: "center", marginTop: 20, fontFamily: "Georama", color: "#65636e" }}>
                                Loading{" "}
                                {patientId
                                    ? "patient's doctors"
                                    : "all doctors"}
                                ...
                            </p>
                        ) : totalRecords === 0 ? (
                            <div
                                style={{
                                    textAlign: "center",
                                    marginTop: 20,
                                    padding: "20px",
                                }}
                            >
                                <p
                                    style={{
                                        fontSize: "16px",
                                        color: "#666",
                                        marginBottom: "10px",
                                        fontFamily: "Georama",
                                        color: "#65636e"
                                    }}
                                >
                                    {patientId
                                        ? `No doctors found for ${patientName || "this patient"}`
                                        : "No doctors found"}
                                </p>
                                <p
                                    style={{
                                        fontSize: "14px",
                                        color: "#999",
                                        fontFamily: "Georama",
                                        color: "#65636e"
                                    }}
                                >
                                    {patientId
                                        ? "This patient hasn't been assigned to any doctors yet."
                                        : "You haven't added any doctors yet."}
                                </p>
                            </div>
                        ) : (
                            pageItems.map((doc) => {
                                // Generate doctor full name and initials
                                const fullName =
                                    `${doc?.doctorFirstName || ""} ${doc?.doctorLastName || ""}`.trim();
                                const initials =
                                    fullName
                                        .split(" ")
                                        .map((n) => n[0] || "")
                                        .join("")
                                        .substring(0, 2)
                                        .toUpperCase() || "D";

                                const backgroundColor =
                                    getColorForName(fullName);

                                return (
                                    <div
                                        key={
                                            doc?.doctorId ||
                                            `${doc?.doctorFirstName}-${doc?.doctorLastName}`
                                        }
                                        className={`patient-list-item ${expandedIndex === (doc?.doctorId ?? doc?.id) ? "expanded" : ""}`}
                                        onClick={() =>
                                            toggleExpand(
                                                doc?.doctorId ?? doc?.id,
                                            )
                                        }
                                    >
                                        <div className="patient-avatar">
                                            {doc?.profilePhotoPath ? (
                                                <img
                                                    src={doc.profilePhotoPath}
                                                    alt={`${doc?.doctorFirstName} ${doc?.doctorLastName}`}
                                                    className="avatar-image"
                                                    onError={(e) => {
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
                                                        doc?.profilePhotoPath
                                                            ? "none"
                                                            : "flex",
                                                    backgroundColor:
                                                        backgroundColor,
                                                    width: "48px",
                                                    height: "48px",
                                                    borderRadius: "50%",
                                                    alignItems: "center",
                                                    justifyContent: "center",
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
                                                    {doc?.doctorTitle}{" "}
                                                    {doc?.doctorFirstName}{" "}
                                                    {doc?.doctorLastName}
                                                </span>
                                            </div>
                                            <div className="patient-info">
                                                <span>Specialty:</span>{" "}
                                                <span>
                                                    {doc?.specialization ||
                                                        "N/A"}
                                                </span>
                                            </div>
                                            <div className="patient-info">
                                                <span>Chamber:</span>{" "}
                                                <span>
                                                    {doc?.chamberName || "N/A"}
                                                </span>
                                            </div>
                                        </div>

                                        <div className="patient-status">
                                            <div className="status-badge-container">
                                                <div
                                                    style={{
                                                        borderRadius: "3px",
                                                        backgroundColor:
                                                            "#008000",
                                                        display: "inline-block",
                                                        padding: "8px",
                                                        color: "white",
                                                        fontWeight: "700",
                                                        fontSize: "10px",
                                                        lineHeight: "96.3%",
                                                        width: "100%",
                                                        textAlign: "center",
                                                        letterSpacing: "0.5px",
                                                        textTransform:
                                                            "uppercase",
                                                        minWidth: "70.25px",
                                                    }}
                                                >
                                                    Smart RX
                                                </div>

                                                <div className="total-count">
                                                    {doc?.smartRxCount || 0}{" "}
                                                    Total
                                                </div>
                                            </div>
                                        </div>

                                        {/* Expanded action buttons */}
                                        <div
                                            className={`button-group-wrapper d-flex justify-content-between gap-2 w-100 ${
                                                expandedIndex ===
                                                (doc?.doctorId ?? doc?.id)
                                                    ? "show mt-0"
                                                    : ""
                                            }`}
                                        >
                                            <CustomButton
                                                type="button"
                                                label="Profile View"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    navigate("/doctorProfile", {
                                                        state: {
                                                            doctorId:
                                                                doc?.doctorId ??
                                                                doc?.id,
                                                        },
                                                    });
                                                }}
                                                width="200px"
                                                height="25px"
                                                textColor="var(--theme-font-color)"
                                                shape="roundedSquare"
                                                borderColor="1px solid var(--theme-font-color)"
                                                backgroundColor="#FAF8FA"
                                                labelStyle={{
                                                    fontSize: "13px",
                                                    fontWeight: "500",
                                                    fontFamily: "Georama",
                                                }}
                                            />
                                            <CustomButton
                                                type="button"
                                                label="Browse Rx"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    const type =
                                                        "smart-prescription-list";
                                                    const prescriptionType =
                                                        "smartrx";
                                                    const owner =
                                                        `${doc?.doctorTitle || ""} ${doc?.doctorFirstName || ""} ${doc?.doctorLastName || ""}`.trim();
                                                    navigate(
                                                        `/browse-rx/${type}`,
                                                        {
                                                            state: {
                                                                UserId: userId ?? 0,
                                                                patientId: null,
                                                                prescriptionType,
                                                                prescriptionOwner:
                                                                    owner ||
                                                                    null,
                                                            },
                                                        },
                                                    );
                                                }}
                                                width="200px"
                                                height="25px"
                                                textColor="var(--theme-font-color)"
                                                shape="roundedSquare"
                                                borderColor="1px solid var(--theme-font-color)"
                                                backgroundColor="#FAF8FA"
                                                labelStyle={{
                                                    fontSize: "13px",
                                                    fontWeight: "500",
                                                    fontFamily: "Georama",
                                                }}
                                            />
                                        </div>
                                    </div>
                                );
                            })
                        )}
                        {/* Pagination Controls */}
                        {totalRecords > 0 && (
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
                                        {`Showing ${startIndex + 1} to ${endIndex} of ${totalRecords} doctors`}
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
                                                fontFamily: "Georama",
                                                color: "#65636e",
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
                                <div className="d-flex justify-content-center align-items-center flex-wrap gap-2 mt-4">
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

export default DoctorProfileList;