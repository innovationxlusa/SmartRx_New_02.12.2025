import { useEffect, useState } from "react";
import { validateField } from "../../../utils/validators";
import CustomInput from "../../static/Commons/CustomInput";
import useFormHandler from "../../../hooks/useFormHandler";
import useApiClients from "../../../services/useApiClients";
import CustomSelect from "../../static/Dropdown/CustomSelect";
import useCurrentUserId from "../../../hooks/useCurrentUserId";
import CustomModal from "../../static/CustomModal/CustomModal";
import ProfileButton from "../../static/Commons/CommonButton";
import {
    CREATE_NEW_VITAL_URL,
    DELETE_SMARTRX_VITAL_BY_ID_URL,
    EDIT_VITAL_URL,
} from "../../../constants/apiEndpoints";
import useApiService from "../../../services/useApiService";
import { handleGeneralError } from "../../../utils/errorHandling";
import PlusIcon from "../../../assets/img/AddBtn.svg";
import MinusIcon from "../../../assets/img/RoundMinusLite.svg";
import RewardPopup from "../../Reward/RewardPopup";

const DoctorReviewModal = ({
    modalType,
    isOpen,
    onClose,
    anotherButton,
    smartRxMasterId,
    prescriptionId,
    doctorId,
    refetch,
    smartRxOverview,
    doctorRating: externalDoctorRating,
    doctorComments: externalDoctorComments,
}) => {
    const {
        dynamicModalName,
        buttonIcons,
        dynamicButtonLabel,
        handleInputChange,
        resetForm,
        dynamicActions,
        toPascalCase,
    } = useFormHandler();
    const currentUserId = useCurrentUserId();

    // Destructuring API service methods
    const { api } = useApiClients();

    // Destructuring API service methods
    const apiServices = useApiService();

    //Tabbing
    const [activeTab, setActiveTab] = useState(null);
    const [isTabModalOpen, setIsTabModalOpen] = useState(false);
    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const [formData, setFormData] = useState({
        chamberFee: "",
        chamberFeeMeasurementUnit: "৳",
        transportFee: "",
        chamberWaitTimeMinute: "",
        travelTimeMinute: "",
        consultingDurationInMinutes: 0,
        otherExpense: 0,
        doctorRating: 0,
        comments: "",
        IsDoctorReviewed: false,
        IsCostAdded: false,
        IsTimeAdded: false,
    });

    // Initialize form data with existing values from smartRxOverview when modal opens
    useEffect(() => {
        if (isOpen && smartRxOverview) {
            // Get rating from external prop or try multiple possible locations in smartRxOverview
            const existingRating =
                externalDoctorRating !== undefined &&
                externalDoctorRating !== null
                    ? externalDoctorRating
                    : smartRxOverview.Rating ||
                      smartRxOverview.rating ||
                      smartRxOverview.doctorRating ||
                      0;

            // Get comments from external prop or try multiple possible locations in smartRxOverview
            const existingComments =
                externalDoctorComments !== undefined &&
                externalDoctorComments !== null
                    ? externalDoctorComments
                    : smartRxOverview.Comments ||
                      smartRxOverview.comments ||
                      "";

            const initialFormData = {
                travelTimeMinute:
                    smartRxOverview.totalTravelTime?.toString() || "",
                chamberWaitTimeMinute:
                    smartRxOverview.totalWaitingTime?.toString() || "",
                consultingDurationInMinutes:
                    smartRxOverview.totalVisitingTime?.toString() || "0",
                transportFee:
                    smartRxOverview.totalTransportExpense?.toString() || "",
                chamberFee:
                    smartRxOverview.totalDoctorExpense?.toString() || "",
                otherExpense: smartRxOverview.OtherExpenses || 0,
                doctorRating: existingRating,
                comments: existingComments,
            };

            setFormData((prev) => ({
                ...prev,
                ...initialFormData,
            }));

            // Store initial values for comparison
            setInitialValues({
                chamberFee: initialFormData.chamberFee,
                transportFee: initialFormData.transportFee,
                chamberWaitTimeMinute: initialFormData.chamberWaitTimeMinute,
                travelTimeMinute: initialFormData.travelTimeMinute,
                consultingDurationInMinutes:
                    initialFormData.consultingDurationInMinutes,
                doctorRating: existingRating,
                comments: existingComments,
            });

            // Also update the rating state for the star display
            setRating(Number(existingRating) || 0);
        }
    }, [isOpen, smartRxOverview, externalDoctorRating, externalDoctorComments]);

    // Sync rating state with formData when Rate Doctor tab becomes active
    useEffect(() => {
        if (
            activeTab === "rate" &&
            formData.doctorRating !== undefined &&
            formData.doctorRating !== null
        ) {
            setRating(Number(formData.doctorRating) || 0);
        }
    }, [activeTab, formData.doctorRating]);

    const [fieldErrors, setFieldErrors] = useState({
        chamberFee: "",
        chamberFeeMeasurementUnit: "",
        transportFee: "",
        chamberWaitTimeMinute: "",
        travelTimeMinute: "",
        consultingDurationInMinutes: "",
        otherExpense: 0,
        doctorRating: 0,
        comments: "",
    });

    const [rating, setRating] = useState(0);

    // Store initial values to compare against for change detection
    const [initialValues, setInitialValues] = useState({
        chamberFee: "",
        transportFee: "",
        chamberWaitTimeMinute: "",
        travelTimeMinute: "",
        consultingDurationInMinutes: "0",
        doctorRating: 0,
        comments: "",
    });

    const handleStarClick = (value) => {
        setRating(value);
        setFormData((prev) => ({
            ...prev,
            doctorRating: value,
        }));
    };

    const handleButtonClick = (tab, e) => {
        e.preventDefault();
        setActiveTab(tab);
        setIsTabModalOpen(true);
    };

    const closeTabModal = () => {
        setIsTabModalOpen(false);
        setActiveTab(null);
    };

    // Reset form data when modal closes
    useEffect(() => {
        if (!isOpen) {
            setFormData({
                chamberFee: "",
                chamberFeeMeasurementUnit: "৳",
                transportFee: "",
                chamberWaitTimeMinute: "",
                travelTimeMinute: "",
                consultingDurationInMinutes: 0,
                otherExpense: 0,
                doctorRating: 0,
                comments: "",
            });
            setInitialValues({
                chamberFee: "",
                transportFee: "",
                chamberWaitTimeMinute: "",
                travelTimeMinute: "",
                consultingDurationInMinutes: "0",
                doctorRating: 0,
                comments: "",
            });
            setRating(0);
            setActiveTab(null);
        }
    }, [isOpen]);

    const modalNames = dynamicModalName("Review");
    const [isLoading, setIsLoading] = useState(false);
    const buttonLabels = dynamicButtonLabel("Vital");

    const handleUpdate = async () => {
        setUploadResponse(null);
        setShowRewardPopup(false);
        setIsLoading(true);

        try {
            const hasChanged = (current, initial) => {
                return (
                    String(current || "").trim() !==
                    String(initial || "").trim()
                );
            };

            const isDoctorReviewed =
                (hasChanged(
                    formData.doctorRating,
                    initialValues.doctorRating,
                ) ||
                    hasChanged(formData.comments, initialValues.comments)) &&
                (Number(formData.doctorRating) > 0 ||
                    formData.comments?.trim());

            const isCostAdded =
                hasChanged(formData.chamberFee, initialValues.chamberFee) ||
                hasChanged(formData.transportFee, initialValues.transportFee);

            const isTimeAdded =
                hasChanged(
                    formData.travelTimeMinute,
                    initialValues.travelTimeMinute,
                ) ||
                hasChanged(
                    formData.chamberWaitTimeMinute,
                    initialValues.chamberWaitTimeMinute,
                ) ||
                hasChanged(
                    formData.consultingDurationInMinutes,
                    initialValues.consultingDurationInMinutes,
                );

            const payload = {
                SmartRxMasterId: smartRxMasterId,
                PrescriptionId: prescriptionId,
                DoctorId: doctorId,
                TravelTimeMinute: formData.travelTimeMinute
                    ? parseInt(formData.travelTimeMinute)
                    : null,
                WaitingTimeMinute: formData.chamberWaitTimeMinute
                    ? parseInt(formData.chamberWaitTimeMinute)
                    : null,
                DoctorConsultingDuration: formData.consultingDurationInMinutes,
                FeeCharged: formData.chamberFee
                    ? parseInt(formData.chamberFee)
                    : 0,
                FeeChargedMeasurementUnit: "৳",
                TransportCost: formData.transportFee
                    ? parseInt(formData.transportFee)
                    : 0,
                OtherCost: formData.otherExpense
                    ? parseInt(formData.otherExpense)
                    : null,
                Rating: formData.doctorRating
                    ? parseFloat(formData.doctorRating)
                    : null,
                Comments: formData.comments || "",
                IsDoctorReviewed: isDoctorReviewed,
                IsCostAdded: isCostAdded,
                IsTimeAdded: isTimeAdded,
                loginUserId: currentUserId ?? 0,
            };
            // Call API
            const response = await api.docReviewUpdate(payload, "DoctorReview");
            setUploadResponse(response);

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        closeTabModal();
                        if (typeof onClose === "function") {
                            onClose();
                        }
                    }, 3000);
                } else {
                    closeTabModal();
                    if (typeof onClose === "function") {
                        onClose();
                    }
                }
            }

            // Refetch parent data to refresh Overview
            if (typeof refetch === "function") {
                try {
                    await refetch();
                } catch (e) {
                    // Swallow refetch errors to not block UI
                }
            }
        } catch (error) {
            handleGeneralError(error);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <>
            <CustomModal
                isOpen={isOpen}
                modalName=""
                subModal=""
                close={onClose}
                animationDirection="top"
                position="top"
                form={true}
                closeOnOverlayClick={false}
                modalSize="tiny"
            >
                <div className="all-buttons">
                    <button
                        className="fee-add-button"
                        onClick={(e) => handleButtonClick("fee", e)}
                    >
                        Cost
                    </button>
                    <button
                        className="wait-add-button"
                        onClick={(e) => handleButtonClick("wait", e)}
                    >
                        Time
                    </button>
                    <button
                        className="rate-add-button"
                        onClick={(e) => handleButtonClick("rate", e)}
                    >
                        Rate Doctor
                    </button>
                </div>
            </CustomModal>

            {/* Second Modal with tabs - Only shown when a button is clicked */}

            <CustomModal
                isOpen={isTabModalOpen}
                close={closeTabModal}
                animationDirection="top"
                position="top"
                form={false}
                closeOnOverlayClick={true}
                modalSize="medium"
            >
                <div className="tabbed-modal">
                    {/* Tab Navigation - Inside modal at the top */}
                    <div className="modal-tabs">
                        <button
                            className={`tab ${activeTab === "fee" ? "active" : ""}`}
                            onClick={() => setActiveTab("fee")}
                        >
                            Cost
                        </button>
                        <button
                            className={`tab ${activeTab === "wait" ? "active" : ""}`}
                            onClick={() => setActiveTab("wait")}
                        >
                            Time
                        </button>
                        <button
                            className={`tab ${activeTab === "rate" ? "active" : ""}`}
                            onClick={() => setActiveTab("rate")}
                        >
                            Rate Doctor
                        </button>
                    </div>

                    {/* Tab Content */}
                    <div className="tab-content">
                        {activeTab === "fee" && (
                            <div className="fee-content">
                                <div className="doctor-fee">
                                    <h3
                                        style={{
                                            fontFamily: "Georama",
                                            fontWeight: 600,
                                            color: "#65636e",
                                            fontSize: "1.0rem",
                                        }}
                                    >
                                        Update Doctor Fee
                                    </h3>
                                    <p
                                        style={{
                                            color: "#fdc34b",
                                            fontFamily: "Georama",
                                            fontSize: "0.7rem",
                                            marginTop: "-10px",
                                        }}
                                    >
                                        Earn points by adding fee
                                    </p>

                                    <div className="fee-options">
                                        <div
                                            className="fee-input-container"
                                            style={{
                                                display: "flex",
                                                alignItems: "center",
                                                width: "100%",
                                                justifyContent: "center",
                                                gap: "8px",
                                                marginTop: "-12px",
                                                marginLeft: "8px",
                                            }}
                                        >
                                            <img
                                                src={MinusIcon}
                                                alt="Minus Icon"
                                                style={{
                                                    width: "25px",
                                                    height: "25px",
                                                    cursor: "pointer",
                                                }}
                                                onClick={() => {
                                                    const currentValue =
                                                        parseInt(
                                                            formData.chamberFee ||
                                                                "0",
                                                        );
                                                    if (currentValue > 0) {
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            chamberFee: (
                                                                currentValue -
                                                                100
                                                            ).toString(),
                                                        }));
                                                    }
                                                }}
                                            />
                                            <div
                                                className="fee-input-container"
                                                style={{ width: "120px" }}
                                            >
                                                <CustomInput
                                                    label=""
                                                    labelPosition=""
                                                    placeholder="Enter amount e.g. 500"
                                                    name="FeeCharged"
                                                    type="text"
                                                    value={formData.chamberFee}
                                                    onChange={(e) => {
                                                        const numericValue =
                                                            e.target.value.replace(
                                                                /\D/g,
                                                                "",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            chamberFee:
                                                                numericValue,
                                                        }));
                                                        setFieldErrors(
                                                            (prev) => ({
                                                                ...prev,
                                                                chamberFee: "",
                                                            }),
                                                        );
                                                    }}
                                                    error={
                                                        fieldErrors.chamberFee
                                                    }
                                                    disabled={isLoading}
                                                    style={{
                                                        textAlign: "center",
                                                        width: "100%",
                                                    }}
                                                />
                                            </div>

                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    gap: "4px",
                                                }}
                                            >
                                                <img
                                                    src={PlusIcon}
                                                    alt="Plus Icon"
                                                    style={{
                                                        width: "30px",
                                                        height: "30px",
                                                        cursor: "pointer",
                                                    }}
                                                    onClick={() => {
                                                        const currentValue =
                                                            parseInt(
                                                                formData.chamberFee ||
                                                                    "0",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            chamberFee: (
                                                                currentValue +
                                                                100
                                                            ).toString(),
                                                        }));
                                                    }}
                                                />
                                                <span
                                                    style={{
                                                        fontFamily: "Georama",
                                                        fontSize: "0.6rem",
                                                        color: "#65636e",
                                                        fontWeight: 600,
                                                        marginTop: "14px",
                                                    }}
                                                >
                                                    TK
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div
                                    className="transport-fee"
                                    style={{ marginTop: "12px" }}
                                >
                                    <h3
                                        style={{
                                            fontFamily: "Georama",
                                            fontWeight: 600,
                                            color: "#65636e",
                                            fontSize: "1.0rem",
                                        }}
                                    >
                                        Update Transport Fee
                                    </h3>
                                    <p
                                        style={{
                                            color: "#fdc34b",
                                            fontFamily: "Georama",
                                            fontSize: "0.7rem",
                                            marginTop: "-10px",
                                        }}
                                    >
                                        Earn points by adding fee
                                    </p>

                                    <div className="transport-fee-options">
                                        <div
                                            className="transport-fee-input-container"
                                            style={{
                                                display: "flex",
                                                alignItems: "center",
                                                width: "100%",
                                                justifyContent: "center",
                                                gap: "8px",
                                                marginTop: "-12px",
                                                marginLeft: "8px",
                                            }}
                                        >
                                            <img
                                                src={MinusIcon}
                                                alt="Minus Icon"
                                                style={{
                                                    width: "25px",
                                                    height: "25px",
                                                    cursor: "pointer",
                                                }}
                                                onClick={() => {
                                                    const currentValue =
                                                        parseInt(
                                                            formData.transportFee ||
                                                                "0",
                                                        );
                                                    if (currentValue > 0) {
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            transportFee: (
                                                                currentValue -
                                                                100
                                                            ).toString(),
                                                        }));
                                                    }
                                                }}
                                            />
                                            <div
                                                className="transport-fee-input-container"
                                                style={{ width: "120px" }}
                                            >
                                                <CustomInput
                                                    label=""
                                                    labelPosition=""
                                                    placeholder="Enter amount e.g. 500"
                                                    name="TransportFee"
                                                    type="text"
                                                    value={
                                                        formData.transportFee
                                                    }
                                                    onChange={(e) => {
                                                        const numericValue =
                                                            e.target.value.replace(
                                                                /\D/g,
                                                                "",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            transportFee:
                                                                numericValue,
                                                        }));
                                                        setFieldErrors(
                                                            (prev) => ({
                                                                ...prev,
                                                                transportFee:
                                                                    "",
                                                            }),
                                                        );
                                                    }}
                                                    error={
                                                        fieldErrors.transportFee
                                                    }
                                                    disabled={isLoading}
                                                    style={{
                                                        textAlign: "center",
                                                        width: "100%",
                                                    }}
                                                />
                                            </div>

                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    gap: "4px",
                                                }}
                                            >
                                                <img
                                                    src={PlusIcon}
                                                    alt="Plus Icon"
                                                    style={{
                                                        width: "30px",
                                                        height: "30px",
                                                        cursor: "pointer",
                                                    }}
                                                    onClick={() => {
                                                        const currentValue =
                                                            parseInt(
                                                                formData.transportFee ||
                                                                    "0",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            transportFee: (
                                                                currentValue +
                                                                100
                                                            ).toString(),
                                                        }));
                                                    }}
                                                />
                                                <span
                                                    style={{
                                                        fontFamily: "Georama",
                                                        fontSize: "0.6rem",
                                                        color: "#65636e",
                                                        fontWeight: 600,
                                                        marginTop: "14px",
                                                    }}
                                                >
                                                    TK
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div
                                    className="d-flex justify-content-center"
                                    style={{
                                        gap: "50px",
                                        width: "auto",
                                    }}
                                >
                                    {/* <button
                                        className="cancel-btn"
                                        style={{
                                            width: "80px",
                                            height: "35px",
                                            fontWeight: 800,
                                        }}
                                        onClick={closeTabModal}
                                    >
                                        Close
                                    </button> */}
                                    <button
                                        className="update-btn"
                                        style={{
                                            marginTop: "20px",
                                            width: "60%",
                                            padding: "0.5rem 1rem",
                                            background: "#e6e4ef",
                                            border: "1px solid #b8aef2",
                                            borderRadius: "6px",
                                            fontSize: "0.95rem",
                                            color: "#4b3b8b",
                                            cursor: "pointer",
                                        }}
                                        onClick={handleUpdate}
                                    >
                                        Update
                                    </button>
                                </div>
                            </div>
                        )}

                        {activeTab === "wait" && (
                            <div className="time-content">
                                <div className="travel-time-container">
                                    <h3
                                        style={{
                                            fontFamily: "Georama",
                                            fontWeight: 600,
                                            color: "#65636e",
                                            fontSize: "1.0rem",
                                            marginLeft: "8px",
                                        }}
                                    >
                                        Update Travel Time
                                    </h3>
                                    <p
                                        style={{
                                            color: "#fdc34b",
                                            fontFamily: "Georama",
                                            fontSize: "0.7rem",
                                            marginTop: "-10px",
                                        }}
                                    >
                                        Earn points by adding time
                                    </p>
                                    <div className="travel-options">
                                        <div
                                            className="travel-input-container"
                                            style={{
                                                display: "flex",
                                                alignItems: "center",
                                                width: "100%",
                                                justifyContent: "center",
                                                gap: "8px",
                                                marginTop: "-8px",
                                                marginLeft: "12px",
                                            }}
                                        >
                                            <img
                                                src={MinusIcon}
                                                alt="Minus Icon"
                                                style={{
                                                    width: "25px",
                                                    height: "25px",
                                                    cursor: "pointer",
                                                }}
                                                onClick={() => {
                                                    const currentValue =
                                                        parseInt(
                                                            formData.travelTimeMinute ||
                                                                "0",
                                                        );
                                                    if (currentValue > 0) {
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            travelTimeMinute: (
                                                                currentValue - 5
                                                            ).toString(),
                                                        }));
                                                    }
                                                }}
                                            />
                                            <div style={{ width: "120px" }}>
                                                <CustomInput
                                                    label=""
                                                    labelPosition=""
                                                    placeholder="Enter travel time e.g. 30"
                                                    name="TravelTimeMinute"
                                                    type="text"
                                                    value={
                                                        formData.travelTimeMinute
                                                    }
                                                    onChange={(e) => {
                                                        let numericValue =
                                                            e.target.value.replace(
                                                                /\D/g,
                                                                "",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            travelTimeMinute:
                                                                numericValue,
                                                        }));
                                                        setFieldErrors(
                                                            (prev) => ({
                                                                ...prev,
                                                                travelTimeMinute:
                                                                    "",
                                                            }),
                                                        );
                                                    }}
                                                    onBlur={() => {
                                                        if (
                                                            !formData.travelTimeMinute ||
                                                            parseInt(
                                                                formData.travelTimeMinute,
                                                            ) < 0
                                                        ) {
                                                            setFormData(
                                                                (prev) => ({
                                                                    ...prev,
                                                                    travelTimeMinute:
                                                                        "0",
                                                                }),
                                                            );
                                                        }
                                                    }}
                                                    error={
                                                        fieldErrors.travelTimeMinute
                                                    }
                                                    disabled={isLoading}
                                                    style={{
                                                        textAlign: "center",
                                                        width: "100%",
                                                    }}
                                                />
                                            </div>

                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    gap: "4px",
                                                }}
                                            >
                                                <img
                                                    src={PlusIcon}
                                                    alt="Plus Icon"
                                                    style={{
                                                        width: "30px",
                                                        height: "30px",
                                                        cursor: "pointer",
                                                    }}
                                                    onClick={() => {
                                                        const currentValue =
                                                            parseInt(
                                                                formData.travelTimeMinute ||
                                                                    "0",
                                                            );

                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            travelTimeMinute: (
                                                                currentValue + 5
                                                            ).toString(),
                                                        }));
                                                    }}
                                                />
                                                <span
                                                    style={{
                                                        fontFamily: "Georama",
                                                        fontSize: "0.6rem",
                                                        color: "#65636e",
                                                        fontWeight: 600,
                                                        marginTop: "14px",
                                                    }}
                                                >
                                                    Min
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div className="wait-time-container">
                                    <h3
                                        style={{
                                            fontFamily: "Georama",
                                            fontWeight: 600,
                                            color: "#65636e",
                                            fontSize: "1.0rem",
                                            marginTop: "20px",
                                            marginLeft: "8px",
                                        }}
                                    >
                                        Chamber Waiting Time
                                    </h3>
                                    <p
                                        style={{
                                            color: "#fdc34b",
                                            fontFamily: "Georama",
                                            fontSize: "0.7rem",
                                            marginTop: "-10px",
                                        }}
                                    >
                                        Earn points by adding time
                                    </p>
                                    <div className="wait-options">
                                        <div
                                            className="wait-input-container"
                                            style={{
                                                display: "flex",
                                                alignItems: "center",
                                                width: "100%",
                                                justifyContent: "center",
                                                gap: "8px",
                                                marginTop: "-8px",
                                                marginLeft: "12px",
                                            }}
                                        >
                                            <img
                                                src={MinusIcon}
                                                alt="Minus Icon"
                                                style={{
                                                    width: "25px",
                                                    height: "25px",
                                                    cursor: "pointer",
                                                }}
                                                onClick={() => {
                                                    const currentValue =
                                                        parseInt(
                                                            formData.chamberWaitTimeMinute ||
                                                                "0",
                                                        );
                                                    if (currentValue > 0) {
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            chamberWaitTimeMinute:
                                                                (
                                                                    currentValue -
                                                                    5
                                                                ).toString(),
                                                        }));
                                                    }
                                                }}
                                            />
                                            <div style={{ width: "120px" }}>
                                                <CustomInput
                                                    label=""
                                                    labelPosition=""
                                                    placeholder="Enter waiting time e.g. 30"
                                                    name="WaitingTimeMinute"
                                                    type="text"
                                                    value={
                                                        formData.chamberWaitTimeMinute
                                                    }
                                                    onChange={(e) => {
                                                        let numericValue =
                                                            e.target.value.replace(
                                                                /\D/g,
                                                                "",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            chamberWaitTimeMinute:
                                                                numericValue,
                                                        }));
                                                        setFieldErrors(
                                                            (prev) => ({
                                                                ...prev,
                                                                chamberWaitTimeMinute:
                                                                    "",
                                                            }),
                                                        );
                                                    }}
                                                    onBlur={() => {
                                                        if (
                                                            !formData.chamberWaitTimeMinute ||
                                                            parseInt(
                                                                formData.chamberWaitTimeMinute,
                                                            ) < 0
                                                        ) {
                                                            setFormData(
                                                                (prev) => ({
                                                                    ...prev,
                                                                    chamberWaitTimeMinute:
                                                                        "0",
                                                                }),
                                                            );
                                                        }
                                                    }}
                                                    error={
                                                        fieldErrors.chamberWaitTimeMinute
                                                    }
                                                    disabled={isLoading}
                                                    style={{
                                                        textAlign: "center",
                                                        width: "100%",
                                                    }}
                                                />
                                            </div>

                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    gap: "4px",
                                                }}
                                            >
                                                <img
                                                    src={PlusIcon}
                                                    alt="Plus Icon"
                                                    style={{
                                                        width: "30px",
                                                        height: "30px",
                                                        cursor: "pointer",
                                                    }}
                                                    onClick={() => {
                                                        const currentValue =
                                                            parseInt(
                                                                formData.chamberWaitTimeMinute ||
                                                                    "0",
                                                            );

                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            chamberWaitTimeMinute:
                                                                (
                                                                    currentValue +
                                                                    5
                                                                ).toString(),
                                                        }));
                                                    }}
                                                />
                                                <span
                                                    style={{
                                                        fontFamily: "Georama",
                                                        fontSize: "0.6rem",
                                                        color: "#65636e",
                                                        fontWeight: 600,
                                                        marginTop: "14px",
                                                    }}
                                                >
                                                    Min
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div className="visit-time-container">
                                    <h3
                                        style={{
                                            fontFamily: "Georama",
                                            fontWeight: 600,
                                            color: "#65636e",
                                            fontSize: "1.0rem",
                                            marginTop: "20px",
                                            marginLeft: "8px",
                                        }}
                                    >
                                        Doctor Visit Time
                                    </h3>
                                    <p
                                        style={{
                                            color: "#fdc34b",
                                            fontFamily: "Georama",
                                            fontSize: "0.7rem",
                                            marginTop: "-10px",
                                        }}
                                    >
                                        Earn points by adding time
                                    </p>
                                    <div className="visit-options">
                                        <div
                                            className="visit-input-container"
                                            style={{
                                                display: "flex",
                                                alignItems: "center",
                                                width: "100%",
                                                justifyContent: "center",
                                                gap: "8px",
                                                marginTop: "-8px",
                                                marginLeft: "12px",
                                            }}
                                        >
                                            <img
                                                src={MinusIcon}
                                                alt="Minus Icon"
                                                style={{
                                                    width: "25px",
                                                    height: "25px",
                                                    cursor: "pointer",
                                                }}
                                                onClick={() => {
                                                    const currentValue =
                                                        parseInt(
                                                            formData.consultingDurationInMinutes ||
                                                                "0",
                                                        );
                                                    if (currentValue > 0) {
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            consultingDurationInMinutes:
                                                                (
                                                                    currentValue -
                                                                    5
                                                                ).toString(),
                                                        }));
                                                    }
                                                }}
                                            />
                                            <div style={{ width: "120px" }}>
                                                <CustomInput
                                                    label=""
                                                    labelPosition=""
                                                    placeholder="Enter visit time e.g. 30"
                                                    name="DoctorConsultingDuration"
                                                    type="text"
                                                    value={
                                                        formData.consultingDurationInMinutes
                                                    }
                                                    onChange={(e) => {
                                                        let numericValue =
                                                            e.target.value.replace(
                                                                /\D/g,
                                                                "",
                                                            );
                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            consultingDurationInMinutes:
                                                                numericValue,
                                                        }));
                                                        setFieldErrors(
                                                            (prev) => ({
                                                                ...prev,
                                                                consultingDurationInMinutes:
                                                                    "",
                                                            }),
                                                        );
                                                    }}
                                                    onBlur={() => {
                                                        if (
                                                            !formData.consultingDurationInMinutes ||
                                                            parseInt(
                                                                formData.consultingDurationInMinutes,
                                                            ) < 0
                                                        ) {
                                                            setFormData(
                                                                (prev) => ({
                                                                    ...prev,
                                                                    consultingDurationInMinutes:
                                                                        "0",
                                                                }),
                                                            );
                                                        }
                                                    }}
                                                    error={
                                                        fieldErrors.consultingDurationInMinutes
                                                    }
                                                    disabled={isLoading}
                                                    style={{
                                                        textAlign: "center",
                                                        width: "100%",
                                                    }}
                                                />
                                            </div>

                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    gap: "4px",
                                                }}
                                            >
                                                <img
                                                    src={PlusIcon}
                                                    alt="Plus Icon"
                                                    style={{
                                                        width: "30px",
                                                        height: "30px",
                                                        cursor: "pointer",
                                                    }}
                                                    onClick={() => {
                                                        const currentValue =
                                                            parseInt(
                                                                formData.consultingDurationInMinutes ||
                                                                    "0",
                                                            );

                                                        setFormData((prev) => ({
                                                            ...prev,
                                                            consultingDurationInMinutes:
                                                                (
                                                                    currentValue +
                                                                    5
                                                                ).toString(),
                                                        }));
                                                    }}
                                                />
                                                <span
                                                    style={{
                                                        fontFamily: "Georama",
                                                        fontSize: "0.6rem",
                                                        color: "#65636e",
                                                        fontWeight: 600,
                                                        marginTop: "14px",
                                                    }}
                                                >
                                                    Min
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div
                                    className="d-flex justify-content-center mt-4"
                                    style={{
                                        gap: "50px",
                                        width: "auto",
                                    }}
                                >
                                    <button
                                        className="update-btn"
                                        style={{
                                            width: "60%",
                                            padding: "0.5rem 1rem",
                                            background: "#e6e4ef",
                                            border: "1px solid #b8aef2",
                                            borderRadius: "6px",
                                            fontSize: "0.95rem",
                                            color: "#4b3b8b",
                                            cursor: "pointer",
                                        }}
                                        onClick={handleUpdate}
                                    >
                                        Update
                                    </button>
                                </div>
                            </div>
                        )}

                        {activeTab === "rate" && (
                            <div className="rate-content">
                                <h3
                                    style={{
                                        fontFamily: "Georama",
                                        fontWeight: 600,
                                        color: "#65636e",
                                        fontSize: "1.0rem",
                                    }}
                                >
                                    Update Doctor Rating
                                </h3>
                                <p
                                    style={{
                                        color: "#fdc34b",
                                        fontFamily: "Georama",
                                        fontSize: "0.7rem",
                                    }}
                                >
                                    Earn points by rating the doctor
                                </p>

                                <div
                                    className="rate-stars"
                                    style={{
                                        display: "flex",
                                        justifyContent: "center",
                                        alignItems: "center",
                                        gap: "22px",
                                        width: "100%",
                                        minHeight: "40px",
                                        flexWrap: "nowrap",
                                    }}
                                >
                                    {[1, 2, 3, 4, 5].map((star) => (
                                        <span
                                            key={`rating-star-${star}`}
                                            onClick={() =>
                                                handleStarClick(star)
                                            }
                                            style={{
                                                cursor: "pointer",
                                                fontSize: "28px",
                                                backgroundColor: "transparent",
                                                color:
                                                    rating >= star
                                                        ? "#ff9800"
                                                        : "#e0e0e0",
                                                WebkitTextStroke:
                                                    rating >= star
                                                        ? "0px"
                                                        : "1px #ff9800",
                                                transition: "color 0.2s ease",
                                                display: "inline-block",
                                                minWidth: "28px",
                                                width: "28px",
                                                height: "28px",
                                                lineHeight: "28px",
                                                textAlign: "center",
                                                userSelect: "none",
                                                flexShrink: 0,
                                            }}
                                        >
                                            ★
                                        </span>
                                    ))}
                                </div>

                                <div
                                    className="rate-comments"
                                    style={{
                                        marginBottom: "25px",
                                        marginTop: "20px",
                                        width: "100%",
                                    }}
                                >
                                    <textarea
                                        name="Comments"
                                        placeholder="Enter your comments if any"
                                        value={formData.comments}
                                        onChange={(e) => {
                                            let value = e.target.value;
                                            setFormData((prev) => ({
                                                ...prev,
                                                comments: value,
                                            }));
                                            setFieldErrors((prev) => ({
                                                ...prev,
                                                comments: "",
                                            }));
                                        }}
                                        disabled={isLoading}
                                        style={{
                                            width: "100%",
                                            minHeight: "60px",
                                            border: "0.5px solid #65636e",
                                            borderRadius: "5px",
                                            padding: "8px",
                                            fontFamily: "Georama",
                                            fontSize: "14px",
                                            color: "#65636e",
                                        }}
                                    />
                                    {fieldErrors.Comments && (
                                        <p
                                            className="error-message mb-0"
                                            style={{ fontFamily: "Georama" }}
                                        >
                                            {fieldErrors.comments}
                                        </p>
                                    )}
                                </div>

                                {/* Add rating form elements here */}

                                <div
                                    className="d-flex justify-content-center"
                                    style={{
                                        gap: "50px",
                                        width: "auto",
                                    }}
                                >
                                    <button
                                        className="update-btn"
                                        style={{
                                            width: "60%",
                                            padding: "0.5rem 1rem",
                                            background: "#e6e4ef",
                                            border: "1px solid #b8aef2",
                                            borderRadius: "6px",
                                            fontSize: "0.95rem",
                                            color: "#4b3b8b",
                                            cursor: "pointer",
                                        }}
                                        onClick={handleUpdate}
                                    >
                                        Update
                                    </button>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
                <RewardPopup
                    isOpen={showRewardPopup}
                    onClose={() => {
                        setShowRewardPopup(false);
                    }}
                    points={uploadResponse?.response?.totalRewardPoints || 0}
                    message={
                        "Congratulations! " +
                        (uploadResponse?.response?.rewardMessage || "")
                    }
                />
            </CustomModal>
        </>
    );
};

export default DoctorReviewModal;
