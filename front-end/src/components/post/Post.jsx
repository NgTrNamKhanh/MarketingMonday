import React, { useEffect, useState } from 'react';
import { ChatBubbleOutline, MoreVert, RecommendRounded, ThumbDownAlt, ThumbDownOffAlt, ThumbUp, ThumbUpOffAlt } from "@mui/icons-material";
import "./post.css";
import apis from '../../services/apis.service';
import authHeader from '../../services/auth.header';
import { HubConnectionBuilder } from '@microsoft/signalr';
import PostModal from "../modal/postModal/PostModal"
import { Link } from 'react-router-dom';
export default function Post({ post, isProfile, currentUser}) {
    console.log(post)
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
    const [likeCount, setLikeCount] = useState(post.likeCount)

    useEffect(() => {
        setLikeCount(post.likeCount);
    }, [post.likeCount]);

    const [isLiked, setIsLiked] = useState(post.isLiked)
    const handleLike =  () => {
        setLikeCount(isLiked? likeCount -1 : likeCount+1)
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
    const [commentCount, setCommentCount] = useState(post.commmentCount)
    useEffect(() => {
        setCommentCount(post.commmentCount);
    }, [post.commmentCount]);

    const [viewCount, setViewCount] = useState(post.viewCount)
    useEffect(() => {
        setViewCount(post.viewCount);
    }, [post.viewCount]);


    const [dislikeCount, setDisLikeCount] = useState(post.dislikeCount)
    useEffect(() => {
        setDisLikeCount(post.dislikeCount); 
    }, [post.dislikeCount]);
    const [isDisliked, setIsDisLiked] = useState(post.isDisliked)
    const handleDislike =  () => {
        setDisLikeCount(isDisliked? dislikeCount -1 : dislikeCount+1)
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
    if (post.listCloudImagePath == null){
        pictureLayout = null
    }
    else if (post.listCloudImagePath.length === 1) {
        pictureLayout = (
            <div className="postCenter">
                {post.listCloudImagePath.map((img, index) => (
                    <img src={img} key={index} className="postImg"  alt={`post image ${index}`}/>
                    // <img key={index} src={img} className="postImg" alt={`post image ${index}`} />
                ))}
            </div>
        );
    } else if (post.listCloudImagePath.length === 2) {
        pictureLayout = (
            <div className="postImgGroup">
                <img src={post.listCloudImagePath[0]} className="postImg postImgBottom" alt="Rendered Image"/>
                <img src={post.listCloudImagePath[1]} className="postImg postImgBottom" alt="Rendered Image"/>
            </div>
        );
    } else if (post.listCloudImagePath.length > 2){
        pictureLayout = (
            <div className="postCenter">
                <div className="postImgGroup">
                    <img src={post.listCloudImagePath[0]}  alt="Rendered Image"/>
                    <img src={post.listCloudImagePath[1]} alt="Rendered Image"/>
                </div>
                <div className="postImgGroup">
                    <img src={post.listCloudImagePath[2]}className="postImg"  alt="Rendered Image"/>
                    {post.listCloudImagePath.length > 3 && 
                        <div className="extraImg">
                            {post.listCloudImagePath.slice(3,7).map((img, index) => (
                                <img src={img} key={index} className="postImg postImgBottom" alt={`post image ${index + 3}`}/>
                            ))}
                            <div className="overlay">+{post.listCloudImagePath.length - 3}</div>
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
            setViewCount(viewCount+1)
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
                return '#a89132';
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
                        <Link
                            to={`/account/${post.studentId}`}
                        >
                            <img src={post.studentAvatar} className="postProfileImg" alt="profile" />
                        </Link>
                        <Link
                            to={`/account/${post.studentId}`}
                        >
                            <span className="postUsername">{post.studentName}</span>
                        </Link>
                        <span className="postDate">{formatDate(post.uploadDate)}</span>
                        <span className="submissionDate">
                            {status&&(
                                <>
                                 (
                                    <span style={{ color: getStatusColor(status) }}>
                                        {status}
                                    </span>
                                )
                                </>
                            )}
                        
                    </span>
                        {!isProfile &&  (
                            (<span style={{ color: getStatusColor(status) }}>
                            {status}
                            </span>)
                            )
                        }
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
                <div className="postFile">
                        <div className="postFileContainer">
                            <a href={post.cloudDocPath} className="postFileLink">File</a>
                        </div>
                </div>
                {(status === 'approved' || !isProfile) && 
                    (
                        <>
                        <div className="postBottom">
                            <div className="postBottomLeft">
                                <RecommendRounded className={`likeIcon postIcon`} />
                                <span className="postLikeCounter">{likeCount}</span>
                                <RecommendRounded className={`disLikeIcon postIcon`} />
                                <span className="postLikeCounter">{dislikeCount}</span>
                            </div>
                            <div className="postBottomRight" >
                                <span className="postCommentText">{viewCount} views</span>
                                <span className="postCommentText" onClick={openModal}>{commentCount} comments</span>
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
                    commentCount={commentCount} 
                    setCommentCount={setCommentCount}
                    viewCount = {viewCount}
                    likeCount={likeCount}
                    dislikeCount={dislikeCount}
                    isLiked={isLiked}
                    isDisliked={isDisliked}
                    handleLike={handleLike}
                    handleDislike={handleDislike}
                    setIsModalOpen={setIsModalOpen}
                    formatDate={formatDate}
                    pictureLayout={pictureLayout}
                />}
        </div>
    );
}
