let BACKEND_HOST, IMAGE_HOST;

const hostname = window.location.hostname;

if (hostname === "localhost") {
    BACKEND_HOST = process.env.REACT_APP_API_URL_LOCAL;
    IMAGE_HOST = process.env.REACT_APP_IMAGE_URL_LOCAL;
} else if (hostname.startsWith("192.168.40.")) {
    BACKEND_HOST = process.env.REACT_APP_API_URL_LAN;
    IMAGE_HOST = process.env.REACT_APP_IMAGE_URL_LAN;
} else {
    BACKEND_HOST = process.env.REACT_APP_API_URL_PROD;
    IMAGE_HOST = process.env.REACT_APP_IMAGE_URL_PROD;
}

export { BACKEND_HOST, IMAGE_HOST };
