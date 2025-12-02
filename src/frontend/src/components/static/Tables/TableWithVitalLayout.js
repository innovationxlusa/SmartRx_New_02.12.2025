import React, { useState } from "react";
import "./TableWithVitalLayout.css";
import { FaPlusCircle, FaSort, FaSortUp, FaSortDown } from "react-icons/fa";
import SearchByDateRange from "../SearchByDateRange/SearchByDateRange";
import CustomButton from "../Commons/CustomButton";
import { Table } from "react-bootstrap";
import MultiSelectSearch from "../Search/MultiSelectSearch";
import { ReactComponent as RespiratoryRate } from "../../../assets/img/RespiratoryRate.svg";
import { ReactComponent as PulseRate } from "../../../assets/img/PulseRate.svg";
import { ReactComponent as BodyTemperature } from "../../../assets/img/BodyTemperature.svg";
import { ReactComponent as BloodPressure } from "../../../assets/img/BloodPressure.svg";
import { ReactComponent as BloodOxygen } from "../../../assets/img/BloodOxygen.svg";

const TableWithVitalLayout = ({ header = true }) => {
    const [viewType, setViewType] = useState("allLastVital");
    const [dateRange, setDateRange] = useState({ startDate: "", endDate: "" });
    const [sortConfig, setSortConfig] = useState({ key: null, direction: "asc" });

    const vitalsData = [
        {
            name: "Blood Pressure",
            image: "BloodPressure",
            date: "12-02-25 srx-01",
            value: "120mmhg/80mmhg",
            status: "Normal",
            stdDetails: "view",
        },
        {
            name: "Body Temperature",
            image: "BodyTemperature",
            date: "12-02-24 srx-04",
            value: "102°F",
            status: "High",
            stdDetails: "view",
        },
        {
            name: "Pulse Rate",
            image: "PulseRate",
            date: "12-02-25 srx-01",
            value: "80",
            status: "Normal",
            stdDetails: "view",
        },
        {
            name: "Respiratory Rate",
            image: "RespiratoryRate",
            date: "12-03-24 srx-04",
            value: "16 resp/min",
            status: "Normal",
            stdDetails: "view",
        },
        {
            name: "Blood Oxygen",
            image: "BloodOxygen",
            date: "12-02-25 srx-11",
            value: "99%",
            status: "Normal",
            stdDetails: "view",
        },
        {
            name: "Body Weight",
            image: "BloodPressure",
            date: "12-08-25 srx-10",
            value: "98 KG",
            status: "Normal",
            stdDetails: "view",
        },
    ];

    const vitalIcons = {
        BloodPressure: <BloodPressure />,
        BodyTemperature: <BodyTemperature />,
        PulseRate: <PulseRate />,
        RespiratoryRate: <RespiratoryRate />,
        BloodOxygen: <BloodOxygen />,
    };

    const handleSort = (key) => {
        let direction = "asc";
        if (sortConfig.key === key && sortConfig.direction === "asc") {
            direction = "desc";
        }
        setSortConfig({ key, direction });
    };

    const sortedVitals = [...vitalsData].sort((a, b) => {
        if (!sortConfig.key) return 0;
        let valA = a[sortConfig.key];
        let valB = b[sortConfig.key];
        if (sortConfig.key === "date") {
            valA = new Date(valA.split(" ")[0]);
            valB = new Date(valB.split(" ")[0]);
        }
        return sortConfig.direction === "asc" ? (valA > valB ? 1 : -1) : valA < valB ? 1 : -1;
    });

    const renderSortIcon = (key) => {
        if (sortConfig.key === key) {
            return sortConfig.direction === "asc" ? <FaSortUp className="sort-icon" /> : <FaSortDown className="sort-icon" />;
        }
        return <FaSort className="sort-icon" />;
    };

    return (
        <div>
            <div className="d-flex mt-4">
                <div className="vital-button">
                    <input type="radio" id="allLastVital" name="vitalView" value="allLastVital" checked={viewType === "allLastVital"} onChange={(e) => setViewType(e.target.value)} />
                    <label className="btn btn-default" htmlFor="allLastVital">
                        All Last Vital
                    </label>
                </div>
                <div className="vital-button">
                    <input type="radio" id="allVitalView" name="vitalView" value="allVitalView" checked={viewType === "allVitalView"} onChange={(e) => setViewType(e.target.value)} />
                    <label className="btn btn-default" htmlFor="allVitalView">
                        All Vital View
                    </label>
                </div>
            </div>

            <SearchByDateRange onDateChange={setDateRange} />
            <div className="my-2">
                <MultiSelectSearch />
            </div>

            <div className="table-container">
                <Table className="vitals-table" bordered responsive>
                    <thead>
                        {header && (
                            <tr>
                                <th onClick={() => handleSort("name")}>
                                    <div className="d-flex justify-content-center align-items-center">
                                        <div>Vitals Name</div>
                                        <div>{renderSortIcon("name")}</div>
                                    </div>
                                </th>
                                <th onClick={() => handleSort("date")}>
                                    <div className="d-flex justify-content-center align-items-center">
                                        <div>Date REF</div>
                                        <div>{renderSortIcon("date")}</div>
                                    </div>
                                </th>
                                <th className="value-column" onClick={() => handleSort("value")}>
                                    <div className="d-flex justify-content-center align-items-center">
                                        <div>Value</div>
                                        <div>{renderSortIcon("value")}</div>
                                    </div>
                                </th>
                                <th>Std details</th>
                            </tr>
                        )}
                    </thead>
                    <tbody>
                        {sortedVitals.map((vital, index) => (
                            <tr key={index}>
                                <td>
                                    <div className="d-flex flex-column">
                                        <div>{vitalIcons[vital.image] || null}</div>
                                        <div>{vital.name}</div>
                                    </div>
                                </td>
                                <td>{vital.date}</td>
                                <td className="value-column">
                                    {vital.value}
                                    <div className={vital.status === "High" ? "status-high" : "status-normal"}>{vital.status}</div>
                                </td>
                                <td>{vital.stdDetails}</td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </div>
            <div className="d-flex justify-content-end">
                <CustomButton
                    isLoading={false}
                    type="submit"
                    icon={<FaPlusCircle />}
                    label={"Add New Vital"}
                    disabled={false}
                    backgroundColor={"#FAF8FA"}
                    textColor={"black"}
                    shape="rounded"
                    borderColor={"1px solid #AB96D5"}
                    borderStyle={"4px"}
                    width={"127px"}
                    iconStyle={{ color: "var(--theme-font-color)" }}
                    labelStyle={{
                        fontWeight: "normal",
                        textTransform: "none",
                        fontSize: "10px",
                        borderRadius: "4px",
                        color: "var(--theme-font-color)",
                    }}
                />
            </div>
        </div>
    );
};

export default TableWithVitalLayout;
