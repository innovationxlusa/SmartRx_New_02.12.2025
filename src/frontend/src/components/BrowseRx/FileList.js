import { useEffect, useState } from "react";
import { detectEnvironment } from "../../utils/envHelper";
import "./FileList.css";
import { IoMdMove } from "react-icons/io";
import TagFileModal from "./TagFileModal";
import MoveFileModal from "./MoveFileModal";
import { ImPriceTags } from "react-icons/im";
import { HiOutlineShare } from "react-icons/hi";
import { Link, useNavigate } from "react-router-dom";
import useApiClients from "../../services/useApiClients";
import CustomButton from "../static/Commons/CustomButton";
import ContextMenu from "../static/ContextMenu/ContextMenu";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import FileManagementModal from "../FileUpload/FileManagementModal";
import { FaCloudArrowDown, FaCopy, FaTrash } from "react-icons/fa6";
import ImagePreviewModal from "../ImagePreviewModal/ImagePreviewModal";
import { IMAGE_HOST } from "../../config/config";
import { getRxColor, getRxText } from "../../utils/utils";
import RewardPopup from "../../components/Reward/RewardPopup";
import ConsumePopup from "../../components/Reward/ConsumePopup";

function getTypeColor(item) {
    const file = item?.filePathList?.find((p) =>
        /_(large|original)\./i.test(p),
    );
    if (file?.endsWith(".pdf")) return "#FF0004";
    return "#008000";
}

function getTypeText(item) {
    const file = item?.filePathList?.find((p) =>
        /_(large|original)\./i.test(p),
    );
    if (file?.endsWith(".pdf")) return "PDF";
    return "IMG";
}

