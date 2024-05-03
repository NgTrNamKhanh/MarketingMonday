import { useEffect, useState } from "react";
import authHeader from "../../../services/auth.header";
import { ChatBubbleOutline, MoreVert, RecommendRounded, ThumbDownAlt, ThumbDownOffAlt, ThumbUp, ThumbUpOffAlt } from "@mui/icons-material";
import CommentForm from '../../commentForm/CommentForm';
import Comments from "../../comments/Comments"
import apis from "../../../services/apis.service";
export default function Modal({
    post, closeModal, currentUser, 
    commentCount, setCommentCount, viewCount,
    likeCount, dislikeCount,
    setIsModalOpen, isLiked, isDisliked, handleLike, handleDislike,
    formatDate, pictureLayout}){
    const [comments, setComments] = useState([]);
    const [loading, setLoading] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false); 
    useEffect(() => {
        const fetchData = async () => {
            if(currentUser){
                setLoading(true)
                console.log(currentUser.id)
                try {
                    const response = await authHeader().get(apis.comment + "getParentComments", { params: { articleId: post.id, userId: currentUser.id }});
                    setComments(response.data);
                    setLoading(false);
                } catch (error) {
                    console.error("Error fetching comments:", error);
                    setLoading(false);
                }
            }
        };

        fetchData();
    }, []);
    
    return(
        <div className="modal">
                <div className="modalWrapper">
                    <div className="modalStickyTop">
                        <div className="modalTop">
                            <div className="modalTopLeft">
                                <h1 className="postUsername">{post.studentName}'s post</h1>
                            </div>
                            <div className="modalTopRight">
                                <button className="closeButton" onClick={closeModal}>X</button>
                            </div>
                        </div>
                    </div>
                    <div className="modalMainContent">
                        <div className="modalTop">
                            <div className="modalTopLeft">
                                <img src={post.studentAvatar} className="postProfileImg" alt="profile" />
                                <span className="postUsername">{post.studentName}</span>
                                <span className="postDate">{formatDate(post.uploadDate)}</span>
                            </div>
                            <div className="postTopRight">
                            </div>
                        </div>
                        <div className="modalCenter">
                            <h2 className='postTitle'>{post.title}</h2>
                            <div className='postContent' dangerouslySetInnerHTML={{ __html: post.description }} />
                            {/* {post.files.map((file, index) => (
                                <div key={index} className="itemContainer">
                                    <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                                </div>
                            ))} */}
                        </div>
                        {pictureLayout}
                        <div className="modalBottom">
                            <div className="postBottomLeft">
                                <RecommendRounded className={`likeIcon postIcon`} />
                                <span className="postLikeCounter">{likeCount}</span>
                                <RecommendRounded className={`disLikeIcon postIcon`} />
                                <span className="postLikeCounter">{dislikeCount}</span>
                            </div>
                            <div className="postBottomRight">
                                <span className="postCommentText">{viewCount} views</span>
                                <span className="postCommentText">{commentCount} comments</span>
                            </div>
                        </div>
                        <hr className="postHr"/>
                        {currentUser.roles.includes("Student","Guest") && (
                            <div className="actionsSection">
                                <div className={`action ${isLiked ? 'liked' : ''}`} onClick={handleLike}>
                                    {isLiked ? <ThumbUp /> : <ThumbUpOffAlt />}
                                    <span className='actionText'>Like</span>
                                </div>
                                <div  className={`action ${isDisliked ? 'disliked ' : ''} `} onClick={handleDislike}>
                                {isDisliked ? <ThumbDownAlt /> : <ThumbDownOffAlt />}
                                    <span className='actionText'>Dislike</span>
                                </div>
                                <div className="action" onClick={()=>{setIsModalOpen(true)}}>
                                    <ChatBubbleOutline/>
                                    <span className='actionText'>Comment</span>
                                </div>
                            </div>
                        )}
                        <div className="commentsSection">
                            <h3>Comments</h3>
                            <Comments
                                comments={comments} 
                                loading={loading} 
                                currentUser={currentUser} 
                                post={post}
                                formatDate={formatDate}
                                setCommentCount={setCommentCount}
                            />
                        </div>
                    </div>
                    {currentUser.roles.includes("Student","Guest") && (
                        <div className="modalStickyBottom">
                                <CommentForm 
                                    currentUser={currentUser} 
                                    isSubmitting={isSubmitting} 
                                    setIsSubmitting={setIsSubmitting}
                                    setComments={setComments}
                                    setCommentCount={setCommentCount}
                                    post={post}
                                />
                        </div>
                    )}
                </div>
        </div>
    )
}