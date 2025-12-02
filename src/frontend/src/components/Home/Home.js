import React from "react";
import "./Home.css";
import HeroSection from "../HeroSection/HeroSection";
import LandingMidSection from "../static/Landing/LandingMidSection";
import useToastMessage from "../../hooks/useToastMessage";

const Home = () => {
    const showToast = useToastMessage();
    const fileRxTitle = "How to use File Rx?";
    const fileRxSubTitle = "Your prescription file. Securely storing files for future.";
    const fileRxDescription =
        "A secure and efficient system for long-term digital file storage and retrieval. Ensures compliance, quick access, and protection of critical documents through smart categorization, indexing, and backup capabilities.";
    const fileRxUrl = "/learnmore/fileRx";

    const smartRxTitle = "How to discover and benefited by SmartRx?";
    const smartRxSubTitle = "Smarter prescriptions, safer patient outcomes.";
    const smartRxDescription =
        "A digital prescription management solution that streamlines doctor-patient interactions, reduces medication errors, and improves healthcare efficiency with secure e-prescriptions, automated drug checks, and real-time pharmacy integration.";
    const smartRxUrl = "/learnmore/smartRx";

    const healthEdxTitle = "How to explore HealthEdx?";
    const healthEdxSubTitle = "Efficient digital exchange for education.";
    const healthEdxDescription =
        "A modern educational data exchange platform that facilitates seamless, secure sharing of academic records, credentials, and learning analytics between institutions, educators, and students—enhancing access, verification, and decision-making.";
    const healthEdxUrl = "/learnmore/healthEdx";
       

    // useEffect(() => {
    //         fetch('https://192.168.40.43:10443/api/user/getall')
    //         .then(res => 
        
    //             showToast("success", "Test successfull", "👋")
    //         )
    // .then(console.log)
    // .catch(console.error);
    //     }, []);
    return (
        <main>
            <div className="container px-0">
                <div className="row m-0">
                    <div className="col-12 px-0">
                        <HeroSection>
                            {/* Middle Section */}
                            <section>
                                <div className="row px-3">
                                    <div className="col-12 p-3">
                                        <LandingMidSection backgroundColor="#ECFDEB" title={fileRxTitle} subTitle={fileRxSubTitle} description={fileRxDescription} url={fileRxUrl} />
                                    </div>
                                    <div className="col-12 p-3 landingMidStyle">
                                        <LandingMidSection backgroundColor="#FDF5F7" title={smartRxTitle} subTitle={smartRxSubTitle} description={smartRxDescription} url={smartRxUrl} />
                                    </div>
                                    <div className="col-12 p-3">
                                        <LandingMidSection backgroundColor="#E3F7F2" title={healthEdxTitle} subTitle={healthEdxSubTitle} description={healthEdxDescription} url={healthEdxUrl} />
                                    </div>
                                </div>
                            </section>
                        </HeroSection>
                    </div>
                </div>
            </div>
        </main>
    );
};

export default Home;
