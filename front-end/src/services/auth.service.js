import axios from "axios";
// import { jwtDecode } from "jwt-decode";
import apis from "./apis.service";
const mockData = [
    {
        username: "khanh@gmail.com",
        password: '123',
        role: 'admin',
    },
    {
        username: "viet@gmail.com",
        password: '123',
        role: 'manager',
    },
    {
        username: "tuyen@gmail.com",
        password: '123',
        role: 'coordinator',
    },
    {
        username: "khiem@gmail.com",
        password: '123',
        role: 'guess',
    },
]
class AuthService {

    login(email, password) {
        //real 
        // return axios
        // .post(apis.account + "login", {
        //     email,
        //     password,
        // })
        // .then((response) => {
        //     if (response.data.jwt_token) {
        //     console.log(response.data);
        //     localStorage.setItem("user", JSON.stringify(response.data));
        //     }
        //     return response.data;
        // });
        //mock
        const user = mockData.find(user => user.username === email && user.password === password);
        
        if (!user) {
            return Promise.reject("Wrong email or password");
        }
        const userData = {
            username: user.username,
            role: user.role,
        };
        localStorage.setItem("user", JSON.stringify(userData));

        // Resolve with user data
        return Promise.resolve(userData);

    }


    logout() {
        localStorage.removeItem("user");
        // return axios.post(apis.account + "logout");
    }

    register(formData) {
        return axios.post(apis.account + "register", formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
        });
    }

    getCurrentUser() {
        const user = JSON.parse(localStorage.getItem("user"));

        // if (user && user.jwt_token) {
        // const decodedToken = jwtDecode(user.jwt_token);
        // const currentTime = Date.now() / 1000;

        // if (decodedToken.exp < currentTime) {
        //     // Token has expired, clear local storage
        //     localStorage.removeItem("user");
        //     return null;
        // }
        // }
        return user;
    }
}

export default new AuthService();
