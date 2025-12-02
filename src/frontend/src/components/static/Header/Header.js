import React, { useState, useRef, useEffect, useCallback } from "react";
import "../Header/Header.css";
import Sidebar from "../Menu/SidebarMenu";
import { VscSignOut } from "react-icons/vsc";
import Logo from "../../../assets/img/RxLogo.png";
import MenuIcon from "../../../assets/img/Menu.svg";
import { Link, useNavigate, useLocation } from "react-router-dom";
import AlertIcon from "../../../assets/img/AlertIcon.svg";
import RewardIcon from "../../../assets/img/RewardIcon.svg";
import ProfileIcon from "../../../assets/img/ProfileIcon.svg";
import useApiServiceCall from "../../../hooks/useApiServiceCall";
import { useUserContext } from "../../../contexts/UserContext";
import useCurrentUserId from "../../../hooks/useCurrentUserId";

function Header() {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isProfileMenuVisible, setIsProfileMenuVisible] = useState(false);
    const profileMenuRef = useRef(null);
    const profileIconRef = useRef(null);
    const navigate = useNavigate();
    const { user } = useUserContext();
    const location = useLocation();


    const firstName = String(location.state?.FirstName ?? user?.FirstName ?? "");
    const lastName = String(location.state?.LastName ?? user?.LastName ?? "");
    const userCode= String(location.state?.UserCode ?? user?.UserCode ?? "");
    const cDateRaw = String(
        location.state?.CreatedDate ??
            location.state?.createdDate ??
            user?.CreatedDate ??
            user?.createdDate ??
            "",
    );
    const cDate = cDateRaw
        ? cDateRaw.includes("T")
            ? cDateRaw.split("T")[0]
            : cDateRaw.split(" ")[0]
        : "";

    // Destructuring api call function
    const { logout } = useApiServiceCall();

    const toggleSidebar = useCallback(() => {
        setIsSidebarOpen((prev) => !prev);
    }, []);

    const toggleProfileMenu = useCallback(
        (e) => {
            // Prevent event bubbling to avoid immediate close when clicking the icon
            e.stopPropagation();
            const newVisibility = !isProfileMenuVisible;
            setIsProfileMenuVisible(newVisibility);

            // Adjust menu position if it would go outside viewport
            if (
                newVisibility &&
                profileMenuRef.current &&
                profileIconRef.current
            ) {
                setTimeout(() => {
                    const menu = profileMenuRef.current;
                    const icon = profileIconRef.current;
                    const rect = icon.getBoundingClientRect();
                    const menuWidth = 220; // Menu width
                    const viewportWidth = window.innerWidth;

                    // Check if menu would go outside right edge
                    if (rect.right - menuWidth < 0) {
                        // Position menu to the left of the icon
                        menu.style.right = "auto";
                        menu.style.left = "0";
                    } else {
                        // Default position to the right
                        menu.style.left = "auto";
                        menu.style.right = "0";
                    }
                }, 10);
            }
        },
        [isProfileMenuVisible],
    );

    const handleLogout = useCallback(() => {
        localStorage.clear();
        logout("");
    }, [navigate]);

    useEffect(() => {
        const handleClickOutside = (event) => {
            // Close if clicked outside both profile icon and menu
            if (
                profileMenuRef.current &&
                !profileMenuRef.current.contains(event.target) &&
                profileIconRef.current &&
                !profileIconRef.current.contains(event.target)
            ) {
                setIsProfileMenuVisible(false);
            }
        };

        if (isProfileMenuVisible) {
            document.addEventListener("mousedown", handleClickOutside);
        }

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [isProfileMenuVisible]);

    return (
        <>
            <div className="header-menu">
                <div className="group-2">
                    <div className="menu-icon">
                        <button
                            onClick={toggleSidebar}
                            className="menu-button p-0"
                        >
                            <img
                                src={MenuIcon}
                                className="showHideMenuImg"
                                alt="Menu Toggle"
                            />
                        </button>

                        <Sidebar
                            isOpen={isSidebarOpen}
                            toggleSidebar={() => setIsSidebarOpen(false)}
                        />
                    </div>
                    <div className="logo ps-4">
                        <Link to="/">
                            <img
                                src={Logo}
                                className="ps-2"
                                alt="Smart Rx Logo"
                            />
                        </Link>
                    </div>
                    <div className="profile-alert">
                        <div className="alert-icon">
                            <img src={AlertIcon} alt="Notifications" />
                        </div>
                        <div
                            className="profile-icon"
                            onClick={toggleProfileMenu}
                            ref={profileIconRef}
                        >
                            <img
                                src={ProfileIcon}
                                className="active cursor-pointer"
                                alt="Profile"
                            />
                        </div>

                        {/* Profile Menu - positioned relative to header */}
                        <div
                            ref={profileMenuRef}
                            className={`profile-menu-container ${
                                isProfileMenuVisible ? "visible" : "hidden"
                            }`}
                        >
                            <ul className="profile-menu">
                                <div
                                    style={{
                                        padding: "8px 10px",
                                        borderBottom: "1px solid #ddd",
                                        fontFamily: "Georama",
                                        color: "#65636e",
                                        fontWeight: 600,
                                    }}
                                >
                                    <div> User Id: {userCode || "-"}</div>
                                    <div>
                                        Name:{" "}
                                        {(firstName + " " + lastName).trim() || "-"}
                                    </div>
                                    {/* <div>Sign Up Date: {cDate || "-"}</div> */}
                                </div>
                                <div>
                                    <li
                                        onClick={handleLogout}
                                        style={{
                                            backgroundColor: "#f8f9fa",
                                            cursor: "pointer",
                                            marginTop: "5px",
                                        }}
                                    >
                                        <div
                                            className="d-flex align-items-center gap-2"
                                            style={{
                                                color: "#c0392b",
                                                fontWeight: "500",
                                                fontFamily: "Georama",
                                            }}
                                        >
                                            <VscSignOut className="fs-5" />
                                            <div>Logout</div>
                                        </div>
                                    </li>
                                </div>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default React.memo(Header);
