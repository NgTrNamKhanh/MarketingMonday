import React, { useEffect, useState } from 'react';
import { ChatBubbleOutline, MoreVert, RecommendRounded, ThumbDown, ThumbDownAlt, ThumbDownOffAlt, ThumbUp, ThumbUpOffAlt } from "@mui/icons-material";
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
        content: "Boooo",
        hasReplies: true,
        likes: 11,
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
        content: "Ewww",
        hasReplies: false,
        likes: 10,
        dislikes: 11,
    },
    {
        id: 4,
        articalId: 1,
        accountId: 1,
        userImg: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: 'User 1',
        parentComment: null,
        commentDate: "February 20, 2024",
        content: "?????",
        hasReplies: false,
        likes: 10,
        dislikes: 3,
    },

];

export default function Post({ post}) {
    const [like, setLike] = useState(post.likes)
    useEffect(() => {
        setLike(post.likes);
    }, [post.likes]);
    const [isLiked, setIsLiked] = useState(false)
    const handleLike = () => {
        setLike(isLiked? like -1 : like+1)
        setIsLiked(!isLiked)
        if(isDisliked){
            handleDislike()
        }
    }
    const [dislike, setDisLike] = useState(post.dislikes)
    useEffect(() => {
        setDisLike(post.dislikes); // Update like count when post prop changes
    }, [post.likes]);
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
    if (post.imageBytes.length === 1) {
        pictureLayout = (
            <div className="postCenter">
                {post.imageBytes.map((img, index) => (
                    <img key={index} src={img} className="postImg" alt={`Post image ${index}`} />
                ))}
            </div>
        );
    } else if (post.imageBytes.length === 2) {
        pictureLayout = (
            <div className="postImgGroup">
                <img src={post.imageBytes[0]} className="postImg postImgBottom" alt="Post image 1" />
                <img src={post.imageBytes[1]} className="postImg postImgBottom" alt="Post image 2" />
            </div>
        );
    } else{
        pictureLayout = (
            <div className="postCenter">
                <div className="postImgGroup">
                    <img src={post.imageBytes[0]} className="postImg postImgBottom" alt="Post image 1" />
                    <img src={post.imageBytes[1]} className="postImg postImgBottom" alt="Post image 2" />
                </div>
                <div className="postImgGroup">
                    <img src={post.imageBytes[2]} className="postImg postImgBottom" alt={`Post image 3`} />
                    {post.imageBytes.length > 3 && 
                        <div className="extraImg">
                            {post.imageBytes.slice(3,7).map((img, index) => (
                                <img key={index} src={img} className="postImg postImgBottom" alt={`Post image ${index + 3}`} />
                            ))}
                            <div className="overlay">+{post.imageBytes.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 
    const [showComment, setShowComment] = useState(false);

    const CommentBlock = ({comment}) => {
        console.log(comment.userImg)
        const [showReplies, setShowReplies] = useState(false);
        const [commentLikes, setCommentLikes] = useState(comment.likes)
        const [isCommentLiked, setIsCommentLiked] = useState(false)
        const handleCommentLike = () => {
            setCommentLikes(isCommentLiked? commentLikes -1 : commentLikes+1)
            setIsCommentLiked(!isCommentLiked)
            if(isCommentDisliked){
                handleCommnetDislike()
            }
        }
        const [commentDislikes, setCommentDisLikes] = useState(comment.dislikes)
        const [isCommentDisliked, setIsCommnetDisLiked] = useState(false)
        const handleCommnetDislike = () => {
            setCommentDisLikes(isCommentDisliked? commentDislikes -1 : commentDislikes+1)
            setIsCommnetDisLiked(!isCommentDisliked)
            if(isCommentLiked){
                handleCommentLike()
            }
        } 
        const toggleReplies = () => {
            setShowReplies(!showReplies);
        };
        return (
        <div key={comment.id} className="comment" >
            <img src={comment.userImg} className="commentProfileImg" alt="profile" />
            <div className="commentTop">
                <div className="commentTopLeft">
                    <span className="commentUsername">{comment.username}</span>
                    <span className="commentDate">{comment.commentDate}</span>
                </div>
            </div>
            <p className="commentContent">{comment.content}</p>
            <div className="commentActions">
                <ThumbUp className={`commentIcon ${isCommentLiked ? 'liked' : ''}`} onClick={() => handleCommentLike()} />
                <span>{commentLikes}</span>
                <ThumbDown className={`commentIcon ${isCommentDisliked ? 'disliked' : ''}`} onClick={() => handleCommnetDislike()} />

                <span>{commentDislikes}</span>
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
        )
    }
    const RenderComments = ({ comments }) => { 
        
        if (comments === undefined || !Array.isArray(comments) || comments.length==0) { 
            return (<p >There is no comment</p>);
        } else {
            return comments.map((comment) => (
                <CommentBlock comment={comment}/>
            ));
        }
    };
    
    return (
        <div className="post">
            <div className="postWrapper">
                <div className="postTop">
                    <div className="postTopLeft">
                        <img src={post.studentId} className="postProfileImg" alt="profile" />
                        <span className="postUsername">{post.studentName}</span>
                        <span className="postDate">{new Date(post.date).toLocaleDateString()} {new Date(post.date).toLocaleTimeString()}</span>
                    </div>
                    <div className="postTopRight">
                        <MoreVert/>
                    </div>
                </div>
                <div className="postCenter">
                    <h2 className='postTitle'>{post.title}</h2>
                    <p className='postContent'>{post.description}</p>
                    {/* {post.files.map((file, index) => (
                        <div key={index} className="itemContainer">
                            <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                        </div>
                    ))} */}
                </div>
                {pictureLayout}
                <div className="postBottom">
                    <div className="postBottomLeft">
                        <RecommendRounded className={`likeIcon postIcon`} />
                        <span className="postLikeCounter">{like}</span>
                        <RecommendRounded className={`disLikeIcon postIcon`} />
                        <span className="postLikeCounter">{dislike}</span>
                    </div>
                    <div className="postBottomRight" onClick={()=>{setShowComment(!showComment)}}>
                        <span className="postCommentText">{post.commentsCount} comments</span>
                    </div>
                </div>
                <hr className="postHr"/>
                <div className="actionsSection">
                    <div className={`action ${isLiked ? 'liked' : ''}`} onClick={handleLike}>
                        {isLiked ? <ThumbUp /> : <ThumbUpOffAlt />}
                        <span className='actionText'>Like</span>
                    </div>
                    <div  className={`action ${isDisliked ? 'disliked ' : ''} `} onClick={handleDislike}>
                    {isDisliked ? <ThumbDownAlt /> : <ThumbDownOffAlt />}
                        <span className='actionText'>Dislike</span>
                    </div>
                    <div className="action" onClick={()=>{setShowComment(!showComment)}}>
                        <ChatBubbleOutline/>
                        <span className='actionText'>Comment</span>
                    </div>
                </div>
                {showComment &&(
                    <div className="commentsSection">
                    <h3>Comments</h3>
                    <RenderComments comments={commentsData.filter((comment) => comment.articalId === post.id && comment.parentComment === null)} />
                </div>
                )}
            </div>
        </div>
    );
}
