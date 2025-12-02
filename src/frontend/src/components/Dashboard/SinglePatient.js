import "./SinglePatient.css";
import Card from "./DashboardCard";
import React, { useEffect, useState, useMemo } from "react";
import { Link, useParams } from "react-router-dom";
import MyPieChart from "../static/Charts/MyPieChart";
import doctorIcon from "../../assets/img/Doctor.svg";
import folderIcon from "../../assets/img/Folder.svg";
import vitalsIcon from "../../assets/img/Vitals.svg";
import PatientVitalList from "../BrowseRx/PatientVitalList";
import PageTitle from "../static/PageTitle/PageTitle";
import MyDonutChart from "../static/Charts/MyDonutChart";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import useApiClients from "../../services/useApiClients";
import ProfileProgress from "../PatientProfile/ProfileProgress";
import ProfilePicture from "../PatientProfile/ProfilePicture";
import { useLocation } from "react-router-dom";
import DefaultProfilePhoto from "../../assets/img/DefaultProfilePhoto.svg";
import useApiService from "../../services/useApiService";
import { DASHBOARD_SUMMARY_URL } from "../../constants/apiEndpoints";
import { getColorForName } from "../../constants/constants";
import { useFetchData } from "../../hooks/useFetchData";
import useCurrentUserId from "../../hooks/useCurrentUserId";


