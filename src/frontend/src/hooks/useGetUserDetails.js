import { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode"; // Fix import statement
import { useAuthContext } from "../contexts/AuthContext"; // Use AuthContext for token management

const useGetUserDetails = () => {
    const { accessToken } = useAuthContext(); // Get accessToken from AuthContext
    const [user, setUser] = useState(null); // State to store decoded user data from accessToken
    const [isLoggedIn, setIsLoggedIn] = useState(false); // State to track user login status

    useEffect(() => {
        if (accessToken) {
            try {
                // Decode accessToken to get user data
                const decodedUserData = jwtDecode(accessToken);

                // Check if decodedUserData contains the expected fields
                if (decodedUserData) {
                    setUser(decodedUserData); // Set user state with decoded user data
                    setIsLoggedIn(true); // Update isLoggedIn state to true
                } else {
                    setUser(null); // Reset user state
                    setIsLoggedIn(false); // Update isLoggedIn state to false
                }
            } catch (error) {
                setUser(null); // Reset user state if decoding fails
                setIsLoggedIn(false); // Update isLoggedIn state to false
            }
        } else {
            setUser(null); // Reset user state if no accessToken is available
            setIsLoggedIn(false); // Update isLoggedIn state to false
        }
    }, [accessToken]); // Run effect whenever accessToken changes

    return { user, setUser, isLoggedIn, setIsLoggedIn }; // Return user data and login status
};

export default useGetUserDetails;
