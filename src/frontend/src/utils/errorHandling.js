/**
 * Custom error handling utility functions.
 */

/**
 * Detects if the error is related to CORS (Cross-Origin Resource Sharing).
 * @param {Error} error - The error object to check.
 * @returns {boolean} - True if it's a CORS-related error.
 */
export const isCorsError = (error) => {
    return (
        !error.response &&
        error.message &&
        (error.message.includes("CORS") ||
            error.message.includes("Network Error") ||
            error.message.includes("cross-origin") ||
            error.message.includes("request did not succeed"))
    );
};

/**
 * General error handler to handle different types of errors.
 * @param {Error} error - The error object to handle.
 * @param {Function} [setError] - Optional state setter to update the error state.
 * @param {Function} [showToast] - Optional function to display toast notifications.
 */
/**
 * Extracts error message from various possible response formats
 * @param {Object} responseData - The response data object from the error
 * @returns {string} - The extracted error message
 */
const extractErrorMessage = (responseData) => {
    if (!responseData) return null;

    // Errors you don't want to show
    const BLOCKED_ERRORS = ["No test center found!"];

    // Try multiple possible fields (camelCase, PascalCase, etc.)
    const possibleFields = [
        'message', 'Message',
        'error', 'Error',
        'errorMessage', 'ErrorMessage',
        'title', 'Title',
        'detail', 'Detail'
    ];

    for (const field of possibleFields) {
        if (responseData[field]) {
            if (BLOCKED_ERRORS.includes(responseData[field])) {
                return null;
            }
            return responseData[field];
        }
    }

    // Handle validation errors array (common in 400 Bad Request)
    if (responseData.errors && Array.isArray(responseData.errors) && responseData.errors.length > 0) {
        if (BLOCKED_ERRORS.includes(responseData.errors[0])) {
            return null;
        }
        // Return the first error message from the array
        return responseData.errors[0];
    }

    // Handle Errors array (PascalCase)
    if (responseData.Errors && Array.isArray(responseData.Errors) && responseData.Errors.length > 0) {
        if (BLOCKED_ERRORS.includes(responseData.Errors[0])) {
            return null;
        }
        return responseData.Errors[0];
    }

    // Handle validation error object (e.g., { field1: ['error1'], field2: ['error2'] })
    if (typeof responseData === "object") {
        const errorKeys = Object.keys(responseData);
        if (errorKeys.length > 0) {
            const firstKey = errorKeys[0];
            const firstValue = responseData[firstKey];

            let finalMessage = null;

            if (Array.isArray(firstValue) && firstValue.length > 0) {
                finalMessage = `${firstKey}: ${firstValue[0]}`;
            } else if (typeof firstValue === "string") {
                finalMessage = `${firstKey}: ${firstValue}`;
            }

            if (BLOCKED_ERRORS.includes(finalMessage)) {
                return null;
            }

            return finalMessage;
        }
    }

    return null;
};

export const handleGeneralError = (
    error,
    setError = null,
    showToast = null
) => {
    let errorMessage = "An unknown error occurred.";

    if (error.response) {
        // Server responded with a status code outside the 2xx range.
        switch (error.response.status) {
            case 400:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    error?.response?.statusText ||
                    "Bad Request. Please check your input.";
                break;
            case 401:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "Unauthorized. Please log in.";
                break;
            case 403:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "Forbidden. You do not have permission to perform this action.";
                break;
            case 404: {
                const extracted = extractErrorMessage(error?.response?.data);

                // If extractErrorMessage suppressed the text (e.g., "No test center found!")
                if (extracted === null) {
                    return; // DO NOT set state or show toast
                }

                errorMessage = extracted || "Not Found. The requested resource could not be found.";
                break;
            }
            case 409:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "The data already exists. Please try with different information.";
                break;
            case 500:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "Internal Server Error. Please try again later.";
                break;
            case 502:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "Bad Gateway. The server received an invalid response.";
                break;
            case 503:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "Service Unavailable. Please try again later.";
                break;
            case 504:
                errorMessage =
                    extractErrorMessage(error?.response?.data) ||
                    "Gateway Timeout. The server did not respond in time.";
                break;
            default:
                errorMessage =
                    extractErrorMessage(error.response?.data) ||
                    error.response?.statusText ||
                    "An error occurred while processing your request.";
        }
    } else if (isCorsError(error)) {
        // Request blocked due to cross-origin.
        errorMessage =
            "CORS error: Request blocked due to cross-origin restrictions. Please check your server settings.";
    } else if (error.request) {
        // No response received from the server.
        errorMessage = "Network error. Please check your internet connection.";
    } else {
        // Other errors (e.g., programming errors, validation errors).
        errorMessage = error.message;
    }

    // Optionally set the error message in a state.
    if (setError) {
        setError(errorMessage);
    }

    // Optionally display a toast notification.
    if (showToast) {
        showToast("error", errorMessage);
    }

    // Log error details in development mode for debugging
    if (process.env.NODE_ENV === 'development' && error?.response) {
        console.error("API Error Details:", {
            status: error.response.status,
            statusText: error.response.statusText,
            data: error.response.data,
            extractedMessage: errorMessage
        });
    }
};

/**
 * Error handler for API calls.
 * @param {Error} error - The error object to handle.
 * @param {Function} setError - State setter to update the error state.
 * @param {Function} showToast - Function to display toast notifications.
 * @param {Function} setLoggedIn - State setter to update login state.
 */
export const handleApiError = (error, setError, showToast, setLoggedIn) => {
    const unauthorizedErrorMessage = "Invalid token.";

    // Handle general errors.
    handleGeneralError(error, setError, showToast);

    // Additional handling for specific errors.
    if (error.response) {
        if (
            error.response.status === 200 &&
            error.response.data?.message === "Invalid OTP"
        ) {
            // Handle server verification error
            setError(error.response.data.message);
            showToast("error", error.response.data.message);
        } else if (
            error.response.status === 401 &&
            error.response.data?.error === unauthorizedErrorMessage
        ) {
            // Handle unauthorized access (invalid token).
            showToast("error", "Login Session expired. Please log in again.");
            setLoggedIn(false); // Log out the user.
        }
    }
};

/**
 * Error handler for form validation errors.
 * @param {Object} validationErrors - Object containing validation errors.
 * @param {Function} setFieldErrors - State setter to update field errors.
 * @param {Function} [showToast] - Optional function to display toast notifications.
 */
export const handleValidationError = (
    validationErrors,
    setFieldErrors,
    showToast = null
) => {
    // Update the form field errors state with the validation errors.
    setFieldErrors(validationErrors);

    // Optionally, display a toast notification for the first validation error.
    const firstErrorKey = Object.keys(validationErrors)[0];
    if (firstErrorKey) {
        const firstErrorMessage = validationErrors[firstErrorKey];
        if (showToast) {
            showToast("error", firstErrorMessage);
        }
    }
};
