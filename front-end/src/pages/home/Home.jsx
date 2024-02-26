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
import ManagerSidebar from "../../components/sidebar/Manager/ManagerSidebar";

export default function Home() {
    return (
        <Router> {/* Wrap your Routes with a Router */}
            <div>
                <Topbar/>
                <div className="homeContainer">
                    {/* <CoordinatorSidebar/> */}
                    {/* <AdminSidebar/> */}
                    {/* <ManagerSidebar/> */}
                    <StudentSidebar/>
                    <div className="mainContent">
                        <Routes>
                            <Route path="/" element={<Feed />} />
                            <Route path="/profile" element={<Profile />} />
                            <Route path="/notifications" element={<Notifications />} />
                        </Routes>
                    </div>
                    <Rightbar/>
                </div>
            </div>
        </Router>
    )
}
