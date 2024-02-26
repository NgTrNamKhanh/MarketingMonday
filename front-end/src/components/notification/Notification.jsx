import { AttachFile, ChatBubble, MoreVert, PermMedia } from '@mui/icons-material'
import "./notification.css"

export default function Notification() {
    return (
        <div class="notification">
            <div class="user-pic">
                <img src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX" alt="User Profile Picture"/>
                <div class="notification-type">
                    <ChatBubble className='notification-type-icon'/>
                </div>
            </div>
            <div class="notification-content">
                <div class="username">JohnDoe</div>
                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed velit ac turpis vestibulum varius. Donec eget arcu nisi.</p>
                <div class="date">February 23, 2024</div>
            </div>
            <div class="top-right-icon">
                <MoreVert />
            </div>
        </div>
    )
}
