import React, { useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { FaEdit } from "react-icons/fa";
import { FaCopy, FaTrash } from "react-icons/fa6";
import FolderIcon from "../../../assets/img/FolderIcon.svg";
import IconContainer1 from "../../../assets/img/IconContainer1.svg";
import IconContainer2 from "../../../assets/img/IconContainer2.svg";
import IconContainer3 from "../../../assets/img/IconContainer3.svg";
import "./CommonTable.css";
import CustomButton from "../Commons/CustomButton";
import ContextMenu from "../ContextMenu/ContextMenu";
import FolderManagementModal from "../../BrowseRx/FolderManagementModal";
import ImagePreviewModal from "../../ImagePreviewModal/ImagePreviewModal";

const tableData = [
    { name: "Father", date: "20-10-2025", size: "25kB", icon: FolderIcon },
    { name: "Liton", date: "20-10-2025", size: "25kB", icon: FolderIcon },
    {
        name: "Harun.jpg",
        date: "20-10-2025",
        size: "25kB",
        icon: IconContainer1,
        badge: "smart rx",
        badgeColor: "bg-success",
    },
    {
        name: "Shobuj.jpg",
        date: "20-10-2025",
        size: "25kB",
        icon: IconContainer2,
        badge: "waiting",
        badgeColor: "bg-warning",
    },
    {
        name: "Joty.jpg",
        date: "20-10-2025",
        size: "25kB",
        icon: IconContainer1,
        badge: "smart rx",
        badgeColor: "bg-success",
    },
    {
        name: "Faruk.jpg",
        date: "20-10-2025",
        size: "25kB",
        icon: IconContainer3,
        badge: "rx only",
        badgeColor: "bg-danger",
    },
    {
        name: "123124f.jpg",
        date: "20-10-2025",
        size: "25kB",
        icon: IconContainer3,
        badge: "rx only",
        badgeColor: "bg-danger",
    },
];

const menuItems = [
    {
        label: "Edit",
        icon: <FaEdit />,
        action: () => handleOpenModal(setSelected, setModalType)(null, "edit"),
    },
    { label: "Copy", icon: <FaCopy />, action: () => alert("Copy clicked") },
    {
        label: "Delete",
        icon: <FaTrash />,
        action: () => handleOpenModal(setSelected, setModalType)(null, "delete"),
    },
];

const CommonTable = ({ data, isFolder, isFile }) => {
    if (!data || data.length === 0) return <p>No data available</p>;

    const [expandedIndex, setExpandedIndex] = useState(null);
    const [modalType, setModalType] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [imageFIle, setImageFile] = useState(null);

    const navigate = useNavigate();

    const toggleExpand = (index) => {
        setExpandedIndex((prev) => (prev === index ? null : index));
    };
    const convertDateFormat = (inputDate) => {
        if (!inputDate) return "";

        const [year, month, day] = inputDate.split("-");
        return `${day}-${month}-${year}`;
    };

    return (
        <table className="table text-start common-table">
            <thead></thead>
            <tbody>
                {data.map((row, index) => (
                    <tr key={index} className={`table-row py-2  ${expandedIndex === index ? "" : ""}`}>
                        <td colSpan={2}>
                            <div className={expandedIndex === index ? "expanded" : ""}>
                                <div className="px-2">
                                    <div className="d-flex justify-content-between align-items-center flex-wrap" style={{ cursor: "pointer" }} onClick={() => toggleExpand(index)}>
                                        <div className="col-sm-4 d-flex align-items-center gap-2">
                                            <span className="position-relative image-icon d-flex align-items-center">
                                                {row.isFolder && <img src={FolderIcon} alt="Icon" />}
                                                {row.isFolder && (
                                                    <span
                                                        className={`badge rounded-pill ${row.badgeColor} text-uppercase px-2 mt-1 position-absolute`}
                                                        style={{
                                                            fontSize: "0.7rem",
                                                            width: "fit-content",
                                                            whiteSpace: "nowrap",
                                                            left: "55%",
                                                            top: "-15%",
                                                        }}
                                                    >
                                                        {row.badge}
                                                    </span>
                                                )}
                                            </span>
                                            <span className="image-name">{row.name}</span>
                                        </div>
                                        <div className="col-sm-4"></div>
                                        <div className="col-sm-4 d-flex flex-column ">
                                            {row.isFolder && <span className="folder-weight">RX Folder</span>} <br />
                                            {row.isFolder && <span className="file-create-date">{row.createdDateStr}</span>}
                                        </div>
                                    </div>

                                    {/* Animated button group inside this td */}
                                    {/* <div className={`button-group-wrapper px-2 ${expandedIndex === index ? "show" : ""}`}>
                                        <button className="btn">View</button>
                                        <button className="btn">Preview</button>
                                        <button className="btn">More</button>
                                    </div> */}
                                    <div className={`button-group-wrapper d-flex justify-content-between w-100 px-3 ${expandedIndex === index ? "show mt-4" : ""}`}>
                                        <ContextMenu
                                            button={
                                                <CustomButton
                                                    isLoading={""}
                                                    type="button"
                                                    label="View Rx"
                                                    disabled={false}
                                                    width="100px"
                                                    backgroundColor=""
                                                    textColor="var(--theme-font-color)"
                                                    shape="roundedSquare"
                                                    borderStyle=""
                                                    borderColor="1px solid var(--theme-font-color)"
                                                    iconStyle={{ color: "var(--theme-font-color)" }}
                                                    labelStyle={{
                                                        fontSize: "16px",
                                                        fontWeight: "400",
                                                        fontFamily: "Source Sans Pro",
                                                        textTransform: "capitalize",
                                                    }}
                                                    hoverEffect="theme"
                                                />
                                            }
                                            menuItems={menuItems}
                                        />
                                        <ContextMenu
                                            button={
                                                <CustomButton
                                                    isLoading={""}
                                                    type="button"
                                                    label="Preview"
                                                    disabled={false}
                                                    width="100px"
                                                    backgroundColor=""
                                                    textColor="var(--theme-font-color)"
                                                    shape="roundedSquare"
                                                    borderStyle=""
                                                    borderColor="1px solid var(--theme-font-color)"
                                                    iconStyle={{ color: "var(--theme-font-color)" }}
                                                    labelStyle={{
                                                        fontSize: "16px",
                                                        fontWeight: "400",
                                                        fontFamily: "Source Sans Pro",
                                                        textTransform: "capitalize",
                                                    }}
                                                    hoverEffect="theme"
                                                />
                                            }
                                            menuItems={menuItems}
                                        />
                                        <ContextMenu
                                            button={
                                                <CustomButton
                                                    isLoading={""}
                                                    type="button"
                                                    label="More"
                                                    disabled={false}
                                                    width="100px"
                                                    backgroundColor=""
                                                    textColor="var(--theme-font-color)"
                                                    shape="roundedSquare"
                                                    borderStyle=""
                                                    borderColor="1px solid var(--theme-font-color)"
                                                    iconStyle={{ color: "var(--theme-font-color)" }}
                                                    labelStyle={{
                                                        fontSize: "16px",
                                                        fontWeight: "400",
                                                        fontFamily: "Source Sans Pro",
                                                        textTransform: "capitalize",
                                                    }}
                                                    hoverEffect="theme"
                                                />
                                            }
                                            menuItems={menuItems}
                                        />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default CommonTable;
