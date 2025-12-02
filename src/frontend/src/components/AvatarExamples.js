import React from "react";
import CustomImage from "./static/Commons/CustomImage";

const AvatarExamples = () => {
    return (
        <div style={{ height: "120vh", overflow: "auto" }}>
            <h3>Avatar Examples</h3>

            <CustomImage src="" name="Jane Smith" initials="Jane Smith" size="large" shape="squareRounded" />

            <h5>Default Avatar</h5>
            <CustomImage name="John Doe" size="tiny" />

            <h5>Avatar with Image</h5>
            <CustomImage name="Jane Doe" initials="J D" size="big" />

            <h5>Square Avatar</h5>
            <CustomImage shape="square" name="Alex Smith" initials="Alex Smith" size="small" />

            <h5>Custom Size Avatar (100x100)</h5>
            <CustomImage className={"mt-5"} name="Chris Evans" initials="Chris Evans" size="medium" />

            <h5>Avatar with Border</h5>
            <CustomImage border borderColor="blue" name="Emma Watson" initials="Emma Watson" />

            <h5>Avatar with Custom Background</h5>
            <CustomImage bgColor="red" textColor="white" name="Bruce Wayne" initials="Bruce Wayne" />
        </div>
    );
};

export default AvatarExamples;
