/**
 * Formats a given date input to a specified format string.
 * Supports tokens like YYYY, MM, DD, HH, mm, ss, and more.
 *
 * @param {string | Date} dateInput - The date input, which can be a Date object, a string, or a timestamp.
 * @param {string} format - The format string. Default is "YYYY-MM-DD".
 * @returns {string} - The formatted date.
 *
 * @example
 * formatDate("2025-04-04T14:05:09", "DD-MM-YYYY") => "04-04-2025"
 * formatDate("2025-04-04", "MM/DD/YYYY") => "04/04/2025"
 * formatDate("2025-04-04T14:05:09", "YYYY-MM-DD HH:mm:ss") => "2025-04-04 14:05:09"
 * formatDate("2025-04-04T14:05:09", "MM/DD/YYYY") // "04/04/2025"
 * formatDateTime24("2025-04-04T14:05:09") // "2025-04-04 14:05:09"
 * formatReadableDate("2025-04-04") // "April 4, 2025"
 * formatTimeAgo("2025-04-04T14:05:09") // "2 hours ago"
 */

/**
 * Formats a given date input (native string, Date object, or timestamp) to a specified format.
 * Supports both "YYYY-MM-DD" native input from <input type="date"> and Date objects.
 *
 * Supported format tokens:
 * - YYYY: Full year (e.g., 2025)
 * - MM: Two-digit month (e.g., 04)
 * - DD: Two-digit day (e.g., 23)
 * - HH: Two-digit hour (24-hour format)
 * - mm: Two-digit minutes
 * - ss: Two-digit seconds
 * - DDD: Full weekday name (e.g., "Wednesday")
 * - MMM: Short month name (e.g., "Apr")
 *
 * @param {string | Date} dateInput - The date input as a native string (YYYY-MM-DD), a Date object, or timestamp.
 * @param {string} format - The format string using tokens. Default is "YYYY-MM-DD".
 * @returns {string} - The formatted date string.
 *
 * @example
 * formatDate("2025-04-23", "DD-MM-YYYY"); // "23-04-2025"
 * formatDate(new Date(), "MMM DD, YYYY"); // "Apr 23, 2025"
 */

export const formatDate = (dateInput, format = "YYYY-MM-DD") => {
    let year,
        month,
        day,
        hours = "00",
        minutes = "00",
        seconds = "00";

    // Check if input is native date string (from input type="date")
    const isoMatch = typeof dateInput === "string" && dateInput.match(/^(\d{4})-(\d{2})-(\d{2})$/);
    if (isoMatch) {
        [, year, month, day] = isoMatch;
    } else {
        // Fallback to Date parsing
        const date = new Date(dateInput);
        if (isNaN(date.getTime())) return "Invalid Date";

        const padZero = (num) => num.toString().padStart(2, "0");
        year = date.getFullYear();
        month = padZero(date.getMonth() + 1);
        day = padZero(date.getDate());
        hours = padZero(date.getHours());
        minutes = padZero(date.getMinutes());
        seconds = padZero(date.getSeconds());
    }

    // Generate full weekday and short month names
    const dateForLocale = new Date(`${year}-${month}-${day}`);
    const dayOfWeek = dateForLocale.toLocaleString("en-us", { weekday: "long" });
    const shortMonth = dateForLocale.toLocaleString("en-us", { month: "short" });

    // Token map
    const map = {
        YYYY: year,
        MM: month,
        DD: day,
        HH: hours,
        mm: minutes,
        ss: seconds,
        DDD: dayOfWeek,
        MMM: shortMonth,
    };

    // Replace tokens in format string
    return format.replace(/YYYY|MM|DD|HH|mm|ss|DDD|MMM/g, (token) => map[token] || token);
};

/**
 * Returns the current date and time formatted according to a given format.
 *
 * @param {string} format - The format string.
 * @returns {string} - The formatted current date and time.
 *
 * @example
 * getCurrentDateTime("YYYY-MM-DD HH:mm:ss") => "2025-04-04 14:05:09"
 */
export const getCurrentDateTime = (format = "YYYY-MM-DD HH:mm:ss") => {
    return formatDate(new Date(), format);
};

