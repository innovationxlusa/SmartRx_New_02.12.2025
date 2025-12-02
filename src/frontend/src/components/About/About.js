import React from "react";
import "./About.css";
import Header from "../static/Header/Header";
import Footer from "../static/Footer/Footer";
import PageTitle from "../static/PageTitle/PageTitle";

const About = ({ onClose }) => {
    return (
        <>
            <Header />
            <div className="mt-5 pt-5" style={{ padding: "20px" }}>
                <PageTitle switchButton={false} pageName={"About Us"} />
                <div className="modal-backdrop-1">
                    <div className="modal-content">
                        {/* <h2 className="about-header">About Us</h2> */}
                        <div
                            className="scrollbar-hidden"
                            style={{ height: "80vh", overflowY: "auto" }}
                        >
                            <h3 className="about-sub-header">
                                Empowering Health Through Clarity
                            </h3>
                            <div className="about-content ">
                                <p>
                                    At SmartRx, we believe that understanding
                                    your prescriptions shouldn’t require a
                                    medical degree. Our mission is simple: to
                                    make healthcare information accessible,
                                    understandable, and actionable for
                                    everyone—especially patients and caregivers.
                                    SmartRx was created to solve a common but
                                    critical problem: the confusion and
                                    frustration that often comes with managing
                                    medical prescriptions. From hard-to-read
                                    handwriting to complicated drug names and
                                    unclear dosage instructions, the traditional
                                    prescription process can leave patients and
                                    families feeling lost. We’re here to change
                                    that.
                                </p>
                            </div>
                            <h3 className="about-sub-header">What We Do</h3>
                            <div className="about-content">
                                <p>
                                    SmartRx is a digital health platform that
                                    transforms your physical prescriptions into
                                    smart, interactive tools that help you take
                                    control of your health. Our technology makes
                                    it easy to:
                                </p>
                                <p>
                                    - Digitize and store prescriptions in a
                                    secure, cloud-based environment
                                </p>
                                <p>
                                    - Decode complex medical terms into clear,
                                    everyday language
                                </p>
                                <p>
                                    - Receive medication reminders and alerts
                                    for refills, potential conflicts, or missed
                                    doses
                                </p>
                                <p>
                                    - Compare treatments over time and track
                                    your medical journey
                                </p>
                                <p>
                                    - Access personalized health education
                                    tailored to your prescriptions and
                                    conditions
                                </p>
                            </div>

                            <h3 className="about-sub-header">Who We Serve</h3>
                            <div className="about-content">
                                <p>SmartRx is built for:</p>
                                <p>
                                    - Patients managing their own treatments and
                                    seeking clarity
                                </p>
                                <p>
                                    - Caregivers and family members supporting
                                    loved ones through complex medication
                                    routines
                                </p>
                                <p>
                                    - Anyone who wants to be more informed and
                                    empowered in their healthcare decisions
                                </p>
                                <p>
                                    Whether you're managing a chronic illness,
                                    caring for aging parents, or just trying to
                                    stay organized, SmartRx gives you the tools
                                    and confidence to do it well.
                                </p>
                            </div>

                            <h3 className="about-sub-header">Why It Matters</h3>
                            <div className="about-content">
                                <p>
                                    Better understanding leads to better
                                    outcomes. When patients and caregivers have
                                    the information they need, they’re more
                                    likely to follow prescriptions correctly,
                                    catch potential issues early, and
                                    communicate more effectively with healthcare
                                    providers.
                                </p>
                                <p>
                                    SmartRx isn’t just about managing
                                    prescriptions — it’s about making health
                                    more human.
                                </p>
                            </div>
                        </div>
                        {/* <button className="btn btn-secondary w-100" style={{ backgroundColor: "#4b3b8b", borderColor: "#4b3b8b", color: "white", fontFamily: "Georama"}} onClick={onClose}>
          Close
        </button> */}
                    </div>
                </div>
            </div>
            <Footer />;
        </>
    );
};
export default About;
