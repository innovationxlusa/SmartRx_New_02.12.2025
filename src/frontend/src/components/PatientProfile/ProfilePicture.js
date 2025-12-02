import React, { useRef, useState, useEffect, useMemo, useCallback } from "react";
import ProfileEdit from "../../assets/img/ProfileEdit.svg";
import DefaultProfilePhoto from "../../assets/img/DefaultProfilePhoto.svg";

const ProfilePicture = ({
  profile: profileProp,
  onFileSelect,
  size = "large",
  disableBackendLoading = false,
}) => {

  const fileInputRef = useRef(null);

  const patientData = useMemo(() => profileProp?.data || {}, [profileProp?.data]);

  const [imageState, setImageState] = useState({
    picture: profileProp?.profilePhotoPath || profileProp?.picture || "",
    file: null,
  });

  const [loadedFile, setLoadedFile] = useState(null);
  const [profileImageUrl, setProfileImageUrl] = useState(null);

  // Get initials
  const getInitials = useCallback(
    (fullName) =>
      fullName
        ?.replace(/\b(Dr\.?|Prof\.?)\s*/gi, "")
        ?.split(/\s+/)
        ?.map((w) => w[0])
        ?.join("")
        ?.substring(0, 2)
        ?.toUpperCase() || "",
    []
  );

  // Color for initials
  const getColorForName = useCallback((name) => {
    if (!name) return "#e6e4ef";
    const colors = ["#5A67D8", "#319795", "#ED8936", "#D53F8C", "#38A169"];
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    return colors[Math.abs(hash % colors.length)];
  }, []);

  const fullName = `${profileProp?.data?.firstName || ""} ${profileProp?.data?.lastName || ""}`.trim();
  const initials = getInitials(fullName);
  const initialsColor = useMemo(() => getColorForName(fullName), [fullName, getColorForName]);

  // Construct image URL
  const constructImageUrl = useCallback((path) => {
    if (!path) return null; // important change!
    if (path.startsWith("http") || path.startsWith("blob:")) return path;
    if (path.includes("/static/")) return path;
    const baseUrl = process.env.REACT_APP_IMAGE_URL || "";
    return baseUrl ? `${baseUrl}/${path}` : path;
  }, []);

  // Load image file
  const loadFileFromPath = useCallback(
    async (filePath) => {
      if (!filePath || disableBackendLoading) return null;
      if (filePath.includes("/static/")) return null;

      try {
        const url = constructImageUrl(filePath);
        const res = await fetch(url);

        if (!res.ok) {
          console.warn(`Image fetch failed: ${res.status} - ${filePath}`);
          return null;
        }

        const blob = await res.blob();
        const file = new File([blob], filePath.split("/").pop() || "profile.jpg", {
          type: blob.type,
        });
        const objectUrl = URL.createObjectURL(blob);

        setProfileImageUrl(objectUrl);
        return file;
      } catch (e) {
        console.error("Error loading profile image:", e);
        return null;
      }
    },
    [disableBackendLoading, constructImageUrl]
  );

  // Initial image load
  useEffect(() => {
    if (!patientData?.profilePhotoPath || disableBackendLoading) return;

    (async () => {
      const file = await loadFileFromPath(patientData.profilePhotoPath);
      if (file) setLoadedFile(file);
    })();
  }, [patientData, disableBackendLoading, loadFileFromPath]);

  // Update picture when profile changes
  useEffect(() => {
    const newUrl =
      profileImageUrl ||
      constructImageUrl(profileProp?.profilePhotoPath) ||
      constructImageUrl(profileProp?.picture) ||
      null;

    setImageState((prev) => ({ ...prev, picture: newUrl }));
  }, [profileImageUrl, profileProp?.profilePhotoPath, profileProp?.picture, constructImageUrl]);

  // Cleanup
  useEffect(
    () => () => {
      if (profileImageUrl) URL.revokeObjectURL(profileImageUrl);
    },
    [profileImageUrl]
  );

  // ✅ Detect real image vs broken URL
  const hasImage = Boolean(imageState.picture);

  // Select file
  const handleFileChange = (e) => {
    const file = e.target.files?.[0];
    if (!file) return;

    const url = URL.createObjectURL(file);
    setImageState({ picture: url, file });
    setProfileImageUrl(url);
    setLoadedFile(file);
    onFileSelect?.(file);
  };

  const handleIconClick = () => fileInputRef.current?.click();

  return (
    <div className="panel panel-default">
      <div className="panel-heading text-center">
        <p style={{ color: "#65636E", fontSize: "15px", fontWeight: "500" }}>
          Profile Picture
        </p>
      </div>

      <div className="panel-body text-center" style={{ position: "relative" }}>
        <div
          className="main-pro-pic"
          style={{
            width: size === "small" ? "50px" : "120px",
            height: size === "small" ? "50px" : "120px",
            borderRadius: "50%",
            border: "2px solid #65636e",
            backgroundSize: "cover",
            backgroundPosition: "center",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",

            backgroundImage: hasImage ? `url(${imageState.picture})` : "none",
            backgroundColor: hasImage ? "transparent" : "#65636e",
            color: hasImage ? "transparent" : initialsColor,
            fontSize: size === "small" ? "20px" : "36px",
            fontFamily: "Georama, sans-serif",
            fontWeight: "bold",
            position: "relative",
          }}
        >
          {!hasImage && initials}

          {/* ✅ Detect broken image & fallback to initials */}
          {imageState.picture && (
            <img
              src={imageState.picture}
              alt=""
              style={{ display: "none" }}
              onError={() => {
                setImageState((prev) => ({ ...prev, picture: null }));
              }}
            />
          )}
        </div>

        <input
          type="file"
          ref={fileInputRef}
          style={{ display: "none" }}
          accept="image/*"
          onChange={handleFileChange}
        />

        <img
          src={ProfileEdit}
          onClick={handleIconClick}
          style={{
            position: "absolute",
            width: size === "small" ? "20px" : "30px",
            height: size === "small" ? "20px" : "30px",
            left: size === "small" ? "32px" : "100px",
            top: size === "small" ? "30px" : "70px",
            cursor: "pointer",
            marginTop: size === "small" ? "15px" : "30px",
          }}
          alt="edit"
        />
      </div>
    </div>
  );
};

export default ProfilePicture;
