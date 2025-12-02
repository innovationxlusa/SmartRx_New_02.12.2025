import { useEffect, useState } from "react";
import { validateField } from "../../../utils/validators";
import CustomInput from "../../static/Commons/CustomInput";
import useFormHandler from "../../../hooks/useFormHandler";
import useApiClients from "../../../services/useApiClients";
import CustomSelect from "../../static/Dropdown/CustomSelect";
import { useUserContext } from "../../../contexts/UserContext";
import CustomModal from "../../static/CustomModal/CustomModal";
import DeleteModal from "../../static/Commons/FormFields/DeleteModal/DeleteModal";
import {
    CREATE_NEW_VITAL_URL,
    DELETE_VITAL_URL,
    EDIT_VITAL_URL,
} from "../../../constants/apiEndpoints";
import useApiService from "../../../services/useApiService";
import VitalFaqs from "./VitalFaq";

const VitalFaqModal = ({
    modalType,
    isOpen,
    vitalData,
    onClose,
    fetchFolders,
    folderRefetch,
    fetchSmartRxVitalData,
    anotherButton,
    smartRxMasterId,
    prescriptionId,
    refetch,
    vitalFAQsData,
    isVitalFAQLoading,
    vitalFAQError,
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

    // Destructuring API service methods
    const { api } = useApiClients();

    // Destructuring API service methods
    const apiServices = useApiService();

    const initialData = {
        Id: vitalData?.folderId ?? "",
        VitalId: vitalData?.folderId ?? "",
        ParentFolderId: vitalData?.folderId ?? user?.PrimaryFolderId,
        PreviousFolderHierarchy:
            vitalData?.folderName === "Primary"
                ? 0
                : vitalData?.folderHierarchy,
        FolderHierarchy:
            vitalData?.folderHierarchy ?? user?.PrimaryFolderHeirarchy,
        VitalName: "",
        VitalId: vitalData?.folderId ?? "",
        UserId: Number(user?.UserId),
        Description: "",
        LoginUserId: Number(user?.UserId),
        VitalUnit: "",
        VitalEntity: "",
        VitalValue: "",
    };

    const [formData, setFormData] = useState(initialData);
    const [fieldErrors, setFieldErrors] = useState(initialData);
    const [isLoading, setIsLoading] = useState(false);

    const modalNames = dynamicModalName("Vital FAQs");
    const buttonLabels = dynamicButtonLabel("Vital");

    const [vitalEntityOptions, setVitalEntityOptions] = useState([]);

    const vitalUnitsMap = {
        bp: [{ value: "mmHg", label: "mmHg" }],
        temp: [
            { value: "°C", label: "°C" },
            { value: "°F", label: "°F" },
        ],
        pr: [{ value: "bpm", label: "Beats/Min" }],
        resp: [{ value: "rpm", label: "Resp/Min" }],
        bo: [{ value: "%", label: "% Saturation" }],
        bmi: [{ value: "kg/m²", label: "kg/m²" }],
        weight: [
            { value: "kg", label: "Kilogram" },
            { value: "g", label: "Gram" },
            { value: "lb", label: "Pound" },
        ],
        glucose: [
            { value: "mg/dL", label: "mg/dL" },
            { value: "mmol/L", label: "mmol/L" },
        ],
    };

    const vitalNameLabelMap = {
        bp: "Blood Pressure",
        temp: "Body Temperature",
        pr: "Pulse Rate",
        resp: "Respiratory Rate",
        bo: "Blood Oxygen",
        bmi: "BMI",
        weight: "Weight",
        glucose: "Blood Glucose",
    };

    const vitalNameLabelForEdit = {
        "Blood Pressure": "bp",
        "Body Temperature": "temp",
        "Pulse Rate": "pr",
        "Respiratory Rate": "resp",
        "Blood Oxygen": "bo",
        BMI: "bmi",
        Weight: "weight",
        "Blood Glucose": "glucose",
    };

    //API to get Entity
    useEffect(() => {
        const fetchVitalEntity = async () => {
            if (
                !formData.VitalName ||
                typeof fetchSmartRxVitalData !== "function"
            )
                return;

            try {
                const response = await fetchSmartRxVitalData({
                    VitalName: vitalNameLabelMap[formData.VitalName],
                });

                if (response?.data?.length > 0) {
                    const entityMap = new Map();

                    response.data.forEach((item) => {
                        const entities =
                            item.applicableEntity
                                ?.split(",")
                                .map((e) => e.trim()) || [];
                        entities.forEach((entity) => {
                            if (entity && !entityMap.has(entity)) {
                                entityMap.set(entity, {
                                    id: item.id,
                                    name: entity,
                                    value: item.id,
                                    label: entity,
                                });
                            }
                        });
                    });

                    const options = Array.from(entityMap.values());
                    setVitalEntityOptions(options);
                } else {
                    setVitalEntityOptions([]);
                }
            } catch (error) {
                console.error(
                    "Failed to fetch applicableEntity from fetchSmartRxVitalData",
                    error
                );
                setVitalEntityOptions([]);
            }
        };

        fetchVitalEntity();
    }, [formData.VitalName]);

    useEffect(() => {
        if ((modalType === "edit" || modalType === "delete") && vitalData) {
            const {
                name,
                vitalId,
                parentFolderId,
                folderHeirarchy,
                measurementUnit,
                vitalValue,
                status,
            } = vitalData;

            setFormData({
                ...formData,
                Id: vitalId,
                ParentFolderId: parentFolderId,
                PreviousFolderHierarchy: folderHeirarchy,
                FolderHierarchy: folderHeirarchy,
                VitalValue: "",
                LoginUserId: Number(user?.UserId),
                UserId: Number(user?.UserId),
                VitalName: vitalNameLabelForEdit[name] || "",
                VitalId: vitalId,
                VitalUnit: measurementUnit,
                VitalValue: vitalValue,
            });
        } else {
            resetForm(initialData, setFormData, setFieldErrors);
        }
    }, [modalType, vitalData]);

    //Reset VitalUnit when VitalName changes
    // useEffect(() => {
    //     setFormData((prev) => ({
    //         ...prev,
    //         VitalValue: "",
    //         VitalUnit: "",
    //     }));
    // }, [formData.VitalName]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (modalType !== "delete") {
            const fieldsToValidate = {
                VitalName: validateField(
                    "VitalName",
                    formData.VitalName,
                    "Vital Name"
                ),
                VitalId: validateField(
                    "VitalId",
                    formData.VitalId,
                    "Vital Entity"
                ),
                VitalValue: validateField(
                    "VitalValue",
                    formData.VitalValue,
                    "Vital Reading"
                ),
                VitalUnit: validateField(
                    "VitalUnit",
                    formData.VitalId,
                    "Vital Measurement Unit"
                ),
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
                VitalName: vitalNameLabelMap[formData.VitalName],
                SmartRxMasterId:
                    modalType === "edit"
                        ? vitalData.smartRxMasterId
                        : smartRxMasterId,
                PrescriptionId:
                    modalType === "edit"
                        ? vitalData.prescriptionId
                        : prescriptionId,
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
                fetchFolders && fetchFolders();
                folderRefetch && folderRefetch();
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
            modalName={modalType != "add" ? "Vital FAQs" : ""}
            subModal={modalType === "add" ? "Earn points by adding vital" : ""}
            close={onClose}
            animationDirection="top"
            position="top"
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
            closeOnOverlayClick={false}
            dataPreview
        >
            <div className="compare-drug-count-border-bottom mb-2" />
            {modalType === "faq" ? (
                <VitalFaqs
                    text="vital"
                    value=""
                    onChange=""
                    helperText=""
                    data={vitalFAQsData}
                    isLoading={isVitalFAQLoading}
                    error={vitalFAQError}
                />
            ) : (
                <div className="vital-fasection mt-4"></div>
            )}
        </CustomModal>
    );
};

export default VitalFaqModal;
