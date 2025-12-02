import { useRef } from "react";
import "./LearnMore.css";
import { FaChevronLeft } from "react-icons/fa6";
import BackButton from "../static/Commons/BackButton";
import { useAuthContext } from "../../contexts/AuthContext";
import { ReactComponent as LearnMoreIcon } from "../../assets/img/LearnMore.svg";

const LearnMoreTemplate = ({ title, subtitle, videoSrc, bannerText, mainContent, backIcon = false }) => {
    const videoRef = useRef(null);
    const { accessToken } = useAuthContext();

    const enterFullscreen = () => {
        const videoWrapper = videoRef.current;
        if (videoWrapper.requestFullscreen) {
            videoWrapper.requestFullscreen();
        } else if (videoWrapper.webkitRequestFullscreen) {
            videoWrapper.webkitRequestFullscreen();
        } else if (videoWrapper.msRequestFullscreen) {
            videoWrapper.msRequestFullscreen();
        }
    };

    const handleBack = () => {
        window.history.back();
    };

    return (
        <>
            {/* Header */}
            <section className="mt-4 mt-md-5">
                <div className="d-flex justify-content-md-center justify-content-start align-content-center learn-more-header">
                    <div className="d-flex align-items-center">
                        {accessToken && backIcon && (
                            <div className="me-0">
                                <BackButton icon={<FaChevronLeft />} iconPosition="left" onBack={handleBack} />
                            </div>
                        )}
                        <LearnMoreIcon className={`learnMoreIcon ${!accessToken && backIcon ? "ms-md-5 ms-0" : ""}`} alt="Learn more icon" />
                    </div>
                    <div className="FileRxRightStyle pt-4">
                        <h4>{title}</h4>
                        <p>{subtitle}</p>
                    </div>
                </div>
            </section>

            {/* Video Section */}
            <section>
                <div className="row text-center">
                    <div className="col-12 d-flex justify-content-center midleVideoSection">
                        <div className="video-wrapper" ref={videoRef} onClick={enterFullscreen}>
                            <video src={videoSrc} controls style={{ width: "100%", height: "auto", borderRadius: "10px" }} />
                        </div>
                    </div>
                    <div className="PrescriptionTitle mt-0 mt-md-4">{bannerText}</div>
                </div>
            </section>

            {/* Middle Section */}
            <section className="mt-3 pb-4">
                <div className="row">
                    <div className="col-md-12 middleTextStyle ps-2 ps-md-3 ps-lg-5">
                        <div className="content-padding">{mainContent}</div>
                    </div>
                </div>
            </section>
        </>
    );
};

export default LearnMoreTemplate;
