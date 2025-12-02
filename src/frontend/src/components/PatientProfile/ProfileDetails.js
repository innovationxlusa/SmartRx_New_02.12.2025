import React, { useState, useEffect, useRef, useMemo } from "react";
import { useLocation } from "react-router-dom";
import ProfilePicture from "./ProfilePicture";
import ProfileButton from "../static/Commons/CommonButton";
import FieldProgress from "../static/Commons/FieldProgress";
import TextField from "../static/Commons/TextField";
import DateField from "../static/Commons/DateField";
import CustomCheck from "../static/Commons/CustomCheck";
import PageTitle from "../static/PageTitle/PageTitle";
import ProfileProgress from "./ProfileProgress";
import relationShip from "../../assets/img/Relationship.svg";
import { FaPlus, FaTrash, FaMobileAlt } from "react-icons/fa";
import { BLOOD_GROUPS, GENDER } from "../../constants/constants";
import CustomSelect from "../static/Dropdown/CustomSelect";
import CustomInput from "../static/Commons/CustomInput";
import useApiClients from "../../services/useApiClients";
import { useUserContext } from "../../contexts/UserContext";
import useToastMessage from "../../hooks/useToastMessage";
import DefaultProfilePhoto from "../../assets/img/DefaultProfilePhoto.svg";
import { validatePhoneNumber } from "../../utils/validators";
import { BASE_URL } from "../../constants/apiEndpoints";
import { getColorForName } from "../../constants/constants";
import axios from "axios";
import RewardPopup from "../Reward/RewardPopup";

