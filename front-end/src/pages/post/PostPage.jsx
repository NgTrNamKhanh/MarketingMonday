import React, { useEffect, useState } from 'react'
import Post from "../../components/post/Post"
import { useParams } from 'react-router-dom';
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import { Box, Skeleton } from '@mui/material';
import authService from '../../services/auth.service';

export default function PostPage() {
    const { postId } = useParams();
    const [loading, setLoading] = useState(true);
    const [post, setPost] = useState(null);
    const [error, setError] = useState()
    const [currentUser, setCurrentUser] = useState(null);
    const fetchArticleAndUser = async () => {
        setLoading(true);
        try {
            setError(null);
            const user = authService.getCurrentUser();
            if (user) {
                setCurrentUser(user);
            }
            
            if (postId && user) {
                const response = await authHeader().get(apis.article + "id/", { params: { articleId: postId, userId: user.id } });
                setPost(response.data);
            }
        } catch (error) {
            setPost(null);
            setError(error.response.data);
            console.error(error.response.data);
        } finally {
            setLoading(false);
        }
    };
    useEffect(()=>{
        fetchArticleAndUser();
    },[postId])
    console.log(loading)

    return (
        <div className="feed">
            <div className="feedWrapper">
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
                    <Post
                        post={post}
                        isProfile={false}
                        currentUser={currentUser}
                    />
                )}
            </div>
        </div>
    )
}
