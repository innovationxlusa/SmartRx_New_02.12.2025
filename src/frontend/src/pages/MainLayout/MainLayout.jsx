import { Outlet } from "react-router-dom"; // Import Outlet
import Header from "../../components/static/Header/Header";
import Footer from "../../components/static/Footer/Footer";

const MainLayout = () => {
    return (
        <main className="app-padding-bottom">
            <div
                className="container"
                style={{
                    display: "flex",
                    flexDirection: "column",
                    paddingTop: "100px",
                }}
            >
                <Header />
                {/* This is where nested routes will render */}
                <div className="main-scroll-container">
                    <Outlet />
                </div>
            </div>
            {/* Footer rendered outside container to remain truly fixed */}
            <Footer />
        </main>
    );
};

export default MainLayout;
