import { useState } from "react";
import { detectEnvironment } from "../../utils/envHelper";
import "./ImagePreviewModal.css";
import CustomModal from "../static/CustomModal/CustomModal";
import { IoIosArrowBack, IoIosArrowForward } from "react-icons/io";
import { IMAGE_HOST } from "../../config/config";
import useCurrentUserId from "../../hooks/useCurrentUserId";

const ImagePreviewModal = ({ onOpen, onClose, file, fileType }) => {
    const { env, hostname, apiPort } = detectEnvironment();
    const [currentIndex, setCurrentIndex] = useState(0);
    const [slideDirection, setSlideDirection] = useState(null); // 'left' or 'right'
    const userId = useCurrentUserId();
    const cacheSuffix = userId ? `userId=${userId}` : "userId=guest";

    const largeImages = (file && file?.filter && file?.filter((f) => f.includes("_large."))) || [];
    const largePDF = (file && file?.filter && file?.filter((f) => f.includes("_original."))) || [];

    const IMAGE_OR_PDF_URL = IMAGE_HOST;
    
    // Ensure currentIndex is within bounds
    const safeCurrentIndex = Math.min(currentIndex, Math.max(0, largeImages.length - 1, largePDF.length - 1));

    const handlePrev = () => {
        if (currentIndex > 0 && largeImages.length > 0) {
            setSlideDirection("right");
            setCurrentIndex(currentIndex - 1);
        }
    };

    const handleNext = () => {
        if (currentIndex < largeImages.length - 1 && largeImages.length > 0) {
            setSlideDirection("left");
            setCurrentIndex(currentIndex + 1);
        }
    };

    return (
        <CustomModal isOpen={onOpen} close={onClose} modalName="" isImagePreview={true} pdf={fileType === "PDF" && true} modalSize="huge">
            {fileType === "PDF" && file && largePDF.length > 0 ? (
                <>
                    <iframe
                        src={`${IMAGE_OR_PDF_URL}/${largePDF[safeCurrentIndex] ? largePDF[safeCurrentIndex].replace(/\\/g, "/") : ""}?file=...&disableAnnotation=true&${cacheSuffix}`}
                        className="custom-pdf-frame"
                        width="100%"
                        height="500px"
                        frameBorder="0"
                    />
                </>
            ) : fileType !== "PDF" && largeImages.length > 0 ? (
                <>
                    <img
                        key={largeImages[safeCurrentIndex] || `image-${safeCurrentIndex}`}
                        src={`${IMAGE_OR_PDF_URL}/${largeImages[safeCurrentIndex] ? largeImages[safeCurrentIndex].replace(/\\/g, "/") : ""}?${cacheSuffix}`}
                        className={`img-fluid slide-${slideDirection}`}
                        alt={`Preview ${currentIndex + 1}`}
                        onAnimationEnd={() => setSlideDirection(null)} // Reset after animation
                    />
                    {largeImages.length > 1 && (
                        <>
                            {/* Left Button */}
                            <button
                                onClick={handlePrev}
                                disabled={currentIndex === 0}
                                className="position-absolute"
                                style={{
                                    left: 0,
                                    top: "50%",
                                    transform: "translateY(-50%)",
                                    background: "none",
                                    border: "none",
                                    cursor: currentIndex === 0 ? "not-allowed" : "pointer",
                                    color: currentIndex === 0 ? "var( --content-color)" : "var(--text-white)",
                                }}
                            >
                                <div style={{ background: currentIndex === 0 ? "#333" : "var(--app-theme-color)", borderRadius: "50%", padding: "0" }}>
                                    <IoIosArrowBack style={{ fontSize: "34px" }} />
                                </div>
                            </button>
                            {/* Right Button */}
                            <button
                                onClick={handleNext}
                                disabled={currentIndex === largeImages.length - 1}
                                className="position-absolute"
                                style={{
                                    right: 0,
                                    top: "50%",
                                    transform: "translateY(-50%)",
                                    background: "none",
                                    border: "none",
                                    cursor: currentIndex === largeImages.length - 1 ? "not-allowed" : "pointer",
                                    color: currentIndex === largeImages.length - 1 ? "var( --content-color)" : "var(--text-white)",
                                }}
                            >
                                <div style={{ background: currentIndex === largeImages.length - 1 ? "#333" : "var(--app-theme-color)", borderRadius: "50%", padding: "0" }}>
                                    <IoIosArrowForward style={{ fontSize: "34px" }} />
                                </div>
                            </button>
                        </>
                    )}
                </>
            ) : (
                <div className="d-flex justify-content-center align-items-center" style={{ height: "400px" }}>
                    <div className="text-center">
                        <p className="text-muted">No preview available</p>
                    </div>
                </div>
            )}
        </CustomModal>
    );
};

export default ImagePreviewModal;
