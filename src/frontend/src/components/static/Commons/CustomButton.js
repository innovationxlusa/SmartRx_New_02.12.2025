import "./CustomButton.css";
import { Spinner } from "react-bootstrap";

/**
 * CustomButton Component
 * A reusable button component with dynamic styling, hover effects, loading state, and optional modal handling.
 *
 * @param {Object} props - Props passed to the component
 * @param {boolean} props.isLoading - Loading state to show spinner instead of label
 * @param {string} props.type - Button type (submit, button, etc.)
 * @param {JSX.Element} props.icon - Optional icon to display inside the button
 * @param {string} props.label - Button label text
 * @param {boolean} props.disabled - Disable state of the button
 * @param {string} props.backgroundColor - Background color of the button
 * @param {boolean} props.borderRadius - Border Radius of the button
 * @param {string} props.textColor - Text color of the button
 * @param {string} props.shape - Shape of the button (pill, square, roundedSquare)
 * @param {string} props.borderStyle - Custom border style (e.g., solid, dotted)
 * @param {Function} props.onClick - OnClick event handler
 * @param {string} props.width - Width of the button
 * @param {boolean} props.openModal - State to adjust padding for modal-specific behavior
 *
 * @returns {JSX.Element} A customizable button with dynamic styling and hover effects.
 */

const CustomButton = ({
    isLoading,
    type,
    icon,
    label,
    disabled,
    backgroundColor,
    borderRadius,
    textColor,
    shape,
    borderStyle,
    borderColor,
    onClick,
    width,
    height,
    openModal = false,
    iconStyle,
    labelStyle,
    className,
    zIndex,
    hoverEffect = "theme", // "theme" | "light" | "none",
}) => {
    // Dynamic class assignment based on the shape prop
    const shapeClasses =
        {
            pill: "rounded-pill", // Fully rounded corners for pill shape
            square: "", // Default square shape
            roundedSquare: "rounded-3", // Slightly rounded corners
            rounded: "rounded-4",
        }[shape] || "";

    return (
        <button
            style={{
                background: backgroundColor, // Dynamic background color
                border: borderColor, // Dynamic border style
                color: textColor, // Dynamic text color
                width: width, // Custom width for the button
                transition: "all 0.3s ease", // Smooth transition for hover effects
                borderRadius: borderStyle || borderRadius,
                height: height,
                // borderRadius: borderRadius,
                cursor: isLoading || disabled ? "not-allowed" : "pointer", // Change cursor based on loading or disabled state
                zIndex: zIndex,
            }}
            type={type}
            onClick={onClick}
            className={`custom-button fw-semibold d-flex justify-content-center align-items-center px-2 text-uppercase ${shapeClasses} ${
                isLoading ? "cursor-not-allowed" : "cursor-pointer"
            } 
                ${!openModal && "py-1"} ${className}
                ${
                    !disabled &&
                    (hoverEffect === "theme"
                        ? "custom-button-theme-hover"
                        : hoverEffect === "light"
                        ? "custom-button-light-hover"
                        : "")
                } 
                `} // Apply conditional padding and hover class
            disabled={isLoading || disabled} // Disable button during loading or if disabled
        >
            {/* Show loading spinner if isLoading is true, otherwise display the icon and label */}
            {isLoading ? (
                <div style={{ padding: "0" }}>
                    <Spinner color="inherit" animation="border" size="sm" />
                </div>
            ) : (
                <>
                    {/* Display icon if passed as a prop */}
                    {icon && (
                        <div
                            className={`fs-5 d-flex align-items-center icon-style ${
                                label ? "me-2" : ""
                            }`}
                            style={iconStyle}
                        >
                            {icon}
                        </div>
                    )}
                    <div
                        className={
                            openModal ? "py-1 py-md-0 pb-md-1" : "button-label"
                        }
                        style={labelStyle}
                    >
                        {label}
                    </div>
                </>
            )}
        </button>
    );
};
export default CustomButton;
