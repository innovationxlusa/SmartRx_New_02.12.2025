import { useEffect, useState } from "react";
import { validateField } from "../../../utils/validators";
import CustomInput from "../../static/Commons/CustomInput";
import useFormHandler from "../../../hooks/useFormHandler";
import useApiClients from "../../../services/useApiClients";
import useApiService from "../../../services/useApiService";
import { convertToBengaliNumber } from "../../../utils/utils";
import useCurrentUserId from "../../../hooks/useCurrentUserId";
import CustomModal from "../../static/CustomModal/CustomModal";
import DeleteModal from "../../static/Commons/FormFields/DeleteModal/DeleteModal";
import {
    CREATE_NEW_VITAL_URL,
    DELETE_VITAL_URL,
    EDIT_VITAL_URL,
} from "../../../constants/apiEndpoints";

const AdviceManagementModal = ({
    modalType,
    isOpen,
    adviceData,
    onClose,
    smartRxMasterId,
    prescriptionId,
    refetch,
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
    const userId = useCurrentUserId();

    // Destructuring API service methods
    const { api } = useApiClients();

    // Destructuring API service methods
    const apiServices = useApiService();

    const initialData = {
        Id: adviceData?.adviceDataId ?? "",
        Advice: "",
        UserId: Number(userId) || 0,
        LoginUserId: Number(userId) || 0,
    };

    const [formData, setFormData] = useState(initialData);
    const [fieldErrors, setFieldErrors] = useState(initialData);
    const [isLoading, setIsLoading] = useState(false);

    const modalNames = dynamicModalName("Advice");
    const buttonLabels = dynamicButtonLabel("Advice");

    useEffect(() => {
        if ((modalType === "edit" || modalType === "delete") && adviceData) {
            const { advice } = adviceData;

            setFormData({
                ...formData,
                Id: "",
                LoginUserId: Number(userId) || 0,
                UserId: Number(userId) || 0,
                Advice: advice || "",
            });
        } else {
            resetForm(initialData, setFormData, setFieldErrors);
        }
    }, [modalType, adviceData]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (modalType !== "delete") {
            const fieldsToValidate = {
                Advice: validateField("Advice", formData.Advice, "Advice"),
            };

            if (Object.values(fieldsToValidate).some((error) => error !== "")) {
                setFieldErrors(fieldsToValidate);
                return;
            }
        }

        try {
            setIsLoading(true);

            const dynamicUrl = {
                add: CREATE_NEW_VITAL_URL,
                edit: EDIT_VITAL_URL,
                delete: DELETE_VITAL_URL,
            };

            const newData = {
                ...formData,
                // SmartRxMasterId: modalType === "edit" ? folderData.smartRxMasterId : smartRxMasterId,
                // PrescriptionId: modalType === "edit" ? folderData.prescriptionId : prescriptionId,
            };

            // Define actions for different modal types
            const actions = dynamicActions(
                dynamicUrl[modalType],
                newData,
                "",
                apiServices,
                ""
            );

            // API call to assign roles to the selected user
            const response = await actions[modalType]();

            // const response = await api.createNewVital(newData, "");

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                refetch && refetch();
                onClose();
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={
                modalType === "learnMore"
                    ? "Doctor Recommendation"
                    : modalNames[modalType]
            }
            close={onClose}
            animationDirection="top"
            position={modalType === "learnMore" ? "middle" : "top"}
            form={true}
            onSubmit={handleSubmit}
            isButtonLoading={isLoading}
            buttonType="submit"
            buttonIcon={buttonIcons[modalType]}
            buttonLabel={toPascalCase(buttonLabels[modalType])}
            isButtonDisabled={isLoading}
            buttonWidth="100%"
            buttonBackgroundColor=""
            buttonTextColor="var(--theme-font-color)"
            buttonShape="pill"
            buttonBorderStyle=""
            buttonBorderColor="2px solid var(--theme-font-color)"
            buttonIconStyle={{ color: "var(--theme-font-color)" }}
            buttonLabelStyle={{ fontWeight: "500" }}
            anotherButton={modalType != "learnMore" ? true : false}
            anotherButtonName={modalType === "add" ? "Add" : "Edit"}
            closeOnOverlayClick={false}
            dataPreview={modalType === "learnMore"}
        >
            {modalType === "delete" ? (
                <DeleteModal
                    text="vital"
                    value=""
                    onChange=""
                    error=""
                    helperText=""
                />
            ) : modalType === "learnMore" ? (
                adviceData && adviceData.question && adviceData.answer ? (
                    <div className="col-12 mb-4">
                        <div className="faq-card pt-4 position-relative text-start w-100">
                            <div className="faq-label question-label">
                                প্রশ্ন
                            </div>
                            <h5
                                className="faq-question w-100"
                                style={{
                                    textAlign: "justify",
                                    textJustify: "inter-word",
                                    lineHeight: 1.6,
                                    wordSpacing: "0.01em",
                                }}
                            >
                                {adviceData.question}
                            </h5>

                            <div className="faq-label answer-label">উত্তর</div>
                            <p
                                className="faq-answer mt-2"
                                style={{
                                    textAlign: "justify",
                                    textJustify: "inter-word",
                                    lineHeight: 1.9,
                                    wordSpacing: "0.01em",
                                }}
                            >
                                {adviceData.answer}
                            </p>
                        </div>
                    </div>
                ) : (
                    <p className="text-center text-muted">
                        এই ওষুধের জন্য কোনো প্রশ্নোত্তর পাওয়া যায়নি।
                    </p>
                )
            ) : (
                <div className="vital-name mt-5" style={{ textAlign: "left" }}>
                    <CustomInput
                        label="Doctor Advice"
                        labelPosition="top-left"
                        placeholder=""
                        name="Advice"
                        type="text"
                        value={formData.Advice}
                        onChange={(e) =>
                            handleInputChange(
                                e,
                                setFormData,
                                setFieldErrors,
                                "input",
                                "Advice"
                            )
                        }
                        disabled={isLoading}
                        error={fieldErrors.Advice}
                    />
                </div>
            )}
        </CustomModal>
    );
};

export default AdviceManagementModal;
