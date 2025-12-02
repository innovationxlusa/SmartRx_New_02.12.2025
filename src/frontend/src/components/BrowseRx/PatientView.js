import { useState } from "react";
import CustomButton from "../static/Commons/CustomButton";

const PatientView = ({key, item, index, expandedIndex, setExpandedIndex, refetch, onClick, onRenameClick, onDeleteClick, foldersList }) => {
    // const toggleExpand = () => {
    //     setExpandedIndex((prev) => (prev === index ? null : index));
    // };

    const [deleteError, setDeleteError] = useState(null);

    const toggleExpand = () => {
        setExpandedIndex((prev) => (prev === index ? null : index));
        setDeleteError(null); // Clear error when toggling
    };

    const handleDelete = (e, item) => {
        e.stopPropagation();
        const hasChildren = item?.children?.length > 0 || item?.prescriptionList?.length > 0;

        if (hasChildren) {
            setDeleteError("Cannot delete: This folder contains files or subfolders.");
        } else {
            setDeleteError(null);
            onDeleteClick(item);
        }
    };
    return (
        <div
            style={{
                borderBottom: "1px solid rgba(69, 108, 139, 0.5)",
                padding: "5px 0",
            }}
        >
            <div
                className={expandedIndex === index ? "expanded" : ""}
                onClick={() => toggleExpand()}
            >
                <div className="px-3 py-1 d-flex justify-content-between align-items-center gap-1 p-1">
                    <div className="col-7">
                        <div
                            className="p-1 folder"
                            style={{
                                border: "1px solid #EEF2FF",
                                borderRadius: "12px",
                                backgroundColor: "#EEF2FF",
                                aspectRatio: "1 / 1",
                                width: "140px",
                                // cursor: "pointer",
                                fontFamily: "Georama"
                            }}
                            // onClick={(e) => {
                            //     e.stopPropagation();
                            //     if (onClick) {
                            //         onClick();
                            //     }
                            // }}
                        >
                            <div className="d-flex justify-content-between align-items-center">
                                <div
                                    className="col-12 text-center folder-text pt-1 truncate-filename"
                                    title={item.folderName}
                                >
                                    {item.folderName.length > 30
                                        ? item.folderName.slice(0, 30) + "..."
                                        : item.folderName}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="col-5 d-flex justify-content-end align-items-start">
                        <div>
                            <div
                                style={{
                                    borderRadius: "4px",
                                    backgroundColor: "#65636E",
                                    display: "inline-block",
                                    padding: "4px",
                                    color: "white",
                                    fontWeight: "bold",
                                    fontSize: "clamp(12px, 2vw, 16px)",
                                    width: "100%",
                                    textAlign: "center",
                                }}
                            >
                                {"RX Folder"}
                            </div>
                            <div
                                className="p-0"
                                style={{
                                    fontSize: "clamp(12px, 2vw, 16px)",
                                    margin: "8px 0",
                                }}
                            >
                                {item.createdDateStr}
                            </div>
                        </div>
                    </div>
                </div>

                {expandedIndex === index && deleteError && (
                    <div
                        className="px-3 text-danger"
                        style={{ fontSize: "13px", fontWeight: "500" }}
                    >
                        {deleteError}
                    </div>
                )}

                <div
                    className={`button-group-wrapper d-flex justify-content-between w-100 px-3 ${expandedIndex === index ? "show mt-0" : ""}`}
                >
                    <CustomButton
                        isLoading={""}
                        type="button"
                        label="View & Update"
                        onClick={(e) => {
                            e.stopPropagation();
                            if (onClick) {
                                onClick();
                            }
                        }}
                        disabled={false}
                        width="100px"
                        height="25px"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        // shape="pill"
                        borderStyle=""
                        borderColor="1px solid var(--theme-font-color)"
                        iconStyle={{ color: "var(--theme-font-color)" }}
                        labelStyle={{
                            fontSize: "13px",
                            fontWeight: "500",
                            fontFamily: "Source Sans Pro",
                            textTransform: "capitalize",
                        }}
                        hoverEffect="theme"
                    />
                    <CustomButton
                        isLoading={""}
                        type="button"
                        label="Browse RX"
                        onClick={(e) => {
                            e.stopPropagation();
                            onRenameClick(item);
                        }}
                        disabled={false}
                        width="100px"
                        height="25px"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        // shape="pill"
                        borderStyle=""
                        borderColor="1px solid var(--theme-font-color)"
                        iconStyle={{ color: "var(--theme-font-color)" }}
                        labelStyle={{
                            fontSize: "13px",
                            fontWeight: "500",
                            fontFamily: "Source Sans Pro",
                            textTransform: "capitalize",
                        }}
                        hoverEffect="theme"
                    />
                </div>
            </div>
        </div>
    );
};

export default PatientView;
