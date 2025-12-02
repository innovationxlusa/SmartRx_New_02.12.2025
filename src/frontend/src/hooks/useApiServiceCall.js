import { useEffect, useState, useRef } from "react";
import axios from "axios";
import axiosClient from "../services/axiosClient";
import { jwtDecode } from "jwt-decode"; // JWT library to decode tokens
import useFormHandler from "./useFormHandler"; // Custom hook to handle form operations like showing success messages
import useToastMessage from "./useToastMessage"; // Custom hook for displaying toast notifications
import useNetworkStatus from "./useNetworkStatus";
import useSmartNavigate from "./useSmartNavigate"; // Custom hook to handle navigation
import { useErrorHandler } from "./useErrorHandler"; // Custom hook to handle all kind of error in one place
import { useUserContext } from "../contexts/UserContext"; // Import UserContext to access user-related data
import { useAuthContext } from "../contexts/AuthContext"; // Import AuthContext for token management
import { BASE_URL, REFRESH_TOKEN_URL } from "../constants/apiEndpoints"; // Base URL for your API

let logoutInitiated = false; // Track whether the logout has been initiated

/**
 * Custom hook to handle API service calls with robust error handling, token management, and loading state.
 * @returns {object} - Object containing executeApiCall method, loading state, and error state.
 */
