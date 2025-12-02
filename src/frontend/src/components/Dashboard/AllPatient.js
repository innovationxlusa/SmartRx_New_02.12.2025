import "./AllPatient.css";
import Card from "./DashboardCard";
import { Link } from "react-router-dom";
import MyPieChart from "../static/Charts/MyPieChart";
import doctorIcon from "../../assets/img/Doctor.svg";
import folderIcon from "../../assets/img/Folder.svg";
import vitalsIcon from "../../assets/img/Vitals.svg";
import PageTitle from "../static/PageTitle/PageTitle";
import MyDonutChart from "../static/Charts/MyDonutChart";
import useSmartNavigate from "../../hooks/useSmartNavigate";
import patientProfileIcon from "../../assets/img/PatientProfileIcon.svg";
import { useState, useEffect, useMemo } from "react";
import useApiService from "../../services/useApiService";
import { useUserContext } from "../../contexts/UserContext";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import { useAuthContext } from "../../contexts/AuthContext";
import { DASHBOARD_SUMMARY_URL } from "../../constants/apiEndpoints";
import useApiClients from "../../services/useApiClients";
import { useFetchData } from "../../hooks/useFetchData";
import { AUTH_REDIRECT_STORAGE_KEY } from "../../constants/navigation";
import axios from "axios";
import RewardPopup from "../Reward/RewardPopup";
import ConsumePopup from "../Reward/ConsumePopup";


const AllPatient = () => {

    sessionStorage.removeItem(AUTH_REDIRECT_STORAGE_KEY);

    const { smartNavigate } = useSmartNavigate({ scroll: "top" });
    const { user } = useUserContext();
    const userId = useCurrentUserId();
    const { getWithParams } = useApiService();
    const { api } = useApiClients();
    const { accessToken } = useAuthContext();
    
        // if (intendedRoute) {
        //     setIntendedRoute("");
        // }
    // State for dashboard data
    const [dashboardData, setDashboardData] = useState({
        userSummary: null,
        expenseSummary: null
    });
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showReward, setShowReward] = useState(false);
    const [showConsume, setShowConsume] = useState(false);

    const consumePoints = () => {
        setShowConsume(true);

        // Auto close (optional)
        // setTimeout(() => setShowReward(false), 3000);
    };

    // Fetch dashboard summary data
    useEffect(() => {

        const fetchDashboardData = async () => {
            try {
                setIsLoading(true);
                setError(null);
                if (!userId) {
                    setDashboardData({
                        userSummary: null,
                        expenseSummary: null
                    });
                    return;
                }

                const response = await getWithParams(DASHBOARD_SUMMARY_URL, {
                    userId: userId
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
                setError("Failed to load dashboard data");
            } finally {
                setIsLoading(false);
            }
        };

        // Always fetch data, regardless of user state
        fetchDashboardData();
    }, [userId]);


    const {
        data: countApiData,
        isLoading: isCountLoading,
        error: countError,
        refetch: refetchCounts,
    } = useFetchData(
        userId ? api.getDashboardCounts : null,
        null,
        null,
        null,
        null,
        userId ?? undefined,
        null
        );


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

    return (
        <>
            <div className="col-12 col-md-7 mx-auto ">
                <PageTitle
                    backButton={false}
                    pageName={"All Patient"}
                    switchButton={true}
                />
                <div className="container chart-section">
                    {isLoading && (
                        <div className="text-center py-4">
                            <div
                                className="spinner-border text-primary"
                                role="status"
                            >
                                <span
                                    className="visually-hidden"
                                    style={{ fontFamily: "Georama" }}
                                >
                                    Loading...
                                </span>
                            </div>
                            <p
                                className="mt-2"
                                style={{ fontFamily: "Georama", color: "#65636e" }}
                            >
                                Loading dashboard data...
                            </p>
                        </div>
                    )}

                    {error && (
                        <div
                            className="alert alert-danger text-center"
                            style={{ fontFamily: "Georama" }}
                            role="alert"
                        >
                            {error}
                        </div>
                    )}

                    {!isLoading && !error && (
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
                                    onClick={() => smartNavigate("/browserx")}
                                >
                                    <Card
                                        logo={folderIcon}
                                        title="Browse Rx/SRx"
                                        count={
                                            countApiData?.apiResponseResult
                                                ?.data
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
                        <div className="col-6">
                            <div className="d-flex justify-content-center">
                                <Link
                                    to="/patientVitalList"
                                    style={{
                                        width: "100%",
                                        aspectRatio: "1 / 1",
                                    }}
                                >
                                    <Card
                                        logo={vitalsIcon}
                                        title="Vitals"
                                        count={
                                            countApiData?.apiResponseResult
                                                ?.data
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
                                    to="/patientProfile"
                                    style={{
                                        width: "100%",
                                        aspectRatio: "1 / 1",
                                    }}
                                >
                                    <Card
                                        logo={patientProfileIcon}
                                        title="All Patient's List"
                                        count={
                                            countApiData?.apiResponseResult
                                                ?.data
                                                ?.totalActivePatientCount || 0
                                        }
                                        bgColor="#E6E4EF"
                                        color="var(--theme-font-color)"
                                        fontSize="15px"
                                        titleBottomPosition="4px"
                                    />
                                </Link>
                            </div>
                        </div>
                        <div className="col-6">
                            <div className="d-flex justify-content-center">
                                <Link
                                    to="/doctorlist"
                                    state={{
                                        userId: userId ?? 0,
                                        patientId: null,
                                    }}
                                    style={{
                                        width: "100%",
                                        aspectRatio: "1 / 1",
                                    }}
                                >
                                    <Card
                                        logo={doctorIcon}
                                        title="Doctor Profile"
                                        count={
                                            countApiData?.apiResponseResult
                                                ?.data
                                                ?.totalActiveDoctorCount || 0
                                        }
                                        bgColor="#B1AACF"
                                        color="var(--text-white)"
                                        fontSize="15px"
                                        titleBottomPosition="4px"
                                    />
                                </Link>
                            </div>
                        </div>
                    </div>
                    {/* <button className="btn btn-primary" onClick={consumePoints}>
                        Consume Points
                    </button> */}
                </div>
            </div>
            {/* <RewardPopup
                isOpen={showReward}
                onClose={() => setShowReward(false)}
                points={100}
            /> */}

            {/* <ConsumePopup
                isOpen={showConsume}
                consumeAmount={30}
                onClose={() => setShowConsume(false)}
                message="30 points have been redeemed from your account."
            /> */}

        </>
    );
};

export default AllPatient;
