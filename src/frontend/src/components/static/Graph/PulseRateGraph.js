import "./PulseRateGraph.css";

const PulseRateGraph = ({
  value,
  low,  
  high,
  overWeightLow,
  overWeightHigh,
  min,
  max,
  bloodPressure,
  bloodPressureLowerValue,
  isBMI,
  slightlyHigh,
}) => {
  // Clamp the value so it never renders outside the bar
  const clamp = (n, lo, hi) => Math.min(Math.max(n, lo), hi);

  // Convert a reading to a % position along the bar
  const toPercent = (n) => ((clamp(n, min, max) - min) / (max - min)) * 100;
  const isLow = value < low;
  const isSlightlyHigh = value > slightlyHigh && value <= high;
  const isNormal = value >= low && ((isSlightlyHigh ? value <= slightlyHigh : value <= high));
  const isOverWeight =overWeightLow && overWeightHigh &&  (value >= overWeightLow && value <= overWeightHigh);
  const bloodPressureIsNormal =
    bloodPressureLowerValue >= low && bloodPressureLowerValue <= high;
  const indicatorColor =isLow?"#069ab8":(isNormal ? "#06b815" :(isSlightlyHigh && isSlightlyHigh?"#e7c905":(isOverWeight && isOverWeight?"#e7c905":"#ff6347")));
  const bloodPressureIndicatorColor = bloodPressureIsNormal
    ? "#06b815"
    : "#e7c905";

    return (
    <div
      className="pulse-rate-graph"
      style={{ marginBottom: bloodPressure ? "40px" : 0 }}
    > 
      {/* main bar – three flex segments whose widths match the zones */}
      <div className="bar">
  {   
    isBMI ? (
      <>
       
        <div
          className="segment low"
          style={{ flexBasis: `${toPercent(low)}%` }}
        />
        <div
          className="segment normal"
          style={{ flexBasis: `${toPercent(slightlyHigh) - toPercent(low)}%` }}
        />
        {/* <div
          className="segment slightly-high"
          style={{ flexBasis: `${toPercent(high) - toPercent(slightlyHigh)}%` }}
        /> */}
         <div
          className="segment overweight"
          style={{ flexBasis: `${toPercent(overWeightHigh) - toPercent(high)}%` }}
        />
        <div
          className="segment high"
          style={{ flexBasis: `${100 - toPercent(overWeightHigh)}%` }}
        />
      </>
    ) : (
      <>
        <div
          className="segment low"
          style={{ flexBasis: `${toPercent(low)}%` }}
        />
        <div
          className="segment normal"
          style={{ flexBasis: `${toPercent(high) - toPercent(low)}%` }}
        />
        <div
          className="segment high"
          style={{ flexBasis: `${100 - toPercent(high)}%` }}
        />
      </>
    )
  }
</div>


      {/* threshold labels */}
      <span className="range-label" style={{ left: `${toPercent(low)}%` }}>
        {low}
      </span>
      {isBMI && (
        <span className="range-label" style={{ left: `${toPercent(slightlyHigh)}%` }}>
          {slightlyHigh}
        </span>
      )}
      <span className="range-label" style={{ left: `${toPercent(high)}%` }}>
        {high}
      </span>

      {/* Blood Pressure Indicator */}
      {bloodPressure && (
        <div
          className="value-indicator-for-blood-pressure"
          style={{ left: `${toPercent(bloodPressureLowerValue)}%` }}
        >
          <div
            className="value-pointer-for-blood-pressure"
            style={{ borderBottomColor: bloodPressureIndicatorColor }}
          />
          <div
            className="value-box-for-blood-pressure"
            style={{ backgroundColor: bloodPressureIndicatorColor }}
          >
            {bloodPressureLowerValue && bloodPressureLowerValue}
          </div>
        </div>
      )}

      {/* current value indicator */}
      <div className="value-indicator" style={{ left: `${toPercent(value)}%` }}>
        <div className="value-box" style={{ backgroundColor: indicatorColor }}>
          {value}
        </div>
        <div
          className="value-pointer"
          style={{ borderTopColor: indicatorColor }}
        />
      </div>
    </div>
  );
};

export default PulseRateGraph;
