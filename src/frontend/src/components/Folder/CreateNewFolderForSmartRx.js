import React, { useState } from "react";
import CreateNewFolder from "../static/NewFolderCreate/CreateNewFolder";
import { handleOpenModal, handleCloseModal } from "../../utils/utils";
import ContextMenu from "../static/ContextMenu/ContextMenu";
import { FaEdit, FaTrash, FaCopy } from "react-icons/fa";
import StepIndicator from "../static/StepIndicator/StepIndicator";
import CustomAccordion from "../static/CustomAccordion/CustomAccordion";
import TableWithVitalLayout from "../static/Tables/TableWithVitalLayout";
import CustomCard from "../static/CustomCard/CustomCard";
import useCurrentUserId from "../../hooks/useCurrentUserId";

const CreateNewFolderForSmartRx = () => {
    const [modalType, setModalType] = useState(null); // 'edit', 'view', 'delete'
    const [selected, setSelected] = useState(null);
    const userId = useCurrentUserId();
    const canCreateFolder = Boolean(userId);
    const openCreateModal = () => {
        if (!canCreateFolder) {
            return;
        }
        handleOpenModal(setSelected, setModalType)(null, "add");
    };

    const menuItems = [
        {
            label: "Edit",
            icon: <FaEdit />,
            action: () => handleOpenModal(setSelected, setModalType)(null, "edit"),
        },
        { label: "Copy", icon: <FaCopy />, action: () => alert("Copy clicked") },
        {
            label: "Delete",
            icon: <FaTrash />,
            action: () => handleOpenModal(setSelected, setModalType)(null, "delete"),
        },
    ];

    const accordionData = [
        {
            title: "Accordion Item #1",
            content: "Content for item 1",
            defaultOpen: true,
        },
        {
            title: "Accordion Item #2",
            content: "Content for item 2",
            defaultOpen: false,
        },
        {
            title: "Accordion Item #3",
            content: "Content for item 3",
            defaultOpen: false,
        },
    ];

    return (
        <div style={{ marginTop: "120px" }}>
            <button
                className="btn btn-primary"
                onClick={openCreateModal}
                disabled={!canCreateFolder}
                title={!canCreateFolder ? "Sign in to create folders" : undefined}
            >
                Create New Folder
            </button>
            {/* Modal for new folder related actions */}
            <CreateNewFolder modalType={modalType} onOpen={modalType} onClose={handleCloseModal(setModalType, setSelected)} />
            <ContextMenu menuItems={menuItems} />

            <div className="mt-5">
                <StepIndicator />
            </div>
            <div className="my-5">
                <h2>Custom Accordion</h2>
                <CustomAccordion accordionHeader={"Overview"}>
                    <div className="text-start">
                        <h2>Caregiver</h2>
                        <div className="mb-2 mt-4">RX By : Dr. Sultana</div>
                        <div>At : Name of chamber</div>
                    </div>
                </CustomAccordion>
                {/* <CustomAccordion accordionHeader={"Overview"} /> */}
            </div>

            <div className="my-5">
                <h2>Card</h2>

                <CustomCard>
                    <div className="text-start">
                        <h2>Caregiver</h2>
                        <div className="mb-2 mt-4">RX By : Dr. Sultana</div>
                        <div>At : Name of chamber</div>
                    </div>
                </CustomCard>
            </div>

            <div className="my-5">
                <h2>Table With Vital Layout</h2>

                <TableWithVitalLayout />
            </div>

            <div className="my-5">
                <h2>Table Without Header</h2>

                <TableWithVitalLayout header={false} />
            </div>
        </div>
    );
};

export default CreateNewFolderForSmartRx;
