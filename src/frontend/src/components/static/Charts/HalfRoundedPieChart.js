import { PieChart, Pie, Cell, LabelList } from "recharts";
import "./Chart.css";

const data = [
    { name: "UNDER", value: 100 },
    { name: "NORMAL", value: 100 },
    { name: "OVER WEIGHT", value: 100 },
    { name: "OBESE", value: 100 },
    { name: "EXTREMELY", value: 100 },
];

const data2 = [
    { name: "< 18.5", value: 100 },
    { name: "18.5 - 24.9", value: 100 },
    { name: "25 - 29.9", value: 100 },
    { name: "30 - 34.9", value: 100 },
    { name: "> 35.0", value: 100 },
];

const COLORS = ["#3EB2F4", "#44B029", "#FEB20C", "#FC4023", "#DE0000"];

const RADIAN = Math.PI / 180;
const renderCustomizedLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, name }) => {
    const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
    const x = cx + radius * Math.cos(-midAngle * RADIAN) + 35;
    const y = cy + radius * Math.sin(-midAngle * RADIAN);

    return (
        <text x={x} y={y} fill="white" textAnchor="end" dominantBaseline="central">
            {name}
        </text>
    );
};

const renderCustomizedLabel2 = ({ cx, cy, midAngle, innerRadius, outerRadius, name }) => {
    const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
    const x = cx + radius * Math.cos(-midAngle * RADIAN) + 25;
    const y = cy + radius * Math.sin(-midAngle * RADIAN);

    return (
        <text x={x} y={y} fill="white" textAnchor="end" dominantBaseline="central">
            {name}
        </text>
    );
};

const HalfRoundedPieChart = ({ width = 200, height = 200, innerRadius = 35, outerRadius = 80, cy = 100, cx = 100 }) => {
    return (
        <div className="chart-font">
            <PieChart width={width} height={height}>
                <Pie
                    data={data}
                    cx={cx}
                    cy={cy}
                    startAngle={180}
                    endAngle={0}
                    innerRadius={innerRadius}
                    outerRadius={outerRadius}
                    label={renderCustomizedLabel}
                    labelLine={true}
                    stroke="none"
                    strokeWidth="0"
                    dataKey="value"
                >
                    {data.map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                </Pie>

                <Pie
                    data={data2}
                    cx={cx}
                    cy={cy}
                    startAngle={180}
                    endAngle={0}
                    innerRadius={innerRadius - 55}
                    outerRadius={innerRadius - 1}
                    label={renderCustomizedLabel2}
                    labelLine={true}
                    stroke="none"
                    strokeWidth="0"
                    dataKey="value"
                >
                    {data.map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                </Pie>
            </PieChart>
        </div>
    );
};

export default HalfRoundedPieChart;
