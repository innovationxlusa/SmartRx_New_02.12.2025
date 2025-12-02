import React from "react";
import "./ProfileDetails.css";

const ProfileData = ({ profile, customStyles }) => {
    if (!profile) {
        return <div className="alert alert-danger text-center">Profile data is unavailable.</div>;
    }

    return (
        <table className="table" style={customStyles}>
            <thead></thead>
            <tbody>
                {profile.data.map((item, index) => (
                    <tr key={index}>
                        <td>
                            <label style={customStyles?.label}>{item.field}</label>
                            <input type="text" className="form-control" value={item.value} style={customStyles?.input} />
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default ProfileData;
