import { useEffect, useState } from "react";
import Post from "../post/Post"
import "./feed.css"
import { useParams } from "react-router-dom";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
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

export default function Feed() {
    const { facultyId } = useParams();
    const [selectedFilter, setSelectedFilter] = useState("recent");
    const [posts, setPosts] = useState([]);
    const [error, setError] = useState()
    useEffect(()=>{
        const fetchPosts = async () => {
            if (facultyId) {
                try {
                    setError(null)
                    const response = await authHeader().get(apis.article + "approved/faculty/" + facultyId);
                    setPosts(response.data)

                }catch (error) {
                    setError(error.response.data)
                    setPosts([])
                    console.error("Error fetching post data:", error);
                }
            }
        }
        fetchPosts();
    },[facultyId])
    const handleFilterChange = (event) => {
        setSelectedFilter(event.target.value);
        sortPosts(event.target.value);
    };

    const sortPosts = (filter) => {
        let sortedPosts = [...posts];
        switch (filter) {
            case "views":
                sortedPosts.sort((a, b) => b.views - a.views);
                break;
            case "likes":
                sortedPosts.sort((a, b) => b.likes - a.likes);
                break;
            case "comments":
                sortedPosts.sort((a, b) => b.commentsCount - a.commentsCount);
                break;
            default:
                // Default to sort by most recent
                sortedPosts.sort((a, b) => new Date(b.date) - new Date(a.date));
                break;
        }
        setPosts(sortedPosts);
    };

    return (
        <div className="feed">
            <div className="feedWrapper">
                <div className="postFilter">
                    <select value={selectedFilter} onChange={handleFilterChange}>
                        <option value="recent">Most Recent</option>
                        <option value="views">Most Viewed</option>
                        <option value="likes">More Liked</option>
                        <option value="comments">Most Commented</option>
                    </select>
                </div>
                {error ? (
                    <div>{error}</div>
                        ) : (
                    posts.map(post => (
                    <Post
                        post = {post}
                    />
                    ))
                )}
            </div>
        </div>
    )
}
