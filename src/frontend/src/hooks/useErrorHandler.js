import { useUserContext } from "../contexts/UserContext";
import useToastMessage from "./useToastMessage";
import { handleGeneralError, handleApiError, handleValidationError } from "../utils/errorHandling";

export const useErrorHandler = () => {
    const { setIsLoggedIn } = useUserContext;
    const showToast = useToastMessage();

    return {
        handleGeneralError: (error, setError) => handleGeneralError(error, setError, showToast),
        handleApiError: (error, setError) => handleApiError(error, setError, showToast, setIsLoggedIn),
        handleValidationError: (validationErrors, setFieldErrors, showToast) => handleValidationError(validationErrors, setFieldErrors),
    };
};
