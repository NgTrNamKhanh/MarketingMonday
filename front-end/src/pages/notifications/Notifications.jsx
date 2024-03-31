import React, { useEffect, useState } from 'react'
import Notification from "../../components/notification/Notification";
import "./notifications.css"
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import { ScaleLoader } from 'react-spinners';
import authService from '../../services/auth.service';
export default function Notifications() {
    const [notifications, setNotifications] = useState([]);
    const [loading ,setLoading] = useState()
    const [currentUser, setCurrentUser] = useState(null);
    useEffect(() => {
        const fetchCurrentUser = async () => {
            setLoading(true)
            const user = authService.getCurrentUser();
            if (user) {
                setCurrentUser(user);
            }
            setLoading(false);
        };
        fetchCurrentUser();
    }, []);
    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            if (currentUser) {
                try {
                    const response = await authHeader().get(apis.notification, {params:{userId: currentUser.id}});
                    if (Array.isArray(response.data)) {
                        setNotifications(response.data);
                    } else {
                        console.error("Unexpected data format for notifications:", response.data);
                    }
                } catch (error) {
                    console.error("Error fetching notifications:", error);
                }
            }
            setLoading(false);
        };
        fetchData();
    }, [currentUser]);
    
    return (
        <div className='notifications'>
            {loading && !notifications ? (
                    <ScaleLoader/>
                ):(
                    <div className="notificationsWrapper" >
                        {notifications.map(notification => (
                            <Notification
                                notification = {notification}
                            />
                        ))}
                    </div>
            )}
        </div>
    )
}
