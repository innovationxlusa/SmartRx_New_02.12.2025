import React, { useState, useEffect } from "react";

const CircularProgress = ({ progress }) => {
  const radius = 50;
  const strokeWidth = 10;
  const circumference = 2 * Math.PI * radius;
  const offset = circumference - (progress / 100) * circumference;

  return (
    <svg width="120" height="120" viewBox="0 0 120 120">
      <circle
        cx="60"
        cy="60"
        r={radius}
        fill="transparent"
        stroke="gray"
        strokeWidth={strokeWidth}
      />
      <circle
        cx="60"
        cy="60"
        r={radius}
        fill="transparent"
        stroke="#4acce6"
        strokeWidth={strokeWidth}
        strokeDasharray={circumference}
        strokeDashoffset={offset}
        strokeLinecap="round"
        transform="rotate(-90 60 60)"
      />
      <text
        x="50%"
        y="50%"
        textAnchor="middle"
        dy=".3em"
        className="text-xl font-bold"
      >
        {progress}%
      </text>
    </svg>
  );
};

const Roundedprogressbar = () => {
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setProgress((oldProgress) => (oldProgress >= 100 ? 0 : oldProgress + 10));
    }, 1000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="p-6 flex flex-col items-center">
      <h4 className="text-xl font-bold mb-4">Round Progress Bar</h4>
      <CircularProgress progress={progress} />
    </div>
  );
};

export default Roundedprogressbar;
