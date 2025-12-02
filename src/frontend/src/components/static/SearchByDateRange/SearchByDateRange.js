import "./SearchByDateRange.css";
import DateField from "./DateField";
import SearchIcon from "../../../assets/img/SearchIcon.svg";

const SearchByDateRange = () => {
    return (
        <div className="date-range-container">
            <div className="overflow-hidden">
                <DateField />
            </div>
            <button>
                <img src={SearchIcon} className="icon-img" height="22px" />
            </button>
            <div className="overflow-hidden">
                <DateField />
            </div>
        </div>
    );
};

export default SearchByDateRange;