const ProfileDetails = ({ data, profile, customStyles, refetch }) => {
    const location = useLocation();
    const initialColoredName = location.state?.coloredName;
    const initialColorForDefaultName = location.state?.colorForDefaultName;
    const { user } = useUserContext();
    const { api } = useApiClients();
    const rawPatientData = location.state?.data;
    const directPatientData = location.state?.patientData;

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    // Initial patient data from location state or props
    const initialPatientData = useMemo(() => {
        let result = {};
        //Data from PageTitle.js
        if (rawPatientData?.data) {
            result = rawPatientData.data;
        }
        //Data from patientInformation.js
        else if (directPatientData) {
            if (directPatientData.data && directPatientData.status) {
                result = directPatientData.data;
            } else {
                result = directPatientData;
            }
        } else if (data && Object.keys(data).length > 0) {
            result = data;
        }

        return result;
    }, [rawPatientData, directPatientData, data]);

    // Local state for patient data that can be updated after refetch
    const [patientData, setPatientData] = useState(initialPatientData);

    // Generate patient name for display
    const patientFullName = useMemo(() => {
        if (!patientData || Object.keys(patientData).length === 0) return "";
        return `${patientData.firstName || ""} ${patientData.lastName || ""} ${patientData.nickName || ""}`.trim();
    }, [patientData?.firstName, patientData?.lastName, patientData?.nickName]);

    // Generate initials and color for patient
    const generateInitials = useMemo(() => {
        if (patientData?.firstName || patientData?.lastName) {
            const firstInitial =
                patientData.firstName?.charAt(0)?.toUpperCase() || "";
            const lastInitial =
                patientData.lastName?.charAt(0)?.toUpperCase() || "";
            if (firstInitial && lastInitial) {
                return firstInitial + lastInitial;
            }
            return firstInitial || lastInitial || "P";
        }
        return initialColoredName || "P";
    }, [patientData?.firstName, patientData?.lastName, initialColoredName]);

    const patientBackgroundColor = useMemo(() => {
        if (patientFullName) {
            return getColorForName(patientFullName);
        }
        return initialColorForDefaultName || "#e6e4ef";
    }, [patientFullName, initialColorForDefaultName]);

    // Update local patientData when initialPatientData changes
    useEffect(() => {
        if (initialPatientData && Object.keys(initialPatientData).length > 0) {
            setPatientData(initialPatientData);
        }
    }, [initialPatientData]);

    const isViewingOwnProfile = useMemo(() => {
        return user?.id === patientData?.id || user?.UserId === patientData?.id;
    }, [user?.id, user?.UserId, patientData?.id]);

    if (!patientData || Object.keys(patientData).length === 0) {
        return (
            <div className="alert alert-danger text-center">
                Profile data is unavailable.
            </div>
        );
    }

    const [emailError, setEmailError] = useState("");
    const [selectedGender, setSelectedGender] = useState("");
    const [selectedHeightF, setSelectedHeightF] = useState("");
    const [selectedHeightI, setSelectedHeightI] = useState("");

    const [isExistingPatient, setIsExistingPatient] = useState(false);
    const [progress, setProgress] = useState(0);
    const [selectedValue, setSelectedValue] = useState("");
    const [patientRelativeOptions, setPatientRelativeOptions] = useState([]);
    const [rows, setRows] = useState([
        { selectValue: "", textValue: "", relationToPatient: "" },
    ]);
    const [isLoading, setIsLoading] = useState(false);
    const [errors, setErrors] = useState({});
    const fileInputRef = useRef([]);
    const [fieldErrors, setFieldErrors] = useState({});
    const [selectedFile, setSelectedFile] = useState(null);
    const [profileImageUrl, setProfileImageUrl] = useState(null);
    const showToast = useToastMessage();

    // Country code state
    const [CountryData, setCountryData] = useState([]);
    const [isCountryLoading, setIsCountryLoading] = useState(false);
    const [countryError, setCountryError] = useState(null);
    const [countryCode, setCountryCode] = useState("+880");
    const hasFetchedCountryCodes = useRef(false);

    const containerRef = useRef(null);

    // Initialize state from patient data
    useEffect(() => {
        if (patientData && Object.keys(patientData).length > 0) {
            // Initialize checkbox state
            setIsExistingPatient(patientData.isExistingPatient || false);

            // Initialize dropdown value only if checkbox is checked
            if (
                patientData.isExistingPatient &&
                patientData.existingPatientId
            ) {
                setSelectedValue(String(patientData.existingPatientId));
            } else {
                setSelectedValue("");
            }
        }
    }, [patientData]);

    // Clear dropdown when checkbox is unchecked
    useEffect(() => {
        if (!isExistingPatient) {
            setSelectedValue("");
            // Clear any existing patient errors
            setErrors((prev) => ({
                ...prev,
                existingPatient: "",
            }));
        }
    }, [isExistingPatient]);

    // Fetch country codes
    useEffect(() => {
        if (hasFetchedCountryCodes.current) return;
        hasFetchedCountryCodes.current = true;

        const fetchCountryCodes = async () => {
            try {
                setIsCountryLoading(true);
                setCountryError(null);

                const response = await axios.get(
                    `${BASE_URL}Country/GetAllCountry`,
                );
                if (response.data && response.data.status === "Success") {
                    const countries = response.data.data || [];
                    setCountryData(countries);

                    if (countries.length > 0) {
                        setCountryCode(countries[0].countryCode);
                    }
                } else {
                    setCountryData([]);
                }
            } catch (error) {
                console.error("Error fetching country codes:", error);
                setCountryError(error);
                setCountryData([]);
            } finally {
                setIsCountryLoading(false);
            }
        };

        fetchCountryCodes();
    }, []);

    const hasProfilePhoto =
        patientData?.profilePhotoPath !== "" &&
        patientData?.profilePhotoPath !== "";

    const scrollToSection = (key) => {
        const target = containerRef?.current;
        if (target) {
            target.scrollIntoView({
                behavior: "smooth",
                top: 0,
            });
        }
    };

    function cleanPhoneNumber(phone) {
        if (!phone) return "";
        // Remove non-digit chars first
        const digitsOnly = phone.replace(/\D/g, "");
        // Get last 11 digits
        return digitsOnly.slice(-11);
    }

    // Function to calculate age from date of birth
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

    //Grid for dynamic select fields
    const handleAddRow = () => {
        setRows([
            ...rows,
            { selectValue: "", textValue: "", relationToPatient: "" },
        ]);
        // Clear any existing relative dropdown errors when adding a new row
        setErrors((prev) => {
            const next = { ...prev };
            // Clear existingPatient error if it was related to relatives
            if (
                prev.existingPatient &&
                prev.existingPatient.includes("relative")
            ) {
                next.existingPatient = "";
            }
            // Clear all per-row relatives errors since we're adding a new row
            if (prev.relatives) {
                next.relatives = {};
            }
            return next;
        });
    };

    const handleDeleteRow = (index) => {
        const updatedRows = [...rows];
        updatedRows.splice(index, 1);
        setRows(updatedRows);
    };

    const handleSelectChange = (index, value) => {
        // Block if trying to pick the same as ExistingPatientId
        if (value && value === selectedValue) {
            setErrors((prev) => ({
                ...prev,
                relatives: {
                    ...(prev.relatives || {}),
                    [index]:
                        "This patient is already selected as Existing Patient",
                },
            }));
            return;
        }

        // Block if trying to pick a patient that's already selected in another relative row
        const isDuplicateInRelatives = rows.some(
            (row, idx) => idx !== index && row.selectValue === value,
        );
        if (value && isDuplicateInRelatives) {
            setErrors((prev) => ({
                ...prev,
                relatives: {
                    ...(prev.relatives || {}),
                    [index]:
                        "This patient is already selected in another relative row",
                    duplicate:
                        "Duplicate patient selected in relatives. Each patient can only be selected once.",
                },
            }));
            return;
        }

        const updatedRows = [...rows];
        updatedRows[index].selectValue = value;
        setRows(updatedRows);

        // Clear relatives error for this specific row and duplicate error on valid selection
        setErrors((prev) => ({
            ...prev,
            relatives: {
                ...(prev.relatives || {}),
                [index]: "",
                duplicate: "",
            },
        }));
    };

    const handleTextChange = (index, value) => {
        const updatedRows = [...rows];
        updatedRows[index].relationToPatient = value;
        setRows(updatedRows);

        // Clear validation error for this row if both patient and relationship are now valid
        const row = updatedRows[index];
        const hasPatient = row.selectValue && row.selectValue !== "";
        const hasRelationship = value && value.trim() !== "";

        if (hasPatient && hasRelationship) {
            setErrors((prev) => ({
                ...prev,
                relatives: {
                    ...(prev.relatives || {}),
                    [index]: "",
                },
            }));
        }
    };

    // Load patient/relative dropdown options
    useEffect(() => {
        let isMounted = true;
        (async () => {
            try {
                const userId = Number(user?.UserId ?? user?.userId);
                if (!userId) {
                    return;
                }
                const res = await api.getPatientOrRelativeDropdown(userId);
                console.log("Patient/Relative dropdown response:", res);
                // Check different possible response structures
                const dataList =
                    res?.patientDropdowns ||
                    res?.response?.data ||
                    res?.response?.patientDropdowns ||
                    res?.data?.patientDropdowns ||
                    res?.data ||
                    [];
                // Get current patient ID for comparison
                const currentPatientId =
                    patientData?.id || patientData?.patientId;
                // Filter out the current patient from the dropdown options
                const filteredList = (
                    Array.isArray(dataList) ? dataList : []
                ).filter((item) => {
                    const itemPatientId = item?.patientId || item?.id;
                    return (
                        itemPatientId != null &&
                        String(itemPatientId) !== String(currentPatientId)
                    );
                });
                const mappedOptions = filteredList.map((item) => ({
                    label: item?.patientName || item?.name || item?.label,
                    value: String(item?.patientId || item?.id || item?.value),
                }));
                if (isMounted) {
                    setPatientRelativeOptions(mappedOptions);

                    // After options are loaded, set the selectedValue if it exists
                    // Only set if isExistingPatient is true AND there's an existingPatientId
                    if (patientData?.existingPatientId && isExistingPatient) {
                        const existingPatientIdStr = String(
                            patientData.existingPatientId,
                        );
                        // Check if the existingPatientId exists in the options
                        const optionExists = mappedOptions.some(
                            (option) => option.value === existingPatientIdStr,
                        );
                        if (optionExists) {
                            setSelectedValue(existingPatientIdStr);
                        }
                    } else {
                        // Clear selectedValue if not an existing patient
                        setSelectedValue("");
                    }
                }
            } catch (e) {
                console.error("Failed to load patient/relative dropdown:", e);
            }
        })();
        return () => {
            isMounted = false;
        };
    }, [
        patientData?.isExistingPatient,
        isExistingPatient,
        user?.UserId,
        user?.userId,
    ]);

    // Function to load file from backend path
    const loadFileFromPath = async (filePath) => {
        if (!filePath) return DefaultProfilePhoto;
        try {
            const path = process.env.REACT_APP_IMAGE_URL + "/" + filePath;
            const response = await fetch(path);
            if (response.ok) {
                const blob = await response.blob();
                const fileName =
                    filePath.split("/").pop() || "profile-photo.jpg";
                const file = new File([blob], fileName, { type: blob.type });

                // Create URL for displaying the image
                const imageUrl = URL.createObjectURL(blob);
                setProfileImageUrl(imageUrl);
                return file;
            }
        } catch (error) {
            console.error("Error loading file from path:", error);
        }
        return null;
    };

    // Function to refetch patient data
    const refetchPatientData = async () => {
        try {
            const patientId = patientData?.id;
            if (patientId) {
                const res = await api.getPatientDataById({ patientId });
                if (res?.data) {
                    // Update the form data with fresh data from API
                    const freshData = res.data;

                    // Update local patientData state with fresh data
                    setPatientData(freshData);

                    // Load profile photo file from backend path
                    if (freshData?.profilePhotoPath) {
                        const profileFile = await loadFileFromPath(
                            freshData.profilePhotoPath,
                        );
                        if (profileFile) {
                            setSelectedFile(profileFile);
                            // Create URL for the fresh image from backend
                            const imageUrl = URL.createObjectURL(profileFile);
                            setProfileImageUrl(imageUrl);
                        }
                    } else {
                        // If no profile photo path, clear the image URL to show default
                        setProfileImageUrl(null);
                        setSelectedFile(null);
                    }

                    // Update form fields
                    const birthDate = freshData?.dateOfBirth
                        ? new Date(freshData.dateOfBirth)
                        : null;
                    // Prioritize freshData.age if available, otherwise calculate from dateOfBirth
                    let calculatedAge = 0;
                    if (
                        freshData?.age !== undefined &&
                        freshData?.age !== null &&
                        Number(freshData.age) > 0
                    ) {
                        // Use the age from freshData directly if available and valid
                        calculatedAge = Number(freshData.age);
                    } else if (birthDate) {
                        // Fallback to calculating from dateOfBirth
                        calculatedAge = calculateAge(birthDate);
                    }

                    // Parse height from API if heightFeet/heightInches are not available
                    let feet = freshData?.heightFeet || 0;
                    let inches = freshData?.heightInches || 0;

                    if (!feet && !inches && freshData?.height) {
                        const numericHeight = freshData.height.replace(
                            /[^\d.]/g,
                            "",
                        );
                        const [feetPart, inchPart] = numericHeight.split(".");
                        feet = parseInt(feetPart, 10) || 0;
                        inches = parseInt(inchPart || "0", 10) / 10;
                    }
                    // Update existing patient checkbox and dropdown first
                    const newIsExistingPatient =
                        freshData.isExistingPatient || false;
                    setIsExistingPatient(newIsExistingPatient);

                    // Only set selectedValue if isExistingPatient is true AND there's an existingPatientId
                    if (newIsExistingPatient && freshData?.existingPatientId) {
                        const existingPatientIdStr = String(
                            freshData.existingPatientId,
                        );
                        setSelectedValue(existingPatientIdStr);
                    } else {
                        setSelectedValue("");
                    }

                    // Update form data with correct existing patient values
                    setFormData((prevData) => ({
                        ...prevData,
                        firstName: freshData?.firstName || "",
                        lastName: freshData?.lastName || "",
                        nickName: freshData?.nickName || "",
                        phoneNumber: (() => {
                            const mobileNo = freshData?.mobileNo || "";
                            if (mobileNo.startsWith("8801")) {
                                return mobileNo.substring(2); // Remove "88" prefix, show "01..."
                            }
                            return mobileNo;
                        })(),
                        email: freshData?.email || "",
                        dateOfBirth: birthDate,
                        profession: freshData?.profession || "",
                        gender: freshData?.gender || 0,
                        age: calculatedAge,
                        weight: freshData?.weight || 0,
                        heightF: feet,
                        heightI: inches,
                        bloodGroup: freshData?.bloodGroup || "",
                        address: freshData?.address || "",
                        IsExistingPatient: newIsExistingPatient,
                        existingPatientId:
                            newIsExistingPatient && freshData?.existingPatientId
                                ? Number(freshData.existingPatientId)
                                : null,
                        isRelative: freshData?.isRelative || false,
                        relationship: freshData?.relationship || "",
                        profileProgress: freshData?.profileProgress || 0,
                        relatives: freshData?.relatives || [],
                    }));

                    // Update gender selection
                    if (freshData?.gender !== undefined) {
                        const genderValue = GENDER[freshData.gender];
                        setSelectedGender(genderValue);
                    }
                    if (
                        freshData?.relatives &&
                        Array.isArray(freshData.relatives) &&
                        freshData.relatives.length > 0
                    ) {
                        const relativesRows = freshData.relatives.map(
                            (relative) => ({
                                selectValue: relative?.id
                                    ? String(relative.id)
                                    : "",
                                textValue: relative?.relationToPatient || "",
                                relationToPatient:
                                    relative?.relationToPatient || "",
                            }),
                        );
                        setRows(relativesRows);
                    } else {
                        setRows([
                            {
                                selectValue: "",
                                textValue: "",
                                relationToPatient: "",
                            },
                        ]);
                    }
                }
            }
        } catch (error) {
            console.error("Error refetching patient data:", error);
        }
    };

    // Cleanup function for image URLs to prevent memory leaks
    useEffect(() => {
        return () => {
            if (profileImageUrl) {
                URL.revokeObjectURL(profileImageUrl);
            }
        };
    }, [profileImageUrl]);

    //Gender initialization
    useEffect(() => {
        if (patientData?.gender !== undefined) {
            const genderValue = GENDER[patientData?.gender];
            setSelectedGender(genderValue);
        }
    }, [patientData?.gender]);

    // Build options dynamically from BLOOD_GROUPS
    const bloodGroupOptions = Object.entries(BLOOD_GROUPS).map(
        ([key, value]) => ({
            label: value,
            value: Number(key),
        }),
    );

    // Create refs for form fields to enable scrolling to validation errors
    const fieldRefs = {
        firstName: useRef(null),
        lastName: useRef(null),
        phoneNumber: useRef(null),
        email: useRef(null),
        dateOfBirth: useRef(null),
        profession: useRef(null),
        gender: useRef(null),
        height: useRef(null),
        heightF: useRef(null),
        heightI: useRef(null),
        weight: useRef(null),
        bloodGroup: useRef(null),
        age: useRef(null),
        existingPatient: useRef(null),
        relatives: useRef(null),
    };
    const validateForm = () => {
        let newErrors = {};

        if (!formData.firstName?.trim())
            newErrors.firstName = "First Name is required";
        if (!formData.lastName?.trim())
            newErrors.lastName = "Last Name is required";
        if (!formData.phoneNumber?.trim())
            newErrors.phoneNumber = "Phone number is required";
        // if (!formData.email?.trim()) newErrors.email = "Email is required";
        if (!formData.dateOfBirth)
            newErrors.dateOfBirth = "Date of Birth is required";
        // if (!formData.profession?.trim()) newErrors.profession = "Profession is required";
        if (!selectedGender) newErrors.gender = "Gender is required";
        // Only validate height if user has actually set values or if there's existing data
        const hasHeightData =
            (patientData?.heightFeet && patientData?.heightFeet > 0) ||
            (patientData?.heightInches && patientData?.heightInches > 0) ||
            (patientData?.height && patientData?.height.trim() !== "");

        if (hasHeightData && (!formData.heightF || formData.heightF <= 0)) {
            newErrors.heightF = "Height in feet is required";
        }
        // if (hasHeightData && (!formData.heightI || formData.heightI <= 0)) {
        //     newErrors.heightI = "Height in inches is required";
        // }
        if (!formData.weight) newErrors.weight = "Weight is required";
        if (!formData.age || formData.age <= 0)
            newErrors.age = "Age is required";

        // Check if existing patient is selected when checkbox is checked
        if (
            isExistingPatient &&
            (!selectedValue ||
                selectedValue === "" ||
                selectedValue === "Tag Here..." ||
                selectedValue === "0")
        ) {
            newErrors.existingPatient =
                "Please select a patient if you are an existing patient";
        }

        // Check if existing patient is also selected in relatives
        if (
            selectedValue &&
            selectedValue !== "" &&
            selectedValue !== "Tag Here..." &&
            selectedValue !== "0"
        ) {
            const isInRelatives = rows.some(
                (row) => row.selectValue === selectedValue,
            );
            if (isInRelatives) {
                newErrors.existingPatient =
                    "A patient cannot be both existing patient and relative";
            }
        }

        // Check for duplicate patient IDs in relatives array
        const relativePatientIds = rows
            .filter((row) => row.selectValue && row.selectValue !== "")
            .map((row) => row.selectValue);

        const duplicateIds = relativePatientIds.filter(
            (id, index) => relativePatientIds.indexOf(id) !== index,
        );
        if (duplicateIds.length > 0) {
            newErrors.relatives = {
                ...newErrors.relatives,
                duplicate:
                    "Duplicate patient selected in relatives. Each patient can only be selected once.",
            };

            // Also mark the specific rows that have duplicates
            duplicateIds.forEach((duplicateId) => {
                rows.forEach((row, index) => {
                    if (row.selectValue === duplicateId) {
                        if (!newErrors.relatives) newErrors.relatives = {};
                        newErrors.relatives[index] =
                            "This patient is selected multiple times";
                    }
                });
            });
        }

        // Check if any relative is selected as existing patient
        if (
            selectedValue &&
            selectedValue !== "" &&
            selectedValue !== "Tag Here..." &&
            selectedValue !== "0"
        ) {
            rows.forEach((row, index) => {
                if (row.selectValue && row.selectValue === selectedValue) {
                    if (!newErrors.relatives) newErrors.relatives = {};
                    newErrors.relatives[index] =
                        "This patient is already selected as Existing Patient";
                }
            });
        }

        // Check for incomplete relative entries (patient selected but no relationship, or relationship but no patient)
        rows.forEach((row, index) => {
            const hasPatient = row.selectValue && row.selectValue !== "";
            const hasRelationship =
                row.relationToPatient && row.relationToPatient.trim() !== "";

            if (hasPatient && !hasRelationship) {
                if (!newErrors.relatives) newErrors.relatives = {};
                newErrors.relatives[index] =
                    "Please enter the relationship for this patient";
            } else if (!hasPatient && hasRelationship) {
                if (!newErrors.relatives) newErrors.relatives = {};
                newErrors.relatives[index] =
                    "Please select a patient for this relationship";
            }
        });

        setErrors(newErrors);

        return newErrors;
    };

    //Blood group initialization
    useEffect(() => {
        if (patientData?.bloodGroup !== undefined) {
            const bloodGroupValue =
                BLOOD_GROUPS[patientData.bloodGroup] || "Others";
            setFormData((prevData) => ({
                ...prevData,
                bloodGroup: bloodGroupValue,
            }));
        }
    }, [patientData?.bloodGroup]);

    // Initialize form data from patientData if available
    useEffect(() => {
        if (patientData && Object.keys(patientData).length > 0) {
            // Load profile photo file from backend path on initial load
            const loadInitialProfilePhoto = async () => {
                if (patientData?.profilePhotoPath) {
                    const profileFile = await loadFileFromPath(
                        patientData.profilePhotoPath,
                    );
                    if (profileFile) {
                        setSelectedFile(profileFile);
                        // Create URL for the initial image from backend
                        const imageUrl = URL.createObjectURL(profileFile);
                        setProfileImageUrl(imageUrl);
                    }
                } else {
                    // If no profile photo path, clear the image URL to show default
                    setProfileImageUrl(null);
                    setSelectedFile(null);
                }
            };
            loadInitialProfilePhoto();

            // Parse height from API
            let feet = patientData?.heightFeet || 0;
            let inches = patientData?.heightInches || 0;

            // If heightFeet/heightInches are not available, try to parse from height string
            if (!feet && !inches && patientData?.height) {
                const numericHeight = patientData.height.replace(/[^\d.]/g, ""); // remove non-numeric except "."
                const [feetPart, inchPart] = numericHeight.split(".");
                feet = parseInt(feetPart, 10) || 0;
                inches = parseInt(inchPart || "0", 10) / 10;
            }

            const birthDate = patientData?.dateOfBirth
                ? new Date(patientData.dateOfBirth)
                : null;
            // Prioritize patientData.age if available, otherwise calculate from dateOfBirth
            let calculatedAge = 0;
            if (
                patientData?.age !== undefined &&
                patientData?.age !== null &&
                Number(patientData.age) > 0
            ) {
                // Use the age from patientData directly if available and valid
                calculatedAge = Number(patientData.age);
            } else if (birthDate) {
                // Fallback to calculating from dateOfBirth
                calculatedAge = calculateAge(birthDate);
            }

            setFormData((prevData) => {
                const newFormData = {
                    ...prevData,
                    firstName: patientData?.firstName || "",
                    lastName: patientData?.lastName || "",
                    nickName: patientData?.nickName || "",
                    phoneNumber: (() => {
                        const mobileNo = patientData?.mobileNo || "";
                        if (mobileNo.startsWith("8801")) {
                            return mobileNo.substring(2); // Remove "88" prefix, show "01..."
                        }
                        return mobileNo;
                    })(),
                    countryCode: patientData?.mobileNo?.startsWith("880")
                        ? "+880"
                        : "+880",
                    email: patientData?.email || "",
                    dateOfBirth: birthDate,
                    profession: patientData?.profession || "",
                    gender: patientData.gender || 0,
                    age: calculatedAge,
                    weight: patientData?.weight || 0,
                    weightMeasurementUnit: "kg",
                    weightMeasurementUnitId:
                        patientData?.weightMeasurementUnitId,
                    height: patientData?.height || 0,
                    heightF: patientData?.heightFeet || feet || 0,
                    heightI: patientData?.heightInches || inches || 0,
                    heightMeasurementUnit: "ftin",
                    heightMeasurementUnitId:
                        patientData?.heightMeasurementUnitId,
                    bloodGroup: patientData?.bloodGroup ?? "",
                    address: patientData?.address || "",
                    profilePhotoName: patientData?.profilePhotoName || null,
                    profilePhotoPath: patientData?.profilePhotoPath || null,
                    profileProgress: patientData?.profileProgress || 0,
                    IsExistingPatient: patientData?.isExistingPatient || false,
                    existingPatientId: patientData?.existingPatientId || null,
                    isRelative: patientData?.isRelative || false,
                    relatedToPatientId: patientData?.patientId || null,
                    relatives: patientData?.relatives || [],
                    relationship: patientData?.relationship || "",
                };
                return newFormData;
            });

            // Set existing patient checkbox
            setIsExistingPatient(patientData.isExistingPatient || false);

            // Set existing patient dropdown value
            if (patientData?.existingPatientId) {
                const existingPatientIdStr = String(
                    patientData.existingPatientId,
                );
                setSelectedValue(existingPatientIdStr);
            }

            // Set relatives rows from patientData.relatives
            if (
                patientData?.relatives &&
                Array.isArray(patientData.relatives) &&
                patientData.relatives.length > 0
            ) {
                const relativesRows = patientData.relatives.map((relative) => ({
                    selectValue: relative?.id ? String(relative.id) : "",
                    textValue:
                        relative?.firstName + " " + relative?.lastName || "",
                    relationToPatient: relative?.relationToPatient || "",
                }));
                setRows(relativesRows);
            } else {
                // Reset to initial row if no relatives
                setRows([
                    { selectValue: "", textValue: "", relationToPatient: "" },
                ]);
            }
        }
    }, [patientData]);

    // State for form fields
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        nickName: "",
        phoneNumber: "",
        countryCode: "+880",
        email: "",
        dateOfBirth: null,
        profession: "",
        bloodGroup: "",
        address: "",
        age: 0,
        gender: 0,
        weight: 0,
        weightMeasurementUnit: "kg",
        weightMeasurementUnitId: 0,
        height: "",
        heightF: 0,
        heightI: 0,
        heightMeasurementUnit: "ftin",
        heightMeasurementUnitId: 0,
        relationship: "",
        IsExistingPatient: false,
        existingPatientId: null,
        isRelative: false,
        relatedToPatientId: null,
        relatives: [],
        profilePhotoName: null,
        profilePhotoPath: null,
        profileProgress: 0,
    });

    //Calculate Progress
    useEffect(() => {
        const totalFields = 15;
        let completedFields = 0;

        if (formData.firstName.trim()) completedFields++;
        if (formData.lastName.trim()) completedFields++;
        if (formData.nickName.trim()) completedFields++;
        if (formData.phoneNumber.trim()) completedFields++;
        if (formData.email.trim()) completedFields++;
        if (formData.dateOfBirth) completedFields++;
        if (formData.profession.trim()) completedFields++;
        if (formData.gender) completedFields++;
        if (formData.bloodGroup) completedFields++;
        if (formData.address.trim()) completedFields++;
        if (formData.age > 0) completedFields++;
        if (formData.weight > 0) completedFields++;
        if (formData.heightF > 0) completedFields++;
        if (formData.heightI > 0) completedFields++;
        if (formData.profilePhotoName || formData.profilePhotoPath)
            completedFields++;

        const percentage = Math.round((completedFields / totalFields) * 100);
        setProgress(percentage);
    }, [formData, selectedGender]);

    const handleFileChange = (e) => {
        const selectedFiles = Array.from(e.target.files || []);
        fileInputRef.current = selectedFiles;
    };
    const handleFileSelect = (file) => {
        setSelectedFile(file);

        // Create URL for the newly selected file
        const imageUrl = URL.createObjectURL(file);
        setProfileImageUrl(imageUrl);
    };
    const handleFileSelected = (fileData = {}, nextModalType) => {
        const files = Array.isArray(fileData.File)
            ? fileData.File
            : [fileData.File];
        if (files.length > 0 && files[0] instanceof File) {
            setSelectedFile(fileData);
        } else {
            setSelectedFile(null);
        }

        setTimeout(() => {
            closeModal();
            setTimeout(() => {
                openModal(nextModalType);
            }, 100);
        }, 100);
    };

    const closeModal = () => {
        setModalType(null);
    };

    //Edit API
    const handleUpdatePatient = async (e) => {
        e.preventDefault();
        const newErrors = validateForm();

        // Check if there are any errors (including nested relatives errors)
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
            const patientId = patientData?.id;
            // Build relatives from UI rows - only include complete entries (both patient and relationship)
            const relatives = rows
                .filter(
                    (r) =>
                        r.selectValue &&
                        r.selectValue !== "" &&
                        r.relationToPatient &&
                        r.relationToPatient.trim() !== "",
                )
                .map((r) => {
                    // Find the selected patient's details from the dropdown options
                    const selectedPatient = patientRelativeOptions.find(
                        (option) => option.value === r.selectValue,
                    );
                    const patientName = selectedPatient
                        ? selectedPatient.label
                        : "";

                    // Split the name into first and last name
                    const nameParts = patientName.split(" ");
                    const firstName = nameParts[0] || "";
                    const lastName = nameParts.slice(1).join(" ") || "";

                    const relativeData = {
                        IsRelative: true,
                        RelationToPatient: r.relationToPatient.trim(),
                        RelatedToPatientId: Number(r.selectValue),
                        FirstName: firstName,
                        LastName: lastName,
                    };
                    return relativeData;
                });

            // Build JSON data
            const jsonData = {
                patientId: patientId,
                patientDetails: {
                    id: patientId,
                    PatientCode: patientData?.patientCode || "",
                    firstName:
                        formData.firstName?.trim() ||
                        patientData?.firstName ||
                        "",
                    lastName:
                        formData.lastName?.trim() ||
                        patientData?.lastName ||
                        "",
                    nickName:
                        formData.nickName?.trim() ||
                        patientData?.nickName ||
                        "",
                    age: formData.age || patientData?.age || 0,
                    dateOfBirth: formData.dateOfBirth
                        ? new Date(formData.dateOfBirth).toISOString()
                        : patientData?.dateOfBirth
                          ? new Date(patientData.dateOfBirth).toISOString()
                          : null,
                    gender: formData.gender || patientData?.gender || null,
                    bloodGroup:
                        formData.bloodGroup || patientData?.bloodGroup || null,
                    height: `${Number(formData.heightF) || Number(patientData?.heightFeet) || 0}ft ${Number(formData.heightI) || Number(patientData?.heightInches) || 0}in`,
                    heightFeet:
                        Number(formData.heightF) > 0
                            ? Number(formData.heightF)
                            : patientData?.heightFeet &&
                                patientData?.heightFeet > 0
                              ? patientData.heightFeet
                              : null,
                    heightInches:
                        Number(formData.heightI) > 0
                            ? Number(formData.heightI)
                            : patientData?.heightInches &&
                                patientData?.heightInches > 0
                              ? patientData.heightInches
                              : null,
                    HeightMeasurementUnit: formData.HeightMeasurementUnit
                        ? formData.HeightMeasurementUnit
                        : patientData?.HeightMeasurementUnit || "ftin",
                    heightMeasurementUnitId: formData.heightMeasurementUnitId
                        ? formData.heightMeasurementUnitId
                        : patientData?.heightMeasurementUnitId,
                    weight: formData.weight
                        ? formData.weight
                        : patientData?.weight || 0,
                    WeightMeasurementUnit: formData.weightMeasurementUnit
                        ? formData.weightMeasurementUnit
                        : patientData.weightMeasurementUnit || "kg",
                    weightMeasurementUnitId: formData.weightMeasurementUnitId
                        ? formData.weightMeasurementUnitId
                        : patientData?.weightMeasurementUnitId,
                    mobileNo: (() => {
                        const mobileNo =
                            formData.phoneNumber || patientData?.mobileNo;
                        const countryCode = formData.countryCode || "+880";
                        const isBangladesh = countryCode === "+880";

                        if (!mobileNo) return "";

                        if (isBangladesh) {
                            if (mobileNo.startsWith("880")) {
                                return mobileNo;
                            } else if (mobileNo.startsWith("01")) {
                                return `88${mobileNo}`;
                            } else if (mobileNo.startsWith("1")) {
                                return `880${mobileNo.slice(1)}`;
                            } else {
                                return `${countryCode.replace("+", "")}${mobileNo}`;
                            }
                        } else {
                            return `${countryCode.replace("+", "")}${mobileNo}`;
                        }
                    })(),
                    email: formData.email || patientData?.email || "",
                    ProfilePhoto:
                        selectedFile || patientData.ProfilePhoto || null, // IFormFile for .NET
                    profilePhotoName: formData.profilePhotoName
                        ? formData.profilePhotoName
                        : patientData?.profilePhotoName || null,
                    profilePhotoPath: formData.profilePhotoPath
                        ? formData.profilePhotoPath
                        : patientData?.profilePhotoPath || null,
                    address: formData.address || patientData?.address || "",
                    policeStationId: patientData?.policeStationId || null,
                    cityId: patientData?.cityId || null,
                    postalCode: patientData?.postalCode || "",
                    emergencyContact: patientData?.emergencyContact || "",
                    maritalStatus: patientData?.maritalStatus || null,
                    profession:
                        formData.profession || patientData?.profession || "",
                    isExistingPatient: isExistingPatient,
                    existingPatientId: (() => {
                        const result =
                            isExistingPatient &&
                            selectedValue &&
                            selectedValue !== "0"
                                ? Number(selectedValue)
                                : null;
                        return result;
                    })(),
                    isRelative: patientData?.isRelative || false,
                    relationToPatient:
                        formData.relationship ||
                        patientData?.relationship ||
                        "",
                    relatedToPatientId: patientData?.patientId || null,
                    profileProgress: progress,
                    relatives,
                },
                loginUserId: Number(user?.UserId),
            };
            const formDataPayload = new FormData();
            // Top-level fields
            if (jsonData.patientId != null)
                formDataPayload.append("PatientId", String(jsonData.patientId));
            if (jsonData.loginUserId != null)
                formDataPayload.append(
                    "LoginUserId",
                    String(jsonData.loginUserId),
                );

            const d = jsonData.patientDetails || {};
            // Simple fields on PatientDetails
            if (d.id != null)
                formDataPayload.append("PatientDetails.Id", String(d.id));
            if (d.PatientCode != null)
                formDataPayload.append(
                    "PatientDetails.PatientCode",
                    String(d.PatientCode),
                );
            // Always include firstName, lastName, and dateOfBirth to prevent validation errors
            formDataPayload.append(
                "PatientDetails.FirstName",
                String(d.firstName || patientData?.firstName || ""),
            );
            formDataPayload.append(
                "PatientDetails.LastName",
                String(d.lastName || patientData?.lastName || ""),
            );
            if (d.nickName != null)
                formDataPayload.append(
                    "PatientDetails.NickName",
                    String(d.nickName),
                );
            if (d.age != null)
                formDataPayload.append("PatientDetails.Age", String(d.age));
            // Always include dateOfBirth to prevent validation errors
            formDataPayload.append(
                "PatientDetails.DateOfBirth",
                String(d.dateOfBirth || patientData?.dateOfBirth || ""),
            );
            if (d.gender != null)
                formDataPayload.append(
                    "PatientDetails.Gender",
                    String(d.gender),
                );
            if (d.bloodGroup != null)
                formDataPayload.append(
                    "PatientDetails.BloodGroup",
                    String(d.bloodGroup),
                );
            if (d.height != null)
                formDataPayload.append(
                    "PatientDetails.Height",
                    String(d.height),
                );
            if (d.heightFeet != null && d.heightFeet > 0)
                formDataPayload.append(
                    "PatientDetails.HeightFeet",
                    String(d.heightFeet),
                );
            if (d.heightInches != null && d.heightInches > 0)
                formDataPayload.append(
                    "PatientDetails.HeightInches",
                    String(d.heightInches),
                );
            if (d.HeightMeasurementUnit != null)
                formDataPayload.append(
                    "PatientDetails.HeightMeasurementUnit",
                    String(d.HeightMeasurementUnit),
                );
            if (d.heightMeasurementUnitId != null)
                formDataPayload.append(
                    "PatientDetails.HeightMeasurementUnitId",
                    String(d.heightMeasurementUnitId),
                );
            if (d.weight != null)
                formDataPayload.append(
                    "PatientDetails.Weight",
                    String(d.weight),
                );
            if (d.WeightMeasurementUnit != null)
                formDataPayload.append(
                    "PatientDetails.WeightMeasurementUnit",
                    String(d.WeightMeasurementUnit),
                );
            if (d.weightMeasurementUnitId != null)
                formDataPayload.append(
                    "PatientDetails.WeightMeasurementUnitId",
                    String(d.weightMeasurementUnitId),
                );
            if (d.mobileNo != null)
                formDataPayload.append(
                    "PatientDetails.mobileNo",
                    String(d.mobileNo),
                );
            if (d.email != null)
                formDataPayload.append("PatientDetails.Email", String(d.email));
            if (d.profilePhotoName != null)
                formDataPayload.append(
                    "PatientDetails.ProfilePhotoName",
                    String(d.profilePhotoName),
                );
            if (d.profilePhotoPath != null)
                formDataPayload.append(
                    "PatientDetails.ProfilePhotoPath",
                    String(d.profilePhotoPath),
                );
            if (d.address != null)
                formDataPayload.append(
                    "PatientDetails.Address",
                    String(d.address),
                );
            if (d.policeStationId != null)
                formDataPayload.append(
                    "PatientDetails.PoliceStationId",
                    String(d.policeStationId),
                );
            if (d.cityId != null)
                formDataPayload.append(
                    "PatientDetails.CityId",
                    String(d.cityId),
                );
            if (d.postalCode != null)
                formDataPayload.append(
                    "PatientDetails.PostalCode",
                    String(d.postalCode),
                );
            if (d.emergencyContact != null)
                formDataPayload.append(
                    "PatientDetails.EmergencyContact",
                    String(d.emergencyContact),
                );
            if (d.maritalStatus != null)
                formDataPayload.append(
                    "PatientDetails.MaritalStatus",
                    String(d.maritalStatus),
                );
            if (d.profession != null)
                formDataPayload.append(
                    "PatientDetails.Profession",
                    String(d.profession),
                );
            if (d.isExistingPatient != null)
                formDataPayload.append(
                    "PatientDetails.IsExistingPatient",
                    String(d.isExistingPatient),
                );
            if (d.isExistingPatient && d.existingPatientId != null) {
                formDataPayload.append(
                    "PatientDetails.ExistingPatientId",
                    String(d.existingPatientId),
                );
            } else {
                formDataPayload.append("PatientDetails.ExistingPatientId", "");
            }

            if (d.isRelative != null)
                formDataPayload.append(
                    "PatientDetails.IsRelative",
                    String(d.isRelative),
                );
            if (d.relationToPatient != null)
                formDataPayload.append(
                    "PatientDetails.RelationToPatient",
                    String(d.relationToPatient),
                );
            if (d.relatedToPatientId != null)
                formDataPayload.append(
                    "PatientDetails.RelatedToPatientId",
                    String(d.relatedToPatientId),
                );
            if (d.profileProgress != null)
                formDataPayload.append(
                    "PatientDetails.ProfileProgress",
                    String(d.profileProgress),
                );

            // Relatives array
            if (Array.isArray(d.relatives)) {
                d.relatives.forEach((rel, idx) => {
                    if (rel?.IsRelative != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].IsRelative`,
                            String(rel.IsRelative),
                        );
                    if (rel?.RelationToPatient != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].RelationToPatient`,
                            String(rel.RelationToPatient),
                        );
                    if (rel?.RelatedToPatientId != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].RelatedToPatientId`,
                            String(rel.RelatedToPatientId),
                        );
                    if (rel?.FirstName != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].FirstName`,
                            String(rel.FirstName),
                        );
                    if (rel?.LastName != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].LastName`,
                            String(rel.LastName),
                        );
                    // Add additional required fields
                    if (rel?.RelatedToPatientId != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].PatientId`,
                            String(rel.RelatedToPatientId),
                        );
                    if (rel?.IsRelative != null)
                        formDataPayload.append(
                            `PatientDetails.Relatives[${idx}].IsActive`,
                            String(rel.IsRelative ? 1 : 0),
                        );
                });
            }

            // Add RelativesDropdown field - required by backend
            if (Array.isArray(d.relatives) && d.relatives.length > 0) {
                // Create RelativesDropdown structure that matches backend expectations
                const relativesDropdown = d.relatives.map((rel) => ({
                    patientId: rel.RelatedToPatientId,
                    patientName: `${rel.FirstName} ${rel.LastName}`.trim(),
                }));
                formDataPayload.append(
                    "PatientDetails.RelativesDropdown",
                    JSON.stringify(relativesDropdown),
                );
            } else {
                formDataPayload.append(
                    "PatientDetails.RelativesDropdown",
                    "[]",
                );
            }
            // File inside PatientDetails as IFormFile
            if (selectedFile instanceof File) {
                formDataPayload.append(
                    "PatientDetails.ProfilePhoto",
                    selectedFile,
                    selectedFile.name,
                );
            }

            const response = await api.patientUpdate(
                patientId,
                formDataPayload,
                "",
            );

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
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        const message = `Patient Information Updated Successfully`;
                        setTimeout(() => {
                            showToast("success", message, "🎉");
                        }, 300);
                    }, 3000);
                } else {
                    const message = `Patient Information Updated Successfully`;
                    setTimeout(() => {
                        showToast("success", message, "🎉");
                    }, 300);

                    //Refetch patient data after successful update
                    await refetchPatientData();
                }
            }
        } catch (error) {
            console.error("Error in handleUpdatePatient:", error);
        } finally {
            setIsLoading(false);
        }
    };
    return (
        <div
            ref={containerRef}
            style={{ scrollBehavior: "smooth" }}
            className="profile-panel panel-default"
        >
            <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                <div className="panel-body text-center">
                    <PageTitle
                        pageName={
                            isViewingOwnProfile
                                ? "My Profile"
                                : "Patient Profile"
                        }
                        switchButton={false}
                        showProfilePicture={true}
                        isSinglePatientView={true}
                    />
                    <div className="position-relative d-inline-block">
                        <ProfilePicture
                            profile={{
                                data: patientData,
                                profilePhotoPath:
                                    profileImageUrl ||
                                    patientData?.profilePhotoPath ||
                                    profile?.profilePhotoPath,
                                picture:
                                    profileImageUrl ||
                                    patientData?.profilePhotoPath ||
                                    profile?.picture,
                                coloredName: generateInitials,
                                colorForDefaultName: patientBackgroundColor,
                            }}
                            onFileSelect={handleFileSelect}
                            disableBackendLoading={true}
                        />
                        <br />
                    </div>

                    <div>
                        <ProfileProgress
                            progress={progress}
                            customStyles={{
                                container: {
                                    margin: "0 auto",
                                },
                            }}
                        />
                    </div>
                    <br />
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

                    {/* Phone Number */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        Phone Number <span style={{ color: "red" }}>*</span>
                    </label>
                    <CustomInput
                        ref={fieldRefs.phoneNumber}
                        className="input-style"
                        label=""
                        labelPosition="top-left"
                        name="phoneNumber"
                        type="tel"
                        placeholder="Mobile No."
                        value={formData.phoneNumber}
                        onChange={(e) => {
                            const value = e.target.value;
                            setFormData((prev) => ({
                                ...prev,
                                phoneNumber: value,
                            }));

                            const error = validatePhoneNumber(
                                value,
                                "Mobile number",
                                formData.countryCode,
                            );
                            setFieldErrors((prev) => ({
                                ...prev,
                                phoneNumber: error,
                            }));
                        }}
                        error={fieldErrors.phoneNumber}
                        disabled={isLoading}
                        icon={<FaMobileAlt />}
                        iconPosition={"left"}
                        minHeight={"0px"}
                        // Country code portion
                        showCountryCode={true}
                        countryCode={formData.countryCode}
                        onCountryCodeChange={(e) => {
                            const newCountryCode = e.target.value;
                            setFormData((prev) => ({
                                ...prev,
                                countryCode: newCountryCode,
                            }));

                            if (formData.phoneNumber) {
                                const error = validatePhoneNumber(
                                    formData.phoneNumber,
                                    "Mobile number",
                                    newCountryCode,
                                );
                                setFieldErrors((prev) => ({
                                    ...prev,
                                    phoneNumber: error,
                                }));
                            }
                        }}
                        countryOptions={(() => {
                            if (CountryData.length > 0) {
                                const options = CountryData.map((country) => ({
                                    label: country.countryCode,
                                    value: country.countryCode,
                                }));
                                return options;
                            } else {
                                return [
                                    {
                                        label: "+880",
                                        value: "+880",
                                    },
                                ];
                            }
                        })()}
                    />
                    <br />

                    {/* Email */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        Email
                    </label>
                    <TextField
                        customStyles={{ ...customStyles }}
                        Placeholder={"Enter Email"}
                        type={"email"}
                        value={formData.email}
                        onChange={(e) => {
                            const value = e.target.value;
                            setFormData({ ...formData, email: value });

                            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                            if (value && !emailRegex.test(value)) {
                                setEmailError("Invalid email address");
                            } else {
                                setEmailError("");
                            }
                        }}
                    />
                    {/* {!formData.email && (
        <div style={{ color: "red", fontSize: "12px" }}>Email is required</div>
    )} */}
                    {emailError && (
                        <div style={{ color: "red", fontSize: "12px" }}>
                            {emailError}
                        </div>
                    )}
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

                    {/* Profession */}
                    <label
                        className="labelStyle"
                        style={{ fontFamily: "Georama", color: "#65636e" }}
                    >
                        Profession
                    </label>
                    <TextField
                        customStyles={{ ...customStyles }}
                        Placeholder={"Enter Profession"}
                        type={"text"}
                        value={formData.profession}
                        onChange={(e) =>
                            setFormData({
                                ...formData,
                                profession: e.target.value,
                            })
                        }
                    />
                    {/* {!formData.profession && (
        <div style={{ color: "red", fontSize: "12px" }}>Profession is required</div>
    )} */}
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
                        How old are you? <span style={{ color: "red" }}>*</span>
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
                    style={{
                        fontSize: "15px",
                        color: "#252525",
                        font: "Georama",
                        textAlign: "left",
                    }}
                >
                    <br />
                    <label style={{ color: "#65636e" }}>
                        What is your blood group?
                    </label>
                    <CustomSelect
                        ref={fieldRefs.bloodGroup}
                        id="blood-group-select"
                        labelPosition="top-left"
                        name="selectedBloodGroup"
                        placeholder="Select Here..."
                        iconTwo=""
                        value={formData.bloodGroup}
                        onChange={(e) => {
                            const value = Number(e.target.value);
                            setFormData((prevData) => ({
                                ...prevData,
                                bloodGroup: value,
                            }));
                            // Clear error when valid value is selected
                            if (value > 0) {
                                setErrors((prev) => ({
                                    ...prev,
                                    bloodGroup: "",
                                }));
                            }
                        }}
                        options={bloodGroupOptions}
                        width="100%"
                        // bgColor="#E6E4EF"
                        textColor="#65636e"
                        borderRadius="8px"
                        borderColors="1px solid #ccc"
                    />
                    <br />
                    <label className="col-12" style={{ color: "#65636e" }}>
                        Address
                    </label>
                    <textarea
                        className="col-12"
                        style={{
                            width: "100%",
                            padding: "8px",
                            borderRadius: "5px",
                            border: "1px solid var(--overview-border)",
                            fontSize: "14px",
                            fontFamily: "Georama",
                            color: "#65636e",
                            resize: "vertical",
                            minHeight: "80px",
                            boxSizing: "border-box",
                            outline: "none",
                        }}
                        placeholder={"Enter Address"}
                        value={formData.address}
                        onChange={(e) =>
                            setFormData({
                                ...formData,
                                address: e.target.value,
                            })
                        }
                    />
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
                        text="Save"
                        onClick={handleUpdatePatient}
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

                    <CustomCheck
                        style={{
                            textAlign: "left",
                            marginTop: "20px",
                            marginBottom: "10px",
                            fontFamily: "Georama",
                            color: "#65636e",
                            fontSize: "16px",
                        }}
                        value={isExistingPatient}
                        onChange={() =>
                            setIsExistingPatient(!isExistingPatient)
                        }
                        label="Are you an existing patient? "
                    />
                </div>

                {isExistingPatient && (
                    <div className="patient-select-section">
                        <div style={{ marginBottom: "10px" }}></div>
                        <CustomSelect
                            ref={fieldRefs.existingPatient}
                            id="patient-select"
                            label="Select Patient"
                            labelPosition="top-left"
                            name="selectedPatient"
                            placeholder="Tag Here..."
                            value={selectedValue}
                            onChange={(e) => {
                                const value = e.target.value;
                                // Prevent selecting an Existing Patient that is already chosen in relatives
                                const isInRelatives = rows.some(
                                    (row) => row.selectValue === value,
                                );
                                if (
                                    value &&
                                    value !== "" &&
                                    value !== "Tag Here..." &&
                                    isInRelatives
                                ) {
                                    setErrors((prev) => ({
                                        ...prev,
                                        existingPatient:
                                            "This patient is already selected as a relative",
                                    }));
                                    return; // do not update selectedValue
                                }

                                const previousSelected = selectedValue;
                                setSelectedValue(value);

                                // Clear existingPatient error and any per-row relatives errors that were tied to previous selection
                                setErrors((prev) => {
                                    const next = { ...prev };
                                    // clear existingPatient message on valid change/clear
                                    next.existingPatient = "";
                                    const nextRelatives = {
                                        ...(prev.relatives || {}),
                                    };
                                    // If previous existing patient matched any row, clear those row errors
                                    rows.forEach((row, idx) => {
                                        if (
                                            row.selectValue === previousSelected
                                        ) {
                                            nextRelatives[idx] = "";
                                        }
                                    });
                                    // If user cleared the existing patient, clear all per-row messages
                                    if (
                                        !value ||
                                        value === "" ||
                                        value === "Tag Here..."
                                    ) {
                                        rows.forEach((_, idx) => {
                                            nextRelatives[idx] = "";
                                        });
                                    }
                                    next.relatives = nextRelatives;
                                    return next;
                                });
                            }}
                            options={[
                                {
                                    label: "Tag Here...",
                                    value: "",
                                    disabled: true,
                                    hidden: true,
                                },
                                ...patientRelativeOptions,
                            ]}
                            width="100%"
                            textColor="#65636e"
                            borderRadius="8px"
                            borderColors="1px solid #ccc"
                        />
                        {errors.existingPatient && (
                            <div
                                style={{
                                    color: "red",
                                    fontSize: "12px",
                                    marginTop: "5px",
                                }}
                            >
                                {errors.existingPatient}
                            </div>
                        )}
                    </div>
                )}
                <br />

                <div>
                    <div style={{ marginLeft: "" }}>
                        <div
                            className="relationship-section"
                            style={{
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "space-between",
                                fontFamily: "Georama",
                                fontWeight: "bold",
                                color: "#65636e",
                            }}
                        >
                            <div
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                }}
                            >
                                <img
                                    src={relationShip}
                                    alt="Relationship"
                                    style={{
                                        width: "20px",
                                        height: "20px",
                                        marginRight: "8px",
                                    }}
                                />
                                <span>Relationship</span>
                            </div>
                            <button
                                onClick={handleAddRow}
                                style={{
                                    background: "none",
                                    border: "none",
                                    cursor: "pointer",
                                    color: "#65636e",
                                    marginRight: "10px",
                                }}
                            >
                                <FaPlus size={20} color="#4b3b8b" />
                            </button>
                        </div>

                        <table className="table" style={{ width: "100%" }}>
                            <thead></thead>
                            <tbody>
                                {rows.map((row, index) => (
                                    <tr key={index}>
                                        <td style={{ width: "45%" }}>
                                            <div className="relation-options">
                                                <CustomSelect
                                                    ref={fieldRefs.relatives}
                                                    id={`row-select-${index}`}
                                                    name="selectValue"
                                                    placeholder="Choose an option"
                                                    value={row.selectValue}
                                                    onChange={(e) => {
                                                        const value =
                                                            e.target.value;

                                                        // Check if the selected patient is already the existing patient
                                                        if (
                                                            value &&
                                                            value ===
                                                                selectedValue
                                                        ) {
                                                            setErrors(
                                                                (prev) => {
                                                                    const updated =
                                                                        {
                                                                            ...prev,
                                                                            relatives:
                                                                                {
                                                                                    ...(prev.relatives ||
                                                                                        {}),
                                                                                    [index]:
                                                                                        "This patient is already selected as Existing Patient",
                                                                                },
                                                                        };

                                                                    return updated;
                                                                },
                                                            );
                                                            return;
                                                        }
                                                        handleSelectChange(
                                                            index,
                                                            value,
                                                        );
                                                        // Clear relatives error when valid selection is made (only for this row)
                                                        setErrors((prev) => ({
                                                            ...prev,
                                                            relatives: {
                                                                ...(prev.relatives ||
                                                                    {}),
                                                                [index]: "",
                                                            },
                                                        }));
                                                        // Don't auto-populate relationship field - let user enter it manually
                                                    }}
                                                    options={[
                                                        {
                                                            label: "--Select--",
                                                            value: "",
                                                        },
                                                        ...patientRelativeOptions,
                                                    ]}
                                                    width="100%"
                                                    textColor="#65636e"
                                                    borderRadius="5px"
                                                    borderColors="1px solid #ccc"
                                                />

                                                {errors.relatives && (
                                                    <div
                                                        style={{
                                                            color: "red",
                                                            fontSize: "12px",
                                                            marginTop: "5px",
                                                        }}
                                                    >
                                                        {
                                                            errors.relatives[
                                                                index
                                                            ]
                                                        }
                                                    </div>
                                                )}
                                                {errors.relatives
                                                    ?.duplicate && (
                                                    <div
                                                        style={{
                                                            color: "red",
                                                            fontSize: "12px",
                                                            marginTop: "5px",
                                                        }}
                                                    >
                                                        {
                                                            errors.relatives
                                                                .duplicate
                                                        }
                                                    </div>
                                                )}
                                            </div>
                                        </td>
                                        <td style={{ width: "55%" }}>
                                            <TextField
                                                customStyles={{
                                                    input: {
                                                        width: "100%",
                                                        padding: "8px",
                                                        borderRadius: "5px",
                                                        border: "1px solid #ccc",
                                                        fontSize: "12px",
                                                        fontFamily: "Georama",
                                                        color: "#65636e",
                                                        height: "36px",
                                                        boxSizing: "border-box",
                                                    },
                                                }}
                                                Placeholder={
                                                    "Mention relationship"
                                                }
                                                type="text"
                                                value={row.relationToPatient}
                                                onChange={(e) =>
                                                    handleTextChange(
                                                        index,
                                                        e.target.value,
                                                    )
                                                }
                                            />
                                        </td>
                                        <td
                                            style={{
                                                width: "10%",
                                                textAlign: "center",
                                            }}
                                        >
                                            <button
                                                onClick={() =>
                                                    handleDeleteRow(index)
                                                }
                                                style={{
                                                    background: "none",
                                                    border: "none",
                                                    cursor: "pointer",
                                                    color: "#4b3b8b",
                                                    marginTop: "4px",
                                                }}
                                                title="Delete Row"
                                            >
                                                <FaTrash size={18} />
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="button-wrapper">
                    <div className="pt-history">
                        <ProfileButton
                            text="Patient History"
                            onClick={() => alert("Patient History Clicked")}
                        />
                    </div>
                    <div className="pt-survey">
                        <ProfileButton
                            text="Patient Survey"
                            onClick={() => alert("Patient Survey Clicked")}
                        />
                    </div>
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

export default ProfileDetails;
