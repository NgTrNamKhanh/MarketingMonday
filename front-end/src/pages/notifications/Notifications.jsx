import React from 'react'
import Notification from "../../components/notification/Notification";
import "./notifications.css"
export default function Notifications() {
    return (
        <div className='notifications'>
            <div className="notificationsWrapper">
                <Notification/>
                <Notification/>
                <Notification/>
                <Notification/>
                <Notification/>
            </div>
        </div>
    )
}
