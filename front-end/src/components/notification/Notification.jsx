import { MoreVert, } from '@mui/icons-material'
import "./notification.css"
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Notification({notification,setNotifications, notifications}) {
    const [optionsOpen, setOptionsOpen] = useState(false);
    const navigator = useNavigate();
    const handleNotification =async()=>{
        try {
            if(!notification.isRead){
                const url = `${apis.notification}markasread?notificationId=${notification.id}`;
                const response = await authHeader().post(url);
                markAsRead();
            }
            navigator(`/post/${notification.articleId}`)

        } catch (error) {
        }
    }

    const markAsRead =()=> {
        const updatedNotifications = notifications.map(notif => {
            if (notif.id === notification.id) {
                return { ...notif, isRead: true };
            }
            return notif;
        });
        setNotifications(updatedNotifications);
    }
    const handleDeleteNotification=()=>{
        
    }
    return (
        <div className={`notification ${!notification.isRead ? 'read' : ''}`} onClick={handleNotification}>
            <div class="user-pic">
                <img src={notification.userNoti.userAvatar} className="topbarImg" alt="profile" />
                {/* <div class="notification-type">
                    <ChatBubble className='notification-type-icon'/>
                </div> */}
            </div>
            <div class="notification-content">
                <div class="username">{notification.userNoti.firstName} {notification.userNoti.lastName}</div>
                <p>{notification.message}</p>
                <div class="date">February 23, 2024</div>
            </div>
            <div class="top-right-icon" onClick={()=>setOptionsOpen(!optionsOpen)}>
                <MoreVert />
                    {optionsOpen && (
                        <div className="notificationDropdownContent" >
                                    <div className="notificationDropdownContentItem" onClick={()=>markAsRead()}>
                                                <span>Mark as read</span> 
                                    </div>
                                    <div className="notificationDropdownContentItem" onClick={()=>handleDeleteNotification()}>
                                                <span>Delete</span> 
                                    </div>
                        </div>
                    )}
            </div>
        </div>
    )
}
