import { useEffect, useState } from "react";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
import EditComment from "../dialogs/editCmt/EditComment";
import DeleteConfirm from "../dialogs/deleteConfirm/DeleteConfirm";
import { MoreVert, ThumbDown, ThumbUp } from "@mui/icons-material";
import CommentForm from "../commentForm/CommentForm";
import Comments from "../comments/Comments"
import { Link } from "react-router-dom";

import "./comment.css"
export default function CommentBlock ({comment, currentUser, post, formatDate, setCommentCount}){
    console.log(comment)
    const [selectedComment, setSelectedComment] = useState(comment);
    const [deleted, setDeleted] = useState(false);
    const [editCmtOpen, setEditCmtCOpen] = useState(false);
    const handleCloseEditCmtCDialog = () => {
        setEditCmtCOpen(false);
    };
    const handleOpenEditCmtCDialog = (hi) => {
        setEditCmtCOpen(true);
    };

    const [deleteCmtOpen, setDeleteCmtCOpen] = useState(false);
    const handleCloseDeleteCmtCDialog = () => {
        setDeleteCmtCOpen(false);
    };
    const handleOpenDeleteCmtCDialog = (e) => {
        setDeleteCmtCOpen(true);
    };

    const [replies, setReplies] = useState([]);
    const [repLoading, setRepLoading] = useState(false);
    useEffect(() => {
        const fetchData = async () => {
            if(selectedComment.hasReplies){
                setRepLoading(true)
                try {
                    const response = await authHeader().get(apis.comment + "getReplies", { params: { parentId: comment.id, userId: currentUser.id }});
                    setReplies(response.data);
                    setRepLoading(false);
                } catch (error) {
                    console.error("Error fetching comments:", error);
                    setRepLoading(false);
                }
            }
        };

        fetchData();
    }, []);

    const [showReplies, setShowReplies] = useState(false);
    const [commentLikesCount, setCommentLikesCount] = useState(selectedComment.likesCount)
    const [isCommentLiked, setIsCommentLiked] = useState(selectedComment.isLiked)
    const handleCommentLike = () => {
        setCommentLikesCount(isCommentLiked? commentLikesCount -1 : commentLikesCount+1)
        setIsCommentLiked(!isCommentLiked)
        try{
            const like ={
                userId : currentUser.id,
                commentId: selectedComment.id
            }
            authHeader().post(apis.like+"comment", like);
        }catch(err){
        }
        if(isCommentDisliked){
            handleCommnetDislike()
        }
    }
    const [commentDislikesCount, setCommentDisLikesCount] = useState(selectedComment.dislikesCount)
    const [isCommentDisliked, setIsCommnetDisLiked] = useState(selectedComment.isDisliked)
    const handleCommnetDislike = () => {
        setCommentDisLikesCount(isCommentDisliked? commentDislikesCount -1 : commentDislikesCount+1)
        setIsCommnetDisLiked(!isCommentDisliked)
        try{
            const dislike ={
                userId : currentUser.id,
                commentId: selectedComment.id
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
        try {
            const reply = {
                content: event.target.comment.value,
                userId: currentUser.id,
                articleId: post.id
            }
            const res = await authHeader().post(apis.comment+"createReply", reply, {params:{parentCommentId:selectedComment.id }});
            if (res.status === 200) {
                setReplies(prevComments => [...prevComments, res.data]);
            } 
        } catch (error) {
        }

    }
    const handleDeleteCmt = async () =>{
        console.log(selectedComment)
        if (selectedComment) {
            const url = apis.comment;
            try {
                const response = await authHeader().delete(url, {params:{commentId: selectedComment.id}});
                setCommentCount((prevCount) => prevCount - 1)
                setDeleted(true)
                handleCloseDeleteCmtCDialog()
            } catch (err) {
                console.error(err);
            }
        }
    }
    const [optionsOpen, setOptionsOpen] = useState(false);
    if(deleted){
        return null;
    }
    return (
        <div key={selectedComment.id} className="comment" >
            <Link
                to={`/account/${selectedComment.userComment.id}`}
            >
                <img src={selectedComment.userComment.userAvatar} className="commentProfileImg" alt="profile" />
            </Link>
            <div className="commentTop">
                <div className="commentTopLeft">
                    <Link
                        to={`/account/${selectedComment.userComment.id}`}
                    >
                        <span className="commentUsername">{selectedComment.userComment.firstName} {selectedComment.userComment.lastName}</span>
                    </Link>
                    <span className="commentDate">{formatDate(selectedComment.createOn)}</span>
                </div>
            </div>
            <p className="commentContent">{selectedComment.content}</p>
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
            {selectedComment.hasReplies && (
                <span onClick={toggleReplies} style={{ cursor: 'pointer' }} className="viewReplies ">
                    {showReplies ? 'Hide Replies' : 'View Replies'}
                </span>
            )}
            <div className="commentReply">
                {showReplies && selectedComment.hasReplies && (
                    <Comments comments= {replies} 
                        loading={repLoading} 
                        currentUser={currentUser} 
                        post={post}
                        formatDate={formatDate}
                    />
                )}
                {openCommentInput && (
                <CommentForm handleComment={handleReply} />
                )}
            </div>
            {currentUser.id === selectedComment.userComment.id && (
                <div className="commentTopRight">
                    <MoreVert className='moreIcon' onClick={()=>setOptionsOpen(!optionsOpen)}/>
                    {optionsOpen && (
                        <div className="commentDropdownContent" >
                            <div className="commentDropdownContentItem" onClick={()=>handleOpenEditCmtCDialog(comment)}>
                                    <span>Edit</span>
                            </div>
                            <div className="commentDropdownContentItem" onClick={()=>handleOpenDeleteCmtCDialog(comment)}>
                                        <span>Delete</span>
                            </div>
                        </div>
                    )}
                </div>
            )}
            {editCmtOpen && (
            <EditComment
                open={editCmtOpen}
                handleClose={handleCloseEditCmtCDialog}
                comment = {selectedComment}
                setSelectedComment={setSelectedComment}
            />
            )}
            <DeleteConfirm
                open={deleteCmtOpen}
                handleClose={handleCloseDeleteCmtCDialog}
                handleConfirm={handleDeleteCmt}
            />
        </div>
        
    )
}