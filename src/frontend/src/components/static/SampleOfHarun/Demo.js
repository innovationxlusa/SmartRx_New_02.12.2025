import React from "react";
import "./Demo.css";
import MultiSelectSearch from "../Search/MultiSelectSearch";
import Autocomplete from "../Search/AutocompleteSearch";
import MultiSelectDropdown from "../Dropdown/MultiSelectDropdown";
import ShowHideSection from "../ShowHideSection/ShowHideSection";
import Img from "./../../../assets/img/IconContainer.svg";
import HorizontalScrollMenu from "../Menu/HorizontalScrollMenu";
import ProfileProgress from "../../PatientProfile/ProfileProgress";
import FloatingMenu from "../FloatingMenu/FloatingMenu";
import LandingMidSction from "../Landing/LandingMidSection";

const buttonsData = [
    { text: "View rx", onClick: () => alert("View RX Clicked!") },
    { text: "Preview", onClick: () => alert("Preview Clicked!") },
    { text: "More", onClick: () => alert("More Clicked!") },
];
function Demo(profile, customStyles) {
    return (
        <>
            <br />
            <FloatingMenu />
            <hr />
            <ProfileProgress
                customStyles={{
                    ...customStyles?.button,
                    background: "linear-gradient(90deg, #96AC57 0%, #B94CF3 46.61%, #6010C6 100%)",
                    border: "1px solid #65636E",
                    borderRadius: "39.0368px",
                    height: "5px",
                }}
                profile={profile}
            />
            <hr />
            <MultiSelectSearch />
            <hr />
            <Autocomplete />
            <hr />
            <MultiSelectDropdown />
            <hr />
            <ShowHideSection imageSrc={Img} date="20-03-2025" fileSize="34KB" buttons={buttonsData} />
            <hr />
            <HorizontalScrollMenu />
            <br />
            <br />
            <LandingMidSction />
            <br />
            <br />
        </>
    );
}
export default Demo;
