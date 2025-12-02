import { useState, useEffect } from "react";
import { FaNewspaper, FaArchway, FaGraduationCap, FaCog, FaSignInAlt, FaUserPlus, FaSignOutAlt, FaAngleRight, FaAngleDown } from "react-icons/fa";
import "./SidebarMenu.css";
import { MdFavoriteBorder } from "react-icons/md";
import { FaRegUser } from "react-icons/fa6";
import { ReactComponent as Wallet } from "../../../assets/img/Wallet.svg";
import { ReactComponent as Favorite } from "../../../assets/img/Favorite.svg";
import { ReactComponent as Privacy } from "../../../assets/img/Privacy.svg";
import { ReactComponent as Help } from "../../../assets/img/Help.svg";
import { ReactComponent as User } from "../../../assets/img/User.svg";
import { ReactComponent as Settings } from "../../../assets/img/Settings.svg";
import { VscSignOut } from "react-icons/vsc";

const Sidebar = ({ isOpen, toggleSidebar }) => {
    const [isSettingsOpen, setIsSettingsOpen] = useState(false);

    useEffect(() => {
        document.body.style.overflow = isOpen ? "hidden" : "auto";
        return () => {
            document.body.style.overflow = "auto";
        };
    }, [isOpen]);

    // Main menu items
    const menuItems = [
        { icon: <User />, text: "Profile", href: "#" },
        { icon: <Favorite />, text: "Favorite", href: "#" },
        { icon: <Wallet />, text: "Payment Method", href: "#" },
        { icon: <Privacy />, text: "Privacy Policy", href: "#" },
        { icon: <Help />, text: "Help", href: "#" },
        { icon: <Settings />, text: "Settings", href: "#" },
        // { icon: <VscSignOut />, text: "Log Out", href: "#" },
    ];

    // Settings dropdown items
    const settingsItems = [
        { icon: <FaSignInAlt />, text: "Login", href: "#" },
        { icon: <FaUserPlus />, text: "Register", href: "#" },
    ];

    return (
        <>
            {/* Sidebar Overlay */}
            <div
                className={`sidebar-overlay ${isOpen ? "show" : ""}`}
                onClick={toggleSidebar}
            />

            {/* Sidebar Container */}
            <div
                className={`sidebar-container ${isOpen ? "visible" : "hidden"}`}
            >
                <ul className="my-nav nav nav-pills flex-column mb-auto">
                    {/* Render main menu items */}
                    {menuItems.map((item, index) => (
                        <li key={index} className="nav-item mb-1">
                            <a href={item.href} className="nav-link">
                                <span className="nav-icon">{item.icon}</span>
                                <span
                                    className="nav-text"
                                    style={{ fontFamily: "Georama" }}
                                >
                                    {item.text}
                                </span>
                            </a>
                        </li>
                    ))}

                    {/* Settings Dropdown */}
                    {/* <li className="sidebar-item nav-item mb-1">
                        <a
                            href="#"
                            className={`nav-link ${isSettingsOpen ? "" : "collapsed"}`}
                            onClick={(e) => {
                                e.preventDefault();
                                setIsSettingsOpen(!isSettingsOpen);
                            }}
                        >
                            <span className="nav-icon">
                                <Settings />
                            </span>
                            <span className="nav-text">Settings</span>
                            {isSettingsOpen ? <FaAngleDown className="dropdown-icon" /> : <FaAngleRight className="dropdown-icon" />}
                        </a>
                        <ul className={`sidebar-dropdown list-unstyled ${isSettingsOpen ? "show" : "collapse"}`}>
                            {settingsItems.map((item, index) => (
                                <li key={index} className="sidebar-item">
                                    <a href={item.href} className="sidebar-link">
                                        <span className="nav-icon">{item.icon}</span>
                                        <span className="nav-text">{item.text}</span>
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </li> */}
                </ul>
            </div>
        </>
    );
};

export default Sidebar;
