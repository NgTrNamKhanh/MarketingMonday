import { ScaleLoader } from "react-spinners";
import Comment from "../comment/Comment"
export default function Comments({ comments, loading, currentUser, post, formatDate, setCommentCount}){
    if (loading) {
        return (<ScaleLoader/>);
    }
    if (!comments.length) {
        return <p>There are no comments.</p>;
    }
    return (
        <div>
            {comments.map(comment => (
                <Comment comment={comment} currentUser={currentUser} post={post} formatDate={formatDate} setCommentCount={setCommentCount}/>
            ))}
        </div>
    );
};