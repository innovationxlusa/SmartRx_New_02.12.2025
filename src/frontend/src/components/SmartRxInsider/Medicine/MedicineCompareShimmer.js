import { FaHeart } from "react-icons/fa6";
import "./MedicineCompareShimmer.css";

const MedicineCompareShimmer = () => {
    return (
        <div className="px-2 py-3">
            {Array(3)
                .fill(0)
                .map((_, idx) => (
                    <div key={idx} className="rounded-3 shadow-sm p-3 mb-3 bg-light">
                        <div className="d-flex justify-content-between align-items-center">
                            <div className="skeleton-title mb-2 w-75" />
                            <div className="skeleton-line w-50 ms-2" />
                        </div>
                        <div className="d-flex justify-content-between align-items-center">
                            <div className="d-flex justify-content-between align-items-start flex-column">
                                <div className="skeleton-subtitle mb-2" />
                                <div className="skeleton-line mb-2" />
                                <div className="skeleton-line mb-2" />
                                <div className="skeleton-subtitle" />
                                {/* <div className="d-flex justify-content-between align-items-center"><div className="skeleton-line w-25" /></div> */}
                            </div>
                            <div className="d-flex justify-content-center align-items-start flex-column">
                                <div className="skeleton-subtitle mb-2" />
                                <div className="skeleton-subtitle" />
                            </div>
                            <div className="d-flex justify-content-between align-items-start flex-column">
                                <div className="skeleton-subtitle mb-2" />
                                <div className="skeleton-subtitle mb-2" />
                                <div className="skeleton-subtitle" />
                            </div>
                            {/* Wishlist icon skeleton */}
                            <FaHeart className="skeleton-heart-icon" />
                            {/* <div id="skeleton-heart-icon" /> */}
                        </div>
                    </div>
                ))}
        </div>
    );
};

export default MedicineCompareShimmer;
