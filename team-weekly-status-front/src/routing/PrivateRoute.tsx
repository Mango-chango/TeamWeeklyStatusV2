// PrivateRoute.tsx
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { userStore } from "../store";

interface PrivateRouteProps {
  allowedRoles?: string[];
  children: React.ReactNode;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ allowedRoles, children }) => {
  const { isAuthenticated, role } = userStore();
  const navigate = useNavigate();
  console.log('isAuthenticated:', isAuthenticated);
  console.log('role:', role);
  
  useEffect(() => {
    // Not authenticated, redirect to root
    if (!isAuthenticated) {
      navigate("/");
      return;
    }

    // Authenticated but either role is missing or not in allowedRoles, redirect to /weekly-status
    if (!role || (allowedRoles && !allowedRoles.includes(role))) {
      navigate("/weekly-status");
      return;
    }
  }, [isAuthenticated, role, navigate, allowedRoles]);

  // If conditions don't meet, render children (the original content of the route)
  return <>{children}</>;
};

export default PrivateRoute;
