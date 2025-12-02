import React, { useState, useEffect, useRef } from "react";
import "./Login.css";
import { jwtDecode } from "jwt-decode";
import { Link, useLocation } from "react-router-dom";
import { FcGoogle } from "react-icons/fc";
import { FaMobileAlt } from "react-icons/fa";
import { TbLockPassword } from "react-icons/tb";
import HeroSection from "../HeroSection/HeroSection";
import { validateField, validatePhoneNumber } from "../../utils/validators";
import CustomCheck from "../static/Commons/CustomCheck";
import useFormHandler from "../../hooks/useFormHandler";
import CustomInput from "../static/Commons/CustomInput";
import useToastMessage from "../../hooks/useToastMessage";
import CustomButton from "../static/Commons/CustomButton";
import useAuthService from "../../services/useAuthService";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import { useUserContext } from "../../contexts/UserContext";
import { useAuthContext } from "../../contexts/AuthContext";
import { tokenStore } from "../../contexts/AuthContext";
import { LOGIN_URL, OTP_VERIFICATION_URL } from "../../constants/apiEndpoints";
import axios from "axios";
import { BASE_URL } from "../../constants/apiEndpoints";

const REMEMBER_ME_KEY = "smartrx-remembered-phone";
const hasLocalStorage =
    typeof window !== "undefined" && typeof window.localStorage !== "undefined";

const getRememberedPhone = () => {
    if (!hasLocalStorage) return null;
    try {
        return window.localStorage.getItem(REMEMBER_ME_KEY);
    } catch (storageError) {
        console.warn("Unable to read remembered phone:", storageError);
        return null;
    }
};

const persistRememberedPhone = (value) => {
    if (!hasLocalStorage) return;
    try {
        if (value) {
            window.localStorage.setItem(REMEMBER_ME_KEY, value);
        } else {
            window.localStorage.removeItem(REMEMBER_ME_KEY);
        }
    } catch (storageError) {
        console.warn("Unable to persist remembered phone:", storageError);
    }
};

