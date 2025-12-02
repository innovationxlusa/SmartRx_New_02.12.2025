import React, { useState } from "react";

const TabComponent = ({ tabs, tabStyle, activeTabStyle, contentStyle }) => {
  const [activeTab, setActiveTab] = useState(0);

  return (
    <div className="">
      {/* Tab Buttons */}
      <div className="flex border-b border-gray-300">
        {tabs.map((tab, index) => (
          <button
            key={index}
            onClick={() => setActiveTab(index)}
            style={activeTab === index ? activeTabStyle : tabStyle}
            className={`px-4 py-3 text-lg font-semibold focus:outline-none`}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Tab Content */}
      <div className={`p-4 mt-2 rounded-b-md shadow-md ${contentStyle}`}>
        <p>{tabs[activeTab].content}</p>
      </div>
    </div>
  );
};

export default TabComponent;
