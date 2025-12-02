import { createContext, useContext, useEffect, useCallback, useState } from "react";

/**
 * Create a context for managing local storage values.
 *
 * @typedef {Object} StoreContextType
 * @property {Object} store - The current store containing key-value pairs.
 * @property {function} setStore - Function to update the store.
 */
const StoreContext = createContext({
    store: {}, // The current store
    setStore: () => {}, // Function to update the store
});

/**
 * LocalStorageProvider Component
 *
 * This component wraps the application and provides access to
 * the store context, allowing child components to read and
 * update local storage values.
 *
 * @param {Object} props - Component props
 * @param {ReactNode} props.children - Child components that need access to the store.
 */
const LocalStorageProvider = ({ children }) => {
    const [store, setStore] = useState({}); // State to hold the store values

    // Provide the store and setStore function to the children components
    return <StoreContext.Provider value={{ store, setStore }}>{children}</StoreContext.Provider>;
};

/**
 * Custom hook to manage local storage.
 *
 * @param {string} key - The key under which the value is stored in local storage.
 * @param {any} [initialValue=null] - The initial value if no value is found in local storage.
 * @param {boolean} [storeInLocalStorage=true] - Whether to store the value in local storage.
 * @returns {[any, function, boolean]} - Returns an array containing:
 *  1. The current value from the store or local value.
 *  2. A function to update the value.
 *  3. A loading state indicating if the value is being processed.
 */
const useLocalStorage = (key, initialValue = null, storeInLocalStorage = true) => {
    const { store, setStore } = useContext(StoreContext); // Access the store context

    /**
     * Function to initialize state by checking local storage.
     *
     * @returns {any} - The initial value for the state.
     */
    const initializeState = useCallback(() => {
        if (storeInLocalStorage && typeof window !== "undefined") {
            const storedValue = localStorage.getItem(key); // Get the value from local storage
            if (storedValue === null || storedValue === "undefined") {
                // If there's no stored value or it's undefined, return the initial value
                return initialValue;
            }

            try {
                // Try to parse the stored value as JSON
                return JSON.parse(storedValue);
            } catch (e) {
                // If parsing fails, log an error and return the initial value
                console.error(`Error parsing localStorage item '${key}':`, e);
                return initialValue;
            }
        }

        return initialValue; // Fallback to the initial value if not using local storage
    }, [key, initialValue, storeInLocalStorage]);

    const [isLoading, setIsLoading] = useState(true); // State to track loading status
    const [localValue, setLocalValue] = useState(initializeState); // State to hold the local value

    // Effect to handle side effects related to local storage and store updates
    useEffect(() => {
        setIsLoading(true); // Set loading to true while processing

        // If using local storage, set the value in local storage
        if (storeInLocalStorage && localValue !== undefined) {
            localStorage.setItem(key, JSON.stringify(localValue)); // Update local storage
        }

        // Update the store context with the new local value
        setStore((prevStore) => ({
            ...prevStore,
            [key]: localValue,
        }));

        setIsLoading(false); // Set loading to false after processing
    }, [key, localValue, storeInLocalStorage, setStore]);

    /**
     * Function to set a new value for local storage and store context.
     *
     * @param {any} value - The new value to set.
     */
    const setValue = useCallback(
        (value) => {
            setLocalValue(value); // Update local value state
            setStore((prevStore) => ({
                ...prevStore,
                [key]: value, // Update store context with new value
            }));

            // If using local storage, update the stored value
            if (storeInLocalStorage) {
                localStorage.setItem(key, JSON.stringify(value));
            }
        },
        [key, storeInLocalStorage, setStore]
    );

    // Return the value from the store (or localValue if not found), setValue function, and loading state
    return [store[key] !== undefined ? store[key] : localValue, setValue, isLoading];
};

// Export the provider and hook for use in other components
export { LocalStorageProvider, useLocalStorage, StoreContext };
