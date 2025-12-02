import { useEffect, useMemo, useState } from "react";
import { validateField } from "../../../utils/validators";
import CustomInput from "../../static/Commons/CustomInput";
import useFormHandler from "../../../hooks/useFormHandler";
import useApiClients from "../../../services/useApiClients";
import CustomSelect from "../../static/Dropdown/CustomSelect";
import { useUserContext } from "../../../contexts/UserContext";
import useCurrentUserId from "../../../hooks/useCurrentUserId";
import CustomModal from "../../static/CustomModal/CustomModal";
import DeleteModal from "../../static/Commons/FormFields/DeleteModal/DeleteModal";
import ProfileButton from "../../static/Commons/CommonButton";
import {
    CREATE_NEW_VITAL_URL,
    DELETE_SMARTRX_VITAL_BY_ID_URL,
    EDIT_VITAL_URL,
} from "../../../constants/apiEndpoints";
import useApiService from "../../../services/useApiService";
import { handleGeneralError } from "../../../utils/errorHandling";
import RewardPopup from "../../Reward/RewardPopup";

const VitalManagementModal = ({
    modalType,
    isOpen,
    folderData,
    vitalData,
    onClose,
    fetchFolders,
    folderRefetch,
    smartRxInsiderVitalData,
    smartRxInsiderAgeData,
    smartRxInsiderGenderData,
    fetchSmartRxVitalData,
    anotherButton,
    smartRxMasterId,
    prescriptionId,
    refetch,
    patientFullName,
    patientId,
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
    // Destructuring API service methods
    const { api } = useApiClients();

    // Destructuring API service methods
    const apiServices = useApiService();

    const initialData = {
        Id: folderData?.folderId ?? "",
        VitalId: folderData?.folderId ?? "",
        ParentFolderId: folderData?.folderId ?? user?.PrimaryFolderId,
        PreviousFolderHierarchy:
            folderData?.folderName === "Primary"
                ? 0
                : folderData?.folderHierarchy,
        FolderHierarchy:
            folderData?.folderHierarchy ?? user?.PrimaryFolderHeirarchy,
        VitalName: "",
        VitalId: folderData?.folderId ?? "",
        UserId: Number(userId),
        Description: "",
        LoginUserId: Number(userId),
        VitalUnit: "",
        VitalEntity: "",
        VitalValue: "",
        SystolicValue: "",
        DiastolicValue: "",
    };

    const [selectedGender, setSelectedGender] = useState("");
    const [selectedItem, setSelectedItem] = useState(null);

    const [formData, setFormData] = useState(initialData);
    const [fieldErrors, setFieldErrors] = useState(initialData);
    const [isLoading, setIsLoading] = useState(false);

    const handleOpenModalEdit = (item, type) => {
        setSelectedItem(item);
        setModalType(type);
    };

    const updatePatientInfo = (field, value) => {
        setPatientData((prev) => ({
            ...prev,
            [field]: value,
        }));

        setModalType(null); // close modal
    };

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const [showZeroError, setShowZeroError] = useState(false);

    const [fieldWarnings, setFieldWarnings] = useState({ DiastolicValue: "" });

    const modalNames = dynamicModalName("Vital");
    const buttonLabels = dynamicButtonLabel("Vital");

    const [vitalEntityOptions, setVitalEntityOptions] = useState([]);

    // Get the current example for placeholder
    const getVitalExample = () => {
        const vitalName = formData.VitalName;
        const unit = formData.VitalUnit;

        if (!vitalName) return "Select a Vital to see normal range";

        // If unit is selected, use it, else pick the first available unit
        const unitsForVital = vitalInfo[vitalName];
        const selectedUnit = unit || Object.keys(unitsForVital)[0];

        return unitsForVital[selectedUnit]?.example || "";
    };

    // Get the current range message
    const getVitalRange = () => {
        const vitalName = formData.VitalName;
        const unit = formData.VitalUnit;

        if (!vitalName) return "";

        const unitsForVital = vitalInfo[vitalName];
        const selectedUnit = unit || Object.keys(unitsForVital)[0];

        return unitsForVital[selectedUnit]?.range || "";
    };

    const vitalUnitsMap = {
        bp: [{ value: "mmHg", label: "mmHg" }],
        temp: [
            { value: "°F", label: "°F" },
            // { value: "°C", label: "°C" },
        ],
        pr: [{ value: "bpm", label: "Beats/Min" }],
        resp: [
            { value: "breaths/min", label: "Breaths/min" },
            { value: "rpm", label: "Resp/Min" },
        ],
        bo: [{ value: "%", label: "% Saturation" }],
        bmi: [{ value: "kg/m²", label: "kg/m²" }],
        weight: [
            { value: "kg", label: "Kilogram" },
            { value: "g", label: "Gram" },
            { value: "lb", label: "Pound" },
        ],
        height: [{ value: "ftin", label: "Feet-Inches" }],
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
        height: "Height",
        weight: "Weight",
        glucose: "Blood Glucose",
        bmi: "BMI",
    };

    const vitalNameLabelForEdit = {
        "Blood Pressure": "bp",
        "Body Temperature": "temp",
        "Pulse Rate": "pr",
        "Respiratory Rate": "resp",
        "Blood Oxygen": "bo",
        Height: "height",
        Weight: "weight",
        Height: "height",
        "Blood Glucose": "glucose",
    };

    const vitalValidation = {
        temp: {
            "°F": {
                invalidLow: 0,
                warningLow: 80,
                warningHigh: 94,
                invalidHigh: 106,
            },
            // "°C": {
            //     invalidLow: 0,
            //     warningLow: 30,
            //     warningHigh: 45,
            //     invalidHigh: 45,
            // },
        },
        // Add other vitals here
    };

    // Map each vital to its range and example
    const vitalInfo = {
        bp: {
            mmHg: {
                range: "Normal Range: 90/60 to 120/80 mmHg",
                example: "120/80",
            },
        },
        temp: {
            "°F": { range: "Normal Range: 98.6°F to 100.4°F", example: "98.8" },
            // "°C": { range: "Normal Range: 37°C to 38°C", example: "37" },
        },
        pr: {
            bpm: { range: "Normal Range: 60 to 100 bpm", example: "75" },
        },
        resp: {
            "breaths/min": {
                range: "Normal Range: 12 to 20 breaths/min",
                example: "16",
            },
            rpm: { range: "Normal Range: 12 to 20 rpm", example: "16" },
        },
        bo: {
            "%": { range: "Normal Range: 95% to 100%", example: "98" },
        },
        height: {
            ftin: {
                range: "Normal Range: 4 to 7 Feet-inches (varies)",
                example: "5.5",
            },
        },
        weight: {
            kg: {
                range: "Normal Range: 50 kg to 100 kg (varies)",
                example: "70",
            },
            g: { range: "Normal Range: 50000 g to 100000 g", example: "70000" },
            lb: { range: "Normal Range: 110 lb to 220 lb", example: "154" },
        },
        glucose: {
            "mg/dL": { range: "Normal Range: 70 to 140 mg/dL", example: "90" },
            "mmol/L": {
                range: "Normal Range: 3.9 to 7.8 mmol/L",
                example: "5.0",
            },
        },
    };

    //Temp Validation
    useEffect(() => {
        const raw = formData.VitalValue;
        if (!raw) {
            setFieldErrors((prev) => ({ ...prev, VitalValue: "" }));
            setFieldWarnings((prev) => ({ ...prev, VitalValue: "" }));
            return;
        }

        const value = parseFloat(raw);
        const vitalName = formData.VitalName;
        const unit = formData.VitalUnit;

        if (!vitalName || !unit || !vitalValidation[vitalName]?.[unit]) return;

        const { invalidLow, warningLow, warningHigh, invalidHigh } =
            vitalValidation[vitalName][unit];
        let timer;

        if (value < warningLow && value >= invalidLow) {
            // Invalid low → cannot submit
            setFieldErrors((prev) => ({
                ...prev,
                VitalValue: `Inaccurate value. Please enter value within the range of ${warningLow} to ${invalidHigh}`,
            }));
            setFieldWarnings((prev) => ({ ...prev, VitalValue: "" }));

            timer = setTimeout(() => {
                setFormData((prev) => ({ ...prev, VitalValue: "" }));
            }, 3000);
        } else if (value > warningLow || value <= warningHigh) {
            // Low but can submit → warning
            setFieldErrors((prev) => ({ ...prev, VitalValue: "" }));
            setFieldWarnings((prev) => ({
                ...prev,
                VitalValue: `You have entered a lower than normal value.`,
            }));
        } else if (value > invalidHigh) {
            // Invalid high → cannot submit
            setFieldErrors((prev) => ({
                ...prev,
                VitalValue: `Inaccurate value. Please enter value within the range of ${warningLow} to ${invalidHigh}`,
            }));
            setFieldWarnings((prev) => ({ ...prev, VitalValue: "" }));

            timer = setTimeout(() => {
                setFormData((prev) => ({ ...prev, VitalValue: "" }));
            }, 3000);
        } else {
            // Normal
            setFieldErrors((prev) => ({ ...prev, VitalValue: "" }));
            setFieldWarnings((prev) => ({ ...prev, VitalValue: "" }));
        }

        return () => clearTimeout(timer);
    }, [formData.VitalValue, formData.VitalName, formData.VitalUnit]);

    // Diastolic Validation
    useEffect(() => {
        const raw = formData.DiastolicValue;
        if (!raw) {
            setFieldErrors((prev) => ({ ...prev, DiastolicValue: "" }));
            setFieldWarnings((prev) => ({ ...prev, DiastolicValue: "" }));
            return;
        }

        const value = parseInt(raw, 10);

        let timer;
        if (value >= 0 && value <= 30) {
            setFieldErrors((prev) => ({
                ...prev,
                DiastolicValue:
                    "Inaccurate value. Please enter value within the range of 40 to 120",
            }));
            setFieldWarnings((prev) => ({ ...prev, DiastolicValue: "" }));

            timer = setTimeout(() => {
                setFormData((prev) => ({ ...prev, DiastolicValue: "" }));
            }, 3000);
        } else if (value > 30 && value < 40) {
            setFieldErrors((prev) => ({ ...prev, DiastolicValue: "" }));
            setFieldWarnings((prev) => ({
                ...prev,
                DiastolicValue: "You have entered lower value.",
            }));
        } else if (value > 120) {
            setFieldErrors((prev) => ({
                ...prev,
                DiastolicValue:
                    "Inaccurate value. Please enter value within the range of 40 to 120",
            }));
            setFieldWarnings((prev) => ({ ...prev, DiastolicValue: "" }));

            timer = setTimeout(() => {
                setFormData((prev) => ({ ...prev, DiastolicValue: "" }));
            }, 3000);
        } else {
            setFieldErrors((prev) => ({ ...prev, DiastolicValue: "" }));
            setFieldWarnings((prev) => ({ ...prev, DiastolicValue: "" }));
        }

        return () => clearTimeout(timer);
    }, [formData.DiastolicValue]);

    // Systolic Validation
    useEffect(() => {
        const raw = formData.SystolicValue;
        if (!raw) {
            setFieldErrors((prev) => ({ ...prev, SystolicValue: "" }));
            setFieldWarnings((prev) => ({ ...prev, SystolicValue: "" }));
            return;
        }

        const value = parseInt(raw, 10);

        let timer;
        if (value >= 0 && value < 60) {
            setFieldErrors((prev) => ({
                ...prev,
                SystolicValue:
                    "Inaccurate value. Please enter value within the range of 100 to 220",
            }));
            setFieldWarnings((prev) => ({ ...prev, SystolicValue: "" }));

            timer = setTimeout(() => {
                setFormData((prev) => ({ ...prev, SystolicValue: "" }));
            }, 3000);
        } else if (value >= 60 && value <= 100) {
            setFieldErrors((prev) => ({ ...prev, SystolicValue: "" }));
            setFieldWarnings((prev) => ({
                ...prev,
                SystolicValue: "You have entered lower value.",
            }));
        } else if (value > 220) {
            setFieldErrors((prev) => ({
                ...prev,
                SystolicValue:
                    "Inaccurate value. Please enter value within the range of 100 to 220",
            }));
            setFieldWarnings((prev) => ({ ...prev, SystolicValue: "" }));

            timer = setTimeout(() => {
                setFormData((prev) => ({ ...prev, SystolicValue: "" }));
            }, 3000);
        } else {
            setFieldErrors((prev) => ({ ...prev, SystolicValue: "" }));
            setFieldWarnings((prev) => ({ ...prev, SystolicValue: "" }));
        }

        return () => clearTimeout(timer);
    }, [formData.SystolicValue]);

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

                if (response?.length > 0) {
                    const entityMap = new Map();

                    response.forEach((item) => {
                        const entities =
                            item.applicableEntity
                                ?.split(",")
                                .map((e) => e.trim()) || [];

                        entities.forEach((entity) => {
                            const vital = formData.VitalName.toLowerCase();

                            //Exclude "Children (Under 6 years)" for glucose if age > 6
                            if (
                                vital === "glucose" &&
                                smartRxInsiderAgeData > 6 &&
                                entity === "Children (under 6 yrs)"
                            ) {
                                return;
                            }

                            //For weight/height, filter based on gender
                            if (
                                (vital === "weight" || vital === "height") &&
                                ((smartRxInsiderGenderData === 1 &&
                                    entity !== "Male") ||
                                    (smartRxInsiderGenderData === 2 &&
                                        entity !== "Female"))
                            ) {
                                return;
                            }

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
                    error,
                );
                setVitalEntityOptions([]);
            }
        };

        fetchVitalEntity();
    }, [formData.VitalName]);

    useEffect(() => {
        if ((modalType === "edit" || modalType === "delete") && folderData) {
            const {
                name,
                vitalId,
                parentFolderId,
                folderHeirarchy,
                measurementUnit,
                vitalValue,
                status,
            } = folderData;

            setFormData({
                ...formData,
                Id: vitalId,
                ParentFolderId: parentFolderId,
                PreviousFolderHierarchy: folderHeirarchy,
                FolderHierarchy: folderHeirarchy,
                VitalValue: "",
                LoginUserId: Number(userId),
                UserId: Number(userId),
                VitalName: vitalNameLabelForEdit[name] || "",
                VitalId: vitalId,
                VitalUnit: measurementUnit,
                VitalValue: vitalValue,
                SystolicValue: "",
                DiastolicValue: "",
            });
        } else {
            resetForm(initialData, setFormData, setFieldErrors);
        }
    }, [modalType, folderData]);

    //New code to show existing data in edit vital
    useEffect(() => {
        if (
            modalType !== "edit" ||
            !formData.VitalName ||
            !formData.VitalId ||
            !smartRxInsiderVitalData?.length
        )
            return;

        const selectedEntityLabel = vitalEntityOptions
            .find((opt) => opt.value === formData.VitalId)
            ?.label?.toLowerCase();

        if (formData.VitalName.toLowerCase() === "bp") {
            const systolic = smartRxInsiderVitalData.find(
                (v) => v.applicableEntity?.toLowerCase() === "systolic",
            );
            const diastolic = smartRxInsiderVitalData.find(
                (v) => v.applicableEntity?.toLowerCase() === "diastolic",
            );

            setFormData((prev) => ({
                ...prev,
                SystolicValue: systolic?.vitalValue || "",
                DiastolicValue: diastolic?.vitalValue || "",
            }));
        } else {
            const vitalMatch = smartRxInsiderVitalData.find(
                (v) =>
                    v.applicableEntity?.toLowerCase() === selectedEntityLabel,
            );

            if (vitalMatch) {
                setFormData((prev) => ({
                    ...prev,
                    VitalValue: vitalMatch.vitalValue || "",
                }));
            }
        }
    }, [
        modalType,
        formData.VitalName,
        formData.VitalId,
        vitalEntityOptions,
        smartRxInsiderVitalData,
    ]);

    useEffect(() => {
        if (!formData.VitalName) return;

        // Only clear values in "add" mode
        if (modalType === "add") {
            setFormData((prev) => ({
                ...prev,
                VitalValue: "",
                SystolicValue: "",
                DiastolicValue: "",
                VitalUnit: vitalUnitsMap[formData.VitalName]?.[0]?.value || "",
                VitalId: "", //clear entity selection
            }));
        }

        setFieldErrors((prev) => ({
            ...prev,
            VitalValue: "",
            SystolicValue: "",
            DiastolicValue: "",
            VitalUnit: "",
            VitalId: "",
        }));
    }, [formData.VitalName, modalType]);

    useEffect(() => {
        if (vitalEntityOptions.length > 0 && !formData.VitalId) {
            setFormData((prev) => ({
                ...prev,
                VitalId: vitalEntityOptions[0].value,
            }));
        }
    }, [vitalEntityOptions]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (modalType !== "delete") {
            const fieldsToValidate = {
                VitalName: validateField(
                    "VitalName",
                    formData.VitalName,
                    "Vital Name",
                ),
                VitalValue:
                    formData.VitalName !== "bp"
                        ? validateField(
                              "VitalValue",
                              formData.VitalValue,
                              "Vital Reading",
                          )
                        : "",
                DiastolicValue:
                    formData.VitalName === "bp"
                        ? validateField(
                              "DiastolicValue",
                              formData.DiastolicValue,
                              "Diastolic",
                          )
                        : "",
                SystolicValue:
                    formData.VitalName === "bp"
                        ? validateField(
                              "SystolicValue",
                              formData.SystolicValue,
                              "Systolic",
                          )
                        : "",
                VitalUnit: validateField(
                    "VitalUnit",
                    formData.VitalUnit,
                    "Vital Measurement Unit",
                ),
            };

            // Check if any field has a validation error
            if (Object.values(fieldsToValidate).some((error) => error !== "")) {
                setFieldErrors(fieldsToValidate);
                return;
            }

            // Additional range checks
            let rangeError = false;
            const newFieldErrors = { ...fieldsToValidate };

            if (formData.VitalName !== "bp") {
                const value = parseFloat(formData.VitalValue);
                if (value === 0) {
                    newFieldErrors.VitalValue = "0 is not allowed as input";
                    rangeError = true;
                    setTimeout(
                        () =>
                            setFormData((prev) => ({
                                ...prev,
                                VitalValue: "",
                            })),
                        1000,
                    );
                }
            }

            if (formData.VitalName === "bp") {
                const diastolic = parseFloat(formData.DiastolicValue);
                const systolic = parseFloat(formData.SystolicValue);

                // Diastolic: block 0 to 30 or >120
                if (diastolic <= 30 || diastolic > 120) {
                    newFieldErrors.DiastolicValue = "Invalid Diastolic input";
                    rangeError = true;
                    setTimeout(
                        () =>
                            setFormData((prev) => ({
                                ...prev,
                                DiastolicValue: "",
                            })),
                        1000,
                    );
                }

                // Systolic: block 0 to 60 or >220
                if (systolic <= 60 || systolic > 220) {
                    newFieldErrors.SystolicValue = "Invalid Systolic input";
                    rangeError = true;
                    setTimeout(
                        () =>
                            setFormData((prev) => ({
                                ...prev,
                                SystolicValue: "",
                            })),
                        1000,
                    );
                }
            }

            if (rangeError) {
                setFieldErrors(newFieldErrors);
                return; // stop submission
            }
        } else {
            // No validation errors
        }
        try {
            setIsLoading(true);

            const dynamicUrl = {
                add: CREATE_NEW_VITAL_URL,
                edit: EDIT_VITAL_URL,
                delete: DELETE_SMARTRX_VITAL_BY_ID_URL,
            };

            let payload = [];

            if (modalType === "delete") {
                try {
                    const actions = dynamicActions(
                        dynamicUrl[modalType],
                        { SmartRxVitalId: vitalData.id }, // Pass as object with correct key
                        "",
                        apiServices,
                        "",
                    );
                    const response = await actions[modalType]();
                    if (
                        response?.message === "Successful" ||
                        typeof response === "object"
                    ) {
                        refetch?.();
                        fetchFolders?.();
                        folderRefetch?.();
                        await fetchSmartRxVitalData?.().catch(console.error);
                        onClose?.();
                    }
                } catch (e) {
                    console.error(e);
                } finally {
                    setIsLoading(false);
                    onClose?.();
                }
                return;
            }

            if (formData.VitalName === "bp") {
                const bpVitals = (smartRxInsiderVitalData || []).filter(
                    (item) =>
                        item.applicableEntity?.toLowerCase() === "diastolic" ||
                        item.applicableEntity?.toLowerCase() === "systolic",
                );

                if (bpVitals.length > 0) {
                    // Use smartRxInsiderVitalData when available
                    payload = bpVitals.map((item) => ({
                        SmartRxMasterId:
                            modalType === "edit"
                                ? folderData.smartRxMasterId
                                : smartRxMasterId,
                        PrescriptionId:
                            modalType === "edit"
                                ? folderData.prescriptionId
                                : prescriptionId,
                        VitalId: item.vitalId,
                        VitalValue:
                            item.applicableEntity?.toLowerCase() === "diastolic"
                                ? formData.DiastolicValue
                                : formData.SystolicValue,
                        LoginUserId: Number(userId),
                        PatientId: patientId,
                    }));
                } else {
                    //Fallback: use vitalEntityOptions to get IDs for systolic & diastolic when smartRxInsiderVitalData empty
                    const systolicVital = vitalEntityOptions.find(
                        (opt) => opt.name.toLowerCase() === "systolic",
                    );
                    const diastolicVital = vitalEntityOptions.find(
                        (opt) => opt.name.toLowerCase() === "diastolic",
                    );

                    payload = [];

                    if (systolicVital) {
                        payload.push({
                            SmartRxMasterId:
                                modalType === "edit"
                                    ? folderData.smartRxMasterId
                                    : smartRxMasterId,
                            PrescriptionId:
                                modalType === "edit"
                                    ? folderData.prescriptionId
                                    : prescriptionId,
                            VitalId: systolicVital.id,
                            VitalValue: formData.SystolicValue,
                            LoginUserId: Number(userId),
                            PatientId: patientId,
                        });
                    }

                    if (diastolicVital) {
                        payload.push({
                            SmartRxMasterId:
                                modalType === "edit"
                                    ? folderData.smartRxMasterId
                                    : smartRxMasterId,
                            PrescriptionId:
                                modalType === "edit"
                                    ? folderData.prescriptionId
                                    : prescriptionId,
                            VitalId: diastolicVital.id,
                            VitalValue: formData.DiastolicValue,
                            LoginUserId: Number(userId),
                            PatientId: patientId,
                        });
                    }

                    if (!systolicVital || !diastolicVital) {
                        console.warn(
                            "Missing systolic or diastolic vital ID in fallback vitalEntityOptions",
                        );
                    }
                }
            } else {
                // For other vitals
                payload = [
                    {
                        SmartRxMasterId:
                            modalType === "edit"
                                ? folderData.smartRxMasterId
                                : smartRxMasterId,
                        PrescriptionId:
                            modalType === "edit"
                                ? folderData.prescriptionId
                                : prescriptionId,
                        VitalId: formData.VitalId,
                        VitalValue: formData.VitalValue,
                        LoginUserId: Number(userId),
                        PatientId: patientId,
                    },
                ];
            }
            //return;
            let response = null;
            setUploadResponse(null);
            setShowRewardPopup(false);
            // Sequential API calls
            for (const item of payload) {
                const actions = dynamicActions(
                    dynamicUrl[modalType],
                    item,
                    "",
                    apiServices,
                    "",
                );

                try {
                    response = await actions[modalType]();
                } catch (error) {
                    console.error("API call failed for:", item, error);
                }
            }
            setUploadResponse(response);
            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        refetch && refetch();
                        fetchFolders && fetchFolders();
                        folderRefetch && folderRefetch();
                        onClose();
                    }, 3000);
                } else {
                    refetch && refetch();
                    fetchFolders && fetchFolders();
                    folderRefetch && folderRefetch();
                    onClose();
                }
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false);
        }
    };

    // Debug log to verify patient name is being passed

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={
                modalType === "add" && patientFullName ? (
                    <>
                        Add Vital of
                        <br />
                        {patientFullName}
                    </>
                ) : (
                    modalNames[modalType]
                )
            }
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
            buttonLabelStyle={{ fontWeight: "500", fontFamily: "Georama" }}
            anotherButton={anotherButton}
            anotherButtonName={
                modalType === "add"
                    ? "Add"
                    : modalType === "edit"
                      ? "Edit"
                      : modalType === "delete"
                        ? "Delete"
                        : ""
            }
            closeOnOverlayClick={false}
            modalNameStyle={{
                fontFamily: "Georama",
                color: "#65636e",
            }}
        >
            {modalType === "delete" ? (
                <DeleteModal
                    text="vital"
                    value=""
                    onChange=""
                    error=""
                    helperText=""
                />
            ) : (
                <div className="vital-section mt-4">
                    <div className="vital-name" style={{ textAlign: "left" }}>
                        <CustomSelect
                            label="Vital"
                            labelPosition="top-left"
                            placeholder="Select a Vital"
                            name="VitalName"
                            value={formData.VitalName}
                            onChange={(e) =>
                                handleInputChange(
                                    e,
                                    setFormData,
                                    setFieldErrors,
                                    "input",
                                    "VitalName",
                                )
                            }
                            error={fieldErrors.VitalName}
                            disabled={isLoading || modalType === "edit"}
                            options={[
                                { value: "bp", label: "Blood Pressure" },
                                { value: "temp", label: "Body Temperature" },
                                { value: "pr", label: "Pulse Rate" },
                                { value: "resp", label: "Respiratory Rate" },
                                { value: "bo", label: "Blood Oxygen" },
                                // { value: "height", label: "Height" },
                                { value: "weight", label: "Weight" },
                                { value: "glucose", label: "Blood Glucose" },
                            ]}
                        />
                    </div>
                    <div
                        className="vital-unit mt-3"
                        style={{ textAlign: "left" }}
                    >
                        <CustomSelect
                            label="Unit"
                            labelPosition="top-left"
                            name="VitalUnit"
                            value={formData.VitalUnit}
                            onChange={(e) =>
                                handleInputChange(
                                    e,
                                    setFormData,
                                    setFieldErrors,
                                    "input",
                                    "VitalUnit",
                                )
                            }
                            error={fieldErrors.VitalUnit}
                            disabled={isLoading}
                            options={vitalUnitsMap[formData.VitalName] || []}
                        />
                    </div>

                    {formData.VitalName === "bp" ? (
                        <div className="bp-flex-row mt-3">
                            <div className="d-flex justify-content-center">
                                <div
                                    className="vit-read me-2"
                                    style={{ width: "50%" }}
                                >
                                    <CustomInput
                                        label="Diastolic"
                                        labelPosition="top-left"
                                        placeholder="Enter Value e.g., 85"
                                        name="DiastolicValue"
                                        type="text"
                                        value={formData.DiastolicValue}
                                        onChange={(e) => {
                                            const numericValue = e.target.value
                                                .replace(/\D/g, "")
                                                .slice(0, 3); // allow only up to 3 digits
                                            setFormData((prev) => ({
                                                ...prev,
                                                DiastolicValue: numericValue,
                                            }));
                                        }}
                                        error={fieldErrors.DiastolicValue}
                                        disabled={isLoading}
                                    />

                                    {/* Non-blocking warning */}
                                    {fieldWarnings?.DiastolicValue && (
                                        <div
                                            style={{
                                                color: "#4b3b8b",
                                                fontSize: "0.85rem",
                                                marginTop: 4,
                                            }}
                                        >
                                            {fieldWarnings.DiastolicValue}
                                        </div>
                                    )}

                                    <div
                                        style={{
                                            color: "#6b7280",
                                            fontSize: "0.70rem",
                                            marginTop: 2,
                                        }}
                                    >
                                        Normal value: 80
                                    </div>
                                </div>

                                <div
                                    style={{ width: "50%", textAlign: "left" }}
                                >
                                    <CustomInput
                                        label="Systolic"
                                        labelPosition="top-left"
                                        placeholder="Enter Value e.g., 130"
                                        name="SystolicValue"
                                        type="text"
                                        value={formData.SystolicValue}
                                        onChange={(e) => {
                                            const numericValue = e.target.value
                                                .replace(/\D/g, "")
                                                .slice(0, 3); // max 3 digits
                                            setFormData((prev) => ({
                                                ...prev,
                                                SystolicValue: numericValue,
                                            }));

                                            // Clear messages while typing
                                            setFieldErrors((prev) => ({
                                                ...prev,
                                                SystolicValue: "",
                                            }));
                                            setFieldWarnings((prev) => ({
                                                ...prev,
                                                SystolicValue: "",
                                            }));
                                        }}
                                        error={fieldErrors.SystolicValue}
                                        disabled={isLoading}
                                    />

                                    {/* Non-blocking warning */}
                                    {fieldWarnings?.SystolicValue && (
                                        <div
                                            style={{
                                                color: "#4b3b8b",
                                                fontSize: "0.85rem",
                                                marginTop: 4,
                                            }}
                                        >
                                            {fieldWarnings.SystolicValue}
                                        </div>
                                    )}

                                    {/* Normal range helper text */}
                                    <div
                                        style={{
                                            color: "#6b7280",
                                            fontSize: "0.70rem",
                                            marginTop: 2,
                                        }}
                                    >
                                        Normal range: 120 to 130
                                    </div>
                                </div>
                            </div>
                        </div>
                    ) : (
                        <>
                            {/* Button new add */}

                            {["resp", "glucose"].includes(
                                formData.VitalName?.toLowerCase(),
                            ) && (
                                <div
                                    style={{
                                        textAlign: "left",
                                        marginTop: "1rem",
                                        marginBottom: "0.5rem",
                                    }}
                                >
                                    <div
                                        style={{
                                            fontFamily: "Georama",
                                            color: "#65636e",
                                            fontWeight: "800",
                                            fontSize: "14px",
                                        }}
                                    >
                                        Vital Entity
                                    </div>

                                    <div
                                        style={{
                                            display: "flex",
                                            flexWrap: "wrap",
                                            justifyContent: "center",
                                            gap: "12px",
                                            marginTop: "0.5rem",
                                        }}
                                    >
                                        {vitalEntityOptions?.map((opt) => (
                                            <div
                                                key={opt.value}
                                                style={{
                                                    boxSizing: "border-box",
                                                    display: "flex",
                                                    justifyContent: "center",
                                                    alignItems: "center",
                                                    minWidth: "120px",
                                                    height: "30px",
                                                    border: "1px solid var(--overview-border)",
                                                    borderRadius: "100px",
                                                    cursor: "pointer",
                                                    transition:
                                                        "background 0.2s, color 0.2s",
                                                    backgroundColor:
                                                        formData.VitalId ===
                                                        opt.value
                                                            ? "#4B3B8B"
                                                            : "",
                                                    whiteSpace: "nowrap",
                                                    flexShrink: 0,
                                                }}
                                                onClick={() => {
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        VitalId: opt.value,
                                                    }));
                                                    setFieldErrors((prev) => ({
                                                        ...prev,
                                                        VitalId: "",
                                                        VitalValue: "",
                                                    }));
                                                }}
                                            >
                                                <ProfileButton
                                                    type="button"
                                                    text={opt.label}
                                                    customStyles={{
                                                        color:
                                                            formData.VitalId ===
                                                            opt.value
                                                                ? "#fff"
                                                                : "#65636e",
                                                        backgroundColor:
                                                            "transparent",
                                                        border: "none",
                                                        padding: 0,
                                                        fontSize: "14px",
                                                        cursor: "pointer",
                                                        width: "100%",
                                                        height: "100%",
                                                        fontFamily: "Georama",
                                                        whiteSpace: "nowrap",
                                                    }}
                                                />
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            )}

                            <div
                                className="vital-reading mt-2"
                                style={{ textAlign: "left" }}
                            >
                                <CustomInput
                                    label="Reading"
                                    labelPosition="top-left"
                                    placeholder={`Enter Value e.g., ${getVitalExample()}`}
                                    name="VitalValue"
                                    type="text"
                                    value={formData.VitalValue}
                                    onChange={(e) => {
                                        let value = e.target.value;

                                        // Allow only numbers and a single dot
                                        value = value.replace(/[^0-9.]/g, "");

                                        // Prevent multiple dots
                                        const parts = value.split(".");
                                        if (parts.length > 2) {
                                            value = parts[0] + "." + parts[1];
                                        }

                                        setFormData((prev) => ({
                                            ...prev,
                                            VitalValue: value,
                                        }));

                                        setFieldErrors((prev) => ({
                                            ...prev,
                                            VitalValue: "",
                                        }));

                                        // Check for zero
                                        if (parseFloat(value) === 0) {
                                            setShowZeroError(true);

                                            // Clear the input after 1 second
                                            setTimeout(() => {
                                                setFormData((prev) => ({
                                                    ...prev,
                                                    VitalValue: "",
                                                }));
                                            }, 500);

                                            // Clear the error after 5 seconds
                                            setTimeout(
                                                () => setShowZeroError(false),
                                                500,
                                            );
                                        } else {
                                            setShowZeroError(false);
                                        }
                                    }}
                                    disabled={isLoading}
                                    error={fieldErrors.VitalValue}
                                />

                                {/* Show zero error */}
                                {showZeroError && (
                                    <div
                                        style={{
                                            color: "red",
                                            fontSize: "0.85rem",
                                            marginTop: 4,
                                        }}
                                    >
                                        Invalid Input
                                    </div>
                                )}
                                {/* Show normal range below input */}
                                {formData.VitalName && (
                                    <p
                                        style={{
                                            fontSize: "12px",
                                            color: "#666",
                                            marginTop: "4px",
                                        }}
                                    >
                                        {getVitalRange()}
                                    </p>
                                )}
                            </div>
                        </>
                    )}
                </div>
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

export default VitalManagementModal;
