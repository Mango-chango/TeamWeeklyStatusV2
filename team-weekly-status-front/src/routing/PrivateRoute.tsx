// PrivateRoute.tsx
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { userStore } from "../store";

interface PrivateRouteProps {
  allowedRoles?: string[];
  children: React.ReactNode;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ allowedRoles, children }) => {
  const { isAuthenticated, isTeamLead, isAdmin, isCurrentWeekReporter } = userStore();
  const navigate = useNavigate();
  // console.log('isAuthenticated:', isAuthenticated);
  // console.log('isTeamLead:', isTeamLead);
  // console.log('isAdmin:', isAdmin);
  // console.log('isCurrentWeekReporter:', isCurrentWeekReporter);
  
  useEffect(() => {
    // Not authenticated, redirect to root
    if (!isAuthenticated) {
      // console.log('NOT AUTHENTICATED');
      navigate("/");
      return;
    } 
    // else {
    //   console.log('AUTHENTICATED');
    // }

    const userRoles = [
      isAdmin && "Admin",
      isTeamLead && "TeamLead",
      isCurrentWeekReporter && "CurrentWeekReporter"
    ].filter(Boolean) as string[];
    // console.log('userRoles:', userRoles);


    // Authenticated but either role is missing or not in allowedRoles, redirect to /weekly-status
    if (!userRoles.length || (allowedRoles && !userRoles.some(role => allowedRoles.includes(role)))) {
      navigate("/weekly-status");
      return;
    }

  }, [isAuthenticated, navigate, allowedRoles]);

  // If conditions don't meet, render children (the original content of the route)
  return <>{children}</>;
};

export default PrivateRoute;
