import "./NotFoundPage.css";
import { FaHome } from "react-icons/fa";
import { IoReturnUpBack } from "react-icons/io5";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import CustomButton from "../../components/static/Commons/CustomButton";

const NotFoundPage = () => {
    const { smartNavigate, navigateBack, navigateForward } = useSmartNavigate({ scroll: "top" });

    /**
     * Navigate to the previous page when the "Go Back" button is clicked.
     */
    const handleGoBack = () => {
        navigateBack();
    };

    /**
     * Navigate to the home page when the "Go to Home" button is clicked.
     */
    const handleGoHome = () => {
        smartNavigate("/");
    };

    return (
        <div className="not-found-container position-relative d-flex justify-content-center align-items-center">
            <div className="not-found-background" aria-hidden="true">
                <div className="circle1 d-flex justify-content-center align-items-center">
                    <img src="/robi-white-bn-logo.svg" alt="logo" className="img-fluid" />
                </div>
                <div className="circle2 d-flex justify-content-center align-items-center">
                    <img src="/robi-white-bn-logo.svg" alt="logo" className="img-fluid" />
                </div>
                <div className="circle3 d-flex justify-content-center align-items-center">
                    <img src="/robi-white-bn-logo.svg" alt="logo" className="img-fluid" />
                </div>
            </div>

            {/* Responsive content container using Bootstrap */}
            <div className="container text-center position-relative z-3 px-3">
                <div className="row justify-content-center">
                    <div className="col-lg-8 col-md-10">
                        <h1 className="display-1 fw-bold text-uppercase not-found-title">404</h1>
                        <h2 className="display-6 mb-3 not-found-subtitle">Oops! Page not found</h2>
                        <p className="lead mb-4 not-found-description mx-auto">The page you are looking for might have been removed or is temporarily unavailable.</p>

                        {/* Responsive button container using Bootstrap */}
                        <div className="d-flex gap-3 justify-content-center">
                            <CustomButton
                                isLoading={false}
                                type="button"
                                icon={<IoReturnUpBack />}
                                label="Go Back"
                                disabled={false}
                                backgroundColor={""}
                                textColor={"var(--theme-font-color)"}
                                shape={"pill"}
                                borderColor="2px solid var(--theme-font-color)"
                                onClick={handleGoBack}
                                width={"200px"}
                                hoverEffect="theme"
                            />
                            <CustomButton
                                isLoading={false}
                                type="button"
                                icon={<FaHome />}
                                label="Go to Home"
                                disabled={false}
                                backgroundColor={""}
                                textColor={"var(--theme-font-color)"}
                                shape={"pill"}
                                borderColor="2px solid var(--theme-font-color)"
                                onClick={handleGoHome}
                                width={"200px"}
                                hoverEffect="theme"
                            />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default NotFoundPage;
