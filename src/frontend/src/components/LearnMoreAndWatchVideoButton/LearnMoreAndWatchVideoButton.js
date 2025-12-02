import React from "react";
import { Link } from "react-router-dom";
import "./LearnMoreAndWatchVideoButton.css";

const LearnMoreAndWatchVideoButton = ({
    dividerWidth = "291.56px",
    dividerHeight = "4px",
    buttonWidth = "100px",
    buttonHeight = "35px",
    backgroundColor = "linear-gradient(to right, #000000, #a86bf8)",
    url,
}) => {
    return (
        <div className="learn-more-container d-flex flex-column align-items-center gap-2 px-3 px-md-5 mx-3 mx-md-5">
            {/* Top Line */}
            <div className="learn-more-divider-line" style={{ background: backgroundColor, width: dividerWidth, height: dividerHeight }}></div>

            <div className="learn-more-button-group">
                <Link to={url} className="text-decoration-none">
                    <div className="learn-more-custom-button learn-more-learn-more-button justify-content-center" style={{ width: buttonWidth, height: buttonHeight }}>
                        Learn More
                    </div>
                </Link>
                <div className="learn-more-custom-button learn-more-video-button" style={{ width: buttonWidth, height: buttonHeight }}>
                    <span className="learn-more-icon-wrapper d-flex justify-content-start align-items-center">
                        <svg viewBox="0 0 24 24">
                            <polygon points="6,2 20,12 6,20" />
                        </svg>
                    </span>
                    <span className="ms-2">Watch Video</span>
                </div>
            </div>

            {/* Bottom Line */}
            <div className="learn-more-divider-line" style={{ background: backgroundColor, width: dividerWidth, height: dividerHeight }}></div>
        </div>
    );
};

export default LearnMoreAndWatchVideoButton;
