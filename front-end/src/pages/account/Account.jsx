import { useEffect, useState } from "react";
import Feed from "../../components/feed/Feed"
import "../profile/profile.css"
import authService from "../../services/auth.service";
import { useLocation, useParams } from "react-router-dom";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
import { Box, Skeleton } from "@mui/material";

export default function Account() {
    const { userId } = useParams();
    const [loading, setLoading] = useState(true)
    const [currentUser, setCurrentUser] = useState(null);
    useEffect(() => {
        const fetchUser = async () =>{
            try {
                const response = await authHeader().get(apis.user, {params:{Id: userId}});
                setCurrentUser(response.data)
                setLoading(false)
            }catch (error) {
                console.error(error.response.data);
                setLoading(false)
            }
        }
        fetchUser()
    }, []);
    console.log(currentUser)
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
                                <img src={currentUser.cloudAvatar} className="profileUserImg" alt="profile"  />
                            </div>
                            
                        </div>
                        <div className="profileRight">
                            <h4 className="profileInfoName">{currentUser.firstName} {currentUser.lastName}</h4>
                            <h5>Email Address</h5>
                            <span>{currentUser.email}</span>
                            <h5>Faculty</h5>
                            <span>{getFacultyName(currentUser.facultyId)}</span>
                            <h5>Number</h5>
                            <span>{currentUser.phoneNumber}</span>
                        </div>
                        
                    </div>
                    <div className="profileSubmissions">
                        <Feed userId = {currentUser.id}/>
                    </div>
                </>
            )}
        </div>
    )
}
