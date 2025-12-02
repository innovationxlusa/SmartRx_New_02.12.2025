import { createContext, useContext, useState } from "react";

const PreviewContext = createContext();

export const PreviewProvider = ({ children }) => {
    const [previewData, setPreviewData] = useState(null);

    return <PreviewContext.Provider value={{ previewData, setPreviewData }}>{children}</PreviewContext.Provider>;
};

export const usePreviewContext = () => useContext(PreviewContext);