const SinglePatient = () => {
    const { patientId } = useParams();
    const { api } = useApiClients();
    const { smartNavigate } = useSmartNavigate({ scroll: "top" });
    const { getWithParams } = useApiService();
    const [patient, setPatient] = useState(null);
    const userId = useCurrentUserId();

    // State for dashboard data
    const [dashboardData, setDashboardData] = useState({
        userSummary: null,
        expenseSummary: null
    });
    const [isDashboardLoading, setIsDashboardLoading] = useState(true);
    const [dashboardError, setDashboardError] = useState(null);

    useEffect(() => {
        const fetchPatient = async () => {
            try {
                if (patientId) {
                    const res = await api.getPatientDataById({ patientId });
                    setPatient(res);
                }
            } catch (error) {
                console.error("Error fetching patient:", error);
            }
        };
        fetchPatient();
    }, [patientId]);

    const getPatientDashboardCountsWrapper = useMemo(() => {
        return (signal, payload) => {
            if (payload && payload.userId && payload.patientId) {
                return api.getPatientDashboardCounts(
                    signal,
                    Number(payload.userId),
                    payload.patientId
                );
            }
            return Promise.resolve(null);
        };
    }, []);

    const {
        data: countApiData,
        isLoading: isCountLoading,
        error: countError,
        refetch: refetchCounts,
    } = useFetchData(
        getPatientDashboardCountsWrapper,
        null,
        null,
        null,
        null,
        userId && patientId ? { userId: Number(userId), patientId } : null,
        patientId,
    );

    // Fetch dashboard summary data for specific patient
    useEffect(() => {
        const fetchDashboardData = async () => {
            try {
                setIsDashboardLoading(true);
                setDashboardError(null);

                const response = await getWithParams(DASHBOARD_SUMMARY_URL, {
                    userId: Number(userId),
                    patientId: patientId
                });
                
                if (response) {
                    // Handle different response structures
                    const responseData = response.data || response;

                    setDashboardData({
                        userSummary: responseData.userSummary || null,
                        expenseSummary: responseData.expenseSummary
                            ? responseData.expenseSummary
                            : null
                    });
                }
            } catch (err) {
                console.error("Error fetching dashboard data:", err);
                setDashboardError("Failed to load dashboard data");
            } finally {
                setIsDashboardLoading(false);
            }
        };

        // Only fetch dashboard data if we have both userId and patientId
        if (userId && patientId) {
            fetchDashboardData();
        }
    }, [userId, patientId]);

    const fullName = useMemo(() => {
        if (!patient) return "Loading...";
        const firstName = patient.firstName || patient.patientFirstName || '';
        const lastName = patient.lastName || patient.patientLastName || '';
        const nickName = patient.nickName || patient.patientNickName || '';
        const name = `${firstName} ${lastName} ${nickName}`.trim();
        return name || "Loading...";
    }, [patient]);
    
    const progress = useMemo(() => {
        return patient?.profileProgress ?? 0;
    }, [patient?.profileProgress]);

    // Default expense data for pie chart fallback
    const defaultExpenseData = [];


    // Use API data if available, otherwise use default
    // Convert object to array format for pie chart
    const expenseData = (() => {
        if (dashboardData.expenseSummary && typeof dashboardData.expenseSummary === 'object') {
            // If it's an object, convert to array format
            if (Array.isArray(dashboardData.expenseSummary)) {
                return dashboardData.expenseSummary;
            } else {
                // Convert object to array format
                return [
                    {
                        name: "Doctor",
                        value:
                            dashboardData.expenseSummary.totalDoctorsCost || 0,
                    },
                    {
                        name: "Medicine",
                        value:
                            dashboardData.expenseSummary.totalMedicinesCost ||
                            0,
                    },
                    {
                        name: "Lab",
                        value: dashboardData.expenseSummary.totalTestsCost || 0,
                    },
                    {
                        name: "Transport",
                        value:
                            dashboardData.expenseSummary.totalTransportCost ||
                            0,
                    },
                    {
                        name: "Others",
                        value:
                            dashboardData.expenseSummary.totalOtherCosts || 0,
                    },
                ];
            }
        }
        return defaultExpenseData;
    })();

    const userSummary = dashboardData.userSummary;
    const location = useLocation();
    const coloredName = location.state?.coloredName;
    const colorForDefaultName = location.state?.colorForDefaultName;

    const generateInitials = useMemo(() => {
        if (patient?.firstName || patient?.lastName) {
            const firstInitial = patient.firstName?.charAt(0)?.toUpperCase() || "";
            const lastInitial = patient.lastName?.charAt(0)?.toUpperCase() || "";
            if (firstInitial && lastInitial) {
                return firstInitial + lastInitial;
            }
            return firstInitial || lastInitial || "P";
        }
        return "P";
    }, [patient?.firstName, patient?.lastName]);

    const patientBackgroundColor = useMemo(() => {
        if (patient && fullName !== "Loading...") {
            return getColorForName(fullName);
        }
        return colorForDefaultName || "#e6e4ef";
    }, [patient, fullName, colorForDefaultName]);

    return (
        <div className="col-12 col-md-7 mx-auto ">
            <PageTitle
                backButton={true}
                pageName={fullName}
                switchButton={true}
                noMargin={true}
                showProfilePicture={true}
                isSinglePatientView={true}
                progressData={progress}
                patientData={patient}
                profileData={{
                    profilePhotoPath: patient?.profilePhotoPath || "",
                    picture: patient?.profilePhotoPath || "",
                    coloredName: coloredName || generateInitials,
                    colorForDefaultName: patientBackgroundColor,
                }}
            ></PageTitle>
            <div className="container chart-section">
                {isDashboardLoading && (
                    <div className="text-center py-4">
                        <div
                            className="spinner-border text-primary"
                            role="status"
                        >
                            <span className="visually-hidden">Loading...</span>
                        </div>
                        <p
                            className="mt-2"
                            style={{ fontFamily: "Georama", color: "#65636e" }}
                        >
                            Loading dashboard data...
                        </p>
                    </div>
                )}

                {dashboardError && (
                    <div
                        className="alert alert-danger text-center"
                        role="alert"
                    >
                        {dashboardError}
                    </div>
                )}

                {!isDashboardLoading && !dashboardError && (
                    <div className="row g-3">
                        <div className="col-6 col-lg-6 col-md-6">
                            <div
                                style={{
                                    color: "#65636e",
                                    fontWeight: "600",
                                    fontFamily: "Georama",
                                    fontSize: "14px",
                                    textAlign: "center",
                                    display: "flex",
                                    justifyContent: "center",
                                    alignItems: "center",
                                    width: "100%",
                                    marginBottom: "2px",
                                }}
                            >
                                Entity Count Chart
                            </div>
                            <div
                                className="chart-container d-flex justify-content-center"
                                style={{
                                    height: "100%",
                                    minHeight: "250px",
                                }}
                            >
                                <MyDonutChart
                                    userSummary={userSummary}
                                    width="100%"
                                    height="100%"
                                    cx="50%"
                                    cy="50%"
                                />
                            </div>
                        </div>
                        <div className="col-6 col-lg-6 col-md-6">
                            <div
                                style={{
                                    color: "#65636e",
                                    fontWeight: "600",
                                    fontFamily: "Georama",
                                    fontSize: "14px",
                                    textAlign: "center",
                                    display: "flex",
                                    justifyContent: "center",
                                    alignItems: "center",
                                    width: "100%",
                                    marginBottom: "2px",
                                }}
                            >
                                Cost Chart
                            </div>
                            <div
                                className="chart-container d-flex justify-content-center"
                                style={{
                                    height: "100%",
                                    minHeight: "250px",
                                }}
                            >
                                <MyPieChart
                                    data={expenseData}
                                    width="100%"
                                    height="100%"
                                    innerRadius="35%"
                                    outerRadius="73%"
                                    cx="50%"
                                    cy="50%"
                                />
                            </div>
                        </div>
                    </div>
                )}
            </div>

            <div className="container card-container">
                <div className="row">
                    <div className="col-6">
                        <div
                            role="button"
                            className="d-flex justify-content-center"
                        >
                            <div
                                style={{
                                    width: "100%",
                                    aspectRatio: "1 / 1",
                                }}
                                onClick={() =>
                                    smartNavigate(`/browserx/${patientId}`)
                                }
                            >
                                <Card
                                    logo={folderIcon}
                                    title="Browse Rx/SRx"
                                    count={
                                        countApiData?.apiResponseResult?.data
                                            ?.totalPrescriptionCount || 0
                                    }
                                    bgColor="#B1AACF"
                                    color="var(--text-white)"
                                    fontSize="15px"
                                    titleBottomPosition="4px"
                                />
                            </div>
                        </div>
                    </div>
                    <div className="col-6 vital-list">
                        <div className="d-flex justify-content-center">
                            <Link
                                to="/patientVitalList"
                                state={{
                                    userId: Number(userId ?? 0),
                                    patientId: patientId,
                                    patientName: fullName,
                                }}
                                style={{
                                    width: "100%",
                                    aspectRatio: "1 / 1",
                                }}
                            >
                                <Card
                                    logo={vitalsIcon}
                                    title="Vitals"
                                    count={
                                        countApiData?.apiResponseResult?.data
                                            ?.totalPatientCountForVital || 0
                                    }
                                    bgColor="#E6E4EF"
                                    color="var(--theme-font-color)"
                                    fontSize="15px"
                                    titleBottomPosition="4px"
                                />
                            </Link>
                        </div>
                    </div>
                </div>

                <div className="row my-3">
                    <div className="col-6">
                        <div className="d-flex justify-content-center">
                            <Link
                                to="/doctorlist"
                                state={{
                                    userId: Number(userId ?? 0),
                                    patientId: patientId,
                                    patientName: fullName,
                                }}
                                style={{
                                    width: "100%",
                                    aspectRatio: "1 / 1",
                                }}
                            >
                                <Card
                                    logo={doctorIcon}
                                    title="Doctor"
                                    count={
                                        countApiData?.apiResponseResult?.data
                                            ?.totalActiveDoctorCount || 0
                                    }
                                    bgColor="#E6E4EF"
                                    color="var(--theme-font-color)"
                                    fontSize="15px"
                                    titleBottomPosition="4px"
                                />
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default SinglePatient;

