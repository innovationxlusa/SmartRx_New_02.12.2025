import { useState, useEffect, useCallback } from "react";

const CONNECTIVITY_CHECK_TIMEOUT = 3000;

const checkInternetConnectivity = async (): Promise<boolean> => {
    // First check the browser's online status
    if (!navigator.onLine) {
        return false;
    }

    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), CONNECTIVITY_CHECK_TIMEOUT);

    try {
        // Try fetching from a reliable source with cache-busting
        const response = await fetch("https://www.google.com/favicon.ico?_=" + Date.now(), {
            method: "HEAD",
            signal: controller.signal,
            cache: "no-store",
        });
        return response.ok;
    } catch (error) {
        return false;
    } finally {
        clearTimeout(timeoutId);
    }
};

export const useInternetAvailability = () => {
    const [isConnected, setIsConnected] = (useState < boolean) | (null > null);
    const [isLoading, setIsLoading] = useState(true);

    const checkConnection = useCallback(async () => {
        setIsLoading(true);
        try {
            const connected = await checkInternetConnectivity();
            setIsConnected(connected);
        } catch (error) {
            setIsConnected(false);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        let mounted = true;
        let retryTimeout: NodeJS.Timeout;

        const handleOnlineChange = async () => {
            if (!mounted) return;

            if (retryTimeout) {
                clearTimeout(retryTimeout);
            }

            if (!navigator.onLine) {
                setIsConnected(false);
            } else {
                // Delay slightly to allow network to stabilize
                retryTimeout = setTimeout(async () => {
                    if (mounted) {
                        await checkConnection();
                    }
                }, 1000);
            }
        };

        // Initial check
        checkConnection();

        // Add event listeners for online/offline changes
        window.addEventListener("online", handleOnlineChange);
        window.addEventListener("offline", handleOnlineChange);

        return () => {
            mounted = false;
            if (retryTimeout) {
                clearTimeout(retryTimeout);
            }
            window.removeEventListener("online", handleOnlineChange);
            window.removeEventListener("offline", handleOnlineChange);
        };
    }, [checkConnection]);

    const recheckConnection = useCallback(() => {
        return checkConnection();
    }, [checkConnection]);

    return {
        isConnected,
        isLoading,
        recheckConnection,
    };
};
