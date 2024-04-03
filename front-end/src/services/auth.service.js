import axios from "axios";
// import { jwtDecode } from "jwt-decode";
import apis from "./apis.service";
import { jwtDecode } from "jwt-decode";
// const mockData = [
//     {
//         username: "khanh@gmail.com",
//         password: '123',
//         role: 'admin',
//     },
//     {
//         username: "viet@gmail.com",
//         password: '123',
//         role: 'manager',
//     },
//     {
//         username: "tuyen@gmail.com",
//         password: '123',
//         role: 'coordinator',
//     },
//     {
//         username: "khiem@gmail.com",
//         password: '123',
//         role: 'guess',
//     },
//     {
//         username: "long@gmail.com",
//         password: '123',
//         role: 'student',
//     },
// ]
class AuthService {

    login(email, password) {
        return axios.post(`${apis.normal}login?email=${encodeURIComponent(email)}&passWord=${encodeURIComponent(password)}`)
        .then((response) => {
            console.log(response.data);
            // Set cookie
            document.cookie = `CMU-user=${JSON.stringify(response.data)}; path=/`;
            return response.data;
        });
    }


    logout() {
        document.cookie = "CMU-user=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        // return axios.post(apis.account + "logout");
    }


    getCurrentUser() {
        const cookieString = document.cookie;
        const cookies = cookieString.split(';').map(cookie => cookie.trim());
        const userCookie = cookies.find(cookie => cookie.startsWith("CMU-user="));
    
        if (userCookie) {
            const user = JSON.parse(userCookie.substring("CMU-user=".length));
            if (user && user.jwt_token) {
                const decodedToken = jwtDecode(user.jwt_token);
                const currentTime = Date.now() / 1000;
    
                if (decodedToken.exp < currentTime) {
                    // Token has expired, clear cookie
                    document.cookie = "CMU-user=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
                    return null;
                }
            }
            return user;
        }
        return null;
    }
    
}

export default new AuthService();
