// import { useNavigate, useNavigation, useLocation } from "react-router-dom";
// import { useEffect } from "react";

// /**
//  * Smoothly scrolls to a target location on the page.
//  *
//  * @param {'top' | 'middle' | 'bottom' | string | number} position
//  */
// const scrollToPosition = (position) => {
//     if (typeof window === "undefined") return;

//     if (typeof position === "string") {
//         const element = document.getElementById(position);
//         if (element) {
//             element.scrollIntoView({ behavior: "smooth" });
//         } else {
//             switch (position) {
//                 case "top":
//                     window.scrollTo({ top: 0, behavior: "smooth" });
//                     break;
//                 case "middle":
//                     window.scrollTo({ top: window.innerHeight / 2, behavior: "smooth" });
//                     break;
//                 case "bottom":
//                     window.scrollTo({ top: document.body.scrollHeight, behavior: "smooth" });
//                     break;
//                 default:
//                     break;
//             }
//         }
//     } else if (typeof position === "number") {
//         window.scrollTo({ top: position, behavior: "smooth" });
//     }
// };

// /**
//  * useSmartNavigate
//  *
//  * A robust React Router hook with enhanced navigation features and scroll positioning.
//  *
//  * @param {Object} options
//  * @param {'top' | 'middle' | 'bottom' | string | number} [options.scroll='top']
//  *
//  * @returns {{
//  *   smartNavigate: (to: string, options?: object) => void;
//  *   navigateBack: () => void;
//  *   navigateForward: () => void;
//  *   isNavigating: boolean;
//  *   navigationState: 'idle' | 'loading' | 'submitting';
//  *   navigation: ReturnType<typeof useNavigation>;
//  *   location: ReturnType<typeof useLocation>;
//  * }}
//  *
//  * @example
//  * const { smartNavigate, navigateBack } = useSmartNavigate({ scroll: 'top' });
//  * smartNavigate('/home');
//  * navigateBack();
//  */
// const useSmartNavigate = ({ scroll = "top" } = {}) => {
//     const navigate = useNavigate();
//     const navigation = useNavigation();
//     const location = useLocation();

//     useEffect(() => {
//         if (navigation.state === "idle") {
//             scrollToPosition(scroll);
//         }
//     }, [navigation.state, location.pathname]);

//     /**
//      * Navigate to a route and apply scroll after loading.
//      */
//     const smartNavigate = (to, options = {}) => {
//         navigate(to, options);
//     };

//     /**
//      * Navigate back in history.
//      */
//     const navigateBack = () => {
//         navigate(-1);
//     };

//     /**
//      * Navigate forward in history.
//      */
//     const navigateForward = () => {
//         navigate(1);
//     };

//     return {
//         smartNavigate,
//         navigateBack,
//         navigateForward,
//         isNavigating: navigation.state !== "idle",
//         navigationState: navigation.state,
//         navigation,
//         location,
//     };
// };

// export default useSmartNavigate;

// import { useNavigate, useLocation } from "react-router-dom";
// import { useState } from "react";

// const scrollToPosition = (scroll) => {
//     const positions = {
//         top: 0,
//         middle: window.innerHeight / 2,
//         bottom: document.body.scrollHeight,
//     };
//     const scrollTo = typeof scroll === "number" ? scroll : positions[scroll] || 0;
//     window.scrollTo({ top: scrollTo, behavior: "smooth" });
// };

// const useSmartNavigate = ({ scroll = "top" } = {}) => {
//     const navigate = useNavigate();
//     const location = useLocation();
//     const [navigationState, setNavigationState] = useState("idle");
//     const [historyStack, setHistoryStack] = useState([location.pathname]);

//     const smartNavigate = (to, options = {}) => {
//         setNavigationState("loading");
//         navigate(to, options);
//         scrollToPosition(scroll);
//         setTimeout(() => setNavigationState("idle"), 500);
//         setHistoryStack((prev) => [...prev, to]);
//     };

//     const navigateBack = () => {
//         window.history.back();
//         scrollToPosition(scroll);
//     };

//     const navigateForward = () => {
//         window.history.forward();
//         scrollToPosition(scroll);
//     };

//     return {
//         smartNavigate,
//         navigateBack,
//         navigateForward,
//         isNavigating: navigationState === "loading",
//         navigationState,
//         navigate,
//         location,
//     };
// };

// export default useSmartNavigate;

import { useNavigate, useLocation, useNavigationType } from "react-router-dom";
import { useEffect, useState } from "react";
import { animateScroll as scroll } from "react-scroll";

const useSmartNavigate = ({ scrollBehavior = "top" } = {}) => {

    const navigate = useNavigate();
    const location = useLocation();
    const navigationType = useNavigationType();
    const [isNavigating, setIsNavigating] = useState(false);

    useEffect(() => {
        if (typeof window === "undefined") return;

        const handleScroll = () => {
            if (scrollBehavior === "top") {
                scroll.scrollToTop({ duration: 500, smooth: "easeInOutQuart" });
            } else if (scrollBehavior === "middle") {
                scroll.scrollTo(document.body.scrollHeight / 2, { duration: 500, smooth: "easeInOutQuart" });
            } else if (scrollBehavior === "bottom") {
                scroll.scrollToBottom({ duration: 500, smooth: "easeInOutQuart" });
            } else if (typeof scrollBehavior === "string" && scrollBehavior.startsWith("#")) {
                const element = document.querySelector(scrollBehavior);
                if (element) {
                    element.scrollIntoView({ behavior: "smooth" });
                }
            } else if (typeof scrollBehavior === "number") {
                scroll.scrollTo(scrollBehavior, { duration: 500, smooth: "easeInOutQuart" });
            }
        };

        // Delay to ensure DOM updates
        const timer = setTimeout(handleScroll, 100);
        return () => clearTimeout(timer);
    }, [location.pathname, location.hash, scrollBehavior]);

    const smartNavigate = (to, options = {}) => {
        setIsNavigating(true);
        navigate(to, options);
        setTimeout(() => setIsNavigating(false), 500);
    };

    const navigateBack = () => {
        setIsNavigating(true);
        navigate(-1);
        setTimeout(() => setIsNavigating(false), 500);
    };

    const navigateForward = () => {
        setIsNavigating(true);
        navigate(1);
        setTimeout(() => setIsNavigating(false), 500);
    };

    const reload = () => {
        window.location.reload();
    };

    const navigateWithState = (to, state) => {
        smartNavigate(to, { state });
    };

    return {
        smartNavigate,
        navigateBack,
        navigateForward,
        navigateWithState,
        reload,

        isNavigating,
        navigationType,
        location,

        navigate: smartNavigate,
        goBack: navigateBack,
        goForward: navigateForward,
    };
};

export default useSmartNavigate;
