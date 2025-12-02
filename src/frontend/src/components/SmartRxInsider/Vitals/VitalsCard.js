import { useState, useRef, useEffect } from "react";
import "./Vitals.css";
import Book from "../../../assets/img/Book.svg";
import WaterDrop from "../../../assets/img/WaterDrop.svg";
import CustomButton from "../../static/Commons/CustomButton";
import SearchIconNew from "../../../assets/img/SearchIconNew.svg";
import { VITALS_OBSERVATION } from "../../../constants/constants";

const VitalsCard = ({
    id,
    value,
    standard,
    observation,
    description,
    vitalData,
    measurementUnit,
    rangeGraph,
    handleOpenModal,
    handleOpenModalEdit,
}) => {
    const [isExpanded, setIsExpanded] = useState(false);
    const descContainerRef = useRef(null);
    const [dimensions, setDimensions] = useState({
        width: "auto",
        height: "3em", // Initial collapsed height
    });

    // Adjust this to however much you want shown before truncation
    const previewLength = 30; // Approx. 2 lines, adjust as needed
    const isTruncated = description.length > previewLength;
    const previewText = description.slice(0, previewLength);

    useEffect(() => {
        if (descContainerRef.current) {
            if (isExpanded) {
                // Get natural dimensions when expanded
                const scrollWidth = descContainerRef.current.scrollWidth;
                const scrollHeight = descContainerRef.current.scrollHeight;
                setDimensions({
                    width: `${scrollWidth}px`,
                    height: `${scrollHeight}px`,
                });
            } else {
                // Collapsed dimensions
                setDimensions({
                    width: "auto",
                    height: "3em",
                });
            }
        }
    }, [isExpanded, description]);

    const buttonProps = {
        isLoading: "",
        disabled: false,
        width: "clamp(120px, 20vw, 200px)",
        height: "clamp(30px, 2.3vw, 40px)",
        backgroundColor: "",
        textColor: "var(--theme-font-color)",
        shape: "roundedSquare",
        borderStyle: "",
        borderColor: "1px solid var(--theme-font-color)",
        iconStyle: { color: "var(--theme-font-color)" },
        labelStyle: {
            fontSize: "clamp(14px, 2vw, 16px)",
            fontWeight: "100",
            textTransform: "capitalize",
        },
        hoverEffect: "theme",
    };

    return (
        <div className="vital-card" id={id}>
            <div className="vital-card-body">
                <div className="d-flex align-items-start">
                    <div className="me-3">
                        <img src={Book} alt="Book" />
                    </div>

                    <div
                        ref={descContainerRef}
                        className="vital-desc-container"
                        style={{
                            width: dimensions.width,
                            height: dimensions.height,
                        }}
                    >
                        <p className="vital-desc">
                            {isExpanded || !isTruncated
                                ? description
                                : previewText + " ..."}
                            {isTruncated && (
                                <span
                                    className="vital-read-inline"
                                    onClick={() => setIsExpanded(!isExpanded)}
                                >
                                    {isExpanded ? " Show Less" : "Read More"}
                                </span>
                            )}
                        </p>
                    </div>
                </div>

                <div className="vital-value-block d-flex align-items-start">
                    <div className="me-3 pt-1">
                        <img src={WaterDrop} alt="Water Drop" />
                    </div>
                    <div>
                        <h3>
                            {value} <span>{measurementUnit}</span>
                        </h3>
                        <p>
                            Standard: {standard} {measurementUnit}
                        </p>
                    </div>
                </div>

                <div className="vital-observation-block d-flex align-items-start">
                    <div className="me-3 pt-1">
                        <img src={SearchIconNew} alt="Search Icon" />
                    </div>
                    <div className="d-flex align-items-center">
                        <div className="vital-title me-4">Observation</div>
                        <div
                            className={`vital-observation ${observation?.toLowerCase()}`}
                        >
                            {/* <Check alt="Check" className="me-2" /> */}
                            {VITALS_OBSERVATION[observation]}
                            {observation}
                        </div>
                    </div>
                </div>

                {rangeGraph && rangeGraph}
            </div>
            {vitalData?.name === "BMI" ? (
                <div className="vital-card-footer mt-12 center-container">
                    <CustomButton
                        type="button"
                        label="FAQ"
                        onClick={(e) => {
                            e.stopPropagation();
                            e.nativeEvent.stopImmediatePropagation();
                            handleOpenModal(vitalData, "faq");
                        }}
                        {...buttonProps}
                    />
                </div>
            ) : (
                <div className="vital-card-footer mt-8 flex justify-center gap-2 px-2 sm:px-4 lg:px-6">
                    <CustomButton
                        type="button"
                        label="Edit"
                        onClick={() => handleOpenModal(vitalData, "edit")}
                        {...buttonProps}
                    />
                    <CustomButton
                        type="button"
                        label="Delete"
                        onClick={() => handleOpenModal(vitalData, "delete")}
                        {...buttonProps}
                    />
                    <CustomButton
                        type="button"
                        label="FAQ"
                        onClick={(e) => {
                            e.stopPropagation();
                            e.nativeEvent.stopImmediatePropagation();
                            handleOpenModal(vitalData, "faq");
                        }}
                        {...buttonProps}
                    />
                </div>
            )}
        </div>
    );
};

export default VitalsCard;
