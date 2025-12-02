import React from "react";

const CommonButton = ({
    text,
    onClick,
    customStyles,
    onMouseOver,
    onMouseOut,
    onTouchStart,
    onTouchEnd,
    type,
}) => {
    return (
        <button
            style={customStyles}
            onClick={onClick}
            type={type}
            onMouseOver={onMouseOver}
            onMouseOut={onMouseOut}
            onTouchStart={onTouchStart}
            onTouchEnd={onTouchEnd}
        >
            {text}
        </button>
    );
};

export default CommonButton;
