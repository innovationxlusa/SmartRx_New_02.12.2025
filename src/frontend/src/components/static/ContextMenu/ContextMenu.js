import { useState, useRef, useEffect } from "react";
import "./ContextMenu.css";
import { GrMoreVertical } from "react-icons/gr";

const ContextMenu = ({ button, menuItems }) => {
    const [menuVisible, setMenuVisible] = useState(false);
    const [menuStyle, setMenuStyle] = useState({});
    const buttonRef = useRef(null);
    const menuRef = useRef(null);

    const toggleMenu = (e) => {
        e.stopPropagation();
        setMenuVisible((prev) => !prev);
    };

    const handleClickOutside = (e) => {
        if (menuRef.current && !menuRef.current.contains(e.target) && buttonRef.current && !buttonRef.current.contains(e.target)) {
            setMenuVisible(false);
        }
    };

    const adjustMenuPosition = () => {
        if (!menuRef.current || !buttonRef.current) return;

        const buttonRect = buttonRef.current.getBoundingClientRect();
        const menuWidth = menuRef.current.offsetWidth;
        const menuHeight = menuRef.current.offsetHeight;
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;

        let left = buttonRect.left;
        let top = buttonRect.bottom;

        // Prevent right-side overflow
        if (left + menuWidth > windowWidth) {
            left = windowWidth - menuWidth - 10; // Adjust with padding
        }

        // Prevent bottom overflow
        if (top + menuHeight > windowHeight) {
            top = buttonRect.top - menuHeight;
        }

        setMenuStyle({
            position: "fixed",
            top: `${top}px`,
            left: `${left}px`,
            zIndex: 1000,
        });
    };

    useEffect(() => {
        if (menuVisible) {
            adjustMenuPosition();
        }
    }, [menuVisible]);

    useEffect(() => {
        document.addEventListener("click", handleClickOutside);
        window.addEventListener("resize", adjustMenuPosition);
        return () => {
            document.removeEventListener("click", handleClickOutside);
            window.removeEventListener("resize", adjustMenuPosition);
        };
    }, []);

    return (
        <div className="d-inline-block">
            <span ref={buttonRef} onClick={toggleMenu} style={{ display: "inline-block" }}>
                {button ? (
                    button
                ) : (
                    <button className="btn border-0 rounded-circle context-button">
                        <GrMoreVertical className="mb-1" />
                    </button>
                )}
            </span>
            {menuVisible && (
                <ul className="dropdown-menu show fade-in" ref={menuRef} style={menuStyle}>
                    {menuItems.map((item, index) => (
                        <li
                            key={index}
                            onClick={() => {
                                item.action();
                                setMenuVisible(false);
                            }}
                        >
                            <button className="dropdown-item d-flex align-items-center gap-2">
                                {item.icon} {item.label}
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default ContextMenu;
