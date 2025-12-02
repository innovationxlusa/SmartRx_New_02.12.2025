import './Favorites.css';
import { ReactComponent as PriceComparison } from "../../../assets/img/PriceComparison.svg";

const Favorites = ({ smartRxInsiderData }) => {
    return (
        <div className="favorite-wrapper">
            <div className="row g-3">
                {/* Medicine Wishlist */}
                <div>
                    <h2 className="favorite-section-title">
                        Your Favorite Medicines
                    </h2>

                    {smartRxInsiderData?.prescriptions?.[0]?.medicineWishlist.map(
                        (item) => (
                            <div className="col-12" key={`med-${item.id}`}>
                                <div className="favorite-card">
                                    <h6 className="ms-4">
                                        <span className="favorite-title">
                                            {item.medicineName}
                                        </span>
                                        <span className="favorite-subtitle ms-2">
                                            {" "}
                                            Your Favorite Alternatives are:
                                        </span>
                                    </h6>

                                    <div className="ms-4 ps-2">
                                        <div className="favorite-timeline">
                                            {Object.entries(
                                                item.wishedMedicines,
                                            ).map(([key, name]) => (
                                                <div
                                                    className="favorite-timeline-item"
                                                    key={key}
                                                >
                                                    <div className="favorite-timeline-dot"></div>
                                                    <div className="favorite-timeline-content">
                                                        <PriceComparison
                                                            height="15"
                                                            fill="#e6e4ef"
                                                            className="me-2 favorite-icon"
                                                            style={
                                                                {
                                                                    // color: "#4b3b8b",
                                                                }
                                                            }
                                                        />
                                                        <span className="favorite-alt-name">
                                                            {name}
                                                        </span>
                                                    </div>
                                                </div>
                                            ))}
                                            </div>
                                    </div>
                                </div>
                            </div>
                        ),
                    )}
                </div>
                {/* Test Center Wishlist */}
                <div>
                    <h2 className="favorite-section-title">
                        Your Favorite Diagnostic Centers
                    </h2>

                    {smartRxInsiderData?.prescriptions?.[0]?.investigationWishList.map(
                        (item) => (
                            <div className="col-12" key={`test-${item.id}`}>
                                <div className="favorite-card">
                                    <h6 className="ms-4">
                                        <span className="favorite-title">
                                            {item.testName}
                                        </span>
                                        <span className="favorite-subtitle ms-2">
                                            {" "}
                                            Your Favorite Alternatives are:
                                        </span>
                                    </h6>

                                    <div className="ms-4 ps-2">
                                        <div className="favorite-timeline">
                                            {Object.entries(
                                                item.wishedTestCenters,
                                            ).map(([key, name]) => (
                                                <div
                                                    className="favorite-timeline-item"
                                                    key={key}
                                                >
                                                    <div className="favorite-timeline-dot"></div>
                                                    <div className="favorite-timeline-content">
                                                        <PriceComparison
                                                            height="15"
                                                            fill="#b8aef2"
                                                            className="me-2 favorite-icon"
                                                            style={
                                                                {
                                                                    // color: "#4b3b8b",
                                                                }
                                                            }
                                                        />
                                                        <span className="favorite-alt-name">
                                                            {name}
                                                        </span>
                                                    </div>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ),
                    )}
                </div>
            </div>
        </div>
    );
};

export default Favorites;
