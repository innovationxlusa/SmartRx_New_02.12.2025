import { Navigate, useLocation } from "react-router-dom";
import { useAuthContext } from "../contexts/AuthContext";
import { useUserContext } from "../contexts/UserContext";
import { AUTH_REDIRECT_STORAGE_KEY } from "../constants/navigation";
import { useEffect } from "react";

const GuestRoute = ({ children }) => {
    const location = useLocation();
    const { accessToken } = useAuthContext();
    const { intendedRoute, setIntendedRoute } = useUserContext();
    
    useEffect(() => {
        if (!accessToken) {
            const redirectTo =
                typeof location?.state?.redirectTo === "string" &&
                location.state.redirectTo.length > 0
                    ? location.state.redirectTo
                    : "";
            if (redirectTo) {
                setIntendedRoute(redirectTo);
            }
        }
    }, [accessToken, location?.state?.redirectTo, setIntendedRoute]);

    if (!accessToken) {
        return children;
    }

    const storedRoute =
        intendedRoute||
        sessionStorage.getItem(AUTH_REDIRECT_STORAGE_KEY) ||
        "";

    const targetRoute = storedRoute.startsWith("/dashboard")
        ? "/dashboard"
        : storedRoute.startsWith("/all-patient")
        ? "/all-patient"
        : null;

    if (targetRoute) {
        // sessionStorage.removeItem(AUTH_REDIRECT_STORAGE_KEY);
        // if (intendedRoute) {
        //     setIntendedRoute("");
        // }
        return <Navigate to={targetRoute} replace />;
    }

    return <Navigate to="/all-patient" replace />;
};

export default GuestRoute;

