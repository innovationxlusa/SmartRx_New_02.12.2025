import axios from "axios";
import { BASE_URL, REFRESH_TOKEN_URL } from "../constants/apiEndpoints";
import { tokenStore } from "../contexts/AuthContext";

// Create a singleton axios instance
const axiosClient = axios.create({
    baseURL: BASE_URL,
});

let isRefreshing = false;
let refreshQueue = [];

function onRefreshed(newAccessToken) {
    refreshQueue.forEach((cb) => {
        try { cb.resolve(newAccessToken); } catch {}
    });
    refreshQueue = [];
}

function onRefreshFailed(error) {
    refreshQueue.forEach((cb) => {
        try { cb.reject(error); } catch {}
    });
    refreshQueue = [];
}

async function refreshAccessToken() {
    if (isRefreshing) {
        // Return a promise that resolves when refresh completes
        return new Promise((resolve, reject) => {
            refreshQueue.push({ resolve, reject });
        });
    }

    if (!tokenStore.refreshAccessToken) {
        throw new Error("AuthContext not initialized");
    }

    isRefreshing = true;
    try {
        // Use AuthContext refresh method which handles state updates
        const newToken = await tokenStore.refreshAccessToken();
        onRefreshed(newToken);
        return newToken;
    } catch (err) {
        onRefreshFailed(err);
        if (tokenStore.forceLogout) {
            tokenStore.forceLogout("Session expired. Please log in again.");
        } else if (tokenStore.clearTokens) {
            tokenStore.clearTokens();
            try { window.location.replace("/"); } catch {}
        }
        throw err;
    } finally {
        isRefreshing = false;
    }
}

// Request interceptor: attach/refresh access token when withAuth=true

axiosClient.interceptors.request.use(async (config) => {
    const withAuth = config.withAuth !== false && config.withAuth !== 0 ? config.withAuth : false;
    if (!withAuth) return config;
    const isRefreshCall = String(config?.url || "").includes(REFRESH_TOKEN_URL);
    let token = tokenStore.accessToken;
    const refreshTokenValue = tokenStore.refreshToken;

    if (!token && !isRefreshCall && refreshTokenValue && tokenStore.refreshAccessToken) {
        try {
            token = await refreshAccessToken();
        } catch (_) {
            // will be handled by response interceptor redirect
        }
    }

    if (token) {
        config.headers = config.headers || {};
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Response interceptor: on 401, try refresh and retry queued requests

axiosClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error?.config || {};
        const status = error?.response?.status;

        // Avoid infinite loop: do not intercept refresh calls
        const isRefreshCall = String(originalRequest?.url || "").includes(REFRESH_TOKEN_URL);
        const withAuth = originalRequest.withAuth !== false && originalRequest.withAuth !== 0 ? originalRequest.withAuth : false;

        if (status === 401 && withAuth && !isRefreshCall) {
            if (tokenStore.refreshAccessToken) {
                try {
                    const newToken = await refreshAccessToken();

                    // Update header and retry
                    originalRequest.headers = originalRequest.headers || {};
                    originalRequest.headers.Authorization = `Bearer ${newToken}`;
                    // Ensure we mark it as withAuth to re-attach on further interceptors
                    originalRequest.withAuth = true;

                    return axiosClient(originalRequest);
                } catch (refreshErr) {
                    if (tokenStore.forceLogout) {
                        tokenStore.forceLogout("Session expired. Please log in again.");
                    }
                    return Promise.reject(refreshErr);
                }
            } else {
                if (tokenStore.forceLogout) {
                    tokenStore.forceLogout("Session expired. Please log in again.");
                }
            }
        }

        return Promise.reject(error);
    },
);

export default axiosClient;


