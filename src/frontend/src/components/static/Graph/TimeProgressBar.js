import "./TimeProgressBar.css";

const TimeProgressBar = ({
  waitTime,
  doctorVisitTime,
  travelTime,
  totalTime,
}) => {
  // Calculate ratios based on the sum to ensure correct proportions
  const toNum = (v) => {
    const n = Number(v);
    return Number.isFinite(n) && n > 0 ? n : 0;
  };
  const travel = toNum(travelTime);
  const wait = toNum(waitTime);
  const visit = toNum(doctorVisitTime);
  const sum = travel + wait + visit;

  const travelTimePercentage = sum > 0 ? (travel / sum) * 100 : 0;
  const waitTimePercentage = sum > 0 ? (wait / sum) * 100 : 0;
  const doctorVisitTimePercentage = sum > 0 ? (visit / sum) * 100 : 0;
  // Convert time to percentage position along the computed total
  const toPercent = (time) => (sum > 0 ? (time / sum) * 100 : 0);
  // Rounded percentage labels
  const roundPct = (v) => Math.round(v * 10) / 10;

  return (
    <>
    <div className="time-progress-bar">
      {/* Main bar - horizontal segments like PulseRateGraph */}
      <div className="bar">
        {/* Travel time segment (blue) */}
        <div
          className="segment travel-time"
          style={{ flexBasis: `${travelTimePercentage}%` }}
        />
        {/* Wait time segment (yellow) */}
        <div
          className="segment wait-time"
          style={{ flexBasis: `${waitTimePercentage}%` }}
        />
        {/* Doctor visit time segment (green) */}
        <div
          className="segment doctor-visit-time"
          style={{ flexBasis: `${doctorVisitTimePercentage}%` }}
        />
      </div>

      {/* Threshold labels */}
      <span className="range-label" style={{ left: `${toPercent(travel)}%` }}>
        {travel} 
        {/* ({roundPct(travelTimePercentage)}%) */}
      </span>
      <span className="range-label" style={{ left: `${toPercent(travel + wait)}%` }}>
         {wait} 
       {/* ({roundPct(waitTimePercentage)}%) */}
      </span>
      <span className="range-label" style={{ left: `${toPercent(sum)}%` }}>
        {visit} 
        {/* ({roundPct(doctorVisitTimePercentage)}%) */}
      </span>

      {/* Travel time indicator */}
      <div className="value-indicator-tt" style={{ left: `${toPercent(travel / 2)}%` }}>
        <div className="value-box" style={{ backgroundColor: "#3B82F6" }}>
          Travel
        </div>
        <div
          className="value-pointer"
          style={{ borderTopColor: "#3B82F6" }}
        />
      </div>

      {/* Wait time indicator */}
      <div className="value-indicator-wt" style={{ left: `${toPercent(travel + (wait / 2))}%` }}>
        <div
          className="value-pointer value-pointer--down"
          style={{ borderBottomColor: "#e7c905" }}
        />
        <div className="value-box" style={{ backgroundColor: "#e7c905" }}>
          Wait
        </div>
       
      </div>

      {/* Doctor visit time indicator */}
      <div className="value-indicator-dt" style={{ left: `${toPercent(travel + wait + (visit / 2))}%` }}>
        <div className="value-box" style={{ backgroundColor: "#06b815" }}>
          Visit
        </div>
        <div
          className="value-pointer"
          style={{ borderTopColor: "#06b815" }}
        />
      </div>

      {/* Unit label at the right end */}
      <span className="unit-label">
        min
      </span>
    </div>
    
    </>
  );
};

export default TimeProgressBar;
