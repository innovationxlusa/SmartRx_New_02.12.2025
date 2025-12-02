import "./DiagnosticShimmer.css";

const DiagnosticShimmer = ({ count = 5 }) => {
    return (
        <div className="diagnostic-list">
            {Array.from({ length: count }).map((_, index) => (
                <div
                    key={index}
                    className="d-flex align-items-center justify-content-between py-2 border-bottom gap-4 shimmer-row"
                >
                    <div
                        className="shimmer-box"
                        style={{ width: "10%", height: 20 }}
                    />
                    <div
                        className="d-flex align-items-center gap-2 flex-grow-1"
                        style={{ width: "90%" }}
                    >
                        <div
                            className="shimmer-box me-2"
                            style={{ width: 30, height: 30 }}
                        />
                        <div
                            className="shimmer-box"
                            style={{ width: "40%", height: 16 }}
                        />
                        <div
                            className="shimmer-box"
                            style={{ width: "40%", height: 16 }}
                        />
                    </div>
                </div>
            ))}
        </div>
    );
};

export default DiagnosticShimmer;
