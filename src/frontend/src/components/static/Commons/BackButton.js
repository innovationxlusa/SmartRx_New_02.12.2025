const BackButton = ({ pageName, icon, iconPosition = "left", onBack, color = "#fff" }) => {
    return (
        <div className="d-flex align-items-center gap-2">
            <button onClick={onBack} className="d-flex ps-0">
                {iconPosition === "left" &&
                    icon &&
                    (typeof icon === "string" ? (
                        <img src={icon} alt="icon" style={{ width: "12px", height: "22px", color: color }} />
                    ) : (
                        <span className="me-2" style={{ fontSize: "20px", color: color }}>
                            {icon}
                        </span> // React icon component
                    ))}
                {iconPosition === "right" &&
                    icon &&
                    (typeof icon === "string" ? (
                        <img src={icon} alt="icon" className="ms-2" style={{ width: "20px", height: "20px" }} />
                    ) : (
                        <span className="ms-2" style={{ fontSize: "20px" }}>
                            {icon}
                        </span>
                    ))}
            </button>
        </div>
    );
};

export default BackButton;
