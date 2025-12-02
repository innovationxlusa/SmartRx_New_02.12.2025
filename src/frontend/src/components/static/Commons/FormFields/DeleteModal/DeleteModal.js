import "./DeleteModal.css";
import { FaTrashAlt } from "react-icons/fa";
// import CustomInputField from "../CustomInputField";
// import { useCustomCSS } from "../../../../hooks/useCustomCSS";

const DeleteModal = ({ text, value, onChange, error, helperText }) => {
    // const CSS_CONSTANTS = useCustomCSS();

    return (
        <div className="text-center content-txt">
            <div className="icon-box pb-4">
                <FaTrashAlt className="red-circle" />
            </div>

            {text !== null ? (
                <div>
                    <h5>
                        <b>Are you sure you want to delete this {text}?</b>
                    </h5>
                    <p> This action cannot be undone.</p>
                </div>
            ) : (
                <div>
                    <h6>Are you sure you want to delete this item?</h6>
                    <br />
                    <p> This action cannot be undone.</p>
                </div>
            )}
            {/* <h2 className={CSS_CONSTANTS.TEXT_COLOR}>Are you sure?</h2>
            <p className={CSS_CONSTANTS.TEXT_COLOR}>Do you really want to delete these records? This process cannot be undone.</p> */}
            {/* <CustomInputField
                id="deleteRemarks"
                label="Remarks"
                name="deleteRemarks"
                type="text"
                value={value}
                onChange={onChange}
                error={error} // Set error prop based on field error
                helperText={helperText} // Provide the error message
                variant="outlined"
                fullWidth
                size="small"
                multiline
                rows={3}
            /> */}
        </div>
    );
};

export default DeleteModal;
