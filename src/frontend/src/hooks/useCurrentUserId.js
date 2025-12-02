import { useLocation } from "react-router-dom";
import { useMemo } from "react";
import { useUserContext } from "../contexts/UserContext";

const candidateFrom = (value) => {
  if (value === undefined || value === null || value === "") {
    return null;
  }
  const numeric = Number(value);
  return Number.isFinite(numeric) && numeric > 0 ? numeric : null;
};

const useCurrentUserId = () => {
  const { user } = useUserContext();
  const location = useLocation();

  return useMemo(() => {
    const candidates = [
      user?.UserId,
      user?.userId,
      user?.id,
      user?.userID,
      location?.state?.userId,
      location?.state?.UserId,
      location?.state?.user?.id,
      location?.state?.user?.UserId,
      location?.state?.patient?.userId,
    ];

    for (const value of candidates) {
      const normalized = candidateFrom(value);
      if (normalized !== null) {
        return normalized;
      }
    }

    return null;
  }, [user, location]);
};

export default useCurrentUserId;

