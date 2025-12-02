import { PieChart, Pie, Cell, ResponsiveContainer } from "recharts";
import "./Chart.css";

// Colors for inner ring (first 3 categories)
const INNER_COLORS = ["#FA9B31", "#EC3762", "#D9D9D9"];

// Colors for outer ring (remaining 3 categories)
const OUTER_COLORS = ["#D9D9D9", "#0066EE", "#FA9B31"];

const MyDonutChart = ({ 
    userSummary,
    width = "100%",
    height = "100%",
    cy = "50%",
    cx = "50%"
}) => {
  // Default data for fallback
  const defaultData = [
    { name: "Patient",  value: 1  },
    { name: "Doctor",   value: 1 },
    { name: "RX File",  value: 1 },
    { name: "Smart RX", value: 1 },
    { name: "Pending",  value: 1 },
    { name: "EdRX",     value: 1 },
  ];

  // Create data from userSummary if available, otherwise use default
  const rawData = userSummary ? [
    { name: "Patient",  value: userSummary.totalPatients || 0  },
    { name: "Doctor",   value: userSummary.totalDoctors || 0 },
    { name: "RX File",  value: userSummary.totalRxFileOnly || 0 },
    { name: "Smart RX", value: userSummary.totalSmartRx || 0 },
    { name: "Pending RX",  value: userSummary.totalPending || 0 },
    { name: "EdRX",     value: userSummary.edRxCount || 0 },
  ] : defaultData;

  // Filter out values that are <= 0
  const data = rawData.filter(item => item.value >= 0);
  
  const total = data.reduce((sum, d) => sum + d.value, 0);

  // Handle empty data case
  if (data.length === 0) {
    return (
      <div className="chart-wrapper d-flex flex-column align-items-center">
        <div className="pie-container position-relative">
          <div className="d-flex justify-content-center align-items-center" style={{ height: "200px" }}>
            <span style={{ color: "#65636e" }}>No data available</span>
          </div>
        </div>
      </div>
    );
  }

  // Split data into two groups for nested pies
  // Inner ring: First 3 categories (Patient, Doctor, RX File)
  const innerData = data.slice(0, 3).filter(item => item.value > 0);
  
  // Outer ring: Remaining categories (Smart RX, Pending, EdRX)
  const outerData = data.slice(3).filter(item => item.value > 0);

  // Combine all data for legend with proper colors
  const allData = [...data];
  const allColors = [
    ...INNER_COLORS.slice(0, Math.min(3, data.length)),
    ...OUTER_COLORS.slice(0, Math.max(0, data.length - 3))
  ];

  return (
      <div className="chart-wrapper d-flex flex-column align-items-center">
          <div className="pie-container position-relative">
              <ResponsiveContainer width="100%" height="100%">
                  <PieChart width={width} height={height}>
                      {/* Inner Pie Ring - First 3 categories */}
                      {innerData.length > 0 && (
                          <Pie
                              data={innerData}
                              cx={cx}
                              cy={cy}
                              innerRadius="40%"
                              outerRadius="55%"
                              dataKey="value"
                              stroke="none"
                          >
                              {innerData.map((_, i) => (
                                  <Cell
                                      key={`inner-cell-${i}`}
                                      fill={INNER_COLORS[i % INNER_COLORS.length]}
                                  />
                              ))}
                          </Pie>
                      )}
                      
                      {/* Outer Pie Ring - Remaining categories */}
                      {outerData.length > 0 && (
                          <Pie
                              data={outerData}
                              cx={cx}
                              cy={cy}
                              innerRadius="60%"
                              outerRadius="75%"
                              dataKey="value"
                              stroke="none"
                          >
                              {outerData.map((_, i) => (
                                  <Cell
                                      key={`outer-cell-${i}`}
                                      fill={OUTER_COLORS[i % OUTER_COLORS.length]}
                                  />
                              ))}
                          </Pie>
                      )}
                  </PieChart>
              </ResponsiveContainer>

              <span className="totalCount position-absolute top-50 start-50 translate-middle">
                  {total}
              </span>
          </div>
          <div className="legend mb-3">
              {allData.map((entry, i) => (
                  <div key={entry.name} className="legend-row">
                      <div className="col-5 d-flex fw-bold">
                          <span className="" style={{ color: "#65636e",  }}>
                              {entry.name}
                          </span>
                      </div>
                      <span
                          className="circle"
                          style={{ backgroundColor: allColors[i]}}
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

export default MyDonutChart;