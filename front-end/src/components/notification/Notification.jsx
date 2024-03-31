import { AttachFile, ChatBubble, MoreVert, PermMedia } from '@mui/icons-material'
import "./notification.css"

export default function Notification({notification}) {
    return (
        <div class="notification">
            <div class="user-pic">
                <img src={`data:image/jpeg;base64,${notification.userNoti.userAvatar}`} className="topbarImg" alt="profile" />
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
