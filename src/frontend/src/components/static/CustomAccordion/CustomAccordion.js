import { useState, useId, useRef, useEffect } from "react";
import "./CustomAccordion.css";
import accDownButton from "../../../assets/img/DownArrow.svg";

const CustomAccordion = ({
    defaultOpen = false,
    accordionHeader = "",
    accordionHeaderData = "",
    className = "",
    children,
    border = "",
    height = "",
    borderRadius = "9px",
    background = "linear-gradient(90deg, #f6f3ff 29%, #c1b8cd 77%, #bdafd2 96%)",
    shadow = true,
    iconClassName = "",
    iconStyleOverride = {},
    accordionArrowIconClass = "me-3",
    isInvestigation=false,
}) => {
    const [isOpen, setIsOpen] = useState(defaultOpen);
    const [maxHeight, setMaxHeight] = useState("0px");
    const contentRef = useRef(null);

    const uniqueId = useId();
    const collapseId = `collapse-${uniqueId}`;
    const headingId = `heading-${uniqueId}`;

    useEffect(() => {
        const el = contentRef.current;
        if (el) {
            setMaxHeight(isOpen ? "100%" : "0px");
        }
    }, [isOpen]);

    const buttonStyle = {
        background: background,
        border: border,
        borderRadius: borderRadius,
        height: "87px",
        filter: shadow ? "drop-shadow(0px 4px 4px rgba(0, 0, 0, 0.25))" : "",
        padding: shadow ? "16px 20px" : "5px",
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        width: "100%",
    };
 const buttonStyleInvestigation = {
        background: background,
        border: border,
        borderRadius: borderRadius,
        height: "115px",
        filter: shadow ? "drop-shadow(0px 4px 4px rgba(0, 0, 0, 0.25))" : "",
        padding: shadow ? "16px 20px" : "5px",
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        width: "100%",
    };
    const customAccordionInv={
        height:"120px"
    }
    const iconStyle = {
        width: "20px",
        height: "20px",
        marginRight: "4px",
        transition: "transform 0.35s ease",
        transform: isOpen ? "rotate(180deg)" : "rotate(0deg)",
        ...iconStyleOverride,
    };

    return (
        <div className={`accordion custom-accordion ${className}`} id={`accordion-${uniqueId}`}>
            <div className="accordion-item border border-0">
                <h2 className="accordion-header" id={headingId}>
                    <button
                        className={`accordion-button z-0 ${!isOpen ? "collapsed" : ""}`}
                        type="button"
                        onClick={() => setIsOpen(!isOpen)}
                        aria-expanded={isOpen}
                        aria-controls={collapseId}
                        style={isInvestigation?buttonStyleInvestigation: buttonStyle}
                    >
                        <div className="d-flex align-items-center justify-content-between w-100">
                            <div className="flex-grow-1">
                                {accordionHeaderData ? <div className={accordionArrowIconClass}>{accordionHeaderData}</div> : <div className="accordion-header">{accordionHeader}</div>}
                            </div>
                            {/* <img src={accDownButton} alt="Toggle Icon" style={iconStyle} className={`accordion-icon ${iconClassName || ""}`} /> */}
                        </div>
                    </button>
                </h2>
                <div
                    id={collapseId}
                    ref={contentRef}
                    className={`accordion-collapse ${isOpen ? "show" : ""}`}
                    aria-labelledby={headingId}
                    style={{
                        maxHeight,
                        overflow: !isOpen ? "hidden" : "",
                        transition: "max-height 0.35s ease, opacity 0.35s ease",
                        opacity: isOpen ? 1 : 0,
                    }}
                >
                    <div className={`accordion-body ${isInvestigation ? "investigation-test-center-body" : ""}`}>
                        {children}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CustomAccordion;
