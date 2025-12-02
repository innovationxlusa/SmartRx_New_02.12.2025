import "./SeeMoreModal.css";
import CustomModal from "../static/CustomModal/CustomModal";
import { ReactComponent as SettingIcon } from "../../assets/img/SettingIcon.svg"; // You can later replace different icons per action

const options = [
    { label: "Move", action: "move" },
    { label: "Tag", action: "tag" },
    { label: "Preview", action: "preview" },
    { label: "Download", action: "download" },
    { label: "Delete", action: "delete" },
    { label: "Cancel", action: "cancel" },
];

const SeeMoreModal = ({ onOpen, onClose, onFileSelected, onOptionSelect }) => {
    const handleOptionClick = (action) => {
        if (onOptionSelect) {
            onOptionSelect(action);
        }
        if (action === "cancel") {
            onClose();
        }
    };

    return (
        <CustomModal isOpen={onOpen} modalName="" close={onClose} animationDirection="bottom" position="bottom" closeOnOverlayClick={false}>
            <div className="see-more-modal-container">
                {options.map((option, idx) => (
                    <div key={idx} className="see-more-item" onClick={() => handleOptionClick(option.action)}>
                        <div className="see-more-icon">
                            <SettingIcon />
                        </div>
                        <div className="see-more-label">{option.label}</div>
                    </div>
                ))}
            </div>
        </CustomModal>
    );
};

export default SeeMoreModal;
