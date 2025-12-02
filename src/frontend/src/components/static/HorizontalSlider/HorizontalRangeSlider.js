import React, { useEffect, useRef } from "react";
import noUiSlider from "nouislider";
import "nouislider/dist/nouislider.css";
import "./HorizontalRangeSlider.css";

const HorizontalRangeSlider = () => {
    const sliderRef = useRef(null);
    const minRange = 0;
    const maxRange = 100;

    useEffect(() => {
        if (!sliderRef.current) return;

        // Create slider
        noUiSlider.create(sliderRef.current, {
            start: [0], // Must be an array
            range: {
                min: minRange,
                max: maxRange,
            },
            orientation: "horizontal",
            direction: "ltr",
            connect: [true, false], // Show filled track
            pips: {
                mode: "range",
                density: 3,
            },
        });

        // Mapping value to pip index
        const mapRange = (value, low1, high1, low2, high2) => low2 + ((high2 - low2) * (value - low1)) / (high1 - low1);

        // Highlight active markers
        const setMarkers = () => {
            const pips = sliderRef.current.querySelectorAll(".noUi-marker");
            const currentValue = parseFloat(sliderRef.current.noUiSlider.get());
            const mappedValue = Math.round(mapRange(currentValue, minRange, maxRange, 0, pips.length - 1));

            pips.forEach((pip, i) => {
                pip.classList.remove("noUi-marker--is-current", "noUi-marker--is-inRange");
                if (i < mappedValue) pip.classList.add("noUi-marker--is-inRange");
                if (i === mappedValue) pip.classList.add("noUi-marker--is-current");
            });
        };

        sliderRef.current.noUiSlider.on("slide", setMarkers);
        setMarkers();

        return () => {
            if (sliderRef.current.noUiSlider) {
                sliderRef.current.noUiSlider.destroy();
            }
        };
    }, []);

    return <div className="Range" ref={sliderRef}></div>;
};

export default HorizontalRangeSlider;
