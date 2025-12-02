import { useMemo, useState } from "react";
import { validateField } from "../../utils/validators";
import useFormHandler from "../../hooks/useFormHandler";
import useApiClients from "../../services/useApiClients";
import CustomSelect from "../static/Dropdown/CustomSelect";
import CustomModal from "../static/CustomModal/CustomModal";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import RewardPopup from "../../components/Reward/RewardPopup";

const MoveFileModal = ({
    modalType,
    onOpen,
    onClose,
    foldersList,
    prescriptionId,
    refetch,
}) => {
    const { buttonIcons, handleInputChange, resetForm } = useFormHandler();
    const currentUserId = useCurrentUserId();
    // Destructuring API service methods
    const { api } = useApiClients();

    const initialData = {
        TaskName: "MOVE",
        PrescriptionId: prescriptionId,
        FolderId: "",
        UserId: currentUserId ?? 0,
        IsExistingPatient: null,
        PatientProfileId: null,
        HasExistingRelative: null,
        RelativePatientIds: null,
        IsLocked: null,
        IsReported: null,
        ReportReason: null,
        ReportDetails: null,
        IsRecommended: null,
        IsApproved: null,
        IsCompleted: null,
        UpdatedBy: 7,
        Tag1: "",
        Tag2: null,
        Tag3: null,
        Tag4: null,
        Tag5: null,
    };

    // State to manage form data
    const [formData, setFormData] = useState(initialData);
    // State to manage individual field errors
    const [fieldErrors, setFieldErrors] = useState(initialData);
    // State to manage loading status
    const [isLoading, setIsLoading] = useState(false);
    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const fieldsToValidate = {
            FolderId: validateField(
                "FolderId",
                formData.FolderId,
                "Folder name",
            ),
        };

        if (Object.values(fieldsToValidate).some((error) => error)) {
            setFieldErrors(fieldsToValidate);
            return;
        }

        try {
            setIsLoading(true);
            const newData = {
                ...formData,
                PrescriptionId: prescriptionId,
            };

            // API call to move file
            const response = await api.moveFile(newData, prescriptionId, "");

            setUploadResponse(response);

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        if (typeof refetch === "function") {
                            refetch(newData.PrescriptionId);
                        }
                        onClose();
                    }, 3000);
                } else {
                    if (typeof refetch === "function") {
                        refetch(newData.PrescriptionId);
                    }
                    onClose();
                }
                resetForm(initialData, setFormData, setFieldErrors); // Reset the form
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false); // Reset loading state
        }
    };

    const folderOptions = useMemo(() => {
        if (!Array.isArray(foldersList)) {
            return [];
        }

        return foldersList
            .map((folder) => {
                const resolvedId =
                    folder?.folderId ??
                    folder?.FolderId ??
                    folder?.id ??
                    folder?.Id ??
                    null;

                if (resolvedId == null) {
                    return null;
                }

                const resolvedLabel =
                    folder?.folderName ??
                    folder?.folderOrFileName ??
                    folder?.name ??
                    folder?.FolderName ??
                    `Folder ${resolvedId}`;

                return {
                    key: resolvedId,
                    label: resolvedLabel,
                    value: resolvedId,
                };
            })
            .filter(Boolean);
    }, [foldersList]);

    return (
        <CustomModal
            isOpen={onOpen}
            modalName={"Move File"}
            modalNameStyle={{ fontFamily: "Georama", color: "#65636e" }}
            close={onClose}
            animationDirection="top"
            modalSize="medium"
            position="top"
            form={true}
            onSubmit={handleSubmit}
            isButtonLoading={isLoading}
            buttonType={"submit"}
            buttonIcon={buttonIcons[modalType]}
            buttonLabel={"Move"}
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
        >
            <div className="overflow-hidden px-1">
                <CustomSelect
                    name="FolderId"
                    value={formData.FolderId}
                    onChange={(e) =>
                        handleInputChange(
                            e,
                            setFormData,
                            setFieldErrors,
                            "select",
                            "FolderId",
                        )
                    }
                    options={folderOptions}
                    placeholder="Select Folder"
                    icon={""}
                    bgColor="#E6E4EF"
                    textColor=""
                    borderRadius="5px"
                    width="100%"
                    error={fieldErrors?.FolderId}
                    className="mb-5 mt-3 pb-5"
                    dropdownTrayHight="80px"
                    showOverModal={true}
                />
            </div>
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

export default MoveFileModal;
