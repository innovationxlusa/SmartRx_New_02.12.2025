import React, { useState, useCallback, useEffect, useRef } from "react";
import "./Signup.css";
import { TbLockPassword } from "react-icons/tb";
import HeroSection from "../HeroSection/HeroSection";
import { validateField, validatePhoneNumber } from "../../utils/validators";
import useFormHandler from "../../hooks/useFormHandler";
import CustomCheck from "../static/Commons/CustomCheck";
import CustomInput from "../static/Commons/CustomInput";
import CustomButton from "../static/Commons/CustomButton";
import useToastMessage from "../../hooks/useToastMessage";
import CustomSelect from "../static/Dropdown/CustomSelect";
import useAuthService from "../../services/useAuthService";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import { useUserContext } from "../../contexts/UserContext";
import { USER_REGISTER_URL } from "../../constants/apiEndpoints";
import { FaMobileAlt, FaCalendarAlt, FaTransgender } from "react-icons/fa";
import { FaRegUser } from "react-icons/fa6";
import { useFetchData } from "../../hooks/useFetchData";
import useApiClients from "../../services/useApiClients";
import axios from "axios";
import { BASE_URL } from "../../constants/apiEndpoints";
import RewardPopup from "../Reward/RewardPopup";


const Signup = () => {
    const { serializeFormData, handleInputChange } = useFormHandler();

    const { signup } = useAuthService();
    const { api } = useApiClients();

    const showToast = useToastMessage();
    const { decodeToken } = useUserContext();
    const { smartNavigate, navigateBack, navigateForward } = useSmartNavigate({
        scroll: "top",
    });

    const initialData = {
        agree: "",
        UserName: "",
        LastName: "",
        MobileNo: "",
        CountryCode: "",
        password: "",
        FirstName: "",
        confirmPassword: "",
    };

    const [CountryData, setCountryData] = useState([]);
    const [isCountryLoading, setIsCountryLoading] = useState(false);
    const [countryError, setCountryError] = useState(null);

    const hasFetchedCountryCodes = useRef(false);

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
                        setFormData((prev) => ({
                            ...prev,
                            CountryCode: countries[0].countryCode,
                        }));
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

    const [isLoading, setIsLoading] = useState(false);
    const [formData, setFormData] = useState(initialData);
    const [fieldErrors, setFieldErrors] = useState(initialData);

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const fieldsToValidate = {
            MobileNo: validatePhoneNumber(
                formData.MobileNo,
                "Mobile number",
                formData.CountryCode,
            ),
            CountryCode: validateField(
                "CountryCode",
                formData.CountryCode,
                "Country Code",
            ),
            FirstName: validateField(
                "FirstName",
                formData.FirstName,
                "First name",
            ),
            LastName: validateField("LastName", formData.LastName, "Last name"),
            password: validateField(
                "password",
                formData.password,
                "Password",
                true,
            ),
            confirmPassword: validateField(
                "confirmPassword",
                {
                    password: formData.password,
                    confirmPassword: formData.confirmPassword,
                },
                "Confirm password",
            ),
            agree: validateField(
                "privacyPolicy",
                formData.agree,
                "You must agree to the Privacy Policy.",
            ),
        };

        if (Object.values(fieldsToValidate).some((error) => error !== "")) {
            setFieldErrors(fieldsToValidate);
            return;
        }

        try {
            setIsLoading(true);
            const serializedData = serializeFormData(e.target);
            const countryCode = formData.CountryCode.replace("+", "");
            const mobileNumber = formData.MobileNo;

            const isBangladesh = formData.CountryCode === "+880";
            let mobileForApi;

            if (isBangladesh) {
                if (mobileNumber.startsWith("880")) {
                    mobileForApi = mobileNumber;
                } else if (mobileNumber.startsWith("17")) {
                    mobileForApi = `880${mobileNumber}`;
                } else if (mobileNumber.startsWith("0")) {
                    mobileForApi = `880${mobileNumber.slice(1)}`;
                } else {
                    mobileForApi = `${countryCode}${mobileNumber}`;
                }
            } else {
                mobileForApi = `${countryCode}${mobileNumber}`;
            }

            const newData = {
                ...serializedData,
                UserName: mobileForApi,
                MobileNo: mobileForApi,
            };

            const response = await signup(USER_REGISTER_URL, newData);
            setUploadResponse(response);
            if (response && response.message === "Successful") {
                if (response?.response?.isRewardUpdated == true) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        const message = `Registration Successful`;
                        smartNavigate("/signIn");
                        setTimeout(() => {
                        showToast("success", message, "🎉");
                        }, 300);
                    }, 3000);
                } else {
                    const message = `Registration Successful`;
                    smartNavigate("/signIn");
                    setTimeout(() => {
                        showToast("success", message, "🎉");
                    }, 300);
                }
            }
        } catch (error) {
            console.error("Error submitting form:", error);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="container px-0">
            <HeroSection
                bottomSection={false}
                backIcon={true}
                landingPage={false}
                backgroundHeight={"140px"}
            >
                <div className="signup-container">
                    <div className="signup-header">
                        <h4>Sign Up</h4>
                    </div>
                    <div className="signup-box">
                        <form onSubmit={handleSubmit} className="">
                            <div className="signup-body">
                                <div className="position-relative w-100">
                                    <CustomInput
                                        className="input-style"
                                        label=""
                                        labelPosition="top-left"
                                        name="FirstName"
                                        type="text"
                                        placeholder="First Name"
                                        value={formData.FirstName}
                                        onChange={(e) =>
                                            handleInputChange(
                                                e,
                                                setFormData,
                                                setFieldErrors,
                                                "input",
                                                "First Name",
                                            )
                                        }
                                        error={fieldErrors.FirstName}
                                        disabled={isLoading}
                                        icon={<FaRegUser />}
                                        iconPosition={"left"}
                                        minHeight={"0px"}
                                    />
                                    <span
                                        style={{
                                            color: "red",
                                            position: "absolute",
                                            right: "-12px",
                                            top: "50%",
                                            transform: "translateY(-50%)",
                                        }}
                                    >
                                        *
                                    </span>
                                </div>

                                <div className="position-relative w-100">
                                    <CustomInput
                                        className="input-style"
                                        label=""
                                        labelPosition="top-left"
                                        name="LastName"
                                        type="text"
                                        placeholder="Last Name"
                                        value={formData.LastName}
                                        onChange={(e) =>
                                            handleInputChange(
                                                e,
                                                setFormData,
                                                setFieldErrors,
                                                "input",
                                                "Last Name",
                                            )
                                        }
                                        error={fieldErrors.LastName}
                                        disabled={isLoading}
                                        icon={<FaRegUser />}
                                        iconPosition={"left"}
                                        minHeight={"0px"}
                                    />
                                    <span
                                        style={{
                                            color: "red",
                                            position: "absolute",
                                            right: "-12px",
                                            top: "50%",
                                            transform: "translateY(-50%)",
                                        }}
                                    >
                                        *
                                    </span>
                                </div>

                                <div className="position-relative w-100">
                                    <CustomInput
                                        className="input-style"
                                        label=""
                                        labelPosition="top-left"
                                        name="MobileNo"
                                        type="tel"
                                        placeholder="Mobile No."
                                        value={formData.MobileNo}
                                        onChange={(e) => {
                                            const value = e.target.value;
                                            setFormData((prev) => ({
                                                ...prev,
                                                MobileNo: value,
                                            }));

                                            const error = validatePhoneNumber(
                                                value,
                                                "Mobile number",
                                                formData.CountryCode,
                                            );
                                            setFieldErrors((prev) => ({
                                                ...prev,
                                                MobileNo: error,
                                            }));
                                        }}
                                        error={fieldErrors.MobileNo}
                                        disabled={isLoading}
                                        icon={<FaMobileAlt />}
                                        iconPosition={"left"}
                                        minHeight={"0px"}
                                        showCountryCode={true}
                                        countryCode={formData.CountryCode}
                                        onCountryCodeChange={(e) => {
                                            const newCountryCode =
                                                e.target.value;
                                            setFormData((prev) => ({
                                                ...prev,
                                                CountryCode: newCountryCode,
                                            }));

                                            if (formData.MobileNo) {
                                                const error =
                                                    validatePhoneNumber(
                                                        formData.MobileNo,
                                                        "Mobile number",
                                                        newCountryCode,
                                                    );
                                                setFieldErrors((prev) => ({
                                                    ...prev,
                                                    MobileNo: error,
                                                }));
                                            }
                                        }}
                                        countryOptions={(() => {
                                            if (CountryData.length > 0) {
                                                const options = CountryData.map(
                                                    (country) => ({
                                                        label: country.countryCode,
                                                        value: country.countryCode,
                                                    }),
                                                );
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
                                    <span
                                        style={{
                                            color: "red",
                                            position: "absolute",
                                            right: "-12px",
                                            top: "50%",
                                            transform: "translateY(-50%)",
                                        }}
                                    >
                                        *
                                    </span>
                                </div>

                                <div className="position-relative w-100">
                                    <CustomInput
                                        className="input-style"
                                        label=""
                                        labelPosition="top-left"
                                        name="password"
                                        type="password"
                                        placeholder="Password"
                                        value={formData.password.replace(
                                            /\//g,
                                            "-",
                                        )}
                                        onChange={(e) =>
                                            handleInputChange(
                                                e,
                                                setFormData,
                                                setFieldErrors,
                                                "password",
                                                "Password",
                                            )
                                        }
                                        error={fieldErrors.password}
                                        disabled={isLoading}
                                        icon={<TbLockPassword />}
                                        iconPosition={"left"}
                                        minHeight={"0px"}
                                    />
                                    <span
                                        style={{
                                            color: "red",
                                            position: "absolute",
                                            right: "-12px",
                                            top: "50%",
                                            transform: "translateY(-50%)",
                                        }}
                                    >
                                        *
                                    </span>
                                </div>

                                <div className="position-relative w-100">
                                <CustomInput
                                    className="input-style"
                                    label=""
                                    labelPosition="top-left"
                                    name="confirmPassword"
                                    type="password"
                                    placeholder="Confirm Password"
                                    value={formData.confirmPassword}
                                    onChange={(e) =>
                                        handleInputChange(
                                            e,
                                            setFormData,
                                            setFieldErrors,
                                            "confirmPassword",
                                            "Confirm Password",
                                        )
                                    }
                                    error={fieldErrors.confirmPassword}
                                    disabled={isLoading}
                                    icon={<TbLockPassword />}
                                    iconPosition={"left"}
                                    minHeight={"0px"}
                                    />
                                    <span
                                        style={{
                                            color: "red",
                                            position: "absolute",
                                            right: "-12px",
                                            top: "50%",
                                            transform: "translateY(-50%)",
                                        }}
                                    >
                                        *
                                    </span>
                                </div>

                                <div className="position-relative w-100">
                                <CustomCheck
                                    label="I agree with the"
                                    name="agree"
                                    value={formData.agree}
                                    onChange={(e) =>
                                        handleInputChange(
                                            e,
                                            setFormData,
                                            setFieldErrors,
                                            "check",
                                            "agree",
                                        )
                                    }
                                    error={fieldErrors.agree}
                                    disabled={isLoading}
                                    linkSection="Privacy Policy"
                                    />
                                    <span
                                        style={{
                                            color: "red",
                                            position: "absolute",
                                            right: "50px",
                                            top: "50%",
                                            transform: "translateY(-50%)",
                                        }}
                                    >
                                        *
                                    </span>
                                </div>

                                <div className="d-flex justify-content-start w-100">
                                    <CustomButton
                                        isLoading={isLoading}
                                        type={"submit"}
                                        icon={""}
                                        label={"Create Account"}
                                        disabled={isLoading}
                                        width={"100%"}
                                        backgroundColor={""}
                                        textColor={"var(--theme-font-color)"}
                                        shape={"roundedSquare"}
                                        borderStyle={""}
                                        borderColor={
                                            "1px solid var(--theme-font-color)"
                                        }
                                        iconStyle={{
                                            color: "var(--theme-font-color)",
                                        }}
                                        labelStyle={{
                                            fontSize: "16px",
                                            fontWeight: "400",
                                            fontFamily: "Georama",
                                            textTransform: "capitalize",
                                        }}
                                        hoverEffect={"theme"}
                                    />
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </HeroSection>
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

export default Signup;