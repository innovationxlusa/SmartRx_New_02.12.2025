import { createContext, useContext, useState } from "react";

const FolderContext = createContext();

export const FolderProvider = ({ children }) => {
    const [selectedFolder, setSelectedFolder] = useState(null);
    const [refetch, setRefetch] = useState(() => () => {});

    return (
        <FolderContext.Provider
            value={{
                selectedFolder,
                setSelectedFolder,
                refetch,
                setRefetch,
            }}
        >
            {children}
        </FolderContext.Provider>
    );
};

export const useFolder = () => useContext(FolderContext);
