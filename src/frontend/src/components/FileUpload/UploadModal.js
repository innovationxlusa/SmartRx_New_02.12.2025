import { useState, useRef, useEffect } from "react";
import "./UploadModal.css";
import Webcam from "react-webcam";
import { FaPlus } from "react-icons/fa6";
import { IoClose } from "react-icons/io5";
import { Spinner } from "react-bootstrap";
import { validateField } from "../../utils/validators";
import CustomInput from "../static/Commons/CustomInput";
import useFormHandler from "../../hooks/useFormHandler";
import CustomButton from "../static/Commons/CustomButton";
import CustomModal from "../static/CustomModal/CustomModal";
import { useUserContext } from "../../contexts/UserContext";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import { ReactComponent as Camera } from "../../assets/img/Capture.svg";
import { ReactComponent as ScanIcon } from "../../assets/img/ScanIcon.svg";
import { ReactComponent as FileUpload } from "../../assets/img/FileUpload.svg";
import { ReactComponent as FolderSmall } from "../../assets/img/FolderSmall.svg";

// Constants for PDF-like dimensions (A4 at 72 DPI)
const PDF_WIDTH = 656;
const PDF_HEIGHT = 850;

const UploadModal = ({ isOpen, onClose, showCamera, setShowCamera, onFileSelected }) => {
    const { handleInputChange, resetForm } = useFormHandler();
    const { user } = useUserContext();
    const userId = useCurrentUserId();

    const generateDefaultFileName = () => {
        const now = new Date();
        const yyyy = String(now.getFullYear());
        const MM = String(now.getMonth() + 1).padStart(2, "0");
        const dd = String(now.getDate()).padStart(2, "0");
        const HH = String(now.getHours()).padStart(2, "0");
        const mm = String(now.getMinutes()).padStart(2, "0");
        const ss = String(now.getSeconds()).padStart(2, "0");
        return `${yyyy}${MM}${dd}${HH}${mm}${ss}_file`;
    };

    const createInitialData = () => ({
        FileName: generateDefaultFileName(),
        File: [],
        Webcam: true,
        FolderId: user?.PrimaryFolderId,
        UserId: Number(userId) || 0,
        LoginUserId: Number(userId) || 0,
    });

    const initialData = createInitialData();

    // State to manage form data
    const [formData, setFormData] = useState(initialData);

    // State to manage individual field errors
    const [fieldErrors, setFieldErrors] = useState(initialData);

    // State to manage loading status
    const [isLoading, setIsLoading] = useState(false);
    const [isWebcamLoading, setIsWebcamLoading] = useState(false);
    const [capturedImages, setCapturedImages] = useState([]);
    const [selectedIndex, setSelectedIndex] = useState(0);

    const webcamRef = useRef(null);
    const thumbScrollRef = useRef(null);

    // Responsive video constraints based on screen size
    const getResponsiveConstraints = () => {
        const width = typeof window !== "undefined" ? window.innerWidth : 1280;
        if (width < 480) {
            return { width: { ideal: 640 }, height: { ideal: 480 }, facingMode: "environment", aspectRatio: 4 / 3 };
        }
        if (width < 768) {
            return { width: { ideal: 960 }, height: { ideal: 720 }, facingMode: "environment", aspectRatio: 4 / 3 };
        }
        return { width: { ideal: 1280 }, height: { ideal: 960 }, facingMode: "environment", aspectRatio: 4 / 3 };
    };

    const [videoConstraints, setVideoConstraints] = useState(getResponsiveConstraints());

    // Responsive top margin for modal container
    const getResponsiveTopMargin = () => {
        const width = typeof window !== "undefined" ? window.innerWidth : 1280;
        if (width < 480) return 12;
        if (width < 768) return 14;
        if (width < 1024) return 16;
        if (width < 1280) return 20;
        return 24;
    };
    const [containerTopMargin, setContainerTopMargin] = useState(getResponsiveTopMargin());

    useEffect(() => {
        const onResize = () => {
            setVideoConstraints(getResponsiveConstraints());
            setContainerTopMargin(getResponsiveTopMargin());
        };
        window.addEventListener("resize", onResize);
        return () => window.removeEventListener("resize", onResize);
    }, []);

    useEffect(() => {
        if (!isOpen) {
            setShowCamera(false);
            resetForm(createInitialData(), setFormData, setFieldErrors);
            setCapturedImages([]);
            setSelectedIndex(0);
        }
    }, [isOpen]);

    useEffect(() => {
        if (thumbScrollRef.current) {
            thumbScrollRef.current.scrollTo({
                left: thumbScrollRef.current.scrollWidth,
                behavior: "smooth",
            });
        }
    }, [capturedImages]);

    // useEffect(() => {
    //     if (thumbScrollRef.current) {
    //         // Scroll to the start instead of the end
    //         thumbScrollRef.current.scrollTo({
    //             left: 0,
    //             behavior: "smooth",
    //         });
    //     }
    // }, [capturedImages]);

    const resizeImage = (imageSrc, width, height) => {
        return new Promise((resolve) => {
            const img = new Image();
            img.onload = () => {
                const canvas = document.createElement("canvas");
                canvas.width = width;
                canvas.height = height;
                const ctx = canvas.getContext("2d");
                ctx.drawImage(img, 0, 0, width, height);
                resolve(canvas.toDataURL("image/jpeg"));
            };
            img.src = imageSrc;
        });
    };

    const handleCaptureClick = () => {
        // Ensure a default filename exists before showing camera
        setFormData((prev) => ({
            ...prev,
            FileName: prev.FileName && prev.FileName.trim() ? prev.FileName : generateDefaultFileName(),
        }));
        setFieldErrors((prev) => ({ ...prev, FileName: "" }));
        setShowCamera(true);
        setIsWebcamLoading(true);
    };

    const captureImage = async () => {
        const imageSrc = webcamRef.current.getScreenshot();
        if (imageSrc) {
            const resized = await resizeImage(imageSrc, PDF_WIDTH, PDF_HEIGHT);
            setCapturedImages((prev) => {
                const updated = [...prev, resized];
                setSelectedIndex(updated.length - 1);
                return updated;
            });
            setShowCamera(false); // Close camera after capture
        }
    };

    const handleAddMoreClick = () => {
        setShowCamera(true);
        setIsWebcamLoading(true);
    };

    const handleDeleteImage = (index) => {
        const newImages = capturedImages.filter((_, i) => i !== index);
        setCapturedImages(newImages);
        setSelectedIndex((prev) => {
            if (prev === index) return Math.max(0, prev - 1);
            if (prev > index) return prev - 1;
            return prev;
        });
    };

    const handleSaveAllCaptures = async () => {
        // Ensure filename presence; generate if missing
        const effectiveFileName = (formData.FileName && formData.FileName.trim()) ? formData.FileName : generateDefaultFileName();
        if (effectiveFileName !== formData.FileName) {
            setFormData((prev) => ({ ...prev, FileName: effectiveFileName }));
        }
        // Clear any stale filename errors when a valid default is present
        setFieldErrors((prev) => ({ ...prev, FileName: "" }));

        try {
            setIsLoading(true);
            const imageFiles = await Promise.all(
                capturedImages.map(async (img, i) => {
                    const blob = await fetch(img).then((res) => res.blob());
                    return new File([blob], `${effectiveFileName || "captured"}-${i + 1}.jpg`, {
                        type: "image/jpeg",
                        lastModified: Date.now(),
                    });
                })
            );

            onFileSelected({ ...formData, File: imageFiles }, "crop");
            onClose();
            resetForm(createInitialData(), setFormData, setFieldErrors);
        } catch (error) {
            console.error("Save error:", error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleCloseCamera = () => {
        setShowCamera(false);
    };

    const handleUploadClick = () => {
        document.getElementById("fileInput").click();
    };

    const handleFileChange = (e) => {
        const selectedFile = e.target.files[0];
        if (selectedFile) {
            onFileSelected(selectedFile, "crop");
            onClose();
        }
    };

    const listItems = [
        { label: "New Folder", icon: <FolderSmall className="upload-icon" />, onClick: () => onFileSelected({}, "add") },
        { label: "Capture", icon: <Camera className="upload-icon" />, onClick: handleCaptureClick },
        { label: "Files Upload", icon: <FileUpload className="upload-icon" />, onClick: () => onFileSelected({}, "upload file") },
        { label: "Scan", icon: <ScanIcon className="upload-icon" />, onClick: handleUploadClick },
    ];

    return (
        <CustomModal isOpen={isOpen} close={onClose} animationDirection="bottom" position="bottom" closeOnOverlayClick={false}>
            <div className="upload-modal-container">
                {!showCamera ? (
                    <>
                        {capturedImages.length === 0 ? (
                            <>
                                <div className="upload-section">
                                    <h5 className="primary-tab-title text-start">CREATE</h5>
                                    <div className="upload-list">
                                        <div className="upload-item" onClick={listItems[0].onClick}>
                                            <div className="upload-icon-container">{listItems[0].icon}</div>
                                            <span className="upload-label">{listItems[0].label}</span>
                                        </div>
                                    </div>
                                </div>
                                <div className="upload-section">
                                    <h5 className="primary-tab-title text-start">IMPORT</h5>
                                    <div className="upload-list">
                                        {listItems.slice(1).map((item, index) => (
                                            <div key={index} className="upload-item" onClick={item.onClick}>
                                                <div className="upload-icon-container">{item.icon}</div>
                                                <span className="upload-label">{item.label}</span>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            </>
                        ) : (
                            <div className="image-carousel">
                                <CustomInput
                                    className="input-style"
                                    label="File Name"
                                    labelPosition="top-left"
                                    name="FileName"
                                    type="text"
                                    placeholder="File Name"
                                    value={formData.FileName}
                                    onChange={(e) => handleInputChange(e, setFormData, setFieldErrors, "input", "File name")}
                                    error={fieldErrors.FileName}
                                    disabled={isLoading}
                                    style={{ marginBottom: "12px" }}
                                />

                                <div className="carousel-main">{capturedImages[selectedIndex] && <img src={capturedImages[selectedIndex]} alt="Selected image" />}</div>

                                <div className="carousel-thumbnails" ref={thumbScrollRef}>
                                    {capturedImages.map((img, idx) => (
                                        <div key={idx} className={`carousel-thumb ${selectedIndex === idx ? "active" : ""}`} onClick={() => setSelectedIndex(idx)}>
                                            <img src={img} className="img-fluid" alt={`capture-${idx}`} />
                                            <div
                                                className="delete-thumb"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleDeleteImage(idx);
                                                }}
                                            >
                                                <IoClose />
                                            </div>
                                        </div>
                                    ))}
                                    <div className="carousel-thumb add-more" onClick={handleAddMoreClick}>
                                        <FaPlus className="plus-icon" />
                                    </div>
                                </div>

                                <div className="camera-controls">
                                    <CustomButton
                                        isLoading={isLoading || isWebcamLoading}
                                        type="button"
                                        icon=""
                                        label="Cancel"
                                        onClick={handleCloseCamera}
                                        disabled=""
                                        width="100%"
                                        backgroundColor=""
                                        textColor="var(--theme-font-color)"
                                        shape="roundedSquare"
                                        borderStyle=""
                                        borderColor="1px solid var(--theme-font-color)"
                                        iconStyle={{ color: "var(--theme-font-color)" }}
                                        labelStyle={{
                                            fontSize: "16px",
                                            fontWeight: "400",
                                            fontFamily: "Source Sans Pro",
                                            textTransform: "capitalize",
                                        }}
                                        hoverEffect="theme"
                                    />
                                    <CustomButton
                                        isLoading={isLoading || isWebcamLoading}
                                        type="button"
                                        icon=""
                                        label="Save"
                                        onClick={handleSaveAllCaptures}
                                        disabled={isLoading || isWebcamLoading}
                                        width="100%"
                                        backgroundColor=""
                                        textColor="var(--theme-font-color)"
                                        shape="roundedSquare"
                                        borderStyle=""
                                        borderColor="1px solid var(--theme-font-color)"
                                        iconStyle={{ color: "var(--theme-font-color)" }}
                                        labelStyle={{
                                            fontSize: "16px",
                                            fontWeight: "400",
                                            fontFamily: "Source Sans Pro",
                                            textTransform: "capitalize",
                                        }}
                                        hoverEffect="theme"
                                    />
                                </div>
                            </div>
                        )}
                    </>
                ) : (
                    <>
                        <div className="camera-container">
                            {isWebcamLoading && (
                                <div className="h-100 d-flex align-items-center flex-column justify-content-center" style={{ height: "140px" }}>
                                    <Spinner color="inherit" animation="border" size="sm" />
                                    <p>Loading camera...</p>
                                </div>
                            )}

                            <Webcam
                                audio={false}
                                ref={webcamRef}
                                screenshotFormat="image/jpeg"
                                videoConstraints={videoConstraints}
                                className="camera-video"
                                mirrored={false}
                                onUserMedia={() => setIsWebcamLoading(false)}
                                onUserMediaError={() => {
                                    setIsWebcamLoading(false);
                                    // I might want to handle errors here
                                }}
                            />
                        </div>
                        <div className="camera-controls">
                            <CustomButton
                                isLoading={isLoading}
                                type="button"
                                icon=""
                                label="Cancel"
                                onClick={() => {
                                    if (capturedImages.length > 0) {
                                        setShowCamera(false);
                                    } else {
                                        onClose();
                                    }
                                }}
                                disabled={isLoading || isWebcamLoading}
                                width="100%"
                                backgroundColor=""
                                textColor="var(--theme-font-color)"
                                shape="roundedSquare"
                                borderStyle=""
                                borderColor="1px solid var(--theme-font-color)"
                                iconStyle={{ color: "var(--theme-font-color)" }}
                                labelStyle={{
                                    fontSize: "16px",
                                    fontWeight: "400",
                                    fontFamily: "Source Sans Pro",
                                    textTransform: "capitalize",
                                }}
                                hoverEffect="theme"
                            />
                            <CustomButton
                                isLoading={isLoading}
                                type="button"
                                icon=""
                                label="Capture"
                                onClick={captureImage}
                                disabled={isLoading || isWebcamLoading}
                                width="100%"
                                backgroundColor=""
                                textColor="var(--theme-font-color)"
                                shape="roundedSquare"
                                borderStyle=""
                                borderColor="1px solid var(--theme-font-color)"
                                iconStyle={{ color: "var(--theme-font-color)" }}
                                labelStyle={{
                                    fontSize: "16px",
                                    fontWeight: "400",
                                    fontFamily: "Source Sans Pro",
                                    textTransform: "capitalize",
                                }}
                                hoverEffect="theme"
                            />
                        </div>
                    </>
                )}
                <input id="fileInput" type="file" style={{ display: "none" }} onChange={handleFileChange} accept="image/*,.pdf" />
            </div>
        </CustomModal>
    );
};

export default UploadModal;
