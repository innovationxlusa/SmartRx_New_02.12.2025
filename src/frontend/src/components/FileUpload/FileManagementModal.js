import { useEffect, useState, useRef } from "react";
import { validateField } from "../../utils/validators";
import CustomInput from "../static/Commons/CustomInput";
import useFormHandler from "../../hooks/useFormHandler";
import useApiService from "../../services/useApiService";
import { useUserContext } from "../../contexts/UserContext";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import CustomModal from "../static/CustomModal/CustomModal";
import DeleteModal from "../static/Commons/FormFields/DeleteModal/DeleteModal";
import {
    CREATE_NEW_FOLDER_URL,
    DELETE_FILE_URL,
    RENAME_FOLDER_URL,
} from "../../constants/apiEndpoints";
import { useParams } from "react-router-dom";
import RewardPopup from "../Reward/RewardPopup";

const FileManagementModal = ({
    modalType,
    isOpen,
    fileData,
    onClose,
    fetchFolders,
    folderRefetch,
    onFileSelected,
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
    const { user } = useUserContext();
    const userId = useCurrentUserId();
    const apiServices = useApiService();

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const { folderId: routeFolderId } = useParams();
    const routeFolderIdNum = Number(routeFolderId);
    const currentFolderId =
        Number.isFinite(routeFolderIdNum) && routeFolderIdNum > 0
            ? routeFolderIdNum
            : (fileData?.folderId ?? user?.PrimaryFolderId);

    const fileInputRef = useRef([]); // 🚫 Avoid keeping large file blobs in React state

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

    const initialData = {
        PrescriptionId: fileData?.fileId ?? "",
        ParentFolderId:
            currentFolderId ?? user?.PrimaryFolderId ?? fileData?.folderId,
        PreviousFolderHierarchy:
            fileData?.folderName === "Primary" ? 0 : fileData?.folderHierarchy,
        FolderHierarchy:
            fileData?.folderHierarchy ?? user?.PrimaryFolderHeirarchy,
        FileName: generateDefaultFileName(),
        FolderId: fileData?.folderId ?? user?.PrimaryFolderId,
        UserId: Number(userId) || 0,
        LoginUserId: Number(userId) || 0,
    };

    const [formData, setFormData] = useState(initialData);
    const [fieldErrors, setFieldErrors] = useState({});
    const [isLoading, setIsLoading] = useState(false);

    const modalNames = dynamicModalName("File");
    const buttonLabels = dynamicButtonLabel("File");

    useEffect(() => {
        if ((modalType === "rename" || modalType === "delete") && fileData) {
            const {
                folderName,
                fileId,
                folderId,
                parentFolderId,
                folderHeirarchy,
            } = fileData;
            setFormData((prev) => ({
                ...prev,
                PrescriptionId: fileId,
                FolderId: folderId ?? user?.PrimaryFolderId,
                ParentFolderId: parentFolderId,
                PreviousFolderHierarchy: folderHeirarchy,
                FolderHierarchy: folderHeirarchy,
                FolderName: folderName || "",
            }));
        } else {
            const refreshedInitial = {
                ...initialData,
                FileName: generateDefaultFileName(),
            };
            resetForm(refreshedInitial, setFormData, setFieldErrors);
        }
    }, [modalType, fileData]);

    const formatFileError = (message) =>
        message ? <span style={{ fontFamily: "Georama" }}>{message}</span> : "";

    const handleFileChange = (e) => {
        const selectedFiles = Array.from(e.target.files || []);

        // Validate file types - only accept images and PDFs
        const allowedTypes = [
            "image/jpeg",
            "image/jpg",
            "image/png",
            "image/gif",
            "image/webp",
            "application/pdf",
        ];

        const validFiles = selectedFiles.filter((file) =>
            allowedTypes.includes(file.type),
        );
        const invalidFiles = selectedFiles.filter(
            (file) => !allowedTypes.includes(file.type),
        );

        let fileError = "";

        if (selectedFiles.length === 0) {
            fileError = "Please select at least one file.";
        } else if (invalidFiles.length > 0) {
            const invalidFileNames = invalidFiles
                .map((file) => file.name)
                .join(", ");
            fileError = `Invalid file type(s): ${invalidFileNames}. Only images (JPEG, PNG, GIF, WebP) and PDF files are allowed.`;
        } else if (validFiles.length !== selectedFiles.length) {
            fileError =
                "Some files were filtered out due to invalid file types.";
        }

        fileInputRef.current = validFiles;

        setFieldErrors((prev) => ({
            ...prev,
            File: formatFileError(fileError),
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (modalType !== "delete") {
            const fileNameError = validateField(
                "FileName",
                formData.FileName,
                "File Name",
            );

            // Additional validation for file types on submit
            const allowedTypes = [
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif",
                "image/webp",
                "application/pdf",
            ];

            let fileError = "";

            if (fileInputRef.current.length === 0) {
                fileError = "At least one file must be selected.";
            } else {
                // Double-check file types on submit
                const invalidFiles = fileInputRef.current.filter(
                    (file) => !allowedTypes.includes(file.type),
                );
                if (invalidFiles.length > 0) {
                    const invalidFileNames = invalidFiles
                        .map((file) => file.name)
                        .join(", ");
                    fileError = `Invalid file type(s): ${invalidFileNames}. Only images (JPEG, PNG, GIF, WebP) and PDF files are allowed.`;
                }
            }

            const fieldsToValidate = {
                FileName: fileNameError,
                File: formatFileError(fileError),
            };

            if (fileNameError || fileError) {
                setFieldErrors(fieldsToValidate);
                return;
            }
        }

        try {
            setIsLoading(true);

            const dynamicUrl = {
                add: CREATE_NEW_FOLDER_URL,
                rename: RENAME_FOLDER_URL,
                delete: DELETE_FILE_URL,
            };

            if (modalType === "delete") {
                const actions = dynamicActions(
                    dynamicUrl[modalType],
                    formData,
                    fileData?.fileId,
                    apiServices,
                    "",
                );
                console.log("Delete Action:", formData);
                const response = await actions.delete();
                console.log("Delete Response:", response);

                setUploadResponse(response);

                if (response?.message === "Successful") {
                    if (response?.response?.isRewardUpdated == true) {
                        setShowRewardPopup(true);
                        setTimeout(() => {
                            setShowRewardPopup(false);
                            fetchFolders?.();
                            folderRefetch?.();
                            onClose();
                        }, 3000);
                    } else {
                        fetchFolders?.();
                        folderRefetch?.();
                        onClose();
                    }
                }
            } else {
                // ⏫ Send files to crop (or upload)
                onFileSelected(
                    {
                        ...formData,
                        File: fileInputRef.current,
                    },
                    "crop",
                );
                onClose();
            }

            // Reset form
            resetForm(initialData, setFormData, setFieldErrors);
            fileInputRef.current = [];
        } catch (error) {
            console.error("File deletion error:", error);
            // Show error message to user
            alert(
                `Error ${modalType === "delete" ? "deleting" : "processing"} file: ${error.message || "Unknown error occurred"}`,
            );
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={modalNames[modalType]}
            modalNameStyle={{ fontFamily: "Georama", color: "#65636e" }}
            close={onClose}
            animationDirection="top"
            position="top"
            form={true}
            onSubmit={handleSubmit}
            isButtonLoading={isLoading}
            buttonType={"submit"}
            buttonIcon={buttonIcons[modalType]}
            buttonLabel={toPascalCase(buttonLabels[modalType])}
            isButtonDisabled={isLoading}
            buttonWidth={"100%"}
            buttonBackgroundColor={""}
            buttonTextColor={"var(--theme-font-color)"}
            buttonShape={"pill"}
            buttonBorderStyle={""}
            buttonBorderColor={"2px solid var(--theme-font-color)"}
            buttonIconStyle={{ color: "var(--theme-font-color)" }}
            buttonLabelStyle={{
                fontWeight: "500",
                fontFamily: "Georama",
                color: "#65636e",
            }}
            closeOnOverlayClick={false}
        >
            {modalType === "delete" ? (
                <DeleteModal
                    text="folder"
                    value={""}
                    onChange={""}
                    error={""}
                    helperText={""}
                />
            ) : (
                <>
                    <CustomInput
                        className="input-style"
                        label="File Name"
                        labelPosition="top-left"
                        name="FileName"
                        type="text"
                        placeholder="File Name"
                        value={formData.FileName}
                        onChange={(e) =>
                            handleInputChange(
                                e,
                                setFormData,
                                setFieldErrors,
                                "input",
                                "File Name",
                            )
                        }
                        error={fieldErrors.FileName}
                        disabled={isLoading}
                    />

                    <CustomInput
                        className="input-style"
                        label="File"
                        labelPosition="top-left"
                        name="File"
                        type="file"
                        placeholder="Upload Images or PDFs"
                        onChange={handleFileChange}
                        disabled={isLoading}
                        error={fieldErrors.File}
                        multiple={true}
                        accept="image/jpeg,image/jpg,image/png,image/gif,image/webp,application/pdf"
                    />
                </>
            )}
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
    );
};

export default FileManagementModal;
