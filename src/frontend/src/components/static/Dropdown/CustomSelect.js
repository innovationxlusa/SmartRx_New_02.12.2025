import React, { useState, useRef, useEffect, useCallback } from "react";
import { createPortal } from "react-dom";
import "./CustomSelect.css";
import { FaChevronDown } from "react-icons/fa";

const CustomSelect = ({
    id,
    label,
    labelPosition = "top-left",
    options = [],
    placeholder = "",
    onChange,
    value,
    name,
    icon,
    iconTwo,
    error,
    className,
    disabled = false,
    bgColor = "#f9f9f9",
    textColor = "#65636e",
    width = "100%",
    borderRadius = "8px",
    borderColors = "1px solid #65636e",
    dropdownTrayHeight = "80%",
    showOverModal = false,
}) => {
    const [isOpen, setIsOpen] = useState(false);
    const [dropdownStyle, setDropdownStyle] = useState({});
    const wrapperRef = useRef(null);
    const dropdownRef = useRef(null);

    const selectedOption = options.find((opt) => opt.value === value);

    const styleVars = {
        "--custom-bg": bgColor,
        "--custom-text": textColor,
        "--custom-width": width,
        "--custom-radius": borderRadius,
        "--custom-border": borderColors,
    };

    /** ✅ Calculate and update dropdown position when showOverModal = true */
    const updateDropdownPosition = useCallback(() => {
        if (wrapperRef.current && showOverModal) {
            const rect = wrapperRef.current.getBoundingClientRect();
            const top = rect.bottom + window.scrollY + 4; // +scrollY ensures exact page position
            const left = rect.left + window.scrollX;
            const width = rect.width;

            setDropdownStyle({
                position: "absolute", // rendered via portal -> relative to document.body
                top,
                left,
                width,
                zIndex: 999999, // ensures above modal
            });
        }
    }, [showOverModal]);

    const handleSelect = (option) => {
        const fakeEvent = { target: { name, value: option.value } };
        onChange(fakeEvent);
        setIsOpen(false);
    };

    const toggleDropdown = () => {
        if (disabled) return;
        if (!isOpen && showOverModal) updateDropdownPosition();
        setIsOpen((prev) => !prev);
    };

    /** ✅ Close when clicking outside */
    useEffect(() => {
        const handleClickOutside = (event) => {
            if (
                wrapperRef.current &&
                !wrapperRef.current.contains(event.target) &&
                (!dropdownRef.current ||
                    !dropdownRef.current.contains(event.target))
            ) {
                setIsOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () =>
            document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    /** ✅ Reposition on scroll/resize */
    useEffect(() => {
        if (!isOpen || !showOverModal) return;
        const handleReposition = () => updateDropdownPosition();
        window.addEventListener("scroll", handleReposition, true);
        window.addEventListener("resize", handleReposition);
        return () => {
            window.removeEventListener("scroll", handleReposition, true);
            window.removeEventListener("resize", handleReposition);
        };
    }, [isOpen, showOverModal, updateDropdownPosition]);

    /** ✅ Dropdown options content */
    const dropdownContent = (
        <div
            ref={dropdownRef}
            className="custom-select-options"
            style={{
                ...dropdownStyle,
                backgroundColor: bgColor,
                color: textColor,
                border: borderColors,
                borderRadius,
                maxHeight: "200px",
                overflowY: "auto",
            }}
        >
            {options.map((option) => (
                <div
                    key={option.value}
                    className={`custom-select-option ${
                        option.value === value ? "selected" : ""
                    }`}
                    onClick={() => handleSelect(option)}
                >
                    {option.label}
                </div>
            ))}
        </div>
    );

    return (
        <div className={`select-flex-wrapper ${className || ""}`}>
            <div id={id} className="custom-select-wrapper">
                {label && (
                    <label
                        htmlFor={id}
                        className={`custom-select-label ${labelPosition}`}
                    >
                        {label}
                    </label>
                )}

                <input type="hidden" name={name} value={value || ""} />

                <div
                    className="custom-select-container"
                    style={styleVars}
                    ref={wrapperRef}
                >
                    {/* ✅ Select Display */}
                    <div
                        className={`custom-select-display ${
                            isOpen ? "focused" : ""
                        } ${disabled ? "disabled" : ""}`}
                        style={{
                            paddingLeft: icon
                                ? "2.5rem"
                                : iconTwo
                                  ? "20px"
                                  : "8px",
                            paddingRight: "2.5rem",
                        }}
                        onClick={toggleDropdown}
                        tabIndex={0}
                    >
                        {icon && (
                            <span className="select-icon left">{icon}</span>
                        )}
                        <span
                            className="selected-value"
                            style={{ marginLeft: icon ? "8px" : "0" }}
                        >
                            {selectedOption?.label || placeholder}
                        </span>
                        <span className="select-icon right">
                            <FaChevronDown
                                className={isOpen ? "rotate-180" : ""}
                            />
                        </span>
                    </div>

                    {/* ✅ Dropdown Rendering */}
                    {isOpen &&
                        (showOverModal ? (
                            // ✅ Dropdown rendered via portal to body (shows over modal)
                            createPortal(dropdownContent, document.body)
                        ) : (
                            // ✅ Normal inline dropdown (original UI & behavior)
                            <div
                                ref={dropdownRef}
                                className="custom-select-options"
                                style={{
                                    position: "absolute",
                                    top: "115%",
                                    left: 0,
                                    right: 0,
                                    backgroundColor: bgColor,
                                    color: textColor,
                                    border: borderColors,
                                    borderRadius,
                                    maxHeight: "200px",
                                    overflowY: "auto",
                                    zIndex: 1000,
                                }}
                            >
                                {options.map((option) => (
                                    <div
                                        key={option.value}
                                        className={`custom-select-option ${
                                            option.value === value
                                                ? "selected"
                                                : ""
                                        }`}
                                        onClick={() => handleSelect(option)}
                                    >
                                        {option.label}
                                    </div>
                                ))}
                            </div>
                        ))}
                </div>

                {error && (
                    <p
                        className="error-message mb-0"
                        style={{ fontFamily: "Georama" }}
                    >
                        {error}
                    </p>
                )}
            </div>
        </div>
    );
};

export default CustomSelect;
