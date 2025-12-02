import { BACKEND_HOST } from "../config/config";

export const BASE_URL = BACKEND_HOST;
export const LOGIN_URL = "auth/login";
export const USER_REGISTER_URL = "user/create";
export const REFRESH_TOKEN_URL = "auth/refresh-token";
export const OTP_VERIFICATION_URL = "auth/verify-otp";
export const PRESCRIPTION_UPLOAD_URL = "prescriptionupload/file-upload";
export const PRESCRIPTION_DOWNLOAD_URL = "prescriptionupload/download";
export const REQUEST_FOR_SMART_RX_URL = "prescriptionupload/update-smartrx-request/";
export const CREATE_NEW_FOLDER_URL = "folder/create-folder";
export const RENAME_FOLDER_URL = "folder/update-folder";
export const DELETE_FOLDER_URL = "folder/delete-folder";
export const FETCH_ALL_FOLDER_URL = "folder/GetAllFolders";
export const FETCH_BROWSE_RX_FOLDER_FILES_URL = "BrowseRx/getallparentfoldersandfiles";
export const MOVE_FILE_URL = "prescriptionupload/update-uploaded-file";
export const TAG_FILE_URL = "prescriptionupload/update-uploaded-file";
export const DELETE_FILE_URL = "prescriptionupload/delete-prescription";
export const FETCH_SMART_RX_INSIDER_URL = "SmartRxInsider/getsmartrxinsiderbyid";
export const FETCH_MEDICINE_LIST_COMPARE_URL = "SmartRxInsider/medicine-list-to-compare";
export const FETCH_TEST_LIST_COMPARE_URL = "SmartRxInsider/investigation-list-to-compare";
export const FETCH_DOCTOR_RECOMMENDED_OR_SELECTED_TEST_LIST_URL = "SmartRxInsider/investigation-list-selected-or-recommended";
export const FETCH_VITAL_URL = "Vital/GetVitalsByVitalName";
export const FETCH_INVESTIGATION_TEST_CENTERS_URL = "SmartRxInsider/investigation-testcenters";
export const EDIT_SMART_RX_INVESTIGATION_WISH_LIST_URL = "SmartRxInsider/edit-smartrx-investigation-wishlist";
export const EDIT_MEDICINE_FAVORITE_URL = "SmartRxInsider/edit-smartrx-medicine-wishlist";
export const EDIT_INVESTIGATION_TEST_CENTERS_URL = "SmartRxInsider/edit-smartrx-investigation-testcenters";
export const CREATE_NEW_VITAL_URL = "SmartRxInsider/add-smartrx-vital";
export const EDIT_VITAL_URL = "SmartRxInsider/edit-smartrx-vital";
export const DELETE_VITAL_URL = "SmartRxInsider/add-smartrx-vital";
export const FETCH_DOCTOR_PROFILE_URL = "Doctor/GetDoctorDetialsById";
export const FETCH_DOCTOR_PROFILE_LIST_URL = "Doctor/GetDoctorProfilesByUserId";
export const FETCH_PATIENT_PROFILE_URL = "PatientProfile/GetPatientDetialsById";
export const UPDATE_PATIENT_PROFILE_URL = "PatientProfile/UpdatePatientInfo/{id}";
export const CREATE_PATIENT_PROFILE_URL = "PatientProfile/CreatePatientProfile";
export const UPDATE_DOCTOR_REVIEW_URL = "SmartRxInsider/change-smartrx-doctor-review";
export const FETCH_MEDICINE_FAQ_URL = "SmartRxInsider/medicine-faq-list";
export const FETCH_INVESTIGATION_FAQ_URL = "SmartRxInsider/investigation-faq-list";
export const FETCH_VITAL_FAQ_URL = "SmartRxInsider/vital-faq-list";
export const DELETE_SMARTRX_VITAL_BY_ID_URL = "SmartRxInsider/smartrx-vital-delete";
export const FETCH_PATIENT_OR_RELATIVE_DROPDOWN_URL = "PatientProfile/GetPatientDropdown";
export const FETCH_PATIENT_PROFILE_LIST_URL = "PatientProfile/GetAllPatientProfilesByUserId";
export const FETCH_PATIENT_VITAL_LIST_URL = "SmartRxInsider/GetAllSmartRxWithVitalsByUserId";
export const FETCH_PATIENT_PRESCRIPTIONS_URL = "BrowseRx/getpatientprescriptions";
export const FETCH_PATIENT_PRESCRIPTIONS_BY_TYPE_URL = "BrowseRx/getpatientprescriptionsbytype";
// Dashboard summary
export const DASHBOARD_SUMMARY_URL = "dashboard/dashboard-summary";
// Reward Benefits
export const GET_ALL_REWARD_BENEFITS_URL = "RewardRule/GetAllVisibleBenefits";
// Reward Points
export const PATIENT_REWARDS_SUMMARY_URL = "RewardTransaction/RewardSummary";
export const UPDATE_PATIENT_REWARD_URL = "PatientReward/UpdatePatientReward";
//Reward Badges List
export const REWARDS_BADGE_LIST_URL = "RewardBadge/GetAllRewardBadges";
//Reward Point Conversions
export const REWARD_POINT_CONVERSIONS_URL = "RewardPointConversions/Convert";
//Patient Rewards by UserId and PatientId
export const GET_PATIENT_REWARDS_BY_USER_ID_AND_PATIENT_ID_URL = "RewardTransaction/RewardDetails";
//Country Code API
export const GET_COUNTRY_CODE_API_URL = "Country/GetAllCountry";
//Dasboard Count
export const DASHBOARD_COUNT_URL = "dashboard/dashboard-counts";