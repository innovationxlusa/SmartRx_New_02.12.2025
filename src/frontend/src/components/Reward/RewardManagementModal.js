import React from "react";
import CustomModal from "../static/CustomModal/CustomModal";
import "./RewardManagementModal.css";
import { ReactComponent as TickMark } from "../../assets/img/TickMark.svg";
import { ReactComponent as PremiumPlus } from "../../assets/img/TrophyCup.svg";
import { ReactComponent as BasicPlus } from "../../assets/img/GoldMedalList.svg";
import MediumPlus from "../../assets/img/MediumPlus.png";
import SuperPlus from "../../assets/img/SuperPlus.png";

const RewardManagementModal = ({ isOpen, onClose, rewardPoints }) => {
    // ✅ Map points to corresponding badge components
    const rewardBadges = {
        100: <BasicPlus />,
        500: <img src={MediumPlus} alt="Medium Plus" style={{ boxShadow: "none" }} />,
        600: <img src={SuperPlus} alt="Super Plus" style={{ boxShadow: "none", marginLeft: "13px" }} />,
        700: <PremiumPlus />,
    };

    // ✅ Pick correct badge icon based on rewardPoints, fallback to PremiumPlus if not matched
    const badgeIcon =
        rewardBadges[rewardPoints] || <PremiumPlus alt="Default Trophy Icon" />;

    // Optional: dynamically choose title/description too
    const badgeTitles = {
        100: "Basic Plus",
        500: "Medium Plus",
        600: "Super Plus",
        700: "Premium Plus",
    };

    const title = badgeTitles[rewardPoints] || "Premium Plus";

    return (
        <CustomModal
            isOpen={isOpen}
            modalName=""
            close={onClose}
            animationDirection="top"
            position="top"
            closeOnOverlayClick={false}
        >
            <div className="badge-card">
                <div className="badge-icon">{badgeIcon}</div>

                <div className="badge-check"><TickMark /></div>
                <h2 className="badge-title">Become a {title}</h2>
                <p className="badge-subtitle">
                    Receive this badge by using multiple functions
                </p>
                <p className="badge-description">
                    Achieve this badge by becoming a {title} Member. You will receive two
                    free Smart RX! Sign up right from your profile!
                </p>
                <button className="badge-button">Achieve Badge</button>
            </div>
        </CustomModal>
    );
};

export default RewardManagementModal;
