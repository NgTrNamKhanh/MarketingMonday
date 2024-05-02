import { Send } from "@mui/icons-material";
import { useState } from "react";
import { Checkbox, FormControlLabel } from "@mui/material";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
import { ScaleLoader } from "react-spinners";
export default function CommentForm ({ currentUser, isSubmitting, setIsSubmitting, setComments,setCommentCount, post }){
    const [commentValue, setCommentValue] = useState('');
    const [isAnonymous, setIsAnonymous] = useState(false);
    const handleCommentChange = (event) => {
        setCommentValue(event.target.value);
    };
    const handleComment =  async (event) =>{
        event.preventDefault();
        setIsSubmitting(true);
        try {
            const comment = {
                content: event.target.comment.value,
                userId: currentUser.id,
                articleId: post.id,
                isAnonymous: isAnonymous
            }
            const res = await authHeader().post(apis.comment+"createComment", comment);
            if (res.status === 200) {
                console.log(res)
                setComments((prevComments) => [res.data,...prevComments ]);
                setCommentCount((prevCount) => prevCount + 1)
                setIsSubmitting(false);
                setCommentValue("")
            } else {
                setCommentValue("")
                setIsSubmitting(false);
            }
        } catch (error) {
            setCommentValue("")
            setIsSubmitting(false);
        }

    }
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
                <FormControlLabel
                    control={<Checkbox checked={isAnonymous} onChange={(e) => setIsAnonymous(e.target.checked)} />}
                    label="Submit as Anonymous"
                />
                <button 
                    disabled={isSubmitting || !commentValue.trim()} 
                    type="submit" 
                    className="commentButton"
                >
                    {isSubmitting ?(
                        <ScaleLoader/>
                    ):(
                        <Send/>
                    )}
                </button>
            </form>
        </div>

    )
}