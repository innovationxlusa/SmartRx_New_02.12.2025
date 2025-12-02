import { Navigate, useLocation } from "react-router-dom";
import { useAuthContext } from "../contexts/AuthContext";

const ProtectedRoute = ({ children }) => {
    const location = useLocation();
    const { accessToken } = useAuthContext();

    if (accessToken) {
        return children;
    }

    return (
        <Navigate
            to="/signIn"
            replace
            state={{ redirectTo: location.pathname + location.search + location.hash }}
        />
    );
};

export default ProtectedRoute;
