import { createContext, useState, useContext, useCallback, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { BASE_URL, REFRESH_TOKEN_URL } from "../constants/apiEndpoints";
import { useLocation, useNavigate } from "react-router-dom";
import { AUTH_REDIRECT_STORAGE_KEY } from "../constants/navigation";

// Create the AuthContext
const AuthContext = createContext();

// Global token accessor for non-React code (axios interceptors)
let tokenStore = {
    accessToken: null,
    refreshToken: null,
    setTokens: null,
};

let isForceLogoutInProgress = false;

const AuthProvider = ({ children }) => {
    const location = useLocation();
    const navigate = useNavigate();

    // Helper function to check if token is expired
    const getTokenExpiryInfo = (token) => {
        if (!token) {
            return { isExpired: true, secondsUntilExpiry: null };
        }

        try {
            const decoded = jwtDecode(token);
            const expirySeconds = decoded?.exp || 0;
            const currentTimeSeconds = Date.now() / 1000;
            const secondsUntilExpiry = expirySeconds - currentTimeSeconds;

            return {
                isExpired: secondsUntilExpiry <= 0,
                secondsUntilExpiry,
            };
        } catch {
            return { isExpired: true, secondsUntilExpiry: null };
        }
    };

    const checkTokenExpiry = (token) => getTokenExpiryInfo(token).isExpired;

    // Initialize tokens from localStorage on mount (for page refresh)
    const initializeTokensFromStorage = () => {
        try {
            const storedAccessToken = localStorage.getItem("accessToken");
            const storedRefreshToken = localStorage.getItem("refreshToken");
            const storedPermissions = localStorage.getItem("accessPermissions");

            // If access token exists and is not expired, restore it
            if (storedAccessToken && !checkTokenExpiry(storedAccessToken)) {
                return {
                    accessToken: storedAccessToken,
                    refreshToken: storedRefreshToken,
                    permissions: storedPermissions ? JSON.parse(storedPermissions) : null,
                };
            }

            // If access token is expired but refresh token is valid, tokens will be refreshed automatically
            // For now, return stored tokens if refresh token is valid
            if (storedRefreshToken && !checkTokenExpiry(storedRefreshToken)) {
                return {
                    accessToken: storedAccessToken,
                    refreshToken: storedRefreshToken,
                    permissions: storedPermissions ? JSON.parse(storedPermissions) : null,
                };
            }

            // Both tokens expired or invalid, clear storage
            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");
            localStorage.removeItem("accessPermissions");
            return null;
        } catch (error) {
            console.error("Error initializing tokens from storage:", error);
            return null;
        }
    };

    // Initialize state from localStorage
    const initialTokens = initializeTokensFromStorage();
    const [accessToken, setAccessTokenState] = useState(initialTokens?.accessToken || null);
    const [refreshToken, setRefreshTokenState] = useState(initialTokens?.refreshToken || null);
    const [accessPermissions, setAccessPermissions] = useState(
        initialTokens?.permissions || []
    );

    // Update global token store when state changes
    useEffect(() => {
        tokenStore.accessToken = accessToken;
        tokenStore.refreshToken = refreshToken;
    }, [accessToken, refreshToken]);

    // Set tokens helper (with localStorage sync)
    const setTokens = useCallback((newAccessToken, newRefreshToken, permissions = null) => {
        setAccessTokenState(newAccessToken);
        setRefreshTokenState(newRefreshToken);
        
        // Persist tokens to localStorage
        if (newAccessToken) {
            localStorage.setItem("accessToken", newAccessToken);
        } else {
            localStorage.removeItem("accessToken");
        }
        
        if (newRefreshToken) {
            localStorage.setItem("refreshToken", newRefreshToken);
        } else {
            localStorage.removeItem("refreshToken");
        }
        
        if (permissions !== null) {
            const permissionsArray = Array.isArray(permissions) ? permissions : [];
            setAccessPermissions(permissionsArray);
            if (permissionsArray.length > 0) {
                localStorage.setItem("accessPermissions", JSON.stringify(permissionsArray));
            } else {
                localStorage.removeItem("accessPermissions");
            }
        }
    }, []);

    // Clear all tokens (with localStorage cleanup)
    const clearTokens = useCallback(() => {
        setAccessTokenState(null);
        setRefreshTokenState(null);
        setAccessPermissions([]);
        tokenStore.accessToken = null;
        tokenStore.refreshToken = null;
        
        // Clear tokens from localStorage
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("accessPermissions");
    }, []);

    const forceLogout = useCallback((reason = "Session expired. Please log in again.") => {
        if (isForceLogoutInProgress) {
            return;
        }
        isForceLogoutInProgress = true;

        clearTokens();

        try {
            sessionStorage.setItem("auth:logoutReason", reason || "");
        } catch {}

        try {
            window.dispatchEvent(
                new CustomEvent("auth:force-logout", {
                    detail: { reason }
                })
            );
        } catch {}

        try {
            window.location.replace("/");
        } catch {}
    }, [clearTokens]);

    // Check if access token is expired
    const isAccessTokenExpired = useCallback(() => {
        return getTokenExpiryInfo(accessToken).isExpired;
    }, [accessToken]);

    // Check if refresh token is expired
    const isRefreshTokenExpired = useCallback(() => {
        return getTokenExpiryInfo(refreshToken).isExpired;
    }, [refreshToken]);

    // Refresh access token using refresh token
    const refreshAccessToken = useCallback(async () => {
        if (!refreshToken) {
            throw new Error("No refresh token available");
        }

        if (isRefreshTokenExpired()) {
            clearTokens();
            throw new Error("Refresh token expired");
        }

        try {
            const response = await axios.post(
                `${BASE_URL}${REFRESH_TOKEN_URL}`,
                {
                    AccessToken: accessToken || "",
                    RefreshToken: refreshToken || "",
                },
            );

            const data = response?.data || {};
            const newAccessToken = data.accessToken || data.AccessToken;
            const newRefreshToken = data.refreshToken || data.RefreshToken;
            const functionCodes = data.functionCodes || data.FunctionCodes;

            if (newAccessToken && newRefreshToken) {
                setTokens(newAccessToken, newRefreshToken, functionCodes);
                return newAccessToken;
            } else {
                throw new Error("Invalid refresh response");
            }
        } catch (error) {
            if (error.response?.status === 401 || error.response?.status === 400) {
                forceLogout("Session expired. Please log in again.");
            }
            throw error;
        }
    }, [accessToken, refreshToken, isRefreshTokenExpired, setTokens, forceLogout]);

    // Auto-refresh token before expiry
    useEffect(() => {
        if (!refreshToken || isRefreshTokenExpired()) {
            return;
        }

        let timeoutId = null;

        const scheduleRefresh = () => {
            const { isExpired, secondsUntilExpiry } = getTokenExpiryInfo(accessToken);

            if (!accessToken || isExpired || typeof secondsUntilExpiry !== "number") {
                refreshAccessToken().catch((err) => {
                    console.error("Auto-refresh failed:", err);
                });
                return;
            }

            const bufferSeconds = 60;
            const delayMs = Math.max((secondsUntilExpiry - bufferSeconds) * 1000, 0);

            timeoutId = window.setTimeout(async () => {
                try {
                    await refreshAccessToken();
                } catch (err) {
                    console.error("Auto-refresh failed:", err);
                }
            }, delayMs);
        };

        scheduleRefresh();

        return () => {
            if (timeoutId !== null) {
                clearTimeout(timeoutId);
            }
        };
    }, [accessToken, refreshToken, isRefreshTokenExpired, refreshAccessToken]);

    // Expose global token getter for axiosClient
    tokenStore.setTokens = setTokens;
    tokenStore.clearTokens = clearTokens;
    tokenStore.refreshAccessToken = refreshAccessToken;
    tokenStore.forceLogout = forceLogout;
 
    useEffect(() => {
        if (!accessToken) {
            return;
        }

        const storedRoute = sessionStorage.getItem(AUTH_REDIRECT_STORAGE_KEY);
        if (!storedRoute) {
            return;
        }

        //sessionStorage.removeItem(AUTH_REDIRECT_STORAGE_KEY);
        if (storedRoute.startsWith("/dashboard")) {
            if (!location.pathname.startsWith("/dashboard")) {
                navigate("/dashboard", { replace: true });
            }
            return;
        }

        if (storedRoute.startsWith("/all-patient")) {
            if (!location.pathname.startsWith("/all-patient")) {
                navigate("/all-patient", { replace: true });
            }
        }
    }, [accessToken, location.pathname, navigate]);

    return (
        <AuthContext.Provider
            value={{
                accessToken,
                refreshToken,
                accessPermissions,
                setTokens,
                clearTokens,
                forceLogout,
                refreshAccessToken,
                isAccessTokenExpired,
                isRefreshTokenExpired,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};

/**
 * Hook to access the AuthContext
 */
const useAuthContext = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuthContext must be used within AuthProvider");
    }
    return context;
};

// Export token store for non-React code
export { tokenStore };

export { AuthProvider, useAuthContext };

