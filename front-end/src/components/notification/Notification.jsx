import { MoreVert, } from '@mui/icons-material'
import "./notification.css"
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';

export default function Notification({notification,notificationCount, setNotificationCount}) {
    const handleNotification =async()=>{
        try {
            if(!notification.isRead){
                const url = `${apis.notification}markasread?notificationId=${notification.id}`;
                const response = await authHeader().post(url);
                setNotificationCount(notificationCount-1)
            }
        } catch (error) {
        }
    }
    return (
        <div class="notification" onClick={handleNotification}>
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
            <div class="top-right-icon">
                <MoreVert />
            </div>
        </div>
    )
}
