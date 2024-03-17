import { MoreVert } from '@mui/icons-material';
import './submission.css';
import { useEffect, useRef, useState } from 'react';
import TermsAndConditions from '../termsAndConditions/TermsAndConditions';
const termsAndConditionsText = (
    <div>
        <p>
            <strong>By submitting an article:</strong> You, as the teacher, agree to the following terms and conditions:
        </p>
        <ul>
            <li>
            <strong>Verification:</strong> You acknowledge that you are responsible for verifying the authenticity and originality of the submitted article.
            </li>
            <li>
            <strong>Public Disclosure:</strong> You agree that upon successful verification, the submitted article may be made public for educational purposes.
            </li>
            <li>
            <strong>Confidentiality:</strong> You agree to handle all submissions with confidentiality and shall not disclose any personally identifiable information of the student without their consent, except as required by law or educational policy.
            </li>
            <li>
            <strong>Ownership:</strong> You acknowledge that the student retains ownership and copyright of their work, and you shall not claim ownership or reproduce the work for commercial purposes without the student's explicit consent.
            </li>
            <li>
            <strong>Agreement:</strong> By submitting the article, you confirm that you have read, understood, and agree to abide by these terms and conditions.
            </li>
        </ul>
    </div>
);
const headerText = (
    <>
        <p>Thank you for submitting this articles, your work is a great asset to the school!</p>
        <p>Before this article can be submitted for the Coodinator to see, there are a few Terms and Conditions:</p>
    </>
);
export default function  Submission ({ submission, onComment, onVerify }) {
    console.log(submission)
    const [optionsOpen, setOptionsOpen] = useState(false);
    const [tncOpen, setTnCOpen] = useState(false);
    const [isCommenting, setIsCommenting] = useState(false);
    const [comment, setComment] = useState(submission.coordinatorComment);

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
    if (submission.imageBytes.length === 1) {
        pictureLayout = (
            <div className="submissionCenter">
                {submission.imageBytes.map((img, index) => (
                    <img key={index} src={img} className="submissionImg" alt={`submission image ${index}`} />
                ))}
            </div>
        );
    } else if (submission.imageBytes.length === 2) {
        pictureLayout = (
            <div className="submissionImgGroup">
                <img src={submission.imageBytes[0]} className="submissionImg submissionImgBottom" alt="submission image 1" />
                <img src={submission.imageBytes[1]} className="submissionImg submissionImgBottom" alt="submission image 2" />
            </div>
        );
    } else{
        pictureLayout = (
            <div className="submissionCenter">
                <div className="submissionImgGroup">
                    <img src={submission.imageBytes[0]} className="submissionImg submissionImgBottom" alt="submission image 1" />
                    <img src={submission.imageBytes[1]} className="submissionImg submissionImgBottom" alt="submission image 2" />
                </div>
                <div className="submissionImgGroup">
                    <img src={submission.imageBytes[2]} className="submissionImg submissionImgBottom" alt={`submission image 3`} />
                    {submission.imageBytes.length > 3 && 
                        <div className="extraImg">
                            {submission.imageBytes.slice(3,7).map((img, index) => (
                                <img key={index} src={img} className="submissionImg submissionImgBottom" alt={`submission image ${index + 3}`} />
                            ))}
                            <div className="overlay">+{submission.imageBytes.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 
    const [status, setStatus] = useState('');

    useEffect(() => {
        if (submission.coordinatorComment) {
            setStatus('commented');
        } else if (submission.publishStatusId === 1) {
            setStatus('approved');
        } else if (submission.publishStatusId === 3 && submission.coordinatorComment === null) {
            setStatus('not commented');
        } else if (submission.publishStatusId === 2) {
            setStatus('reject');
        }
    }, [submission.coordinatorComment, submission.publishStatusId]);


    return (
        <div className="submission">
            <div className="submissionWrapper">
                <div className="submissionTop">
                    <div className="submissionTopLeft">
                        <img src={submission.studentName} className="submissionProfileImg" alt="profile" />
                        <span className="submissionUsername">{submission.studentName}</span>
                        <span className="submissionDate">
                            {new Date(submission.date).toLocaleDateString()} {new Date(submission.date).toLocaleTimeString()}
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
                    <p className='submissionContent'>{submission.description}</p>
                    {/* {submission.files.map((file, index) => (
                        <div key={index} className="itemContainer">
                            <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                        </div>
                    ))} */}
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
            <TermsAndConditions
                open = {tncOpen}
                handleClose = {handleCloseTnCDialog}
                handleConfirm = {handleSubmit}
                termsAndConditionsText = {termsAndConditionsText}
                headerText = {headerText}
            />
        </div>
    );
};

