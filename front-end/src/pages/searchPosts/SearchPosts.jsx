import React, { useEffect, useState } from 'react'
import authService from '../../services/auth.service';
import authHeader from '../../services/auth.header';
import { Box, Skeleton } from '@mui/material';
import Submission from '../../components/submission/Submission';
import apis from '../../services/apis.service';
import {  useParams } from 'react-router-dom';
import Post from '../../components/post/Post';

export default function SearchPosts() {
    const [submissions, setSubmissions] = useState([]);
    const [error, setError] = useState()
    const [currentUser, setCurrentUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const { search } = useParams();

    useEffect(() => {
        const fetchCurrentUser = async () => {
            setLoading(true)
            const user = authService.getCurrentUser();
            if (user) {
                setCurrentUser(user);
            }
            setLoading(false)
        };
        fetchCurrentUser();
    }, []);
    const fetchSubmissions = async () => {
        if (search && currentUser) {
            setLoading(true);
            try {
                let response;
                if (currentUser.roles.some(role => role === "Admin" || role === "Manager")) {
                    response = await authHeader().get(apis.article + `title/${encodeURIComponent(search)}?userId=${currentUser.id}`);
                } else if (currentUser.roles.some(role => role === "Student" || role === "Coordinator")) {
                    response = await authHeader().get(apis.article + `approved/title/${encodeURIComponent(search)}?userId=${currentUser.id}`);
                } else if (currentUser.roles.includes("Guest")) {
                    response = await authHeader().get(apis.article + `guest/approved/title/${encodeURIComponent(search)}?userId=${currentUser.id}`);
                }
                setError(null);
                setSubmissions(response.data);
                setLoading(false);
            } catch (error) {
                setSubmissions([]);
                setError(error.response.data);
                console.error(error.response.data);
                setLoading(false);
            }
        }
    }
    
    useEffect(() => {
        fetchSubmissions();
    }, [search, currentUser]);
    

    return (
        <div className="submissions">
            <h1>Search for</h1>
            {loading ? (
                <Box style={{ width: "100vh" }}>
                    {Array(10)
                    .fill()
                    .map((_, i) => (
                        <>
                        <Skeleton />
                        <Skeleton animation={i % 2 === 0 ? "wave" : false} />
                        </>
                    ))}
                </Box>
            ):(
                <div className="submissionsWrapper">
                    
                    {error ? (
                        <div>{error}</div>
                    ) : (
                    <>
                    {currentUser.roles.some(role => role === "Admin" || role === "Manager" || role === "Coordinator") && (
                        submissions.map((submission) => (
                            <div key={submission.id}>
                                <Submission submission={submission} currentUser={currentUser}/>
                            </div>
                        ))
                    )}
                    {currentUser.roles.some(role => role === "Student" || role === "Guest") && (
                        submissions.map((submission) => (
                            <div key={submission.id}>
                                <Post post={submission} currentUser={currentUser} isProfile = {false}/>
                            </div>
                        ))
                    )}
                    </>
                    )}
                </div>
            )}
        </div>
    )
}
