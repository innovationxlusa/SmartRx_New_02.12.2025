import { useState, useEffect, useRef } from "react";
import "./CustomInput.css";
import { FaRegEye, FaRegEyeSlash } from "react-icons/fa6";

const CustomInput = ({
    icon,
    iconPosition = "left",
    leftIcon,
    rightIcon,
    placeholder = "Search...",
    type = "text",
    value,
    onChange,
    error,
    label,
    labelPosition = "top",
    disabled = false,
    name,
    className,
    focus = false,
    minHeight = "",
    multiple = false,
    borderColor,
    // Country code props
    showCountryCode = false,
    countryCode = "+880",
    onCountryCodeChange,
    countryOptions = [],
    ...rest
}) => {
    const [internalError, setInternalError] = useState("");
    const [showPassword, setShowPassword] = useState(false);

    const inputRef = useRef(null);

    useEffect(() => {
        if (focus && inputRef.current) {
            requestAnimationFrame(() => {
                inputRef.current.focus();
            });
        }
    }, [focus]);

    useEffect(() => {
        let errorMessage = "";
        switch (type) {
            case "email":
                if (value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                    errorMessage = "Invalid email format";
                }
                break;
            case "number":
                if (value && isNaN(value)) {
                    errorMessage = "Only numbers are allowed";
                }
                break;
            case "password":
                if (value && value.length < 6) {
                    errorMessage = "Password must be at least 6 characters";
                }
                break;
            default:
                errorMessage = "";
        }
        setInternalError(errorMessage);
    }, [value, type]);

    return (
        <div
            className={`custom-input-wrapper ${className || ""}`}
            style={{ minHeight }}
        >
            <div
                className={`input-container ${
                    error || internalError ? "error" : ""
                } ${disabled ? "disabled" : ""} label-${labelPosition}`}
            >
                {label && labelPosition === "top" && (
                    <label className="input-label">{label}</label>
                )}
                {label && labelPosition === "top-left" && (
                    <div className="label-top-left-wrapper">
                        <label className="input-label top-left">{label}</label>
                    </div>
                )}
                {label && labelPosition === "top-right" && (
                    <div className="label-top-right-wrapper">
                        <label className="input-label top-right">{label}</label>
                    </div>
                )}
                <div
                    className={`input-wrapper ${
                        labelPosition === "left" || labelPosition === "right"
                            ? "with-side-label"
                            : ""
                    }`}
                >
                    {label &&
                        (labelPosition === "left" ||
                            labelPosition === "right") && (
                            <label
                                className={`input-label side-label ${labelPosition}`}
                            >
                                {label}
                            </label>
                        )}
                    <div
                        className={
                            type === "file" ? "" : "search-input-wrapper"
                        }
                    >
                        {icon && iconPosition === "left" && (
                            <span className="icon left">
                                {typeof icon === "string" ? (
                                    <img
                                        src={icon}
                                        alt="icon"
                                        className="icon-img"
                                    />
                                ) : (
                                    icon
                                )}
                            </span>
                        )}
                        
                        {/* Country Code Dropdown for Mobile Input */}
                        {showCountryCode && type === "tel" && (
                            <div className="country-code-dropdown">
                                <select
                                    value={countryCode}
                                    onChange={onCountryCodeChange}
                                    disabled={disabled}
                                    className="country-code-select"
                                >
                                    {countryOptions.map((option, index) => (
                                        <option key={`${option.value}-${index}`} value={option.value}>
                                            {option.label}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        )}
                        
                        <input
                            ref={inputRef}
                            type={
                                type === "password" && showPassword
                                    ? "text"
                                    : type
                            }
                            placeholder={placeholder}
                            className={`${
                                type === "file" ? "" : "search-input"
                            } ${type === "file" ? "" : className} ${
                                icon && iconPosition === "left"
                                    ? "with-icon-left"
                                    : ""
                            } ${
                                icon && iconPosition === "right"
                                    ? "with-icon-right"
                                    : ""
                            } ${type === "date" ? "form-control w-100" : ""} ${
                                !icon && type !== "file" ? "ps-3" : ""
                            } ${showCountryCode ? "with-country-code" : ""}`}
                            style={{borderColor:borderColor}}
                            name={name}
                            onChange={onChange}
                            disabled={disabled}
                            multiple={multiple}
                            {...(type === "file"
                                ? { accept: "image/*,.pdf" }
                                : { value })}
                            {...rest}
                        />
                        {type === "password" && (
                            <span
                                className="icon right"
                                onClick={() => setShowPassword((prev) => !prev)}
                            >
                                {showPassword ? (
                                    <FaRegEye />
                                ) : (
                                    <FaRegEyeSlash />
                                )}
                            </span>
                        )}
                        {rightIcon && (
                            <span className="icon right">
                                {typeof rightIcon === "string" ? (
                                    <img
                                        src={rightIcon}
                                        alt="icon"
                                        className="icon-img"
                                    />
                                ) : (
                                    rightIcon
                                )}
                            </span>
                        )}
                    </div>
                </div>
                {(error || internalError) && (
                    <p className="error-message mb-0" style={{fontFamily: "Georama"}}>
                        {error || internalError}
                    </p>
                )}
            </div>
        </div>
    );
};

export default CustomInput;
