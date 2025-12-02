import useToastMessage from "../hooks/useToastMessage";
import useApiServiceCall from "../hooks/useApiServiceCall";
import { useAuthContext } from "../contexts/AuthContext";
import axios from "axios";
import { BASE_URL } from "../constants/apiEndpoints";

/**
 * Custom hook to provide API service functions.
 * @returns {object} - Object containing API service functions like `getAll`.
 */
const useApiService = () => {
    const { accessToken } = useAuthContext(); // Get accessToken from AuthContext

    // Destructuring api call function
    const { executeApiCall } = useApiServiceCall();

    const showToast = useToastMessage();

    /**
     * Function to retrieve all items from a specified API endpoint.
     *
     * @param {string} endpoint - The API endpoint to fetch the data from.
     * @param {AbortSignal|null} signal - Optional abort signal to cancel the fetch request if needed (useful for cleaning up requests when a component unmounts). Default is null.
     *
     * @returns {Promise} - A promise that resolves with the API response if successful, or rejects with an error message if the user is not logged in or if the API call fails.
     *
     * - Checks if the `accessToken` is available. If not, returns a rejected promise with the message "User not logged in".
     * - Uses the `executeApiCall` function to perform a GET request to the specified endpoint.
     * - If the request is successful, the function resolves with the response.
     * - If the request fails, the function catches the error and logs "Failed to fetch all items:" along with the error details to the console.
     */
    const getAll = (endpoint, signal = null, view = false) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(
            endpoint,
            "GET",
            true,
            null,
            "",
            false,
            false,
            null,
            null,
            signal,
            false,
        )
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to fetch all items:", error),
            );
    };

    const getAllWithPostMethod = (endpoint, signal = null, payload) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(
            endpoint,
            "POST",
            true,
            payload,
            "",
            false,
            false,
            null,
            null,
            signal,
            false,
        )
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to fetch all items:", error),
            );
    };

    /* SHAHRIAR VAI'S CODE START HERE*/
    // const post = async (endpoint, body) => {
    //     try {
    //         const response = await axios.post(`${BASE_URL}${endpoint}`, body, {
    //             headers: {
    //                 "Content-Type": "application/json",
    //             },
    //         });
    //         return response?.data;
    //     } catch (error) {
    //         console.error("POST error:", error);
    //         throw error;
    //     }
    // };

    const post = async (endpoint, body) => {
        try {
            const result = await executeApiCall(
                endpoint,
                "POST",
                true,
                body,
                "",
                body instanceof FormData,
                false,
            );
            return result?.response;
        } catch (error) {
            console.error("POST error:", error);
            throw error;
        }
    };

    const getWithParams = async (endpoint, params, authentication = true) => {
        try {
            const qs = params
                ? `?${new URLSearchParams(params).toString()}`
                : "";
            const result = await executeApiCall(
                `${endpoint}${qs}`,
                "GET",
                authentication,
            );
            return result?.response;
        } catch (error) {
            console.error("getWithParams error:", error);
            throw error;
        }
    };

    /* SHAHRIAR VAI'S CODE END HERE*/

    /**
     * Fetches an item by ID from the endpoint.
     * @param {string} endpoint - API endpoint.
     * @param {number|string} id - Item ID.
     * @returns {Promise} - API response.
     */
    const getById = (endpoint, signal, id) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(
            `${endpoint}/${id}`,
            "GET",
            true,
            null,
            "",
            false,
            false,
            null,
            null,
            signal,
            false,
        )
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to fetch item by ID:", error),
            );
    };

    /**
     * Fetches paginated list from the endpoint.
     * @param {string} endpoint - API endpoint.
     * @param {number} page - Page number.
     * @param {number} limit - Number of items per page.
     * @param {string} sortBy - Field to sort by.
     * @returns {Promise} - API response.
     */
    const getPaginatedList = (endpoint, signal = null, page, limit, sortBy) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        const params = `page=${page}&limit=${limit}&sortBy=${sortBy}`;
        return executeApiCall(
            `${endpoint}?${params}`,
            "GET",
            true,
            null,
            "",
            false,
            false,
            null,
            null,
            signal,
        )
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to fetch paginated list:", error),
            );
    };

    /**
     * Creates a new item.
     * @param {string} endpoint - API endpoint.
     * @param {object} payload - Data to be sent to the API.
     * @returns {Promise} - API response.
     */
    const create = (endpoint, payload, topic) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(endpoint, "POST", true, payload, topic)
            .then((response) => response)
            .catch((error) => console.error("Failed to create item:", error));
    };

    /**
     * Updates an existing item.
     * @param {string} endpoint - API endpoint.
     * @param {number|string} id - Item ID.
     * @param {object} payload - Data to be sent to the API.
     * @returns {Promise} - API response.
     */
    // const update = (endpoint, id, payload) => {
    //     if (!accessToken) return showToast("error", "User not logged in.");
    //     return executeApiCall(`${endpoint}/${id}`, "PUT", true, payload)
    //         .then((response) => response)
    //         .catch((error) => console.error("Failed to update item:", error));
    // };

    /**
     * Updates an existing item.
     * @param {string} endpoint - API endpoint.
     * @param {number|string} id - Item ID.
     * @param {object} payload - Data to be sent to the API.
     * @returns {Promise} - API response.
     */
    // const update = (endpoint, payload, topic) => {
    //     if (!accessToken) return showToast("error", "User not logged in.");
    //     return executeApiCall(`${endpoint}`, "PUT", false, payload, topic)
    //         .then((response) => response)
    //         .catch((error) => console.error("Failed to update item:", error));
    // };

    const update = (
        endpoint,
        payload,
        topic,
        method = "PUT",
        isFileUpload = false,
    ) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(
            `${endpoint}`,
            method,
            true,
            payload,
            topic,
            isFileUpload,
            false,
        )
            .then((response) => response)
            .catch((error) => console.error("Failed to update item:", error));
    };

    /**
     * Removes an item.
     * @param {string} endpoint - API endpoint.
     * @param {number|string} id - Item ID.
     * @returns {Promise} - API response.
     */
    const remove = (endpoint, id, topic, payload = null) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(
            `${endpoint}/${id}`,
            "DELETE",
            true,
            payload,
            topic,
        )
            .then((response) => response)
            .catch((error) => console.error("Failed to delete item:", error));
    };

    /**
     * Changes user password.
     * @param {string} endpoint - API endpoint.
     * @param {object} payload - Data to be sent to the API.
     * @returns {Promise} - API response.
     */
    const changePassword = (endpoint, payload) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(endpoint, "PUT", true, payload)
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to change password:", error),
            );
    };

    /**
     * Updates user profile.
     * @param {string} endpoint - API endpoint.
     * @param {object} payload - Data to be sent to the API.
     * @returns {Promise} - API response.
     */
    const updateProfile = (endpoint, payload) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(endpoint, "PUT", true, payload)
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to update profile:", error),
            );
    };

    /**
     * Uploads a file.
     * @param {string} endpoint - API endpoint.
     * @param {File} file - File to be uploaded.
     * @param {function} onUploadProgress - Callback for tracking upload progress.
     * @returns {Promise} - API response.
     */

    const uploadFile = async (
        endpoint,
        payload,
        onUploadProgress = null,
        topic,
    ) => {
        if (!accessToken) return showToast("error", "User not logged in.");

        try {
            const response = await executeApiCall(
                endpoint,
                "POST",
                true, // authentication required
                payload,
                topic, // topic (optional for toast)
                true, // isFileUpload
                false, // isFileDownload
                onUploadProgress,
            );

            return response;
        } catch (err) {
            console.error("Failed to upload file:", err);
            throw err;
        }
    };

    /**
     * Downloads a file from the specified endpoint while tracking the download progress.
     * This function will handle authentication, display download progress, and trigger the download of the file.
     *
     * @async
     * @function downloadFileWithProgress
     * @returns {Promise<void>} - Resolves when the file is successfully downloaded.
     * @throws Will throw an error if the download or API call fails.
     *
     * @example
     * // Download a file and log the progress in the console
     * downloadFile();
     */
    const downloadFile = async (endpoint) => {
        // Define the API endpoint for the file download
        // const endpoint = "/files/download";

        // Optional: Specify the file name for the downloaded file. Defaults to 'example.pdf'
        // const fileName = "example.pdf";

        try {
            /**
             * Calls the executeApiCall function to download the file.
             * - The method is GET since we are downloading a file.
             * - Requires authentication.
             * - No payload or specific topic for toast messages.
             * - Not uploading a file, but is a file download.
             * - The onDownloadProgress callback tracks the download progress.
             */
            await executeApiCall(
                endpoint,
                "GET",
                true, // Indicates that the request requires authentication
                null, // No payload needed for GET request
                "", // No specific toast topic for download-related messages
                false, // Not a file upload, so set to false
                true, // Indicates this is a file download request
                null, // No upload progress handler since we're not uploading a file
                (progressEvent) => {
                    /**
                     * Callback for tracking download progress.
                     *
                     * @param {ProgressEvent} progressEvent - The event object containing progress information.
                     */
                    const total = progressEvent.total; // Total size of the file
                    const current = progressEvent.loaded; // Bytes downloaded so far

                    // Calculate download progress as a percentage
                    const percentage = Math.round((current / total) * 100);

                    // Log the current download progress in the console

                    if (percentage === 100)
                        showToast("success", `File downloaded successfully!`);
                },
            );
        } catch (error) {
            // Log or handle any errors that occur during the download process
            console.error("Error during file download:", error);
            throw error; // Optionally re-throw the error to propagate it further
        }
    };

    /**
     * Downloads a file and forces the browser to use the provided filename.
     */
    const downloadFileAs = async (endpoint, fileName = "download") => {
        if (!accessToken) return showToast("error", "User not logged in.");
        try {
            const response = await axios.get(`${BASE_URL}${endpoint}`, {
                responseType: "blob",
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });

            // Resolve filename from Content-Disposition if present
            const disposition =
                response?.headers?.["content-disposition"] ||
                response?.headers?.["Content-Disposition"];
            let resolvedName = fileName || "download";
            if (disposition && /filename=/i.test(disposition)) {
                const match = disposition.match(
                    /filename\*=UTF-8''([^;\n]+)|filename="?([^";\n]+)"?/i,
                );
                const raw = match ? match[1] || match[2] : null;
                if (raw) {
                    try {
                        resolvedName = decodeURIComponent(raw);
                    } catch {
                        resolvedName = raw;
                    }
                }
            }

            const contentType = response?.headers?.["content-type"] || "";

            // Ensure common extensions when missing
            if (
                /application\/zip/i.test(contentType) &&
                !/\.zip$/i.test(resolvedName)
            ) {
                resolvedName = `${resolvedName}.zip`;
            }

            const blob = new Blob([response.data], {
                type: contentType || undefined,
            });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = resolvedName;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
            showToast("success", `File downloaded successfully!`);

            return {
                headers: response?.headers,
            };
        } catch (error) {
            console.error("Error during file download:", error);
            throw error;
        }
    };

    /**
     * Fetches tickets by project ID.
     * @param {string} endpoint - API endpoint.
     * @param {number|string} id - Project ID.
     * @returns {Promise} - API response.
     */
    const getTicketsByProjectId = (endpoint, id) => {
        if (!accessToken) return showToast("error", "User not logged in.");
        return executeApiCall(`${endpoint}/${id}`, "GET", true)
            .then((response) => response)
            .catch((error) =>
                console.error("Failed to fetch tickets by project ID:", error),
            );
    };

    return {
        getAll,
        getAllWithPostMethod,
        getById,
        getPaginatedList,
        create,
        update,
        remove,
        changePassword,
        updateProfile,
        uploadFile,
        downloadFile,
        downloadFileAs,
        getTicketsByProjectId,
        getWithParams,
        post,
    };
};

export default useApiService;
