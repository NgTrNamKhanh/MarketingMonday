import { MoreVert, } from '@mui/icons-material'
import "./notification.css"
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

export default function Notification({notification,setNotifications, notifications}) {
    const [optionsOpen, setOptionsOpen] = useState(false);
    const navigator = useNavigate();
    const handleNotificationClicked = async () => {
        await handleNotification;
        navigator(`/post/${notification.articleId}`)
    }
    const handleNotification =async()=>{
        try {
            if(!notification.isRead){
                const url = `${apis.notification}markasread`;
                const response = await authHeader().post(url, {params:{notificationId: notification.id}});
                markAsRead();
            }
        } catch (error) {
        }
    }

    const markAsRead = ()=> {
        const updatedNotifications = notifications.map(notif => {
            if (notif.id === notification.id) {
                return { ...notif, isRead: true };
            }
            return notif;
        });
        setNotifications(updatedNotifications);
    }
    const handleDeleteNotification=async()=>{
        try {
            const url = `${apis.notification}deletenoti`;
            const response = await authHeader().delete(url, {params:{notificationId: notification.id}});
            deleteNotification(notification.id)
        } catch (error) {
        }
        
    }
    const deleteNotification = (notificationId) => {
        const updatedNotifications = notifications.filter(notif => notif.id !== notificationId);
        setNotifications(updatedNotifications);
    };
    return (
        <div className={`notification ${!notification.isRead ? 'read' : ''}`} >
            <div class="user-pic">
                <Link
                    to={`/account/${notification.userNoti.id}`}
                >
                    <img src={notification.userNoti.userAvatar} className="topbarImg" alt="profile" />
                </Link>
                {/* <div class="notification-type">
                    <ChatBubble className='notification-type-icon'/>
                </div> */}
            </div>
            <div class="notification-content" onClick={handleNotificationClicked}>
                <Link
                    to={`/account/${notification.userNoti.id}`}
                >
                    <div class="username">{notification.userNoti.firstName} {notification.userNoti.lastName}</div>
                </Link>
                <p>{notification.message}</p>
                <div class="date">February 23, 2024</div>
            </div>
            <div class="top-right-icon" onClick={()=>setOptionsOpen(!optionsOpen)}>
                <MoreVert />
                    {optionsOpen && (
                        <div className="notificationDropdownContent" >
                            {!notification.isRead && (
                                <div className="notificationDropdownContentItem" onClick={()=>handleNotification()}>
                                                <span>Mark as read</span> 
                                    </div>
                            )}
                            <div className="notificationDropdownContentItem" onClick={()=>handleDeleteNotification()}>
                                        <span>Delete</span> 
                            </div>
                        </div>
                    )}
            </div>
        </div>
    )
}
