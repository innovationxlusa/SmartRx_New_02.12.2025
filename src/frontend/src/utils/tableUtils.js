/**
 * Filters the data based on the search query across all fields.
 *
 * @param {Array} data - The array of data objects to filter.
 * @param {string} searchQuery - The search query string to filter the data.
 * @returns {Array} - The filtered data array.
 */
export const filterData = (data, searchQuery) => {
    if (!Array.isArray(data)) return data; // Check for valid data array

    if (!searchQuery) return data; // Return unfiltered data if search query is empty

    const lowerCaseQuery = searchQuery.toLowerCase();

    return data.filter((item) => {
        return Object.values(item).some(
            (value) => value?.toString().toLowerCase().includes(lowerCaseQuery) // Convert each value to string and check if it contains the query
        );
    });
};

/**
 * Filters the data based on the search query and the specified field.
 *
 * @param {Array} data - The array of data objects to filter.
 * @param {string} searchQuery - The search query string to filter the data.
 * @param {string} field - The field name to filter by within each data object.
 * @returns {Array} - The filtered data array.
 */
export const filterDataWithSingleField = (data, searchQuery, field) => {
    if (!Array.isArray(data) || !field) return data; // Check for valid data array and field

    if (!searchQuery) return data; // Return unfiltered data if search query is empty

    return data.filter((item) => {
        const fieldValue = item[field]?.toString().toLowerCase(); // Ensure field value is a string and in lowercase
        return fieldValue?.includes(searchQuery.toLowerCase()); // Compare search query with the field value
    });
};

/**
 * Sorts the data array based on the provided field and order.
 *
 * @param {Array} data - The array of data objects to sort.
 * @param {string} sortField - The field name to sort by.
 * @param {string} sortOrder - The sorting order, either 'asc' or 'desc'.
 * @returns {Array} - The sorted data array.
 */
export const sortData = (data, sortField, sortOrder) => {
    if (!Array.isArray(data) || !sortField || !sortOrder) return data;

    return [...data].sort((a, b) => {
        const x = a[sortField];
        const y = b[sortField];

        if (typeof x === "string" && typeof y === "string") {
            return sortOrder === "asc" ? x.localeCompare(y) : y.localeCompare(x);
        }

        if (typeof x === "number" && typeof y === "number") {
            return sortOrder === "asc" ? x - y : y - x;
        }

        if (x instanceof Date && y instanceof Date) {
            return sortOrder === "asc" ? x.getTime() - y.getTime() : y.getTime() - x.getTime();
        }

        return 0;
    });
};

/**
 * Paginates the data by slicing it into chunks based on the current page and rows per page.
 *
 * @param {Array} data - The array of data objects to paginate.
 * @param {number} page - The current page number (1-based).
 * @param {number} rowsPerPage - The number of rows to display per page.
 * @returns {Array} - The paginated data array.
 */
export const paginateData = (data, page, rowsPerPage) => {
    // if (!Array.isArray(data) || page < 1 || rowsPerPage < 1) return data; // Validate inputs

    // const startIndex = (page - 1) * rowsPerPage;
    // return data.slice(startIndex, startIndex + rowsPerPage); // Slice data for the current page
    const startIndex = page * rowsPerPage; // Calculate the start index for pagination
    const endIndex = startIndex + rowsPerPage; // Calculate the end index for pagination
    return data.slice(startIndex, endIndex); // Return the sliced data for the current page
};

/**
 * Handles the change of sorting field and sorting order.
 * Toggles sort order if the same field is clicked; sets the new field with ascending order otherwise.
 *
 * @param {string} sortField - The current sorting field.
 * @param {Function} setSortField - Function to update the sorting field state.
 * @param {string} sortOrder - The current sorting order ('asc' or 'desc').
 * @param {Function} setSortOrder - Function to update the sorting order state.
 * @returns {Function} - A function to handle sort changes by field.
 */
export const handleSortChange = (sortField, setSortField, sortOrder, setSortOrder) => (field) => {
    if (!field) return; // Validate the field before proceeding

    if (sortField === field) {
        // If the same field is clicked, toggle between ascending and descending order
        setSortOrder(sortOrder === "asc" ? "desc" : "asc");
        // setSortField(field);
    } else {
        // Set the new field and default the order to ascending
        setSortField(field);
        setSortOrder("asc");
    }
};

/**
 * Handles changing the current page.
 *
 * @param {Function} setPage - Function to update the current page state.
 * @returns {Function} - A function to handle page changes.
 */
export const handlePageChange = (setPage) => (event, newPage) => {
    // Ensure page is valid and update it
    if (typeof newPage === "number" && newPage >= 0) {
        setPage(newPage); // Zero-indexed page handling
    }
};

/**
 * Handles changing the number of rows displayed per page.
 * Resets the page to 1 when rows per page change.
 *
 * @param {Function} setRowsPerPage - Function to update the rows per page state.
 * @param {Function} setPage - Function to reset the current page state.
 * @returns {Function} - A function to handle rows per page changes.
 */
export const handleChangeRowsPerPage = (setRowsPerPage, setPage) => (event) => {
    const rowsPerPage = parseInt(event.target.value, 10); // Convert input to number
    if (!isNaN(rowsPerPage) && rowsPerPage > 0) {
        setRowsPerPage(rowsPerPage); // Update the rows per page
        setPage(0); // Reset to the first page (0-indexed)
    }
};

/**
 * Handles the search input changes and resets the pagination.
 *
 * @param {Function} setSearchQuery - Function to update the search query state.
 * @param {Function} setPage - Function to update the page number state (typically for pagination).
 *
 * @returns {Function} - An event handler function that takes an event as its argument and performs the following actions:
 *
 * - Updates the search query state with the value entered in the input field (`event.target.value`).
 * - Resets the page number to 1 (assumes that whenever a new search query is entered, pagination should start from the first page).
 *
 * This function is typically used in search inputs where user input needs to be captured and pagination needs to reset upon every change.
 */
export const handleSearchChange = (setSearchQuery, setPage) => (event) => {
    setSearchQuery(event.target.value);
    setPage(0);
};
