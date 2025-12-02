import React, {
    useEffect,
    useState,
    useRef,
    useMemo,
    useCallback,
} from "react";
import "./Reward.css";
import PageTitle from "../static/PageTitle/PageTitle";
import investigationIcon from "../../assets/img/InvestigationIcon.svg";
import useApiClients from "../../services/useApiClients";
import { useFetchData } from "../../hooks/useFetchData";
import { useNavigate, useParams } from "react-router-dom";

const RewardBenefitDetails = () => {
    const { api } = useApiClients();
    const navigate = useNavigate();
    const { benefitId } = useParams();
    const [selectedBenefit, setSelectedBenefit] = useState(null);

    const getAllRewardBenefitsWrapper = useCallback(
        (signal, page, rowsPerPage, sortField, sortOrder) => {
            return api.getAllRewardBenefits(
                page,
                rowsPerPage,
                sortField,
                sortOrder,
                signal,
            );
        },
        [api],
    );

    const {
        data: benefitsApiData,
        isLoading,
        error,
        refetch: refetchBenefits,
    } = useFetchData(
        getAllRewardBenefitsWrapper,
        1,
        100,
        "CreatedDate",
        "desc",
        null,
        null,
    );

    const benefits = useMemo(() => {
        if (!benefitsApiData) {
            return [];
        }

        if (
            benefitsApiData &&
            benefitsApiData.data &&
            Array.isArray(benefitsApiData.data)
        ) {
            const mappedBenefits = benefitsApiData.data.map((benefit) => ({
                id: benefit.id,
                heading: benefit.heading,
                details: benefit.details,
                nonCashablePoints: benefit.nonCashablePoints,
                cashablePoints: benefit.cashablePoints,
                isCashable: benefit.isCashable,
                isActive: benefit.isActive,
                icon: investigationIcon,
            }));
            return mappedBenefits;
        } else {
            return [];
        }
    }, [benefitsApiData]);

    // Find the selected benefit based on benefitId from URL params
    useEffect(() => {
        if (benefits.length > 0 && benefitId) {
            const benefit = benefits.find(b => b.id.toString() === benefitId);
            if (benefit) {
                setSelectedBenefit(benefit);
            } else {
                // If benefit not found, redirect back to benefits list
                navigate('/rewardBenefits');
            }
        }
    }, [benefits, benefitId, navigate]);

    return (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle
                        pageName={"Reward Point Benefits"}
                        switchButton={false}
                    />

                    {isLoading && (
                        <div
                            style={{
                                textAlign: "center",
                                padding: "40px 0",
                                color: "#65636e",
                            }}
                        >
                            <div
                                className="spinner-border"
                                role="status"
                                style={{
                                    width: "3rem",
                                    height: "3rem",
                                    color: "#4b3b8b",
                                }}
                            >
                                <span className="visually-hidden">
                                    Loading...
                                </span>
                            </div>
                            <p
                                style={{
                                    marginTop: "20px",
                                    fontSize: "16px",
                                    fontFamily: "Georama",
                                }}
                            >
                                Loading benefit details...
                            </p>
                        </div>
                    )}

                    {error && !isLoading && (
                        <div
                            className="alert alert-warning mt-3"
                            role="alert"
                            style={{ fontFamily: "Georama" }}
                        >
                            <h5>⚠️ Unable to load benefit details</h5>
                            <p>
                                {error?.message ||
                                    "There was an error loading the benefit details."}
                            </p>
                        </div>
                    )}

                    {!isLoading && !error && selectedBenefit && (
                        <div style={{ marginTop: "20px" }}>
                            {/* Section Title */}
                            <div
                                style={{
                                    fontSize: "18px",
                                    fontWeight: "bold",
                                    color: "#4b3b8b",
                                    marginBottom: "10px",
                                    fontFamily: "Georama",
                                    borderBottom: "1px solid #65636e",
                                }}
                            >
                                Benefits with smartRX
                            </div>

                            {/* Benefit Icon and Title */}
                            <div
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: "16px",
                                    marginBottom: "20px",
                                }}
                            >
                                <div
                                    style={{
                                        width: "30px",
                                        height: "30px",
                                        borderRadius: "50%",
                                        backgroundColor: "#ffffff",
                                        border: "2px solid #e8f5e8",
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center",
                                        flexShrink: 0,
                                    }}
                                >
                                    <img
                                        src={selectedBenefit.icon}
                                        alt="Benefit Icon"
                                        style={{
                                            width: "16px",
                                            height: "16px",
                                        }}
                                    />
                                </div>
                                <div
                                    style={{
                                        fontSize: "14px",
                                        color: "#65636e",
                                        fontWeight: "500",
                                        fontFamily: "Georama",
                                    }}
                                >
                                    {selectedBenefit.heading}
                                </div>
                            </div>

                            {/* Benefit Description */}
                            <div
                                style={{
                                    fontSize: "12px",
                                    color: "#65636e",
                                    lineHeight: "1.6",
                                    marginBottom: "24px",
                                    fontFamily: "Georama",
                                }}
                            >
                                {selectedBenefit.details}
                            </div>

                            {/* Benefit Details */}
                            <div>
                                <div
                                    style={{
                                        fontSize: "12px",
                                        display: "flex",
                                        flexDirection: "column",
                                        gap: "4px",
                                    }}
                                >
                                    {/* Hospital Name */}
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                        }}
                                    >
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                                marginRight: "4px",
                                            }}
                                        >
                                            Hospital name:
                                        </span>
                                        <span
                                            style={{
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                                textAlign: "left",
                                            }}
                                        >
                                            Popular diagnostics center
                                        </span>
                                    </div>

                                    {/* Discount */}
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                        }}
                                    >
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                                marginRight: "4px",
                                            }}
                                        >
                                            Discount:
                                        </span>
                                        <span
                                            style={{
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                            }}
                                        >
                                            10%
                                        </span>
                                    </div>

                                    {/* Test Name */}
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                        }}
                                    >
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                                marginRight: "4px",
                                            }}
                                        >
                                            Test Name:
                                        </span>
                                        <span
                                            style={{
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                            }}
                                        >
                                            CVS
                                        </span>
                                    </div>

                                    {/* Address */}
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                        }}
                                    >
                                        <span
                                            style={{
                                                fontWeight: "bold",
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                                marginRight: "4px",
                                            }}
                                        >
                                            Address:
                                        </span>
                                        <span
                                            style={{
                                                color: "#65636e",
                                                fontFamily: "Georama",
                                            }}
                                        >
                                            Dhanmondi, Dhaka - 1219
                                        </span>
                                    </div>
                                </div>

                                {/* Points Information */}
                                {(selectedBenefit.nonCashablePoints > 0 ||
                                    (selectedBenefit.isCashable &&
                                        selectedBenefit.cashablePoints >
                                            0)) && (
                                    <div
                                        style={{
                                            paddingTop: "16px",
                                            display: "flex",
                                            gap: "16px",
                                            fontSize: "12px",
                                        }}
                                    >
                                        {selectedBenefit.nonCashablePoints >
                                            0 && (
                                            <span
                                                style={{
                                                    color: "#10B981",
                                                    fontFamily: "Georama",
                                                }}
                                            >
                                                ✓ Non-Cashable:{" "}
                                                {
                                                    selectedBenefit.nonCashablePoints
                                                }{" "}
                                                pts
                                            </span>
                                        )}
                                        {selectedBenefit.isCashable &&
                                            selectedBenefit.cashablePoints >
                                                0 && (
                                                <span
                                                    style={{
                                                        color: "#3B82F6",
                                                        fontFamily: "Georama",
                                                    }}
                                                >
                                                    💰 Cashable:{" "}
                                                    {
                                                        selectedBenefit.cashablePoints
                                                    }{" "}
                                                    pts
                                                </span>
                                            )}
                                    </div>
                                )}
                            </div>
                        </div>
                    )}

                    {!isLoading && !error && !selectedBenefit && (
                        <div
                            style={{
                                textAlign: "center",
                                padding: "40px 0",
                                color: "#65636e",
                            }}
                        >
                            <p
                                style={{
                                    fontSize: "16px",
                                    fontFamily: "Georama",
                                }}
                            >
                                Benefit not found.
                            </p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default RewardBenefitDetails;
