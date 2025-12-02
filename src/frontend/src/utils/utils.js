import { useEffect } from "react";
import { useLocation } from "react-router-dom";

// Function to get the value of a CSS variable
export const getCSSVariableValue = (variable) => {
    return getComputedStyle(document.documentElement).getPropertyValue(variable).trim();
};

// Handle page change for pagination
export const handlePageChange = (setPage) => (newPage) => {
    setPage(newPage);
};

// Handle rows per page change for pagination
export const handleRowsPerPageChange = (setRowsPerPage, setPage) => (event) => {
    setRowsPerPage(+event.target.value);
    setPage(1);
};

// Handle modal opening
export const handleOpenModal = (setSelected, setModalType) => (selected, modalType) => {
    // setSelected(selected);
    setModalType(modalType);
};

// Handle modal closing
export const handleCloseModal = (setModalType, setSelectedRole) => () => {
    setModalType(null);
    // setSelectedRole(null);
};

// Handle search query change
export const handleSearchChange = (setSearchQuery, setPage) => (event) => {
    setSearchQuery(event.target.value);
    setPage(1);
};

// Handle sort field and order change
export const handleSortChange = (sortField, setSortField, sortOrder, setSortOrder) => (field) => {
    setSortField(field);
    setSortOrder(sortOrder === "asc" ? "desc" : "asc");
};

// Handle crop white spaces automatically
// Utility: Crop excess white space from canvas
export const cropWhiteSpace = (canvas, imageData) => {
    const { data, width, height } = imageData;
    let top = height,
        bottom = 0,
        left = width,
        right = 0;

    for (let y = 0; y < height; y++) {
        for (let x = 0; x < width; x++) {
            const index = (y * width + x) * 4;
            const r = data[index],
                g = data[index + 1],
                b = data[index + 2];

            if (r < 240 || g < 240 || b < 240) {
                if (x < left) left = x;
                if (x > right) right = x;
                if (y < top) top = y;
                if (y > bottom) bottom = y;
            }
        }
    }

    const cropWidth = right - left;
    const cropHeight = bottom - top;

    const croppedCanvas = document.createElement("canvas");
    croppedCanvas.width = cropWidth;
    croppedCanvas.height = cropHeight;
    const croppedCtx = croppedCanvas.getContext("2d");
    croppedCtx.drawImage(canvas, left, top, cropWidth, cropHeight, 0, 0, cropWidth, cropHeight);

    return { croppedCanvas };
};

// Handle crop image
export default function getCroppedImg(imageSrc, pixelCrop, rotation = 0) {
    const createImage = (url) =>
        new Promise((resolve, reject) => {
            const image = new Image();
            image.setAttribute("crossOrigin", "anonymous"); // to avoid CORS issues
            image.src = url;
            image.onload = () => resolve(image);
            image.onerror = (error) => reject(error);
        });

    const getRadianAngle = (degreeValue) => (degreeValue * Math.PI) / 180;

    return new Promise(async (resolve, reject) => {
        const image = await createImage(imageSrc);
        const canvas = document.createElement("canvas");
        const ctx = canvas.getContext("2d");

        const safeArea = Math.max(image.width, image.height) * 2;

        canvas.width = safeArea;
        canvas.height = safeArea;

        ctx.translate(safeArea / 2, safeArea / 2);
        ctx.rotate(getRadianAngle(rotation));
        ctx.translate(-safeArea / 2, -safeArea / 2);

        ctx.drawImage(image, (safeArea - image.width) / 2, (safeArea - image.height) / 2);

        const data = ctx.getImageData(0, 0, safeArea, safeArea);

        canvas.width = pixelCrop.width;
        canvas.height = pixelCrop.height;

        ctx.putImageData(data, Math.round(0 - pixelCrop.x), Math.round(0 - pixelCrop.y));

        canvas.toBlob((blob) => {
            if (!blob) {
                reject(new Error("Canvas is empty"));
                return;
            }
            resolve(blob);
        }, "image/jpeg");
    });
}

