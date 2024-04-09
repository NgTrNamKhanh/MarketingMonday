import axios from "axios";
import { jwtDecode } from "jwt-decode";

export default function authHeader() {
    const cookieString = decodeURIComponent(document.cookie);
    const cookies = cookieString.split(';').map(cookie => cookie.trim());
    const userCookie = cookies.find(cookie => cookie.startsWith("CMU-user="));

    if (userCookie) {
        const userJsonString = userCookie.substring("CMU-user=".length);
        const user = JSON.parse(userJsonString);
        if (user && user.jwt_token) {
            return axios.create({
                headers: {
                    Authorization: user.jwt_token ? `Bearer ${user.jwt_token}` : null
                },
                withCredentials: true
            });
        }
    } else {
        // Handle case where user is not logged in
        return axios.create({
            withCredentials: true
        });
    }
}
