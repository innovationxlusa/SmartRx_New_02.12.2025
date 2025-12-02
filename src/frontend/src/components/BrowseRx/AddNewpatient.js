import React, { useState, useEffect, useRef, useMemo } from "react";
// import ProfilePicture from "../../components/Profile/ProfilePicture";
import ProfileButton from "../static/Commons/CommonButton";
import FieldProgress from "../static/Commons/FieldProgress";
import TextField from "../static/Commons/TextField";
import DateField from "../static/Commons/DateField";
import PageTitle from "../static/PageTitle/PageTitle";
import useApiClients from "../../services/useApiClients";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import useToastMessage from "../../hooks/useToastMessage";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import RewardPopup from "../Reward/RewardPopup";


const AddNewpatient = ({customStyles, refetch }) => {
    const { api } = useApiClients();
    const loginUserId = useCurrentUserId();

    const [selectedGender, setSelectedGender] = useState("");
    const [selectedHeightF, setSelectedHeightF] = useState("");
    const [selectedHeightI, setSelectedHeightI] = useState("");

    // State for existing patient checkbox
    const [isLoading, setIsLoading] = useState(false);
    const [errors, setErrors] = useState({});
    const [fieldErrors, setFieldErrors] = useState({});
    const showToast = useToastMessage();
    const containerRef = useRef(null);
    const { smartNavigate } = useSmartNavigate({ scroll: "top" });
    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const scrollToSection = (key) => {
        const target = containerRef?.current;
        if (target) {
            target.scrollIntoView({
                behavior: "smooth",
                top: 0,
            });
        }
    };

    const calculateAge = (birthDate) => {
        if (!birthDate) return 0;
        const today = new Date();
        const birth = new Date(birthDate);
        let age = today.getFullYear() - birth.getFullYear();
        const monthDiff = today.getMonth() - birth.getMonth();

        if (
            monthDiff < 0 ||
            (monthDiff === 0 && today.getDate() < birth.getDate())
        ) {
            age--;
        }
        return age >= 0 ? age : 0;
    };

    // Create refs for form fields to enable scrolling to validation errors
    const fieldRefs = {
        firstName: useRef(null),
        lastName: useRef(null),
        dateOfBirth: useRef(null),
        gender: useRef(null),
        height: useRef(null),
        heightF: useRef(null),
        heightI: useRef(null),
        weight: useRef(null),
        age: useRef(null),
    };
    const validateForm = () => {
        let newErrors = {};

        if (!formData.firstName?.trim())
            newErrors.firstName = "First Name is required";
        if (!formData.lastName?.trim())
            newErrors.lastName = "Last Name is required";
        if (!formData.dateOfBirth)
            newErrors.dateOfBirth = "Date of Birth is required";
        if (!selectedGender) newErrors.gender = "Gender is required";

        if (!formData.heightF || formData.heightF <= 0) {
            newErrors.heightF = "Height in feet is required";
        }
        // if (!formData.heightI || formData.heightI <= 0) {
        //     newErrors.heightI = "Height in inches is required";
        // }
        if (!formData.weight) newErrors.weight = "Weight is required";
        if (!formData.age || formData.age <= 0)
            newErrors.age = "Age is required";

        setErrors(newErrors);

        return newErrors;
    };

    // State for form fields
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        nickName: "",
        dateOfBirth: null,
        age: 0,
        gender: 0,
        weight: 0,
        weightMeasurementUnit: "kg",
        weightMeasurementUnitId: 0,
        height: "",
        heightF: 0,
        heightI: 0,
        heightMeasurementUnit: "ftin",
        heightMeasurementUnitId: 0
    });

    //Create API
    const handleCreatePatient = async (e) => {
        
        e.preventDefault();
        const newErrors = validateForm();
        const hasErrors =
            Object.keys(newErrors).length > 0 ||
            (newErrors.relatives &&
                (newErrors.relatives.duplicate ||
                    Object.values(newErrors.relatives).some(
                        (error) => error && error !== "",
                    )));
        if (hasErrors) {
            const firstErrorField = Object.keys(newErrors)[0];
            fieldRefs[firstErrorField].current?.scrollIntoView({
                behavior: "smooth",
                block: "center",
            });
            fieldRefs[firstErrorField].current?.focus();

            return;
        }
        setIsLoading(true);

        try {
            // Build JSON data
            const jsonData = {
                patientDetails: {
                    firstName:
                        formData.firstName?.trim() ||
                        "",
                    lastName:
                        formData.lastName?.trim() ||
                        "",
                    nickName:
                        formData.nickName?.trim() ||
                        "",
                    age: Number(formData.age || 0),
                    dateOfBirth: formData.dateOfBirth
                        ? new Date(formData.dateOfBirth).toISOString()
                          : null,
                    gender: formData.gender || null,
                    height: `${Number(formData.heightF)  || 0}ft ${Number(formData.heightI) || 0}in`,
                    heightFeet:
                        Number(formData.heightF) > 0
                            ? Number(formData.heightF)
                              : null,
                    heightInches:
                        Number(formData.heightI) > 0
                            ? Number(formData.heightI)
                              : 0,
                    HeightMeasurementUnit: "ftin",
                    heightMeasurementUnitId: 21,
                    weight: formData.weight || 0,
                    WeightMeasurementUnit: "kg",
                    weightMeasurementUnitId: 7,
                },
                loginUserId: loginUserId ?? 0,
            };

            const formDataPayload = new FormData();

            formDataPayload.append("loginUserId", loginUserId ?? 0);
            formDataPayload.append(
                "patientDetails.firstName",
                formData.firstName?.trim() || "",
            );
            formDataPayload.append(
                "patientDetails.lastName",
                formData.lastName?.trim() || "",
            );
            formDataPayload.append(
                "patientDetails.nickName",
                formData.nickName?.trim() || "",
            );
            formDataPayload.append(
                "patientDetails.age",
                formData.age || 0
            );
            formDataPayload.append("patientDetails.AgeYear", 2000);
            formDataPayload.append("patientDetails.AgeMonth", 8);
            formDataPayload.append(
                "patientDetails.dateOfBirth",
                formData.dateOfBirth
                    ? new Date(formData.dateOfBirth).toISOString()
                    : "",
            );
            formDataPayload.append(
                "patientDetails.gender",
                formData.gender || "",
            );
            formDataPayload.append(
                "patientDetails.height",
                `${Number(formData.heightF) || 0}ft ${Number(formData.heightI) || 0}in`,
            );
            formDataPayload.append(
                "patientDetails.heightFeet",
                String(formData.heightF || 0),
            );
            formDataPayload.append(
                "patientDetails.heightInches",
                String(formData.heightI || 0),
            );
            formDataPayload.append(
                "patientDetails.heightMeasurementUnit",
                "ftin",
            );
            formDataPayload.append(
                "patientDetails.heightMeasurementUnitId",
                "21",
            );
            formDataPayload.append(
                "patientDetails.weight",
                String(formData.weight || 0),
            );
            formDataPayload.append(
                "patientDetails.weightMeasurementUnit",
                "kg",
            );
            formDataPayload.append(
                "patientDetails.weightMeasurementUnitId",
                "7",
            );

            const response = await api.createPatient(formDataPayload, "");

            scrollToSection(containerRef);

            setUploadResponse(response);

            try {
                window.scrollTo({
                    top: 0,
                    behavior: "smooth",
                });
            } catch (error) {
                try {
                    document.documentElement.scrollTop = 0;
                } catch (error2) {
                    try {
                        document.body.scrollTop = 0;
                    } catch (error3) {
                        window.scrollTo(0, 0);
                    }
                }
            }

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
                        smartNavigate("/patientProfile");
                    }, 3000);
                } else {
                    smartNavigate("/patientProfile");
                }
            }
        } catch (error) {
            console.error("Error in handleCreatePatient:", error);
        } finally {
            setIsLoading(false);
        }
    };
    // Remove this useEffect as it was causing infinite loop
    // The scroll should be handled in the form submission success callback
    useEffect(() => {
        if (containerRef.current) {
            containerRef.current.scrollTo({ top: 0, behavior: "smooth" });
        }
    }, [handleCreatePatient]);

    return (
        <div
            ref={containerRef}
            style={{ scrollBehavior: "smooth" }}
            className="profile-panel panel-default"
        >
            <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                <div className="panel-body text-center">
                    <PageTitle
                        // pageName={
                        //     isViewingOwnProfile
                        //         ? "My Profile"
                        //         : "Patient Profile"
                        // }
                        pageName="Add Patient"
                        switchButton={false}
                        showProfilePicture={true}
                        isSinglePatientView={true}
                    />
                </div>
                <div
                    style={{
                        textAlign: "left",
                        fontFamily: "Georama",
                    }}
                >
                    {/* First Name */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        First Name: <span style={{ color: "red" }}>*</span>
                    </label>
                    <TextField
                        ref={fieldRefs.firstName}
                        customStyles={{
                            ...customStyles,
                            borderCollapse: "collapse",
                            width: "100%",
                            textAlign: "left",
                            fontFamily: "Georama",
                            input: {
                                width: "100%",
                                padding: "8px",
                                borderRadius: "5px",
                                border: "1px solid var(--overview-border)",
                                fontSize: "14px",
                                fontFamily: "Georama",
                                color: "#65636e",
                            },
                        }}
                        Placeholder={"Enter First Name"}
                        type={"text"}
                        value={formData.firstName}
                        onChange={(e) => {
                            const value = e.target.value;
                            setFormData({
                                ...formData,
                                firstName: value,
                            });
                            // Clear error when valid value is entered
                            if (value.trim()) {
                                setErrors((prev) => ({
                                    ...prev,
                                    firstName: "",
                                }));
                            }
                        }}
                    />
                    {!formData.firstName && (
                        <div style={{ color: "red", fontSize: "12px" }}>
                            First Name is required
                        </div>
                    )}
                    <br />

                    {/* Last Name */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        Last Name: <span style={{ color: "red" }}>*</span>
                    </label>
                    <TextField
                        ref={fieldRefs.lastName}
                        customStyles={{ ...customStyles }}
                        Placeholder={"Enter Last Name"}
                        type={"text"}
                        value={formData.lastName}
                        onChange={(e) => {
                            const value = e.target.value;
                            setFormData({
                                ...formData,
                                lastName: value,
                            });

                            if (value.trim()) {
                                setErrors((prev) => ({
                                    ...prev,
                                    lastName: "",
                                }));
                            }
                        }}
                    />
                    {!formData.lastName && (
                        <div style={{ color: "red", fontSize: "12px" }}>
                            Last Name is required
                        </div>
                    )}
                    <br />

                    {/* Nick Name */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        Nick Name:
                    </label>
                    <TextField
                        customStyles={{ ...customStyles }}
                        Placeholder={"Enter Nick Name"}
                        type={"text"}
                        value={formData.nickName}
                        onChange={(e) =>
                            setFormData({
                                ...formData,
                                nickName: e.target.value,
                            })
                        }
                    />
                    <br />
                    {/* Date of Birth */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        Date of Birth <span style={{ color: "red" }}>*</span>
                    </label>

                    <DateField
                        ref={fieldRefs.dateOfBirth}
                        customStyles={{ ...customStyles }}
                        placeholderText={"Select Date of Birth"}
                        type={"text"}
                        value={formData.dateOfBirth}
                        onChange={(selectedDateObj) => {
                            if (!selectedDateObj) return;

                            // Calculate age using the helper function
                            const calculatedAge = calculateAge(selectedDateObj);

                            setFormData({
                                ...formData,
                                dateOfBirth: selectedDateObj,
                                age: calculatedAge,
                            });

                            // Clear errors when valid value is selected
                            setErrors((prev) => ({
                                ...prev,
                                dateOfBirth: "",
                                age: calculatedAge > 0 ? "" : "Age is required",
                            }));
                        }}
                    />

                    {!formData.dateOfBirth && (
                        <div style={{ color: "red", fontSize: "12px" }}>
                            Date of Birth is required
                        </div>
                    )}
                    <br />
                </div>

                <div>
                    <div style={{ textAlign: "left" }}>
                        <b
                            style={{
                                fontFamily: "Georama",
                                fontStyle: "normal",
                                fontSize: "15px",
                                fontWeight: "500",
                                display: "flex",
                                color: "#65636e",
                                marginBottom: "10px",
                            }}
                        >
                            What is your gender?{" "}
                            <span style={{ color: "red" }}>*</span>
                        </b>
                    </div>
                    <table className="">
                        <thead></thead>
                        <tbody>
                            <tr>
                                <td>
                                    <ProfileButton
                                        ref={fieldRefs.gender}
                                        text="Male"
                                        onClick={() => {
                                            setSelectedGender("Male");
                                            setFormData({
                                                ...formData,
                                                gender: 1,
                                            }); // 1 for Male
                                            setErrors({
                                                ...errors,
                                                gender: "",
                                            }); // clear error on select
                                        }}
                                        customStyles={{
                                            ...customStyles?.button,
                                            boxSizing: "border-box",
                                            display: "flex",
                                            flexDirection: "row",
                                            justifyContent: "center",
                                            alignItems: "center",
                                            padding: "8px 12px",
                                            gap: "10px",
                                            width: "92px",
                                            height: "30px",
                                            left: "18px",
                                            border: "1px solid var(--overview-border)",
                                            borderRadius: "100px",
                                            backgroundColor:
                                                selectedGender === "Male"
                                                    ? "#4B3B8B"
                                                    : "",
                                            color:
                                                selectedGender === "Male"
                                                    ? "#fff"
                                                    : "#65636e",
                                            cursor: "pointer",
                                            transition:
                                                "background 0.2s, color 0.2s",
                                        }}
                                    />
                                </td>
                                <td style={{ paddingLeft: "10px" }}>
                                    <ProfileButton
                                        ref={fieldRefs.gender}
                                        text="Female"
                                        onClick={() => {
                                            setSelectedGender("Female");
                                            setFormData({
                                                ...formData,
                                                gender: 2,
                                            }); // 2 for Female
                                            setErrors({
                                                ...errors,
                                                gender: "",
                                            }); // clear error on select
                                        }}
                                        customStyles={{
                                            ...customStyles?.button,
                                            boxSizing: "border-box",
                                            display: "flex",
                                            flexDirection: "row",
                                            justifyContent: "center",
                                            alignItems: "center",
                                            padding: "8px 12px",
                                            gap: "10px",
                                            width: "92px",
                                            height: "30px",
                                            left: "18px",
                                            border: "1px solid var(--overview-border)",
                                            borderRadius: "100px",
                                            backgroundColor:
                                                selectedGender === "Female"
                                                    ? "#4B3B8B"
                                                    : "",
                                            color:
                                                selectedGender === "Female"
                                                    ? "#fff"
                                                    : "#65636e",
                                            cursor: "pointer",
                                            transition:
                                                "background 0.2s, color 0.2s",
                                        }}
                                    />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    {errors.gender && (
                        <div style={{ color: "red", fontSize: "12px" }}>
                            {errors.gender}
                        </div>
                    )}
                </div>
                <br />
                <div style={{ textAlign: "left" }}>
                    <b
                        style={{
                            fontFamily: "Georama",
                            fontStyle: "normal",
                            fontSize: "15px",
                            fontWeight: "500",
                            display: "flex",
                            color: "#65636e",
                            marginBottom: "15px",
                        }}
                    >
                        Age:
                    </b>

                    <FieldProgress
                        ref={fieldRefs.age}
                        customStyles={{
                            ...customStyles,
                            background:
                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                            borderRadius: "39.0368px",
                            height: "5px",
                            width: "100%",
                        }}
                        profile={0}
                        value={formData.age}
                        maxValue={120}
                        onValueChange={(value) => {
                            setFormData({ ...formData, age: value });
                            // Clear error when valid value is selected
                            if (value > 0) {
                                setErrors((prev) => ({ ...prev, age: "" }));
                            }
                        }}
                        readOnly={true}
                    />
                    {errors.age && (
                        <div
                            style={{
                                color: "red",
                                fontSize: "12px",
                                marginTop: "5px",
                            }}
                        >
                            {errors.age}
                        </div>
                    )}

                    <b
                        style={{
                            fontFamily: "Georama",
                            fontStyle: "normal",
                            fontSize: "15px",
                            fontWeight: "500",
                            display: "flex",
                            color: "#65636e",
                            marginBottom: "15px",
                        }}
                    >
                        What is your weight?{" "}
                        <span style={{ color: "red" }}>*</span>
                    </b>

                    <FieldProgress
                        ref={fieldRefs.weight}
                        customStyles={{
                            ...customStyles,
                            background:
                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                            borderRadius: "39.0368px",
                            height: "5px",
                            width: "100%",
                        }}
                        profile={0}
                        maxValue={250}
                        value={formData.weight}
                        onValueChange={(value) => {
                            setFormData({ ...formData, weight: value });
                            // Clear error when valid value is selected
                            if (value > 0) {
                                setErrors((prev) => ({ ...prev, weight: "" }));
                            }
                        }}
                    />
                    {errors.weight && (
                        <div
                            style={{
                                color: "red",
                                fontSize: "12px",
                                marginTop: "5px",
                            }}
                        >
                            {errors.weight}
                        </div>
                    )}
                    <br />
                    <b
                        style={{
                            fontFamily: "Georama",
                            fontStyle: "normal",
                            fontSize: "15px",
                            fontWeight: "500",
                            display: "flex",
                            color: "#65636e",
                        }}
                    >
                        What is your height?
                    </b>
                    <b
                        style={{
                            fontFamily: "Georama",
                            fontStyle: "normal",
                            fontSize: "12px",
                            fontWeight: "500",
                            display: "flex",
                            color: "#65636e",
                            marginBottom: "15px",
                        }}
                    >
                        Feet <span style={{ color: "red" }}>*</span>
                    </b>
                    <FieldProgress
                        ref={fieldRefs.heightF}
                        customStyles={{
                            ...customStyles,
                            background:
                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                            borderRadius: "39.0368px",
                            height: "5px",
                            width: "100%",
                        }}
                        profile={0}
                        value={formData.heightF}
                        maxValue={8}
                        onValueChange={(value) => {
                            setFormData({ ...formData, heightF: value });
                            // Clear error when valid value is selected
                            if (value > 0) {
                                setErrors((prev) => ({ ...prev, heightF: "" }));
                            }
                        }}
                    />
                    {errors.heightF && (
                        <div
                            style={{
                                color: "red",
                                fontSize: "12px",
                                marginTop: "5px",
                            }}
                        >
                            {errors.heightF}
                        </div>
                    )}

                    <b
                        style={{
                            fontFamily: "Georama",
                            fontStyle: "normal",
                            fontSize: "12px",
                            fontWeight: "500",
                            display: "flex",
                            color: "#65636e",
                            marginBottom: "15px",
                        }}
                    >
                        Inches <span style={{ color: "red" }}>*</span>
                    </b>
                    <FieldProgress
                        ref={fieldRefs.heightI}
                        customStyles={{
                            ...customStyles,
                            background:
                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                            borderRadius: "39.0368px",
                            height: "5px",
                            width: "100%",
                        }}
                        profile={0}
                        value={formData.heightI}
                        maxValue={12}
                        onValueChange={(value) => {
                            setFormData({ ...formData, heightI: value });
                            // Clear error when valid value is selected
                            if (value > 0) {
                                setErrors((prev) => ({ ...prev, heightI: "" }));
                            }
                        }}
                    />
                    {errors.heightI && (
                        <div
                            style={{
                                color: "red",
                                fontSize: "12px",
                                marginTop: "5px",
                            }}
                        >
                            {errors.heightI}
                        </div>
                    )}
                </div>
                <div
                    className="save-section"
                    style={{
                        textAlign: "center",
                        width: "100%",
                        margin: "auto",
                        fontSize: "16px",
                        backgroundColor: "transparent",
                        background: "none",
                    }}
                >
                    <br />
                    <ProfileButton
                        text="Add"
                        onClick={handleCreatePatient}
                        customStyles={{
                            ...customStyles?.button,
                            boxSizing: "border-box",
                            display: "flex",
                            flexDirection: "row",
                            justifyContent: "center",
                            alignItems: "center",
                            padding: "8px 12px",
                            border: "1px solid #4B3B8B",
                            borderRadius: "100px",
                            fontSize: "20px",
                            fontFamily: "Georama",
                            fontWeight: 600,
                            backgroundColor: "#fff",
                            color: "#4b3b8b",
                            cursor: "pointer",
                            margin: "0 auto",
                            transition: "background 0.2s, color 0.2s",
                            width: "120px",
                            height: "40px",
                        }}
                    />
                </div>
                <br />
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
        </div>
    );
};

export default AddNewpatient;
