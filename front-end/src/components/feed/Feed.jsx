import { useEffect, useState } from "react";
import Post from "../post/Post"
import "./feed.css"
import { useParams } from "react-router-dom";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
import authService from "../../services/auth.service";
import Skeleton from "@mui/material/Skeleton";
import { Box} from "@mui/material";
// const postsData = [
//     {
//         id: 1,
//         img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//         username: "user1",
//         date: "February 20, 2024",
//         title: "Post Title 1",
//         content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
//         postimgs: [
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//             "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
//             "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX"
//         ],
//         likes: 10,
//         dislikes: 2,
//         commentsCount: 5,
//         files: [
//             // {
//             //     name: 'tut3-RADconcepts.doc',
//             //     lastModified: 1706591705000,
//             //     lastModifiedDate: new Date('Tue Jan 30 2024 12:15:05 GMT+0700 (Indochina Time)'),
//             //     size: 30208,
//             //     type: 'application/msword',
//             //     webkitRelativePath: ''
//             // }
//         ]
//     },
//     {
//         id: 2,
//         img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//         username: "user2",
//         date: "February 19, 2024",
//         title: "Post Title 2",
//         content: "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
//         postimgs: [
//             "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX"
//         ],
//         likes: 20,
//         dislikes: 1,
//         commentsCount: 8,
//         files: [
//             // Add file objects here if needed
//         ]
//     },
//     {
//         id: 3,
//         img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
//         username: "user2",
//         date: "February 22, 2024",
//         title: "Post Title 3",
//         content: "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
//         postimgs: [
//             "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
//             "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX"
//         ],
//         likes: 1,
//         dislikes: 10,
//         commentsCount: 100,
//         files: [
//             // Add file objects here if needed
//         ]
//     }
//     // Add more posts as needed
// ];

export default function Feed({userId}) {
    const { facultyId } = useParams();
    const [approvedSelectedFilter, setApprovedSelectedFilter] = useState("recent");
    const isProfile = userId ? true : false;
    const [posts, setPosts] = useState([]);
    const [error, setError] = useState()
    const [currentUser, setCurrentUser] = useState(null);
    const [loading, setLoading] = useState(false)
    useEffect(() => {
        setLoading(true)
        const user = authService.getCurrentUser();
        if (user) {
            setCurrentUser(user);
            setLoading(false)
        }
    }, []);
    const [filteredPosts, setFilteredPosts] = useState([]);
    const [unapprovedSelectedFilter, setSelectedFilter] = useState("all");
    const handleUnapprovedFilterChange = (event) => {
        setSelectedFilter(event.target.value);
        filterUnapprovedPosts(event.target.value);
    };
    const filterUnapprovedPosts = (filter) => {
        let filteredPosts = [...posts];
        switch (filter) {
            case "all":
                filteredPosts = posts;
                break;
            case "approved":
                filteredPosts = filteredPosts.filter(submission => submission.publishStatusId === 1);
                break;
            case "not commented":
                filteredPosts = filteredPosts.filter(submission => submission.coordinatorComment === null && submission.publishStatusId === 3);
                break;
            case "commented":
                filteredPosts = filteredPosts.filter(submission => submission.coordinatorComment !== null && submission.publishStatusId === 3);
                break;
            case "reject":
                filteredPosts = filteredPosts.filter(submission => submission.publishStatusId === 2);
                break;
            default:
                filteredPosts = posts;
                break;
        }
        setFilteredPosts(filteredPosts);
    };
    useEffect(() => {
        const fetchPosts = async () => {
            if (facultyId && currentUser) {
                setLoading(true)
                try {
                    setError(null)
                    let url;
                    if (currentUser.roles.includes('guest')) {
                        url = `${apis.article}guest/faculty`;
                    } else {
                        url = `${apis.article}approved/faculty`;
                    }
                    const response = await authHeader().get(url, { params: { facultyId: facultyId, userId: currentUser.id }});
                    setFilteredPosts(response.data)
                    setPosts(response.data)
                    setLoading(false)
                } catch (error) {
                    console.error("Error fetching post data:", error);
                    if (error.response) {
                        // Server responded with an error
                        setError(error.response.data)
                    } else if (error.request) {
                        // Request was made but no response was received
                        setError("No response received from server.")
                    } else {
                        // Something else went wrong
                        setError("An error occurred while fetching data.")
                    }
                    setPosts([])
                    setFilteredPosts([])
                    setLoading(false)
                }
            }
        }
        fetchPosts();
    }, [facultyId, currentUser])
    
    useEffect(()=>{
        const fetchPosts = async () => {
            if (userId) {
                setLoading(true)
                try {
                    setError(null)
                    const response = await authHeader().get(apis.article + "profile", {params: {userId: userId}});
                    setPosts(response.data)
                    setFilteredPosts(response.data)
                    setLoading(false)
                }catch (error) {
                    setError(error.response.data)
                    setPosts([])
                    setFilteredPosts([])
                    console.error("Error fetching post data:", error);
                    setLoading(false)
                }
            }
        }
        fetchPosts();
    },[userId])
    const handleApprovedFilterChange = (event) => {
        setSelectedFilter("all");
        setApprovedSelectedFilter(event.target.value);
        sortApprovedPosts(event.target.value);
    };

    const sortApprovedPosts = (filter) => {
        let sortedPosts = [...posts];
        switch (filter) {
            case "views":
                sortedPosts.sort((a, b) => b.viewCount - a.viewCount);
                break;
            case "likes":
                sortedPosts.sort((a, b) => b.likeCount - a.likeCount);
                break;
            case "comments":
                sortedPosts.sort((a, b) => b.commentsCount - a.commentsCount);
                break;
            default:
                // Default to sort by most recent
                sortedPosts.sort((a, b) => new Date(b.date) - new Date(a.date));
                break;
        }
        setFilteredPosts(sortedPosts);
    };
    return (
        <div className="feed">
            <div className="feedWrapper">
                <div className="postFilter">
                    <select value={approvedSelectedFilter} onChange={handleApprovedFilterChange} className="filter" disabled={loading}>
                        <option value="recent">Most Recent</option>
                        <option value="views">Most Viewed</option>
                        <option value="likes">More Liked</option>
                        <option value="comments">Most Commented</option>
                    </select>
                </div>
                {userId && (
                        <div className="postFilter">
                            <select value={unapprovedSelectedFilter} onChange={handleUnapprovedFilterChange} className="filter" disabled={loading}>
                            <option value="all">All</option>
                            <option value="approved">Approved</option>
                            <option value="not commented">Not Commented</option>
                            <option value="commented">Commented</option>
                            <option value="reject">Reject</option>
                            </select>
                        </div>
                )}
                {(loading && currentUser) ? (
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
                ) : error ? (
                <p>{error}</p>
                ) : (
                    filteredPosts.map(post => (
                        <Post
                            post = {post}
                            isProfile = {isProfile}
                            currentUser={currentUser}
                        />
                        ))
                )}
            </div>
        </div>
    )
}
