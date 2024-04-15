import axios from "axios";
// import { jwtDecode } from "jwt-decode";
import apis from "./apis.service";
import { jwtDecode } from "jwt-decode";
import Cookies from 'universal-cookie'
class AuthService {
    constructor() {
        this.cookies = new Cookies();
    }
    login(email, password) {
        // Check if "CMU-user" cookie exists and delete it if it does
        if (this.cookies.get("CMU-user")) {
            this.cookies.remove("CMU-user");
        }
    
        return axios.post(`${apis.normal}login?email=${encodeURIComponent(email)}&passWord=${encodeURIComponent(password)}`)
        .then((response) => {
            // Set cookie
            const decodedToken = jwtDecode(response.data.jwt_token);
            this.cookies.set("CMU-user", response.data, {
                expires: new Date(decodedToken.exp * 1000)
            });
            return response.data;
        });
    }
    


    logout() {
        this.cookies.remove("CMU-user");
        // return axios.post(apis.account + "logout");
    }


    getCurrentUser() {
        const cookieString = decodeURIComponent(document.cookie);
        const cookies = cookieString.split(';').map(cookie => cookie.trim());
        const userCookie = cookies.find(cookie => cookie.startsWith("CMU-user="));
        if (userCookie) {
            const userJsonString = userCookie.substring("CMU-user=".length);
            const user = JSON.parse(userJsonString);
            if (user && user.jwt_token) {
                const decodedToken = jwtDecode(user.jwt_token);
                const currentTime = Date.now() / 1000;
    
                if (decodedToken.exp < currentTime) {
                    // Token has expired, clear cookie
                    this.cookies.remove("CMU-user")
                    return null;
                }
            }
            return user;
        }
        return null;
    }
    
    
    
}

export default new AuthService();