/**
 * Gets a human-readable date like "April 4, 2025" or "4th April 2025".
 *
 * @param {string | Date} dateInput - The input date.
 * @param {boolean} [includeDayOfWeek=false] - Whether to include the day of the week (e.g., "Monday").
 * @returns {string} - The formatted date.
 *
 * @example
 * formatReadableDate("2025-04-04") => "April 4, 2025"
 * formatReadableDate("2025-04-04", true) => "Friday, April 4, 2025"
 */
export const formatReadableDate = (dateInput, includeDayOfWeek = false) => {
    const date = new Date(dateInput);
    if (isNaN(date.getTime())) return "Invalid Date";

    const dayOfWeek = date.toLocaleString("en-us", { weekday: "long" });
    const month = date.toLocaleString("en-us", { month: "long" });
    const day = date.getDate();
    const year = date.getFullYear();

    return includeDayOfWeek ? `${dayOfWeek}, ${month} ${day}, ${year}` : `${month} ${day}, ${year}`;
};

/**
 * Formats a date to include the full date and time in 24-hour format (YYYY-MM-DD HH:mm:ss).
 *
 * @param {string | Date} dateInput - The input date.
 * @returns {string} - The formatted date and time.
 *
 * @example
 * formatDateTime24("2025-04-04T14:05:09") => "2025-04-04 14:05:09"
 */
export const formatDateTime24 = (dateInput) => {
    return formatDate(dateInput, "YYYY-MM-DD HH:mm:ss");
};

/**
 * Formats a date into a friendly "Month D, YYYY" format (e.g., "April 4, 2025").
 *
 * @param {string | Date} dateInput - The input date.
 * @returns {string} - The formatted date.
 *
 * @example
 * formatMonthDayYear("2025-04-04") => "April 4, 2025"
 */
export const formatMonthDayYear = (dateInput) => {
    return formatDate(dateInput, "MMMM D, YYYY");
};

/**
 * Returns the time elapsed since a given date in a human-readable format.
 * Example: "2 hours ago", "5 minutes ago".
 *
 * @param {string | Date} dateInput - The input date to compare against the current time.
 * @returns {string} - A human-readable time ago string.
 *
 * @example
 * formatTimeAgo("2025-04-04T14:05:09") => "2 hours ago"
 */
export const formatTimeAgo = (dateInput) => {
    const date = new Date(dateInput);
    const now = new Date();
    const diffInMs = now - date;
    const diffInSec = Math.floor(diffInMs / 1000);
    const diffInMin = Math.floor(diffInSec / 60);
    const diffInHours = Math.floor(diffInMin / 60);
    const diffInDays = Math.floor(diffInHours / 24);

    if (diffInSec < 60) return `${diffInSec} seconds ago`;
    if (diffInMin < 60) return `${diffInMin} minutes ago`;
    if (diffInHours < 24) return `${diffInHours} hours ago`;
    return `${diffInDays} days ago`;
};

export const formatDateDDMMYYYY = (isoString) => {
    if (!isoString) return ""; // graceful fallback for null / undefined
    const date = new Date(isoString); // 2025‑05‑29T15:57:04.2033333
    const day = String(date.getDate()).padStart(2, "0"); // 29
    const month = String(date.getMonth() + 1).padStart(2, "0"); // 05
    const year = date.getFullYear(); // 2025
    return `${day}-${month}-${year}`; // 29‑05‑2025
};

export const formatDateYYYYMMDD = (isoString) => {
    if (!isoString) return ""; // graceful fallback for null / undefined
    const date = new Date(isoString); // 2025‑05‑29T15:57:04.2033333
    const day = String(date.getDate()).padStart(2, "0"); // 29
    const month = String(date.getMonth() + 1).padStart(2, "0"); // 05
    const year = date.getFullYear(); // 2025
    return `${year}/${month}/${day}`; // 2025/05/29
};

// formatTime12Hour (for time like 4:27pm)
export const formatTime12Hour = (isoString) => {
    if (!isoString) return "";

    const date = new Date(isoString);
    let hours = date.getHours();
    const minutes = String(date.getMinutes()).padStart(2, "0");
    const ampm = hours >= 12 ? "pm" : "am";
    hours = hours % 12 || 12; // 0 becomes 12

    return `${hours}:${minutes}${ampm}`;
};
