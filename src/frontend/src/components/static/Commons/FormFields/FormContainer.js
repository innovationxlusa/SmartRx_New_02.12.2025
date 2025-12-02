import React from "react";
import CustomButton from "./CustomButton";
import { Container, Row, Col } from "react-bootstrap";
// import { useCustomCSS } from "../../../hooks/useCustomCSS";
// import { useHasAuthority } from "../../../contexts/AuthoritiesContext";

/**
 * FormContainer Component
 * A reusable form container component that handles permissions, loading states,
 * and dynamic theming using CSS constants.
 *
 * @param {Object} props - Props passed to the component
 * @param {boolean} props.isComponentLoading - Loading state for the entire form component
 * @param {Array} props.data - Data array to check if form should be rendered
 * @param {boolean} props.hasVisibilityPermission - Permission check for visibility of the form
 * @param {boolean} props.hasCreatePermission - Permission check for form submission button
 * @param {JSX.Element} props.header - Optional header to display at the top of the form
 * @param {JSX.Element} props.children - Child components (form fields, etc.)
 * @param {string} props.containerClassName - Additional class names for the container
 * @param {string} props.colClassName - Additional class names for the column
 * @param {JSX.Element} props.buttonIcon - Icon for the submit button
 * @param {string} props.buttonLabelName - Label for the submit button
 * @param {JSX.Element} props.skeletonLoader - Loader to display while the component is loading
 * @param {string} props.buttonWidth - Width of the submit button
 * @param {Function} props.handleAssignRoles - Function to handle form submission
 *
 * @returns {JSX.Element} A form container with dynamic content and permission-based rendering.
 */
const FormContainer = ({
    rowClassName = "justify-content-md-center mt-5",
    colClassName = "col-10 col-md-6 mx-auto",
    isComponentLoading,
    isDataAvailable,
    hasVisibilityPermission,
    hasCreatePermission,
    header,
    children,
    buttonIcon,
    buttonLabelName,
    buttonWidth,
    handleSubmit,
    skeletonLoader,
    isLoading,
}) => {
    // Custom CSS constants for dynamic theming and styling based on theme
    const CSS_CONSTANTS = useCustomCSS();

    // Check user permissions for visibility and create actions
    const { hasAuthority } = useHasAuthority();

    return (
        <Container className="overflow-x-hidden" style={{ height: "93vh", overflowY: "auto" }}>
            <Row className={rowClassName}>
                <Col className={colClassName}>
                    {/* If not loading and data is available, check visibility permissions */}
                    {!isComponentLoading && isDataAvailable ? (
                        hasAuthority(hasVisibilityPermission) ? (
                            <>
                                {/* Optional header display */}
                                {header && <h2 className={CSS_CONSTANTS.TEXT_COLOR}>{header}</h2>}

                                {/* Form rendering */}
                                <form onSubmit={handleSubmit}>
                                    {children}

                                    {/* Conditionally render the submit button based on create permissions */}
                                    {hasAuthority(hasCreatePermission) && (
                                        <div className="d-flex justify-content-center mt-5">
                                            <CustomButton
                                                isLoading={isLoading} // Loading state for the button
                                                type="submit" // Form submission button
                                                icon={buttonIcon} // Button icon
                                                label={buttonLabelName} // Button label
                                                disabled={isLoading || isComponentLoading} // Disable button if loading
                                                backgroundColor={CSS_CONSTANTS.PRIMARY_COLOR} // Dynamic primary color
                                                textColor={CSS_CONSTANTS.WHITE_COLOR} // Dynamic text color
                                                shape="pill" // Pill-shaped button
                                                borderStyle={`2px solid ${CSS_CONSTANTS.BLACK_COLOR}`} // Dynamic border
                                                width={buttonWidth} // Button width customization
                                            />
                                        </div>
                                    )}
                                </form>
                            </>
                        ) : null
                    ) : (
                        // Display skeleton loader if loading or data is not available
                        skeletonLoader
                    )}
                </Col>
            </Row>
        </Container>
    );
};

export default FormContainer;
