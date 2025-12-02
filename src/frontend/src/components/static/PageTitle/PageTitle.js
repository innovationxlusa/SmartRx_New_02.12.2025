import BackButton from "../Commons/BackButton";
import Chevron_Right from "../../../assets/img/ChevronRight.svg";
import SwitchPatientToggle from "../Toggles/SwitchPatientToggle";
import useScreenSize from "../../../hooks/useScreenSize";
import useSmartNavigate from "../../../hooks/useSmartNavigate";
import ProfilePicture from "../../PatientProfile/ProfilePicture";
import ProfileProgress from "../../PatientProfile/ProfileProgress";
import SwitchImg from "../../../assets/img/SwitchUserIconNew.svg";

const PageTitle = ({
    backButton = true,
    pageName,
    switchButton = true,
    noMargin = false,
    lowMargin = false,  
    leftContent,
    profileData,
    showProfilePicture = false,
    isSinglePatientView = false,
    progressData,
    patientData,
    smallText = false,
}) => {
    const handleBack = () => {
        window.history.back();
    };
    const { smartNavigate } = useSmartNavigate({ scroll: "top" });
    const { width } = useScreenSize();
    return (
        <div
            className={`position-relative d-flex justify-content-between align-items-center ${noMargin ? "mb-2" :
                lowMargin ? "mb-0" :"mb-3"
                } ${!backButton ? "py-3" : ""}`}
        >
            <div className="d-flex align-items-center gap-2">
                {backButton && (
                    <BackButton
                        icon={Chevron_Right}
                        iconPosition="left"
                        onBack={handleBack}
                    />
                )}
            </div>

            {showProfilePicture && profileData && isSinglePatientView ? (
                <div className="d-flex align-items-center gap-3 profile-content-container-full-width">
                    <div className="profile-pic-small">
                        <ProfilePicture
                            size="small"
                            profile={{
                                data: patientData,
                                profilePhotoPath: profileData?.profilePhotoPath || patientData?.profilePhotoPath || "",
                                picture: profileData?.picture || profileData?.profilePhotoPath || patientData?.profilePhotoPath || "",
                                coloredName: profileData?.coloredName || (patientData ? `${patientData.firstName?.charAt(0) || 'P'}` : 'P'),
                                colorForDefaultName: profileData?.colorForDefaultName || "#e6e4ef",
                            }}
                            disableBackendLoading={false}
                        />
                    </div>
                    <div className="profile-info-full-width">
                        <div className="fw-semibold page-name" style={{ fontSize: "18px", marginBottom: "8px" }}>
                            {pageName}
                        </div>
                        {progressData !== undefined && (
                            <div className="progress-container-full-width">
                                <div className="progress-bar-wrapper-full-width">
                                    <ProfileProgress
                                        customStyles={{
                                            background:
                                                "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                                            borderRadius: "2px",
                                            height: "2px",
                                            width: "100%",
                                            pointerEvents: "none",
                                        }}
                                        progress={progressData}
                                    />
                                    <span
                                        className="update-profile"
                                        onClick={() => smartNavigate("/profileDetails", {
                                            state: {
                                                data: {
                                                    data: patientData,
                                                    progress: progressData,
                                                    profilePhotoPath: profileData?.profilePhotoPath,
                                                    picture: profileData?.picture
                                                },
                                                coloredName: profileData?.coloredName,
                                                colorForDefaultName: profileData?.colorForDefaultName,
                                                currentProgress: progressData,
                                                source: 'PageTitle'
                                            }
                                        })}
                                    >
                                        Update Profile
                                    </span>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            ) : (
                <div
                    className={`text-center ${!switchButton ? 'w-100' : 'position-absolute start-50 translate-middle-x'}`}
                    style={{ pointerEvents: "none" }}
                >
                    <div
                        className="fw-semibold page-name"
                        style={{
                            fontSize: smallText
                                ? width < 380
                                    ? "14px"
                                    : width < 576
                                        ? "16px"
                                        : "18px"
                                : switchButton && backButton
                                    ? "20px"
                                    : pageName.length > 10 && width < 380
                                        ? "18px"
                                        : "16px",
                            ...(switchButton && backButton
                                ? { marginLeft: "-18px" }
                                : {}),
                        }}
                    >
                        {pageName}
                    </div>
                </div>
            )}

            {switchButton && (
                <div className={`switch-button-container ${showProfilePicture && isSinglePatientView ? 'match-profile-height' : ''}`}>
                    {/* <SwitchPatientToggle
                        pageName={pageName}
                        onClick={() => smartNavigate("/switch-patient")}
                    /> */}
                    <div className="switch-patient-toggle-wrapper" onClick={() => smartNavigate("/switch-patient")}>

                        <label
                            className="switch-patient-toggle-container"
                            htmlFor="pin-toggle-SwitchPatientToggle"
                            style={{ 
                                border: "1.5px solid #4b3b8b", 
                                background: 'rgb(84, 71, 138)', 
                                cursor: 'pointer',
                                overflow: 'hidden',
                                ...(isSinglePatientView ? {
                                    width: 'clamp(90px, 18vw, 100px)',
                                    height: 'clamp(32px, 7vw, 36px)'
                                } : {})
                            }}
                        >
                            <div className="switch-patient-toggle-square"
                                style={{
                                    border: "1.5px solid #4b3b8b",
                                    borderRight: "none",
                                    borderTopRightRadius: 0,
                                    borderBottomRightRadius: 0,
                                    background: 'rgb(84, 71, 138)',
                                    ...(isSinglePatientView ? {
                                        width: 'clamp(30px, 7vw, 35px)',
                                        height: 'clamp(30px, 7vw, 35px)'
                                    } : {})
                                }}
                            >
                                <div className="switch-patient-toggle-icon">
                                    <img 
                                        src={SwitchImg} 
                                        alt="Switch Icon"
                                        style={isSinglePatientView ? {
                                            width: 'clamp(16px, 4vw, 20px)',
                                            height: 'clamp(16px, 4vw, 20px)'
                                        } : {}}
                                    />
                                </div>
                            </div>
                            <div 
                                className="d-flex justify-content-between switch-patient-label-container"
                                style={isSinglePatientView ? {
                                    width: 'auto',
                                    minWidth: 0,
                                    flex: 1,
                                    paddingLeft: '3px',
                                    paddingRight: '3px',
                                    alignItems: 'center',
                                    justifyContent: 'center'
                                } : {}}
                            >
                                <div className={`switch-patient-label-left ${isSinglePatientView ? 'single-patient-label-override' : ''}`}></div>
                                <div 
                                    className={`switch-patient-label-right ${isSinglePatientView ? 'single-patient-label-override' : ''}`}
                                    style={{ 
                                        color: "#ffffff",
                                        ...(isSinglePatientView ? {
                                            fontSize: 'clamp(10px, 1.5vw, 10px)',
                                            lineHeight: 'clamp(12px, 2.2vw, 12px)',
                                            marginLeft: '0px'
                                        } : {})
                                    }}
                                >
                                    Switch Patient
                                </div>
                            </div>
                        </label>
                    </div>
                </div>
            )}
        </div>
    );
};

export default PageTitle;
