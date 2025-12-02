import useApiService from "./useApiService";
import {
    PRESCRIPTION_UPLOAD_URL,
    PRESCRIPTION_DOWNLOAD_URL,
    REQUEST_FOR_SMART_RX_URL,
    CREATE_NEW_FOLDER_URL,
    FETCH_BROWSE_RX_FOLDER_FILES_URL,
    FETCH_ALL_FOLDER_URL,
    MOVE_FILE_URL,
    TAG_FILE_URL,
    FETCH_SMART_RX_INSIDER_URL,
    FETCH_MEDICINE_LIST_COMPARE_URL,
    FETCH_TEST_LIST_COMPARE_URL,
    FETCH_DOCTOR_RECOMMENDED_OR_SELECTED_TEST_LIST_URL,
    FETCH_MEDICINE_FAQ_URL,
    FETCH_VITAL_URL,
    FETCH_INVESTIGATION_TEST_CENTERS_URL,
    EDIT_SMART_RX_INVESTIGATION_WISH_LIST_URL,
    EDIT_MEDICINE_FAVORITE_URL,
    EDIT_INVESTIGATION_TEST_CENTERS_URL,
    UPDATE_PATIENT_PROFILE_URL,
    CREATE_PATIENT_PROFILE_URL,
    UPDATE_DOCTOR_REVIEW_URL,
    CREATE_NEW_VITAL_URL,
    FETCH_PATIENT_PROFILE_URL,
    FETCH_DOCTOR_PROFILE_URL,
    FETCH_INVESTIGATION_FAQ_URL,
    FETCH_VITAL_FAQ_URL,
    DELETE_SMARTRX_VITAL_BY_ID_URL,
    FETCH_PATIENT_OR_RELATIVE_DROPDOWN_URL,
    FETCH_PATIENT_PRESCRIPTIONS_URL,
    FETCH_PATIENT_PRESCRIPTIONS_BY_TYPE_URL,
    FETCH_PATIENT_PROFILE_LIST_URL,
    FETCH_DOCTOR_PROFILE_LIST_URL,
    FETCH_PATIENT_VITAL_LIST_URL,
    PATIENT_REWARDS_SUMMARY_URL,
    GET_ALL_REWARD_BENEFITS_URL,
    REWARDS_BADGE_LIST_URL,
    UPDATE_PATIENT_REWARD_URL,
    REWARD_POINT_CONVERSIONS_URL,
    GET_PATIENT_REWARDS_BY_USER_ID_AND_PATIENT_ID_URL,
    GET_COUNTRY_CODE_API_URL,
    DASHBOARD_COUNT_URL,
} from "../constants/apiEndpoints";

