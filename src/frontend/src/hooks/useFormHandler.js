import { BiRename } from "react-icons/bi";
import { FaCirclePlus } from "react-icons/fa6";
import { RiUploadCloudLine } from "react-icons/ri";
import { validateField } from "../utils/validators";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import { formatDate } from "../utils/dateTimeFormatterUtils";
// import moment from "moment";

/**
 * Custom hook to handle form operations including data serialization, error handling,
 * input change management, and dynamic modal and button handling.
 * @param {Object} initialData - Initial form data to be set as default.
 */
const useFormHandler = (initialData = {}) => {
    /**
     * Utility function to serialize form data into an object.
     * Extracts key-value pairs from form input elements and returns them in object format.
     * @param {HTMLFormElement} formElement - Form element to be serialized.
     * @returns {Object} - Serialized form data as key-value pairs.
     */
    const serializeFormData = (formElement) => {
        const formData = new FormData(formElement);
        const serialized = {};
        formData.forEach((value, key) => {
            serialized[key] = value;
        });
        return serialized;
    };

    /**
     * Displays an error message for a specific input element.
     * If the input element has a validation error, this will display it below the input field.
     * @param {HTMLElement} inputElement - The input element where the error occurred.
     * @param {string} errorMessage - The error message to display.
     */
    const displayFieldError = (inputElement, errorMessage) => {
        clearFieldError(inputElement); // Clear existing errors before adding a new one
        const errorElement = document.createElement("div");
        errorElement.className = "error-message";
        errorElement.textContent = errorMessage;

        const parent = inputElement.parentElement;
        parent.appendChild(errorElement);

        inputElement.classList.add("error-input"); // Highlight the error input
    };

    /**
     * Clears the error message from a specific input element.
     * This is called before adding new error messages to avoid duplication.
     * @param {HTMLElement} inputElement - The input element to clear the error from.
     */
    const clearFieldError = (inputElement) => {
        const parent = inputElement.parentElement;
        const errorElement = parent.querySelector(".error-message");
        if (errorElement) {
            parent.removeChild(errorElement);
        }
        inputElement.classList.remove("error-input"); // Remove the error highlight
    };

    /**
     * Formats a phone number by adding '880' as a prefix if missing, and removing any digits
     * before the first occurrence of '1'.
     * @param {string} phoneNumber - Phone number to be formatted.
     * @returns {string} - Formatted phone number with '880' prefix.
     */
    const formatPhoneNumber = (phoneNumber) => {
        const trimmedValue = phoneNumber.trim();
        const firstOneIndex = trimmedValue.indexOf("1");
        let slicedValue = firstOneIndex !== -1 ? trimmedValue.slice(firstOneIndex) : trimmedValue;

        // Remove existing '880' prefix if present
        if (slicedValue.startsWith("880")) {
            slicedValue = slicedValue.slice(3);
        }

        // Ensure the result starts with '880'
        return slicedValue ? `880${slicedValue}` : "";
    };

    /**
     * Handles input changes for different input types including text, autocomplete, checkboxes, etc.
     * It updates the form data and validates the field, setting errors if any.
     * @param {Object|Event} eventOrValue - Event object or value based on the input type.
     * @param {Function} setFormData - Function to update form data.
     * @param {Function} setFieldErrors - Function to update field error state.
     * @param {string} inputType - Type of input (e.g., 'input', 'date', 'check', etc.).
     * @param {string|null} fieldName - The name of the field being changed.
     * @param {string} id - The ID used for checkboxes to uniquely identify roles.
     */
    const handleInputChange = (eventOrValue, setFormData, setFieldErrors, inputType = "input", fieldName = null, id = "") => {
        let name, value;

        // Handle different input types
        if (inputType === "autocomplete" || inputType === "select") {
            name = fieldName;
            value = inputType === "select" ? eventOrValue.target.value : eventOrValue;
        } else if (inputType === "date" || inputType === "datetime-local") {
            name = fieldName;
            value = eventOrValue.target.value;
        } else if (inputType === "singleFile") {
            name = fieldName;
            value = eventOrValue.target.files[0];
        } else if (inputType === "multipleFile") {
            name = fieldName;
            value = Array.from(eventOrValue.target.files); // Handle multiple files;
        } else if (inputType === "dateRange") {
            name = fieldName;
            value = {
                // Placeholder for actual range logic
            };
        } else if (inputType === "check") {
            name = fieldName;
            value = eventOrValue.target.checked;
        } else if (inputType === "checkboxes") {
            // Handle multi-checkbox arrays
            name = fieldName;
            setFormData((prevData) => {
                const updatedItems = prevData[name].includes(id) ? prevData[name].filter((item) => item !== id) : [...prevData[name], id];

                return {
                    ...prevData,
                    [name]: updatedItems,
                };
            });

            return; // Skip validation for checkboxes
        } else if (eventOrValue.target) {
            // Fallback for standard inputs (text, number, password, etc.)
            name = eventOrValue.target.name;
            value = eventOrValue.target.value;
        } else {
            // Direct value assignment (edge case)
            name = fieldName;
            value = eventOrValue;
        }

        // Update form data
        setFormData((prevData) => {
            const updatedData = {
                ...prevData,
                [name]: value,
            };

            // ✅ Special validation case: Confirm Password needs to match original password
            if (name === "confirmPassword") {
                const error = validateField(
                    "confirmPassword",
                    {
                        password: updatedData.password,
                        confirmPassword: value,
                    },
                    "Confirm password"
                );

                setFieldErrors((prevErrors) => ({
                    ...prevErrors,
                    [name]: error,
                }));
            } else {
                // Standard field validation
                const error = validateField(inputType, value, fieldName);

                setFieldErrors((prevErrors) => ({
                    ...prevErrors,
                    [name]: error,
                }));
            }

            return updatedData;
        });
    };

    /**
     * Resets form data and field errors to their initial values.
     * @param {Object} initialData - Initial form data to reset to.
     * @param {Function} setFormData - Function to update form data state.
     * @param {Function} setFieldErrors - Function to update field error state.
     */
    const resetForm = (initialData, setFormData, setFieldErrors) => {
        setFormData(initialData);
        const initialErrors = {};
        Object.keys(initialData).forEach((key) => {
            initialErrors[key] = ""; // Reset errors to empty strings
        });
        setFieldErrors(initialErrors);
    };

    /**
     * Dynamically generates modal header name based on the modal type.
     * @param {string} name - Name of the entity being added, edited, or deleted.
     * @returns {Object} - Object with modal names for different actions (add, edit, delete).
     */
    const dynamicModalName = (name) => {
        const modalNames = {
            add: `Add ${name}`, // Label for the "add" modal type
            edit: `Edit ${name}`, // Label for the "edit" modal type
            delete: `Delete ${name}`, // Label for the "delete" modal type
            view: `View Details of ${name}`,
            rename: `Rename ${name}`,
            move: `Move ${name}`,
            tag: `Tag ${name}`,
            upload: `Upload ${name}`,
            testinformation: `Test Information`,
        };
        return modalNames;
    };

    /**
     * Dynamically generates button labels based on modal type.
     * @param {string} name - Name of the entity for the action button.
     * @returns {Object} - Object with button labels for different actions (add, edit, delete).
     */
    const dynamicButtonLabel = (name) => {
        return {
            add: `Create ${name}`,
            edit: `Update ${name}`,
            delete: `Confirm`,
            rename: `Rename ${name}`,
            move: `Move ${name}`,
            tag: `Tag ${name}`,
            upload: `Upload ${name}`,
            testinformation: `Test Information`,
        };
    };

    /**
     * Dynamically handles API actions based on the modal type.
     * @param {string} url - API endpoint URL.
     * @param {Object} formData - Form data to be sent.
     * @param {Object} selected - Object containing selected entity data (for editing or deleting).
     * @param {Object} apiServices - Object containing API service functions (create, update, remove).
     * @param {string} topic - Name of the entity for success messages.
     * @returns {Object} - Object with functions for performing create, edit, and delete actions.
     */
    const dynamicActions = (url, formData, selectedId, apiServices, topic) => {
        const { create, update, remove } = apiServices;

        return {
            add: () => create(url, formData, topic),
            edit: () => update(url, formData, topic),
            delete: () => remove(url, selectedId, topic, formData),
            rename: () => update(`${url}/${selectedId}`, formData, topic),
            tag: () => update(url, formData, topic),
            move: () => update(url, formData, topic),
            faq: () => getById(url, formData, topic),
        };
    };

    /**
     * Returns success messages based on HTTP methods.
     * @param {string} topic - The entity being added, updated, or deleted.
     * @returns {Object} - Object containing success messages for different actions.
     */
    const successMessages = (topic) => {
        const messages = {
            POST: `${topic} successfully`,
            PUT: `${topic} successfully`,
            DELETE: `${topic} successfully`,
        };

        return messages;
    };

    /**
     * Provides corresponding icons for the modal buttons based on the action type.
     */
    const topicMessage = {
        add: "Added", // Label for the "add" modal type
        edit: "Updated", // Label for the "edit" modal type
        delete: "Deleted", // Label for the "delete" modal type
        move: "Moved",
        tag: "Tagged",
        rename: "Renamed",
    };

    // Object to show button icons based on the modal type
    const buttonIcons = {
        add: <FaCirclePlus />, // Icon for "add" action
        edit: <FaEdit />, // Icon for "edit" action
        rename: <BiRename />, // Icon for "edit" action
        delete: <FaTrashAlt />, // Icon for "delete" action
        upload: <RiUploadCloudLine />, // Icon for "delete" action
    };
    function toPascalCase(text) {
        if (typeof text !== "string") return "";
        return text.replace(/(\w)(\w*)/g, (_, first, rest) => first.toUpperCase() + rest.toLowerCase());
    }
    function toPascalCaseRemovingSpace(text) {
        if (typeof text !== "string") return "";
        return text.replace(/(\w)(\w*)/g, (_, first, rest) => first.toUpperCase() + rest.toLowerCase()).replace(/\s+/g, "");
    }

    return {
        serializeFormData,
        displayFieldError,
        clearFieldError,
        handleInputChange,
        resetForm,
        dynamicModalName,
        dynamicButtonLabel,
        dynamicActions,
        successMessages,
        topicMessage,
        buttonIcons,
        formatPhoneNumber,
        toPascalCase,
        toPascalCaseRemovingSpace,
    };
};

export default useFormHandler;
