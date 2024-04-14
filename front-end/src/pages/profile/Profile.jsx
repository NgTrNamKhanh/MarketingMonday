import { useEffect, useState } from "react";
import Feed from "../../components/feed/Feed"
import "./profile.css"
import authService from "../../services/auth.service";
import { ScaleLoader } from "react-spinners";
import { Modal } from "@mui/material";
import AvatarForm from "../../components/forms/avatar/AvatarForm";
import PasswordForm from "../../components/forms/password/PasswordForm";

export default function Profile() {
    const [currentUser, setCurrentUser] = useState(null);
    const [optionsOpen, setOptionsOpen] = useState(false);
    const [editAvatarOpen, setEditAvatarOpen] = useState(false);
    const handleOpenEditAvatar = () => {
        setEditAvatarOpen(true)
    }
    const handleCloseEditAvatar = () => {
        setEditAvatarOpen(false)
    }
    const [changePasswordOpen, setChangePasswordOpen] = useState(false);
    const handleOpenChangePassword = () => {
        setChangePasswordOpen(true)
    }
    const handleCloseChangePassword = () => {
        setChangePasswordOpen(false)
    }
    useEffect(() => {
        const user = authService.getCurrentUser();
        if (user) {
            setCurrentUser(user);
        }
    }, []);
    if (!currentUser) {
        return (<ScaleLoader/>);
    }
    const faculties = JSON.parse(localStorage.getItem("faculties"));
    const getFacultyName = (facultyId) => {
        const faculty = faculties.find(faculty => faculty.id === facultyId);
        return faculty ? faculty.name : "Unknown Faculty";
    };
    return (
        <div className="profile">
            <div className="profileInformation">
                <div className="profileLeft">
                    <div className="profileCover">
                        <img src={currentUser.avatar} className="profileUserImg" alt="profile"  onClick={()=>setOptionsOpen(!optionsOpen)}/>
                    </div>
                    {optionsOpen && (
                        <div className="profileDropdownContent" >
                            <div className="profileDropdownContentItem" onClick={()=>handleOpenEditAvatar()}>
                                        <span className="dropdownContentItemLink">Change profile picture</span>
                            </div>
                        </div>
                    )}
                </div>
                <div className="profileRight">
                    <h4 className="profileInfoName">{currentUser.firstName} {currentUser.lastName}</h4>
                    <h5>Email Address</h5>
                    <span>{currentUser.email}</span>
                    <h5>Faculty</h5>
                    <span>{getFacultyName(currentUser.facultyId)}</span>
                    <h5>Change password</h5>
                    <div className="changePassword">
                        <span>Password must be at least 8 characters long</span>
                        <button className="changePasswordBtn" onClick={()=>handleOpenChangePassword()}>Change</button>
                    </div>
                    <h5>Number</h5>
                    <span>{currentUser.phoneNumber}</span>
                </div>
                
            </div>
            <div className="profileSubmissions">
                <Feed userId = {currentUser.id}/>
            </div>
            {editAvatarOpen && (
                <AvatarForm
                    userId={currentUser.id}
                    handleClose={handleCloseEditAvatar}
                />
            )}
            {changePasswordOpen && (
                <PasswordForm
                    email={currentUser.email}
                    handleClose={handleCloseChangePassword}
                />
            )}
        </div>
    )
}
