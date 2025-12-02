// envHelper.js
export function detectEnvironment() {
  const hostname = window.location.hostname;
  const port = window.location.port;
  const customEnv = process.env.REACT_APP_ENV; // from .env file

  let runtimeEnv = "production"; // default fallback

  if (
    hostname === "localhost" ||
    hostname === "127.0.0.1" ||
    hostname === "::1"
  ) {
    runtimeEnv = "localhost";
  } else if (
    /^192\.168\.\d+\.\d+$/.test(hostname) ||
    /^10\.\d+\.\d+\.\d+$/.test(hostname)
  ) {
    runtimeEnv = "lan";
  }

  return {
    env: runtimeEnv,
    hostname,
    port,
    apiPort: process.env.REACT_APP_API_PORT || "7000"
  };
}
