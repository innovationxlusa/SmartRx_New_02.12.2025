import { useState, useMemo, useEffect } from "react";
import DrugInformation from "./DrugInformation";
import CustomModal from "../../static/CustomModal/CustomModal";
import MedicineCompareShimmer from "./MedicineCompareShimmer";
import { ReactComponent as PriceComparison } from "../../../assets/img/PriceComparison.svg";
import {
    FaArrowDown,
    FaArrowUp,
    FaEquals,
    FaArrowsAltH,
    FaHeart,
    FaChevronLeft,
    FaChevronRight,
    FaChevronDown,
} from "react-icons/fa";
import CustomButton from "../../static/Commons/CustomButton";
import useApiClients from "../../../services/useApiClients";
import { BsFillBookmarkFill } from "react-icons/bs";
import RewardPopup from "../../Reward/RewardPopup";

const MedicineManagementModal = ({
    isOpen,
    modalType,
    onClose,
    data,
    isLoading,
    error,
    currentPage,
    setCurrentPage,
    sortBy,
    setSortBy,
    itemsPerPage,
    setItemsPerPage,
    totalDosageQty,
    smartRxInsiderData,
    refetch,
    smartRxInsiderDataRefetch
}) => {
    const { api } = useApiClients();
    const comparedList = data?.comparedMedicine?.data ?? [];

    const [qty, setQty] = useState(1);
    const [showTotal, setShowTotal] = useState(false);
    const [selectedCard, setSelectedCard] = useState(null);
    const [wishlist, setWishlist] = useState([]);
    const [isWishListLoading, setIsWishListLoading] = useState(false);
    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);
    const [favorite, setFavorite] = useState(() =>
        comparedList
            .filter((medicine) => medicine.wished)
            .map((medicine) => medicine.medicineId)
    );
    
    useEffect(() => {
        if (comparedList.length && !selectedCard) {
            setSelectedCard(data?.sourceMedicine ?? comparedList[0]);
        }
    }, [comparedList, data?.sourceMedicine, selectedCard]);

    const sortedCards = useMemo(() => {
        const sorted = [...comparedList];
        switch (sortBy) {
            case "lowToHigh":
                return sorted.sort(
                    (a, b) => a.unitPriceValue - b.unitPriceValue
                );
            case "highToLow":
                return sorted.sort(
                    (a, b) => b.unitPriceValue - a.unitPriceValue
                );
            case "alphabeticAsc":
                return sorted.sort((a, b) =>
                    a.medicineName.localeCompare(b.medicineName)
                );
            case "alphabeticDesc":
                return sorted.sort((a, b) =>
                    b.medicineName.localeCompare(a.medicineName)
                );
            default:
                return sorted;
        }
    }, [comparedList, sortBy]);

    // Filter out the selected card from the displayable cards
    const displayableCards = useMemo(() => {
        if (!selectedCard) return sortedCards;
        return sortedCards.filter(
            (card) => card.medicineId !== selectedCard.medicineId
        );
    }, [sortedCards, selectedCard]);

    const totalPages = useMemo(() => {
        if (displayableCards.length === 0) return 1;
        return Math.ceil(displayableCards.length / itemsPerPage);
    }, [displayableCards, itemsPerPage]);

    // Adjust currentPage if it exceeds totalPages
    useEffect(() => {
        if (currentPage > totalPages && totalPages > 0) {
            setCurrentPage(totalPages);
        }
    }, [currentPage, totalPages, setCurrentPage]);

    useEffect(() => {
        if (
            selectedCard &&
            !sortedCards.find((c) => c.medicineId === selectedCard.medicineId)
        ) {
            setSelectedCard(sortedCards[0]);
        }
    }, [sortedCards, selectedCard]);

    const visibleCards = useMemo(() => {
        const start = (currentPage - 1) * itemsPerPage;
        const end = start + itemsPerPage;
        const paginated = displayableCards.slice(start, end);
        return paginated;
    }, [displayableCards, currentPage, itemsPerPage]);

    const priceLabel = showTotal ? "Total Price" : "Unit Price";
    const priceValue = selectedCard
        ? (showTotal
            ? selectedCard.unitPriceValue * qty
            : selectedCard.unitPriceValue
        ).toFixed(2)
        : "0.00";

    const toggleWishlist = (id) => {
        setWishlist((prev) =>
            prev.includes(id) ? prev.filter((i) => i !== id) : [...prev, id]
        );
    };

    const TrendDisplay = ({ basePrice, comparePrice, multiplier = 1 }) => {
        if (basePrice == null || comparePrice == null) return null;

        const base = basePrice * multiplier;
        const compare = comparePrice * multiplier;
        const diff = compare - base;
        let symbol = "";
        let color = "";
        let text = "";
        let Icon = null;

        const equal = () => {

            return (
                <span className="color-blue d-block my-2">
                    No loss<br />or saving
                </span>
            );
        }

        const pct = Math.abs((diff / base) * 100);
        if (diff > 0) {
            Icon = FaArrowUp;
            symbol = "▲"; // Up arrow
            color = "color-green";
            text = "Saved";
        } else if (diff < 0) {
            Icon = FaArrowDown;
            symbol = "▼"; // Down arrow
            color = "color-red";
            text = "Extra cost";
        } else {
            Icon = FaArrowsAltH;
            symbol = "↔"; // Horizontal bar for no change
            color = "color-blue";
            text = equal();
        }

        return (
            <div
                className={`d-flex flex-column justify-content-center align-items-center ${color}`}
                style={{ width: "20%" }}
            >
                <Icon />
                {/* {symbol} */}
                <span className={`${color} d-block my-2 fw-semibold`}>
                    {pct.toFixed(1)}%
                </span>
                <span className={color}>৳ {Math.abs(diff).toFixed(2)}</span>
                <span className={color}>{text}</span>
            </div>
        );
    };

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

    const handleSaveAllWishlist = async () => {
        setUploadResponse(null);
        setShowRewardPopup(false);

        try {
            setIsWishListLoading(true);
            const newData = {
                SmartRxMasterId: smartRxInsiderData?.smartRxId,
                PrescriptionId:
                    smartRxInsiderData?.prescriptions[0].prescriptionId,
                SourceMedicineId: data?.sourceMedicine.medicineId, // or handle multiple IDs if needed
                WishListIds: favorite, // send all selected favorites as array
                LoginUserId: smartRxInsiderData?.userId,
            };
            
            const response = await api.updateMedicineWishList(newData, "");

            setUploadResponse(response);

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (
                    response?.response?.isRewardUpdated == true
                ) {
                    console.log("Reward points updated:", response?.response?.totalRewardPoints);
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        refetch && refetch();
                        smartRxInsiderDataRefetch && smartRxInsiderDataRefetch();
                        onClose();
                    }, 3000);
                } else {
                    refetch && refetch();
                    smartRxInsiderDataRefetch && smartRxInsiderDataRefetch();
                }

                //refetch && refetch();
                //smartRxInsiderDataRefetch && smartRxInsiderDataRefetch();
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsWishListLoading(false);
        }
    };

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={
                modalType === "add" ? "Compare Drug Price" : "Drug Information"
            }
            close={onClose}
            animationDirection="bottom"
            modalSize="small"
            position="middle"
            modalNameStyle={{
                fontFamily: "Georama",
                color: "#4b3b8b",
            }}
            closeOnOverlayClick={false}
            form
            dataPreview
        >
            <div className="compare-drug-wrapper">
                <div className="fade-in">
                    <div className="compare-drug-count-border-bottom mb-2" />
                    {modalType === "dragInfo" ? (
                        <DrugInformation
                            data={data}
                            isLoading={isLoading}
                            error={error}
                        />
                    ) : (
                        <>
                            {isLoading && (
                                <div
                                    className="my-4 h-100"
                                    style={{ minHeight: "450px" }}
                                >
                                    <MedicineCompareShimmer />
                                </div>
                            )}
                            {error && !comparedList.length && (
                                <div
                                    className="d-flex justify-content-center align-items-center my-4 h-100"
                                    style={{ minHeight: "500px" }}
                                >
                                    <p className="text-danger text-center my-3">
                                        {error.message ||
                                            "Failed to fetch medicine list."}
                                    </p>
                                </div>
                            )}
                            {!isLoading && comparedList.length > 0 && (
                                <>
                                    <div className="d-flex justify-content-end">
                                        <div className="compare-drug-count-wrapper">
                                            <div className="compare-drug-count">
                                                {comparedList.length} Total
                                            </div>
                                        </div>
                                    </div>
                                    <div className="compare-drug-count-border-bottom" />

                                    {selectedCard && (
                                        <div className="compare-drug-header my-3 p-2">
                                            <h5 className="fw-semibold mb-1 compare-drug-header-title">
                                                {selectedCard.medicineName}
                                                <sub className="ms-1 mt-1">
                                                    {selectedCard.strength}
                                                </sub>
                                            </h5>
                                            <p className="mb-0">
                                                {
                                                    selectedCard.medicineDosageFormName
                                                }
                                            </p>
                                            <p className="mb-1">
                                                {selectedCard.manufacturerName}
                                            </p>

                                            <div className="compare-drug-header-bottom mb-3">
                                                <p className="mb-0">
                                                    {priceLabel}: ৳ {priceValue}
                                                </p>
                                            </div>

                                            <div className="row align-items-center gy-2">
                                                <div className="col-6 d-flex align-items-center gap-2">
                                                    <span className="small">
                                                        Show Total
                                                    </span>
                                                    <div className="form-check form-switch">
                                                        <input
                                                            className="form-check-input"
                                                            type="checkbox"
                                                            checked={showTotal}
                                                            onChange={() => {
                                                                setShowTotal(
                                                                    (prev) => {
                                                                        const next =
                                                                            !prev;
                                                                        if (
                                                                            next
                                                                        )
                                                                            setQty(
                                                                                totalDosageQty ||
                                                                                    1,
                                                                            );
                                                                        else
                                                                            setQty(
                                                                                1,
                                                                            );
                                                                        return next;
                                                                    },
                                                                );
                                                            }}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="col-6 text-end">
                                                    <label className="small me-2">
                                                        Qty
                                                    </label>
                                                    <input
                                                        type="text"
                                                        min={1}
                                                        className="form-control form-control-sm d-inline-block text-center"
                                                        style={{ width: 82 }}
                                                        value={qty}
                                                        onChange={(e) => {
                                                            const value =
                                                                e.target.value.trim();
                                                            if (
                                                                /^\d*$/.test(
                                                                    value,
                                                                )
                                                            ) {
                                                                const num =
                                                                    Number(
                                                                        value,
                                                                    );
                                                                setQty(
                                                                    num >= 1
                                                                        ? num
                                                                        : 1,
                                                                );
                                                            }
                                                        }}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                    )}

                                    {/* SORT */}
                                    <div className="medicine-comparison-modal d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                                        <div className="d-flex align-items-center gap-2">
                                            <span className="small">
                                                Sort By
                                            </span>
                                            <div
                                                className="position-relative"
                                                style={{ maxWidth: 165 }}
                                            >
                                                <select
                                                    className="form-select form-select-sm pe-4"
                                                    value={sortBy}
                                                    onChange={(e) =>
                                                        setSortBy(
                                                            e.target.value,
                                                        )
                                                    }
                                                    style={{
                                                        backgroundColor:
                                                            "#E9E7FA",
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
                                                        Alphabetic: A to Z
                                                    </option>
                                                    <option value="alphabeticDesc">
                                                        Alphabetic: Z to A
                                                    </option>
                                                </select>

                                                <FaChevronDown
                                                    className="position-absolute top-50 end-0 translate-middle-y me-2 text-secondary"
                                                    style={{
                                                        pointerEvents: "none",
                                                    }}
                                                />
                                            </div>
                                        </div>

                                        <div className="d-flex align-items-center">
                                            <span className="small">
                                                Items per page
                                            </span>
                                            <div
                                                className="position-relative"
                                                style={{ maxWidth: 165 }}
                                            >
                                                <select
                                                    className="form-select form-select-sm pe-4"
                                                    value={itemsPerPage}
                                                    onChange={(e) => {
                                                        const newItemsPerPage =
                                                            Number(
                                                                e.target.value,
                                                            );
                                                        setItemsPerPage(
                                                            newItemsPerPage,
                                                        );
                                                        setCurrentPage(1);
                                                    }}
                                                    style={{
                                                        backgroundColor:
                                                            "#E9E7FA",
                                                        appearance: "none",
                                                        cursor: "pointer",
                                                    }}
                                                >
                                                    {[3, 5, 10, 20].map((n) => (
                                                        <option
                                                            key={n}
                                                            value={n}
                                                        >
                                                            {n}
                                                        </option>
                                                    ))}
                                                </select>
                                                <FaChevronDown
                                                    className="position-absolute top-50 end-0 translate-middle-y me-2 text-secondary"
                                                    style={{
                                                        pointerEvents: "none",
                                                    }}
                                                />
                                            </div>
                                        </div>
                                    </div>

                                    {/* CARD LIST */}
                                    {visibleCards.map((card) => {
                                        return (
                                            <div
                                                key={card.medicineId}
                                                className={`compare-drug-card mb-1 rounded-3 shadow-sm`}
                                            >
                                                <div className="compare-drug-card-body d-flex justify-content-between align-items-center p-2 gap-2">
                                                    <div
                                                        className="text-start"
                                                        style={{ width: "60%" }}
                                                    >
                                                        <h6 className="compare-drug-card-title mb-2">
                                                            {card.medicineName}
                                                        </h6>
                                                        <small className="d-block mb-2 text-muted">
                                                            {
                                                                card.medicineDosageFormName
                                                            }
                                                        </small>
                                                        <small className="d-block mb-2 text-muted">
                                                            {card.strength}
                                                        </small>
                                                        <span className="d-block text-muted">
                                                            {
                                                                card.manufacturerName
                                                            }
                                                        </span>
                                                    </div>

                                                    <div
                                                        className="mt-2 small text-muted d-flex flex-column justify-content-center align-items-center"
                                                        style={{ width: "20%" }}
                                                    >
                                                        <strong className="d-block mb-2">
                                                            ৳{" "}
                                                            {card.unitPriceValue.toFixed(
                                                                2,
                                                            )}
                                                        </strong>
                                                        <small>
                                                            Unit Price
                                                        </small>
                                                    </div>

                                                    <TrendDisplay
                                                        basePrice={
                                                            selectedCard?.unitPriceValue
                                                        }
                                                        comparePrice={
                                                            card.unitPriceValue
                                                        }
                                                        multiplier={qty}
                                                    />

                                                    <div
                                                        className="text-end"
                                                        style={{ width: "10%" }}
                                                    >
                                                        <BsFillBookmarkFill
                                                            className="fs-5"
                                                            style={{
                                                                color: favorite.includes(
                                                                    card.medicineId,
                                                                )
                                                                    ? "#4b3b8b"
                                                                    : "#9ea4abbf",
                                                                cursor: "pointer",
                                                            }}
                                                            role="button"
                                                            onClick={(e) => {
                                                                e.preventDefault();
                                                                e.stopPropagation();
                                                                const medicineId =
                                                                    card.medicineId;
                                                                const isAlreadyFavorite =
                                                                    favorite.includes(
                                                                        medicineId,
                                                                    );
                                                                const updatedFavorites =
                                                                    isAlreadyFavorite
                                                                        ? favorite.filter(
                                                                              (
                                                                                  id,
                                                                              ) =>
                                                                                  id !==
                                                                                  medicineId,
                                                                          )
                                                                        : [
                                                                              ...favorite,
                                                                              medicineId,
                                                                          ];
                                                                setFavorite(
                                                                    updatedFavorites,
                                                                );
                                                            }}
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        );
                                    })}

                                    {/* PAGINATION */}
                                    <div className="d-flex justify-content-center gap-2 flex-wrap mt-4">
                                        <button
                                            className="btn btn-light btn-sm"
                                            style={{ fontFamily: "Georama" }}
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
                                                        padding: "0rem 0.7rem",
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
                                            style={{ fontFamily: "Georama" }}
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
                                    <div className="add-to-wishlist-center">
                                        <CustomButton
                                            type="button"
                                            label="Add to Wishlist"
                                            width="clamp(300px, 30vw, 450px)"
                                            height="clamp(38px, 2.3vw, 40px)"
                                            textColor="var(--theme-font-color)"
                                            backgroundColor="#FAF8FA"
                                            borderRadius="3px"
                                            shape="Square"
                                            borderColor="1px solid var(--theme-font-color)"
                                            className="action-btn"
                                            labelStyle={{
                                                fontSize:
                                                    "clamp(14px, 2vw, 16px)",
                                                fontWeight: "100",
                                                textTransform: "capitalize",
                                            }}
                                            hoverEffect="theme"
                                            disabled={
                                                isLoading || isWishListLoading
                                            }
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                e.nativeEvent.stopImmediatePropagation();
                                                handleSaveAllWishlist();
                                            }}
                                        />
                                    </div>
                                </>
                            )}
                        </>
                    )}
                </div>
            </div>
            <RewardPopup
                            isOpen={showRewardPopup}
                            onClose={() => {
                                setShowRewardPopup(false);
                                onClose();
                            }}
                            points={uploadResponse?.response?.totalRewardPoints || 0}
                            message={
                                "Congratulations! " +
                                (uploadResponse?.response?.rewardMessage || "")
                            }
            />
        </CustomModal>
    );
};

export default MedicineManagementModal;
