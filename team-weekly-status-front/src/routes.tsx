import React from 'react';
import { Route, Routes } from "react-router-dom";
import { userStore } from './store';

const SignIn = React.lazy(() => import('./components/SignIn'));
const WeeklyStatus = React.lazy(() => import('./components/WeeklyStatus'));

const AppRoutes: React.FC = () => {
    const { role } = userStore();

    return (
        <Routes>
            <Route path="/" element={<SignIn />} />
            <Route path="/weekly-status" element={<WeeklyStatus role={role} />} />
        </Routes>
    );
}

export default AppRoutes;
