import { toast } from "react-toastify";

const useToastMessage = () => {
    const showToast = (type, message, icon = "", animation = "wave") => {
        const animationClass = animation === "pop" ? "animated-icon" : "waving-icon";

        const content = (
            <div className={`toast-${type}`}>
                {icon && <span className={animationClass}>{icon}</span>}
                {message}
            </div>
        );

        switch (type) {
            case "success":
                toast.success(content, { className: "toast-success" });
                break;
            case "error":
                toast.error(content, { className: "toast-error" });
                break;
            case "info":
                toast.info(content, { className: "toast-info" });
                break;
            case "warning":
                toast.warning(content, { className: "toast-warning" });
                break;
            default:
                toast(content, { className: "toast-custom" });
        }
    };

    return showToast;
};

export default useToastMessage;
