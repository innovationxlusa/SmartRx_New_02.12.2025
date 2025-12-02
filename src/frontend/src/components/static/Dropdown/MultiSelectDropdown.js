import React, { useState } from "react";
import Form from "react-bootstrap/Form";

function MultiSelectDropdown() {
  const options = [
    { id: "1", label: "One" },
    { id: "2", label: "Two" },
    { id: "3", label: "Three" },
  ];

  const [selectedValues, setSelectedValues] = useState([]);

  const handleSelect = (id) => {
    if (selectedValues.includes(id)) {
      setSelectedValues(selectedValues.filter((value) => value !== id));
    } else {
      setSelectedValues([...selectedValues, id]);
    }
  };

  return (
    <div>
      <Form.Group>
        {options.map((option) => (
          <Form.Check
            key={option.id}
            type="checkbox"
            label={option.label}
            value={option.id}
            checked={selectedValues.includes(option.id)}
            onChange={() => handleSelect(option.id)}
          />
        ))}
      </Form.Group>

      {/* Display selected values */}
      <div className="mt-2">
        <strong>Selected:</strong> {selectedValues.join(", ")}
      </div>
    </div>
  );
}

export default MultiSelectDropdown;
