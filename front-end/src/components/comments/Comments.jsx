import { ScaleLoader } from "react-spinners";
import Comment from "../comment/Comment"
export default function RenderComments({ comments, loading, currentUser, post, formatDate}){
    if (loading) {
        return (<ScaleLoader/>);
    }
    if (!comments.length) {
        return <p>There are no comments.</p>;
    }
    return (
        <div>
            {comments.map(comment => (
                <Comment comment={comment} currentUser={currentUser} post={post} formatDate={formatDate}/>
            ))}
        </div>
    );
};