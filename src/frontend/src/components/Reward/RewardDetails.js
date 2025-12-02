import React, { useState, useMemo, useCallback } from "react";
import "./RewardDetails.css";
import { useNavigate, useLocation } from "react-router-dom";
import PageTitle from "../static/PageTitle/PageTitle";
import useApiClients from "../../services/useApiClients";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import { useFetchData } from "../../hooks/useFetchData";
import rewardIcon from "../../assets/img/GoldMedalList.svg";
import DateField from "../static/Commons/DateField";
import { VscSearch } from "react-icons/vsc";
import CustomButton from "../static/Commons/CustomButton";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";


const RewardDetails = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { api } = useApiClients();
    const loginUserId = useCurrentUserId();
    
    // Get PatientId from location state if available
    const patientId = location?.state?.patientId || location?.state?.PatientId || null;

    const [formData, setFormData] = useState({
        fromDate: "",
        toDate: ""
    });

    const [isLoading, setIsLoading] = useState(false);

    // Pagination state
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);

    // State for filter tabs (both enabled by default)
    const [selectedFilters, setSelectedFilters] = useState({
        earned: true,
        consumed: true
    });

    // Determine filter type based on selected filters
    const filterType = useMemo(() => {
        if (selectedFilters.earned && selectedFilters.consumed) {
            return "all"; // Both enabled = show all
        } else if (selectedFilters.earned && !selectedFilters.consumed) {
            return "earned"; // Only earned enabled
        } else if (!selectedFilters.earned && selectedFilters.consumed) {
            return "consumed"; // Only consumed enabled
        }
        return null; // Both disabled = no data
    }, [selectedFilters]);

    // Create payload for API call
    const apiPayload = useMemo(() => {
        if (!loginUserId || !filterType) {
            return null; // Don't call API if userId is not available or both filters are disabled
        }

        return {
            UserId: Number(loginUserId),
            PatientId: patientId ? Number(patientId) : null,
            FilterType: filterType, // "all", "earned", or "consumed"
            StartDate: formData.fromDate || null,
            EndDate: formData.toDate || null,
            pagingSorting: {
                pageNumber: currentPage,
                pageSize: itemsPerPage,
                sortBy: "CreatedDate",
                sortDirection: "desc"
            }
        };
    }, [loginUserId, patientId, filterType, formData.fromDate, formData.toDate, currentPage, itemsPerPage]);

    const handleSubmitSearch = async () => { 
        setIsLoading(true);
        setCurrentPage(1); // Reset to first page on search
        try {
            // Refetch data with current form data and filter (apiPayload will be updated automatically)
            await refetchRewardDetails();
        } catch (error) {
            console.error("Error fetching reward details:", error);
        } finally {
            setIsLoading(false);
        }
    };

    // Fetch reward point details/transactions
    const {
        data: rewardDetailsData,
        isLoading: isRewardDetailsLoading,
        error: rewardDetailsError,
        refetch: refetchRewardDetails,
    } = useFetchData(
        api.getPatientRewardsByUserIdAndPatientId,
        null,
        null,
        null,
        null,
        apiPayload,
    );

    // Process API response data based on FilterType
    const transactions = useMemo(() => {
        // If both filters are disabled, return empty array
        if (!filterType) {
            return [];
        }

        if (!rewardDetailsData) {
            return [];
        }

        // Extract data based on FilterType
        let dataArray = null;
        
        if (filterType === "all") {
            // Get data from rewardDetailsData?.all?.data
            dataArray = rewardDetailsData?.all?.data;
        } else if (filterType === "earned") {
            // Get data from rewardDetailsData?.earned?.data
            dataArray = rewardDetailsData?.earned?.data;
        } else if (filterType === "consumed") {
            // Get data from rewardDetailsData?.consumed?.data
            dataArray = rewardDetailsData?.consumed?.data;
        }

        // Fallback: Handle different possible response structures if above paths don't exist
        if (!dataArray || !Array.isArray(dataArray)) {
            if (Array.isArray(rewardDetailsData)) {
                dataArray = rewardDetailsData;
            } else if (Array.isArray(rewardDetailsData?.data)) {
                dataArray = rewardDetailsData.data;
            } else if (Array.isArray(rewardDetailsData?.response?.data)) {
                dataArray = rewardDetailsData.response.data;
            } else if (Array.isArray(rewardDetailsData?.response)) {
                dataArray = rewardDetailsData.response;
            }
        }

        if (!dataArray || !Array.isArray(dataArray)) {
            return [];
        }

        // Map and normalize transaction data
        return dataArray.map((transaction) => ({
            id: transaction.id || transaction.Id || transaction.rewardTransactionId || transaction.RewardTransactionId,
            rewardName: transaction.rewardName || transaction.RewardName || transaction.activityHeader || transaction.ActivityHeader || "",
            totalPoints: Number(transaction.totalPoints || transaction.TotalPoints || transaction.points || transaction.Points || 0),
            earnedNonCashablePoints: Number(transaction.earnedNonCashablePoints || transaction.EarnedNonCashablePoints || 0),
            consumedNonCashablePoints: Number(transaction.consumedNonCashablePoints || transaction.ConsumedNonCashablePoints || 0),
            createdDate: transaction.createdDate || transaction.CreatedDate || transaction.date || transaction.Date || "",
            transactionType: transaction.transactionType || transaction.TransactionType || transaction.type || transaction.Type || "",
            isEarned: transaction.isEarned !== undefined ? transaction.isEarned : (transaction.IsEarned !== undefined ? transaction.IsEarned : (transaction.totalPoints > 0 || transaction.TotalPoints > 0)),
            isConsumed: transaction.isConsumed !== undefined ? transaction.isConsumed : (transaction.IsConsumed !== undefined ? transaction.IsConsumed : (transaction.totalPoints < 0 || transaction.TotalPoints < 0)),
            isDeduct: transaction.isDeduct !== undefined ? transaction.isDeduct : (transaction.isDeduct !== undefined ? transaction.isDeduct : false),
        }));
    }, [rewardDetailsData, filterType]);

    // Get total records from API response based on FilterType
    const totalRecords = useMemo(() => {
        // If both filters are disabled, return 0
        if (!filterType) return 0;
        
        if (!rewardDetailsData) return 0;
        
        // Get totalRecords from the correct path based on FilterType
        let total = 0;
        
        if (filterType === "all") {
            total = rewardDetailsData?.all?.totalRecords || 
                    rewardDetailsData?.all?.totalCount ||
                    rewardDetailsData?.all?.data?.length;
        } else if (filterType === "earned") {
            total = rewardDetailsData?.earned?.totalRecords || 
                    rewardDetailsData?.earned?.totalCount ||
                    rewardDetailsData?.earned?.data?.length;
        } else if (filterType === "consumed") {
            total = rewardDetailsData?.consumed?.totalRecords || 
                    rewardDetailsData?.consumed?.totalCount ||
                    rewardDetailsData?.consumed?.data?.length;
        }
        
        // Fallback to other possible paths
        if (!total) {
            total = rewardDetailsData?.totalRecords || 
                   rewardDetailsData?.totalCount || 
                   rewardDetailsData?.response?.totalRecords || 
                   rewardDetailsData?.response?.totalCount ||
                   transactions.length; // Fallback to array length if totalRecords not available
        }
        
        return total;
    }, [rewardDetailsData, filterType, transactions.length]);

    // Ensure transactions is always an array
    const safeTransactions = useMemo(() => {
        return Array.isArray(transactions) ? transactions : [];
    }, [transactions]);

    // Calculate total points based on current page transactions
    // Note: This calculates points for the current page only
    // For total points across all pages, you may need a separate API call
    const totalPoints = useMemo(() => {
        return safeTransactions.reduce((total, transaction) => {
            return total + (Number(transaction?.totalPoints) || 0);
        }, 0);
    }, [safeTransactions]);

    // Handle filter tab toggle
    const handleFilterToggle = useCallback((filterType) => {
        setSelectedFilters(prev => {
            const newState = {
                ...prev,
                [filterType]: !prev[filterType]
            };
            // Prevent both buttons from being disabled
            if (!newState.earned && !newState.consumed) {
                // If trying to disable the last enabled button, don't allow it
                return prev;
            }
            return newState;
        });
        setCurrentPage(1); // Reset to first page when filter changes
        // Note: No need to manually refetch here - useFetchData will automatically
        // refetch when apiPayload changes (which depends on filterType)
    }, []);

    // Pagination helper function - same as DoctorProfileList
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

    // Calculate pagination values
    const totalPages = Math.max(1, Math.ceil(totalRecords / itemsPerPage));
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = Math.min(startIndex + itemsPerPage, totalRecords);

    // Format date for display
    const formatDateForDisplay = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-GB', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    };

    // Format date for input
    const formatDateForInput = (dateString) => {
        if (!dateString) return "";
        const date = new Date(dateString);
        return date.toISOString().split('T')[0];
    };

    return (
        <div className="reward-details-container">
            <div className="reward-details-content">
                <PageTitle
                    pageName={"Reward Point Details"}
                    switchButton={false}
                    lowMargin={true}
                />

                <div
                    style={{
                        display: "flex",
                        justifyContent: "right",
                    }}
                >
                    <img
                        src={rewardIcon}
                        alt="Reward Badge"
                        onClick={() => navigate("/rewardPointBadges")}
                        style={{
                            width: "50px",
                            height: "50px",
                            cursor: "pointer",
                        }}
                    />
                </div>

                {/* Total Points Section */}
                <div className="total-points-section">
                    <div className="total-points-content">
                        <span className="total-points-label">Total Point</span>
                        <span className="total-points-value">
                            {totalPoints}
                        </span>
                    </div>
                </div>

                {/* Filter Tabs Section */}
                <div className="filter-tabs-section">
                    <div className="filter-tabs">
                        <button
                            className={`filter-tab ${selectedFilters.earned ? "active" : ""}`}
                            onClick={() => handleFilterToggle("earned")}
                        >
                            Earned
                        </button>
                        <button
                            className={`filter-tab ${selectedFilters.consumed ? "active" : ""}`}
                            onClick={() => handleFilterToggle("consumed")}
                        >
                            Consumed
                        </button>
                    </div>
                   
                </div>

                <div
                    className="point-select-date"
                    style={{
                        display: "flex",
                        gap: "10px",
                        marginBottom: "15px",
                    }}
                >
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
                        />
                    </div>
                    <div className="date-container" style={{ width: "50%" }}>
                        <DateField
                            className="toDate"
                            placeholderText="To date"
                            type="text"
                            style={{ height: "40px" }}
                            value={formData.toDate}
                            onChange={(selectedDateObj) => {
                                setFormData((prev) => ({
                                    ...prev,
                                    toDate: selectedDateObj,
                                }));
                            }}
                        />
                    </div>
                </div>

                <div style={{ display: "flex", justifyContent: "center" }}>
                    <CustomButton
                        type="button"
                        label={
                            <div
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                    fontFamily: "Georama",
                                    fontSize: "12px",
                                    fontWeight: "600",
                                    verticalAlign: "middle",
                                    justifyContent: "center",
                                }}
                            >
                                {isLoading ? "Searching..." : "Search"}
                                <span className="search-img">
                                    <VscSearch
                                        style={{
                                            color: "#4b3b8b",
                                            height: "12px",
                                            width: "12px",
                                        }}
                                    />
                                </span>
                            </div>
                        }
                        className="search-action-btn"
                        width="30%"
                        textColor="#4b3b8b"
                        backgroundColor="#FAF8FA"
                        borderRadius="4px"
                        shape="pill"
                        borderColor="1px solid #4b3b8b"
                        labelStyle={{
                            fontSize: "16px",
                            fontWeight: "500",
                            textTransform: "capitalize",
                        }}
                        hoverEffect="theme"
                        onClick={() => {
                            handleSubmitSearch();
                        }}
                        disabled={isLoading}
                    />
                </div>

                {/* Transaction List Section */}
                <div className="transaction-list-section">
                    {isRewardDetailsLoading || isLoading ? (
                        <div className="loading-container">
                            <div className="spinner-border" role="status" style={{ color: "#4b3b8b" }}>
                                <span className="visually-hidden">Loading...</span>
                            </div>
                            <p style={{ marginTop: "10px", fontFamily: "Georama" }}>Loading transactions...</p>
                        </div>
                    ) : rewardDetailsError ? (
                        <div className="error-container" style={{ fontFamily: "Georama" }}>
                            <h5>⚠️ Error Loading Transactions</h5>
                            <p>{rewardDetailsError?.message || "Failed to load transaction data. Please try again."}</p>
                            <CustomButton
                                type="button"
                                label="Retry"
                                onClick={refetchRewardDetails}
                                width="auto"
                                height="35px"
                                textColor="#fff"
                                backgroundColor="var(--app-theme-color)"
                                borderRadius="4px"
                                labelStyle={{ fontSize: "14px", fontWeight: "500" }}
                            />
                        </div>
                    ) : !filterType ? (
                        <div className="empty-container" style={{ fontFamily: "Georama" }}>
                            <p>Please enable at least one filter (Earned or Consumed) to view transactions.</p>
                        </div>
                    ) : !Array.isArray(safeTransactions) || safeTransactions.length === 0 ? (
                        <div className="empty-container" style={{ fontFamily: "Georama" }}>
                            <p>No transactions found for the selected filter.</p>
                        </div>
                    ) : (
                        <>
                            <div className="transaction-list">
                                {safeTransactions.map((transaction, index) => (
                                    <div
                                        key={`${transaction?.id ?? transaction?.rewardId ?? transaction?.createdDate ?? "tx"}-${index}`}
                                        className="transaction-item"
                                    >
                                        <div className="transaction-info">
                                            <div className="transaction-action">
                                                {transaction.rewardName}
                                            </div>
                                            <div className="transaction-date">
                                                {transaction.createdDate ? formatDateForDisplay(transaction.createdDate) : "N/A"}
                                            </div>
                                        </div>
                                        <div
                                            className={`transaction-points ${
                                                filterType === "consumed" || (filterType === "all" && transaction.isDeduct)
                                                    ? "negative"
                                                    : "positive"
                                            }`}
                                        >
                                            {filterType === "consumed" || (filterType === "all" && transaction.isDeduct)
                                                ? "-"
                                                : "+"
                                            }
                                            {filterType === "consumed"
                                                ? Math.abs(transaction.consumedNonCashablePoints || 0)
                                                : filterType === "earned"
                                                ? Math.abs(transaction.earnedNonCashablePoints || 0)
                                                : transaction.isDeduct
                                                ? Math.abs(transaction.consumedNonCashablePoints || 0)
                                                : Math.abs(transaction.earnedNonCashablePoints || 0)
                                            }
                                            {console.log('Transaction Points:', transaction)}
                                        </div>
                                    </div>
                                ))}
                            </div>

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
                                            {`Showing ${startIndex + 1} to ${endIndex} of ${totalRecords} transactions`}
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
                                                    setItemsPerPage(Number(e.target.value));
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
                                            onClick={() => setCurrentPage((p) => p - 1)}
                                        >
                                            <FaChevronLeft className="me-1" />
                                            Prev
                                        </button>

                                        {getPageNumbers(currentPage, totalPages).map((page, idx) =>
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
                                                    onClick={() => setCurrentPage(page)}
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
                                            onClick={() => setCurrentPage((p) => p + 1)}
                                        >
                                            Next
                                            <FaChevronRight className="ms-1" />
                                        </button>
                                    </div>
                                </div>
                            )}
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

export default RewardDetails;
