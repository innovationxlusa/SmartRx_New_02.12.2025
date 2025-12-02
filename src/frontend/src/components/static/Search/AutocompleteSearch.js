import React, { useState } from "react";
import SearchIcon from "../../../assets/img/Vector.svg";


const Autocomplete = ({ suggestions = ["Shahnaj", "Harun", "Raihan"] }) => {
  const [query, setQuery] = useState("");
  const [filteredSuggestions, setFilteredSuggestions] = useState([]);
  const [showSuggestions, setShowSuggestions] = useState(false);

  const handleChange = (e) => {
    const value = e.target.value;
    setQuery(value);
    if (value.length > 0) {
      const filtered = suggestions.filter((suggestion) =>
        suggestion.toLowerCase().includes(value.toLowerCase())
      );
      setFilteredSuggestions(filtered);
      setShowSuggestions(true);
    } else {
      setShowSuggestions(false);
    }
  };

  const handleSelect = (suggestion) => {
    setQuery(suggestion);
    setShowSuggestions(false);
  };

  return (
    <div className="relative w-[339px]">
      <div style={{ margin: "auto", position: "relative", width: "339px" }}>
        <input
          type="text"
          placeholder="Search.."
          name="search2"
          style={{
            border: "2px solid #4B3B8B",
            borderRadius: "10px",
            width: "100%",
            height: "40px",
            paddingLeft: "22px",
            boxShadow: "0px 50px 100px rgba(233, 235, 237, 0.2)",
            outline: "none",
          }}
          value={query}
          onChange={handleChange}
        />
        <img
          src={SearchIcon}
          alt="Search Icon"
          style={{
            position: "absolute",
            right: "22px",
            top: "50%",
            transform: "translateY(-50%)",
            width: "18px",
            height: "18px",
          }}
        />
      </div>
      {showSuggestions && (
        <ul className="absolute w-full bg-white border-gray-300 rounded-lg mt-1 max-h-40 overflow-y-auto ">
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

export default Autocomplete;