const Login = () => {
    const showToast = useToastMessage();
    const { decodeToken, setIntendedRoute } = useUserContext();
    const { signIn, verifyOtp } = useAuthService();
    const { setTokens } = useAuthContext(); // Use AuthContext for token management
    const { handleInputChange, togglePasswordVisibility } = useFormHandler();
    const { smartNavigate, navigateBack, navigateForward } = useSmartNavigate({
        scroll: "top",
    });
    const location = useLocation();

    const [showPassword, setShowPassword] = useState(false);

    const initialData = {
        Otp: "",
        UserId: "",
        AuthType: 2,
        UserName: "",
        CountryCode: "",
        Password: "",
        rememberMe: false,
    };

    const [isOtpSent, setIsOtpSent] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [formData, setFormData] = useState(() => {
        const savedPhone = getRememberedPhone();
        return {
            ...initialData,
            UserName: savedPhone || initialData.UserName,
            rememberMe: Boolean(savedPhone),
        };
    });
    const [fieldErrors, setFieldErrors] = useState(initialData);
    // Temporary token for OTP verification (not stored in AuthContext)
    const [tempAccessToken, setTempAccessToken] = useState(null);
    const [tempRefreshToken, setTempRefreshToken] = useState(null);

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
                // Gracefully handle unauthorized country API in login screen
                if (error?.response?.status !== 401) {
                    console.error("Error fetching country codes:", error);
                    setCountryError(error);
                }
                setCountryData([]);
            } finally {
                setIsCountryLoading(false);
            }
        };

        fetchCountryCodes();
    }, []);

    useEffect(() => {
        if (!hasLocalStorage) return;

        if (formData.rememberMe && formData.UserName) {
            persistRememberedPhone(formData.UserName);
        } else if (!formData.rememberMe || !formData.UserName) {
            persistRememberedPhone(null);
        }
    }, [formData.rememberMe, formData.UserName]);

    const [isGoogleLoading, setIsGoogleLoading] = useState(false);
    const [userPrimaryFolder, setUserPrimaryFolder] = useState({
        id: "",
        folderName: "",
        description: "",
        folderHierarchy: "",
        parentFolderId: "",
        patientProfileId: "",
        userId: "",
        createdDate: "",
        createdById: "",
    });

    const handleOtpSend = async (e) => {
        e.preventDefault();
        const fieldsToValidate = {
            UserName: validatePhoneNumber(
                formData.UserName,
                "Mobile number",
                formData.CountryCode,
            ),
            CountryCode: validateField(
                "CountryCode",
                formData.CountryCode,
                "Country Code",
            ),
        };

        if (Object.values(fieldsToValidate).some((error) => error)) {
            setFieldErrors(fieldsToValidate);
            return;
        }

        try {
            setIsLoading(true);
            const countryCode = formData.CountryCode.replace("+", "");
            const mobileNumber = formData.UserName;

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

            const loginData = {
                ...formData,
                UserName: mobileForApi,
            };

            const response = await signIn(LOGIN_URL, loginData);
            if (response?.message === "Successful") {
                const raw = response.response || {};
                const access = raw.accessToken || raw.AccessToken;
                const refresh = raw.refreshToken || raw.RefreshToken;
                const decodedToken = jwtDecode(access);
                if (decodedToken.sub === "AUTHENTICATION") {
                    // Store tokens temporarily (not in AuthContext) for verifyOtp call only
                    setTempAccessToken(access);
                    setTempRefreshToken(refresh);
                    setIsOtpSent(true); // Show password/OTP input field
                    setFormData((prev) => ({
                        ...prev,
                        UserId: response.response.userId,
                    }));
                    showToast(
                        "success",
                        "OTP sent to your mobile number",
                        "👋",
                    );
                } else {
                    showToast("error", "User not found", "");
                }
            }
        } catch (error) {
            showToast("error", "Failed to send OTP. Please try again.", "⚠️");
        } finally {
            setIsLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const fieldsToValidate = {
            UserName: validateField(
                "UserName",
                formData.UserName,
                "Mobile Number",
            ),
            Password: validateField(
                "Password",
                formData.Password,
                "Password",
                true,
            ),
        };

        if (Object.values(fieldsToValidate).some((error) => error)) {
            setFieldErrors(fieldsToValidate);
            return;
        }

        try {
            setIsLoading(true);

            // Temporarily set token in tokenStore for verifyOtp API call
            const originalAccessToken = tokenStore.accessToken;
            const originalRefreshToken = tokenStore.refreshToken;
            
            if (tempAccessToken) {
                tokenStore.accessToken = tempAccessToken;
                tokenStore.refreshToken = tempRefreshToken;
            }

            const newData = {
                ...formData,
                Otp: formData.Password,
            };

            try {
                const response = await verifyOtp(OTP_VERIFICATION_URL, newData);
                
                // Restore original tokens (or clear if they were null)
                tokenStore.accessToken = originalAccessToken;
                tokenStore.refreshToken = originalRefreshToken;

                if (
                    response?.message === "Successful" &&
                    (response.response?.accessToken || response.response?.AccessToken)
                ) {
                    const raw = response.response || {};
                    const access = raw.accessToken || raw.AccessToken;
                    const refresh = raw.refreshToken || raw.RefreshToken;
                    const decodedToken = jwtDecode(access);
                    const folder = response.response.userPrimaryFolder;
                    setUserPrimaryFolder(response.response.userPrimaryFolder);
                    
                    if (decodedToken.sub === "AUTHENTICATION") {
                        const message = `Welcome back ${decodedToken.FirstName || "User"} ${decodedToken.LastName || ""}!`;
                        decodeToken(access);
                        setTokens(access, refresh, decodedToken.role); // Store tokens via AuthContext (full login)

                        // Clear temporary tokens
                        setTempAccessToken(null);
                        setTempRefreshToken(null);

                        const hasFilesRaw = response?.response?.isExistAnyFile;
                        const hasFiles = (() => {
                            if (typeof hasFilesRaw === "boolean") {
                                return hasFilesRaw;
                            }
                            if (typeof hasFilesRaw === "number") {
                                return hasFilesRaw === 1;
                            }
                            if (typeof hasFilesRaw === "string") {
                                return /^(true|1)$/i.test(hasFilesRaw.trim());
                            }
                            return false;
                        })();

                        const routeFromState =
                            typeof location?.state?.redirectTo === "string" &&
                            location.state.redirectTo.length > 0
                                ? location.state.redirectTo
                                : "";
                        const fallbackRoute = hasFiles ? "/all-patient" : "/dashboard";
                        const targetRoute = routeFromState || fallbackRoute;
                        setIntendedRoute(targetRoute);
                        smartNavigate(targetRoute, { replace: true });

                        setTimeout(() => showToast("success", message, "👋"), 100);
                    } else {                    
                        smartNavigate("/signIn");
                    }
                }
            } catch (verifyError) {
                // Restore original tokens on error
                tokenStore.accessToken = originalAccessToken;
                tokenStore.refreshToken = originalRefreshToken;
                throw verifyError;
            }
        } catch (error) {
            console.error("Error verifying OTP:", error);
            showToast("error", "Invalid OTP. Please try again.", "⚠️");
            // Clear temporary tokens on error
            setTempAccessToken(null);
            setTempRefreshToken(null);
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
            >
                <div className="signin-container">
                    <div className="signin-header">
                        <h4>Sign In</h4>
                    </div>
                    <div className="signin-box">
                        <form
                            onSubmit={isOtpSent ? handleSubmit : handleOtpSend}
                        >
                            <div className="signin-body">
                                <CustomInput
                                    className="input-style"
                                    name="UserName"
                                    type="tel"
                                    placeholder="Mobile No."
                                    value={formData.UserName}
                                    onChange={(e) => {
                                        const value = e.target.value;
                                        setFormData((prev) => ({
                                            ...prev,
                                            UserName: value,
                                        }));
                                        const error = validatePhoneNumber(
                                            value,
                                            "Mobile number",
                                            formData.CountryCode,
                                        );
                                        setFieldErrors((prev) => ({
                                            ...prev,
                                            UserName: error,
                                        }));
                                    }}
                                    error={fieldErrors.UserName}
                                    disabled={isLoading}
                                    icon={<FaMobileAlt />}
                                    iconPosition="left"
                                    minHeight="0px"
                                    // Country code portion
                                    showCountryCode={true}
                                    countryCode={formData.CountryCode}
                                    onCountryCodeChange={(e) => {
                                        const newCountryCode = e.target.value;
                                        setFormData((prev) => ({
                                            ...prev,
                                            CountryCode: newCountryCode,
                                        }));

                                        if (formData.UserName) {
                                            const error = validatePhoneNumber(
                                                formData.UserName,
                                                "Mobile number",
                                                newCountryCode,
                                            );
                                            setFieldErrors((prev) => ({
                                                ...prev,
                                                UserName: error,
                                            }));
                                        }
                                    }}
                                    countryOptions={
                                        CountryData.length > 0
                                            ? CountryData.map((country) => ({
                                                  label: country.countryCode,
                                                  value: country.countryCode,
                                              }))
                                            : [{ label: "+880", value: "+880" }]
                                    }
                                />
                                {isOtpSent && (
                                    <>
                                        <CustomInput
                                            className="input-style"
                                            name="Password"
                                            type="password"
                                            placeholder="Pin"
                                            value={formData.Password}
                                            onChange={(e) =>
                                                handleInputChange(
                                                    e,
                                                    setFormData,
                                                    setFieldErrors,
                                                    "input",
                                                    "Password",
                                                )
                                            }
                                            error={fieldErrors.Password}
                                            disabled={isLoading}
                                            icon={<TbLockPassword />}
                                            iconPosition="left"
                                            minHeight="0px"
                                        />
                                        <CustomCheck
                                            label="Remember Me"
                                            name="rememberMe"
                                            value={formData.rememberMe}
                                            onChange={(e) =>
                                                handleInputChange(
                                                    e,
                                                    setFormData,
                                                    setFieldErrors,
                                                    "check",
                                                    "rememberMe",
                                                )
                                            }
                                            disabled={isLoading}
                                        />
                                    </>
                                )}

                                <div className="d-flex justify-content-start w-100">
                                    <CustomButton
                                        isLoading={isLoading}
                                        type="submit"
                                        label={
                                            isOtpSent ? "Verify PIN" : "Sign In"
                                        }
                                        disabled={isLoading}
                                        width="100%"
                                        textColor="var(--theme-font-color)"
                                        shape="roundedSquare"
                                        borderColor="1px solid var(--theme-font-color)"
                                        labelStyle={{
                                            fontSize: "16px",
                                            fontWeight: "400",
                                            fontFamily: "Georama",
                                            textTransform: "capitalize",
                                        }}
                                        hoverEffect="theme"
                                    />
                                </div>

                                {!isOtpSent && (
                                    <>
                                        <div className="or-divider">
                                            <span className="divider-line"></span>
                                            <span className="text">or</span>
                                            <span className="divider-line"></span>
                                        </div>
                                        <div className="d-flex justify-content-center w-100">
                                            <CustomButton
                                                isLoading={isGoogleLoading}
                                                type="button"
                                                icon={<FcGoogle size={20} />}
                                                label="Google Sign-in"
                                                disabled={
                                                    isGoogleLoading || isLoading
                                                }
                                                width="100%"
                                                textColor="var(--theme-font-color)"
                                                shape="roundedSquare"
                                                borderColor="1px solid var(--theme-font-color)"
                                                labelStyle={{
                                                    fontSize: "16px",
                                                    fontWeight: "400",
                                                    fontFamily: "Georama",
                                                    textTransform: "capitalize",
                                                }}
                                                hoverEffect="theme"
                                            />
                                        </div>
                                    </>
                                )}
                            </div>
                        </form>

                        <div className="signin-footer">
                            {isOtpSent ? (
                                <p>
                                    Didn't get your verification code?{" "}
                                    <Link to="#" className="link">
                                        Click here
                                    </Link>{" "}
                                    to resend.
                                </p>
                            ) : (
                                <>
                                    <div
                                        className="forgot-password"
                                        style={{ fontFamily: "Georama" }}
                                    >
                                        <Link to="#">Forgot password?</Link>
                                    </div>
                                    <div
                                        className="forgot-password"
                                        style={{ fontFamily: "Georama" }}
                                    >
                                        <Link to="/signUp">
                                            Create new account
                                        </Link>
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            </HeroSection>
        </div>
    );
};

export default Login;