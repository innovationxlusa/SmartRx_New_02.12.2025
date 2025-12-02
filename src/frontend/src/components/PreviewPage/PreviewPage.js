import React, { useState, useEffect, useRef } from "react";
import "./PreviewPage.css";
import SeeMoreModal from "../SeeMore/SeeMoreModal";
import { Link, useNavigate } from "react-router-dom";
import useApiClients from "../../services/useApiClients";
import CustomButton from "../static/Commons/CustomButton";
import { useUserContext } from "../../contexts/UserContext";
import { usePreviewContext } from "../../contexts/PreviewContext";
import ImagePreviewModal from "../ImagePreviewModal/ImagePreviewModal";
import { IoIosArrowDropleft, IoIosArrowDropright } from "react-icons/io";
import { Viewer } from "@react-pdf-viewer/core";
import { defaultLayoutPlugin } from "@react-pdf-viewer/default-layout";
import "@react-pdf-viewer/core/lib/styles/index.css";
import "@react-pdf-viewer/default-layout/lib/styles/index.css";
import RewardPopup from "../../components/Reward/RewardPopup";
import ConsumePopup from "../../components/Reward/ConsumePopup";
// import { Document, Page, pdfjs } from 'react-pdf';

const PreviewPage = () => {
    const navigate = useNavigate();
    const { user } = useUserContext();
    const { previewData } = usePreviewContext();

    // Destructuring API service methods
    const { api } = useApiClients();

    const initialData = {
        PrescriptionId: "",
        UpdatedBy: "",
    };

    const isPdfFile = (file) => file?.type === "application/pdf";
    const isImageFile = (file) => file?.type?.startsWith("image/");

    const [formData, setFormData] = useState(initialData);
    const [isLoading, setIsLoading] = useState(false);
    const [imageFIle, setImageFile] = useState(null);
    const [modalType, setModalType] = useState(null);
    const [currentFileIndex, setCurrentFileIndex] = useState(0);
    const [pdfUrl, setPdfUrl] = useState(null);
    const defaultLayoutPluginInstance = defaultLayoutPlugin();
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [showConsumePopup, setShowConsumePopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    // Guard clause to prevent errors
    if (!previewData)
        return <div className="text-center mt-5">No file found</div>;

    // Destructure or use previewData
    const {
        croppedImages = [],
        pdfFiles = [],
        serverData,
        folder,
    } = previewData;

    // Combine all files for proper navigation
    const allFiles = [...croppedImages, ...pdfFiles];
    const currentFile = allFiles[currentFileIndex];
    const totalFiles = allFiles.length;

    const handleNextFile = () => {
        if (currentFileIndex < totalFiles - 1) {
            setCurrentFileIndex(currentFileIndex + 1);
        }
    };

    const handlePrevFile = () => {
        if (currentFileIndex > 0) {
            setCurrentFileIndex(currentFileIndex - 1);
        }
    };

    const handleFileSelected = (value) => {
        // Pass all files to the preview modal
        setImageFile(allFiles);
        closeModal();
        setTimeout(() => openModal("view"), 300);
    };

    // PDF Preview Component - Direct Display
    const PDFPreviewComponent = ({ file }) => {
        const [pdfUrl, setPdfUrl] = useState(null);

        useEffect(() => {
            if (file) {
                const url = URL.createObjectURL(file);
                setPdfUrl(url);

                // Clean up previous URL
                return () => {
                    if (pdfUrl) {
                        URL.revokeObjectURL(pdfUrl);
                    }
                };
            }
        }, [file]);

        return (
            <div className="pdf-preview-component">
                <div className="pdf-viewer-wrapper">
                    {pdfUrl && (
                        <div className="pdf-plugin-viewer">
                            <div className="pdf-plugin-container">
                                <div className="pdf-plugin-content">
                                    <Viewer
                                        fileUrl={pdfUrl}
                                        plugins={[defaultLayoutPluginInstance]}
                                    />
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    // Create a custom preview modal for File objects
    const CustomFilePreviewModal = ({ isOpen, onClose, files }) => {
        const [currentIndex, setCurrentIndex] = useState(0);

        if (!isOpen || !files || files.length === 0) return null;

        const currentFile = files[currentIndex];
        const isImage = isImageFile(currentFile);
        const isPdf = isPdfFile(currentFile);

        const handlePrev = () => {
            if (currentIndex > 0) {
                setCurrentIndex(currentIndex - 1);
            }
        };

        const handleNext = () => {
            if (currentIndex < files.length - 1) {
                setCurrentIndex(currentIndex + 1);
            }
        };

        return (
            <>
                {isOpen && (
                    <div
                        className="modal-backdrop show"
                        onClick={onClose}
                    ></div>
                )}
                <div
                    className={`modal ${isOpen ? "show" : ""}`}
                    style={{
                        display: isOpen ? "block" : "none",
                        zIndex: 1050,
                        position: "fixed",
                        top: 0,
                        left: 0,
                        width: "100%",
                        height: "100%",
                    }}
                >
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">
                                    Preview - File {currentIndex + 1} of{" "}
                                    {files.length}
                                    <span className="ms-2 text-muted">
                                        (
                                        {isImage
                                            ? "Image"
                                            : isPdf
                                              ? "PDF"
                                              : "File"}
                                        )
                                    </span>
                                </h5>
                                <button
                                    type="button"
                                    className="btn-close"
                                    onClick={onClose}
                                ></button>
                            </div>
                            <div className="modal-body text-center">
                                {isImage && (
                                    <img
                                        src={URL.createObjectURL(currentFile)}
                                        className="img-fluid"
                                        alt={`Preview ${currentIndex + 1}`}
                                        style={{ maxHeight: "70vh" }}
                                    />
                                )}
                                {isPdf && (
                                    <PDFPreviewComponent file={currentFile} />
                                )}
                            </div>
                            {files.length > 1 && (
                                <div className="modal-footer justify-content-between">
                                    <button
                                        className="btn btn-outline-secondary"
                                        onClick={handlePrev}
                                        disabled={currentIndex === 0}
                                    >
                                        <IoIosArrowDropleft /> Previous
                                    </button>
                                    <span className="text-muted">
                                        {currentIndex + 1} of {files.length}
                                    </span>
                                    <button
                                        className="btn btn-outline-secondary"
                                        onClick={handleNext}
                                        disabled={
                                            currentIndex === files.length - 1
                                        }
                                    >
                                        Next <IoIosArrowDropright />
                                    </button>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </>
        );
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!serverData) return;

        try {
            setIsLoading(true);

            const newData = {
                ...formData,
                PrescriptionId: serverData?.id,
                UpdatedBy: Number(user?.UserId),
            };
            // API call to assign roles to the selected user
            const response = await api.requestForSmartRxUpload(
                newData,
                serverData?.id,
                "",
            );

            setUploadResponse(response);

            // If API call is successful or returns an object, reset the form
            if (
                response.message === "Successful" ||
                typeof response === "object"
            ) {
                // Pass croppedImageURL while navigating
                {
                    if (
                        response?.response?.isRewardUpdated == true &&
                        response?.response?.totalRewardPoints > 0 &&
                        response?.response?.requestedForSmartRx != true
                    ) {
                        setShowRewardPopup(true);
                        setTimeout(() => {
                            setShowRewardPopup(false);
                            navigate("/all-patient");
                        }, 3000);
                    } else if (
                        response?.response?.isRewardUpdated == true &&
                        response?.response?.requestedForSmartRx == true
                    ) {
                        setShowConsumePopup(true);
                        setTimeout(() => {
                            setShowConsumePopup(false);
                            navigate("/all-patient");
                        }, 3000);
                    } else {
                        navigate("/all-patient");
                    }
                }
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false); // Reset loading state
        }
    };

    return (
        <div className="container-fluid preview-page">
            <div className="row justify-content-center">
                <div className="col-12 col-lg-10 col-xl-8">
                    {/* Image Preview */}
                    <div className="row justify-content-center mb-4">
                        {totalFiles > 1 && (
                            <div className="col-12 mb-3">
                                <div className="file-navigation d-flex justify-content-between align-items-center px-3">
                                    <button
                                        onClick={handlePrevFile}
                                        disabled={currentFileIndex === 0}
                                        className="btn btn-outline-secondary border-0"
                                        style={{
                                            padding: "8px 12px",
                                            cursor:
                                                currentFileIndex === 0
                                                    ? "not-allowed"
                                                    : "pointer",
                                            opacity:
                                                currentFileIndex === 0
                                                    ? 0.5
                                                    : 1,
                                        }}
                                    >
                                        <IoIosArrowDropleft size={20} />
                                    </button>
                                    <span className="fw-medium">
                                        File {currentFileIndex + 1} of{" "}
                                        {totalFiles}
                                        {currentFile && (
                                            <span className="ms-2 text-muted">
                                                (
                                                {isImageFile(currentFile)
                                                    ? "Image"
                                                    : isPdfFile(currentFile)
                                                      ? "PDF"
                                                      : "File"}
                                                )
                                            </span>
                                        )}
                                    </span>
                                    <button
                                        onClick={handleNextFile}
                                        disabled={
                                            currentFileIndex === totalFiles - 1
                                        }
                                        className="btn btn-outline-secondary border-0"
                                        style={{
                                            padding: "8px 12px",
                                            cursor:
                                                currentFileIndex ===
                                                totalFiles - 1
                                                    ? "not-allowed"
                                                    : "pointer",
                                            opacity:
                                                currentFileIndex ===
                                                totalFiles - 1
                                                    ? 0.5
                                                    : 1,
                                        }}
                                    >
                                        <IoIosArrowDropright size={20} />
                                    </button>
                                </div>
                            </div>
                        )}
                        <div className="col-12 text-center image-preview">
                            {isImageFile(currentFile) && (
                                <img
                                    src={URL.createObjectURL(currentFile)}
                                    className="img rounded border"
                                    alt="Cropped Preview"
                                />
                            )}
                            {isPdfFile(currentFile) && (
                                <div className="mx-auto text-center">
                                    <p className="mb-3">Preview of the PDF:</p>
                                    <PDFPreviewComponent file={currentFile} />
                                </div>
                            )}
                        </div>
                    </div>

                    {/* Form Section */}
                    <form
                        onSubmit={handleSubmit}
                        className="row justify-content-center"
                    >
                        <div className="col-12 col-md-8 col-lg-6">
                            {/* Buttons */}
                            <div className="footer d-flex flex-column align-items-center">
                                <div className="w-100 mb-4">
                                    <CustomButton
                                        isLoading={isLoading}
                                        type="button"
                                        label="Request For Smart RX"
                                        disabled={isLoading}
                                        width="100%"
                                        backgroundColor=""
                                        textColor="var(--theme-font-color)"
                                        shape="roundedSquare"
                                        borderStyle=""
                                        borderColor="1px solid var(--theme-font-color)"
                                        iconStyle={{
                                            color: "var(--theme-font-color)",
                                        }}
                                        labelStyle={{
                                            fontSize: "16px",
                                            fontWeight: "600",
                                            fontFamily: "Georama",
                                            textTransform: "capitalize",
                                        }}
                                        hoverEffect="theme"
                                        onClick={(e) => handleSubmit(e)}
                                    />
                                </div>
                                <div className="row w-100 g-3">
                                    <div className="col-6">
                                        <CustomButton
                                            isLoading={""}
                                            type="button"
                                            label="See More"
                                            disabled={isLoading}
                                            width="100%"
                                            backgroundColor=""
                                            textColor="var(--theme-font-color)"
                                            shape="roundedSquare"
                                            borderStyle=""
                                            borderColor="1px solid var(--theme-font-color)"
                                            iconStyle={{
                                                color: "var(--theme-font-color)",
                                            }}
                                            labelStyle={{
                                                fontSize: "16px",
                                                fontWeight: "600",
                                                fontFamily: "Georama",
                                                textTransform: "capitalize",
                                            }}
                                            hoverEffect="theme"
                                            onClick={() => openModal("add")}
                                        />
                                    </div>
                                    <div className="col-6">
                                        <Link
                                            to="/all-patient"
                                            className="text-decoration-none"
                                        >
                                            <CustomButton
                                                isLoading={""}
                                                type="button"
                                                label="Close"
                                                disabled={isLoading}
                                                width="100%"
                                                backgroundColor=""
                                                textColor="var(--theme-font-color)"
                                                shape="roundedSquare"
                                                borderStyle=""
                                                borderColor="1px solid var(--theme-font-color)"
                                                iconStyle={{
                                                    color: "var(--theme-font-color)",
                                                }}
                                                labelStyle={{
                                                    fontSize: "16px",
                                                    fontWeight: "600",
                                                    fontFamily: "Georama",
                                                    textTransform: "capitalize",
                                                }}
                                                hoverEffect="theme"
                                            />
                                        </Link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            <SeeMoreModal
                onOpen={modalType === "add"}
                onClose={closeModal}
                onOptionSelect={handleFileSelected}
            />
            <CustomFilePreviewModal
                isOpen={modalType === "view"}
                onClose={closeModal}
                files={imageFIle}
            />
            <RewardPopup
                isOpen={showRewardPopup}
                onClose={() => {
                    setShowRewardPopup(false);
                }}
                points={uploadResponse?.response?.totalRewardPoints || 0}
                message={uploadResponse?.response?.rewardMessage || ""}
            />

            <ConsumePopup
                isOpen={showConsumePopup}
                onClose={() => {
                    setShowConsumePopup(false);
                }}
                consumeAmount={uploadResponse?.response?.totalRewardPoints || 0}
                message={uploadResponse?.response?.rewardMessage || ""}
            />
        </div>
    );
};

export default PreviewPage;
