import Feed from "../../components/feed/Feed";
import Rightbar from "../../components/rightbar/Rightbar";
import CoordinatorSidebar from "../../components/sidebar/Coordinator/CoordinatorSidebar";
import StudentSidebar from "../../components/sidebar/Student/StudentSidebar";
import AdminSidebar from "../../components/sidebar/Admin/AdminSidebar";
import Topbar from "../../components/topbar/Topbar";
import Notifications from "../notifications/Notifications";
import Profile from "../profile/Profile";
import "./home.css"
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "../authentication/login/Login";
import ManagerSidebar from "../../components/sidebar/Manager/ManagerSidebar";
import Dashboard from "../dashboard/Dashboard";
import Submissions from "../submissions/Submissions";
import Accounts from "../admin/accounts/Accounts";
import Article from "../../components/article/Article";
import Events from "../admin/events/Events";

export default function Home() {
    return (
        <Router> 
            <div>
                <Topbar/>
                <div className="homeContainer">
                    {/* <CoordinatorSidebar/> */}
                    <AdminSidebar/>
                    {/* <ManagerSidebar/> */}
                    {/* <StudentSidebar/> */}
                    <div className="mainContent">
                        <Routes>
                            <Route path="/" element={<Feed />} />
                            <Route path="/profile" element={<Profile />} />
                            <Route path="/notifications" element={<Notifications />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/admin/dashboard" element={<Dashboard />} />
                            <Route path="/student/submission" element={<Article />} />
                            <Route path="/admin/events" element={<Events />} />
                            <Route path="/admin/submissions" element={<Submissions />} />
                            <Route path="/admin/accounts" element={<Accounts />} />
                        </Routes>
                    </div>
                </div>
            </div>
        </Router>
    )
}
