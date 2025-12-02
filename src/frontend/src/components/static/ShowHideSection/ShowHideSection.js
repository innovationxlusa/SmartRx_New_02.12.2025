import Card from "react-bootstrap/Card";
import ProfileButton from "../Commons/CommonButton";
import "./ShowHideSection.css";

const ShowHideSection = ({ customStyles, imageSrc, date, fileSize, buttons = [] }) => {
    return (
        <Card className="custom-card">
            <Card.Body>
                <div className="row">
                    <div className="col-sm-4 col-md-4 col-lg-4">
                        <Card.Img variant="top" src={imageSrc} className="custom-image" />
                    </div>
                    <div className="col-sm-8 col-md-8 col-lg-8">
                        <h5 className="custom-text-title">{date}</h5>
                        <b className="custom-text-desc">{fileSize}</b>
                    </div>
                </div>
                <br />
                <div className="row">
                    {buttons.map((button, index) => (
                        <div key={index} className="col-sm-4 col-md-4 col-lg-4">
                            <ProfileButton
                                text={button.text}
                                onClick={button.onClick}
                                customStyles={{
                                    ...customStyles?.button,
                                    justifyContent: "center",
                                    border: "1px solid #4B3B8B",
                                    borderRadius: "4px",
                                    fontWeight: "400",
                                    padding: "4px 6px",
                                    fontSize: "12px",
                                    backgroundColor: "#fff",
                                }}
                            />
                        </div>
                    ))}
                </div>
            </Card.Body>
        </Card>
    );
};

export default ShowHideSection;
