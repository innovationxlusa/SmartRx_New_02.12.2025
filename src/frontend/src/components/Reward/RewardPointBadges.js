import React, { useEffect, useState, useMemo } from "react";
import "./Reward.css";
import { useLocation, useNavigate } from "react-router-dom";
import PageTitle from "../static/PageTitle/PageTitle";
import useApiClients from "../../services/useApiClients";
import { useUserContext } from "../../contexts/UserContext";
import rewardIcon from "../../assets/img/GoldMedalList.svg";
import trophyIcon from "../../assets/img/TrophyCup.svg";
import TickIcon from "../../assets/img/TickIcon.svg";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import { useFetchData } from "../../hooks/useFetchData";
import RewardManagementModal from "./RewardManagementModal";
import { ReactComponent as PremiumPlus } from "../../assets/img/TrophyCup.svg";
import { ReactComponent as BasicPlus } from "../../assets/img/GoldMedalList.svg";
import MediumPlus from "../../assets/img/MediumPlus.png";
import SuperPlus from "../../assets/img/SuperPlus.png";

const RewardPointBadges = () => {
    const [modalType, setModalType] = useState(null);
    const [rewardPoints, setRewardPoints] = useState(130.0);
    const navigate = useNavigate();
    const { smartNavigate } = useSmartNavigate({ scroll: "top" });

    const { state } = useLocation();
    const { api } = useApiClients();
    const { user } = useUserContext();
    const loginUserId = Number(user?.UserId);

    /* ───────────── Fetch Data ───────────── */
    const {
        data: badgesListData,
        isLoading: isBadgesLoading,
        error: badgesError,
    } = useFetchData(api.getRewardsBadgesList, 1);

    const {
        data: patientRewardsData,
        isLoading: isPatientRewardsLoading,
        error: patientRewardsError,
    } = useFetchData(api.getPatientRewardsSummary, 1, null, null, null, loginUserId, null);

    /* ───────────── Icon Map ───────────── */
    const rewardBadges = {
        basicPlus: <BasicPlus />,
        mediumPlus: <img src={MediumPlus} alt="Medium Plus" />,
        superPlus: <img src={SuperPlus} alt="Super Plus" />,
        premiumPlus: <PremiumPlus />,
    };

    console.log("badgesListData: ", badgesListData);
    console.log("patientRewardsData: ", patientRewardsData);


    /* ───────────── Utility ───────────── */
    const toCamelCase = (str = "") =>
        str
            .replace(/[^a-zA-Z0-9 ]/g, "")
            .split(" ")
            .map((word, index) =>
                index === 0
                    ? word.charAt(0).toLowerCase() + word.slice(1)
                    : word.charAt(0).toUpperCase() + word.slice(1)
            )
            .join("");

    /* ───────────── Build Badge Data ───────────── */
    const processedBadgesData = useMemo(() => {
        if (!badgesListData || !patientRewardsData) {
            return { currentBadgeId: null, badges: [] };
        }

        const currentBadgeId = patientRewardsData?.data?.badgeId || null;

        const badgesArray =
            badgesListData?.data?.data ||
            badgesListData?.data ||
            badgesListData ||
            [];

        return {
            currentBadgeId,
            badges: badgesArray.map((badge, index) => {
                const badgeKey = toCamelCase(badge.name || badge.badgeName || `Badge ${index + 1}`);

                // ✅ Define point values for each badge
                const pointsMapping = {
                    basicPlus: 100,
                    mediumPlus: 500,
                    superPlus: 600,
                    premiumPlus: 700,
                };

                return {
                    id: badge.id,
                    name: badge.name || badge.badgeName || `Badge ${index + 1}`,
                    isSelected: badge.id === currentBadgeId,
                    isCompleted: badge.id === currentBadgeId,
                    icon: rewardBadges[badgeKey] || rewardIcon,
                    description: badge.description || badge.details || "",
                    pointsValue: pointsMapping[badgeKey] || 100, // ✅ default
                };
            }),
        };
    }, [badgesListData, patientRewardsData]);

    /* ───────────── Modal Helpers ───────────── */
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    const handleOpenModal = (sourceTestIds, type, point) => {
        openModal(type);
        setRewardPoints(point);
    };

    /* ───────────── UI ───────────── */
    return (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5 mb-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle pageName={"Reward Point Badge"} switchButton={false} />

                    {(isBadgesLoading || isPatientRewardsLoading) && (
                        <div
                            style={{
                                display: "flex",
                                justifyContent: "center",
                                alignItems: "center",
                                marginTop: "40px",
                                fontFamily: "Georama",
                                color: "#4b3b8b",
                            }}
                        >
                            <p>Loading badges...</p>
                        </div>
                    )}

                    {(badgesError || patientRewardsError) &&
                        !isBadgesLoading &&
                        !isPatientRewardsLoading && (
                            <div
                                style={{
                                    display: "flex",
                                    justifyContent: "center",
                                    alignItems: "center",
                                    marginTop: "40px",
                                    fontFamily: "Georama",
                                    color: "#dc3545",
                                }}
                            >
                                <p>Error loading badges. Please try again.</p>
                            </div>
                        )}

                    {!isBadgesLoading &&
                        !isPatientRewardsLoading &&
                        !badgesError &&
                        !patientRewardsError && (
                            <div
                                style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                    justifyContent: "center",
                                    marginTop: "40px",
                                    fontFamily: "Georama",
                                    color: "#4b3b8b",
                                    fontWeight: "600",
                                    gap: "20px",
                                    textAlign: "center",
                                }}
                            >
                                {processedBadgesData.badges.length === 0 ? (
                                    <div
                                        style={{
                                            textAlign: "center",
                                            padding: "40px 0",
                                            color: "#65636e",
                                        }}
                                    >
                                        <p
                                            style={{
                                                fontSize: "16px",
                                                fontFamily: "Georama",
                                            }}
                                        >
                                            No badges available at the moment.
                                        </p>
                                    </div>
                                ) : (
                                    <div
                                        style={{
                                            display: "grid",
                                            gridTemplateColumns: "repeat(auto-fit, minmax(120px, 1fr))",
                                            gap: "28px",
                                            justifyContent: "center",
                                            textAlign: "center",
                                        }}
                                    >
                                        {processedBadgesData.badges.map((badge, idx) => (
                                            <div
                                                key={badge.id}
                                                style={{
                                                    display: "flex",
                                                    flexDirection: "column",
                                                    alignItems: "center",
                                                    justifyContent: "center",
                                                }}
                                            >
                                                <div
                                                    style={{
                                                        position: "relative",
                                                        width: "70px",
                                                        height: "70px",
                                                        transition: "opacity 0.18s, transform 0.12s",
                                                        opacity: badge.isSelected ? 1 : 0.4,
                                                        filter: badge.isSelected ? "none" : "grayscale(100%)",
                                                        cursor: "pointer",
                                                    }}
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        e.nativeEvent.stopImmediatePropagation();
                                                        handleOpenModal(null, "add", badge.pointsValue);
                                                    }}
                                                >
                                                    {badge.isSelected && (
                                                        <img
                                                            src={TickIcon}
                                                            alt="selected"
                                                            style={{
                                                                position: "absolute",
                                                                top: -8,
                                                                left: -8,
                                                                width: 25,
                                                                height: 24,
                                                                padding: 3,
                                                            }}
                                                        />
                                                    )}

                                                    {typeof badge.icon === "string" ? (
                                                        <img
                                                            src={badge.icon}
                                                            alt={badge.name}
                                                            style={{ width: "70px", height: "70px" }}
                                                        />
                                                    ) : (
                                                        React.cloneElement(badge.icon, {
                                                            style: { width: "70px", height: "70px" },
                                                        })
                                                    )}
                                                </div>

                                                <span
                                                    style={{
                                                        marginTop: 8,
                                                        fontFamily: "Georama",
                                                        color: badge.isSelected ? "#4b3b8b" : "#9b98a3",
                                                        fontWeight: badge.isSelected ? 700 : 600,
                                                        fontSize: 14,
                                                        textAlign: "center",
                                                    }}
                                                >
                                                    {badge.name}
                                                </span>
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </div>
                        )}
                </div>
            </div>

            <RewardManagementModal
                isOpen={!!modalType}
                onClose={closeModal}
                rewardPoints={rewardPoints}
            />
        </div>
    );
};

export default RewardPointBadges;
