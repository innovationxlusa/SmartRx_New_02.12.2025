import React, { useEffect, useState } from "react";
import "./CreateNewFolder.css";
import CustomInput from "../Commons/CustomInput";
import CustomModal from "../CustomModal/CustomModal";
import useFormHandler from "../../../hooks/useFormHandler";
import DeleteModal from "../Commons/FormFields/DeleteModal/DeleteModal";
import { useUserContext } from "../../../contexts/UserContext";
import useApiClients from "../../../services/useApiClients";
import { validateField } from "../../../utils/validators";

const CreateNewFolder = ({ modalType, folder, data, isOpen, onClose }) => {
    const { dynamicModalName, buttonIcons, dynamicButtonLabel, serializeFormData, handleInputChange, resetForm, dynamicActions, topicMessage } = useFormHandler();
    const { user } = useUserContext();
    const [folderInfo, setfolderInfo] = useState(data);
    // Destructuring API service methods
    const { api } = useApiClients();

    const initialData = {
        FolderName: "",
        Description: "",
        ParentFolderId: data?.id,
        PreviousFolderHierarchy: data?.FolderHierarchy ?? 0,
        FolderHierarchy: (data?.FolderHierarchy ?? 0) + 1,
        PatientProfileId: null,
        LoginUserId: Number(user?.UserId ?? 0),
    };

    // State to manage form data
    const [formData, setFormData] = useState(initialData);

    // State to manage individual field errors
    const [fieldErrors, setFieldErrors] = useState(initialData);

    // State to manage loading status
    const [isLoading, setIsLoading] = useState(false);

    // Get modal names and button labels dynamically based on modal type
    const modalNames = dynamicModalName("New folder");
    const buttonLabels = dynamicButtonLabel("New folder");

    // Function to handle reset fields value

    // Effect to initialize form data based on modal type and topic(passed props)
    useEffect(() => {
        if (modalType === "edit" && folder) {
            const { FolderName, Description, componentId, uniqueCode, status } = folder;

            setFormData({
                ...formData,
                FolderName: FolderName || "",
                Description: Description || "",
                componentId: componentId || "",
                uniqueCode: uniqueCode || "",
                status: status || "",
            });
        } else {
            // Clear form data if modalType is not 'edit' and Clear empty field error on close modal
            resetForm(initialData, setFormData, setFieldErrors);
        }
    }, [modalType, folder, folderInfo]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const fieldsToValidate = {
            MobileNo: validateField("FolderName", formData.FolderName, "Folder name"),
        };

        if (Object.values(fieldsToValidate).some((error) => error !== "")) {
            setFieldErrors(fieldsToValidate);
            return;
        }

        try {
            setIsLoading(true);

            // API call to assign roles to the selected user
            const response = await api.createNewFolder(formData, "");

            // If API call is successful or returns an object, reset the form
            if (response.message === "Successful" || typeof response === "object") {
                setFormData(initialData);
                setFieldErrors({});
                onClose();
            }
        } catch (e) {
            console.error(e);
        } finally {
            setIsLoading(false); // Reset loading state
        }
    };

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={modalNames[modalType]}
            close={onClose}
            animationDirection="top"
            position="top"
            form={true}
            onSubmit={handleSubmit}
            isButtonLoading={isLoading}
            buttonType={"submit"}
            buttonIcon={buttonIcons[modalType]}
            buttonLabel={buttonLabels[modalType]}
            isButtonDisabled={isLoading}
            buttonWidth={"100%"}
            buttonBackgroundColor={""}
            buttonTextColor={"black"}
            buttonShape={"pill"}
            buttonBorderStyle={""}
            buttonBorderColor={"2px solid var(--theme-font-color)"}
            buttonIconStyle={{ color: "var(--theme-font-color)" }}
            buttonLabelStyle={{ fontWeight: "500" }}
        >
            {/* <form className="w-100" onSubmit={}> */}
            {modalType === "delete" ? (
                <div>
                    <DeleteModal
                        // value={formData.deleteRemarks}
                        // onChange={(e) => handleInputChange(e, setFormData, setFieldErrors, "input", "deleteRemarks")}
                        // error={!!fieldErrors.deleteRemarks}
                        // helperText={fieldErrors.deleteRemarks}
                        value={""}
                        onChange={""}
                        error={""}
                        helperText={""}
                    />
                </div>
            ) : (
                <>
                    <CustomInput
                        className="mb-3 input-style"
                        label="Folder Name"
                        labelPosition="top-left"
                        name="FolderName"
                        type="text"
                        placeholder="Folder Name"
                        value={formData.FolderName}
                        onChange={handleChange}
                        error={fieldErrors.FolderName}
                        disabled={isLoading}
                        focus={true}
                    />
                    <CustomInput
                        className="input-style"
                        label="Description"
                        labelPosition="top-left"
                        name="Description"
                        type="text"
                        placeholder="Description"
                        value={formData.Description}
                        onChange={handleChange}
                        disabled={isLoading}
                    />
                </>
            )}
            {/* </form> */}
        </CustomModal>
    );
};

export default CreateNewFolder;
