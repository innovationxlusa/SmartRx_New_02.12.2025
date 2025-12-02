import React, { useState } from "react";
import "./CustomImage.css";
import { Tooltip, OverlayTrigger } from "react-bootstrap";
import { FaUser } from "react-icons/fa";

const CustomImage = ({
  src = "",
  alt = "Avatar",
  size = "medium",
  shape = "round",
  border = false,
  borderColor = "#000",
  bgColor = "#ccc",
  textColor = "#fff",
  initials = "",
  name = "",
  className = "",
}) => {
  const sizeClasses = {
    tiny: "avatar-tiny",
    small: "avatar-small",
    medium: "avatar-medium",
    large: "avatar-large",
    big: "avatar-big",
  };

  const shapeClasses = {
    round: "rounded-circle",
    square: "rounded-0",
    squareRounded: "rounded",
  };

  const defaultStyles = {
    backgroundColor: bgColor,
    color: textColor,
    borderColor: border ? borderColor : "transparent",
    borderWidth: border ? "1px" : "0",
    borderStyle: border ? "solid" : "none",
  };

  const [showTooltip, setShowTooltip] = useState(false);

  const handleMouseEnter = () => name && setShowTooltip(true);
  const handleMouseLeave = () => setShowTooltip(false);

  const avatarInitials = (fullName) => {
    if (fullName) {
      const names = fullName.split(" ");
      const initials = names.slice(0, 2).map((n) => n[0].toUpperCase());
      return initials.join("");
    }
    return <FaUser />;
  };

  return (
    <div
      className={`d-flex align-items-center justify-content-center ${sizeClasses[size]} ${shapeClasses[shape]} ${className}`}
      style={defaultStyles}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
    >
      <OverlayTrigger
        placement="top"
        overlay={<Tooltip>{name}</Tooltip>}
        show={showTooltip}
      >
        <span>
          {src ? (
            <img
              src={src}
              alt={alt}
              className={`img-fluid ${shapeClasses[shape]} w-100 h-100`}
            />
          ) : (
            <span className="d-flex align-items-center justify-content-center w-100 h-100">
              {avatarInitials(initials)}
            </span>
          )}
        </span>
      </OverlayTrigger>
    </div>
  );
};

export default CustomImage;