const FileList = ({
    item,
    index,
    expandedIndex,
    setExpandedIndex,
    refetch,
    foldersList,
}) => {
    const { env, hostname, apiPort } = detectEnvironment();
    const { api } = useApiClients();
    const currentUserId = useCurrentUserId();
    const navigate = useNavigate();
    const [folderId, setFolderId] = useState("");
    const [modalType, setModalType] = useState(null);
    const [imageFile, setImageFile] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [prescriptionId, setPrescriptionId] = useState("");
    const [isImageBroken, setIsImageBroken] = useState(false);
    const [selectedFile, setSelectedFile] = useState(null);
    const IMAGE_URL = IMAGE_HOST;

    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [showConsumePopup, setShowConsumePopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    const toggleExpand = (index) => {
        setExpandedIndex((prev) => (prev === index ? null : index));
    };

    const handleOpenModal = (value, modalType) => {
        if (value) {
            setImageFile(value); // Used for preview
            setPrescriptionId(value?.fileId); // Store selected file (fix)
            setFolderId(value?.folderId); // Store selected file (fix)
            setSelectedFile(value); // Store selected file for download
        }
        setTimeout(() => openModal(modalType), 300);
    };

    const handleDownloadPrescription = async (item) => {
        setUploadResponse(null);
        setShowRewardPopup(false);

        const suggestedName =
            item?.fileName || `prescription-${item?.fileId || ""}`;

        try {
            const responseDownload = await api.downloadPrescription(
                item?.fileId,
                suggestedName,
            );

            const headers = responseDownload?.headers || {};
            const rewardMessage = headers["x-reward-message"] || "";
            const rewardPoints = headers["x-reward-points"] || "0";
            const rewardUpdated = headers["x-reward-updated"] || "false";
            const rewardTitle = headers["x-reward-title"] || "";

            const isRewardUpdated = rewardUpdated.toLowerCase() === "true";
            const totalRewardPoints = parseInt(rewardPoints, 10) || 0;
            const responseData = {
                response: {
                    isRewardUpdated,
                    totalRewardPoints,
                    rewardMessage: rewardMessage || rewardTitle || "",
                },
            };

            setUploadResponse(responseData);

            if (isRewardUpdated) {
                setShowRewardPopup(true);
                setTimeout(() => {
                    setShowRewardPopup(false);
                    refetch();
                }, 3000);
            } else {
                refetch();
            }
        } catch (e) {
            console.error(e);
        }
    };

    const handleRequestRx = async (e, item) => {
        e.preventDefault();

        if (!item) return;

        try {
            setUploadResponse(null);
            setShowRewardPopup(false);

            setIsLoading(true);

            const newData = {
                PrescriptionId: item?.fileId,
                UpdatedBy: currentUserId ?? 0,
            };
            // API call to assign roles to the selected user
            const response = await api.requestForSmartRxUpload(
                newData,
                item?.fileId,
                "",
            );

            setUploadResponse(response);

            // If API call is successful or returns an object, reset the form
            if (
                response.message === "Successful" ||
                typeof response === "object"
            ) {
                if (
                    response?.response?.isRewardUpdated == true &&
                    response?.response?.totalRewardPoints > 0 &&
                    response?.response?.requestedForSmartRx != true
                ) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        refetch();
                    }, 3000);
                } else if (
                    response?.response?.isRewardUpdated == true &&
                    response?.response?.requestedForSmartRx == true
                ) {
                    setShowConsumePopup(true);
                    setTimeout(() => {
                        setShowConsumePopup(false);
                        refetch();
                    }, 3000);
                } else {
                    refetch();
                }
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false); // Reset loading state
        }
    };

    const moreMenuItems = [
        {
            label: "Move",
            icon: <IoMdMove />,
            action: () => handleOpenModal(item, "move"),
        },
        {
            label: "Tag",
            icon: <ImPriceTags />,
            action: () => handleOpenModal(item, "tag"),
        },
        {
            label: "Download",
            icon: <FaCloudArrowDown />,
            action: () => handleDownloadPrescription(item),
        },
        {
            label: "Delete",
            icon: <FaTrash />,
            action: () => handleOpenModal(item, "delete"),
        },
    ];

    return (
        <div
            style={{
                borderBottom: "1px solid rgba(69, 108, 139, 0.5)",
                padding: "5px 0",
            }}
        >
            <div
                className={expandedIndex === index ? "expanded px-3" : "px-3"}
                onClick={() => toggleExpand(index)}
            >
                <div className="py-1 d-flex justify-content-between align-items-center gap-1 py-1">
                    <div
                        className="p-1 p-md-2 "
                        style={{
                            border: "1px solid #EEF2FF",
                            borderRadius: "10px",
                            backgroundColor: "#EEF2FF",
                            aspectRatio: "1 / 1",
                            width: "140px",
                            fontFamily: "Georama",
                            color: "#65636e",
                        }}
                    >
                        <div className="d-flex justify-content-between align-items-center pt-0">
                            <div
                                style={{
                                    borderRadius: "4px",
                                    backgroundColor: getTypeColor(item),
                                    color: "white",
                                    display: "inline-block",
                                    padding: "0 4px",
                                    fontWeight: "bold",
                                    fontSize: "clamp(12px, 2vw, 16px)",
                                }}
                            >
                                {getTypeText(item)}
                            </div>
                            <div
                                className="truncate-filename"
                                title={item.fileName || "Unknown file"}
                                style={{
                                    maxWidth: "70%",
                                    whiteSpace: "nowrap",
                                    overflow: "hidden",
                                    textOverflow: "ellipsis",
                                    fontSize: "clamp(10px, 1.8vw, 14px)",
                                    lineHeight: 1.2,
                                }}
                            >
                                {item.fileName || "Unknown"}
                            </div>
                        </div>
                        <div
                            className="mt-1 my-auto"
                            style={{ position: "relative" }}
                        >
                            <img
                                src={`${IMAGE_URL}/${item?.filePath}`}
                                alt="Description"
                                onError={() => setIsImageBroken(true)}
                                style={{
                                    width: "100%",
                                    borderRadius: "8px",
                                    objectFit: "fill",
                                    aspectRatio: "1 / .85",
                                    // filter: getRxText(item) === "RX Only" || getRxText(item) === "Pending" ? "blur(2px)" : "",
                                }}
                            />

                            {/* Overlay for light blur and button */}
                            {expandedIndex === index && (
                                <div
                                    style={{
                                        position: "absolute",
                                        top: isImageBroken ? "100%" : "0",
                                        left: 0,
                                        width: "100%",
                                        height: "100%",
                                        borderRadius: "12px",
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center",
                                    }}
                                >
                                    <CustomButton
                                        isLoading={""}
                                        type="button"
                                        label="VIEW"
                                        disabled={false}
                                        width="clamp(50px, 20vw, 80px)"
                                        height="clamp(30px, 2.3vw, 40px)"
                                        backgroundColor=""
                                        textColor="var(--theme-font-color)"
                                        shape="roundedSquare"
                                        borderStyle={""}
                                        borderColor="2px solid var(--content-color)"
                                        iconStyle={{
                                            color: "var(--theme-font-color)",
                                        }}
                                        labelStyle={{
                                            fontSize: "clamp(14px, 2vw, 16px)",
                                            fontWeight: "800",
                                            fontFamily: "Source Sans Pro",
                                            textTransform: "capitalize",
                                        }}
                                        hoverEffect="theme"
                                        onClick={(e) => {
                                            e.stopPropagation(); // ⛔ Prevent parent toggleExpand from firing
                                            e.nativeEvent.stopImmediatePropagation();
                                            handleOpenModal(
                                                item.filePathList,
                                                "preview",
                                            );
                                        }}
                                    />
                                </div>
                            )}
                        </div>
                    </div>
                    <div className="d-flex justify-content-center align-items-center gap-1 mb-1">
                        <CustomButton
                            isLoading={
                                getRxText(item) === "Smart RX"
                                    ? ""
                                    : getRxText(item) === "Pending"
                                      ? isLoading
                                      : isLoading
                            }
                            type="button"
                            label={
                                getRxText(item) === "Smart RX"
                                    ? "VIEW RX"
                                    : getRxText(item) === "Pending"
                                      ? "REQUEST SRX"
                                      : "REQUEST SRX"
                            }
                            disabled={
                                getRxText(item) === "Smart RX"
                                    ? false
                                    : getRxText(item) === "Pending"
                                      ? true
                                      : false
                            }
                            width="clamp(50px, 20vw, 150px)"
                            height="clamp(30px, 2.3vw, 40px)"
                            backgroundColor={
                                getRxText(item) === "Smart RX"
                                    ? ""
                                    : getRxText(item) === "Pending"
                                      ? "#65636E"
                                      : "var(--app-theme-color)"
                            }
                            textColor={
                                getRxText(item) === "Smart RX"
                                    ? "var(--theme-font-color)"
                                    : getRxText(item) === "Pending"
                                      ? "var(--text-white)"
                                      : "var(--text-white)"
                            }
                            shape=""
                            borderStyle={"16px 0 0 16px"}
                            borderColor={
                                getRxText(item) === "Smart RX"
                                    ? "2px solid var(--theme-font-color)"
                                    : getRxText(item) === "Pending"
                                      ? "2px solid #65636E"
                                      : "2px solid var(--theme-font-color)"
                            }
                            iconStyle={{ color: "var(--theme-font-color)" }}
                            labelStyle={{
                                fontSize: "clamp(8px, 2vw, 16px)",
                                fontWeight: "400",
                                fontFamily: "Source Sans Pro",
                                textTransform: "capitalize",
                            }}
                            hoverEffect={
                                getRxText(item) === "Smart RX"
                                    ? "theme"
                                    : getRxText(item) === "Pending"
                                      ? "light"
                                      : "light"
                            }
                            onClick={(e) => {
                                e.stopPropagation(); // ✅ Prevent toggleExpand
                                getRxText(item) === "Smart RX"
                                    ? navigate("/smartRxInsider", {
                                          state: {
                                              fileId: item.fileId,
                                              smartRxMasterId:
                                                  item.smartRxMasterId,
                                          },
                                      })
                                    : getRxText(item) === "Pending"
                                      ? handleRequestRx(e, item)
                                      : handleRequestRx(e, item);
                            }}
                        />
                        <CustomButton
                            isLoading={""}
                            type="button"
                            icon={<HiOutlineShare />}
                            label=""
                            disabled={
                                getRxText(item) === "Smart RX"
                                    ? false
                                    : getRxText(item) === "Pending"
                                      ? true
                                      : false
                            }
                            width="clamp(30px, 2.3vw, 35px)"
                            height="clamp(30px, 2.3vw, 40px)"
                            backgroundColor=""
                            textColor={
                                getRxText(item) === "Smart RX"
                                    ? "var(--theme-font-color)"
                                    : getRxText(item) === "Pending"
                                      ? "#65636E"
                                      : "var(--theme-font-color)"
                            }
                            shape=""
                            borderStyle={"0 16px 16px 0"}
                            borderColor={
                                getRxText(item) === "Smart RX"
                                    ? "2px solid var(--theme-font-color)"
                                    : getRxText(item) === "Pending"
                                      ? "2px solid #65636E"
                                      : "2px solid var(--theme-font-color)"
                            }
                            iconStyle={{
                                color:
                                    getRxText(item) === "Smart RX"
                                        ? "var(--theme-font-color)"
                                        : getRxText(item) === "Pending"
                                          ? "#65636E"
                                          : "var(--theme-font-color)",
                                fontSize: "14px !important",
                                marginRight: "5px",
                            }}
                            labelStyle={{}}
                            hoverEffect="theme"
                            onClick={(e) => {
                                e.stopPropagation(); // ✅ Prevent toggleExpand
                            }}
                        />
                    </div>
                    <div>
                        <div
                            style={{
                                borderRadius: "4px",
                                backgroundColor: getRxColor(item),
                                display: "inline-block",
                                padding: "4px",
                                color: "white",
                                fontWeight: "bold",
                                fontSize: "clamp(12px, 2vw, 16px)",
                                width: "100%",
                                textAlign: "center",
                                whiteSpace: "nowrap",
                                overflow: "hidden",
                                textOverflow: "ellipsis",
                            }}
                        >
                            {getRxText(item)}
                        </div>
                        <div
                            className="p-0"
                            style={{
                                fontSize: "clamp(12px, 2vw, 16px)",
                                marginTop: "8px",
                                whiteSpace: "nowrap",
                                overflow: "hidden",
                                textOverflow: "ellipsis",
                            }}
                        >
                            {item.createdDateStr}
                        </div>
                    </div>
                </div>
                <div
                    className={`button-group-wrapper d-flex align-items-center justify-content-between w-100 px-1 ${expandedIndex === index ? "show mt-0" : ""}`}
                >
                    <CustomButton
                        isLoading={isLoading}
                        type="button"
                        label={
                            getRxText(item) === "Smart RX"
                                ? "VIEW RX"
                                : "REQUEST SMARTRX"
                        }
                        disabled={getRxText(item) === "Pending" || isLoading}
                        height="25px"
                        width="clamp(140px, 16vw, 160px)"
                        backgroundColor=""
                        textColor={
                            getRxText(item) === "Pending"
                                ? "var(--content-color)"
                                : "var(--theme-font-color)"
                        }
                        shape="rounded"
                        borderStyle=""
                        borderColor={
                            getRxText(item) === "Pending"
                                ? "2px solid var(--content-color)"
                                : "2px solid var(--theme-font-color)"
                        }
                        iconStyle={{ color: "var(--theme-font-color)" }}
                        labelStyle={{
                            fontSize: "clamp(11px, 1.4vw, 14px)",
                            fontWeight: "1000",
                            fontFamily: "Georama",
                            textTransform: "capitalize",
                        }}
                        hoverEffect="theme"
                        zIndex={"5"}
                        onClick={(e) => {
                            e.stopPropagation(); // ⛔ Prevent parent toggleExpand from firing
                            if (getRxText(item) === "Smart RX") {
                                navigate("/smartRxInsider", {
                                    state: {
                                        fileId: item.fileId,
                                        smartRxMasterId: item.smartRxMasterId,
                                    },
                                });
                            } else {
                                handleRequestRx(e, item);
                            }
                        }}
                    />

                    <ContextMenu
                        button={
                            <CustomButton
                                isLoading={""}
                                type="button"
                                label="MORE"
                                disabled={
                                    getRxText(item) === "Pending" || isLoading
                                }
                                height="25px"
                                width="clamp(140px, 16vw, 160px)"
                                backgroundColor=""
                                textColor={
                                    getRxText(item) === "Pending"
                                        ? "var(--content-color)"
                                        : "var(--theme-font-color)"
                                }
                                shape="pill"
                                borderStyle=""
                                borderColor={
                                    getRxText(item) === "Pending"
                                        ? "2px solid var(--content-color)"
                                        : "2px solid var(--theme-font-color)"
                                }
                                iconStyle={{ color: "var(--theme-font-color)" }}
                                labelStyle={{
                                    fontSize: "clamp(11px, 1.4vw, 14px)",
                                    fontWeight: "1000",
                                    fontFamily: "Georama",
                                    textTransform: "capitalize",
                                }}
                                hoverEffect="theme"
                            />
                        }
                        menuItems={moreMenuItems}
                    />
                </div>
            </div>
            <ImagePreviewModal
                onOpen={modalType === "preview"}
                onClose={closeModal}
                file={imageFile}
                fileType={getTypeText(item)}
            />
            <MoveFileModal
                onOpen={modalType === "move"}
                onClose={closeModal}
                foldersList={foldersList}
                prescriptionId={prescriptionId}
                refetch={refetch}
            />
            <TagFileModal
                onOpen={modalType === "tag"}
                onClose={closeModal}
                foldersList={foldersList}
                prescriptionId={prescriptionId}
                folderId={folderId}
                refetch={refetch}
                item={item}
            />
            <FileManagementModal
                modalType={modalType}
                isOpen={modalType === "delete"}
                fileData={selectedFile}
                onClose={closeModal}
                fetchFolders={refetch}
            />

            <RewardPopup
                isOpen={showRewardPopup}
                onClose={() => {
                    setShowRewardPopup(false);
                }}
                points={uploadResponse?.response?.totalRewardPoints || 0}
                message={uploadResponse?.response?.rewardMessage || ""}
            />

            <ConsumePopup
                isOpen={showConsumePopup}
                onClose={() => {
                    setShowConsumePopup(false);
                }}
                consumeAmount={uploadResponse?.response?.totalRewardPoints || 0}
                message={uploadResponse?.response?.rewardMessage || ""}
            />
        </div>
    );
};
export default FileList;