const useApiServiceCall = () => {
    const { setIsLoggedIn, user, setUser } = useUserContext(); // Access user context to check login status
    const { accessToken, refreshAccessToken: refreshTokenFromContext, clearTokens, forceLogout } = useAuthContext(); // Get tokens from AuthContext
    // For handling redirection
    const { smartNavigate } = useSmartNavigate({ scroll: "top" });

    // State variables to manage loading and error states
    const [loading, setLoading] = useState(false);
    const loadingRef = useRef(false);
    const [error, setError] = useState(null);

    // Custom hooks for handling errors and displaying success messages
    const { handleApiError } = useErrorHandler(); // Handle various API errors
    const { successMessages } = useFormHandler(); // Handle success messages for form actions
    // const { isOnline } = useNetworkStatus();
    const showToast = useToastMessage(); // Toast notification hook for user feedback

    // Inside your hook:
    const hasShownExpiryMessage = useRef(false); // Flag to ensure message shows once

    // Note: Token refresh is now handled by AuthContext automatically
    // This effect is kept for any additional logic but refresh happens in AuthContext
    useEffect(() => {
        if (!accessToken) return;
        // Token refresh is handled by AuthContext's auto-refresh mechanism
    }, [accessToken]);

    /**
     * Helper function to refresh the access token when it expires.
     * Now uses AuthContext's refresh method.
     * @returns {Promise<string|null>} - The new access token if refresh is successful, otherwise null.
     */
    const refreshAccessToken = async () => {
        // Use AuthContext's refresh method which handles all token state updates
        try {
            return await refreshTokenFromContext();
        } catch (err) {
            if (err.response?.status === 401 || err.response?.status === 400) {
                const message = "Session has expired. Please log in again to continue.";
                forceLogout(message);
            } else {
                handleApiError(err, setError); // Handle refresh token error
            }
            throw err;
        }
    };

    /**
     * Executes an API call with optional authentication, payload, and additional configurations.
     * Automatically handles token refresh on 401 errors and retries the API call.
     * @param {string} endpoint - The API endpoint to call.
     * @param {string} method - The HTTP method (GET, POST, PUT, DELETE).
     * @param {boolean} authentication - Whether the API call requires authentication.
     * @param {object|null} [payload=null] - The request payload for POST/PUT requests.
     * @param {string} [topic=""] - The action/topic for toast notifications.
     * @param {boolean} [isFileUpload=false] - Whether the request involves file upload.
     * @param {boolean} [isFileDownload=false] - Whether the request involves file download.
     * @param {string} [fileName=""] - Optional filename for downloaded file.
     * @param {function|null} [onUploadProgress=null] - Callback for upload progress.
     * @param {function|null} [onDownloadProgress=null] - Callback for download progress.
     * @param {AbortSignal|null} [signal=null] - Signal to abort the request.
     * @returns {Promise<object>} - The API response object containing message and response data.
     */
    const executeApiCall = async (
        endpoint,
        method,
        authentication,
        payload = null,
        topic = "",
        isFileUpload = false,
        isFileDownload = false,
        onUploadProgress = null,
        onDownloadProgress = null,
        signal = null,
        viewFile = false,
    ) => {
        if (!loadingRef.current) {
            loadingRef.current = true;
            setLoading(true); // Start loading state
        }
        setError(null); // Clear previous errors


        const axiosInstance = axiosClient;
        try {
            // Get access token and check expiration
            let token = accessToken;
            const decodedToken = token && jwtDecode(token);
            const currentTime = Date.now() / 1000; // Current time in seconds

            // Check if the token is expired
            const isTokenExpired = decodedToken?.exp < currentTime;

            // Refresh the token if it is expired
            if (authentication && isTokenExpired) {
                // If token is expired or not available, refresh the token
                token = await refreshAccessToken(); // Await the refresh token call
                if (!token)
                    throw new Error(
                        "Failed to refresh token. Unable to proceed.",
                    );
            }

            // Configure the request with the new or existing access token
            const isFormData = payload instanceof FormData;

            const requestConfig = {
                url: endpoint,
                method,
                withAuth: Boolean(authentication),
                headers: {
                    // Content-Type below is set per payload rules; Authorization handled by interceptor when withAuth=true
                    ...(isFormData && {
                        "Content-Type": "multipart/form-data",
                    }),
                    ...(isFileUpload && {
                        "Content-Type": "multipart/form-data",
                    }), // Handle file uploads
                    ...(!isFormData && !isFileUpload && payload != null && {
                        "Content-Type": "application/json",
                    }), // Default JSON for non-form payloads
                },
                ...(payload && { data: payload }), // Add payload for POST or PUT requests
                ...(onUploadProgress && { onUploadProgress }), // Add upload progress handler if provided
                ...(onDownloadProgress && { onDownloadProgress }), // Add download progress handler
                responseType: isFileDownload || viewFile ? "blob" : "json", // Handle file downloads as blob
                signal, // Include the signal for request cancellation
            };
     
            // Make the API call
            let response = await axiosInstance(requestConfig);
            const credentialErrorMessage = "Incorrect Username or Password!";

            if (credentialErrorMessage === response?.data?.message?.trim())
                throw new Error(credentialErrorMessage);

            if (
                !response?.data?.accessToken &&
                !response?.data?.refreshToken &&
                response?.data?.functionCodes === null
            ) {
                const showErrorMessage = response?.data?.message;
                showToast("error", showErrorMessage); // Show success message
            }

            // Handle file download if necessary
            if (isFileDownload) {
                const fileName = response.headers["content-disposition"]
                    ?.split("filename=")[1]
                    ?.replace(/"/g, "");
                const url = window.URL.createObjectURL(
                    new Blob([response.data]),
                );
                const link = document.createElement("a");
                link.href = url;
                link.setAttribute("download", fileName || "file.zip");
                document.body.appendChild(link);
                link.click();
                link.remove();
            }

            // // Show success message for POST/PUT/DELETE if necessary
            // if (topic && (response?.data?.data || response?.data?.status === true) && ["POST", "PUT", "DELETE"].includes(method)) {
            //     const showMessage = successMessages(topic);
            //     showToast("success", showMessage[method]); // Show success message
            // }

            // Show success message for POST/PUT/DELETE if necessary
            if (
                (response?.data?.data ||
                    response?.data?.status === true ||
                    response?.data) &&
                !response?.data?.data?.accessToken &&
                !response?.data?.data?.userCode &&
                ["GET", "POST", "PUT", "DELETE"].includes(method)
            ) {
                const serverMsg =
                    response?.data?.message?.trim() || response?.message;
                const topicMsg = topic && successMessages(topic)?.[method];

                // Define substrings of endpoints where you want to skip the toast
                const skipToastEndpoints = [
                    "getallparentfoldersandfiles",
                    "getsmartrxinsiderbyid",
                    "GetAllFolders",
                    "medicine-list-to-compare",
                    "getvitalsbyvitalname",
                    "medicine-faq-list",
                    "investigation-list-to-compare",
                    "investigation-list-selected-or-recommended",
                    "investigation-testcenters",
                    "download",
                    "investigation-faq-list",
                    "vital-faq-list",
                    "GetDoctorDetialsById",
                    "GetPatientDropdown",
                    "getpatientprescriptions",
                    "GetAllPatientProfilesByUserId",
                    "GetPatientRewardsSummary",
                    "GetAllRewards",
                    "GetAllRewardBadges",
                    "dashboard-counts",
                    "GetAllSmartRxWithVitalsByUserId",
                    "GetDoctorProfilesByUserId",
                    "dashboard-summary",
                    "GetPatientDetialsById",
                    "GetPatientRewardsByUserIdAndPatientId",
                    "getallparentfoldersandfiles",
                    "RewardSummary",
                    "RewardDetails",
                    
                ];

                // Check if current endpoint includes any of the skip keywords
                const shouldSkipToast = skipToastEndpoints.some((key) =>
                    endpoint.toLowerCase().includes(key.toLowerCase()),
                );
                if (!shouldSkipToast) {
                    showToast(
                        "success",
                        serverMsg ?? topicMsg ?? "Action successful.",
                    );
                }
            }

            if (
                !response?.data?.status &&
                response?.data?.data === null &&
                response?.data?.message
            ) {
                const showErrorMessage = response?.data?.message;
                showToast("error", showErrorMessage); // Show success message
            }

            return {
                message: "Successful", // your custom message
                serverMessage: response?.message, // message from server (if any)
                response: response?.data?.accessToken
                    ? response.data
                    : response.data?.data
                      ? response.data?.data
                      : response.data,
            };
        } catch (err) {
            if (authentication && err?.response?.status === 401) {
                const message = "Session has expired. Please log in again to continue.";
                forceLogout(message);
                return;
            }
            handleApiError(err, setError);
        } finally {
            if (loadingRef.current) {
                loadingRef.current = false;
                setLoading(false); // Stop loading state
            }
        }
    };

    /**
     * Function to log the user out and clear stored tokens, permissions, and user data.
     * @returns {Promise} - Resolves with a status and message indicating the outcome.
     */
    const logout = async (userMessage = "") => {
        if (logoutInitiated) return; // Prevent multiple logout calls
        logoutInitiated = true; // Set the flag to indicate that logout is in progress
        
        if (!accessToken) {
            showToast("error", "User is already logged out.", "⚠️");
            return;
        }

        const message = `Bye ${user?.FirstName} ${user?.LastName}! See you soon.`;
        clearTokens(); // Clear all tokens via AuthContext

        setTimeout(() => {
            setIsLoggedIn(false); // Update user login status
            setUser(null); // Clear user data
            smartNavigate("/"); // Redirect to login page
            userMessage
                ? showToast("error", userMessage, "⏰")
                : showToast("success", message, "👋");
        }, 0);
    };

    useEffect(() => {
        return () => {
            logoutInitiated = false; // Reset the flag when the component unmounts
        };
    }, []);

    return { executeApiCall, loading, error, logout }; // Return the executeApiCall, loading state, error, and logout functions
};

export default useApiServiceCall;
