import { Send } from "@mui/icons-material";
import { useState } from "react";

export default function CommentForm ({handleComment, currentUser, isSubmitting}){
    const [commentValue, setCommentValue] = useState('');
    const handleCommentChange = (event) => {
        setCommentValue(event.target.value);
    };
    return (
        <div className="commentInputWrapper">
            <img src={currentUser.avatar} className="userAvatar" alt="profile" />
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