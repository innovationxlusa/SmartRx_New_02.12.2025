import { useState } from "react";
import { FaStar } from "react-icons/fa";

const StarRating = ({ rating = 0, onChange, size = 24, readOnly = false }) => {
    const [hoverRating, setHoverRating] = useState(null);

    const getFillPercent = (index) => {
        const value = hoverRating ?? rating;
        const diff = value - index;
        return Math.max(0, Math.min(1, diff));
    };

    const handleMouseMove = (event, index) => {
        if (readOnly) return;

        const { left, width } = event.currentTarget.getBoundingClientRect();
        const percent = (event.clientX - left) / width;
        const preciseValue = +(index + percent).toFixed(2);
        setHoverRating(Math.min(5, Math.max(0, preciseValue)));
    };

    const handleClick = (event, index) => {
        if (readOnly) return;

        const { left, width } = event.currentTarget.getBoundingClientRect();
        const percent = (event.clientX - left) / width;
        const preciseValue = +(index + percent).toFixed(2);
        onChange?.(Math.min(5, Math.max(0, preciseValue)));
    };

    return (
        <div
            style={{
                display: "flex",
                cursor: readOnly ? "default" : "pointer",
                gap: 4,
            }}
            onMouseLeave={() => setHoverRating(null)}
        >
            {[...Array(5)].map((_, i) => {
                const fillPercent = getFillPercent(i) * 100;

                return (
                    <div
                        key={i}
                        onMouseMove={(e) => handleMouseMove(e, i)}
                        onClick={(e) => handleClick(e, i)}
                        style={{
                            position: "relative",
                            width: size,
                            height: size,
                        }}
                    >
                        {/* Base gray star */}
                        <FaStar
                            size={size}
                            color="#e0e0e0"
                            style={{ position: "absolute", top: 0, left: 0 }}
                        />

                        {/* Filled portion of star */}
                        <div
                            style={{
                                position: "absolute",
                                top: 0,
                                left: 0,
                                width: `${fillPercent}%`,
                                height: "100%",
                                overflow: "hidden",
                            }}
                        >
                            <FaStar
                                size={size}
                                color="var(--app-theme-color)"
                                style={{
                                    position: "absolute",
                                    top: 0,
                                    left: 0,
                                }}
                            />
                        </div>
                    </div>
                );
            })}
        </div>
    );
};

export default StarRating;
