import axios from "axios";

export default function authHeader() {
    const cookieString = document.cookie;
    const cookies = cookieString.split(';').map(cookie => cookie.trim());
    const userCookie = cookies.find(cookie => cookie.startsWith("CMU-user="));

    if (userCookie) {
        const user = JSON.parse(userCookie.substring("CMU-user=".length));
        const token = user.jwt_token;

        return axios.create({
            headers: {
                Authorization: token ? `Bearer ${token}` : null
            },
            withCredentials: true
        });
    } else {
        // Handle case where user is not logged in
        return axios.create({
            withCredentials: true
        });
    }
}
