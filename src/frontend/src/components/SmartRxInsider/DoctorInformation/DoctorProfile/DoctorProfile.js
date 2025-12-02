import "./DoctorProfile.css";
import { useLocation } from "react-router-dom";
import PageTitle from "../../../static/PageTitle/PageTitle";
import { useFetchData } from "../../../../hooks/useFetchData";
import useApiClients from "../../../../services/useApiClients";
import CustomButton from "../../../static/Commons/CustomButton";
import DoctorProfileShimmer from "./DoctorProfileShimmer/DoctorProfileShimmer";
import { ReactComponent as ClockIcon } from "../../../../assets/img/ClockIcon2.svg";
import { ReactComponent as AddressIcon } from "../../../../assets/img/AddressIcon.svg";
import { useRef, useState, useMemo } from "react";
import { getColorForName } from "../../../../constants/constants";


const DoctorProfileList = () => {

    const { api } = useApiClients();
    const { state } = useLocation();
    const viewMoreRef = useRef(null);
    const [imageError, setImageError] = useState(false);
    const scrollToSection = (sectionRef) => {
        const target = sectionRef?.current;
        if (target) {
            target.scrollIntoView({
                behavior: "smooth",
                block: "start",
                inline: "nearest",
            });
        }
    };

    const {
        data: doctor = {},
        isLoading,
        error,
        refetch,
    } = useFetchData(
        api.getDoctorDetailsById,
        0, // page
        0, // rowsPerPage
        null, // sortField
        null, // sortOrder
        { DoctorId: state?.doctorId } // fileId custom param for payload
    );

    const doctorFullName = useMemo(() => {
        if (!doctor) return "";
        const firstName = doctor.doctorFirstName || "";
        const lastName = doctor.doctorLastName || "";
        return `${firstName} ${lastName}`.trim();
    }, [doctor?.doctorFirstName, doctor?.doctorLastName]);

    const generateInitials = useMemo(() => {
        if (doctorFullName) {
            const initials = doctorFullName
                .split(" ")
                .map((n) => n[0] || "")
                .join("")
                .substring(0, 2)
                .toUpperCase();
            return initials || "D";
        }
        return "D";
    }, [doctorFullName]);

    const doctorBackgroundColor = useMemo(() => {
        if (doctorFullName) {
            return getColorForName(doctorFullName);
        }
        return "#e6e4ef";
    }, [doctorFullName]);

    const hasValidProfilePhoto = doctor?.profilePhotoPath && 
                                 doctor.profilePhotoPath.trim() !== "" && 
                                 !imageError;
    const profilePhotoUrl = useMemo(() => {
        if (!hasValidProfilePhoto) return null;
        const path = doctor.profilePhotoPath;
        // Check if it's already a full URL
        if (path.startsWith('http') || path.startsWith('blob:')) {
            return path;
        }
        const baseUrl = process.env.REACT_APP_IMAGE_URL || "";
        if (baseUrl) {
            const cleanPath = path.startsWith('/') ? path.substring(1) : path;
            const cleanBaseUrl = baseUrl.endsWith('/') ? baseUrl.slice(0, -1) : baseUrl;
            return `${cleanBaseUrl}/${cleanPath}`;
        }
        return path;
    }, [doctor?.profilePhotoPath, hasValidProfilePhoto]);

    return (
        <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto px-4 pb-4">
            {isLoading ? (<DoctorProfileShimmer />) : (
                <div className="content-container">
                    <PageTitle
                        pageName={"Doctor Profile"}
                        switchButton={false}
                    />
                    {/* Header Section */}
                    <div className="profile-header d-flex mt-4 pt-3">
                        <div className="profile-photo">
                            {hasValidProfilePhoto && profilePhotoUrl ? (
                                <img
                                    src={profilePhotoUrl}
                                    alt={doctor.doctorFirstName}
                                    className="img-fluid rounded-circle"
                                    onError={() => setImageError(true)}
                                    onLoad={() => setImageError(false)}
                                />
                            ) : (
                                <div
                                    className="rounded-circle d-flex align-items-center justify-content-center"
                                    style={{
                                        width: "100%",
                                        height: "100%",
                                        backgroundColor: doctorBackgroundColor,
                                        color: "#ffffff",
                                        fontSize: "1.5rem",
                                        fontWeight: "600",
                                        fontFamily: "Georama",
                                    }}
                                >
                                    {generateInitials}
                                </div>
                            )}
                        </div>
                        <div className="profile-info">
                            <span className="doctor-name">
                                {doctor.doctorTitle} {doctor.doctorFirstName} {doctor.doctorLastName}
                            </span>
                            <p className="doctor-degree">
                                {doctor?.doctorEducationDegrees?.map((edu) => edu.educationDegreeName).join(", ")}
                            </p>
                            <p className="doctor-specialization">
                                {doctor.doctorExperiences?.[0]?.doctorSpecialization}
                            </p>
                            <p className="doctor-meta">
                                {doctor.doctorYearOfExperiences} Years of Experience Overall
                                <br />
                                BMDC Reg: {doctor.doctorBMDCRegNo}
                                <br />
                                ID: {doctor.doctorCode}
                            </p>
                        </div>
                    </div>

                    {/* Serves For */}
                    <div className="serves-for mt-3">
                        <h6>Serves for:</h6>
                        <div className="tags-container">
                            {doctor.doctorExperiences?.[0]?.doctorSpecialization
                                .split(",")
                                .map((s, i) => (
                                    <span className="tag" key={i}>
                                        {s.trim()}
                                    </span>
                                ))}
                        </div>
                        <div className="view-more" onClick={() => scrollToSection(viewMoreRef)} >View more</div>
                    </div>


                    {/* About */}
                    <div className="about mt-3">
                        <h6>About</h6>
                        <p>{doctor.doctorProfessionalSummary}</p>
                    </div>

                    {/* Locations */}
                    <div className="locations mt-3">
                        <h6 className="mb-0">Locations</h6>
                        {doctor?.doctorChambers?.map((chamber, index) => (
                            <div key={index}>
                                <div className="chamber d-block d-md-flex gap-0 gap-md-4 my-4 my-md-2 py-2 py-md-4 px-0 px-md-4 responsive-border" >
                                    <div className="d-flex align-items-start mb-2">
                                        <AddressIcon className="me-2 address-icon" />
                                        <div>
                                            <p className="chamber-hospital">
                                                {chamber.hospitalName}
                                            </p>
                                            <p className="chamber-address">
                                                {chamber.chamberAddress}
                                                <br />
                                                {chamber.chamberCityName}
                                            </p>
                                        </div>
                                    </div>
                                    <div className="d-flex align-items-start">
                                        <ClockIcon className="me-2 clock-icon" />
                                        <div>
                                            <p className="availability">
                                                Availability
                                            </p>
                                            <p className="availability-time">{chamber.visitingHour}</p>
                                        </div>
                                    </div>
                                </div>
                                <div className="divider" />
                            </div>
                        ))}
                    </div>

                    {/* Field of Concentration */}
                    <div ref={viewMoreRef} className="field-of-concentration mt-3">
                        <h6>Field of Concentration</h6>
                        <ul>
                            {doctor.doctorExperiences?.[0]?.doctorSpecialization
                                .split(",")
                                .map((s, i) => (
                                    <li key={i}>
                                        {s.trim()}
                                    </li>
                                ))}
                        </ul>
                    </div>

                    {/*Specializations */}
                    <div className="specializations mt-3">
                        <h6>Specializations</h6>
                        <ul>
                            {doctor.doctorExperiences?.[0]?.doctorSpecialization
                                .split(",")
                                .map((s, i) => (
                                    <li key={i}>
                                        {s.trim()}
                                    </li>
                                ))}
                        </ul>
                    </div>

                    {/* Work Experience */}
                    <div className="experience mt-3">
                        <h6>Work Experiences</h6>
                        <ul>
                            <li>Head of Department - Cardiology - United Hospital Present</li>
                        </ul>
                    </div>

                    {/* Education */}
                    <div className="education mt-3">
                        <h6>Education</h6>
                        <ul>
                            {doctor?.doctorEducationDegrees?.map((edu) => (
                                <li key={edu.educationId}>
                                    {edu.educationDegreeName} - {edu.educationInstitutionName}
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            )}
        </div>
    );
};

export default DoctorProfileList;
