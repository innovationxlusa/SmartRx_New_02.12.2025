import "./DeleteModal.css";
import { FaTrashAlt } from "react-icons/fa";
// import CustomInputField from "../CustomInputField";
// import { useCustomCSS } from "../../../../hooks/useCustomCSS";

const CheckDeleteModal = ({ value, onChange, error, helperText }) => {
    // const CSS_CONSTANTS = useCustomCSS();

    return (
        <div className="text-center">
            <div className="icon-box">
                <FaTrashAlt className="red-circle" />
            </div>
            <h2>Are you sure?</h2>
            <p>Do you really want to delete these records? This process cannot be undone.</p>
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

export default CheckDeleteModal;
