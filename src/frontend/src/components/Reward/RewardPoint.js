import React, { useCallback, useEffect, useState, useMemo } from "react";
import "./Reward.css";
import "./RewardPoint.css";
import { useLocation } from "react-router-dom";
import PageTitle from "../static/PageTitle/PageTitle";
import useApiClients from "../../services/useApiClients";
import { useUserContext } from "../../contexts/UserContext";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import rewardIcon from "../../assets/img/GoldMedal.svg";
import { useNavigate } from "react-router-dom";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import CustomButton from "../static/Commons/CustomButton";
import StepIndicator from "../static/StepIndicator/StepIndicator";
import PlusIcon from "../../assets/img/AddBtn.svg";
import MinusIcon from "../../assets/img/RoundMinus.svg";
import tkIcon from "../../assets/img/TkLogo.svg";
import rxIcon from "../../assets/img/RxLogo2.svg";
import { useFetchData } from "../../hooks/useFetchData";
import CustomInput from "../static/Commons/CustomInput";
import { RewardConversionType } from "../../constants/rewardConversionType";

const RewardPoint = () => {
    const [activeTab, setActiveTab] = useState("nonCashable");
    const [pointsInput, setPointsInput] = useState(1000);
    const [convertedAmount, setConvertedAmount] = useState(130.0);
    const [ncToCPoints, setNcToCPoints] = useState(""); // Non-cashable -> Cashable
    const [cToNcPoints, setCToNcPoints] = useState(""); // Cashable -> Non-cashable

    const conversionRate = 0.13;
    const [conversionMessages, setConversionMessages] = useState({
        ncToC: "",
        cToNc: "",
        money: "",
    });

    const { smartNavigate } = useSmartNavigate({ scroll: "top" });
    const { state } = useLocation();
    const { api } = useApiClients();
    const { user } = useUserContext();
    const currentUserId = useCurrentUserId();
    const navigate = useNavigate();
    const loginUserId = currentUserId ?? null;
    const isValidUserId = Number.isFinite(loginUserId) && !!loginUserId;
    
    const {
        data: rewardApiData,
        isLoading: isRewardLoading,
        error: rewardError,
        refetch: refetchRewards,
    } = useFetchData(
        api.getPatientRewardsSummary,
        null,
        null,
        null,
        null,
        isValidUserId ? loginUserId : null, // Only pass userId if valid, otherwise null to prevent API call
        null,
    );
    
    const rewardData = useMemo(() => {
        // If no API data or invalid userId, return default values
        if (!rewardApiData || !isValidUserId) {
            return {
                userId: loginUserId,
                badgeId: 0,
                badgeName: "",

                totalNonCashable: 0,
                totalCashable: 0,
                TotalEarnedPoints: 0,
                TotalConsumedPoints: 0,  
                TotalPoint: 0,
                progress: 0,
                tiers: (() => {
                    const tierNames = [
                        "Basic Plus",
                        "Medium Plus",
                        "Super Plus",
                        "Premium Plus",
                    ];
                    return tierNames.map((name) => ({
                        name,
                        isActive: false,
                        isCompleted: false,
                    }));
                })(),
            };
        }

        // Access data directly from rewardApiData (useFetchData returns response.response)
        // The API response structure may vary, so we check both possible paths
        const apiData = rewardApiData?.data || rewardApiData;

        return {
            userId: apiData?.userId || loginUserId,
            badgeId: apiData?.badgeId || 0,
            badgeName: apiData?.badgeName || "",
            patientId: apiData?.patientId || null,

            totalNonCashable: apiData?.totalNonCashable || apiData?.totalNonCashablePoints || 0,
            totalCashable: apiData?.totalCashable || apiData?.totalCashablePoints || 0,

            TotalEarnedPoints: apiData?.TotalEarnedPoints || apiData?.totalEarnedPoints || 0,
            TotalConsumedPoints: apiData?.TotalConsumedPoints || apiData?.totalConsumedPoints || 0,
            TotalPoint: apiData?.TotalPoint || apiData?.totalPoint || 0,

            progress:
                Math.min(
                    100,
                    ((apiData?.totalNonCashable || apiData?.totalNonCashablePoints || 0) / 10000) * 100,
                ) || 0,
            tiers: (() => {
                const tierNames = [
                    "Basic Plus",
                    "Medium Plus",
                    "Super Plus",
                    "Premium Plus",
                ];
                const currentBadgeName = apiData?.badgeName || "";
                const currentBadgeIndex = tierNames.findIndex(
                    (name) => name === currentBadgeName,
                );

                return tierNames.map((name, index) => ({
                    name,
                    isActive: index === currentBadgeIndex,
                    isCompleted: index <= currentBadgeIndex,
                }));
            })(),
        };
    }, [rewardApiData, loginUserId, isValidUserId]);

    const handlePointsChange = useCallback(
        (value) => {
            const maxPoints = rewardData?.totalCashable ?? 0;
            const numericValue = Number(value);
            const sanitizedValue = Number.isFinite(numericValue)
                ? numericValue
                : 0;
            const safeValue = Math.min(
                Math.max(sanitizedValue, 0),
                maxPoints,
            );
            setPointsInput(safeValue);
            setConvertedAmount((safeValue * conversionRate).toFixed(2));
        },
        [rewardData?.totalCashable, conversionRate],
    );

    const incrementPoints = useCallback(() => {
        const newValue = pointsInput + 100;
        handlePointsChange(newValue);
    }, [pointsInput, handlePointsChange]);

    const decrementPoints = useCallback(() => {
        if (pointsInput > 100) {
            const newValue = pointsInput - 100;
            handlePointsChange(newValue);
        }
    }, [pointsInput, handlePointsChange]);

    useEffect(() => {
        const maxPoints = rewardData?.totalCashable ?? 0;
        if (pointsInput > maxPoints) {
            handlePointsChange(maxPoints);
        }
    }, [
        rewardData?.finalCashableBalance,
        pointsInput,
        conversionRate,
        handlePointsChange,
    ]);

    // Handlers for point-type conversions (UI only; integrate API when available)
    const clamp = (value, min, max) => {
        const num = Number(value) || 0;
        if (num < min) return min;
        if (num > max) return max;
        return num;
    };

    const onChangeNcToCPoints = (value) => {
        if (value === "") {
            setNcToCPoints("");
            return;
        }
        const max = rewardData?.totalNonCashable || 0;
        setNcToCPoints(clamp(value, 0, max));
    };

    const onChangeCToNcPoints = (value) => {
        if (value === "") {
            setCToNcPoints("");
            return;
        }
        const max = rewardData?.totalCashable || 0;
        setCToNcPoints(clamp(value, 0, max));
    };

    const convertNcToC = async () => {
        const amount = Number(ncToCPoints);
        const available = rewardData?.totalNonCashable || 0;
        if (!Number.isFinite(amount) || amount <= 0) {
            setConversionMessages((prev) => ({
                ...prev,
                ncToC: "Enter a valid non-cashable point amount.",
            }));
            return;
        }
        if (amount > available) {
            setConversionMessages((prev) => ({
                ...prev,
                ncToC: "You do not have enough non-cashable points.",
            }));
            return;
        }
        setConversionMessages((prev) => ({ ...prev, ncToC: "" }));
        try {
            const payload = {
                userId: loginUserId,
                fromType: RewardConversionType.NONCASHABLE,
                toType: RewardConversionType.CASHABLE,
                amount,
            };
            await api.convertRewardPoints(
                payload,
                "ConvertNonCashableToCashable",
            );
            // Always refetch rewards data after conversion
            await refetchRewards?.();
            setNcToCPoints(0);
        } catch (e) {
            console.error("Failed converting non-cashable to cashable points", e);
            // Still refetch to ensure UI is up to date
            await refetchRewards?.();
        }
    };

    const convertCToNc = async () => {
        const amount = Number(cToNcPoints);
        const available = rewardData?.totalCashable || 0;
        if (!Number.isFinite(amount) || amount <= 0) {
            setConversionMessages((prev) => ({
                ...prev,
                cToNc: "Enter a valid cashable point amount.",
            }));
            return;
        }
        if (amount > available) {
            setConversionMessages((prev) => ({
                ...prev,
                cToNc: "You do not have enough cashable points.",
            }));
            return;
        }
        setConversionMessages((prev) => ({ ...prev, cToNc: "" }));
        try {
            const payload = {
                userId: loginUserId,
                fromType: RewardConversionType.CASHABLE,
                toType: RewardConversionType.NONCASHABLE,
                amount,
            };
            await api.convertRewardPoints(
                payload,
                "ConvertCashableToNonCashable",
            );
            // Always refetch rewards data after conversion
            await refetchRewards?.();
            setCToNcPoints(0);
        } catch (e) {
            console.error("Failed to convert cashable to non-cashable points", e);
            // Still refetch to ensure UI is up to date
            await refetchRewards?.();
        }
    };

    return (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle pageName={"Reward Point"} switchButton={false} />

                    {isRewardLoading && (
                        <div
                            style={{
                                textAlign: "center",
                                padding: "40px 0",
                                color: "#65636e",
                            }}
                        >
                            <div
                                className="spinner-border"
                                role="status"
                                style={{
                                    width: "3rem",
                                    height: "3rem",
                                    color: "#4b3b8b",
                                }}
                            >
                                <span className="visually-hidden">
                                    Loading...
                                </span>
                            </div>
                            <p
                                style={{
                                    marginTop: "20px",
                                    fontSize: "16px",
                                    fontFamily: "Georama",
                                }}
                            >
                                Loading your rewards...
                            </p>
                        </div>
                    )}

                    {rewardError && !isRewardLoading && isValidUserId && (
                        <div
                            className="alert alert-danger mt-3"
                            role="alert"
                            style={{ fontFamily: "Georama" }}
                        >
                            <h5>⚠️ Error Loading Rewards</h5>
                            <p>
                                {rewardError?.message ||
                                    "Failed to load reward points. Please try again."}
                            </p>
                            <CustomButton
                                type="button"
                                label="Retry"
                                onClick={() => refetchRewards?.()}
                                width="auto"
                                height="35px"
                                textColor="#fff"
                                backgroundColor="var(--app-theme-color)"
                                borderRadius="4px"
                                labelStyle={{
                                    fontSize: "14px",
                                    fontWeight: "500",
                                }}
                            />
                        </div>
                    )}

                    {!isValidUserId && !isRewardLoading && (
                        <div
                            className="alert alert-info mt-3"
                            role="alert"
                            style={{ fontFamily: "Georama" }}
                        >
                            <h5>ℹ️ User Not Logged In</h5>
                            <p>Please log in to view your reward points.</p>
                            <CustomButton
                                type="button"
                                label="Go to Login"
                                onClick={() => navigate("/login")}
                                width="auto"
                                height="35px"
                                textColor="#fff"
                                backgroundColor="var(--app-theme-color)"
                                borderRadius="4px"
                                labelStyle={{
                                    fontSize: "14px",
                                    fontWeight: "500",
                                }}
                            />
                        </div>
                    )}
                    {!isRewardLoading && isValidUserId && !rewardError && (
                        <>
                            <div
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "space-between",
                                    marginBottom: "20px",
                                }}
                            >
                                <div
                                    style={{
                                        flex: 1,
                                        maxWidth: "50%",
                                        marginLeft: "50px",
                                    }}
                                >
                                    <div
                                        style={{
                                            fontSize: "18px",
                                            fontWeight: "600",
                                            color: "#65636e",
                                            marginBottom: "8px",
                                            fontFamily: "Georama",
                                        }}
                                    >
                                        {rewardData.badgeName}
                                    </div>

                                    <div
                                        style={{
                                            width: "50%",
                                            height: "6px",
                                            backgroundColor: "#e6e4ef",
                                            borderRadius: "3px",
                                            overflow: "hidden",
                                        }}
                                    >
                                        <div
                                            style={{
                                                width: `${rewardData.progress}%`,
                                                height: "100%",
                                                backgroundColor: "#4b3b8b",
                                                transition: "width 0.3s ease",
                                            }}
                                        ></div>
                                    </div>
                                </div>

                                <div style={{ marginRight: "50px" }}>
                                    <img
                                        src={rewardIcon}
                                        alt="Reward Badge"
                                        onClick={() =>
                                            smartNavigate("/rewardPointBadges")
                                        }
                                        style={{
                                            width: "50px",
                                            height: "50px",
                                            cursor: "pointer",
                                        }}
                                    />
                                </div>
                            </div>

                            <div className="tabbed-rewards">
                                <div className="reward-tabs">
                                    <button
                                        className={`tab ${activeTab === "nonCashable" ? "active" : ""}`}
                                        onClick={() =>
                                            setActiveTab("nonCashable")
                                        }
                                        type="button"
                                    >
                                        Non Cashable Points
                                        <br />
                                        {rewardData.totalNonCashable}{" "}
                                        Total
                                    </button>
                                    <button
                                        className={`tab ${activeTab === "cashable" ? "active" : ""}`}
                                        onClick={() => setActiveTab("cashable")}
                                        type="button"
                                    >
                                        Cashable Points
                                        <br />
                                        {rewardData.totalCashable} Total
                                    </button>
                                </div>

                                {activeTab === "nonCashable" ? (
                                    <div className="tab-content mt-3">
                                        <div
                                            style={{
                                                marginBottom: "20px",
                                            }}
                                        >
                                            <div
                                                style={{
                                                    display: "flex",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                    marginBottom: "15px",
                                                }}
                                            >
                                                <div
                                                    style={{
                                                        textAlign: "left",
                                                    }}
                                                >
                                                    <div
                                                        style={{
                                                            fontSize: "14px",
                                                            color: "#65636e",
                                                            marginBottom: "5px",
                                                            fontFamily:
                                                                "Georama",
                                                            fontWeight: "bold",
                                                        }}
                                                    >
                                                        Earned Points
                                                    </div>
                                                    <div
                                                        style={{
                                                            fontSize: "20px",
                                                            fontWeight: "200",
                                                            color: "#65636e",
                                                            fontFamily:
                                                                "Georama",
                                                        }}
                                                    >
                                                        {rewardData.TotalEarnedPoints}
                                                    </div>
                                                </div>
                                                <div
                                                    style={{
                                                        textAlign: "right",
                                                    }}
                                                >
                                                    <div
                                                        style={{
                                                            fontSize: "14px",
                                                            color: "#65636e",
                                                            marginBottom: "5px",
                                                            fontFamily:
                                                                "Georama",
                                                            fontWeight: "bold",
                                                        }}
                                                    >
                                                        Consumed Points
                                                    </div>
                                                    <div
                                                        style={{
                                                            fontSize: "20px",
                                                            fontWeight: "200",
                                                            color: "#65636e",
                                                            fontFamily:
                                                                "Georama",
                                                        }}
                                                    >
                                                        {rewardData.TotalConsumedPoints}
                                                    </div>
                                                </div>
                                            </div>

                                            <div
                                                style={{
                                                    textAlign: "center",
                                                }}
                                            >
                                                <div
                                                    style={{
                                                        fontSize: "18px",
                                                        fontWeight: "700",
                                                        color: "#65636e",
                                                        marginBottom: "5px",
                                                        fontFamily: "Georama",
                                                    }}
                                                >
                                                    Total Points
                                                </div>
                                                <div
                                                    style={{
                                                        fontSize: "24px",
                                                        fontWeight: "700",
                                                        color: "#65636e",
                                                        fontFamily: "Georama",
                                                    }}
                                                >
                                                    {rewardData.TotalPoint.toLocaleString()}
                                                </div>
                                            </div>

                                            <div
                                                style={{
                                                    marginTop: "30px",
                                                }}
                                            >
                                                <StepIndicator
                                                    steps={rewardData.tiers.map(
                                                        (tier) => ({
                                                            label: tier.name,
                                                            isActive:
                                                                tier.isCompleted,
                                                        }),
                                                    )}
                                                />
                                            </div>
                                        </div>

                                        <div
                                            style={{
                                                display: "flex",
                                                gap: "30px",
                                                marginTop: "50px",
                                            }}
                                        >
                                            <CustomButton
                                                type="button"
                                                label="See Details"
                                                width="50%"
                                                textColor="var(--theme-font-color)"
                                                backgroundColor="#FAF8FA"
                                                borderRadius="4px"
                                                shape="Square"
                                                borderColor="1px solid var(--theme-font-color)"
                                                labelStyle={{
                                                    fontSize: "16px",
                                                    fontWeight: "500",
                                                    textTransform: "capitalize",
                                                    fontFamily: "Georama",
                                                }}
                                                hoverEffect="theme"
                                                onClick={() =>
                                                    smartNavigate(
                                                        "/rewardDetails",
                                                    )
                                                }
                                            />
                                            <CustomButton
                                                type="button"
                                                label="Benefits"
                                                width="50%"
                                                textColor="var(--theme-font-color)"
                                                backgroundColor="#FAF8FA"
                                                borderRadius="4px"
                                                shape="Square"
                                                borderColor="1px solid var(--theme-font-color)"
                                                labelStyle={{
                                                    fontSize: "16px",
                                                    fontWeight: "500",
                                                    textTransform: "capitalize",
                                                    fontFamily: "Georama",
                                                }}
                                                hoverEffect="theme"
                                                onClick={() =>
                                                    smartNavigate(
                                                        "/rewardBenefits",
                                                    )
                                                }
                                            />
                                        </div>
                                    </div>
                                ) : (
                                    <div className="tab-content mt-3">
                                        {/* Conversion between non-cashable and cashable points */}
                                        <div
                                            className="conversion-container"
                                            style={{
                                                width: "100%",
                                                backgroundColor: "#ffffff",
                                                borderRadius: "10px",
                                                marginBottom: "20px",
                                                boxShadow:
                                                    "0 2px 8px rgba(0,0,0,0.1)",
                                                display: "flex",
                                                flexDirection: "column",
                                                gap: "24px",
                                                padding: "24px 20px",
                                                boxSizing: "border-box",
                                                textAlign: "center",
                                                alignItems: "center",
                                            }}
                                        >
                                            {/* Non-cashable → Cashable */}
                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    justifyContent: "center",
                                                    gap: "12px",
                                                    flexWrap: "wrap",
                                                }}
                                            >
                                                <div
                                                    style={{
                                                        fontFamily: "Georama",
                                                        color: "#4b3b8b",
                                                        fontWeight: 600,
                                                    }}
                                                >
                                                    Non-cashable → Cashable
                                                </div>
                                                <div
                                                    style={{
                                                        display: "flex",
                                                        alignItems: "center",
                                                        gap: "10px",
                                                    }}
                                                >
                                                    <CustomInput
                                                        type="number"
                                                        max={
                                                            rewardData?.totalNonCashable ||
                                                            0
                                                        }
                                                        value={ncToCPoints}
                                                        onChange={(e) =>
                                                            onChangeNcToCPoints(
                                                                e.target.value,
                                                            )
                                                        }
                                                        placeholder="Enter Points"
                                                        style={{
                                                            width: "120px",
                                                            height: "38px",
                                                            border: "1px solid #e9ecef",
                                                            borderRadius: "6px",
                                                            padding: "0 12px",
                                                            fontSize: "16px",
                                                            fontFamily:
                                                                "Georama",
                                                            color: "#65636e",
                                                            textAlign: "right",
                                                        }}
                                                    />
                                                    <CustomButton
                                                        type="button"
                                                        label="Convert"
                                                        width="120px"
                                                        textColor="#fff"
                                                        backgroundColor="var(--app-theme-color)"
                                                        borderRadius="6px"
                                                        shape="Square"
                                                        labelStyle={{
                                                            fontSize: "14px",
                                                            fontWeight: "600",
                                                            textTransform:
                                                                "capitalize",
                                                            fontFamily:
                                                                "Georama",
                                                        }}
                                                        onClick={convertNcToC}
                                                    />
                                                </div>
                                            </div>
                                            {conversionMessages.ncToC && (
                                                <div
                                                    style={{
                                                        color: "#c53030",
                                                        fontSize: "14px",
                                                        marginTop: "6px",
                                                    }}
                                                >
                                                    {conversionMessages.ncToC}
                                                </div>
                                            )}

                                            {/* Cashable → Non-cashable */}
                                            <div
                                                style={{
                                                    display: "flex",
                                                    alignItems: "center",
                                                    justifyContent: "center",
                                                    gap: "12px",
                                                    flexWrap: "wrap",
                                                }}
                                            >
                                                <div
                                                    style={{
                                                        fontFamily: "Georama",
                                                        color: "#4b3b8b",
                                                        fontWeight: 600,
                                                    }}
                                                >
                                                    Cashable → Non-cashable
                                                </div>
                                                <div
                                                    style={{
                                                        display: "flex",
                                                        alignItems: "center",
                                                        gap: "10px",
                                                    }}
                                                >
                                                    <CustomInput
                                                        type="number"
                                                        max={
                                                            rewardData?.totalCashable ||
                                                            0
                                                        }
                                                        value={cToNcPoints}
                                                        onChange={(e) =>
                                                            onChangeCToNcPoints(
                                                                e.target.value,
                                                            )
                                                        }
                                                        placeholder="Enter Points"
                                                        style={{
                                                            width: "120px",
                                                            height: "38px",
                                                            border: "1px solid #e9ecef",
                                                            borderRadius: "6px",
                                                            padding: "0 12px",
                                                            fontSize: "16px",
                                                            fontFamily:
                                                                "Georama",
                                                            color: "#65636e",
                                                            textAlign: "right",
                                                        }}
                                                    />
                                                    <CustomButton
                                                        type="button"
                                                        label="Convert"
                                                        width="120px"
                                                        textColor="#fff"
                                                        backgroundColor="var(--app-theme-color)"
                                                        borderRadius="6px"
                                                        shape="Square"
                                                        labelStyle={{
                                                            fontSize: "14px",
                                                            fontWeight: "600",
                                                            textTransform:
                                                                "capitalize",
                                                            fontFamily:
                                                                "Georama",
                                                        }}
                                                        onClick={convertCToNc}
                                                    />
                                                </div>
                                            </div>
                                            {conversionMessages.cToNc && (
                                                <div
                                                    style={{
                                                        color: "#c53030",
                                                        fontSize: "14px",
                                                        marginTop: "6px",
                                                    }}
                                                >
                                                    {conversionMessages.cToNc}
                                                </div>
                                            )}
                                        </div>

                                        <div
                                            style={{
                                                width: "100%",
                                                backgroundColor: "#ffffff",
                                                borderRadius: "10px",
                                                marginBottom: "20px",
                                                boxShadow:
                                                    "0 2px 8px rgba(0,0,0,0.1)",
                                                display: "flex",
                                                flexDirection: "column",
                                                gap: "10px",
                                                padding: "20px 10px",
                                                boxSizing: "border-box",
                                                overflowX: "auto",
                                                minWidth: "100%",
                                            }}
                                        >
                                            <div>
                                                <div
                                                    style={{
                                                        fontSize: "14px",
                                                        color: "#65636e",
                                                        marginBottom: "12px",
                                                        fontFamily: "Georama",
                                                        fontWeight: "500",
                                                        textAlign: "left",
                                                    }}
                                                >
                                                    Point
                                                </div>
                                                <div
                                                    style={{
                                                        display: "flex",
                                                        alignItems: "center",
                                                        gap: "8px",
                                                        flexWrap: "nowrap",
                                                        width: "100%",
                                                        justifyContent:
                                                            "space-between",
                                                    }}
                                                    className="point-input-section"
                                                >
                                                    <div
                                                        style={{
                                                            display: "flex",
                                                            alignItems:
                                                                "center",
                                                            gap: "8px",
                                                            flexShrink: 0,
                                                        }}
                                                    >
                                                        <img
                                                            src={rxIcon}
                                                            alt="Rx Icon"
                                                            style={{
                                                                width: "40px",
                                                                height: "40px",
                                                                cursor: "pointer",
                                                                flexShrink: 0,
                                                            }}
                                                        />
                                                        <span
                                                            style={{
                                                                fontSize:
                                                                    "16px",
                                                                color: "#4b3b8b",
                                                                fontFamily:
                                                                    "Georama",
                                                                fontWeight:
                                                                    "600",
                                                                whiteSpace:
                                                                    "nowrap",
                                                            }}
                                                        >
                                                            Point
                                                        </span>
                                                        <span
                                                            style={{
                                                                color: "#65636e",
                                                                fontSize:
                                                                    "18px",
                                                                flexShrink: 0,
                                                            }}
                                                        >
                                                            &gt;
                                                        </span>
                                                    </div>
                                                    <div
                                                        style={{
                                                            display: "flex",
                                                            alignItems:
                                                                "center",
                                                            gap: "8px",
                                                            flexShrink: 0,
                                                        }}
                                                    >
                                                        <img
                                                            src={MinusIcon}
                                                            alt="Minus Icon"
                                                            style={{
                                                                width: "25px",
                                                                height: "25px",
                                                                cursor: "pointer",
                                                                flexShrink: 0,
                                                            }}
                                                            onClick={
                                                                decrementPoints
                                                            }
                                                        />

                                                        <CustomInput
                                                            type="text"
                                                            value={pointsInput}
                                                            onChange={(e) =>
                                                                handlePointsChange(
                                                                    Number(
                                                                        e.target
                                                                            .value,
                                                                    ) || 0,
                                                                )
                                                            }
                                                            style={{
                                                                width: "80px",
                                                                minWidth:
                                                                    "60px",
                                                                height: "40px",
                                                                // backgroundColor:
                                                                //     "#f8f9fa",
                                                                border: "1px solid #e9ecef",
                                                                borderRadius:
                                                                    "6px",
                                                                padding:
                                                                    "0 8px",
                                                                fontSize:
                                                                    "16px",
                                                                fontFamily:
                                                                    "Georama",
                                                                color: "#65636e",
                                                                outline: "none",
                                                                flexShrink: 0,
                                                                textAlign:
                                                                    "center",
                                                            }}
                                                        />
                                                        <img
                                                            src={PlusIcon}
                                                            alt="Plus Icon"
                                                            style={{
                                                                width: "30px",
                                                                height: "30px",
                                                                cursor: "pointer",
                                                                flexShrink: 0,
                                                            }}
                                                            onClick={
                                                                incrementPoints
                                                            }
                                                        />
                                                    </div>
                                                </div>
                                            </div>

                                            <div
                                                style={{
                                                    display: "flex",
                                                    justifyContent: "center",
                                                    alignItems: "center",
                                                    position: "relative",
                                                    padding: "0 20px",
                                                }}
                                            >
                                                <div
                                                    style={{
                                                        position: "absolute",
                                                        left: "0",
                                                        top: "50%",
                                                        width: "100%",
                                                        height: "1px",
                                                        backgroundColor:
                                                            "#cecdd4ff",
                                                        zIndex: 1,
                                                    }}
                                                ></div>
                                            </div>

                                            <div>
                                                <div
                                                    style={{
                                                        fontSize: "14px",
                                                        color: "#65636e",
                                                        marginBottom: "12px",
                                                        fontFamily: "Georama",
                                                        fontWeight: "500",
                                                        textAlign: "left",
                                                    }}
                                                >
                                                    Converted Money
                                                </div>
                                                <div
                                                    style={{
                                                        display: "flex",
                                                        alignItems: "center",
                                                        gap: "8px",
                                                        flexWrap: "nowrap",
                                                        width: "100%",
                                                        justifyContent:
                                                            "space-between",
                                                    }}
                                                    className="converted-money-section"
                                                >
                                                    <div
                                                        style={{
                                                            display: "flex",
                                                            alignItems:
                                                                "center",
                                                            gap: "8px",
                                                            flexShrink: 0,
                                                        }}
                                                    >
                                                        <img
                                                            src={tkIcon}
                                                            alt="Taka Icon"
                                                            style={{
                                                                width: "40px",
                                                                height: "40px",
                                                                flexShrink: 0,
                                                            }}
                                                        />
                                                        <span
                                                            style={{
                                                                fontSize:
                                                                    "16px",
                                                                color: "#4b3b8b",
                                                                fontFamily:
                                                                    "Georama",
                                                                fontWeight:
                                                                    "bold",
                                                                whiteSpace:
                                                                    "nowrap",
                                                            }}
                                                        >
                                                            Taka
                                                        </span>
                                                        <span
                                                            style={{
                                                                color: "#65636e",
                                                                fontSize:
                                                                    "18px",
                                                                flexShrink: 0,
                                                            }}
                                                        >
                                                            &gt;
                                                        </span>
                                                    </div>
                                                    <div
                                                        style={{
                                                            width: "120px",
                                                            minWidth: "80px",
                                                            height: "40px",
                                                            backgroundColor:
                                                                "#f8f9fa",
                                                            border: "1px solid #e9ecef",
                                                            borderRadius: "6px",
                                                            padding: "0 12px",
                                                            display: "flex",
                                                            alignItems:
                                                                "center",
                                                            justifyContent:
                                                                "flex-end",
                                                            fontSize: "16px",
                                                            fontFamily:
                                                                "Georama",
                                                            color: "#65636e",
                                                            flexShrink: 0,
                                                        }}
                                                    >
                                                        {convertedAmount}
                                                    </div>
                                                </div>
                                            </div>
                                            <div
                                                style={{
                                                    display: "flex",
                                                    justifyContent: "center",
                                                    marginTop: "2px",
                                                    marginBottom: "3px",
                                                }}
                                            >
                                                <CustomButton
                                                    type="button"
                                                    label="Convert To Money"
                                                    width="200px"
                                                    textColor="#4b3b8b"
                                                    backgroundColor="#FAF8FA"
                                                    borderRadius="2px"
                                                    shape="Square"
                                                    borderColor="1px solid #4b3b8b"
                                                    labelStyle={{
                                                        fontSize: "16px",
                                                        fontWeight: "500",
                                                        textTransform:
                                                            "capitalize",
                                                        fontFamily: "Georama",
                                                    }}
                                                    hoverEffect="theme"
                                                    onClick={async () => {
                                                        const numericPoints = Number(pointsInput);
                                                        const available = rewardData?.totalCashable || 0;
                                                        if (!Number.isFinite(numericPoints) || numericPoints <= 0) {
                                                            setConversionMessages((prev) => ({
                                                                ...prev,
                                                                money: "Enter a valid cashable point amount.",
                                                            }));
                                                            return;
                                                        }
                                                        if (numericPoints > available) {
                                                            setConversionMessages((prev) => ({
                                                                ...prev,
                                                                money: "You do not have enough cashable points.",
                                                            }));
                                                            return;
                                                        }
                                                        setConversionMessages((prev) => ({
                                                            ...prev,
                                                            money: "",
                                                        }));
                                                        try {
                                                            const payload = {
                                                                userId: loginUserId,
                                                                fromType: RewardConversionType.CASHABLE,
                                                                toType: RewardConversionType.MONEY,
                                                                amount: numericPoints,
                                                            };
                                                            await api.convertRewardPoints(
                                                                payload,
                                                                "ConvertCashableToMoney",
                                                            );
                                                            // Always refetch rewards data after conversion
                                                            await refetchRewards?.();
                                                            alert(
                                                                `Successfully converted ${numericPoints} points to ${convertedAmount} Taka!`,
                                                            );
                                                        } catch (e) {
                                                            console.error(
                                                                "ConvertCashableToMoney failed",
                                                                e,
                                                            );
                                                            // Still refetch to ensure UI is up to date
                                                            await refetchRewards?.();
                                                        }
                                                    }}
                                                />
                                            </div>
                                            {conversionMessages.money && (
                                                <div
                                                    style={{
                                                        color: "#c53030",
                                                        fontSize: "14px",
                                                        textAlign: "center",
                                                        marginTop: "6px",
                                                    }}
                                                >
                                                    {conversionMessages.money}
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                )}
                            </div>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

export default RewardPoint;
