import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import "bootstrap/dist/css/bootstrap.min.css";
import "react-toastify/dist/ReactToastify.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import "./assets/css/toastStyles.css";
import { UserProvider } from "./contexts/UserContext";
import { AuthProvider } from "./contexts/AuthContext";
import { FolderProvider } from "./contexts/FolderContext";
import { PreviewProvider } from "./contexts/PreviewContext";

const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(
    <React.StrictMode>
        <BrowserRouter>
            <AuthProvider>
                <UserProvider>
                    <FolderProvider>
                        <PreviewProvider>
                            <App />
                        </PreviewProvider>
                    </FolderProvider>
                </UserProvider>
            </AuthProvider>
        </BrowserRouter>
    </React.StrictMode>
);
