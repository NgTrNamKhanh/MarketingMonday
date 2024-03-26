import React, { useEffect, useRef, useState } from 'react';
import { ChatBubbleOutline, MoreVert, RecommendRounded, Send, ThumbDown, ThumbDownAlt, ThumbDownOffAlt, ThumbUp, ThumbUpOffAlt } from "@mui/icons-material";
import "./post.css";
import useFetch from '../../hooks/useFetch';
import apis from '../../services/apis.service';
import {  ScaleLoader } from 'react-spinners';
import authHeader from '../../services/auth.header';
import authService from '../../services/auth.service';
import DeleteConfirm from '../dialogs/deleteConfirm/DeleteConfirm';
import EditComment from '../dialogs/editCmt/EditComment';

export default function Post({ post, isProfile, currentUser}) {
    const formatDate = (dateString) => {
        const options = {
            year: 'numeric',
            month: 'numeric',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            second: 'numeric',
            hour12: true, 
        };
    
        return new Date(dateString).toLocaleString(undefined, options);
    };
    const [comments, setComments] = useState([]);
    const [loading, setLoading] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false); 
    const [optionsOpen, setOptionsOpen] = useState(false);
    const [likesCount, setLikesCount] = useState(post.likeCount)
    const [editCmtOpen, setEditCmtCOpen] = useState(false);
    const [selectedComment, setSelectedComment] = useState();
    const handleCloseEditCmtCDialog = () => {
        setEditCmtCOpen(false);
    };
    const handleOpenEditCmtCDialog = (e) => {
        setSelectedComment(e)
        setEditCmtCOpen(true);
    };
    const [deleteCmtOpen, setDeleteCmtCOpen] = useState(false);
    const handleCloseDeleteCmtCDialog = () => {
        setDeleteCmtCOpen(false);
    };
    const handleOpenDeleteCmtCDialog = (e) => {
        setSelectedComment(e)
        setDeleteCmtCOpen(true);
    };
    const [reportCmtOpen, setReportCmtCOpen] = useState(false);
    const handleCloseReportCmtCDialog = () => {
        setReportCmtCOpen(false);
    };
    const handleOpenReportCmtCDialog = () => {
        setSelectedComment()
        setReportCmtCOpen(true);
    };
    const optionsRef = useRef(null);
    useEffect(() => {
        function handleClickOutside(event) {
            if (optionsRef.current && !optionsRef.current.contains(event.target)) {
                setOptionsOpen(false);
            }
        }
    
        function handleScrollOutside(event) {
            if (optionsRef.current && !optionsRef.current.contains(event.target)) {
                setOptionsOpen(false);
            }
        }
    
        document.addEventListener("mousedown", handleClickOutside);
        document.addEventListener("scroll", handleScrollOutside);
    
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
            document.removeEventListener("scroll", handleScrollOutside);
        };
    }, []);
    useEffect(() => {
        setLikesCount(post.likeCount);
    }, [post.likeCount]);

    const [isLiked, setIsLiked] = useState(post.isLiked)
    const handleLike =  () => {
        setLikesCount(isLiked? likesCount -1 : likesCount+1)
        setIsLiked(!isLiked)
        try{
                const like ={
                    userId : currentUser.id,
                    articleId: post.id
                }
                authHeader().post(apis.like+"article", like);
        }catch(err){
        }
        if(isDisliked){
            handleDislike()
        }
    }
    const [commentsCount, setCommentsCount] = useState(post.commmentCount)
    useEffect(() => {
        setCommentsCount(post.commmentCount);
    }, [post.commmentCount]);
    const [dislikesCount, setDisLikesCount] = useState(post.dislikeCount)
    useEffect(() => {
        setDisLikesCount(post.dislikeCount); 
    }, [post.dislikeCount]);
    const [isDisliked, setIsDisLiked] = useState(post.isDisliked)
    const handleDislike =  () => {
        setDisLikesCount(isDisliked? dislikesCount -1 : dislikesCount+1)
        setIsDisLiked(!isDisliked)
        try{
            const dislike ={
                userId : currentUser.id,
                articleId: post.id
            }
            authHeader().post(apis.dislike+"article", dislike);
            }catch(err){
            }
        if(isLiked){
            handleLike()
        }
    } 
    const handleDeleteCmt = () =>{

    }

    let pictureLayout;
    if (post.imageBytes.length === 1) {
        pictureLayout = (
            <div className="postCenter">
                {post.imageBytes.map((img, index) => (
                    <img src={`data:image/jpeg;base64,${img}`} key={index} className="postImg"  alt={`post image ${index}`}/>
                    // <img key={index} src={img} className="postImg" alt={`post image ${index}`} />
                ))}
            </div>
        );
    } else if (post.imageBytes.length === 2) {
        pictureLayout = (
            <div className="postImgGroup">
                <img src={`data:image/jpeg;base64,${post.imageBytes[0]}`} className="postImg postImgBottom" alt="Rendered Image"/>
                <img src={`data:image/jpeg;base64,${post.imageBytes[1]}`} className="postImg postImgBottom" alt="Rendered Image"/>
            </div>
        );
    } else{
        pictureLayout = (
            <div className="postCenter">
                <div className="postImgGroup">
                    <img src={`data:image/jpeg;base64,${post.imageBytes[0]}`}  alt="Rendered Image"/>
                    <img src={`data:image/jpeg;base64,${post.imageBytes[1]}`} alt="Rendered Image"/>
                </div>
                <div className="postImgGroup">
                    <img src={`data:image/jpeg;base64,${post.imageBytes[2]}`}className="postImg"  alt="Rendered Image"/>
                    {post.imageBytes.length > 3 && 
                        <div className="extraImg">
                            {post.imageBytes.slice(3,7).map((img, index) => (
                                <img src={`data:image/jpeg;base64,${img}`} key={index} className="postImg postImgBottom" alt={`post image ${index + 3}`}/>
                            ))}
                            <div className="overlay">+{post.imageBytes.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 
    const [isModalOpen, setIsModalOpen] = useState(false);

    // Open modal
    const openModal = () => {
        setIsModalOpen(true);
    };

    // Close modal
    const closeModal = () => {
        setIsModalOpen(false);
    };
    const CommentBlock = ({comment}) => {
        const [replies, setReplies] = useState([]);
        const [repLoading, setRepLoading] = useState(false);
        useEffect(() => {
            const fetchData = async () => {
                setRepLoading(true)
                try {
                    const response = await authHeader().get(apis.comment + "getReplies", { params: { parentId: comment.id, userId: currentUser.id }});
                    setReplies(response.data);
                    setRepLoading(false);
                } catch (error) {
                    console.error("Error fetching comments:", error);
                    setRepLoading(false);
                }
            };

            fetchData();
        }, [comment.id]);

        const [showReplies, setShowReplies] = useState(false);
        const [commentLikesCount, setCommentLikesCount] = useState(comment.likesCount)
        const [isCommentLiked, setIsCommentLiked] = useState(comment.isLiked)
        const handleCommentLike = () => {
            setCommentLikesCount(isCommentLiked? commentLikesCount -1 : commentLikesCount+1)
            setIsCommentLiked(!isCommentLiked)
            try{
                const like ={
                    userId : currentUser.id,
                    commentId: comment.id
                }
                authHeader().post(apis.like+"comment", like);
            }catch(err){
            }
            if(isCommentDisliked){
                handleCommnetDislike()
            }
        }
        const [commentDislikesCount, setCommentDisLikesCount] = useState(comment.dislikesCount)
        const [isCommentDisliked, setIsCommnetDisLiked] = useState(comment.isDisliked)
        const handleCommnetDislike = () => {
            setCommentDisLikesCount(isCommentDisliked? commentDislikesCount -1 : commentDislikesCount+1)
            setIsCommnetDisLiked(!isCommentDisliked)
            try{
                const dislike ={
                    userId : currentUser.id,
                    commentId: comment.id
                }
                authHeader().post(apis.dislike+"comment", dislike);
            }catch(err){
            }
            if(isCommentLiked){
                handleCommentLike()
            }
        } 
        const toggleReplies = () => {
            setShowReplies(!showReplies);
        };
        const [openCommentInput, setOpenCommentInput] = useState(false);
        const handleReply =  async (event) =>{
            event.preventDefault();
            setIsSubmitting(true)
            try {
                const reply = {
                    content: event.target.comment.value,
                    userId: currentUser.id,
                    articleId: post.id
                }
                setIsSubmitting(true);
                const res = await authHeader().post(apis.comment+"createReply", reply, {params:{parentCommentId:comment.id }});
                if (res.status === 200) {
                    setReplies(prevComments => [...prevComments, res.data]);
                    // localStorage.setItem("accounts", JSON.stringify(updatedData));
                    setIsSubmitting(false);
                    // setMessage("Account edited successfully.");
                } else {
                    setIsSubmitting(false);
                    // setMessage(`An error occurred: ${res.data}`);
                }
            } catch (error) {
                setIsSubmitting(false);
                // setMessage(error.response.data);
            }
    
        }
        console.log(comment);
        return (
            <div key={comment.id} className="comment" >
                <img src={`data:image/jpeg;base64,${comment.userComment.userAvatar}`} className="commentProfileImg" alt="profile" />
                <div className="commentTop">
                    <div className="commentTopLeft">
                        <span className="commentUsername">{comment.userComment.userName}{comment.userComment.lastName}</span>
                        <span className="commentDate">{formatDate(comment.createOn)}</span>
                    </div>
                </div>
                <p className="commentContent">{comment.content}</p>
                <div className="commentActions">
                    <ThumbUp 
                    className={`commentIcon ${isCommentLiked ? 'liked' : ''}`} 
                    onClick={currentUser.roles.includes("Student") || currentUser.roles.includes("Guest") ? handleCommentLike : null} 
                    />
                    <span>{commentLikesCount}</span>
                    <ThumbDown 
                        className={`commentIcon ${isCommentDisliked ? 'disliked' : ''}`} 
                        onClick={currentUser.roles.includes("Student") || currentUser.roles.includes("Guest") ? handleCommnetDislike : null} 
                    />
                    <span>{commentDislikesCount}</span>
                    {currentUser.roles.includes("Student","Guest") && (
                        <span className='replyClick' onClick={()=>setOpenCommentInput(!openCommentInput)}>Reply</span>
                    )}
                </div>
                {comment.hasReplies && (
                    <span onClick={toggleReplies} style={{ cursor: 'pointer' }} className="viewReplies ">
                        {showReplies ? 'Hide Replies' : 'View Replies'}
                    </span>
                )}
                <div className="commentReply">
                    {showReplies && comment.hasReplies && (
                        <RenderComments comments= {replies} loading={repLoading}/>
                    )}
                    {openCommentInput && (
                    <CommentForm handleComment={handleReply} />
                    )}
                </div>
                <div className="commentTopRight">
                    <MoreVert className='moreIcon' onClick={()=>setOptionsOpen(!optionsOpen)}/>
                    {optionsOpen && (
                        <div className="commentDropdownContent" ref={optionsRef}>
                            <div className="commentDropdownContentItem" onClick={()=>handleCloseReportCmtCDialog(1)}>
                                        <span>Report</span> 
                            </div>
                            {currentUser.id === comment.userComment.userId && (
                                <>
                                    <div className="commentDropdownContentItem" onClick={()=>handleOpenEditCmtCDialog(comment)}>
                                            <span>Edit</span>
                                    </div>
                                    <div className="commentDropdownContentItem" onClick={()=>handleOpenDeleteCmtCDialog()}>
                                                <span>Delete</span>
                                    </div>
                                </>
                            )}
                        </div>
                    )}
                    </div>
                
            </div>
        )
    }
    const RenderComments = ({ comments, loading}) => {
        if (loading) {
            return (<ScaleLoader/>);
        }
        if (!comments.length) {
            return <p>There are no comments.</p>;
        }
        return (
            <div>
                {comments.map(comment => (
                    <CommentBlock key={comment.id} comment={comment} />
                ))}
            </div>
        );
    };
    
    useEffect(() => {
        const fetchData = async () => {
            if(currentUser){
                console.log("re render")
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
    }, [post.id, currentUser]);

    const Modal = ({post, closeModal}) =>{
        const handleComment =  async (event) =>{
            event.preventDefault();
            setIsSubmitting(true);
            try {
                const comment = {
                    content: event.target.comment.value,
                    userId: currentUser.id,
                    articleId: post.id
                }
                const res = await authHeader().post(apis.comment+"createComment", comment);
                if (res.status === 200) {
                    console.log(res)
                    setComments(prevComments => [...prevComments, res.data]);
                    setCommentsCount(prevCount => prevCount + 1)
                    // localStorage.setItem("accounts", JSON.stringify(updatedData));
                    setIsSubmitting(false);
                    console.log("6")
                    // setMessage("Account edited successfully.");
                } else {
                    setIsSubmitting(false);
                    // setMessage(`An error occurred: ${res.data}`);
                }
            } catch (error) {
                setIsSubmitting(false);
                // setMessage(error.response.data);
            }
    
        }
        
        
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
                                    <img src={`data:image/jpeg;base64,${post.studentAvatar}`} className="postProfileImg" alt="profile" />
                                    <span className="postUsername">{post.studentName}</span>
                                    <span className="postDate">{formatDate(post.uploadDate)}</span>
                                </div>
                                <div className="postTopRight">
                                    <MoreVert/>
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
                                    <span className="postLikeCounter">{likesCount}</span>
                                    <RecommendRounded className={`disLikeIcon postIcon`} />
                                    <span className="postLikeCounter">{dislikesCount}</span>
                                </div>
                                <div className="postBottomRight" onClick={()=>{setIsModalOpen(true)}}>
                                    <span className="postCommentText">{post.commmentCount} comments</span>
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
                                <RenderComments comments={comments} loading={loading}/>
                            </div>
                        </div>
                        {currentUser.roles.includes("Student","Guest") && (
                            <div className="modalStickyBottom">
                                    <CommentForm handleComment={handleComment}/>
                            </div>
                        )}
                    </div>
            </div>
        )
    }
    const CommentForm = ({handleComment}) => {
        const [commentValue, setCommentValue] = useState('');
        const handleCommentChange = (event) => {
            setCommentValue(event.target.value);
        };
        return (
            <div className="commentInputWrapper">
                <img src={`data:image/jpeg;base64,${currentUser.avatar}`} className="userAvatar" alt="profile" />
                <form className='commentForm' onSubmit={handleComment}>
                    <input 
                        type="text"
                        name="comment"
                        value={commentValue}
                        onChange={handleCommentChange}
                        placeholder="Write a comment..."
                        className="commentInput"
                        disabled={isSubmitting} 
                    />
                        <button 
                            disabled={isSubmitting || !commentValue.trim()} 
                            type="submit" 
                            className="commentButton"
                        >
                                <Send/>
                        </button>
                </form>
            </div>

        )
    }
    function getStatusColor(status) {
        switch (status) {
            case 'approved':
                return 'green';
            case 'reject':
                return 'red';
            case 'commented':
                return 'yellow';
            default:
                return 'orange';
        }
    }
    const [coordinatorComment, setCoordinatorComment] = useState(post.coordinatorComment);
    const [status, setStatus] = useState('');
    useEffect(() => {
        if (post.publishStatusId === 1) {
            setStatus('approved');
        }  else if (post.publishStatusId === 2) {
            setStatus('reject');
        }else if (post.publishStatusId === 3 && post.coordinatorComment === null) {
            setStatus('not commented');
        } 
        else if (post.coordinatorComment) {
            setStatus('commented');
        }
        
    }, [post.coordinatorComment, post.publishStatusId]);
    return (
        <div className="post">
            <div className="postWrapper">
                <div className="postTop">
                    <div className="postTopLeft">
                        <img src={`data:image/jpeg;base64,${post.studentAvatar}`} className="postProfileImg" alt="profile" />
                        <span className="postUsername">{post.studentName}</span>
                        <span className="postDate">{formatDate(post.uploadDate)}</span>
                        {!isProfile &&  (
                            (<span style={{ color: getStatusColor(status) }}>
                            {status}
                            </span>)
                            )
                        }
                        
                    </div>
                    <div className="postTopRight">
                        <MoreVert/>
                    </div>
                </div>
                <div className="postCenter">
                    <h2 className='postTitle'>{post.title}</h2>
                    <div className='postContent' dangerouslySetInnerHTML={{ __html: post.description }} />
                    {/* <p className='postContent'>{post.description}</p> */}
                    {/* {post.files.map((file, index) => (
                        <div key={index} className="itemContainer">
                            <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                        </div>
                    ))} */}
                </div>
                {pictureLayout}
                {(status === 'approved' || !isProfile) && 
                    (
                        <>
                        <div className="postBottom">
                            <div className="postBottomLeft">
                                <RecommendRounded className={`likeIcon postIcon`} />
                                <span className="postLikeCounter">{likesCount}</span>
                                <RecommendRounded className={`disLikeIcon postIcon`} />
                                <span className="postLikeCounter">{dislikesCount}</span>
                            </div>
                            <div className="postBottomRight" onClick={()=>{setIsModalOpen(true)}}>
                                <span className="postCommentText">{commentsCount} comments</span>
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
                        </>
                    )
                }
                {coordinatorComment && (
                    <div className="commentSection" id='comment'>
                        <span>Coordinator Comment: </span>
                        <textarea 
                            value={coordinatorComment}
                            disabled={true}
                        />
                    </div>
                )}
            </div>
            {isModalOpen && <Modal post={post} closeModal={closeModal} />}
            <DeleteConfirm
                open={deleteCmtOpen}
                handleClose={handleCloseDeleteCmtCDialog}
                handleConfirm={handleDeleteCmt}
            />
            <EditComment
                open={editCmtOpen}
                handleClose={handleCloseEditCmtCDialog}
                comment = {selectedComment}
            />
        </div>
    );
}
