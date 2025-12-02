import React, { useState } from "react";
import SearchIcon from "../../../assets/img/Vector.svg";

const MultiSelectSearch = ({
  suggestions = ["Shahnaj", "Harun", "Raihan"],
}) => {
  const [query, setQuery] = useState("");
  const [filteredSuggestions, setFilteredSuggestions] = useState([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  const [selectedValues, setSelectedValues] = useState([]);

  const handleChange = (e) => {
    const value = e.target.value;
    setQuery(value);
    if (value.length > 0) {
      const filtered = suggestions.filter(
        (suggestion) =>
          suggestion.toLowerCase().includes(value.toLowerCase()) &&
          !selectedValues.includes(suggestion)
      );
      setFilteredSuggestions(filtered);
      setShowSuggestions(true);
    } else {
      setShowSuggestions(false);
    }
  };

  const handleSelect = (suggestion) => {
    if (!selectedValues.includes(suggestion)) {
      setSelectedValues([...selectedValues, suggestion]);
    }
    setQuery("");
    setShowSuggestions(false);
  };

  const handleRemove = (value) => {
    setSelectedValues(selectedValues.filter((item) => item !== value));
  };

  return (
    <div className="relative w-[339px]">
      <div style={{ margin: "auto", position: "relative", width: "339px" }}>
        <div
          style={{
            border: "2px solid #4B3B8B",
            borderRadius: "10px",
            width: "100%",
            minHeight: "40px",
            padding: "5px",
            display: "flex",
            flexWrap: "wrap",
            alignItems: "center",
            boxShadow: "0px 50px 100px rgba(233, 235, 237, 0.2)",
          }}
        >
          {selectedValues.map((value, index) => (
            <div
              key={index}
              className="flex items-center bg-blue-500 text-black px-2 py-1 rounded mr-2 mb-1 cursor-pointer"
              onClick={() => handleRemove(value)}
            >
              {value} &times;
            </div>
          ))}
          <input
            type="text"
            placeholder="Search.."
            name="search2"
            style={{
              outline: "none",
              border: "none",
              flex: "1",
              paddingLeft: "5px",
            }}
            value={query}
            onChange={handleChange}
          />
          <img
            src={SearchIcon}
            alt="Search Icon"
            style={{
              width: "18px",
              height: "18px",
              marginLeft: "auto",
              cursor: "pointer",
            }}
          />
        </div>
      </div>
      {showSuggestions && (
        <ul className="absolute w-full bg-white border border-gray-300 rounded-lg mt-1 max-h-40 overflow-y-auto shadow-md">
          {filteredSuggestions.length > 0 ? (
            filteredSuggestions.map((suggestion, index) => (
              <li
                key={index}
                className="p-2 hover:bg-blue-500 hover:text-black cursor-pointer"
                onClick={() => handleSelect(suggestion)}
                style={{ listStyle: "none" }}
              >
                {suggestion}
              </li>
            ))
          ) : (
            <li className="p-2 text-gray-500" style={{ listStyle: "none" }}>
              No results found
            </li>
          )}
        </ul>
      )}
    </div>
  );
};

export default MultiSelectSearch;
