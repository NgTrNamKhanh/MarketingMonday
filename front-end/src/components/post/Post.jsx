import React, { useState } from 'react';
import { ModeComment, MoreVert, ThumbDown, ThumbUp } from "@mui/icons-material";
import "./post.css";
const commentsData = [
    {
        id: 1,
        articalId: 1,
        accountId: 1,
        userImg: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: 'User 1',
        parentComment: null,
        commentDate: "February 20, 2024",
        content: "This sucks",
        hasReplies: true,
        likes: 10,
        dislikes: 2,
    },
    {
        id: 2,
        articalId: 1,
        accountId: 1,
        userImg: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: 'User 1',
        parentComment: 1,
        commentDate: "February 20, 2024",
        content: "This sucks",
        hasReplies: true,
        likes: 10,
        dislikes: 2,
    },
    {
        id: 3,
        articalId: 1,
        accountId: 1,
        userImg: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: 'User 1',
        parentComment: 2,
        commentDate: "February 20, 2024",
        content: "This sucks",
        hasReplies: false,
        likes: 10,
        dislikes: 2,
    },
    {
        id: 4,
        articalId: 1,
        accountId: 1,
        userImg: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: 'User 1',
        parentComment: null,
        commentDate: "February 20, 2024",
        content: "This sucks",
        hasReplies: false,
        likes: 10,
        dislikes: 2,
    },

];

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
    
    const RenderComments = ({ comments }) => { 
        const [showReplies, setShowReplies] = useState(false);
        const [commentLike, setCommentLike] = useState(post.likes)
        const [isCommentLiked, setIsCommentLiked] = useState(false)
        const handleCommentLike = (comment) => {
            setCommentLike(isCommentLiked? comment.likes -1 : comment.likes+1)
            setIsCommentLiked(!isCommentLiked)
            if(isCommentDisliked){
                handleCommnetDislike()
            }
        }
        const [commentDislike, setCommentDisLike] = useState(post.dislikes)
        const [isCommentDisliked, setIsCommnetDisLiked] = useState(false)
        const handleCommnetDislike = (comment) => {
            setCommentDisLike(isCommentDisliked? comment.dislikes -1 : comment.dislikes+1)
            setIsCommnetDisLiked(!isCommentDisliked)
            if(isCommentLiked){
                handleCommentLike()
            }
        } 
        const toggleReplies = () => {
            setShowReplies(!showReplies);
        };
        if (comments === undefined || !Array.isArray(comments)) { 
            return (<p style={{ color: 'red' }}>There is no comment</p>);
        } else {
            return comments.map((comment) => (
                <div key={comment.id} className="comment" style={{ backgroundColor: 'lightgray' }}>
                    <img src={comment.userImg} className="commentProfileImg" alt="profile" />
                    <div className="commentTop">
                        <div className="commentTopLeft">
                            <span className="commentUsername">{comment.username}</span>
                            <span className="commentDate">{comment.commentDate}</span>
                        </div>
                    </div>
                    <p className="commentContent">{comment.content}</p>
                    <div className="commentActions">
                        <ThumbUp className={`commentIcon ${isCommentLiked ? 'liked' : ''}`} onClick={() => handleCommentLike(comment)} />
                        <span>{comment.likes}</span>
                        <ThumbDown className={`commentIcon ${isCommentDisliked ? 'disliked' : ''}`} onClick={() => handleCommnetDislike(comment)} />
                        <span>{comment.dislikes}</span>
                    </div>
                    {comment.hasReplies && (
                        <span onClick={toggleReplies} style={{ cursor: 'pointer' }} className="viewReplies ">
                            {showReplies ? 'Hide Replies' : 'View Replies'}
                        </span>
                    )}
                    <div className="commentReply">
                    {showReplies && comment.hasReplies && (
                        <RenderComments comments={commentsData.filter((reply) => reply.articalId === post.id && reply.parentComment === comment.id)} />
                    )}
                    </div>
                </div>
            ));
        }
    };
   
    
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
                <div className="commentsSection">
                    <h3>Comments</h3>
                    <RenderComments comments={commentsData.filter((comment) => comment.articalId === post.id && comment.parentComment === null)} />
                </div>
            </div>
        </div>
    );
}
