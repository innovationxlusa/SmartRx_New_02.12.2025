import { useState, useEffect } from "react";
import "./CustomCarousel.css";
import SliderImage1 from "../../../assets/img/SliderImage1.svg";
import SliderImage2 from "../../../assets/img/SliderImage2.svg";
import SliderImage3 from "../../../assets/img/SliderImage3.svg";
import { SlArrowLeft, SlArrowRight } from "react-icons/sl";


const slides = [
    {
        title: "Your Prescription Archive –",
        subtitle: "A Smarter Way to Store and Access Your Health Records.",
        image: SliderImage1,
    },
    {
        title: "Digital Health Vault –",
        subtitle: "Securely manage all your medical history in one place.",
        image: SliderImage2,
    },
    {
        title: "Instant Access –",
        subtitle: "Retrieve your prescriptions anytime, anywhere.",
        image: SliderImage3,
    },
];

const CustomCarousel = () => {
    const [current, setCurrent] = useState(0);
    const [transitioning, setTransitioning] = useState(false);

    const goToSlide = (index) => {
        if (index === current || transitioning) return;
        setTransitioning(true);
        setTimeout(() => {
            setCurrent(index);
            setTransitioning(false);
        }, 300);
    };

    const goToPrevious = () => {
        const prevIndex = current === 0 ? slides.length - 1 : current - 1;
        goToSlide(prevIndex);
    };

    const goToNext = () => {
        const nextIndex = current === slides.length - 1 ? 0 : current + 1;
        goToSlide(nextIndex);
    };

    useEffect(() => {
        const interval = setInterval(() => {
            goToNext();
        }, 3000);

        return () => clearInterval(interval);
    }, [current]);

    return (
        <div className="smart-slider">
            <SlArrowLeft className="carousel-arrow carousel-arrow-left" onClick={goToPrevious} />
            <SlArrowRight className="carousel-arrow carousel-arrow-right" onClick={goToNext} />
            <div
                className={`slide-content ${transitioning ? "fade-out" : "fade-in"}`}
            >
                <div className="image-wrapper" style={{ height: "100px" }}>
                    <img
                        src={slides[current].image}
                        alt="slide"
                        className="slide-image"
                    />
                </div>
                <h3 className="slide-title" style={{ height: "20px" }}>
                    {slides[current].title}
                </h3>
                <p className="slide-subtitle" style={{ height: "40px" }}>
                    {slides[current].subtitle}
                </p>
            </div>
            <div className="slide-dots">
                {slides.map((_, i) => (
                    <div
                        key={i}
                        className={`dot ${i === current ? "active" : ""}`}
                        onClick={() => goToSlide(i)}
                    />
                ))}
            </div>
        </div>
    );
};

export default CustomCarousel;
