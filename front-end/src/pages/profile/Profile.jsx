import { useEffect, useState } from "react";
import Feed from "../../components/feed/Feed"
import "./profile.css"
import authService from "../../services/auth.service";
import { ScaleLoader } from "react-spinners";

export default function Profile() {
    const [currentUser, setCurrentUser] = useState(null);

    useEffect(() => {
        const user = authService.getCurrentUser();
        if (user) {
            setCurrentUser(user);
        }
    }, []);
    if (!currentUser) {
        return (<ScaleLoader/>);
    }
    return (
            <div className="profile">
            <div className="profileRight">
                <div className="profileRightTop">
                    <div className="profileCover">
                        <img src={`data:image/jpeg;base64,${currentUser.avatar}`} className="profileCoverImg" alt="profile" />
                        <img src={`data:image/jpeg;base64,${currentUser.avatar}`} className="profileUserImg" alt="profile" />
                    </div>
                </div>
                <div className="profileInfo">
                    <h4 className="profileInfoName">{currentUser.firstName} {currentUser.lastName}</h4>
                </div>
                <div className="profileRightBottom">
                    <Feed userId = {currentUser.id}/>
                </div>
            </div>
        </div>
    )
}
