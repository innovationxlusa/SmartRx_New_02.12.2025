import React from "react";
import "./LandingFooterSection.css";
import ProfileButton from "../Commons/CommonButton";

const LandingFooterSection = () => {
    return (
        <div className="d-flex justify-content-center">
            <div className="me-3">
                <ProfileButton
                    text="Sign Up"
                    onClick={() => alert("Button Clicked!")}
                    customStyles={{
                        boxSizing: "border-box",
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: "center",
                        alignItems: "center",
                        padding: "18px",
                        border: "2px solid #CBC3E3",
                        borderRadius: "20px",
                        fontFamily: "Georama",
                        fontSize: "16px",
                        fontWeight: "700",
                        color: "#4B3B8B",
                        height: "60px",
                        width: "160px",
                        background: "#F4F3F8",
                    }}
                />
            </div>

            <div>
                <ProfileButton
                    text="Sign In"
                    onClick={() => alert("Button Clicked!")}
                    customStyles={{
                        boxSizing: "border-box",
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: "center",
                        alignItems: "center",
                        padding: "18px",
                        border: "2px solid #CBC3E3",
                        borderRadius: "20px",
                        fontFamily: "Georama",
                        fontSize: "16px",
                        fontWeight: "700",
                        color: "#4B3B8B",
                        height: "60px",
                        width: "160px",
                        background: "#F4F3F8",
                    }}
                />
            </div>
        </div>
    );
};

export default LandingFooterSection;
