import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import authService from "../../services/auth.service";
import Feed from "../../components/feed/Feed";
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
import Unauthorized from "../errors/unauthorized/Unauthorized";
import { ToastContainer } from "react-toastify";
import { HubConnectionBuilder } from "@microsoft/signalr";
export default function Home() {

    const [isLoading, setIsLoading] = useState(false);
    const [currentUser, setCurrentUser] = useState(null);
    useEffect(() => {
        const fetchCurrentUser = async () => {
            setIsLoading(true)
            const user = authService.getCurrentUser();
            console.log(user)
            if (user) {
                setCurrentUser(user);
            }
            setIsLoading(false);
        };
        fetchCurrentUser();
    }, []);
    // const [message, setMessage] = useState()
    // console.log(message)
       // const [connection, setConnection] = useState()
    // useEffect(() => {
    //     const connect = async ()=>{
    //         const connection = new HubConnectionBuilder()
    //         .withUrl(apis.normal+"notificationHub")
    //         .build();
    //         connection.on("ReceiveNotification", (message)=>{
    //             setMessage(message)
    //         })

    //         await connection.start();
    //         setConnection(connection)
    //     }
    //     connect()
    // }, []);
    if (isLoading) {
        return <div>Loading...</div>;
    }
    return (
        <Router> 
            <div>
            {currentUser && <Topbar  setCurrentUser={setCurrentUser} user={currentUser}/>}
                <div className="homeContainer">
                    {currentUser && (
                            <>
                                {currentUser.roles.includes('Admin') ? (
                                    <AdminSidebar/>
                                ) : currentUser.roles.includes("Manager") ? (
                                    <ManagerSidebar/>
                                ) : currentUser.roles.includes("Student") ? (
                                    <StudentSidebar  />
                                ) : currentUser.roles.includes("Coordinator") ? (
                                    <CoordinatorSidebar />
                                ) : null}
                            </>
                        )}
                    <div className="mainContent">
                        <Routes>
                            <Route
                                path="/"
                                element={
                                    currentUser ? (
                                        currentUser.roles.includes('Admin') ? (
                                            <Navigate to="/accounts" />
                                        ) : currentUser.roles.includes('Manager') ? (
                                            <Navigate to="/dashboard" />
                                        ) : currentUser.roles.includes('Student', 'Guest') ? (
                                            <Navigate to={`/feed/${currentUser.facultyId}`} />
                                        ) : currentUser.roles.includes('Coordinator') ? (
                                            <Navigate to="/submissions" />
                                        ) : (
                                            <Navigate to="/unauthorized" />
                                        )
                                    ) : (
                                        <Navigate to="/login" replace />
                                    )
                                }
                            />
                            <Route path="/feed/:facultyId" element=
                                {<ProtectedRoute
                                    element={<Feed />}
                                    requiredRoles={['Admin', 'Manager', 'Coordinator','Student','Guest']}
                                />}  
                            />
                            <Route path="/profile" element={<Profile/>} />
                            <Route path="/notifications" element={<Notifications />} />
                            <Route path="/login" element={<Login  setCurrentUser={setCurrentUser}/>} />
                            <Route path="/dashboard" element=
                                {<ProtectedRoute
                                    element={<Dashboard />}
                                    requiredRoles={['Manager']}
                                />}  
                            />
                            <Route path="/submission" element=
                                {<ProtectedRoute
                                    element={<Article />}
                                    requiredRoles={['Student']}
                                />}  
                            />
                            <Route path="/events" element=
                                {<ProtectedRoute
                                    element={<Events />}
                                    requiredRoles={['Admin']}
                                />} 
                            />
                            <Route path="/submissions/:facultyId" element=
                                {<ProtectedRoute
                                    element={<Submissions />}
                                    requiredRoles={['Admin', 'Manager', 'Coordinator']}
                                />} 
                            />
                            <Route path="/accounts" element=
                                {<ProtectedRoute
                                    element={<Accounts />}
                                    requiredRoles={['Admin']}
                                />}  
                            />
                            <Route path="/unauthorized" element={<Unauthorized/>}/>
                        </Routes>
                    </div>
                </div>
            </div>
        </Router>
    )
}
