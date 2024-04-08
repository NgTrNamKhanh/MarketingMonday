import React, { useEffect, useState } from 'react';
import { ChatBubbleOutline, MoreVert, RecommendRounded, ThumbDownAlt, ThumbDownOffAlt, ThumbUp, ThumbUpOffAlt } from "@mui/icons-material";
import "./post.css";
import apis from '../../services/apis.service';
import authHeader from '../../services/auth.header';
import { HubConnectionBuilder } from '@microsoft/signalr';
import PostModal from "../modal/postModal/PostModal"
export default function Post({ post, isProfile, currentUser}) {
    console.log(post)
    // const [message, setMessage] = useState()
    // const [connection, setConnection] = useState()
    // useEffect(() => {
    //     const connect = async ()=>{
    //         const connection = new HubConnectionBuilder()
    //         .withUrl(apis.normal+"notificationHub")
    //         .build();
    //         connection.on("ReceiveNotification", (message)=>{
    //             console.log(message)
    //             setMessage(message)
    //         })

    //         await connection.start();
    //         setConnection(connection)
    //     }
    //     connect()
    // }, []);

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

    

    const [likesCount, setLikesCount] = useState(post.likeCount)

    
    
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
    

    let pictureLayout;
    if (post.cloudImagePath == null){
        pictureLayout = null
    }
    else if (post.cloudImagePath.length === 1) {
        pictureLayout = (
            <div className="postCenter">
                {post.cloudImagePath.map((img, index) => (
                    <img src={img} key={index} className="postImg"  alt={`post image ${index}`}/>
                    // <img key={index} src={img} className="postImg" alt={`post image ${index}`} />
                ))}
            </div>
        );
    } else if (post.cloudImagePath.length === 2) {
        pictureLayout = (
            <div className="postImgGroup">
                <img src={post.cloudImagePath[0]} className="postImg postImgBottom" alt="Rendered Image"/>
                <img src={post.cloudImagePath[1]} className="postImg postImgBottom" alt="Rendered Image"/>
            </div>
        );
    } else if (post.cloudImagePath.length > 2){
        pictureLayout = (
            <div className="postCenter">
                <div className="postImgGroup">
                    <img src={post.cloudImagePath[0]}  alt="Rendered Image"/>
                    <img src={post.cloudImagePath[1]} alt="Rendered Image"/>
                </div>
                <div className="postImgGroup">
                    <img src={post.cloudImagePath[2]}className="postImg"  alt="Rendered Image"/>
                    {post.cloudImagePath.length > 3 && 
                        <div className="extraImg">
                            {post.cloudImagePath.slice(3,7).map((img, index) => (
                                <img src={img} key={index} className="postImg postImgBottom" alt={`post image ${index + 3}`}/>
                            ))}
                            <div className="overlay">+{post.cloudImagePath.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 
    const [isModalOpen, setIsModalOpen] = useState(false);

    // Open modal
    const openModal = () => {
        if(!post.isViewed){
            authHeader().post(apis.article+"countView/"+post.id)
        }
        setIsModalOpen(true);
    };

    // Close modal
    const closeModal = () => {
        setIsModalOpen(false);
    };
    
    
    
    

    
    
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
                        <img src={post.studentAvatar} className="postProfileImg" alt="profile" />
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
                {/* {pictureLayout} */}
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
                            <div className="postBottomRight" onClick={openModal}>
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
                            <div className="action" onClick={openModal}>
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
            {isModalOpen && 
                <PostModal 
                    post={post} 
                    closeModal={closeModal} 
                    currentUser={currentUser} 
                    commentsCount={commentsCount} 
                    setCommentsCount={setCommentsCount}
                    likesCount={likesCount}
                    dislikesCount={dislikesCount}
                    isLiked={isLiked}
                    isDisliked={isDisliked}
                    handleLike={handleLike}
                    handleDislike={handleDislike}
                    setIsModalOpen={setIsModalOpen}
                    formatDate={formatDate}
                />}
        </div>
    );
}
