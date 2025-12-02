import { useState } from "react";
import "./Footer.css";
import { FaHome } from "react-icons/fa";
import { Link } from "react-router-dom";
import { FiUpload } from "react-icons/fi";
import CropModal from "../../FileUpload/CropModal";
import UploadModal from "../../FileUpload/UploadModal";
import { useFolder } from "../../../contexts/FolderContext";
import FileManagementModal from "../../FileUpload/FileManagementModal";
import FolderManagementModal from "../../BrowseRx/FolderManagementModal";
import { handleOpenModal, handleCloseModal } from "../../../utils/utils";
import { ReactComponent as PlusIcon } from "../../../assets/img/PlusIcon.svg";
import { ReactComponent as RewardIcon } from "../../../assets/img/RewardIcon2.svg";
import { ReactComponent as House } from "../../../assets/img/House.svg";
import { ReactComponent as Settings } from "../../../assets/img/Settings.svg";
import { FaRankingStar } from "react-icons/fa6";
import { IoSettingsSharp } from "react-icons/io5";

const Footer = () => {
    const [modalType, setModalType] = useState(null);
    const [selectedFile, setSelectedFile] = useState(null);
    const [showCamera, setShowCamera] = useState(false);

    const { selectedFolder, refetch } = useFolder();

    const openModal = (type) => {
        setModalType(type); // Open the modal and set the type
    };

    const closeModal = () => {
        setModalType(null); // Close the modal
    };

    const handleFileSelected = (fileData = {}, nextModalType) => {
        const files = Array.isArray(fileData.File) ? fileData.File : [fileData.File];
        if (files.length > 0 && files[0] instanceof File) {
            setSelectedFile(fileData); // Store entire fileData including file array
        } else {
        }

        fileData.Webcam && setShowCamera(true);

        setTimeout(() => {
            closeModal();
            setTimeout(() => {
                openModal(nextModalType);
            }, 100);
        }, 100);
    };

    const handleCropDone = (croppedBlob) => {
        setIsCropOpen(false);
    };

    return (
        <>
            <div className="footer-menu">
                <div className="footer-background">
                    {/* Upload Button */}
                    <div
                        className="upload-btn"
                        onClick={() => openModal("upload")}
                    >
                        <button className="upload-circle-button">
                            {/* <FiUpload /> */}
                            <PlusIcon />
                        </button>
                    </div>
                    <div className="v-shape"></div>
                    <div
                        className="d-flex align-items-bottom justify-content-between"
                        style={{ gap: "clamp(1.5rem, 4vw, 3rem)" }}
                    >
                        {/* Home Button */}
                        <div className="home-btn d-flex align-items-center">
                            <Link
                                to="/all-patient"
                                className="text-decoration-none"
                                style={{ color: "#fff" }}
                            >
                                {/* <FaHome /> */}
                                <House />
                            </Link>
                            <span>HOME</span>
                        </div>
                        {/* Reward Button */}
                        <div className="home-btn d-flex align-items-center">
                            <Link
                                to="/rewardPoint"
                                className="text-decoration-none"
                                style={{ color: "#fff" }}
                            >
                                <RewardIcon />
                            </Link>
                            <span>REWARD</span>
                        </div>
                    </div>

                    <div
                        className="d-flex align-items-center justify-content-between"
                        style={{ gap: "clamp(1.5rem, 4vw, 3rem)" }}
                    >
                        {/* Rank Button */}
                        <div className="home-btn d-flex align-items-center">
                            <Link
                                to="/profileDetails"
                                className="text-decoration-none"
                                style={{ color: "#fff" }}
                            >
                                <FaRankingStar />
                            </Link>
                            <span>RANK</span>
                        </div>

                        {/* Settings Button */}
                        <div className="home-btn d-flex align-items-center">
                            <Link
                                to="/profileDetails"
                                className="text-decoration-none"
                                style={{ color: "#fff" }}
                            >
                                <IoSettingsSharp />
                            </Link>
                            <span>SETTINGS</span>
                        </div>
                    </div>
                </div>
            </div>
            <UploadModal
                isOpen={modalType === "upload"}
                onClose={closeModal}
                showCamera={showCamera}
                setShowCamera={setShowCamera}
                onFileSelected={handleFileSelected}
            />
            <CropModal
                isOpen={modalType === "crop"}
                onClose={closeModal}
                files={selectedFile || []}
                folderData={selectedFolder}
                onCropDone={handleCropDone}
                onFileSelected={handleFileSelected}
            />
            {/* Modal for new folder related actions */}
            <FolderManagementModal
                modalType={modalType}
                isOpen={modalType === "add"}
                folderData={selectedFolder}
                onClose={closeModal}
                fetchFolders={refetch}
            />
            <FileManagementModal
                modalType={modalType?.split(" ")[0]}
                isOpen={modalType === "upload file"}
                fileData={selectedFolder}
                onClose={closeModal}
                fetchFolders={refetch}
                onFileSelected={handleFileSelected}
            />
        </>
    );
};

export default Footer;
