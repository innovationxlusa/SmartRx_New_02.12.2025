import { useEffect, useState } from "react";
import "./FolderManagementModal.css";
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
    DELETE_FOLDER_URL,
    RENAME_FOLDER_URL,
} from "../../constants/apiEndpoints";
import { useParams } from "react-router-dom";
import RewardPopup from "../../components/Reward/RewardPopup";
import useSmartNavigate from "../../hooks/useSmartNavigate";

const FolderManagementModal = ({
    modalType,
    isOpen,
    folderData,
    onClose,
    fetchFolders,
    folderRefetch,
    navigateToAllPatient = false,
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
    const currentUserId = useCurrentUserId();

    // Destructuring API service methods
    const apiServices = useApiService();
    const { folderId: routeFolderId } = useParams();
    const routeFolderIdNum = Number(routeFolderId);
    const currentFolderId =
        Number.isFinite(routeFolderIdNum) && routeFolderIdNum > 0
            ? routeFolderIdNum
            : (folderData?.folderId ?? user?.PrimaryFolderId);

    const initialData = {
        Id: folderData?.folderId ?? "",
        FolderId: folderData?.folderId ?? "",
        ParentFolderId:
            currentFolderId ??
            user?.PrimaryFolderId ??
            folderData?.parentFolderId,
        PreviousFolderHierarchy:
            folderData?.folderName === "Primary"
                ? 0
                : folderData?.folderHierarchy,
        FolderHierarchy:
            folderData?.folderHierarchy ?? user?.PrimaryFolderHeirarchy,
        FolderName: "",
        FolderId: folderData?.folderId ?? "",
        UserId: currentUserId ?? 0,
        Description: "",
        LoginUserId: currentUserId ?? 0,
    };

    // State to manage form data
    const [formData, setFormData] = useState(initialData);

    // State to manage individual field errors
    const [fieldErrors, setFieldErrors] = useState(initialData);

    // State to manage loading status
    const [isLoading, setIsLoading] = useState(false);

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    // Get modal names and button labels dynamically based on modal type
    const modalNames = dynamicModalName("Folder");
    const buttonLabels = dynamicButtonLabel("Folder");

    // Function to handle reset fields value

    // Effect to initialize form data based on modal type and topic(passed props)
    useEffect(() => {
        setUploadResponse(null);
        setShowRewardPopup(false);

        if ((modalType === "rename" || modalType === "delete") && folderData) {
            // Extract necessary properties from folderData
            const {
                folderName,
                folderOrFileName,
                folderId,
                parentFolderId,
                folderHeirarchy,
                Description,
                uniqueCode,
                status,
            } = folderData;

            setFormData((prevFormData) => ({
                ...prevFormData,
                Id: folderId,
                FolderId: folderId,
                ParentFolderId: parentFolderId,
                PreviousFolderHierarchy: folderHeirarchy,
                FolderHierarchy: folderHeirarchy,
                FolderName: folderName || folderOrFileName || "",
                LoginUserId: currentUserId ?? 0,
                UserId: currentUserId ?? 0, // user?.UserId
            }));
        } else {
            // Clear form data if modalType is not 'edit' and Clear empty field error on close modal
            resetForm(initialData, setFormData, setFieldErrors);
        }
    }, [modalType, folderData, user, currentUserId]);

    // Function to handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        if (isLoading) {
            return;
        }

        // Validate fields based on modalType
        if (modalType !== "delete") {
            const fieldsToValidate = {
                FolderName: validateField(
                    "FolderName",
                    formData.FolderName,
                    "Folder Name",
                ),
            };
            // Validate all fields before submission
            if (Object.values(fieldsToValidate).some((error) => error !== "")) {
                setFieldErrors(fieldsToValidate);
                return;
            }
        }

        try {
            setIsLoading(true);

            const dynamicUrl = {
                add: CREATE_NEW_FOLDER_URL, // Label for the "add" modal type
                rename: RENAME_FOLDER_URL, // Label for the "edit" modal type
                delete: DELETE_FOLDER_URL, // Label for the "delete" modal type
            };

            // Define actions for different modal types
            const actions = dynamicActions(
                dynamicUrl[modalType],
                formData,
                folderData?.folderId
                    ? folderData?.folderId
                    : user?.PrimaryFolderId,
                apiServices,
                "",
            );

            // API call to assign roles to the selected user
            const response = await actions[modalType]();
            setUploadResponse(response);

            // Check the response and update the form data with success or error message
            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (
                    response?.response?.isRewardUpdated == true
                ) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        fetchFolders &&
                            fetchFolders(
                                response?.response?.folderId ??
                                    folderData?.folderId,
                                modalType,
                                formData?.FolderName?.trim?.() ??
                                    folderData?.folderName ??
                                    folderData?.folderOrFileName,
                            );
                        folderRefetch && folderRefetch();
                        onClose();
                    }, 3000);
                } else {
                    fetchFolders &&
                        fetchFolders(
                            response?.response?.folderId ??
                                folderData?.folderId,
                            modalType,
                            formData?.FolderName?.trim?.() ??
                                folderData?.folderName ??
                                folderData?.folderOrFileName,
                        );
                    folderRefetch && folderRefetch();
                    onClose();
                }
            }
        } catch (e) {
            console.error("Folder operation error:", e);
            // Show error message to user
            alert(
                `Error ${modalType === "delete" ? "deleting" : modalType === "rename" ? "renaming" : "processing"} folder: ${e.message || "Unknown error occurred"}`,
            );
        } finally {
            setIsLoading(false); // Reset loading state
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
                <div>
                    <DeleteModal
                        text="folder"
                        value={""}
                        onChange={""}
                        error={""}
                        helperText={""}
                    />
                </div>
            ) : (
                <>
                    <CustomInput
                        className="mb-3 input-style"
                        label="Folder Name"
                        labelPosition="top-left"
                        name="FolderName"
                        type="text"
                        placeholder="Folder Name"
                        value={formData.FolderName}
                        onChange={(e) =>
                            handleInputChange(
                                e,
                                setFormData,
                                setFieldErrors,
                                "input",
                                "FolderName",
                            )
                        }
                        error={fieldErrors.FolderName}
                        disabled={isLoading}
                    />
                    {modalType !== "rename" && (
                        <CustomInput
                            className="input-style"
                            label="Description"
                            labelPosition="top-left"
                            name="Description"
                            type="text"
                            placeholder="Description"
                            value={formData.Description}
                            onChange={(e) =>
                                handleInputChange(
                                    e,
                                    setFormData,
                                    setFieldErrors,
                                    "input",
                                    "Description",
                                )
                            }
                            disabled={isLoading}
                        />
                    )}
                </>
            )}
            <RewardPopup
                isOpen={showRewardPopup}
                onClose={() => {
                    setShowRewardPopup(false);
                    onClose();
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

export default FolderManagementModal;
