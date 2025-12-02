import React from "react";
import HeroSection from "../HeroSection/HeroSection";

const PrivacyPolicy = () => {
    return (
        <div className="container px-0">
            <HeroSection bottomSection={false} backIcon={true} landingPage={false} backgroundHeight="140px">
                <section className="col-md-12 d-flex justify-content-center align-items-center" style={{ minHeight: "350px" }}>
                    <h1 className=" text-white">Under Development</h1>
                </section>
                {/* Middle Section */}
                <section className="mt-5 pb-4 pt-0 pt-md-3">
                    <div className="row">
                        <div className="col-md-12 middleTextStyle px-4">
                            <div className="content-padding">
                                <h2 className="mb-3">Privacy Policy</h2>
                                <p>
                                    At SmartRx, your privacy is a top priority. This Privacy Policy explains how we collect, use, and protect your information when you register for and use our
                                    services.
                                </p>

                                <h5 className="mt-4">1. Information We Collect</h5>
                                <ul>
                                    <li>
                                        <strong>Personal Information:</strong> Name, email, phone number, date of birth, address.
                                    </li>
                                    <li>
                                        <strong>Health Information:</strong> Medical history, prescriptions, doctor visits (only if required for services).
                                    </li>
                                    <li>
                                        <strong>Device Information:</strong> IP address, browser type, operating system.
                                    </li>
                                </ul>

                                <h5 className="mt-4">2. How We Use Your Information</h5>
                                <ul>
                                    <li>Create and manage your HealthRx account.</li>
                                    <li>Provide healthcare and wellness services.</li>
                                    <li>Send notifications and service updates.</li>
                                    <li>Improve the performance and reliability of our services.</li>
                                    <li>Ensure compliance with medical and legal requirements.</li>
                                </ul>

                                <h5 className="mt-4">3. Sharing Your Information</h5>
                                <p>
                                    We do <strong>not</strong> sell your personal data. We may share information with:
                                </p>
                                <ul>
                                    <li>Healthcare providers (with your consent)</li>
                                    <li>Authorized partners under HIPAA-compliant agreements</li>
                                    <li>Law enforcement or government agencies as required by law</li>
                                </ul>

                                <h5 className="mt-4">4. Data Security</h5>
                                <p>
                                    We use industry-standard security measures, including encryption and secure servers, to protect your data. Access to sensitive information is restricted and
                                    monitored.
                                </p>

                                <h5 className="mt-4">5. Your Rights</h5>
                                <ul>
                                    <li>Access or update your personal information</li>
                                    <li>Request deletion of your account</li>
                                    <li>Withdraw consent for certain data uses</li>
                                </ul>
                                <p>
                                    Contact us at <a href="mailto:support@healthrx.com">support@healthrx.com</a> for any requests.
                                </p>

                                <h5 className="mt-4">6. Cookies and Tracking</h5>
                                <p>Our website uses cookies for authentication and performance. You can manage cookie preferences in your browser.</p>

                                <h5 className="mt-4">7. Changes to This Policy</h5>
                                <p>We may update this policy. Any changes will be posted here, and you’ll be notified via email or app notification.</p>
                            </div>
                        </div>
                    </div>
                </section>
            </HeroSection>
        </div>
    );
};

export default PrivacyPolicy;
