import { motion, AnimatePresence } from "framer-motion";
import { FaBolt, FaStar } from "react-icons/fa";
import "./ConsumePopup.css";

export default function ConsumePopup({
    isOpen,
    onClose,
    consumeAmount = 0,
    message
}) {
    return (
        <AnimatePresence>
            {isOpen && (
                <motion.div
                    className="consume-overlay"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    exit={{ opacity: 0 }}
                    onClick={onClose}
                >
                    <motion.div
                        className="consume-popup"
                        initial={{ scale: 0.4, opacity: 0, rotate: 8 }}
                        animate={{ scale: 1, opacity: 1, rotate: 0 }}
                        exit={{ scale: 0.4, opacity: 0 }}
                        transition={{
                            type: "spring",
                            stiffness: 150,
                            damping: 12,
                        }}
                        onClick={(e) => e.stopPropagation()}
                    >
                        {/* Floating Glow Stars */}
                        <FaStar className="consume-star cstar-1" />
                        <FaStar className="consume-star cstar-2" />

                        {/* Center Bolt Icon */}
                        <motion.div
                            initial={{ scale: 0 }}
                            animate={{ scale: 1 }}
                            transition={{ type: "spring", stiffness: 200 }}
                            className="consume-icon"
                        >
                            <FaBolt />
                        </motion.div>

                        <h3 className="consume-title">Redeem Points</h3>

                        <motion.div
                            className="consume-amount"
                            initial={{ scale: 0.7, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            transition={{ type: "spring", stiffness: 160 }}
                        >
                            {consumeAmount}
                        </motion.div>

                        <p className="consume-note">
                            You have redeemed {consumeAmount} points
                        </p>
                    </motion.div>
                </motion.div>
            )}
        </AnimatePresence>
    );
}
