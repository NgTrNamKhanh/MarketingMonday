import { useEffect, useState } from "react";
import authHeader from "../services/auth.header";
// import authHeader from "../services/auth-header";

const useFetch = (url) => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState();

    useEffect(() => {
    const fetchData = async () => {
        if (!url) {
            // Skip fetching if url is null
            setData([]);
            setLoading(false);
            return;
        }
        setLoading(true);
        try {
            const res = await authHeader().get(url)
            setData(res.data)
            // const instance = axios.create({
            //     baseURL: url
            // });
            //     instance.interceptors.request.use(
            //     (config) => {
            //             const token = JSON.parse(localStorage.getItem('CMU-user')).jwt_token;
            //             if (token) {
            //                 config.headers.Authorization = `Bearer ${token}`;
            //             }
            //             return config;
            //     },
            //     (error) => {
            //         return Promise.reject(error);
            //     }
            // );
            // const response = await instance.get('');
            // setData(response.data);
            // const res = await axios.get(url, {
            //     headers: authHeader(),
            //     withCredentials: true,
            // });
            // setData(res.data);
        } catch (err) {
        setError(err.response?.data || err.toString());
        }
        setLoading(false);
    };

    fetchData();
    }, [url]);

    const reFetch = async () => {
    if (!url) {
        // Skip refetching if url is null
        setData([]);
        setLoading(false);
        return [];
    }

    setLoading(true);
    try {
        const res = await authHeader().get(url)
        const newData = res.data;
        setData(newData);
        setLoading(false);
        return newData;
    } catch (err) {
        setError(err.response?.data?.message || err.toString());
        setLoading(false);
        throw err;
    }
    };

    return { data, loading, error, reFetch };
};

export default useFetch;
