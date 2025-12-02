import { useRef, useEffect } from "react";
import ProfileButton from "../Commons/CommonButton";

const HorizontalScrollMenu = ({ customStyles, data, scrollToSection }) => {
    const scrollContainerRef = useRef(null);

    useEffect(() => {
        const container = scrollContainerRef.current;

        const handleWheel = (e) => {
            if (container && e.deltaY !== 0) {
                e.preventDefault();
                container.scrollLeft += e.deltaY;
            }
        };

        if (container) {
            container.addEventListener("wheel", handleWheel, {
                passive: false,
            });
        }

        return () => {
            if (container) {
                container.removeEventListener("wheel", handleWheel);
            }
        };
    }, []);

    const customLabels = {
        Favorites: "Favorites",
        DoctorsInformation: "Doctor's Information",
        PatientInformation: "Patient Information",
        ChiefComplaint: "Chief Complaint",
        PatientHistory: "Patient History",
        Vitals: "Vitals",
        Medicine: "Medicine",
        Investigation: "Investigation",
        Advice: "Advice",
        Overview: "Overview",
    };

    const formatSectionName = (key) => {
        return (
            customLabels[key] ||
            key
                .replace(/([A-Z])/g, " $1")
                .replace(/^./, (char) => char.toUpperCase())
                .trim()
        );
    };

    return (
        <div className="p-0 tab-scroll-container">
            <div className="overflow-hidden">
                <div
                    ref={scrollContainerRef}
                    // scrollbar-thin-custom
                    className="d-flex overflow-x-auto pe-0 py-0 rounded-lg scrollbar-hidden"
                    style={{
                        scrollBehavior: "smooth",
                        whiteSpace: "nowrap",
                        gap: "clamp(6px, 2vw, 10px)",
                        paddingLeft: "2px"
                    }}
                >
                    {Object.keys(data)?.map((item, index) => (
                        <div
                            key={index}
                            className="d-flex justify-content-center py-3 rounded-lg"
                            onClick={() => scrollToSection(item)}
                        >
                            <ProfileButton
                                text={formatSectionName(item)}
                                customStyles={{
                                    ...customStyles?.button,
                                    boxSizing: "border-box",
                                    display: "flex",
                                    flexDirection: "row",
                                    justifyContent: "center",
                                    alignItems: "center",
                                    width: "100%",
                                    height: "100%",
                                    paddingTop: "clamp(5px, 2vw, 10px)",
                                    paddingBottom: "clamp(5px, 2vw, 10px)",
                                    paddingLeft: "clamp(10px, 3vw, 20px)",
                                    paddingRight: "clamp(10px, 3vw, 20px)",
                                    borderRadius: "30px",
                                    backgroundColor: "#F8F8F8",
                                    boxShadow:
                                        "0px 4px 4px rgba(0, 0, 0, 0.25)",
                                    color: "#4d3d9c",
                                    fontFamily: "Georama",
                                    fontWeight: "bold",
                                    fontSize: "clamp(12px, 2vw, 16px)",
                                }}
                            />
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default HorizontalScrollMenu;
