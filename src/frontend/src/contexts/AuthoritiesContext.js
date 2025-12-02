import React, { createContext, useContext, useEffect } from "react";
import { useFetchData } from "../hooks/useFetchData";
import useApiClients from "../services/useApiClients";
import { useAuthContext } from "./AuthContext";

// Create context for authorities
const AuthoritiesContext = createContext();

/**
 * Authorities Provider Component
 * Fetches authorities data and provides it to children components through context.
 *
 * @param {object} children - React child components
 */
const AuthoritiesProvider = ({ children }) => {
    const { api } = useApiClients();
    const { accessToken } = useAuthContext();

    // Fetch authorities data using custom hook
    const { data: authoritiesData = [], isLoading, error, refetch } = useFetchData(api.getUserAuthorities);

    useEffect(() => {
        if (accessToken) {
            refetch();
        }
    }, [accessToken]);

    return <AuthoritiesContext.Provider value={{ authoritiesData, isLoading, error }}>{children}</AuthoritiesContext.Provider>;
};

/**
 * Custom hook to check if a user has a specific authority.
 *
 * @returns {function} hasAuthority - Function to check if an authority exists.
 */
const useHasAuthority = () => {
    const { authoritiesData, fetchAuthorities } = useContext(AuthoritiesContext);

    /**
     * Checks if the user has a specific authority based on the unique code.
     *
     * @param {string} authority - The unique code of the authority to check.
     * @returns {boolean} - Returns true if the authority exists, otherwise false.
     */
    const hasAuthority = (authority) => {
        if (!authoritiesData || authoritiesData.length === 0) {
            return false;
        }

        return authoritiesData.some((auth) => auth.uniqueCode === authority);
    };

    return { hasAuthority, fetchAuthorities };
};

/**
 * Custom hook to access the entire authorities context.
 *
 * @returns {object} - The entire context value including authorities data, loading state, and error.
 */
const useAuthoritiesContext = () => {
    const context = useContext(AuthoritiesContext);

    if (!context) {
        return {
            authoritiesData: [],
            isLoading: true,
            error: new Error("Authorities context is not available."),
        };
    }

    return context;
};

export { AuthoritiesProvider, useHasAuthority, useAuthoritiesContext };
