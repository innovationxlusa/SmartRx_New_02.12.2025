import { useState, useCallback, useEffect } from "react";
import "./CropModal.css";
import Cropper from "react-easy-crop";
import getCroppedImg from "../../utils/utils";
import { useNavigate } from "react-router-dom";
import CustomSlider from "../CustomSlider/CustomSlider";
import useApiClients from "../../services/useApiClients";
import CustomButton from "../static/Commons/CustomButton";
import CustomModal from "../static/CustomModal/CustomModal";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import { usePreviewContext } from "../../contexts/PreviewContext";
import { IoIosArrowDropleft, IoIosArrowDropright } from "react-icons/io";
import { useParams } from "react-router-dom";
import { useUserContext } from "../../contexts/UserContext";
import RewardPopup from "../../components/Reward/RewardPopup";

const CropModal = ({ isOpen, onClose, files, folderData, onCropDone, onFileSelected }) => {
    const navigate = useNavigate();
    const { user } = useUserContext();
    // State for each file's crop settings
    const [currentFileIndex, setCurrentFileIndex] = useState(0);
    const [cropSettings, setCropSettings] = useState(
        files?.File?.map(() => ({
            crop: { x: 0, y: 0 },
            zoom: 1,
            rotation: 0,
            brightness: 100,
            contrast: 100,
            extraFilter: "",
            aspect: 1,
            croppedAreaPixels: null,
        })) || []
    );

    const userId = useCurrentUserId();
    const { setPreviewData } = usePreviewContext();

    // State to manage loading status
    const [isLoading, setIsLoading] = useState(false);
    const [isReTakeLoading, setIsReTakeLoading] = useState(false);
    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    // Initial form data structure
    const initialData = {
        File: null,
        FileName: "",
        FolderName: "",
        UniqueFileId: "",
        InsertedBy: "",
        FilePath: "",
        SeqNo: "",
        FolderId: "",
        FolderHierarchy: "",
        UserId: Number(userId) || 0,
    };

    // State to manage form data
    const [formData, setFormData] = useState(initialData);

    // State to track field-level errors for form validation
    const [fieldErrors, setFieldErrors] = useState(initialData);

    // Destructuring API service methods
    const { api } = useApiClients();

    const { folderId: routeFolderId } = useParams();
    const routeFolderIdNum = Number(routeFolderId);
    const currentFolderId =
        Number.isFinite(routeFolderIdNum) && routeFolderIdNum > 0
            ? routeFolderIdNum
            : (folderData?.folderId ?? user?.PrimaryFolderId);

    useEffect(() => {
        if (!isOpen) {
            resetAllSettings();
            setUploadResponse(null);
            setShowRewardPopup(false);
        } else {
            // Initialize crop settings when modal opens with files
            setCropSettings(
                files?.File?.map(() => ({
                    crop: { x: 0, y: 0 },
                    zoom: 1,
                    rotation: 0,
                    brightness: 100,
                    contrast: 100,
                    extraFilter: "",
                    aspect: 1,
                    croppedAreaPixels: null,
                })) || []
            );
            setCurrentFileIndex(0);
        }
    }, [isOpen, files]);

    const isImageFile = (file) => file?.type?.startsWith("image/");
    const isPdfFile = (file) => file?.type === "application/pdf";

    const onCropComplete = useCallback(
        (croppedArea, croppedAreaPixels) => {
            setCropSettings((prev) => {
                const newSettings = [...prev];
                newSettings[currentFileIndex] = {
                    ...newSettings[currentFileIndex],
                    croppedAreaPixels,
                };
                return newSettings;
            });
        },
        [currentFileIndex]
    );

    const resetAllSettings = () => {
        setCropSettings(
            files?.File?.map(() => ({
                crop: { x: 0, y: 0 },
                zoom: 1,
                rotation: 0,
                brightness: 100,
                contrast: 100,
                extraFilter: "",
                aspect: 1,
                croppedAreaPixels: null,
            })) || []
        );
        setCurrentFileIndex(0);
    };

    const handleCrop = useCallback(async () => {
        if (!files?.File || !files?.File[currentFileIndex] || !cropSettings[currentFileIndex]?.croppedAreaPixels) return;

        try {
            const currentFile = files?.File[currentFileIndex];
            const currentSettings = cropSettings[currentFileIndex];
            const croppedImgBlob = await getCroppedImg(URL.createObjectURL(currentFile), currentSettings.croppedAreaPixels, currentSettings.rotation);
            const croppedImageURL = URL.createObjectURL(croppedImgBlob);
            onCropDone(croppedImageURL); // sending cropped image back
            onClose(); // close modal after crop done
        } catch (e) {
            console.error(e);
        }
    }, [cropSettings, currentFileIndex, files, onCropDone, onClose]);

    // if (!files?.File || files?.File.length === 0) return null;

    const currentFile = files?.File && files?.File[currentFileIndex];
    const currentSettings = cropSettings[currentFileIndex] || {};

    const handleRetake = () => {
        onFileSelected({ Webcam: true }, "upload");
    };

    const handleSave = async (e) => {
        e.preventDefault();

        if (!files?.File || files?.File?.length === 0) return;

        try {
            setIsLoading(true);

            // Process all files
            const processedFiles = [];
            const navigationState = {
                croppedImages: [],
                pdfFiles: [],
                serverData: null,
                folder: folderData,
            };

            for (let i = 0; i < files?.File?.length; i++) {
                const file = files?.File[i];
                const settings = cropSettings[i];

                if (isImageFile(file)) {
                    // If cropped area exists, use the cropped version, otherwise use original
                    if (settings?.croppedAreaPixels) {
                        const croppedImgBlob = await getCroppedImg(URL.createObjectURL(file), settings.croppedAreaPixels, settings.rotation);
                        const fileToSend = new File([croppedImgBlob], file.name, { type: file.type });
                        // processedFiles.push(fileToSend);
                        processedFiles.push(file);
                        // navigationState.croppedImages.push(URL.createObjectURL(file));
                        navigationState.croppedImages.push(file);
                    } else {
                        // No crop area selected - use original image
                        processedFiles.push(file);
                        // navigationState.croppedImages.push(URL.createObjectURL(file));
                        navigationState.croppedImages.push(file);
                    }
                } else if (isPdfFile(file)) {
                   
                    processedFiles.push(file);
                    // navigationState.pdfFiles.push(URL.createObjectURL(file));
                    navigationState.pdfFiles.push(file);
                } else {
                    console.error("Unsupported file type");
                    continue; // Skip this file instead of returning, so other files can still be processed
                }
            }
            
            // Prepare form data for upload
            const newData = {
                files: processedFiles,
                FileName: files?.FileName,
                LoginUserId: Number(userId) || 0,
                FolderId: currentFolderId?? folderData?.folderId,
                FileExtension: files?.FileExtension,
                IsCaputred: files?.Webcam ? true : false,
                IsScanned: files?.Scanned ? true : false,
                IsUploaded: !files?.Webcam && !files?.Scanned ? true : false,
            };
            const formDataToSend = new FormData();
            // Append each file
            newData.files.forEach((file) => {
                formDataToSend.append("files", file);
            });
            // Append additional fields (example)
            formDataToSend.append("FolderId", newData.FolderId);
            formDataToSend.append("FileName", newData.FileName);
            formDataToSend.append("LoginUserId", newData.LoginUserId || Number(userId) || 0);
            formDataToSend.append("FileExtension", newData.FileExtension);
            formDataToSend.append("IsCaputred", newData.IsCaputred);
            formDataToSend.append("IsScanned", newData.IsScanned);
            formDataToSend.append("IsUploaded", newData.IsUploaded);

            const response = await api.prescriptionUpload(
                formDataToSend,
                (uploadEvent) => {
                    const progress = Math.round((uploadEvent.loaded * 100) / uploadEvent.total);
                },
                ""
            );
            
            setUploadResponse(response);
        
            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                setPreviewData({
                    ...navigationState,
                    serverData: response.response,
                });
                
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        onClose();
                        navigate("/preview");
                    }, 3000);
                } else {
                    onClose();
                    navigate("/preview");
                }
            }
        } catch (error) {
            console.error("Error during upload:", error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleNextFile = () => {
        if (currentFileIndex < files?.File?.length - 1) {
            setCurrentFileIndex(currentFileIndex + 1);
        }
    };

    const handlePrevFile = () => {
        if (currentFileIndex > 0) {
            setCurrentFileIndex(currentFileIndex - 1);
        }
    };

    return (
        <CustomModal
            isOpen={isOpen}
            close={onClose}
            animationDirection="top"
            position="top"
            closeOnOverlayClick={false}
        >
            <div className="upload-modal-container">
                {/* {!isPdfFile(currentFile) && ( */}
                <div className="file-navigation d-flex justify-content-between align-items-center">
                    <button
                        onClick={handlePrevFile}
                        disabled={currentFileIndex === 0}
                        style={{
                            padding: "5px 10px 5px 0",
                            cursor:
                                currentFileIndex === 0
                                    ? "not-allowed"
                                    : "pointer",
                        }}
                    >
                        <IoIosArrowDropleft />
                    </button>
                    <span>
                        File {currentFileIndex + 1} of{" "}
                        {files?.File?.length || 0}
                    </span>
                    <button
                        onClick={handleNextFile}
                        disabled={currentFileIndex === files?.File?.length - 1}
                        style={{
                            padding: "5px 0 5px 10px",
                            cursor:
                                currentFileIndex === files?.File?.length - 1
                                    ? "not-allowed"
                                    : "pointer",
                        }}
                    >
                        <IoIosArrowDropright />
                    </button>
                </div>
                {/* )} */}
                <div className="cropper-wrapper">
                    {isImageFile(currentFile) && (
                        <Cropper
                            image={URL.createObjectURL(currentFile)}
                            crop={currentSettings.crop || { x: 0, y: 0 }}
                            zoom={currentSettings.zoom || 1}
                            rotation={currentSettings.rotation || 0}
                            aspect={currentSettings.aspect || 1}
                            onCropChange={(crop) => {
                                setCropSettings((prev) => {
                                    const newSettings = [...prev];
                                    newSettings[currentFileIndex] = {
                                        ...newSettings[currentFileIndex],
                                        crop,
                                    };
                                    return newSettings;
                                });
                            }}
                            onZoomChange={(zoom) => {
                                setCropSettings((prev) => {
                                    const newSettings = [...prev];
                                    newSettings[currentFileIndex] = {
                                        ...newSettings[currentFileIndex],
                                        zoom,
                                    };
                                    return newSettings;
                                });
                            }}
                            onRotationChange={(rotation) => {
                                setCropSettings((prev) => {
                                    const newSettings = [...prev];
                                    newSettings[currentFileIndex] = {
                                        ...newSettings[currentFileIndex],
                                        rotation,
                                    };
                                    return newSettings;
                                });
                            }}
                            onCropComplete={onCropComplete}
                            style={{
                                containerStyle: {
                                    filter: `brightness(${currentSettings.brightness || 100}%) contrast(${currentSettings.contrast || 100}%) ${currentSettings.extraFilter || ""}`,
                                },
                            }}
                        />
                    )}
                    {isPdfFile(currentFile) && (
                        <>
                            <div className="pdf-preview">
                                <p>Preview of the PDF:</p>
                                <iframe
                                    src={URL.createObjectURL(currentFile)}
                                    title="PDF Preview"
                                ></iframe>
                            </div>
                        </>
                    )}
                    {!isImageFile(currentFile) && !isPdfFile(currentFile) && (
                        <p>Unsupported file type</p>
                    )}
                </div>

                {isImageFile(currentFile) && (
                    <div className="editor-controls">
                        {/* Zoom Control */}
                        <div className="">
                            <label>Zoom</label>
                            <CustomSlider
                                value={currentSettings.zoom || 1}
                                min={1}
                                max={3}
                                step={0.1}
                                onChange={(zoom) => {
                                    setCropSettings((prev) => {
                                        const newSettings = [...prev];
                                        newSettings[currentFileIndex] = {
                                            ...newSettings[currentFileIndex],
                                            zoom,
                                        };
                                        return newSettings;
                                    });
                                }}
                            />
                        </div>
                        {/* Rotation Control */}
                        {/* <div className="control-group">
                            <label>Rotation</label>
                            <CustomSlider
                                value={currentSettings.rotation || 0}
                                min={0}
                                max={360}
                                step={1}
                                onChange={(rotation) => {
                                    setCropSettings((prev) => {
                                        const newSettings = [...prev];
                                        newSettings[currentFileIndex] = {
                                            ...newSettings[currentFileIndex],
                                            rotation,
                                        };
                                        return newSettings;
                                    });
                                }}
                            />
                        </div> */}
                    </div>
                )}

                {/* Action Buttons */}
                <div className="buttons">
                    {files?.Webcam && (
                        <CustomButton
                            isLoading={isReTakeLoading}
                            type={"button"}
                            icon={""}
                            label={"Re-Take"}
                            onClick={handleRetake}
                            disabled={isLoading || isReTakeLoading}
                            width={"100%"}
                            backgroundColor={""}
                            textColor={"var(--theme-font-color)"}
                            shape={"roundedSquare"}
                            borderStyle={""}
                            borderColor={"1px solid var(--theme-font-color)"}
                            iconStyle={{ color: "var(--theme-font-color)" }}
                            labelStyle={{
                                fontSize: "16px",
                                fontWeight: "600",
                                fontFamily: "Georama",
                                textTransform: "capitalize",
                            }}
                            hoverEffect={"theme"}
                        />
                    )}

                    <CustomButton
                        isLoading={isLoading}
                        type={"button"}
                        icon={""}
                        label={"Save"}
                        onClick={handleSave}
                        disabled={isLoading || isReTakeLoading}
                        width={"100%"}
                        backgroundColor={""}
                        textColor={"var(--theme-font-color)"}
                        shape={"roundedSquare"}
                        borderStyle={""}
                        borderColor={"1px solid var(--theme-font-color)"}
                        iconStyle={{ color: "var(--theme-font-color)" }}
                        labelStyle={{
                            fontSize: "16px",
                            fontWeight: "600",
                            fontFamily: "Georama",
                            textTransform: "capitalize",
                        }}
                        hoverEffect={"theme"}
                    />
                </div>
            </div>
            <RewardPopup
                isOpen={showRewardPopup}
                onClose={() => {
                    setShowRewardPopup(false);
                    onClose();
                    navigate("/preview");
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

export default CropModal;
