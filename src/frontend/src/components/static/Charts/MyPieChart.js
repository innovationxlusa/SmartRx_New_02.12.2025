import { PieChart, Pie, Cell, ResponsiveContainer } from "recharts";
import "./Chart.css";
import { BiFontSize } from "react-icons/bi";
import { color } from "framer-motion";

const defaultData = [
    { name: "Doctor", value: 0 },
    { name: "Medicine", value: 0 },
    { name: "Lab", value: 0 },
    { name: "Transport", value: 0 },
    { name: "Others", value: 0 },
];

const COLORS = ["#F99BAB", "#9BDFC4", "#62B2FD", "#9F97F7", "#FFB44F"];

const MyPieChart = ({
    width = "100%",
    height = "100%",
    innerRadius = "35%",
    outerRadius = "73%",
    cy = "50%",
    cx = "50%",
    data,
    circle = true,
    font = false,
}) => {
    
    // Ensure chartData is always an array
    const chartData = Array.isArray(data) ? data : defaultData;
    return (
        <div className="chart-wrapper d-flex flex-column align-items-center">
            <div className="pie-container position-relative">
                <ResponsiveContainer width="100%" height="100%">
                    <PieChart width={width} height={height}>
                        <Pie
                            data={chartData}
                            cx={cx}
                            cy={cy}
                            innerRadius={innerRadius}
                            outerRadius={outerRadius}
                            labelLine={true}
                            stroke="none"
                            strokeWidth="0"
                            dataKey="value"
                        >
                            {chartData.map((entry, index) => (
                                <Cell
                                    key={`cell-${index}`}
                                    fill={COLORS[index % COLORS.length]}
                                />
                            ))}
                        </Pie>
                    </PieChart>
                </ResponsiveContainer>
            </div>
            <div className="legend mb-3">
                {chartData.map((entry, index) => (
                    <div key={entry.name} className="legend-row">
                        <div className="col-5 d-flex fw-bold">
                            <span className="" style={{ color: "#65636e" }}>
                                {entry.name}
                            </span>
                        </div>
                        <span
                            className="circle"
                            style={{
                                backgroundColor: COLORS[index % COLORS.length]
                            }}
                        />
                        <div className="col-5 d-flex">
                            <span className="">{entry.value}</span>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default MyPieChart;
