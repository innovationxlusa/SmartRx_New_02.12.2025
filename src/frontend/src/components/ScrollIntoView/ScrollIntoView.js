import { useEffect } from "react";
import { animateScroll as scroll } from "react-scroll";
import { useLocation } from "react-router-dom";

const ScrollIntoView = ({ children }) => {
    const location = useLocation();

    useEffect(() => {
        scroll.scrollToTop({
            duration: 100,
            smooth: "easeInOutQuint",
        });
    }, [location]);

    return children;
};

export default ScrollIntoView;
