import React, { useEffect, useRef } from 'react';
import './PatientProfileMenu.css';
import { getColorForName } from '../../constants/constants';

const DoctorProfileMenu = ({ isVisible, onClose }) => {
    const menuRef = useRef(null);

    // Sample doctor data - replace with props/API later
    const doctors = [
        { id: 1, name: 'Dr. Rahman', specialization: 'Cardiologist', chamber: 'City Clinic', totalPrescriptions: 120 },
        { id: 2, name: 'Dr. Ayesha', specialization: 'Dermatologist', chamber: 'Skin Care Center', totalPrescriptions: 85 },
        { id: 3, name: 'Dr. Karim', specialization: 'Neurologist', chamber: 'Neuro Hub', totalPrescriptions: 60 },
        { id: 4, name: 'Dr. Nabila', specialization: 'Pediatrician', chamber: "Children's Health", totalPrescriptions: 150 },
    ];

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (menuRef.current && !menuRef.current.contains(event.target)) {
                onClose && onClose();
            }
        };

        if (isVisible) {
            document.addEventListener('mousedown', handleClickOutside);
        }
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, [isVisible, onClose]);

    if (!isVisible) return null;

    return (
        <div
            ref={menuRef}
            className="patient-profile-menu-container"
        >
            <div className="patient-profile-menu">
                <div className="menu-header">
                    <h6>Switch Doctor</h6>
                    <button
                        className="close-menu-btn"
                        onClick={onClose}
                    >
                        ×
                    </button>
                </div>

                <div className="menu-content">
                    <div className="patients-list">
                        {doctors.map((doc) => {
                            const firstLetter = (doc.name || 'D').charAt(0).toUpperCase();
                            const backgroundColor = getColorForName(doc.name);
                            return (
                                <div key={doc.id} className="patient-menu-item">
                                    <div className="patient-avatar">
                                        <div
                                            className="patient-avatar-circle"
                                            style={{ backgroundColor: backgroundColor }}
                                        >
                                            {firstLetter}
                                        </div>
                                    </div>
                                    <div className="patient-info">
                                        <div className="patient-name">{doc.name}</div>
                                        <div className="patient-details">
                                            {doc.specialization || 'N/A'}
                                        </div>
                                        <div className="patient-prescriptions">
                                            {doc.chamber || 'N/A'}
                                        </div>
                                        <div className="patient-prescriptions">
                                            {doc.totalPrescriptions || 0} Total Prescriptions
                                        </div>
                                    </div>
                                </div>
                            );
                        })}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default DoctorProfileMenu;


