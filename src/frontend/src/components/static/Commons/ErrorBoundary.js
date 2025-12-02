import React from "react";

class ErrorBoundary extends React.Component {
    constructor(props) {
        super(props);
        this.state = { hasError: false, error: null };
    }

    static getDerivedStateFromError(error) {
        return { hasError: true, error };
    }

    componentDidCatch(error, errorInfo) {
        if (this.props.onError) {
            try { this.props.onError(error, errorInfo); } catch (_) {}
        }
        if (process.env.NODE_ENV !== "production") {
            // eslint-disable-next-line no-console
            console.error("ErrorBoundary caught: ", error, errorInfo);
        }
    }

    handleReset = () => {
        this.setState({ hasError: false, error: null });
        if (this.props.onReset) {
            try { this.props.onReset(); } catch (_) {}
        }
    };

    render() {
        if (this.state.hasError) {
            return (
                <div className="container py-5">
                    <div className="alert alert-danger" role="alert">
                        <div className="d-flex justify-content-between align-items-start">
                            <div>
                                <h5 className="alert-heading mb-2">Something went wrong.</h5>
                                <div>Please try again or go back.</div>
                                {this.props.showDetails && this.state.error && (
                                    <pre className="mt-3" style={{ whiteSpace: "pre-wrap" }}>
                                        {String(this.state.error?.message || this.state.error)}
                                    </pre>
                                )}
                            </div>
                            <button className="btn btn-sm btn-outline-secondary" onClick={this.handleReset}>
                                Reload
                            </button>
                        </div>
                    </div>
                    {this.props.fallback}
                </div>
            );
        }
        return this.props.children;
    }
}

export default ErrorBoundary;

