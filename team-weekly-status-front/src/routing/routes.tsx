import React from "react";
import { Route, Routes } from "react-router-dom";
import { userStore } from "../store";
import PrivateRoute from "./PrivateRoute";
import AssignReporter from "../components/AssignReporter";

const SignIn = React.lazy(() => import("../components/SignIn"));
const WeeklyStatus = React.lazy(() => import("../components/WeeklyStatus"));
const StatusReporting = React.lazy(
  () => import("../components/StatusReporting/index")
);

const AppRoutes: React.FC = () => {
  const { role } = userStore();

  return (
    <Routes>
      <Route path="/" element={<SignIn />} />
      <Route
        path="/weekly-status"
        element={
          <PrivateRoute>
            <WeeklyStatus />
          </PrivateRoute>
        }
      />
      <Route
        path="/status-reporting"
        element={
          <PrivateRoute allowedRoles={["CurrentWeekReporter"]}>
            <StatusReporting />
          </PrivateRoute>
        }
      />
      <Route
        path="/assign-reporter"
        element={
          <PrivateRoute allowedRoles={["TeamLead"]}>
            <AssignReporter />
          </PrivateRoute>
        }
      />
      <Route
        path="*"
        element={
          <PrivateRoute>
            <WeeklyStatus />
          </PrivateRoute>
        }
      />
    </Routes>
  );
};

export default AppRoutes;
