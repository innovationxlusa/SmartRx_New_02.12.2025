import React from "react";

const ProfileProgress = ({ progress = 0, customStyles = {} }) => {
    return (
        <div
            style={{
                width: "100%",
                maxWidth: "600px",
                position: "relative",
                ...customStyles.container,
            }}
        >
            {/* Progress bar container */}
            <div
                style={{
                    width: "100%",
                    height: "4px",
                    top: "-10px",
                    background: "#e0e0e0",
                    borderRadius: "39.0368px",
                    position: "relative",
                    overflow: "visible",
                }}
            >
                {/* Filled progress bar */}
                <div
                    style={{
                        width: `${progress}%`,
                        height: "100%",

                        background:
                            "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                        borderRadius: "39.0368px",
                        transition: "width 0.3s ease-in-out",
                    }}
                />

                {/* Floating percentage box */}
                <div
                    className="progress-percentage"
                    style={{
                        position: "absolute",
                        top: "5px",
                        left: `calc(${progress}%)`,
                        transform: "translateX(-50%)",
                        transition: "left 0.3s ease-in-out",
                        backgroundColor: "#4b3b8b",
                        color: "white",
                        padding: "2px 3px",
                        borderRadius: "5px",
                        fontSize: "8px",
                        fontFamily: "Georama",
                        whiteSpace: "nowrap",
                    }}
                >
                    {progress}%
                </div>
            </div>
        </div>
    );
};

export default ProfileProgress;
