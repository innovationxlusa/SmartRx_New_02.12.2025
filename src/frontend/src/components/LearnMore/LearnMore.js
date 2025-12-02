import "./LearnMore.css";
import { useParams } from "react-router-dom";
import LearnMoreTemplate from "./LearnMoreTemplate";
import HeroSection from "../HeroSection/HeroSection";
import { learnMoreConfigs } from "./learnMoreConfigs";

const LearnMore = () => {
    const { type } = useParams();
    const config = learnMoreConfigs[type];

    if (!config) return <div>Invalid LearnMore type: {type}</div>;

    return (
        <div className="container px-0">
            <div className="row">
                <div className="col-12">
                    <HeroSection landingPage={false} backIcon={true}>
                        <div className="px-4 px-md-0 mx-1 mx-md-0">
                            <LearnMoreTemplate
                                title={config.title}
                                subtitle={config.subtitle}
                                videoSrc={config.videoSrc}
                                bannerText={config.bannerText}
                                mainContent={config.mainContent}
                                backIcon={true}
                            />
                        </div>
                    </HeroSection>
                </div>
            </div>
        </div>
    );
};

export default LearnMore;
