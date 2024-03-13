import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import authService from "../../services/auth.service";
import Feed from "../../components/feed/Feed";
import Rightbar from "../../components/rightbar/Rightbar";
import CoordinatorSidebar from "../../components/sidebar/Coordinator/CoordinatorSidebar";
import StudentSidebar from "../../components/sidebar/Student/StudentSidebar";
import AdminSidebar from "../../components/sidebar/Admin/AdminSidebar";
import Topbar from "../../components/topbar/Topbar";
import Notifications from "../notifications/Notifications";
import Profile from "../profile/Profile";
import Login from "../authentication/login/Login";
import ManagerSidebar from "../../components/sidebar/Manager/ManagerSidebar";
import Dashboard from "../dashboard/Dashboard";
import Submissions from "../submissions/Submissions";
import Accounts from "../admin/accounts/Accounts";
import Events from "../admin/events/Events";
import Article from "../article/Article";
import './home.css'
import { ProtectedRoute } from "../../common/with-router";

export default function Home() {
    const [currentUser, setCurrentUser] = useState(null);

    useEffect(() => {
        const user = authService.getCurrentUser();
        if (user) {
            setCurrentUser(user);
        }
    }, []);


    return (
        <Router> 
            <div>
            {currentUser && <Topbar  setCurrentUser={setCurrentUser}/>}
                <div className="homeContainer">
                    {currentUser && (
                            <>
                                {currentUser.role === 'admin' ? (
                                    <AdminSidebar/>
                                ) : currentUser.role === "manager" ? (
                                    <ManagerSidebar/>
                                ) : currentUser.roles === "student" ? (
                                    <StudentSidebar  />
                                ) : currentUser.role === "coordinator" ? (
                                    <CoordinatorSidebar />
                                ) : null}
                            </>
                        )}
                    <div className="mainContent">
                        <Routes>
                            <Route path="/" element=
                                {<ProtectedRoute
                                    element={<Feed />}
                                    requiredRoles={['admin', 'manager', 'coordinator','student','guess']}
                                />}  
                            />
                            <Route path="/profile" element={<Profile />} />
                            <Route path="/notifications" element={<Notifications />} />
                            <Route path="/login" element={<Login  setCurrentUser={setCurrentUser}/>} />
                            <Route path="/admin/dashboard" element=
                                {<ProtectedRoute
                                    element={<Dashboard />}
                                    requiredRoles={['admin', 'manager']}
                                />}  
                            />
                            <Route path="/student/submission" element=
                                {<ProtectedRoute
                                    element={<Article />}
                                    requiredRoles={['student']}
                                />}  
                            />
                            <Route path="/admin/events" element=
                                {<ProtectedRoute
                                    element={<Events />}
                                    requiredRoles={['admin']}
                                />} 
                            />
                            <Route path="/manager/submissions" element=
                                {<ProtectedRoute
                                    element={<Submissions />}
                                    requiredRoles={['admin', 'manager']}
                                />} 
                            />
                            <Route path="/admin/accounts" element=
                                {<ProtectedRoute
                                    element={<Accounts />}
                                    requiredRoles={['admin']}
                                />}  
                                />
                        </Routes>
                    </div>
                </div>
            </div>
        </Router>
    )
}
