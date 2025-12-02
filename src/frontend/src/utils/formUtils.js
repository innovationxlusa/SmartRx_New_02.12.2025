import { validateField } from "./validators";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import { FaCirclePlus } from "react-icons/fa6";

// Utility function to serialize form data into an object
export function serializeFormData(formElement) {
    const formData = new FormData(formElement);
    const serialized = {};
    formData.forEach((value, key) => {
        serialized[key] = value;
    });
    return serialized;
}

// Utility function to display an error message for a specific input element
export function displayFieldError(inputElement, errorMessage) {
    clearFieldError(inputElement); // Clear existing errors before adding a new one
    const errorElement = document.createElement("div");
    errorElement.className = "error-message";
    errorElement.textContent = errorMessage;

    const parent = inputElement.parentElement;
    parent.appendChild(errorElement);

    inputElement.classList.add("error-input"); // Highlight the error input
}

// Utility function to clear error messages from a specific input element
export function clearFieldError(inputElement) {
    const parent = inputElement.parentElement;
    const errorElement = parent.querySelector(".error-message");
    if (errorElement) {
        parent.removeChild(errorElement);
    }

    inputElement.classList.remove("error-input"); // Remove the error class
}

// Function to handle input change for various input types and set form data and errors
export function handleInputChange(eventOrValue, setFormData, setFieldErrors, inputType = "input", fieldName = null) {
    let name, value;

    if (inputType === "autocomplete" || inputType === "select") {
        name = fieldName;
        value = eventOrValue;
    } else if (inputType === "date") {
        name = fieldName;
        value = eventOrValue;
    } else if (inputType === "dateRange") {
        name = fieldName;
        value = {
            start: eventOrValue.start,
            end: eventOrValue.end,
        };
    } else if (eventOrValue.target) {
        name = eventOrValue.target.name;
        value = eventOrValue.target.value;
    } else {
        name = fieldName;
        value = eventOrValue;
    }

    setFormData((prevData) => ({
        ...prevData,
        [name]: value,
    }));

    const error = validateField(inputType, value, fieldName);
    setFieldErrors((prevErrors) => ({
        ...prevErrors,
        [name]: error,
    }));
}

// Function to reset form data and field errors
export const resetForm = (initialData, setFormData, setFieldErrors) => {
    setFormData(initialData);
    const initialErrors = {};
    Object.keys(initialData).forEach((key) => {
        initialErrors[key] = "";
    });
    setFieldErrors(initialErrors);
};

// Function to handle Modal Header Name based on the modal type dynamically
export const dynamicModalName = (name) => {
    const modalNames = {
        add: `Add ${name}`, // Label for the "add" modal type
        edit: `Edit ${name}`, // Label for the "edit" modal type
        delete: `Delete ${name}`, // Label for the "delete" modal type
    };

    return modalNames;
};

// Function to handle button labels based on the modal type dynamically
export const dynamicButtonLabel = (name) => {
    const buttonLabels = {
        add: `Create ${name}`, // Label for the "add" modal type
        edit: `Update ${name}`, // Label for the "edit" modal type
        delete: `Confirm`, // Label for the "delete" modal type
        rename:`Rename ${name}`,
        move: `Move ${name}`,
        tag:`Tag ${name}`,
    };

    return buttonLabels;
};

// Function to handle api service call based on the different modal types dynamically
export const dynamicActions = (url, formData, selected, apiServices) => {
    const { create, update, remove } = apiServices;

    const actions = {
        add: () => create(url, formData),
        edit: () => update(url, formData),
        delete: () => remove(url, selected.id),
        rename: () => update(url, formData),
        move: () => update(url, formData),
        tag: () => update(url, formData)
    };

    return actions;
};

// Object to show button icons based on the modal type
export const buttonIcons = {
    add: <FaCirclePlus />, // Label for the "add" modal type
    edit: <FaEdit />, // Label for the "edit" modal type
    delete: <FaTrashAlt />, // Label for the "delete" modal type
};
