import { FcHighPriority, FcLowPriority } from "react-icons/fc";
import { LuCircleCheckBig } from "react-icons/lu";
import { ReactComponent as Check } from "../assets/img/Check.svg";
import { ReactComponent as High } from "../assets/img/High.svg";
import { ReactComponent as Low } from "../assets/img/Low.svg";

export const MOBILE_NUMBER_LENGTH = 9;

export const BLOOD_GROUPS = {
    1: "A+",
    2: "A-",
    3: "B+",
    4: "B-",
    5: "AB+",
    6: "AB-",
    7: "O+",
    8: "O-",
};

export const STATUS = {
    1: "Active",
    2: "InActive",
};

export const LOGIN_TYPE = {
    1: "UserName",
    2: "Mobile",
    3: "Google",
    4: "Facebook",
    5: "Twitter",
    6: "Email",
};

export const GENDER = {
    1: "Male",
    2: "Female",
};

export const MARITAL_STATUSES = {
    1: "Unmarried",
    2: "Married",
    3: "Divorced",
    4: "Widowed",
    5: "Separated",
};

export const VITALS = {
    1: "BloodPressure",
    2: "BodyTemperature",
    3: "PulseRate",
    4: "RespirationRate",
    5: "BloodOxygen",
    6: "Height",
    7: "Weight",
    8: "BloodGlucoseLevel",
    9: "BodyMassIndex",
};

export const VITALS_OBSERVATION = {
    High: (
        <High
            alt="High"
            className="me-2"
            style={{ height: "22px", width: "22px" }}
        />
    ),
    Overweight: (
        <High
            alt="Overweight"
            className="me-2"
            style={{ height: "22px", width: "22px" }}
        />
    ),
    Normal: <Check alt="Standard" className="me-2" />,
    // Normal: <LuCircleCheckBig alt="Standard" className="me-2 text-white" />,
    Low: (
        <Low
            alt="Low"
            className="me-2"
            style={{ height: "24px", width: "24px" }}
        />
    ),
};

// Avatar color palette for patient/user initials background
export const AVATAR_COLORS = [
    "#4F46E5", // Indigo
    "#EF4444", // Red
    "#10B981", // Green
    "#F59E0B", // Amber
    "#3B82F6", // Blue
    "#EC4899", // Pink
    "#8B5CF6", // Purple
    "#14B8A6", // Teal
    "#F97316", // Orange
    "#06B6D4", // Cyan
    "#8B5A2B", // Brown
    "#DC2626", // Dark Red
    "#16A34A", // Dark Green
    "#7C3AED", // Violet
    "#DB2777", // Deep Pink
    "#0891B2", // Dark Cyan
    "#EA580C", // Dark Orange
    "#059669", // Emerald
    "#9333EA", // Purple
    "#BE123C", // Rose
    "#0D9488", // Dark Teal
    "#CA8A04", // Yellow
    "#6366F1", // Indigo Blue
    "#E11D48", // Crimson
    "#0284C7", // Sky Blue
];

// Utility function to generate consistent color based on name
export const getColorForName = (name = "", colorPalette = AVATAR_COLORS) => {
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
        hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    return colorPalette[Math.abs(hash) % colorPalette.length];
};
