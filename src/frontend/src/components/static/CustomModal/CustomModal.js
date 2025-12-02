import React from "react";
import "./CustomModal.css";
import CustomButton from "../Commons/CustomButton";
// import { ReactComponent as CloseIcon } from "../../../assets/img/CloseIcon.svg"; // Adjust the path as necessary
// import CloseIcon from "../../../assets/img/CloseIcon.svg"; // Adjust the path as necessary
import { IoClose } from "react-icons/io5";

const CustomModal = ({
    isOpen,
    close,
    modalName,
    subModal,
    formData,
    children,
    animationDirection = "top",
    position = "middle",
    form,
    onSubmit,
    isButtonLoading,
    buttonType,
    buttonIcon,
    buttonLabel,
    isButtonDisabled,
    buttonWidth,
    buttonBackgroundColor,
    buttonTextColor,
    buttonShape,
    buttonBorderStyle,
    buttonBorderColor,
    buttonIconStyle,
    buttonLabelStyle,
    anotherButtonName,
    closeOnOverlayClick = true,
    pdf = false, // Prop to indicate if the modal is for PDF preview
    hoverEffect = "theme",
    isImagePreview = false, // New prop for image preview
    dataPreview = false, // New prop for image preview
    modalSize = "small", // Prop for modal size
    anotherButton = false,
    modalNameStyle = {}, // Prop for custom modal name styling
}) => {
    const modalSizeClass = `modal-size-${modalSize}`; // Dynamically apply size class

    return (
        <div
            className={`modal-box ${modalSizeClass} ${
                isOpen ? "open" : ""
            } position-${position} animation-${animationDirection}`}
        >
          
            <div
                className="modal-overlay"
                onClick={() => {
                    closeOnOverlayClick && close();
                }}
            ></div>
            <div
                className={`modal-dialog ${
                    isImagePreview ? "image-preview-modal" : ""
                }`}
                style={{
                    background: isImagePreview ? "transparent" : "white",
                    boxShadow: isImagePreview
                        ? ""
                        : "0 0 10px rgba(0, 0, 0, 0.3)",
                }}
            >
                <div
                    className={
                        dataPreview ? "modal-content p-1" : "modal-content"
                    }
                >
                    {!isImagePreview && (
                        <div className="close" onClick={close}>
                            <IoClose />
                        </div>
                    )}

                    {modalName && !isImagePreview && (
                        <>
                        <span
                            className={`${
                                dataPreview
                                    ? "modal-name-data-preview"
                                    : " text-left fs-5 fw-semibold"
                            }`}
                            style={modalNameStyle}
                        >
                            {modalName}
                        </span>
                       
                        </>
                    )}
                    {subModal && !isImagePreview && (
                        <span className="sub-modal-name-data-preview">
                            {subModal}
                        </span>
                    )}

                    <div
                        className={`"modal-data" ${
                            dataPreview ? "modal-data pt-0" : "modal-data"
                        }`}
                    >
                        <div
                            className={` ${
                                isImagePreview
                                    ? "modal-body-file d-flex justify-content-center"
                                    : dataPreview
                                      ? "modal-body-data-preview"
                                      : "modal-body"
                            }`}
                        >
                            {isImagePreview ? (
                                <div className="image-preview-wrapper">
                                    {children} {/* Render image preview */}
                                    <div
                                        className={
                                            pdf ? "close-pdf" : "close-image"
                                        }
                                        onClick={close}
                                    >
                                        <IoClose />
                                        {/* <img src={CloseIcon} alt="Close" /> */}
                                    </div>
                                </div>
                            ) : form && !dataPreview ? (
                                <>
                                    {formData?.error && (
                                        <div className="error-banner">
                                            {formData.error}
                                        </div>
                                    )}
                                    {formData?.success && (
                                        <div className="success-banner">
                                            {formData.success}
                                        </div>
                                    )}
                                    <form className="w-100" onSubmit={onSubmit}>
                                        {/* Render children */}
                                        {children}
                                        <div className="d-flex justify-content-center w-100 mt-2">
                                            {anotherButton ? (
                                                <div
                                                    className="d-flex justify-content-center mt-2"
                                                    style={{
                                                        gap: "80px",
                                                        width: "auto",
                                                    }}
                                                >
                                                    {/* <div
                                                        className="close-vital-btn"
                                                        style={{
                                                            width: "100px",
                                                            height: "35px",
                                                        }}
                                                    >
                                                        <CustomButton
                                                            isLoading=""
                                                            type="button"
                                                            icon=""
                                                            label="Close"
                                                            onClick={close}
                                                            disabled={
                                                                isButtonDisabled
                                                            }
                                                            width="100px"
                                                            height="35px"
                                                            backgroundColor="#ffffff"
                                                            textColor="#4b3b8b"
                                                            borderRadius="5%"
                                                            hoverEffect={
                                                                hoverEffect
                                                            }
                                                            labelStyle={
                                                                buttonLabelStyle
                                                            }
                                                            borderColor={
                                                                buttonBorderColor
                                                            }
                                                        />
                                                        </div> */}

                                                    <div
                                                        className="add-vital-btn"
                                                    >
                                                        <CustomButton
                                                            isLoading=""
                                                            type="submit"
                                                            icon=""
                                                            label={
                                                                anotherButtonName
                                                            }
                                                            disabled={
                                                                isButtonDisabled
                                                            }
                                                            width="100px"
                                                            height="35px"
                                                            backgroundColor="#e6e4ef"
                                                            textColor="#4b3b8b"
                                                            borderRadius="6px"
                                                            fontSize="0.95rem"
                                                            hoverEffect={
                                                                hoverEffect
                                                            }
                                                            labelStyle={
                                                                buttonLabelStyle
                                                            }
                                                            borderColor={
                                                                "1px solid #b8aef2"
                                                            }
                                                        />
                                                    </div>
                                                </div>
                                            ) : (
                                                <CustomButton
                                                    isLoading={isButtonLoading}
                                                    type={buttonType}
                                                    icon={buttonIcon}
                                                    label={buttonLabel}
                                                    disabled={isButtonDisabled}
                                                    width={buttonWidth}
                                                    backgroundColor={
                                                        buttonBackgroundColor
                                                    }
                                                    textColor={buttonTextColor}
                                                    shape={buttonShape}
                                                    borderStyle={
                                                        buttonBorderStyle
                                                    }
                                                    borderColor={
                                                        buttonBorderColor
                                                    }
                                                    iconStyle={buttonIconStyle}
                                                    labelStyle={
                                                        buttonLabelStyle
                                                    }
                                                    hoverEffect={hoverEffect}
                                                />
                                            )}
                                        </div>
                                    </form>
                                </>
                            ) : (
                                children // Render generic modal content
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CustomModal;