export const findFolderById = (data, id) => {
    if (data.id === id) return data;
    if (!data.children) return null;
    for (let child of data.children) {
        const result = findFolderById(child, id);
        if (result) return result;
    }
    return null;
};

export const renameFolderById = (data, id, newName) => {
    if (data.id === id) {
        data.folderName = newName;
        return true;
    }
    if (data.children) {
        for (let child of data.children) {
            if (renameFolderById(child, id, newName)) return true;
        }
    }
    return false;
};

export const findFolderByPath = (data, pathSegments) => {
    // Normalize pathSegments to an array of non-empty strings
    const segments = Array.isArray(pathSegments)
        ? pathSegments
        : typeof pathSegments === "string" && pathSegments.length > 0
            ? pathSegments.split("/")
            : [];

    if (segments.length === 0) return data;

    // Support both an object with children and a root array of folders
    let currentNode = Array.isArray(data) ? { children: data } : (data || {});

    for (const rawName of segments) {
        const name = decodeURIComponent(String(rawName)).trim();
        const children = currentNode?.children || [];
        currentNode = children.find((child) => child?.folderName === name);
        if (!currentNode) return null;
    }
    return currentNode;
};
export const findFilesByPath = (data, pathSegments) => {
    if (pathSegments.length === 0) return data;
    let currentFiles = data;
    for (const name of pathSegments) {
        currentFiles = currentFiles.paginatedPrescriptionList?.find((child) => child.folderName === name);
        if (!currentFiles) break;
    }
    return currentFiles;
};

export const ScrollToTop = () => {
    const { pathname } = useLocation();
    useEffect(() => {
        window.scrollTo(0, 0);
    }, [pathname]);

    return null;
};

export const ALWAYS_SCROLL_TO_TOP = () => {
    return window.scroll(0, 0);
};

export const getGraphRange = (vitalName) => {
    const name = vitalName?.toLowerCase();

    if (name.includes("blood pressure")) return { min: 0, max: 200 };
    if (name.includes("blood oxygen")) return { min: 50, max: 100 };
    if (name.includes("body temperature")) return { min: 96, max: 106 };
    if (name.includes("bmi")) return { min: 15, max: 35 };
    if (name.includes("blood glucose")) return { min: 50, max: 200 };
    if (name.includes("weight")) return { min: 30, max: 150 };
    if (name.includes("pulse rate")) return { min: 10, max: 120 };
    if (name.includes("respiratory rate")) return { min: 10, max: 60 };
    // default range
    return { min: 0, max: 200 };
};

export const convertToBengaliNumber = (number) => {
    const bengaliDigits = ["০", "১", "২", "৩", "৪", "৫", "৬", "৭", "৮", "৯"];
    return number
        .toString()
        .split("")
        .map((digit) => bengaliDigits[+digit])
        .join("");
};

export const convertDecimalHeightToFeetInches = (height) => {
    const heightNumber = parseFloat(height);
    if (isNaN(heightNumber)) {
        return { feet: 0, inches: 0 };
    }
    const feet = Math.floor(heightNumber);
    const heightStr = heightNumber.toFixed(2);
    const split = heightStr.split(".");
    let inches = 0;

    if (split.length === 2) {
        const inchPart = split[1].padEnd(2, "0").substring(0, 2);
        const parsedInches = parseInt(inchPart, 10);
        if (!isNaN(parsedInches)) {
            inches = Math.max(0, Math.min(parsedInches, 11));
        }
    }

    return { feet, inches };
}

export const getRxText = (item) => {
    if (item?.isSmarted || item?.isSmartRxBlank) return "Smart RX";
    else if (item?.isWaiting || item?.isWaitingBlank) return item?.isWaiting ? "Pending" : "Waiting";
    return "RX Only";
}

export const getRxColor = (item) => {
    if (item?.isSmarted || item?.isSmartRxBlank) return "#008000";
    else if (item?.isWaiting || item?.isWaitingBlank) return item?.isWaiting ? "#FFC000" : "#B9BB39";
    return "#FF3B30";
}
