/**
 * The `AUTHORITIES` object is a centralized collection of authorization codes
 * representing the permissions required to perform various operations in the application.
 *
 * - **Provides**: A single source of truth for all the permission strings used in the application,
 *   grouped by functionality such as function codes, special numbers, whitelists, roles, users, and search requests.
 *
 * - **Returns**: The `AUTHORITIES` object itself does not return anything as it is not a function.
 *   Instead, it provides key-value pairs where each key is a descriptive constant name, and each value
 *   is the corresponding permission string used in the application.
 *
 * - **Needs**:
 *   1. Proper integration into your application's permission-checking logic to ensure consistency and security.
 *   2. Regular updates to add, modify, or remove permissions as the application evolves and new features are implemented.
 *   3. Usage in conjunction with user roles and permissions logic to control access to different parts of the application.
 */
export const AUTHORITIES = {
    // Function Code Section
    FUNCTION_CODE_POST: "functioncode_post", // Permission to create a new function code entry
    FUNCTION_CODES_GET: "functioncodes_get", // Permission to retrieve function code details
    FUNCTION_CODE_GET: "functioncode_get", // Permission to retrieve function code details
    FUNCTION_CODE_PUT: "functioncode_put", // Permission to update an existing function code
    FUNCTION_CODE_DEL: "functioncode_delete", // Permission to delete a function code
    FUNCTION_CODE_PAGE: "functioncode_page", // Permission to paginate through function code entries

    // Special Number Section
    SPECIAL_NUMBER_POST: "specialnumber_post", // Permission to create a new special number
    SPECIAL_NUMBER_GET: "specialnumbers_get", // Permission to retrieve special number details
    SPECIAL_NUMBER_DEL: "specialnumber_delete", // Permission to delete a special number
    SPECIAL_NUMBER_STATUS: "special_number_status_put", // Permission to update the status of a special number

    // Whitelist Number Section
    WHITELIST_POST: "whitelistnumber_post", // Permission to add a number to the whitelist
    WHITELIST_GET: "whitelistnumbers_get", // Permission to retrieve whitelist numbers
    WHITELIST_DEL: "whitelistnumber_delete", // Permission to delete a number from the whitelist
    WHITELIST_STATUS: "whitelist_number_status_put", // Permission to update the status of a whitelist number

    // Role Management Section
    ROLE_POST: "role_post", // Permission to create a new role
    ROLE_GET: "role_get", // Permission to retrieve role details
    ROLE_PUT: "role_put", // Permission to update an existing role
    ROLE_DEL: "role_delete", // Permission to delete a role

    // User Management Section
    USER_POST: "user_post", // Permission to create a new user
    USER_GET: "user_get", // Permission to retrieve user details
    USER_PUT: "user_put", // Permission to update an existing user
    USER_DEL: "user_delete", // Permission to delete a user

    // Application Audit logs Section
    AUDIT_LOGS_GET: "audit_logs_get", // Permission to retrieve application audit logs

    // Search Requests Section
    CDR_GET: "search_cdr", // Permission to search Call Detail Records (CDR)
    SMS_GET: "search_sms", // Permission to search SMS records
    FNF_GET: "search_fnf", // Permission to search Friends and Family (FNF) records
    LRL_GET: "search_lrl", // Permission to search Local Routing Lookup (LRL) records
    LCL_GET: "search_lcl", // Permission to search Local Carrier Lookup (LCL) records
    ESAF_GET: "search_esaf", // Permission to search ESAF records (Emergency Service Access Framework)
    SUBSCRIBER_GET: "search_subscriber", // Permission to search ESAF records (Emergency Service Access Framework)
    SUBSCRIBER_BALANCE_GET: "search_subscriber_balance",
    SUBSCRIBER_ROAMING_GET: "search_subscriber_roaming",
    INBOUND_ROAMING_GET: "search_inbound_roaming",
    VAS_GET: "search_vas",
    RECHARGE_GET: "search_recharge",
};
