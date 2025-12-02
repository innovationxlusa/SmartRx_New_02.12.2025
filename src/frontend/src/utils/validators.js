const validateEmail = (email) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email) {
        return "Email is required";
    } else if (!emailRegex.test(email)) {
        return "Email is not valid";
    }
    return "";
};

const validatePassword = (password, login) => {
    if (!password) {
        return "Password is required.";
    }

    const errors = [];

    if (!login && password.length < 8) {
        errors.push("Password must be at least 8 characters long.");
    }
    if (!login && !/[a-z]/.test(password)) {
        errors.push("Password must contain at least one lowercase letter.");
    }
    if (!login && !/[A-Z]/.test(password)) {
        errors.push("Password must contain at least one uppercase letter.");
    }
    if (!login && !/\d/.test(password)) {
        errors.push("Password must contain at least one digit.");
    }
    if (!login && !/[@*()_+\-=[\]{};':"\\|,.<>/?]/.test(password)) {
        errors.push("Password must contain at least one special character.");
    }

    return errors.length > 0 ? errors.join("\r\n") : "";
};

const validateConfirmPassword = ({ password, confirmPassword }, fieldName) => {
    if (!confirmPassword) return `${fieldName} is required`;
    if (password !== confirmPassword)
        return `${fieldName} does not match password`;
    return "";
};

const validateText = (text, fieldName) => {
    if (!text) {
        return `${fieldName?.charAt(0).toUpperCase() + fieldName?.slice(1)} is required`;
    }
    return "";
};

const validatePrivacyPolicy = (text, fieldName) => {
    if (!text) {
        return `${fieldName}`;
    }
    return "";
};

const validateNumber = (number, fieldName) => {
    const numberRegex = /^\d*$/;
    if (!number) {
        return `${fieldName?.charAt(0).toUpperCase() + fieldName?.slice(1)} is required`;
    } else if (isNaN(number)) {
        return `${fieldName?.charAt(0).toUpperCase() + fieldName?.slice(1)} must be a number`;
    }
    return "";
};

const validatePhoneNumber = (phoneNumber, fieldName, countryCode = "+880") => {
    if (!phoneNumber) {
        return `${fieldName?.charAt(0).toUpperCase() + fieldName?.slice(1)} is required`;
    }

    if (countryCode === "+880") {
        const bangladeshPhoneRegex = /^(?:8801|01|1)\d{9}$/;
        if (!bangladeshPhoneRegex.test(phoneNumber)) {
            return `${fieldName?.charAt(0).toUpperCase() + fieldName?.slice(1)} must be valid (Bangladesh format)`;
        }
    } else {
        const internationalPhoneRegex = /^\d+$/;
        if (!internationalPhoneRegex.test(phoneNumber)) {
            return `${fieldName?.charAt(0).toUpperCase() + fieldName?.slice(1)} must contain only numbers`;
        }
    }
    return "";
};

const validateDate1 = (date, fieldName) => {
    const formattedField = fieldName?.split(/(?=[A-Z])/)?.join(" ");

    if (!date.isValid()) {
        return `${formattedField?.charAt(0)?.toUpperCase() + formattedField?.slice(1)} must be a valid date`;
    }
    if (!date || date?.toString()?.trim() === "") {
        return `${formattedField?.charAt(0)?.toUpperCase() + formattedField?.slice(1)} is required`;
    }

    return "";
};

const validateDate = (dateStr, fieldName) => {
    const formattedField = fieldName?.split(/(?=[A-Z])/)?.join(" ");
    const displayName =
        formattedField?.charAt(0)?.toUpperCase() + formattedField?.slice(1);

    if (!dateStr || dateStr.toString().trim() === "") {
        return `${displayName} is required`;
    }

    const parsedDate = new Date(dateStr);

    // Check if it's a valid date (not NaN)
    if (isNaN(parsedDate.getTime())) {
        return `${displayName} must be a valid date`;
    }

    return "";
};

const validateDateRange = (startDate, endDate, fieldName) => {
    if (!startDate || !endDate) {
        return `${fieldName?.charAt(0)?.toUpperCase() + fieldName?.slice(1)} range is required`;
    } else if (isNaN(Date.parse(startDate)) || isNaN(Date.parse(endDate))) {
        return `${fieldName?.charAt(0)?.toUpperCase() + fieldName?.slice(1)} range must be valid dates`;
    } else if (new Date(startDate) > new Date(endDate)) {
        return `${fieldName?.charAt(0)?.toUpperCase() + fieldName?.slice(1)} start date must be before end date`;
    }
    return "";
};

const validateCheckboxes = (formData) => {
    const {
        cdr,
        eSaf,
        fnf,
        sms,
        lastCallLocation,
        lastRadioLocation,
        biometricInfo,
        subscriberType,
    } = formData;
    if (
        [
            cdr,
            eSaf,
            fnf,
            sms,
            lastCallLocation,
            lastRadioLocation,
            biometricInfo,
            subscriberType,
        ].filter((v) => v).length < 1
    ) {
        return "Request type must not be empty";
    }
    return "";
};

const validateSelect = (value, fieldName) => {
    if (!value) {
        const formattedFieldName = fieldName
            .replace(/([A-Z])/g, " $1")
            .replace(/^./, (str) => str?.toUpperCase());

        return `${formattedFieldName} is required`;
    }
    return "";
};

const validateAutocomplete = (value, fieldName) => {
    if (!value) {
        return `${fieldName?.charAt(0)?.toUpperCase() + fieldName?.slice(1)} is required`;
    }
    return "";
};

const validateField = (type, value, fieldName, login = false) => {
    switch (type) {
        case "email":
            return validateEmail(value);
        case "password":
            return validatePassword(value, login);
        case "confirmPassword":
            return validateConfirmPassword(value, fieldName);
        case "date":
            return validateDate(value, fieldName);
        case "dateRange":
            return validateDateRange(value.start, value.end, fieldName);
        case "select":
            return validateSelect(value, fieldName);
        case "autocomplete":
            return validateAutocomplete(value, fieldName);
        case "mobileNo":
            return validatePhoneNumber(value, fieldName);
        case "checkboxes":
            return validateCheckboxes(value);
        case "privacyPolicy":
            return validatePrivacyPolicy(value, fieldName);
        default:
            if (typeof value === "number") {
                return validateNumber(value, fieldName);
            } else {
                return validateText(value, fieldName);
            }
    }
};

export {
    validateEmail,
    validatePassword,
    validateText,
    validateNumber,
    validateDate,
    validateDateRange,
    validateCheckboxes,
    validateSelect,
    validateAutocomplete,
    validateField,
    validatePhoneNumber,
};
