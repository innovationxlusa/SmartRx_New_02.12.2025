import { useState } from "react";
import "../SmartRxInsider/Medicine/Medicine.css";

const ReadMoreCard = ({ id, description }) => {
    const [isExpanded, setIsExpanded] = useState(false);

    const previewLength = 30;
    const isTruncated = description?.length > previewLength;
    const previewText = description?.slice(0, previewLength);

    return (
        <div className="medicine-card" id={id}>
            <div className="card-body">
                <div className="d-flex flex-column align-items-start">
                    <p
                        className="med-desc"
                        style={{
                            maxHeight: "70px",
                            height: isExpanded ? "none" : "100%",
                            overflowY: isExpanded ? "auto" : "auto",
                            scrollbarWidth: "none", // hide scrollbar for Firefox
                            msOverflowStyle: "none", // hide scrollbar for IE/Edge
                        }}
                    >
                        {isExpanded || !isTruncated
                            ? description
                            : previewText + " ..."}

                        {isTruncated && (
                            <span
                                className="read-inline"
                                tabIndex="0"
                                style={{
                                    color: "#4b3b8b",
                                    cursor: "pointer",
                                    marginTop: "4px",
                                    fontWeight: "500",
                                }}
                                onClick={(e) => {
                                    e.stopPropagation();
                                    setIsExpanded((prev) => !prev);
                                }}
                            >
                                {isExpanded ? " Show Less" : " Read More"}
                            </span>
                        )}
                    </p>
                </div>
            </div>
        </div>
    );
};

export default ReadMoreCard;
