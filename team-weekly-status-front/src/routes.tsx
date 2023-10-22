import React from 'react';
import { Route, Routes } from "react-router-dom";
import { userStore } from './store';

const SignIn = React.lazy(() => import('./components/SignIn'));
const WeeklyStatus = React.lazy(() => import('./components/WeeklyStatus'));
const StatusReporting = React.lazy(() => import('./components/StatusReporting/index'));

const AppRoutes: React.FC = () => {
    const { role } = userStore();

    return (
        <Routes>
            <Route path="/" element={<SignIn />} />
            <Route path="/weekly-status" element={<WeeklyStatus />} />
            <Route path="/status-reporting" element={<StatusReporting />} />
        </Routes>
    );
}

export default AppRoutes;
