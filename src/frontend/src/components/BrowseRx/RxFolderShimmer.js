import "./RxFolderShimmer.css";

const RxFolderShimmer = () => {
    return (
        <div className="rx-folder-shimmer col-12 col-md-9 col-lg-7 col-xl-6 mx-auto px-4 ">
            <div className="d-flex justify-content-between align-items-center">
                <div className="shimmer-icon" style={{ borderRadius: "4px", width: "20px" }} />
                <div className="shimmer-btn" style={{ borderRadius: "4px", marginLeft: "62px" }} />
                <div className="shimmer-badge" style={{ height: "30px" }} />
            </div>
            <div className="d-flex justify-content-between align-items-center mb-3 gap-2 gap-md-0">
                <div className="shimmer-btn" style={{ borderRadius: "4px", width: "100%", maxWidth: "500px" }} />
                <div className="shimmer-badge" style={{ height: "30px" }} />
            </div>
            {Array(5)
                .fill(0)
                .map((_, index) => (
                    <div className="shimmer-card" key={index}>
                        <div className="shimmer-thumbnail">
                            <div className="shimmer-preview" />
                        </div>
                        <div className="shimmer-details">
                            <div className="shimmer-btn-group">
                                <div className="shimmer-btn" />
                                <div className="shimmer-icon" />
                            </div>
                        </div>
                        <div className="shimmer-status-date">
                            <div className="shimmer-badge" />
                            <div className="shimmer-line" />
                            <div className="shimmer-line short" />
                        </div>
                    </div>
                ))}
        </div>
    );
};

export default RxFolderShimmer;
