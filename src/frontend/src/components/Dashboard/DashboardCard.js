const DashboardCard = ({ logo, title, count, bgColor, color, fontSize, titleLeftPosition, titleBottomPosition }) => {
    return (
        <div
            className="position-relative d-flex flex-column justify-content-center align-items-center w-100 h-100"
            style={{
                backgroundColor: bgColor,
                borderRadius: "18px",
            }}
        >
            <div
                className="d-flex justify-content-center align-items-center"
                style={{
                    width: "45%",
                    aspectRatio: "1 / 1",
                    backgroundColor: "#FFF",
                    borderRadius: "12px",
                }}
            >
                <img src={logo} style={{ maxWidth: "60%", height: "auto" }} />
            </div>

            <div
                className="position-absolute"
                style={{
                    bottom: titleBottomPosition,
                    left: titleLeftPosition,
                    color: color,
                    fontSize: fontSize,
                    fontWeight: "bold",
                }}
            >
                {title}
            </div>

            <div
                className="position-absolute text-white"
                style={{
                    height: "35px",
                    width: "35px",
                    top: "-8px",
                    right: "-8px",
                    backgroundColor: "#EC3762",
                    borderRadius: "50%",
                }}
            >
                <div className="position-relative w-100 h-100">
                    <span
                        style={{
                            position: "absolute",
                            top: "50%",
                            left: "50%",
                            transform: "translate(-50%, -50%)",
                        }}
                    >
                        {count}
                    </span>
                </div>
            </div>
        </div>
    );
};

export default DashboardCard;
