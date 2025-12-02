import React, { useMemo, useState, useEffect } from 'react'
import './Overview.css'
import CustomAccordion from '../../static/CustomAccordion/CustomAccordion'
import MyPieChart from "../../static/Charts/MyPieChart";
import CustomCard from "../../static/CustomCard/CustomCard";
import DoctorReviewModal from '../DoctorInformation/DoctorReviewModal';
import TimeProgressBar from '../../static/Graph/TimeProgressBar';

const Overview = ({ smartRxInsiderData, fileId, refetch }) => {
    const [modalType, setModalType] = useState(null);
    const [selectedFolder, setSelectedFolder] = useState(null);

    // Local state for editable inputs
    const [travelTime, setTravelTime] = useState(smartRxInsiderData?.smartRxOverview?.totalTravelTime ?? 0);
    const [chamberWaitTime, setChamberWaitTime] = useState(smartRxInsiderData?.smartRxOverview?.totalWaitingTime ?? 0);
    const [transportFee, setTransportFee] = useState(smartRxInsiderData?.smartRxOverview?.totalTransportExpense ?? 0);
    const [doctorFee, setDoctorFee] = useState(smartRxInsiderData?.smartRxOverview?.totalDoctorExpense ?? 0);
    const [visitingTime, setVisitingTime] = useState(smartRxInsiderData?.smartRxOverview?.totalVisitingTime ?? 0);
    const [totalOtherExpense, setTotalOtherExpense] = useState(smartRxInsiderData?.smartRxOverview?.TotalOtherExpense ?? 0);
    const [otherExpenses, setOtherExpenses] = useState(smartRxInsiderData?.smartRxOverview?.OtherExpenses ?? 0);

    useEffect(() => {
        if (smartRxInsiderData?.smartRxOverview) {
            const o = smartRxInsiderData.smartRxOverview;

            setTravelTime(o.totalTravelTime ?? 0);
            setChamberWaitTime(o.totalWaitingTime ?? 0);
            setTransportFee(o.totalTransportExpense ?? 0);
            setDoctorFee(o.totalDoctorExpense ?? 0);
            setVisitingTime(o.totalVisitingTime ?? 0);
            setTotalOtherExpense(o.TotalOtherExpense ?? 0);
            setOtherExpenses(o.OtherExpenses ?? 0);
        }
    }, [smartRxInsiderData]);


    const costChartData = useMemo(() => {
        const overviewRaw = smartRxInsiderData?.smartRxOverview;
        const source = Array.isArray(overviewRaw)
            ? (overviewRaw[0] || {})
            : (overviewRaw || {});

        const toNumber = (v) => {
            const n = Number(v);
            return Number.isFinite(n) ? n : 0;
        };

        const doctor = toNumber(source.totalDoctorExpense ?? 0);
        const medicine = toNumber(source.totalMedicineConsumption ?? 0);
        const lab = toNumber(source.totalLabInvestigationExpense ?? 0);
        const transport = toNumber(source.totalTransportExpense ?? 0);
        const others = toNumber(source.totalOtherExpense ?? 0);

        const items = [
            { name: "Doctor", value: doctor },
            { name: "Medicine", value: medicine },
            { name: "Lab", value: lab },
            { name: "Transport", value: transport },
            { name: "Others", value: others },
        ];

        if (items.length === 0) {
            return [
                { name: "Doctor", value: 0 },
                { name: "Medicine", value: 0 },
                { name: "Lab", value: 0 },
                { name: "Transport", value: 0 },
                { name: "Others", value: 0 },
            ];
        }
        return items;
    }, [smartRxInsiderData?.smartRxOverview]);

    const totalMedicinesPrescribed = useMemo(() => {
        const medicines = smartRxInsiderData?.prescriptions?.[0]?.medicines;

        if (Array.isArray(medicines)) {
            return medicines.length;
        }

        const parsed = Number(medicines);
        return Number.isFinite(parsed) && parsed >= 0 ? parsed : 0;
    }, [smartRxInsiderData?.prescriptions]);

    const totalInvestigationsOrdered = useMemo(() => {
        const investigations = smartRxInsiderData?.prescriptions?.[0]?.investigations;

        if (Array.isArray(investigations)) {
            return investigations.length;
        }

        const parsed = Number(investigations);
        return Number.isFinite(parsed) && parsed >= 0 ? parsed : 0;
    }, [smartRxInsiderData?.prescriptions]);

    /* ───────── Modal helpers ───────── */
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);
    
    return (
        <>
            {/* Cost Chart */}
            <div className="ovInfo mt-1">
                <CustomAccordion
                    accordionHeader={
                        <div style={{ fontSize: "16px" }}>Cost Chart</div>
                    }
                    background="#ffffff"
                    border="1px solid #D9D9D9"
                    borderRadius="4px"
                    shadow={false}
                    defaultOpen={true}
                    iconStyleOverride={{
                        marginRight: "20px",
                        marginTop: "-8px",
                    }}
                >
                    <div className="col-12 extra-small-screen">
                        <div className="d-flex justify-content-center">
                            <MyPieChart data={costChartData} />
                        </div>
                    </div>
                </CustomAccordion>
            </div>

            {/* Medication Card */}
            <div className="ovInfo mt-1">
                <CustomCard>
                    <div className="text-start">
                        <div className="custom-card-header">Medication</div>
                        <div className="d-flex justify-content-between align-items-center px-3">
                            <div className="custom-card-title">
                                Prescribed <br />
                                Medication
                            </div>
                            <div className="custom-card-title-2 text-center">
                                <span>{totalMedicinesPrescribed}</span> <br />{" "}
                                <span>Medicine</span>
                            </div>
                            <div className="custom-card-title-2 text-center">
                                {
                                    smartRxInsiderData?.smartRxOverview
                                        ?.totalMedicineConsumption
                                }{" "}
                                ৳ <br /> Cost
                            </div>
                        </div>
                    </div>
                </CustomCard>
            </div>

            {/* Investigation Card */}
            <div className="ovInfo mt-1">
                <CustomCard>
                    <div className="text-start">
                        <div className="custom-card-header">Investigation</div>
                        <div className="d-flex justify-content-between align-items-center px-3">
                            <div className="custom-card-title">
                                Prescribed <br />
                                Test
                            </div>
                            <div className="custom-card-title-2 text-center">
                                <span>{totalInvestigationsOrdered}</span> <br />{" "}
                                <span>Tests</span>
                            </div>
                            <div className="custom-card-title-2 text-center">
                                {
                                    smartRxInsiderData?.smartRxOverview
                                        ?.totalLabInvestigationExpense
                                }{" "}
                                ৳ <br /> Cost
                            </div>
                        </div>
                    </div>
                </CustomCard>
            </div>

            {/* Add Button */}
            <div className="text-center mt-3 mb-4">
                <button
                    className="add-button"
                    onClick={() => setModalType("add")}
                >
                    Add
                </button>
            </div>

            {/* Other Costs & Waiting Time */}
            <div className="ovInfo mt-1">
                <div className="mini-accordion">
                    <CustomAccordion
                        accordionHeader={
                            <div style={{ fontSize: "16px" }}>
                                Other Costs & Waiting Time
                            </div>
                        }
                        background="#ffffff"
                        border="1px solid #D9D9D9"
                        borderRadius="4px"
                        shadow={false}
                        defaultOpen={true}
                        iconStyleOverride={{
                            marginRight: "20px",
                            marginTop: "-8px",
                        }}
                    >
                        <div className="mini-acc-body">
                            <div className="cost-time-form-container">
                                {/* Travel Time */}
                                <div className="cost-time-row">
                                    <span className="cost-time-label">
                                        Travel Time
                                    </span>
                                    <span className="cost-time-separator">
                                        :
                                    </span>
                                    <div className="cost-time-input-group">
                                        <input
                                            readOnly={true}
                                            type="text"
                                            min={0}
                                            className="cost-time-input"
                                            value={travelTime}
                                            onChange={(e) =>
                                                setTravelTime(
                                                    Number(e.target.value),
                                                )
                                            }
                                        />
                                        <span className="cost-time-unit">
                                            minute
                                        </span>
                                    </div>
                                </div>

                                {/* Chamber Wait Time */}
                                <div className="cost-time-row">
                                    <span className="cost-time-label">
                                        Chamber Wait Time
                                    </span>
                                    <span className="cost-time-separator">
                                        :
                                    </span>
                                    <div className="cost-time-input-group">
                                        <input
                                            readOnly={true}
                                            type="text"
                                            min={0}
                                            className="cost-time-input"
                                            value={chamberWaitTime}
                                            onChange={(e) =>
                                                setChamberWaitTime(
                                                    Number(e.target.value),
                                                )
                                            }
                                        />
                                        <span className="cost-time-unit">
                                            minute
                                        </span>
                                    </div>
                                </div>

                                {/* Doctor visiting time */}
                                <div className="cost-time-row">
                                    <span className="cost-time-label">
                                        Doctor Visiting Time
                                    </span>
                                    <span className="cost-time-separator">
                                        :
                                    </span>
                                    <div className="cost-time-input-group">
                                        <input
                                            readOnly={true}
                                            type="text"
                                            min={0}
                                            className="cost-time-input"
                                            value={visitingTime}
                                            onChange={(e) =>
                                                setVisitingTime(
                                                    Number(e.target.value),
                                                )
                                            }
                                        />
                                        <span className="cost-time-unit">
                                            minute
                                        </span>
                                    </div>
                                </div>
                                {/* Transport Cost */}
                                <div className="cost-time-row">
                                    <span className="cost-time-label">
                                        Transport Cost
                                    </span>
                                    <span className="cost-time-separator">
                                        :
                                    </span>
                                    <div className="cost-time-input-group">
                                        <input
                                            readOnly={true}
                                            type="text"
                                            min={0}
                                            className="cost-time-input"
                                            value={transportFee}
                                            onChange={(e) =>
                                                setTransportFee(
                                                    e.target.value === ""
                                                        ? 0
                                                        : Number(
                                                              e.target.value,
                                                          ) || 0,
                                                )
                                            }
                                        />
                                        <span className="cost-time-unit">
                                            {smartRxInsiderData
                                                ?.prescriptions?.[0].doctor
                                                ?.chamberFeeMeasurementUnit ??
                                                "৳"}
                                        </span>
                                    </div>
                                </div>

                                {/* Doctor Fee */}
                                <div className="cost-time-row">
                                    <span className="cost-time-label">
                                        Doctor Fee
                                    </span>
                                    <span className="cost-time-separator">
                                        :
                                    </span>
                                    <div className="cost-time-input-group">
                                        <input
                                            readOnly={true}
                                            type="text"
                                            min={0}
                                            className="cost-time-input"
                                            value={doctorFee}
                                            onChange={(e) =>
                                                setDoctorFee(
                                                    Number(e.target.value),
                                                )
                                            }
                                        />
                                        <span className="cost-time-unit">
                                            {smartRxInsiderData
                                                ?.prescriptions?.[0].doctor
                                                ?.chamberFeeMeasurementUnit ??
                                                "৳"}
                                        </span>
                                    </div>
                                </div>
                                {/* Other Expense */}
                                <div className="cost-time-row">
                                    <span className="cost-time-label">
                                        Other Expenses
                                    </span>
                                    <span className="cost-time-separator">
                                        :
                                    </span>
                                    <div className="cost-time-input-group">
                                        <input
                                            readOnly={true}
                                            type="text"
                                            min={0}
                                            className="cost-time-input"
                                            value={otherExpenses}
                                            onChange={(e) =>
                                                setOtherExpenses(
                                                    Number(e.target.value),
                                                )
                                            }
                                        />
                                        <span className="cost-time-unit">
                                            {smartRxInsiderData
                                                ?.prescriptions?.[0].doctor
                                                ?.chamberFeeMeasurementUnit ??
                                                "৳"}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div className="other-costs-progressbar-container">
                                <TimeProgressBar
                                    waitTime={chamberWaitTime}
                                    doctorVisitTime={visitingTime} // Assuming 30 minutes for doctor visit time
                                    travelTime={travelTime} // Assuming 20 minutes for travel time
                                    totalTime={
                                        chamberWaitTime +
                                        visitingTime +
                                        travelTime
                                    } // Total time is sum of wait time and visit time
                                />
                            </div>
                        </div>
                    </CustomAccordion>
                </div>
            </div>

            <DoctorReviewModal
                modalType={modalType}
                isOpen={modalType === "add" || modalType === "edit"}
                folderData={selectedFolder}
                onClose={closeModal}
                anotherButton={"true"}
                smartRxMasterId={smartRxInsiderData?.smartRxId}
                prescriptionId={
                    smartRxInsiderData?.prescriptions?.[0]?.prescriptionId
                }
                doctorId={
                    smartRxInsiderData?.prescriptions?.[0]?.doctor?.doctorId
                }
                refetch={refetch}
                smartRxOverview={smartRxInsiderData?.smartRxOverview}
                doctorRating={
                    smartRxInsiderData?.prescriptions?.[0]?.doctor?.doctorRating
                }
                doctorComments={
                    smartRxInsiderData?.prescriptions?.[0]?.doctor
                        ?.doctorComments ||
                    smartRxInsiderData?.prescriptions?.[0]?.doctor?.comments
                }
            />
        </>
    );
}

export default Overview
