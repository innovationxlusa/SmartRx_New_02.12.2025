import { useState } from "react";
import { useNavigate } from "react-router-dom";
import CustomButton from "../static/Commons/CustomButton";

const FolderView = ({
    item,
    index,
    expandedIndex,
    setExpandedIndex,
    refetch,
    onClick,
    onRenameClick,
    onDeleteClick,
    foldersList,
}) => {
    const navigate = useNavigate();
    const [deleteError, setDeleteError] = useState(null);

    const toggleExpand = () => {
        setExpandedIndex((prev) => (prev === index ? null : index));
        setDeleteError(null);
    };

    const handleDelete = (e, item) => {
        e.stopPropagation();
        const hasChildren =
            item?.children?.length > 0 || item?.prescriptionList?.length > 0;

        if (hasChildren) {
            setDeleteError(
                "Cannot delete: This folder contains files or subfolders.",
            );
        } else {
            setDeleteError(null);
            onDeleteClick(item);
        }
    };

    const handleViewClick = (e) => {
        e.stopPropagation();
        if (!item?.folderId) {
            console.error("No folderId found in item:", item);
            return;
        }

        const folderId = item.folderId;
        const folderName = item.folderOrFileName;
        const childrenObj = item.children;
        const childrenData = childrenObj?.data || [];
        const childrenTotal =
            typeof childrenObj?.totalRecords === "number"
                ? childrenObj.totalRecords
                : childrenData.length;

        if (childrenTotal > 0 && childrenData.length === 1) {
            const onlyChild = childrenData[0];
            const childIsFolder = !!onlyChild?.isFolder;
            const targetId = childIsFolder
                ? onlyChild?.folderId
                : onlyChild?.fileId;

            if (targetId) {
                navigate(`/browserx/folder/${folderId}`, {
                    state: {
                        folderId: folderId,
                        folderName: folderName,
                        childrenData: [onlyChild],
                        parentFolder: item,
                    },
                });
                return;
            }
        }
        navigate(`/browserx/folder/${folderId}`, {
            state: {
                folderId: folderId,
                folderName: folderName,
                childrenData: childrenData,
                parentFolder: item,
                parentFolderId: item?.parentFolderId ?? null,
            },
        });
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
                                transition: "all 0.2s ease",
                                fontFamily: "Georama"
                            }}
                            // onClick={(e) => {
                            //     e.stopPropagation();
                            //     if (onClick) {
                            //         onClick();
                            //     }
                            // }}
                            onMouseEnter={(e) => {
                                e.target.style.backgroundColor = "#E3F2FD";
                                e.target.style.transform = "scale(1.02)";
                            }}
                            onMouseLeave={(e) => {
                                e.target.style.backgroundColor = "#EEF2FF";
                                e.target.style.transform = "scale(1)";
                            }}
                        >
                            <div className="d-flex justify-content-between align-items-center">
                                <div
                                    className="col-12 text-center folder-text pt-1 truncate-filename"
                                    title={
                                        item.folderOrFileName ||
                                        "Unknown folder"
                                    }
                                >
                                    {(item.folderOrFileName || "Unknown")
                                        .length > 30
                                        ? (
                                              item.folderOrFileName || "Unknown"
                                          ).slice(0, 30) + "..."
                                        : item.folderOrFileName || "Unknown"}
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
                        label="VIEW"
                        onClick={handleViewClick}
                        disabled={false}
                        width="100px"
                        height="25px"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        shape="pill"
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
                        label="RENAME"
                        onClick={(e) => {
                            e.stopPropagation();
                            onRenameClick(item);
                        }}
                        disabled={false}
                        width="100px"
                        height="25px"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        shape="pill"
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
                        label="DELETE"
                        onClick={(e) => {
                            handleDelete(e, item);
                        }}
                        disabled={false}
                        width="100px"
                        height="25px"
                        backgroundColor=""
                        textColor="var(--theme-font-color)"
                        shape="pill"
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

export default FolderView;
