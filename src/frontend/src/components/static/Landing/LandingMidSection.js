import React from "react";
import "./LandingMidSection.css";
import CountUp from "react-countup";
import LearnMoreAndWatchVideoButton from "../../LearnMoreAndWatchVideoButton/LearnMoreAndWatchVideoButton";

const LandingMidSection = ({ title = "", subTitle = "", description = "", backgroundColor, url }) => {
    return (
        <div className="landing-mid-section container-fluid p-4 p-md-5" style={{ backgroundColor }}>
            <div className="row">
                <div className="col-12 mb-3">
                    <div className="section-title">{title}</div>
                    <div className="section-subtitle">{subTitle}</div>
                </div>
                <div className="col-12 mb-4">
                    <p className="section-description">{description}</p>
                </div>
                <div className="col-12 d-flex justify-content-center mb-4">
                    <LearnMoreAndWatchVideoButton backgroundColor="linear-gradient(90deg, #000 0%, #D0BDBD 100%)" dividerHeight="2px" url={url} />
                </div>
                <div className="col-12 d-flex justify-content-center">
                    <div className="stats-wrapper d-flex gap-5 flex-wrap justify-content-center">
                        <div className="stat-item text-center">
                            <p className="stat-count">
                                <CountUp start={0} end={199} duration={5} suffix=" +" />
                            </p>
                            <p className="stat-label">Registered Users</p>
                        </div>
                        <div className="stat-item text-center">
                            <p className="stat-count">
                                <CountUp start={0} end={199} duration={5} suffix=" +" />
                            </p>
                            <p className="stat-label">Medical Videos</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LandingMidSection;
