import "./SmartRxInsiderShimmer.css";

const SmartRxInsiderShimmer = () => {
    return (
        <div className="col-12 col-md-9 col-lg-7 col-xl-5 mx-auto px-4 py-5 my-5" style={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0
        }}>
            <div className="d-flex justify-content-between align-items-center mb-4 gap-3">
                <div className="smart-rx-shimmer-title" />
                <div className="smart-rx-shimmer-title" />
            </div>
            <div className="d-flex justify-content-between smart-rx-shimmer-patient-info mb-4">
                <div className="smart-rx-shimmer-line smart-rx-short" />
                <div className="smart-rx-shimmer-line smart-rx-short" />
            </div>
            <div className="d-flex smart-rx-shimmer-tabs overflow-hidden mb-4">
                {Array(5)
                    .fill(0)
                    .map((_, idx) => (
                        <div key={idx} className="smart-rx-shimmer-tab" />
                    ))}
            </div>

            <div className="d-flex justify-content-between smart-rx-shimmer-patient-info mb-4">
                <div>
                    <div className="smart-rx-shimmer-line smart-rx-short mb-3" />
                    <div className="smart-rx-shimmer-line smart-rx-short" />
                </div>
                <div>
                    <div className="smart-rx-shimmer-line smart-rx-short mb-3" />
                    <div className="smart-rx-shimmer-line smart-rx-short" />
                </div>
            </div>

            <div className="smart-rx-shimmer-card smart-rx-large mb-4" />

            <div className="smart-rx-shimmer-card-big smart-rx-medium mb-4">
                <div className="smart-rx-shimmer-title" />
                <div className="smart-rx-shimmer-line smart-rx-long" />
                <div className="smart-rx-shimmer-line smart-rx-long" />
                <div className="smart-rx-shimmer-progress-line">
                    {Array(5)
                        .fill(0)
                        .map((_, i) => (
                            <div key={i} className="smart-rx-shimmer-dot" />
                        ))}
                </div>
                <div className="smart-rx-shimmer-add-button" />
            </div>

            <div className="smart-rx-shimmer-card-big smart-rx-medium">
                <div className="smart-rx-shimmer-title" />
                <div className="smart-rx-shimmer-line smart-rx-long" />
                <div className="smart-rx-shimmer-line smart-rx-long" />
                <div className="smart-rx-shimmer-progress-line">
                    {Array(5)
                        .fill(0)
                        .map((_, i) => (
                            <div key={i} className="smart-rx-shimmer-dot" />
                        ))}
                </div>
                <div className="smart-rx-shimmer-add-button" />
            </div>
        </div>
    );
};

export default SmartRxInsiderShimmer;
