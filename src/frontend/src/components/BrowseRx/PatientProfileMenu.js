import React, { useState, useRef, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './PatientProfileMenu.css';
import { getColorForName } from '../../constants/constants';

const PatientProfileMenu = ({ isVisible, onClose }) => {
    const menuRef = useRef(null);

    // Sample patient data - this would come from props or API
    const patients = [
        { id: 1, name: "Harun", age: 32, gender: "Male", totalPrescriptions: 4 },
        { id: 2, name: "Sarah", age: 28, gender: "Female", totalPrescriptions: 12 },
        { id: 3, name: "Ahmed", age: 45, gender: "Male", totalPrescriptions: 8 },
        { id: 4, name: "Fatima", age: 35, gender: "Female", totalPrescriptions: 6 },
    ];

    // Close menu when clicking outside
    useEffect(() => {
        const handleClickOutside = (event) => {
            if (menuRef.current && !menuRef.current.contains(event.target)) {
                onClose();
            }
        };

        if (isVisible) {
            document.addEventListener('mousedown', handleClickOutside);
        }

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [isVisible, onClose]);

    if (!isVisible) return null;

    return (
        <div 
            ref={menuRef}
            className="patient-profile-menu-container"
        >
            <div className="patient-profile-menu">
                <div className="menu-header">
                    <h6>Switch Patient</h6>
                    <button 
                        className="close-menu-btn"
                        onClick={onClose}
                    >
                        ×
                    </button>
                </div>
                
                <div className="menu-content">
                    <div className="patients-list">
                        {patients.map((patient) => {
                            const firstLetter = patient.name.charAt(0).toUpperCase();
                            const backgroundColor = getColorForName(patient.name);
                            return (
                                <div key={patient.id} className="patient-menu-item">
                                    <div className="patient-avatar">
                                        <div
                                            className="patient-avatar-circle"
                                            style={{
                                                backgroundColor: backgroundColor,
                                            }}
                                        >
                                            {firstLetter}
                                        </div>
                                    </div>
                                    <div className="patient-info">
                                        <div className="patient-name">{patient.name}</div>
                                        <div className="patient-details">
                                            Age: {patient.age} | Gender: {patient.gender}
                                        </div>
                                        <div className="patient-prescriptions">
                                            {patient.totalPrescriptions} Total Prescriptions
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

export default PatientProfileMenu;
