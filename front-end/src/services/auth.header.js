import axios from "axios";

export default function authHeader() {
    const token = JSON.parse(localStorage.getItem('CMU-user')).jwt_token;

    return axios.create({
        headers: {
            Authorization: token ? `Bearer ${token}` : null
        },
        withCredentials: true
    })
}