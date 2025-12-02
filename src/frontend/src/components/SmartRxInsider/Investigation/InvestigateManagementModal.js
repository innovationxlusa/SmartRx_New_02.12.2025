import { useState, useMemo, useEffect, useRef } from "react";
import CustomModal from "../../static/CustomModal/CustomModal";
import { FaChevronLeft, FaChevronRight, FaChevronDown } from "react-icons/fa";
import DiagnosticComparison from "./DiagnosticComparison";
import CustomInput from "../../static/Commons/CustomInput";
import SearchIcon from "../../../assets/img/SearchIcon2.svg";
import DiagnosticCenter from "../../../assets/img/DiagnosticCenter.png";
import DiagnosticShimmer from "./DiagnosticShimmer";
import InvestigationTestInformation from "./InvestigationTestInformation";
import useToastMessage from "../../../hooks/useToastMessage";

const InvestigateManagementModal = ({
    isOpen,
    modalType,
    onClose,
    data = {},
    isLoading,
    error,
    currentPage,
    setCurrentPage,
    itemsPerPage,
    setItemsPerPage,
    sortBy,
    setSortBy,
    sortDirection,
    setSortDirection,
    testCenterName,
    setTestCenterName,
    onParamsChange,
    userSelectedListData,
    doctorRecommendedTestListData,
    wishedListData,
    smartRxInsiderData,
    testCenterListRefetch,
    selectedInvestigationId,
    smartRxInsiderDataRefetch,
}) => {
    const [selectedDiagnosticIds, setSelectedDiagnosticIds] = useState(
        new Set(),
    );
    const [showComparison, setShowComparison] = useState(false);
    const [isTestCenterWishedMinimumOne, setIsTestCenterWishedMinimumOne] =
        useState(false);
    const [selectedTests, setSelectedTests] = useState({});
    const [failedImages, setFailedImages] = useState(new Set());

    // Use a ref to track if we're in the middle of a refetch
    const isRefetching = useRef(false);

    const showToast = useToastMessage();
    const getCenterId = (item) => item.diagnosticTestCenterId;

    // Reconstruct selectedDiagnostics from IDs and current data
    const selectedDiagnostics = useMemo(() => {
        if (!data?.comparedTestList?.data) return [];

        return Array.from(selectedDiagnosticIds)
            .map((centerId) =>
                data.comparedTestList.data.find(
                    (item) => getCenterId(item) === centerId,
                ),
            )
            .filter(Boolean);
    }, [selectedDiagnosticIds, data?.comparedTestList?.data]);

    // Enhanced refetch function that preserves selections
    const handleRefetch = async () => {
        isRefetching.current = true;
        try {
            await testCenterListRefetch();
        } finally {
            // Small timeout to ensure state updates are processed
            setTimeout(() => {
                isRefetching.current = false;
            }, 100);
        }
    };

    const handleToggle = (centerId) => {
        const newSelectedIds = new Set(selectedDiagnosticIds);

        if (newSelectedIds.has(centerId)) {
            newSelectedIds.delete(centerId);
        } else {
            if (newSelectedIds.size >= 3) {
                showToast(
                    "error",
                    "You can only select up to 3 diagnostics.",
                    "",
                );
                return;
            }
            newSelectedIds.add(centerId);
        }

        setSelectedDiagnosticIds(newSelectedIds);
        setSelectedTests(data?.comparedTestList?.data);
    };

    const handleChangeDiagnosticCenter = () => setShowComparison(true);

    const totalPages = Math.max(
        1,
        Math.ceil(
            (data?.testCentersListWithBranch?.totalRecords || 0) / itemsPerPage,
        ),
    );

    const visibleCenters = useMemo(() => {
        return data?.testCentersListWithBranch?.data || [];
    }, [data]);

    useEffect(() => {
        if (currentPage > totalPages) {
            setCurrentPage(totalPages);
        }
    }, [currentPage, totalPages, setCurrentPage]);

    useEffect(() => {
        if (!isOpen) {
            setSelectedDiagnosticIds(new Set());
            setShowComparison(false);
            setCurrentPage(1);
            setSortBy("alphabeticAsc");
            setSortDirection("asc");
            setTestCenterName("");
        }
    }, [
        isOpen,
        setCurrentPage,
        setSortBy,
        setSortDirection,
        setTestCenterName,
    ]);

    useEffect(() => {
        if (onParamsChange && isOpen) {
            onParamsChange({
                currentPage,
                itemsPerPage,
                sortBy,
                sortDirection,
                testCenterName,
            });
        }
    }, [
        currentPage,
        itemsPerPage,
        sortBy,
        sortDirection,
        testCenterName,
        onParamsChange,
        isOpen,
    ]);

    // Enhanced auto-selection that doesn't override user selections during refetch
    useEffect(() => {
        if (
            isOpen &&
            doctorRecommendedTestListData?.length &&
            data?.comparedTestList?.data?.length &&
            !isRefetching.current
        ) {
            const recommendedCenters = doctorRecommendedTestListData.filter(
                (item) => item.isDoctorRecommended || item.isUserSelected,
            );

            const uniqueCenterIds = new Set(selectedDiagnosticIds); // Start with current selections

            recommendedCenters.forEach((item) => {
                const centerId = item.diagnosticTestCenterId;
                const centerExists = data?.comparedTestList?.data?.some(
                    (d) => getCenterId(d) === centerId,
                );

                if (centerExists) {
                    uniqueCenterIds.add(centerId);
                }
            });

            // Update selectedDiagnosticIds if there are new selections
            if (uniqueCenterIds.size !== selectedDiagnosticIds.size) {
                setSelectedDiagnosticIds(uniqueCenterIds);
            }
            
            // Always set selectedTests to ensure price comparison data is available
            if (data?.comparedTestList?.data && uniqueCenterIds.size > 0) {
                setSelectedTests(data?.comparedTestList?.data);
            }
        }
    }, [isOpen, doctorRecommendedTestListData, data?.comparedTestList?.data]);

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

    const handleImageError = (centerId, imageUrl) => {
        setFailedImages((prev) => new Set(prev).add(imageUrl));
    };

    const getDiagnosticIcon = (center) => {
        const centerId = getCenterId(center);
        const imageUrl = center?.diagnosticTestCenterIcon;

        if (failedImages.has(imageUrl)) {
            return DiagnosticCenter;
        }

        if (
            imageUrl &&
            imageUrl !== "null" &&
            imageUrl !== "" &&
            imageUrl !== "undefined"
        ) {
            return imageUrl;
        }

        return DiagnosticCenter;
    };

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={
                showComparison
                    ? "Change Diagnostic Center"
                    : modalType === "test information"
                      ? "Test Information"
                      : "Compare Test Price"
            }
            close={onClose}
            animationDirection="bottom"
            modalSize="medium"
            position="middle"
            closeOnOverlayClick={false}
            form
            dataPreview
        >
            <div className="compare-drug-wrapper">
                <div className="fade-in">
                    {modalType != "test information" && (
                        <div className="d-flex justify-content-end">
                            <div className="compare-drug-count-wrapper">
                                <div className="compare-drug-count">
                                    {data?.testCentersListWithBranch
                                        ?.totalRecords || 0}
                                    &nbsp;Total
                                </div>
                            </div>
                        </div>
                    )}
                    <div className="compare-drug-count-border-bottom mb-2" />

                    {showComparison ? (
                        <div className="diagnostic-comparison-modal">
                            <DiagnosticComparison
                                smartRxInsiderData={smartRxInsiderData}
                                selectedDiagnostics={selectedDiagnostics}
                                selectedTests={selectedTests}
                                setSelectedTests={setSelectedTests}
                                onReturn={() => setShowComparison(false)}
                                testCenterListRefetch={handleRefetch} // Use the enhanced refetch
                                smartRxInsiderDataRefetch={
                                    smartRxInsiderDataRefetch
                                }
                            />
                        </div>
                    ) : modalType === "test information" ? (
                        <InvestigationTestInformation
                            data={data}
                            selectedInvestigationId={selectedInvestigationId}
                            isLoading={isLoading}
                            error={error}
                        />
                    ) : (
                        <div className="diagnostic-comparison-modal">
                            <div className="d-flex justify-content-between align-items-center mb-3">
                                <p className="mb-0 small text-start w-50">
                                    Info: Please select diagnostic center(s) for
                                    comparison
                                </p>
                                <div className="d-flex align-items-center gap-2">
                                    <div
                                        className="position-relative"
                                        style={{ maxWidth: 165 }}
                                    >
                                        <select
                                            className="form-select form-select-sm pe-4"
                                            value={sortBy}
                                            onChange={(e) => {
                                                setSortBy(e.target.value);
                                                setCurrentPage(1);
                                            }}
                                            style={{
                                                backgroundColor: "#E9E7FA",
                                                appearance: "none",
                                                cursor: "pointer",
                                            }}
                                        >
                                            <option value="lowToHigh">
                                                Price: Low to High
                                            </option>
                                            <option value="highToLow">
                                                Price: High to Low
                                            </option>
                                            <option value="alphabeticAsc">
                                                Established year: A to Z
                                            </option>
                                            <option value="alphabeticDesc">
                                                Established year: Z to A
                                            </option>
                                            <option value="alphabeticAsc">
                                                Alphabetic: A to Z
                                            </option>
                                            <option value="alphabeticDesc">
                                                Alphabetic: Z to A
                                            </option>
                                            <option value="alphabeticAsc">
                                                Year Established: A to Z
                                            </option>
                                            <option value="alphabeticDesc">
                                                Year Established: Z to A
                                            </option>
                                        </select>
                                        <FaChevronDown
                                            className="position-absolute top-50 end-0 translate-middle-y me-2 text-secondary"
                                            style={{ pointerEvents: "none" }}
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="mb-3">
                                <CustomInput
                                    className="input-style"
                                    rightIcon={SearchIcon}
                                    placeholder="Search"
                                    value={testCenterName || ""}
                                    borderColor="var(--app-theme-color)"
                                    minHeight={0}
                                    onChange={(e) => {
                                        setTestCenterName(e.target.value);
                                        setCurrentPage(1);
                                    }}
                                />
                            </div>
                            {isLoading && <DiagnosticShimmer count={6} />}

                            {error && visibleCenters.length === 0 && (
                                <div className="text-danger py-5 text-center">
                                    {error}
                                </div>
                            )}
                            {!isLoading &&
                                !error &&
                                visibleCenters.length > 0 && (
                                    <>
                                        <div className="diagnostic-list">
                                            {visibleCenters.map(
                                                (center, index) => {
                                                    const centerId =
                                                        getCenterId(center);
                                                    const imageSrc =
                                                        getDiagnosticIcon(
                                                            center,
                                                        );

                                                    return (
                                                        <div
                                                            key={index}
                                                            className={`d-flex align-items-center justify-content-between px-1 py-2 ${
                                                                visibleCenters.length !==
                                                                index + 1
                                                                    ? " border-bottom"
                                                                    : ""
                                                            }`}
                                                        >
                                                            <div
                                                                className="diagnostic-item-content d-flex align-items-center gap-2 flex-grow-1"
                                                                style={{
                                                                    width: "100%",
                                                                }}
                                                            >
                                                                <input
                                                                    type="checkbox"
                                                                    checked={selectedDiagnosticIds.has(
                                                                        getCenterId(
                                                                            center,
                                                                        ),
                                                                    )}
                                                                    onChange={() =>
                                                                        handleToggle(
                                                                            getCenterId(
                                                                                center,
                                                                            ),
                                                                        )
                                                                    }
                                                                    style={{
                                                                        minWidth:
                                                                            "5%",
                                                                        width: "5%",
                                                                    }}
                                                                />
                                                                <div
                                                                    className="diagnostic-logo"
                                                                    style={{
                                                                        width: "10%",
                                                                    }}
                                                                >
                                                                    <img
                                                                        src={
                                                                            imageSrc
                                                                        }
                                                                        alt={
                                                                            center.diagnosticTestCenterName
                                                                        }
                                                                        width={
                                                                            30
                                                                        }
                                                                        height={
                                                                            30
                                                                        }
                                                                        onError={() =>
                                                                            handleImageError(
                                                                                centerId,
                                                                                center.diagnosticTestCenterIcon,
                                                                            )
                                                                        }
                                                                    />
                                                                </div>
                                                                <div
                                                                    className="diagnostic-name flex-grow-1 text-start"
                                                                    style={{
                                                                        width: "50%",
                                                                        whiteSpace:
                                                                            "normal",
                                                                        wordBreak:
                                                                            "break-word",
                                                                    }}
                                                                >
                                                                    <strong>
                                                                        {center.diagnosticTestCenterName ||
                                                                            "—"}
                                                                    </strong>
                                                                </div>
                                                                <div
                                                                    className="diagnostic-branch text-muted small d-flex flex-column text-start"
                                                                    style={{
                                                                        width: "35%",
                                                                    }}
                                                                >
                                                                    <span>
                                                                        {center.diagnosticTestCenterBranchName ||
                                                                            "—"}
                                                                    </span>
                                                                    <span>
                                                                        {center.diagnosticTestCenterBranchLocation ||
                                                                            "—"}
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    );
                                                },
                                            )}
                                        </div>

                                        <div className="d-flex justify-content-center gap-2 flex-wrap mt-4">
                                            <button
                                                className="btn btn-light btn-sm"
                                                style={{
                                                    fontFamily: "Georama",
                                                }}
                                                disabled={currentPage === 1}
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
                                                            fontFamily:
                                                                "Georama",
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
                                                style={{
                                                    fontFamily: "Georama",
                                                }}
                                                disabled={
                                                    currentPage === totalPages
                                                }
                                                onClick={() =>
                                                    setCurrentPage((p) => p + 1)
                                                }
                                            >
                                                Next
                                                <FaChevronRight className="ms-1" />
                                            </button>
                                        </div>

                                        {selectedDiagnostics.length > 0 && (
                                            <>
                                                <div className="mt-3">
                                                    {selectedDiagnostics.map(
                                                        (item) => (
                                                            <div
                                                                key={getCenterId(
                                                                    item,
                                                                )}
                                                                className="d-flex justify-content-between align-items-center border p-2 mb-2 rounded"
                                                            >
                                                                <span
                                                                    className="text-start"
                                                                    style={{
                                                                        width: "50%",
                                                                    }}
                                                                >
                                                                    {
                                                                        item.diagnosticTestCenterName
                                                                    }
                                                                </span>
                                                                <div
                                                                    className="diagnostic-branch text-muted small d-flex flex-column text-start"
                                                                    style={{
                                                                        width: "40%",
                                                                    }}
                                                                >
                                                                    <span>
                                                                        {
                                                                            item.diagnosticTestCenterBranchName
                                                                        }
                                                                    </span>
                                                                    <span>
                                                                        {
                                                                            item.diagnosticTestCenterBranchLocation
                                                                        }
                                                                    </span>
                                                                </div>
                                                                <button
                                                                    className="btn btn-link text-danger p-0"
                                                                    style={{
                                                                        width: "10%",
                                                                    }}
                                                                    onClick={() =>
                                                                        handleToggle(
                                                                            getCenterId(
                                                                                item,
                                                                            ),
                                                                        )
                                                                    }
                                                                >
                                                                    ❌
                                                                </button>
                                                            </div>
                                                        ),
                                                    )}
                                                </div>
                                                <button
                                                    className="investigation-action-btn mt-2 mb-4"
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        e.nativeEvent.stopImmediatePropagation();
                                                        handleChangeDiagnosticCenter();
                                                    }}
                                                >
                                                    Compare
                                                </button>
                                            </>
                                        )}
                                    </>
                                )}
                        </div>
                    )}
                </div>
            </div>
        </CustomModal>
    );
};

export default InvestigateManagementModal;
