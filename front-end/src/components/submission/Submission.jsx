import { MoreVert } from '@mui/icons-material';
import './submission.css';
import { useEffect, useRef, useState } from 'react';
import Confirm from '../confirm/Confirm';

export default function  Submission ({ submission, onComment, onVerify }) {
    const [optionsOpen, setOptionsOpen] = useState(false);
    const [tncOpen, setTnCOpen] = useState(false);
    const [isCommenting, setIsCommenting] = useState(false);
    const [comment, setComment] = useState("Hi, this is very good");

    const optionsRef = useRef(null);
    useEffect(() => {
        // Function to handle click outside of the options dropdown
        function handleClickOutside(event) {
            if (optionsRef.current && !optionsRef.current.contains(event.target)) {
                setOptionsOpen(false);
            }
        }
    
        // Function to handle scroll outside of the options dropdown
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

    const handleCloseTnCDialog = () => {
        setTnCOpen(false);
    };
    const handleOpenTnCDialog = () => {
        setTnCOpen(true);
    };
    const handleSubmit = () =>{
        console.log("Hi")
    };
    const handleComment = (e) => {
        e.preventDefault();
        setIsCommenting(false);
        setComment(e.target.value);
    };

    const handleVerify = () => {
        // Implement verify functionality
        onVerify(submission.id);
    };
    
    let pictureLayout;
    if (submission.submissionimgs.length === 1) {
        pictureLayout = (
            <div className="submissionCenter">
                {submission.submissionimgs.map((img, index) => (
                    <img key={index} src={img} className="submissionImg" alt={`submission image ${index}`} />
                ))}
            </div>
        );
    } else if (submission.submissionimgs.length === 2) {
        pictureLayout = (
            <div className="submissionImgGroup">
                <img src={submission.submissionimgs[0]} className="submissionImg submissionImgBottom" alt="submission image 1" />
                <img src={submission.submissionimgs[1]} className="submissionImg submissionImgBottom" alt="submission image 2" />
            </div>
        );
    } else{
        pictureLayout = (
            <div className="submissionCenter">
                <div className="submissionImgGroup">
                    <img src={submission.submissionimgs[0]} className="submissionImg submissionImgBottom" alt="submission image 1" />
                    <img src={submission.submissionimgs[1]} className="submissionImg submissionImgBottom" alt="submission image 2" />
                </div>
                <div className="submissionImgGroup">
                    <img src={submission.submissionimgs[2]} className="submissionImg submissionImgBottom" alt={`submission image 3`} />
                    {submission.submissionimgs.length > 3 && 
                        <div className="extraImg">
                            {submission.submissionimgs.slice(3,7).map((img, index) => (
                                <img key={index} src={img} className="submissionImg submissionImgBottom" alt={`submission image ${index + 3}`} />
                            ))}
                            <div className="overlay">+{submission.submissionimgs.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 
    const [status, setStatus] = useState('');

    useEffect(() => {
        const submissionDateObj = new Date(submission.date);

        const currentDate = new Date();

        const timeDiff = currentDate.getTime() - submissionDateObj.getTime();
        const daysDiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
        const hoursDiff = Math.floor((timeDiff % (1000 * 3600 * 24)) / (1000 * 3600));
        const minutesDiff = Math.floor((timeDiff % (1000 * 3600)) / (1000 * 60));
        console.log(daysDiff)
        if(comment){
            setStatus('commented')
        }
        else if (daysDiff < 13) {
            setStatus(`${14 - daysDiff} days remain`);
        }else if(hoursDiff<23){
            setStatus(`${hoursDiff} hours remain`)
        } else if(minutesDiff<59){
            setStatus(`${minutesDiff} minutes remain`)
        }
        else {
            setStatus('expired');
        }
    }, [submission.date]);

    return (
        <div className="submission">
            <div className="submissionWrapper">
                <div className="submissionTop">
                    <div className="submissionTopLeft">
                        <img src={submission.img} className="submissionProfileImg" alt="profile" />
                        <span className="submissionUsername">{submission.username}</span>
                        <span className="submissionDate">
                            {submission.date} 
                            ({status})
                            </span>
                    </div>
                    <div className="submissionTopRight">
                    <MoreVert className='moreIcon' onClick={()=>setOptionsOpen(!optionsOpen)}/>
                    {optionsOpen && (
                        <div className="submissionDropdownContent" ref={optionsRef}>
                            {/* <Link to="/profile" className="dropdownContentItemLink"> */}
                                <a className="submissionDropdownContentItem" href="#comment" onClick={()=>setIsCommenting(true)}>
                                        Comment
                                </a>
                            {/* </Link> */}
                            {/* <Link to="/profile" className="dropdownContentItemLink"> */}
                            <div className="submissionDropdownContentItem">
                                        <span>Report</span>
                                </div>
                            {/* </Link> */}
                            {/* <Link to="/settings" className="dropdownContentItemLink"> */}
                                <div className="submissionDropdownContentItem" onClick={handleOpenTnCDialog}>
                                        <span>Confirm</span> 
                                </div>
                            {/* </Link> */}
                        </div>
                    )}
                    </div>
                </div>
                <div className="submissionCenter">
                    <h2 className='submissionTitle'>{submission.title}</h2>
                    <p className='submissionContent'>{submission.content}</p>
                    {submission.files.map((file, index) => (
                        <div key={index} className="itemContainer">
                            <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                        </div>
                    ))}
                </div>
                {pictureLayout}
                <div className="commentSection" id='comment'>
                    <form onSubmit={handleComment}>
                        <textarea 
                            value={comment}
                            disabled={!isCommenting}
                            placeholder="Write your comment here..." 
                            onChange={(e) => setComment(e.target.value)}
                        />
                        {isCommenting && (
                            <button type="submit">Submit</button>
                        )}
                    </form>
                </div>
            </div>
            <Confirm
                open = {tncOpen}
                handleClose = {handleCloseTnCDialog}
                handleConfirm = {handleSubmit}
            />
        </div>
    );
};

