import { useEffect, useRef, useState } from "react";
import { validateField } from "../../../utils/validators";
import CustomInput from "../../static/Commons/CustomInput";
import useFormHandler from "../../../hooks/useFormHandler";
import useApiClients from "../../../services/useApiClients";
import CustomSelect from "../../static/Dropdown/CustomSelect";
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
import CustomButton from "../../static/Commons/CustomButton";
import PlusIcon from "../../../assets/img/AddBtn.svg";
import MinusIcon from "../../../assets/img/RoundMinus.svg";
import useToastMessage from "../../../hooks/useToastMessage";
import { invisibleValues } from "framer-motion";
const EditNewModal = ({
    modalType,
    isOpen,
    onClose,
    patientId,
    smartRxInsiderWeightData,
    smartRxInsiderHeightData,
    smartRxInsiderAgeData,
    smartRxInsiderGenderData,
    anotherButton,
    smartRxMasterId,
    prescriptionId,
    refetch,
    selectedItem,
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
    const currentUserId = useCurrentUserId();
    // Destructuring API service methods
    const { api } = useApiClients();

    // Destructuring API service methods
    const apiServices = useApiService();
    const showToast = useToastMessage();
    const [feetError, setFeetError] = useState("");
    const [inchesError, setInchesError] = useState("");
    const [weightError, setWeightError] = useState("");
    const [ageError, setAgeError] = useState("");
    const [ageMode, setAgeMode] = useState("age");
    const [dobErrors, setDobErrors] = useState({ year: "", month: "" });

    let feet = "";
    let inches = "";

    const [patientData, setPatientData] = useState(() => {
        

        if (smartRxInsiderHeightData) {
            const match = smartRxInsiderHeightData.match(
                /(\d+)\s*ft\s*(\d+)\s*in/,
            );
            if (match) {
                feet = match[1];
                inches = match[2];
            }
        }

        return {
            gender: smartRxInsiderGenderData,
            age: smartRxInsiderAgeData,
            height: smartRxInsiderHeightData,
            heightFeet: feet,
            heightInches: inches,
            weight: smartRxInsiderWeightData,
        };
    });

    useEffect(() => {
        if (isOpen) {
            setPatientData((prev) => ({
                ...prev,
                age: smartRxInsiderAgeData || "",
            }));
        }
    }, [isOpen, smartRxInsiderAgeData, setPatientData]);

    useEffect(() => {
        if (isOpen) {
            let feet = "";
            let inches = "";

            if (smartRxInsiderHeightData) {
                const match = smartRxInsiderHeightData.match(
                    /(\d+)\s*ft\s*(\d+)\s*in/,
                );
                if (match) {
                    feet = match[1];
                    inches = match[2];
                }
            }

            setPatientData((prev) => ({
                ...prev,
                heightFeet: feet,
                heightInches: inches,
            }));
        }
    }, [isOpen, smartRxInsiderHeightData, setPatientData]);

    const [isLoading, setIsLoading] = useState(false);
    const timeoutRef = useRef(null);

    const updatePatientInfo = (field, value) => {
        setPatientData((prev) => ({
            ...prev,
            [field]: value,
        }));
    };

    const handleChange = (e) => {
        let value = e.target.value.replace(/\D/g, "");
        let num = parseInt(value, 10);

        if (timeoutRef.current) clearTimeout(timeoutRef.current);

        if (value) {
            if (num > 10) {
                value = "10";
            } else if (num < 1) {
                timeoutRef.current = setTimeout(() => {
                    setPatientData((prev) => ({
                        ...prev,
                        heightFeet: feet || "",
                    }));
                    setFeetError("Invalid Input");
                    setTimeout(() => setFeetError(""), 500);
                }, 500);
            } else {
                setFeetError("");
            }
        } else {
            setFeetError("");
        }

        setPatientData((prev) => ({
            ...prev,
            heightFeet: value,
        }));
    };

    const handleInchesChange = (e) => {
        let value = e.target.value.replace(/\D/g, "");
        if (value.length > 2) value = value.slice(0, 2);

        if (parseInt(value, 10) > 11) {
            setInchesError("Invalid Input");
            setTimeout(() => {
                setPatientData((prev) => ({
                    ...prev,
                    heightInches: inches || "",
                }));
                setInchesError("");
            }, 500);
        } else {
            setInchesError("");
            setPatientData((prev) => ({
                ...prev,
                heightInches: value,
            }));
        }
    };

    const handleWeightChange = (e) => {
        let value = e.target.value.replace(/\D/g, "");
        let num = parseInt(value, 10);

        if (value.length > 3) {
            value = value.slice(0, 3);
            num = parseInt(value, 10);
        }

        if (timeoutRef.current) clearTimeout(timeoutRef.current);

        if (value) {
            if (num === 0) {
                timeoutRef.current = setTimeout(() => {
                    setPatientData((prev) => ({
                        ...prev,
                        weight: smartRxInsiderWeightData || "",
                    }));
                    setWeightError("Invalid Input (Weight cannot be 0)");
                    setTimeout(() => setWeightError(""), 500);
                }, 500);
            } else if (num > 600) {
                value = "600";
                setWeightError("");
            } else {
                setWeightError("");
            }
        } else {
            setWeightError("");
        }

        setPatientData((prev) => ({
            ...prev,
            weight: value,
        }));
    };

    useEffect(() => {
        if (smartRxInsiderHeightData) {
            const match = smartRxInsiderHeightData.match(/(\d+)ft\s*(\d+)in/);

            if (match) {
                const feet = match[1]; // e.g. "5"
                const inches = match[2]; // e.g. "10"

                setPatientData((prev) => ({
                    ...prev,
                    heightFeet: feet,
                    heightInches: inches,
                }));
            }
        }
    }, [smartRxInsiderHeightData]);

    useEffect(() => {
        if (isOpen) {
            setPatientData((prev) => ({
                ...prev,
                weight: smartRxInsiderWeightData || "",
            }));
        }
    }, [isOpen, smartRxInsiderWeightData]);

    //API Call
    const handleUpdatePatient = async (e) => {
        e.preventDefault();
        setIsLoading(true);
        try {
           
            // Build FormData payload
            const formDataPayload = new FormData();
            formDataPayload.append("PatientId", String(patientId));
            formDataPayload.append("LoginUserId", String(currentUserId ?? ""));

            formDataPayload.append(
                "PatientDetails.Age",
                String(patientData.age, 10),
            );
            formDataPayload.append(
                "PatientDetails.Gender",
                String(patientData.gender),
            );
            formDataPayload.append(
                "PatientDetails.Height",
                String(
                    patientData.heightFeet +
                        " ft " +
                        patientData.heightInches +
                        " in",
                ),
            );
            formDataPayload.append(
                "PatientDetails.HeightFeet",
                String(patientData.heightFeet),
            );
            formDataPayload.append(
                "PatientDetails.HeightInches",
                String(patientData.heightInches),
            );
            formDataPayload.append(
                "PatientDetails.HeightMeasurementUnit",
                "ftin",
            );
            formDataPayload.append(
                "PatientDetails.Weight",
                String(patientData.weight),
            );
            formDataPayload.append(
                "PatientDetails.WeightMeasurementUnit",
                "kg",
            );

            // API call
            const response = await api.patientUpdate(
                patientId,
                formDataPayload,
                "",
            );

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                {
                    const message = `Patient Information Updated Successfully`;
                    setTimeout(() => {
                        showToast("success", message, "🎉");
                    }, 300);
                }
                onClose();
                refetch();
            }
        } catch (error) {
            console.error("Error in handleUpdatePatient:", error);
        } finally {
            setIsLoading(false);
        }
    };

    // Calculate Age from Year & Month
    const calculateAgeFromYearMonth = (year, month) => {
        if (!year || !month) return "";
        const today = new Date();
        const birthDate = new Date(year, month - 1, 1); // month is 0-based
        let age = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();
        if (monthDiff < 0) age--;
        return age;
    };

    // Estimate DOB (Year/Month) from Age
    const calculateDobFromAge = (age) => {
        if (!age) return { year: "", month: "" };
        const today = new Date();
        const birthYear = today.getFullYear() - age;
        const birthMonth = today.getMonth() + 1; // 1-based month
        return { year: birthYear.toString(), month: birthMonth.toString() };
    };

    useEffect(() => {
        if (patientData.birthYear && patientData.birthMonth) {
            const newAge = calculateAgeFromYearMonth(
                patientData.birthYear,
                patientData.birthMonth,
            );
            if (newAge && newAge !== patientData.age) {
                setPatientData((prev) => ({
                    ...prev,
                    age: newAge.toString(),
                }));
            }
        }
    }, [patientData.birthYear, patientData.birthMonth]);

    return (
        <>
            {isOpen && selectedItem && (
                <CustomModal
                    modalName={`Edit Patient's ${selectedItem.label}`}
                    isOpen={true}
                    close={onClose}
                    onSubmit={handleUpdatePatient}
                    buttonWidth="100%"
                    buttonBackgroundColor=""
                    buttonTextColor="var(--theme-font-color)"
                    buttonBorderStyle=""
                    buttonBorderColor="2px solid var(--theme-font-color)"
                    buttonIconStyle={{ color: "var(--theme-font-color)" }}
                    buttonLabelStyle={{ fontWeight: "500" }}
                    anotherButton={true}
                    modalSize="small"
                    form={true}
                    anotherButtonName="Edit"
                >
                    {selectedItem.label === "Gender" && (
                        <div
                            className="gender-options"
                            style={{
                                display: "flex",
                                justifyContent: "center",
                                alignItems: "center",
                                gap: "20px",
                                marginTop: "20px",
                                marginBottom: "20px",
                            }}
                        >
                            <CustomButton
                                type="button"
                                label="Male"
                                onClick={() => updatePatientInfo("gender", 1)}
                                backgroundColor={
                                    patientData.gender === 1
                                        ? "#4B3B8B"
                                        : "#fff"
                                }
                                textColor={
                                    patientData.gender === 1
                                        ? "#fff"
                                        : "#4B3B8B"
                                }
                                borderColor="1px solid #4B3B8B"
                                shape="pill"
                                width="90px"
                                height="40px"
                            />
                            <CustomButton
                                type="button"
                                label="Female"
                                onClick={() => updatePatientInfo("gender", 2)}
                                backgroundColor={
                                    patientData.gender === 2
                                        ? "#4B3B8B"
                                        : "#fff"
                                }
                                textColor={
                                    patientData.gender === 2
                                        ? "#fff"
                                        : "#4B3B8B"
                                }
                                borderColor="1px solid #4B3B8B"
                                shape="pill"
                                width="90px"
                                height="40px"
                            />
                        </div>
                    )}

                    {selectedItem.label === "Age" && (
                        <div
                            className="age-input"
                            style={{
                                display: "flex",
                                flexDirection: "column",
                                justifyContent: "center",
                                alignItems: "center",
                                marginTop: "10px",
                                marginBottom: "40px",
                                gap: "15px",
                            }}
                        >
                            {/* Toggle Buttons */}
                            <div style={{ display: "flex", gap: "10px" }}>
                                <CustomButton
                                    type="button"
                                    label="Age"
                                    onClick={() => setAgeMode("age")}
                                    backgroundColor={
                                        ageMode === "age" ? "#4B3B8B" : "#fff"
                                    }
                                    textColor={
                                        ageMode === "age" ? "#fff" : "#4B3B8B"
                                    }
                                    borderColor="1px solid #4B3B8B"
                                    shape="pill"
                                    width="100px"
                                    height="40px"
                                />
                                <CustomButton
                                    type="button"
                                    label="Date of Birth"
                                    onClick={() => setAgeMode("dob")}
                                    backgroundColor={
                                        ageMode === "dob" ? "#4B3B8B" : "#fff"
                                    }
                                    textColor={
                                        ageMode === "dob" ? "#fff" : "#4B3B8B"
                                    }
                                    borderColor="1px solid #4B3B8B"
                                    shape="pill"
                                    width="120px"
                                    height="40px"
                                />
                            </div>

                            {/* AGE MODE */}
                            {ageMode === "age" && (
                                <div
                                    style={{
                                        display: "flex",
                                        gap: "10px",
                                        alignItems: "center",
                                        marginBottom: "-30px",
                                    }}
                                >
                                    {/* Minus Button */}
                                    <img
                                        src={MinusIcon}
                                        alt="Minus Icon"
                                        style={{
                                            width: "25px",
                                            height: "25px",
                                            cursor: "pointer",
                                            marginBottom: "15px",
                                        }}
                                        onClick={() => {
                                            const currentValue = parseInt(
                                                patientData.age || "0",
                                                10,
                                            );
                                            const newValue = currentValue - 1;
                                            if (newValue >= 1) {
                                                const { year, month } =
                                                    calculateDobFromAge(
                                                        newValue,
                                                    );
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    age: newValue.toString(),
                                                    birthYear: year,
                                                    birthMonth: month,
                                                }));
                                                setAgeError("");
                                            } else {
                                                setAgeError("Invalid input");
                                                setTimeout(() => {
                                                    setPatientData((prev) => ({
                                                        ...prev,
                                                        age: "",
                                                    }));
                                                    setAgeError("");
                                                }, 500);
                                            }
                                        }}
                                    />

                                    {/* Age Input */}
                                    <div className="age-edit-vital">
                                        <CustomInput
                                            placeholder="Enter Age"
                                            type="text"
                                            value={patientData.age || ""}
                                            style={{
                                                textAlign: "center",
                                                width: "160px",
                                            }}
                                            onChange={(e) => {
                                                let value =
                                                    e.target.value.replace(
                                                        /\D/g,
                                                        "",
                                                    );
                                                if (value.length > 3)
                                                    value = value.slice(0, 3);

                                                if (value === "0") {
                                                    setAgeError(
                                                        "Invalid input",
                                                    );
                                                    setTimeout(() => {
                                                        setPatientData(
                                                            (prev) => ({
                                                                ...prev,
                                                                age: "",
                                                            }),
                                                        );
                                                        setAgeError("");
                                                    }, 500);
                                                } else if (
                                                    parseInt(value, 10) > 150
                                                ) {
                                                    setAgeError(
                                                        "Age cannot be more than 150",
                                                    );
                                                    setTimeout(() => {
                                                        setPatientData(
                                                            (prev) => ({
                                                                ...prev,
                                                                age:
                                                                    smartRxInsiderAgeData ||
                                                                    "",
                                                            }),
                                                        );
                                                        setAgeError("");
                                                    }, 500);
                                                } else {
                                                    setAgeError("");
                                                    const { year, month } =
                                                        calculateDobFromAge(
                                                            parseInt(value, 10),
                                                        );
                                                    setPatientData((prev) => ({
                                                        ...prev,
                                                        age: value,
                                                        birthYear: year,
                                                        birthMonth: month,
                                                    }));
                                                }
                                            }}
                                        />
                                        <span
                                            style={{
                                                fontSize: "8px",
                                                color: "#65636e",
                                            }}
                                        >
                                            Note: Input cannot be 0 or more than
                                            150
                                        </span>
                                        {ageError && (
                                            <div
                                                style={{
                                                    color: "red",
                                                    fontSize: "12px",
                                                    marginTop: "5px",
                                                }}
                                            >
                                                {ageError}
                                            </div>
                                        )}
                                    </div>

                                    {/* Plus Button */}
                                    <img
                                        src={PlusIcon}
                                        alt="Plus Icon"
                                        style={{
                                            width: "30px",
                                            height: "30px",
                                            cursor: "pointer",
                                            marginBottom: "15px",
                                        }}
                                        onClick={() => {
                                            const currentValue = parseInt(
                                                patientData.age || "0",
                                                10,
                                            );
                                            if (currentValue < 150) {
                                                const newValue =
                                                    currentValue + 1;
                                                const { year, month } =
                                                    calculateDobFromAge(
                                                        newValue,
                                                    );
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    age: newValue.toString(),
                                                    birthYear: year,
                                                    birthMonth: month,
                                                }));
                                                setAgeError("");
                                            } else {
                                                setAgeError(
                                                    "Age cannot be more than 150",
                                                );
                                                setTimeout(() => {
                                                    setPatientData((prev) => ({
                                                        ...prev,
                                                        age: "",
                                                    }));
                                                    setAgeError("");
                                                }, 500);
                                            }
                                        }}
                                    />
                                </div>
                            )}

                            {/* DOB MODE */}
                            {ageMode === "dob" && (
                                <div
                                    style={{
                                        display: "flex",
                                        flexDirection: "column",
                                        alignItems: "center",
                                        gap: "15px",
                                    }}
                                >
                                    {/* Year Dropdown */}
                                    <span
                                        style={{
                                            fontWeight: 600,
                                            fontSize: "0.9rem",
                                            marginBottom: "-12px",
                                        }}
                                    >
                                        Year
                                    </span>
                                    <CustomSelect
                                        id="birthYear"
                                        name="birthYear"
                                        placeholder="Select Year"
                                        value={patientData.birthYear || ""}
                                        options={Array.from(
                                            {
                                                length:
                                                    new Date().getFullYear() -
                                                    1900 +
                                                    1,
                                            },
                                            (_, i) => {
                                                const year =
                                                    new Date().getFullYear() -
                                                    i;
                                                return {
                                                    value: year.toString(),
                                                    label: year.toString(),
                                                };
                                            },
                                        )}
                                        onChange={(e) => {
                                            const value = e.target.value;

                                            const currentYear =
                                                new Date().getFullYear();
                                            if (
                                                value &&
                                                (parseInt(value, 10) < 1900 ||
                                                    parseInt(value, 10) >
                                                        currentYear)
                                            ) {
                                                setDobErrors((prev) => ({
                                                    ...prev,
                                                    year: `Year must be between 1900 and ${currentYear}`,
                                                }));
                                            } else {
                                                setDobErrors((prev) => ({
                                                    ...prev,
                                                    year: "",
                                                }));
                                            }

                                            setPatientData((prev) => ({
                                                ...prev,
                                                birthYear: value,
                                            }));

                                            // Sync Age
                                            if (
                                                value &&
                                                patientData.birthMonth
                                            ) {
                                                const newAge =
                                                    calculateAgeFromYearMonth(
                                                        value,
                                                        patientData.birthMonth,
                                                    );
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    age: newAge.toString(),
                                                }));
                                            }
                                        }}
                                        error={dobErrors.year}
                                        width="130px"
                                    />

                                    {/* Month Dropdown */}
                                    <span
                                        style={{
                                            fontWeight: 600,
                                            fontSize: "0.9rem",
                                            marginBottom: "-12px",
                                        }}
                                    >
                                        Month
                                    </span>
                                    <CustomSelect
                                        id="birthMonth"
                                        name="birthMonth"
                                        placeholder="Select Month"
                                        value={patientData.birthMonth || ""}
                                        options={Array.from(
                                            { length: 12 },
                                            (_, i) => {
                                                const monthNum = (i + 1)
                                                    .toString()
                                                    .padStart(2, "0");
                                                return {
                                                    value: monthNum,
                                                    label: monthNum,
                                                };
                                            },
                                        )}
                                        onChange={(e) => {
                                            const value = e.target.value;

                                            if (
                                                value &&
                                                (parseInt(value, 10) < 1 ||
                                                    parseInt(value, 10) > 12)
                                            ) {
                                                setDobErrors((prev) => ({
                                                    ...prev,
                                                    month: "Month must be between 01 and 12",
                                                }));
                                            } else {
                                                setDobErrors((prev) => ({
                                                    ...prev,
                                                    month: "",
                                                }));
                                            }

                                            setPatientData((prev) => ({
                                                ...prev,
                                                birthMonth: value,
                                            }));

                                            // Sync Age
                                            if (
                                                value &&
                                                patientData.birthYear
                                            ) {
                                                const newAge =
                                                    calculateAgeFromYearMonth(
                                                        patientData.birthYear,
                                                        value,
                                                    );
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    age: newAge.toString(),
                                                }));
                                            }
                                        }}
                                        error={dobErrors.month}
                                        width="120px"
                                    />
                                </div>
                            )}
                        </div>
                    )}

                    {selectedItem.label === "Height" && (
                        <div
                            className="height-input"
                            style={{
                                display: "flex",
                                flexDirection: "column",
                                alignItems: "center",
                                marginTop: "10px",
                                gap: "20px",
                            }}
                        >
                            <div
                                style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                    gap: "5px",
                                }}
                            >
                                <span
                                    style={{
                                        fontWeight: 600,
                                        fontSize: "0.9rem",
                                    }}
                                >
                                    Feet
                                </span>
                                <div
                                    style={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "10px",
                                    }}
                                >
                                    <img
                                        src={MinusIcon}
                                        alt="Minus Icon"
                                        style={{
                                            width: "25px",
                                            height: "25px",
                                            cursor: "pointer",
                                            marginBottom: "15px",
                                        }}
                                        onClick={() => {
                                            const currentFeet = parseInt(
                                                patientData.heightFeet || "0",
                                                10,
                                            );

                                            if (currentFeet > 0) {
                                                const newValue =
                                                    currentFeet - 1;

                                                if (newValue < 1) {
                                                    setPatientData((prev) => ({
                                                        ...prev,
                                                        heightFeet:
                                                            newValue.toString(),
                                                    }));
                                                    setFeetError(
                                                        "Invalid Input",
                                                    );

                                                    setTimeout(() => {
                                                        setPatientData(
                                                            (prev) => ({
                                                                ...prev,
                                                                heightFeet: feet || "",
                                                            }),
                                                        );
                                                        setFeetError("");
                                                    }, 500);
                                                } else {
                                                    setPatientData((prev) => ({
                                                        ...prev,
                                                        heightFeet:
                                                            newValue.toString(),
                                                    }));
                                                    setFeetError("");
                                                }
                                            }
                                        }}
                                    />
                                    <div className="edit-feet-vital">
                                        <CustomInput
                                            placeholder="e.g. 5 Feet, 6 Feet etc."
                                            type="text"
                                            value={patientData.heightFeet || ""}
                                            style={{
                                                textAlign: "center",
                                                width: "160px",
                                            }}
                                            onChange={handleChange}
                                        />
                                        <span
                                            style={{
                                                fontSize: "8px",
                                                color: "#65636e",
                                                textAlign: "center",
                                            }}
                                        >
                                            Note: Input cannot be 0 or more than
                                            10
                                        </span>
                                        {feetError && (
                                            <div
                                                style={{
                                                    color: "red",
                                                    marginTop: "5px",
                                                    fontSize: "12px",
                                                    display: "block",
                                                }}
                                            >
                                                {feetError}
                                            </div>
                                        )}
                                    </div>

                                    <img
                                        src={PlusIcon}
                                        alt="Plus Icon"
                                        style={{
                                            width: "30px",
                                            height: "30px",
                                            cursor: "pointer",
                                            marginBottom: "15px",
                                        }}
                                        onClick={() => {
                                            const currentFeet = parseInt(
                                                patientData.heightFeet || "0",
                                                10,
                                            );

                                            if (currentFeet < 10) {
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    heightFeet: (
                                                        currentFeet + 1
                                                    ).toString(),
                                                }));
                                                setFeetError("");
                                            } else {
                                                setFeetError("Invalid Input");
                                                setTimeout(
                                                    () => setFeetError(""),
                                                    500,
                                                );
                                            }
                                        }}
                                    />
                                </div>
                            </div>

                            <div
                                style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                    gap: "5px",
                                }}
                            >
                                <span
                                    style={{
                                        fontWeight: 600,
                                        fontSize: "0.9rem",
                                    }}
                                >
                                    Inches
                                </span>
                                <div
                                    style={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "10px",
                                    }}
                                >
                                    <img
                                        src={MinusIcon}
                                        alt="Minus Icon"
                                        style={{
                                            width: "25px",
                                            height: "25px",
                                            cursor: "pointer",
                                            marginBottom: "15px",
                                        }}
                                        onClick={() => {
                                            const currentInches = parseInt(
                                                patientData.heightInches || "0",
                                                10,
                                            );
                                            if (currentInches > 0) {
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    heightInches: (
                                                        currentInches - 1
                                                    ).toString(),
                                                }));
                                            }
                                        }}
                                    />
                                    <div className="height-inches-edit">
                                        <CustomInput
                                            placeholder="e.g. 0 inch, 10 inches etc."
                                            type="text"
                                            value={
                                                patientData.heightInches || ""
                                            }
                                            style={{
                                                textAlign: "center",
                                                width: "160px",
                                            }}
                                            onChange={handleInchesChange}
                                        />
                                        <span
                                            style={{
                                                fontSize: "10px",
                                                color: "#65636e",
                                                textAlign: "center",
                                            }}
                                        >
                                            Note: Input cannot be more than 11
                                        </span>
                                        {inchesError && (
                                            <div
                                                style={{
                                                    color: "red",
                                                    marginTop: "5px",
                                                    fontSize: "12px",
                                                    display: "block",
                                                }}
                                            >
                                                {inchesError}
                                            </div>
                                        )}
                                    </div>

                                    <img
                                        src={PlusIcon}
                                        alt="Plus Icon"
                                        style={{
                                            width: "30px",
                                            height: "30px",
                                            cursor: "pointer",
                                            marginBottom: "15px",
                                        }}
                                        onClick={() => {
                                            const currentInches = parseInt(
                                                patientData.heightInches || "0",
                                                10,
                                            );

                                            if (currentInches < 11) {
                                                setPatientData((prev) => ({
                                                    ...prev,
                                                    heightInches: (
                                                        currentInches + 1
                                                    ).toString(),
                                                }));
                                                setInchesError("");
                                            } else {
                                                setInchesError("Invalid Input");
                                                setTimeout(
                                                    () => setInchesError(""),
                                                    500,
                                                );
                                            }
                                        }}
                                    />
                                </div>
                            </div>
                        </div>
                    )}

                    {selectedItem.label === "Weight" && (
                        <div
                            className="weight-input"
                            style={{
                                display: "flex",
                                justifyContent: "center",
                                alignItems: "center",
                                marginTop: "10px",
                                gap: "10px",
                            }}
                        >
                            {/* Minus */}
                            <img
                                src={MinusIcon}
                                alt="Minus Icon"
                                style={{
                                    width: "25px",
                                    height: "25px",
                                    cursor: "pointer",
                                    marginLeft: "32px",
                                    marginBottom: "15px",
                                }}
                                onClick={() => {
                                    const currentValue = parseInt(
                                        patientData.weight || "0",
                                        10,
                                    );
                                    const newValue = currentValue - 1;

                                    if (newValue >= 1) {
                                        setPatientData((prev) => ({
                                            ...prev,
                                            weight: newValue.toString(),
                                        }));
                                        setWeightError("");
                                    } else {
                                        setPatientData((prev) => ({
                                            ...prev,
                                            weight: newValue.toString(),
                                        }));
                                        setWeightError("Invalid Input");

                                        setTimeout(() => {
                                            setPatientData((prev) => ({
                                                ...prev,
                                                weight: "",
                                            }));
                                            setWeightError("");
                                        }, 500);
                                    }
                                }}
                            />

                            {/* Input + Error */}
                            <div
                                className="weight-edit"
                                style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                }}
                            >
                                <CustomInput
                                    placeholder="Enter Weight"
                                    type="text"
                                    value={
                                        patientData.weight
                                            ? parseInt(patientData.weight)
                                            : ""
                                    }
                                    style={{
                                        textAlign: "center",
                                        width: "100px",
                                    }}
                                    onChange={handleWeightChange}
                                />
                                <span
                                    style={{
                                        fontSize: "8px",
                                        color: "#65636e",
                                        textAlign: "center",
                                        marginTop: "5px",
                                    }}
                                >
                                    Note: Input between 1 and 600
                                </span>
                                {weightError && (
                                    <div
                                        style={{
                                            color: "red",
                                            marginTop: "5px",
                                            fontSize: "12px",
                                            textAlign: "center",
                                        }}
                                    >
                                        {weightError}
                                    </div>
                                )}
                            </div>

                            {/* Plus */}
                            <img
                                src={PlusIcon}
                                alt="Plus Icon"
                                style={{
                                    width: "30px",
                                    height: "30px",
                                    cursor: "pointer",
                                    marginBottom: "15px",
                                }}
                                onClick={() => {
                                    const currentValue = parseInt(
                                        patientData.weight || "0",
                                        10,
                                    );
                                    const newValue = currentValue + 1;

                                    if (newValue <= 600) {
                                        setPatientData((prev) => ({
                                            ...prev,
                                            weight: newValue.toString(),
                                        }));
                                        setWeightError("");
                                    } else {
                                        setPatientData((prev) => ({
                                            ...prev,
                                            weight: newValue.toString(),
                                        }));
                                        setWeightError("Invalid Input");

                                        setTimeout(() => {
                                            setPatientData((prev) => ({
                                                ...prev,
                                                weight: "",
                                            }));
                                            setWeightError("");
                                        }, 500);
                                    }
                                }}
                            />

                            {/* Unit */}
                            <span
                                style={{
                                    fontFamily: "Georama",
                                    fontSize: "0.9rem",
                                    color: "#65636e",
                                    fontWeight: 600,
                                    marginBottom: "15px",
                                }}
                            >
                                Kg
                            </span>
                        </div>
                    )}
                </CustomModal>
            )}
        </>
    );
};

export default EditNewModal;
