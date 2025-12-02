import { useState, useEffect } from "react";
// import { motion } from "framer-motion";
import Card from "react-bootstrap/Card";
import "./FloatingMenu.css";

const FloatingMenu = () => {
    const [showMenu, setShowMenu] = useState(true);
    const [lastScrollY, setLastScrollY] = useState(0);
    const handleScroll = () => {
        const currentScrollY = window.scrollY;
        if (currentScrollY < lastScrollY) {
            setShowMenu(true);
        } else {
            setShowMenu(false);
        }
        setLastScrollY(currentScrollY);
    };

    useEffect(() => {
        window.addEventListener("scroll", handleScroll);
        return () => {
            window.removeEventListener("scroll", handleScroll);
        };
    }, [lastScrollY]);

    const menuVariants = {
        visible: { y: 0, opacity: 1 },
        hidden: { y: -100, opacity: 0 },
    };

    return (
        <div className="relative w-full min-h-screen p-5">
            <div
                className="fixed top-0 left-0 w-full bg-gray-800 text-black p-4 flex justify-center space-x-6 shadow-md"
                variants={menuVariants}
                animate={showMenu ? "visible" : "hidden"}
                transition={{ duration: 0.5 }}
            >
                <span>About</span>
                <span>Sale</span>
                <span>Purchase</span>
                <span>Paint</span>
                <span>Reward</span>
                <span>Upload</span>
            </div>
            <div className="mt-16">
                <div className="row">
                    <div className="col-md-4">
                        <Card className="rx-success">
                            <Card.Body>
                                <Card.Title>About</Card.Title>
                            </Card.Body>
                        </Card>
                    </div>
                    <div className="col-md-4">
                        <Card className="rx-smart">
                            <Card.Body>
                                <Card.Title>Sale</Card.Title>
                            </Card.Body>
                        </Card>
                    </div>
                    <div className="col-md-4">
                        <Card className="rx-info">
                            <Card.Body>
                                <Card.Title>Purchase</Card.Title>
                            </Card.Body>
                        </Card>
                    </div>
                </div>
                <br />
                <div className="row">
                    <div className="col-md-4">
                        <Card className="rx-info">
                            <Card.Body>
                                <Card.Title>Paint</Card.Title>
                            </Card.Body>
                        </Card>
                    </div>
                    <div className="col-md-4">
                        <Card className="rx-success">
                            <Card.Body>
                                <Card.Title>Reward</Card.Title>
                            </Card.Body>
                        </Card>
                    </div>
                    <div className="col-md-4">
                        <Card className="rx-smart">
                            <Card.Body>
                                <Card.Title>Upload</Card.Title>
                            </Card.Body>
                        </Card>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default FloatingMenu;
