import { motion, AnimatePresence } from "framer-motion";
import { FaGift, FaStar } from "react-icons/fa";
import "./RewardPopup.css";

export default function RewardPopup({ isOpen, onClose, points = 50, message }) {
  return (
      <AnimatePresence>
          {isOpen && (
              <motion.div
                  className="reward-overlay"
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  exit={{ opacity: 0 }}
                  onClick={onClose}
              >
                  <motion.div
                      className="reward-popup"
                      initial={{ scale: 0.4, opacity: 0, rotate: -8 }}
                      animate={{ scale: 1, opacity: 1, rotate: 0 }}
                      exit={{ scale: 0.4, opacity: 0 }}
                      transition={{
                          type: "spring",
                          stiffness: 150,
                          damping: 12,
                      }}
                      onClick={(e) => e.stopPropagation()}
                  >
                      {/* Floating Stars */}
                      <FaStar className="star star-1" />
                      <FaStar className="star star-2" />

                      {/* Icon */}
                      <motion.div
                          initial={{ scale: 0 }}
                          animate={{ scale: 1 }}
                          transition={{
                              delay: 0.2,
                              type: "spring",
                              stiffness: 200,
                          }}
                          className="reward-icon"
                      >
                          <FaGift />
                      </motion.div>

                      <motion.h2
                          className="reward-points"
                          initial={{ opacity: 0, y: 10 }}
                          animate={{ opacity: 1, y: 0 }}
                          transition={{ delay: 0.35 }}
                      >
                          {points} Points!
                      </motion.h2>

                      <motion.p
                          className="reward-text"
                          initial={{ opacity: 0, y: 10 }}
                          animate={{ opacity: 1, y: 0 }}
                          transition={{ delay: 0.45 }}
                      >
                      {message + "🎉"|| "Congratulations! You earned a reward 🎉"}
                      </motion.p>

                      <motion.button
                          type="button"
                          whileTap={{ scale: 0.9 }}
                          className="btn btn-light mt-3 px-4"
                          onClick={(e) => {
                              e.preventDefault();
                              e.stopPropagation();
                              onClose();
                          }}
                          style={{ fontFamily: "Georama" }}
                      >
                          Close
                      </motion.button>
                  </motion.div>
              </motion.div>
          )}
      </AnimatePresence>
  );
}
