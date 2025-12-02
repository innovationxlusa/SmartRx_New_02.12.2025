import { useState } from "react";
import StarSolid from "../../../assets/img/StarSolid.svg";
import StarThin from "../../../assets/img/StarThin.svg";

const StarRating = ({ totalStars = 5 }) => {
    const [rating, setRating] = useState(0);
    const [hover, setHover] = useState(0);

    return (
        <div className="d-flex my-5 justify-content-between">
            {[...Array(totalStars)].map((_, index) => {
                const starValue = index + 1;
                return (
                    <img
                        src={starValue <= (hover || rating) ? StarSolid : StarThin}
                        key={index}
                        className="me-1 justify-content-between"
                        onMouseEnter={() => setHover(starValue)}
                        onMouseLeave={() => setHover(0)}
                        onClick={() => setRating(starValue)}
                        style={{ cursor: "pointer", transition: "color 0.2s", height: "50px", width: "50px" }}
                    />
                );
            })}
        </div>
    );
};

export default StarRating;