const useApiClients = () => {
    // Destructure methods from the useApiService hook
    const {
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
        getWithParams,
        post,
    } = useApiService();

    const api = {
        /**
         * SmartRx - Insider -related API methods.
         */

        getSmartRxInsiderByUserId: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_SMART_RX_INSIDER_URL,
                (signal = null),
                payload,
            ),
        getSmartRxInsiderMedicineFAQByMedicineId: (signal, medicineId) =>
            getById(FETCH_MEDICINE_FAQ_URL, medicineId),
        getSmartRxInsiderInvestigationFAQByTestId: (signal, investigationId) =>
            getById(FETCH_INVESTIGATION_FAQ_URL, null, investigationId),
        getSmartRxInsiderVitalFAQByVitalId: (signal, vitalId) =>
            getById(FETCH_VITAL_FAQ_URL, null, vitalId),
        getPatientProfileListById: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_PATIENT_PROFILE_LIST_URL,
                (signal = null),
                payload,
            ),
        getPatientVitalListById: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_PATIENT_VITAL_LIST_URL,
                (signal = null),
                payload,
            ),
        getPatientVitalListFilterById: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_PATIENT_VITAL_LIST_URL,
                signal,
                JSON.stringify(payload),
                { "Content-Type": "application/json" },
            ),
        /**
         * Prescription-related API methods.
         */
        getBrowseRxFolderAndFileList: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_BROWSE_RX_FOLDER_FILES_URL,
                (signal = null),
                payload,
            ),
        getSmartRxInsiderByUserId: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_SMART_RX_INSIDER_URL,
                (signal = null),
                payload,
            ),
        getMedicineListToCompare: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_MEDICINE_LIST_COMPARE_URL,
                (signal = null),
                payload,
            ),
        getTestCenterListToCompare: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_TEST_LIST_COMPARE_URL,
                (signal = null),
                payload,
            ),
        getDoctorRecommendedTestList: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_DOCTOR_RECOMMENDED_OR_SELECTED_TEST_LIST_URL,
                (signal = null),
                payload,
            ),
        getInvestigationTestCenterList: (signal = null) => {
            return getAll(FETCH_INVESTIGATION_TEST_CENTERS_URL, signal);
        },

        // Reward Benefits (Get all)
        getAllRewardBenefits: (
            pageNumber = 1,
            pageSize = 100,
            sortBy = "CreatedDate",
            sortDirection = "desc",
            signal = null,
        ) => {
            const params = new URLSearchParams();
            params.append("pageNumber", pageNumber);
            params.append("pageSize", pageSize);
            params.append("sortBy", sortBy);
            params.append("sortDirection", sortDirection);
            return getAll(
                `${GET_ALL_REWARD_BENEFITS_URL}?${params.toString()}`,
                signal,
            );
        },
        updateInvestigationWishList: (payload, topic) =>
            update(EDIT_SMART_RX_INVESTIGATION_WISH_LIST_URL, payload, topic),
        updateMedicineWishList: (payload, topic) =>
            update(EDIT_MEDICINE_FAVORITE_URL, payload, topic),
        getVitalsByVitalName: ({ VitalName }) => {
            return post(FETCH_VITAL_URL, { VitalName });
        },
        getPatientDataById: ({ patientId }) => {
            return post(FETCH_PATIENT_PROFILE_URL, { patientId });
        },
        createNewVital: (payload, topic) =>
            create(CREATE_NEW_VITAL_URL, payload, topic),
        getSmartRxInsiderMedicineFAQByMedicineId: (signal = null, payload) =>
            getById(FETCH_MEDICINE_FAQ_URL, (signal = null), payload),
        moveFile: (payload, prescriptionId, topic) =>
            update(`${MOVE_FILE_URL}/${prescriptionId}`, payload, topic),
        tagFile: (payload, prescriptionId, topic) =>
            update(`${TAG_FILE_URL}/${prescriptionId}`, payload, topic),
        getAllFoldersByUserId: (signal, userId) =>
            getAll(`${FETCH_ALL_FOLDER_URL}/${userId}`, signal),
        downloadPrescription: async (prescriptionId, fileName) => {
            if (fileName) {
                return await downloadFileAs(
                    `${PRESCRIPTION_DOWNLOAD_URL}/${prescriptionId}`,
                    fileName,
                );
            } else {
                return await downloadFile(
                    `${PRESCRIPTION_DOWNLOAD_URL}/${prescriptionId}`,
                );
            }
        },
        investigationCenterListUpdate: (payload, topic) =>
            update(`${EDIT_INVESTIGATION_TEST_CENTERS_URL}`, payload, topic),
        patientUpdate: (id, payload, topic) =>
            update(
                UPDATE_PATIENT_PROFILE_URL.replace("{id}", id),
                payload,
                topic,
                "PATCH",
                payload instanceof FormData,
            ),
        createPatient: (payload, topic) =>
            create(
                CREATE_PATIENT_PROFILE_URL,
                payload,
                topic,
                payload instanceof FormData,
            ),
        docReviewUpdate: (payload, topic) =>
            update(UPDATE_DOCTOR_REVIEW_URL, payload, topic, "PATCH"),
        patientOrRelativeDropdownLoad: (signal = null, payload) =>
            getById(
                FETCH_PATIENT_OR_RELATIVE_DROPDOWN_URL,
                (signal = null),
                payload,
            ),

        /**
         * Prescription-related API methods.
         */
        prescriptionUpload: (payload, onUploadProgress = null, topic) =>
            uploadFile(
                PRESCRIPTION_UPLOAD_URL,
                payload,
                onUploadProgress,
                topic,
            ),
        requestForSmartRxUpload: (payload, prescriptionId, topic) =>
            update(REQUEST_FOR_SMART_RX_URL + prescriptionId, payload, topic),
        createNewFolder: (payload, topic) =>
            create(CREATE_NEW_FOLDER_URL, payload, topic),

        // Doctor-related API methods.
        getDoctorDetailsById: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_DOCTOR_PROFILE_URL,
                (signal = null),
                payload,
            ),

        // Patient/Relative dropdown for profile details
        getPatientOrRelativeDropdown: (userId, signal = null) =>
            getWithParams(
                FETCH_PATIENT_OR_RELATIVE_DROPDOWN_URL,
                userId ? { userId } : undefined,
            ),

        // Doctor profile list by user id
        getDoctorProfilesByUserId: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_DOCTOR_PROFILE_LIST_URL,
                (signal = null),
                payload,
            ),

        // Patient prescriptions with pagination
        getPatientPrescriptions: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_PATIENT_PRESCRIPTIONS_URL,
                (signal = null),
                payload,
            ),

        // Patient prescriptions by type with pagination
        getPatientPrescriptionsByType: (signal = null, payload) =>
            getAllWithPostMethod(
                FETCH_PATIENT_PRESCRIPTIONS_BY_TYPE_URL,
                (signal = null),
                payload,
            ),
        deleteSmartRxVitalById: (smartRxVitalId, topic, payload = null) =>
            remove(
                DELETE_SMARTRX_VITAL_BY_ID_URL,
                smartRxVitalId,
                topic,
                payload,
            ),

        //Reward Points API
        createPatientReward: (payload, topic) =>
            create(UPDATE_PATIENT_REWARD_URL, payload, topic),
        updatePatientReward: (payload, topic) =>
            update(UPDATE_PATIENT_REWARD_URL, payload, topic),
        getPatientRewardsSummary: (signal, userId) =>
            getAll(`${PATIENT_REWARDS_SUMMARY_URL}?userId=${userId}`, signal),
        getPatientRewardsByUserIdAndPatientId: (signal, payload) =>
            getAllWithPostMethod(
                GET_PATIENT_REWARDS_BY_USER_ID_AND_PATIENT_ID_URL,
                signal,
                payload,
            ),
        getRewardsBadgesList: () => getAll(`${REWARDS_BADGE_LIST_URL}`),

        //Reward Point Conversions API
        convertRewardPoints: (payload, topic) =>
            post(REWARD_POINT_CONVERSIONS_URL, payload, topic),

        //Country Code API
        getAllCountryCodes: (signal = null) =>
            getAll(GET_COUNTRY_CODE_API_URL, signal),

        //Dashboard Counts
        getDashboardCounts: (signal, userId) =>
            getAll(`${DASHBOARD_COUNT_URL}?userId=${userId}`, signal),

        //Patient Dashboard Counts
        getPatientDashboardCounts: (signal, userId, patientId) =>
            getAll(
                `${DASHBOARD_COUNT_URL}?userId=${userId}&patientId=${patientId}`,
                signal,
            ),
    };

    return { api };
};

export default useApiClients;
