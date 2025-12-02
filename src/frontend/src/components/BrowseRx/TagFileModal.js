import { useState, useEffect } from "react";
import { validateField } from "../../utils/validators";
import useFormHandler from "../../hooks/useFormHandler";
import CustomInput from "../static/Commons/CustomInput";
import useApiClients from "../../services/useApiClients";
import CustomModal from "../static/CustomModal/CustomModal";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import { FaPlus, FaMinus } from "react-icons/fa";
import CustomButton from "../static/Commons/CustomButton";
import { FiMinus, FiPlus } from "react-icons/fi";
import RewardPopup from "../../components/Reward/RewardPopup";

const MAX_TAGS = 5;

const TagFileModal = ({
    modalType,
    onOpen,
    onClose,
    foldersList,
    prescriptionId,
    folderId,
    refetch,
    item,
}) => {
    const { buttonIcons, handleInputChange, resetForm } = useFormHandler();
    const currentUserId = useCurrentUserId();
    const { api } = useApiClients();

    // Extract existing tags from individual tag fields (tag1, tag2, tag3, tag4, tag5)
    const getExistingTags = () => {
        if (!item) return [];

        const tags = [];

        // Check for individual tag fields (tag1, tag2, tag3, tag4, tag5)
        for (let i = 1; i <= 5; i++) {
            const tagValue = item[`tag${i}`] || item[`Tag${i}`];
            if (tagValue && tagValue.trim() !== "") {
                tags.push(tagValue.trim());
            }
        }
        return tags;
    };

    const savedTags = getExistingTags();

    // Create initial data with existing tag values
    const createInitialData = () => {
        const baseData = {
            TaskName: "TAG",
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
        };

        // Add existing tag values
        for (let i = 1; i <= 5; i++) {
            const tagValue = item?.[`tag${i}`] || item?.[`Tag${i}`] || "";
            baseData[`Tag${i}`] = tagValue;
        }

        return baseData;
    };

    const initialData = createInitialData();

    const [formData, setFormData] = useState(initialData);
    const [fieldErrors, setFieldErrors] = useState(initialData);
    const [isLoading, setIsLoading] = useState(false);
    const [tagCount, setTagCount] = useState(
        savedTags.length > 0 ? savedTags.length : 1,
    ); // Set based on existing tags

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    // Update form data when item changes
    useEffect(() => {
        setUploadResponse(null);
        setShowRewardPopup(false);

        if (item) {
            const updatedData = { ...initialData };

            // Update tag values
            for (let i = 1; i <= 5; i++) {
                const tagValue = item[`tag${i}`] || item[`Tag${i}`] || "";
                updatedData[`Tag${i}`] = tagValue;
            }

            setFormData(updatedData);
            setTagCount(savedTags.length > 0 ? savedTags.length : 1);
        }
    }, [item]);

    const handleAddTag = () => {
        if (tagCount < MAX_TAGS) setTagCount((prev) => prev + 1);
    };

    const handleRemoveTag = () => {
        if (tagCount > 1) {
            const tagKey = `Tag${tagCount}`;
            setFormData((prev) => ({ ...prev, [tagKey]: null }));
            setFieldErrors((prev) => ({ ...prev, [tagKey]: null }));
            setTagCount((prev) => prev - 1);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const fieldsToValidate = {};

        // Validate only the visible tag fields (Tag1 to Tag{tagCount})
        for (let i = 1; i <= tagCount; i++) {
            const tagKey = `Tag${i}`;
            fieldsToValidate[tagKey] = validateField(
                tagKey,
                formData[tagKey],
                `Tag ${i}`,
            );
        }

        if (Object.values(fieldsToValidate).some((error) => error)) {
            setFieldErrors((prev) => ({ ...prev, ...fieldsToValidate }));
            return;
        }

        try {
            setIsLoading(true);
            const newData = {
                ...formData,
                PrescriptionId: prescriptionId,
                FolderId: folderId,
            };

            const response = await api.tagFile(newData, prescriptionId, "");

            setUploadResponse(response);

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        onClose();
                        refetch();
                        setTagCount(1);
                    }, 3000);
                } else {
                    onClose();
                    refetch();
                    setTagCount(1);
                }
                resetForm(initialData, setFormData, setFieldErrors);
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <CustomModal
            isOpen={onOpen}
            modalName={"Tag Prescription"}
            modalNameStyle={{ fontFamily: "Georama", color: "#65636e" }}
            close={onClose}
            animationDirection="top"
            position="top"
            form={true}
            onSubmit={handleSubmit}
            isButtonLoading={isLoading}
            buttonType={"submit"}
            buttonIcon={buttonIcons[modalType]}
            buttonLabel={"SAVE"}
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
            modalSize="medium"
        >
            {/* Saved Tags Display */}
            {savedTags && savedTags.length > 0 && (
                <div className="mt-3 mb-4">
                    <div className="d-flex flex-wrap gap-2 align-items-center">
                        <span
                            className="text-muted small fw-medium"
                            style={{ fontFamily: "Georama" }}
                        >
                            Tags:
                        </span>
                        {savedTags.map((tag, index) => (
                            <span
                                key={index}
                                className="badge bg-light text-dark border"
                                style={{
                                    fontSize: "12px",
                                    padding: "6px 10px",
                                    borderRadius: "15px",
                                    border: "1px solid #dee2e6",
                                    backgroundColor: "#f8f9fa",
                                    fontFamily: "Georama",
                                }}
                            >
                                {tag}
                            </span>
                        ))}
                    </div>
                </div>
            )}

            <div className="mt-4 space-y-3">
                {/* Button Container */}
                <div className="d-flex justify-content-end align-items-center gap-2 mb-3">
                    <CustomButton
                        isLoading={""}
                        type="button"
                        icon={<FiPlus />}
                        label=""
                        disabled={tagCount >= MAX_TAGS || isLoading}
                        width="30px"
                        height="clamp(30px, 2.3vw, 50px)"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        shape=""
                        borderStyle={"50%"}
                        borderColor="2px solid var(--theme-font-color)"
                        iconStyle={{
                            color: "var(--theme-font-color)",
                            fontSize: "14px !important",
                        }}
                        labelStyle={{}}
                        hoverEffect="theme"
                        onClick={handleAddTag}
                    />
                    <CustomButton
                        isLoading={""}
                        type="button"
                        icon={<FiMinus />}
                        label=""
                        disabled={tagCount <= 1 || isLoading}
                        width="30px"
                        height="clamp(30px, 2.3vw, 50px)"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        shape=""
                        borderStyle={"50%"}
                        borderColor="2px solid var(--theme-font-color)"
                        iconStyle={{
                            color: "var(--theme-font-color)",
                            fontSize: "14px !important",
                        }}
                        labelStyle={{}}
                        hoverEffect="theme"
                        onClick={handleRemoveTag}
                    />
                </div>
                {[...Array(tagCount)].map((_, index) => {
                    const tagKey = `Tag${index + 1}`;
                    const currentValue = formData[tagKey] || "";

                    return (
                        <div key={tagKey} className="mb-3">
                            <CustomInput
                                key={tagKey}
                                className="input-style"
                                label=""
                                labelPosition="top-left"
                                name={tagKey}
                                type="text"
                                placeholder={`Tag ${index + 1}`}
                                value={currentValue}
                                onChange={(e) =>
                                    handleInputChange(
                                        e,
                                        setFormData,
                                        setFieldErrors,
                                        "input",
                                        tagKey,
                                    )
                                }
                                disabled={isLoading}
                                icon={""}
                                iconPosition={"left"}
                                minHeight={"0px"}
                                error={fieldErrors?.[tagKey]}
                            />
                        </div>
                    );
                })}
            </div>
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

export default TagFileModal;
