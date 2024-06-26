import { MoreVert } from '@mui/icons-material';
import './submission.css';
import { useEffect, useRef, useState } from 'react';
import TermsAndConditions from '../dialogs/termsAndConditions/TermsAndConditions';
import apis from '../../services/apis.service';
import authHeader from '../../services/auth.header';
import { Link } from 'react-router-dom';
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
        <p>Thank you for assessing this submission, your work is a great asset to the school!</p>
        <p>Before this submission can be assessed, there are a few Terms and Conditions:</p>
    </>
);
export default function  Submission ({ submission, currentUser, reFetch }) {
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
    const [optionsOpen, setOptionsOpen] = useState(false);
    const [tncOpen, setTnCOpen] = useState(false);
    const [isCommenting, setIsCommenting] = useState(false);
    const [comment, setComment] = useState(submission.coordinatorComment);
    const [isSubmitting, setIsSubmitting] = useState();
    const optionsRef = useRef(null);

    const [submitStatus, setSubmitStatus] =  useState();
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

    const handleCloseTnCDialog = () => {
        setTnCOpen(false);
    };
    const handleOpenTnCDialog = (submitStatus) => {
        setSubmitStatus(submitStatus)
        setTnCOpen(true);
    };
    const handleSubmit = async () =>{
        await handleVerifyOrReject();
    };
    const handleComment = async (e) => {
        e.preventDefault();
        setIsCommenting(false);
        setComment(e.target.value);
        try {
            setIsSubmitting(true);
            const url = apis.article+"addComment/"+submission.id+"?comment="+comment
            const res = await authHeader().put(url, {});
            if (res.status === 200) {
                await reFetch(currentUser.facultyId,currentUser);
                // localStorage.setItem("accounts", JSON.stringify(updatedData));
                setIsSubmitting(false);
                // setMessage("Account edited successfully.");
            } else {
                setIsSubmitting(false);
                // setMessage(`An error occurred: ${res.data}`);
            }
        } catch (error) {
            console.log(error)
            setIsSubmitting(false);
            // setMessage(error.response.data);
        }
    };

    
    let pictureLayout;
    if (submission.listCloudImagePath == null){
        pictureLayout = null
    }
    else if (submission.listCloudImagePath.length === 1) {
        pictureLayout = (
            <div className="submissionCenter">
                {submission.listCloudImagePath.map((img, index) => (
                    <img src={img} key={index} className="submissionImg"  alt={`submission image ${index}`}/>
                    // <img key={index} src={img} className="submissionImg" alt={`submission image ${index}`} />
                ))}
            </div>
        );
    } else if (submission.listCloudImagePath.length === 2) {
        pictureLayout = (
            <div className="submissionImgGroup">
                <img src={submission.listCloudImagePath[0]} className="submissionImg submissionImgBottom" alt="Rendered Image"/>
                <img src={submission.listCloudImagePath[1]} className="submissionImg submissionImgBottom" alt="Rendered Image"/>
            </div>
        );
    } else if (submission.listCloudImagePath.length > 2){
        pictureLayout = (
            <div className="submissionCenter">
                <div className="submissionImgGroup">
                    <img src={submission.listCloudImagePath[0]}  alt="Rendered Image"/>
                    <img src={submission.listCloudImagePath[1]} alt="Rendered Image"/>
                </div>
                <div className="submissionImgGroup">
                    <img src={submission.listCloudImagePath[2]}className="submissionImg"  alt="Rendered Image"/>
                    {submission.listCloudImagePath.length > 3 && 
                        <div className="extraImg">
                            {submission.listCloudImagePath.slice(3,7).map((img, index) => (
                                <img src={img} key={index} className="submissionImg submissionImgBottom" alt={`submission image ${index + 3}`}/>
                            ))}
                            <div className="overlay">+{submission.listCloudImagePath.length - 3}</div>
                        </div>
                    }
                </div>
            </div>
        );
    } 
    const [status, setStatus] = useState('');

    useEffect(() => {
        if (submission.publishStatusId === 1 && submission.coordinatorStatus == true) {
            setStatus('guest approved');
        }
        else if (submission.publishStatusId === 1) {
            setStatus('approved');
        }  else if (submission.publishStatusId === 2) {
            setStatus('reject');
        }else if (submission.publishStatusId === 3 && submission.coordinatorComment === null) {
            setStatus('not commented');
        } 
        else if (submission.coordinatorComment) {
            setStatus('commented');
        }
        
    }, []);
    function getStatusColor(status) {
        switch (status) {
            case 'approved':
                return 'green';
            case 'guest approved':
                return 'green';
            case 'reject':
                return 'red';
            case 'commented':
                return '#a89132';
            default:
                return 'orange';
        }
    }
    const handleVerifyOrReject = async () => {
        try {
            setIsSubmitting(true);
            const url = `${apis.article}updatePublishStatus/${submission.id}?publicStatus=${submitStatus}`;

            const res = await authHeader().put(url, {});
            if (res.status === 200) {
                await reFetch(currentUser.facultyId,currentUser);
                // localStorage.setItem("accounts", JSON.stringify(updatedData));
                setIsSubmitting(false);
                // setMessage("Account edited successfully.");
            } else {
                console.log(res)
                setIsSubmitting(false);
                // setMessage(`An error occurred: ${res.data}`);
            }
        } catch (error) {
            console.log(error)
            setIsSubmitting(false);
            // setMessage(error.response.data);
        }
    };
    
    const [isDownloading, setIsDownloading] = useState(false);
    const handleDownload = async () => {
        setIsDownloading(true);
        try {
            const response = await authHeader().post(apis.article+"download", null, { params: {articleId: submission.id},
                responseType: 'blob' 
            });

            // Create a blob URL from the response data
            const blob = new Blob([response.data], { type: 'application/zip' });
            const url = window.URL.createObjectURL(blob);

            // Create a temporary link element to trigger the download
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'submission.zip');
            document.body.appendChild(link);
            link.click();

            // Clean up
            window.URL.revokeObjectURL(url);
            document.body.removeChild(link);
        } catch (error) {
            console.error('Error downloading submission:', error);
            // Handle error
        } finally {
            setIsDownloading(false);
        }
    };
    return (
        <div className="submission">
            <div className="submissionWrapper">
                <div className="submissionTop">
                    <div className="submissionTopLeft">
                        <Link
                            to={`/account/${submission.studentId}`}
                        >
                        <img src={submission.studentAvatar} className="submissionProfileImg" alt="profile" />
                        </Link>
                        <Link
                            to={`/account/${submission.studentId}`}
                        >
                        <span className="submissionUsername">{submission.studentName}</span>
                        </Link>
                        {
                            submission.isAnonymous && (
                                <span className="submissionUsername">(Anonymous)</span>
                            )
                        }
                        <span className="submissionDate">
                            {formatDate(submission.uploadDate)}
                        </span>
                        <span className="submissionDate">
                            (
                            <span style={{ color: getStatusColor(status) }}>
                                {status}
                            </span>
                            )
                        </span>
                    </div>
                    <div className="submissionTopRight">
                        {currentUser.roles && currentUser.roles.some(role => role === "Coordinator" || role === "Manager") && (
                            <>
                                <MoreVert className='moreIcon' onClick={()=>setOptionsOpen(!optionsOpen)}/>
                                {optionsOpen && (
                                    <div className="submissionDropdownContent" ref={optionsRef}>
                                        {currentUser.roles.includes("Coordinator") && (
                                            <>
                                                {/* <Link to="/profile" className="dropdownContentItemLink"> */}
                                                <div className="submissionDropdownContentItem" onClick={()=>setIsCommenting(true)}>
                                                        Comment
                                                </div>
                                                {/* </Link> */}
                                                {/* <Link to="/settings" className="dropdownContentItemLink"> */}
                                                <div className="submissionDropdownContentItem" onClick={()=>handleOpenTnCDialog(1)}>
                                                            <span>Confirm</span> 
                                                </div>
                                                {/* </Link> */}
                                                {/* <Link to="/profile" className="dropdownContentItemLink"> */}
                                                <div className="submissionDropdownContentItem" onClick={()=>handleOpenTnCDialog(2)}>
                                                            <span>Reject</span>
                                                </div>
                                                {/* </Link> */}
                                                {/* <Link to="/settings" className="dropdownContentItemLink"> */}
                                                <div className="submissionDropdownContentItem" onClick={()=>handleOpenTnCDialog(3)}>
                                                            <span>Put On Hold</span> 
                                                </div>
                                                {/* </Link> */}
                                            </>
                                        )}
                                        {currentUser.roles.includes("Manager") && (
                                            <div className="submissionDropdownContentItem" onClick={()=>handleDownload()}>
                                                    <span>Download</span> 
                                        </div>
                                        )}
                                    </div>
                                )}
                            </>
                        )}
                    
                    </div>
                </div>
                <div className="submissionCenter">
                    <h2 className='submissionTitle'>{submission.title}</h2>
                    <div className='submissionContent' dangerouslySetInnerHTML={{ __html: submission.description }} />
                    {/* {submission.files.map((file, index) => (
                        <div key={index} className="itemContainer">
                            <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                        </div>
                    ))} */}
                </div>
                {pictureLayout}
                <div className="submissionFile">
                        <div className="submissionFileContainer">
                            <a href={submission.cloudDocPath} className="submissionFileLink">File</a>
                        </div>
                </div>
                <div className="commentSection" id='comment'>
                    <form onSubmit={handleComment}>
                        <textarea 
                            value={comment}
                            disabled={!isCommenting}
                            placeholder="Write your comment here..." 
                            onChange={(e) => setComment(e.target.value)}
                        />
                        {isCommenting && (
                            <button type="submit" disabled={isSubmitting}>{isSubmitting?'Loading...':'Comment'}</button>
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

