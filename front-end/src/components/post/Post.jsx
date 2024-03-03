import React, { useState } from 'react';
import { ModeComment, MoreVert, ThumbDown, ThumbUp } from "@mui/icons-material";
import "./post.css";

export default function Post({ post}) {
    const [like, setLike] = useState(post.likes)
    const [isLiked, setIsLiked] = useState(false)
    const handleLike = () => {
        setLike(isLiked? like -1 : like+1)
        setIsLiked(!isLiked)
        if(isDisliked){
            handleDislike()
        }
    }
    const [dislike, setDisLike] = useState(post.dislikes)
    const [isDisliked, setIsDisLiked] = useState(false)
    const handleDislike = () => {
        setDisLike(isDisliked? dislike -1 : dislike+1)
        setIsDisLiked(!isDisliked)
        if(isLiked){
            handleLike()
        }
    } 

    // Determine the layout of pictures based on their count
    let pictureLayout;
    if (post.postimgs.length === 1) {
        pictureLayout = (
            <div className="postCenter">
                {post.postimgs.map((img, index) => (
                    <img key={index} src={img} className="postImg" alt={`Post image ${index}`} />
                ))}
            </div>
        );
    } else if (post.postimgs.length === 2) {
        pictureLayout = (
            <div className="postImgGroup">
                <img src={post.postimgs[0]} className="postImg postImgBottom" alt="Post image 1" />
                <img src={post.postimgs[1]} className="postImg postImgBottom" alt="Post image 2" />
            </div>
        );
    } else{
        pictureLayout = (
            <div className="postCenter">
                <div className="postImgGroup">
                    <img src={post.postimgs[0]} className="postImg postImgBottom" alt="Post image 1" />
                    <img src={post.postimgs[1]} className="postImg postImgBottom" alt="Post image 2" />
                </div>
                <div className="postImgGroup">
                    <img src={post.postimgs[2]} className="postImg postImgBottom" alt={`Post image 3`} />
                    {post.postimgs.length > 3 && 
                        <div className="extraImg">
                            {post.postimgs.slice(3,7).map((img, index) => (
                                <img key={index} src={img} className="postImg postImgBottom" alt={`Post image ${index + 3}`} />
                            ))}
                            <div className="overlay">+{post.postimgs.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 

    return (
        <div className="post">
            <div className="postWrapper">
                <div className="postTop">
                    <div className="postTopLeft">
                        <img src={post.img} className="postProfileImg" alt="profile" />
                        <span className="postUsername">{post.username}</span>
                        <span className="postDate">{post.date}</span>
                    </div>
                    <div className="postTopRight">
                        <MoreVert/>
                    </div>
                </div>
                <div className="postCenter">
                    <h2 className='postTitle'>{post.title}</h2>
                    <p className='postContent'>{post.content}</p>
                    {post.files.map((file, index) => (
                        <div key={index} className="itemContainer">
                            <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                        </div>
                    ))}
                </div>
                {pictureLayout}
                <div className="postBottom">
                    <div className="postBottomLeft">
                        <ThumbUp className={`postIcon ${isLiked ? 'liked' : ''}`} onClick={handleLike}/>
                        <span className="postLikeCounter">{like}</span>
                        <ThumbDown className={`postIcon ${isDisliked ? 'disliked' : ''}`} onClick={handleDislike}/>
                        <span className="postLikeCounter">{dislike}</span>
                    </div>
                    <div className="postBottomRight">
                        <ModeComment className='postIcon'/>
                        <span className="postCommentText">{post.commentsCount}</span>
                    </div>
                </div>
            </div>
        </div>
    );
}
