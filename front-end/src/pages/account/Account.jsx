import { useEffect, useState } from "react";
import Feed from "../../components/feed/Feed"
import "../profile/profile.css"
import authService from "../../services/auth.service";
import { useLocation, useParams } from "react-router-dom";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
import { Box, Skeleton } from "@mui/material";
import Submissions from "../submissions/Submissions";

export default function Account() {
    const { userId } = useParams();
    const [loading, setLoading] = useState(true)
    const [user, setUser] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);
    const fetchCurrentUsereAndUser = async () => {
        setLoading(true);
        try {
            const user = authService.getCurrentUser();
            if (user) {
                setCurrentUser(user);
            }
            if (user) {
                const response = await authHeader().get(apis.user, {params:{Id: userId}});
                setUser(response.data)
            }
        } catch (error) {
            setLoading(false)
            console.error(error.response.data);
        } finally {
            setLoading(false);
        }
    };
    useEffect(()=>{
        fetchCurrentUsereAndUser();
    },[])
    console.log(user)
    const faculties = JSON.parse(localStorage.getItem("faculties"));
    const getFacultyName = (facultyId) => {
        const faculty = faculties.find(faculty => faculty.id === facultyId);
        return faculty ? faculty.name : "Unknown Faculty";
    };
    return (

        <div className="profile">
            {loading ? (
                <Box style={{ width: "100vh" }}>
                    {Array(10)
                    .fill()
                    .map((_, i) => (
                        <>
                        <Skeleton />
                        <Skeleton animation={i % 2 === 0 ? "wave" : false} />
                        </>
                    ))}
                </Box>
            ):(
                <>
                    <div className="profileInformation">
                        <div className="profileLeft">
                            <div className="profileCover">
                                <img src={user.cloudAvatar} className="profileUserImg" alt="profile"  />
                            </div>
                            
                        </div>
                        <div className="profileRight">
                            <h4 className="profileInfoName">{user.firstName} {user.lastName}</h4>
                            <h5>Email Address</h5>
                            <span>{user.email}</span>
                            <h5>Faculty</h5>
                            <span>{getFacultyName(user.facultyId)}</span>
                            <h5>Number</h5>
                            <span>{user.phoneNumber}</span>
                        </div>
                        
                    </div>
                    <div className="profileSubmissions">
                        {currentUser.roles.some(role => role === "Coordinator" || role === "Manager" || role === "Admin")? (
                            <Submissions userId = {user.id}/>
                        ):(
                            <Feed userId = {user.id}/>
                        )}
                    </div>
                </>
            )}
        </div>
    )
}
