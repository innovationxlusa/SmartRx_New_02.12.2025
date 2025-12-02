import "./VitalFaq.css";

const VitalFaq = ({ isLoading, error, data }) => {
    return (
        <section className="faq-container p-2">
            <div className="text-center mb-5 justify-content-left">
                {/* <h2 className="faq-title">Vital FAQs for {data.name}</h2> */}
                <span className="faq-subtitle">
                   People are often curious about the significance of their vital signs. 
                   This section provides answers to frequently asked questions(FAQ) about vitals, helping you understand their importance and how they relate to your health. 
                   <br></br><br></br>
                   <span style={{ fontSize: "12px", color: "#6c757d" }}>
                        If you have specific questions, feel free to reach out to us for more information or personalized advice.
                        Explore the FAQs below to learn more about your vital signs and their implications for your health.
                   </span>
                </span>
            </div>

            <div
                className="row justify-content-center"
                style={{
                    fontFamily:
                        "Bangla, Noto Sans Bengali, SolaimanLipi, sans-serif",
                }}
            >
                {isLoading ? (
                    Array(4)
                        .fill(0)
                        .map((_, idx) => (
                            <div key={idx} className="col-md-10 col-lg-8 mb-4">
                                <div className="faq-shimmer-card p-4 rounded-4 position-relative">
                                    <div>
                                        <div className="faq-shimmer faq-shimmer-title mb-3" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line" />
                                    </div>
                                    <div>
                                        <div className="faq-shimmer faq-shimmer-title mb-3" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line" />
                                    </div>
                                    <div>
                                        <div className="faq-shimmer faq-shimmer-title mb-3" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line" />
                                    </div>
                                    <div>
                                        <div className="faq-shimmer faq-shimmer-title mb-3" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line mb-2" />
                                        <div className="faq-shimmer faq-shimmer-line" />
                                    </div>
                                </div>
                            </div>
                        ))
                ) : Array.isArray(data?.vitalFAQDTOs) &&
                  data.vitalFAQDTOs.length > 0 ? (
                    data.vitalFAQDTOs.map(({ id, question, answer }, index) => (
                        <div className="accordion-item" key={id}>
                            <h2
                                className="accordion-header"
                                id={`heading-${id}`}
                            >
                                <button
                                    className={`accordion-button ${
                                        index !== 0 ? "collapsed" : ""
                                    }`}
                                    type="button"
                                    data-bs-toggle="collapse"
                                    data-bs-target={`#collapse-${id}`}
                                    aria-expanded={
                                        index === 0 ? "true" : "false"
                                    }
                                    aria-controls={`collapse-${id}`}
                                    style={{ fontSize: "15px" }}
                                >
                                    {question}
                                </button>
                            </h2>
                            <div
                                id={`collapse-${id}`}
                                className={`accordion-collapse collapse ${
                                    index === 0 ? "show" : ""
                                }`}
                                aria-labelledby={`heading-${id}`}
                                data-bs-parent="#testFAQAccordion"
                            >
                                <div
                                    className="accordion-body text-start"
                                    style={{
                                        fontSize: "13px",
                                        textAlign: "justify",
                                    }}
                                >
                                    {answer}
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <p
                        className="text-center text-muted"
                        style={{ fontFamily: "Georama" }}
                    >
                        No FAQs available for this vital.
                    </p>
                )}
            </div>
        </section>
    );
};

export default VitalFaq;
