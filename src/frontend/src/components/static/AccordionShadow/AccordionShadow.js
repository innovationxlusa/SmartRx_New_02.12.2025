import React, { useState, useId } from "react";
import "./AccordionShadow.css";
import accUpButton from "../../../assets/img/DropdownArrow.svg";
import accDownButton from "../../../assets/img/DownArrow.svg";

const AccordionShadow = ({ accordionHeader = "", children, defaultOpen }) => {
  const [isOpen, setIsOpen] = useState(defaultOpen);
  const uniqueId = useId();
  const collapseId = `collapse-${uniqueId}`;
  const headingId = `heading-${uniqueId}`;

  return (
    <div className="accordion accordion-shadow" id={`accordion-${uniqueId}`}>
      <div className="accordion-item border border-0">
        <h2 className="accordion-header" id={headingId}>
          <button
            className={`accordion-button ${!isOpen ? "collapsed" : ""}`}
            type="button"
            onClick={() => setIsOpen(!isOpen)}
            aria-expanded={isOpen}
            aria-controls={collapseId}
          >
            <div className="accordion-button-content">
              <div className="accordion-header">{accordionHeader}</div>
              <img
                src={isOpen ? accUpButton : accDownButton}
                alt="Toggle Icon"
                className="accordion-icon"
              />
            </div>
          </button>
        </h2>
        <div
          id={collapseId}
          className={`accordion-collapse collapse ${isOpen ? "show" : ""} mt-3`}
          aria-labelledby={headingId}
        >
          <div className="accordion-body">{children}</div>
        </div>
      </div>
    </div>
  );
};

export default AccordionShadow;