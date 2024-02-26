import { useState } from "react";
import Post from "../post/Post"
import Share from "../article/Article"
import "./feed.css"
const posts = [
    {
        id: 1,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: "user1",
        date: "February 20, 2024",
        post: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        postimgs: [
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            // Add more image URLs as needed
        ],
        likes: 10,
        dislikes: 2,
        commentsCount: 5
    },
    {
        id: 2,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: "user2",
        date: "February 19, 2024",
        post: "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
        postimgs: [
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            // Add more image URLs as needed
        ],
        likes: 20,
        dislikes: 1,
        commentsCount: 8
    },
    // Add more posts as needed
];

export default function Feed() {
    // const [posts, setPosts] = useState(mockData.map(post => ({
    //     ...post,
    //     liked: false,
    //     disliked: false
    // })));
    // console.log(posts)
    // const [posts, setPosts] = useState(mockData.map(post => ({
    //     ...post,
    //     isLiked: false,
    //     isDisliked: false
    // })));
//     const handleLike = (postId) => {
//         setPosts(prevPosts => prevPosts.map(post => {
//             if (post.id === postId) {
//                 if (post.isLiked) {
//                     // Post is already liked, remove the like
//                     return {...post, likes: post.likes - 1, isLiked: false};
//                 } else {
//                     if(post.isDisliked){
//                         return {...post, likes: post.likes + 1, isLiked: true, dislikes: post.dislikes - 1, isDisliked: false};
//                     }
//                     // Post is not liked, like the post
//                     return {...post, likes: post.likes + 1, isLiked: true};
//                 }
//             }
//             return post;
//         }));
//     };
    

//     const handleDislike = (postId) => {
//     setPosts(prevPosts => prevPosts.map(post => {
//         if (post.id === postId) {
//             if (post.isDisliked) {
//                 // Post is already disliked, remove the dislike
//                 return {...post, dislikes: post.dislikes - 1, isDisliked: false};
//             } else {
//                 if(post.isLiked){
//                     return {...post, dislikes: post.dislikes + 1, isDisliked: true, likes:post.likes - 1, isLiked: false};
//                 }
//                 // Post is not disliked, dislike the post
//                 return {...post, dislikes: post.dislikes + 1, isDisliked: true};
//             }
//         }
//         return post;
//     }));
// };

    return (
        <div className="feed">
            <div className="feedWrapper">
                {posts.map(post => (
                    <Post
                        post = {post}
                        isLiked={post.isLiked}
                        isDisliked={post.isDisliked}
                        // handleLike={() => handleLike(post.id)}
                        // handleDislike={() => handleDislike(post.id)}
                    />
                ))}
            </div>
        </div>
    )
}
