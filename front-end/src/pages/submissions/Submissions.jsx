import React, { useState, useEffect, useRef } from 'react';
import Submission from '../../components/submission/Submission';
import "./submissions.css"
import { useParams } from 'react-router-dom';
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import authService from '../../services/auth.service';
import Skeleton from "@mui/material/Skeleton";
import { Box} from "@mui/material";
export default function Submissions ({userId}) {
    const { facultyId } = useParams();
    const [submissions, setSubmissions] = useState([]);
    const  [filteredSubmissions, setFilteredSubmissions] = useState([]);
    const [selectedFilter, setSelectedFilter] = useState("all");
    const [error, setError] = useState()
    const [currentUser, setCurrentUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const [selectedSubmissions, setSelectedSubmissions] = useState([]);
    const [isSubmitting, setIsSubmitting] = useState(false);
    useEffect(() => {
        const fetchCurrentUser = async () => {
            setLoading(true)
            const user = authService.getCurrentUser();
            if (user) {
                setCurrentUser(user);
            }
            setLoading(false)
        };
        fetchCurrentUser();
    }, []);
    const fetchSubmissions = async () => {
        if (facultyId && currentUser) {
            setLoading(true)
            try {
                setError(null)
                const response = await authHeader().get(apis.article + "faculty/" + facultyId, {params:{userId: currentUser.id}});
                setSubmissions(response.data)
                setFilteredSubmissions(response.data);
                setLoading(false)

            }catch (error) {
                setSubmissions([])
                setFilteredSubmissions([])
                setError(error.response.data)
                console.error(error.response.data);
                setLoading(false)
            }
        }
    }
    useEffect(()=>{
        fetchSubmissions();
    },[facultyId,currentUser])


    useEffect(()=>{
        const fetchPosts = async () => {
            if (userId && currentUser) {
                setLoading(true)
                try {
                    setError(null)
                    let url;
                    url = apis.article + "profile/submission"
                    const response = await authHeader().get(url, {params: {userId: userId}});
                    setSubmissions(response.data)
                    setFilteredSubmissions(response.data)
                    setLoading(false)
                }catch (error) {
                    // setError(error.response.data)
                    setSubmissions([])
                    setFilteredSubmissions([])
                    console.error("Error fetching post data:", error);
                    setLoading(false)
                }
            }
        }
        fetchPosts();
    },[userId, currentUser])
    const handleFilterChange = (event) => {
        setSelectedFilter(event.target.value);
        // Call function to sort posts based on selected filter
        filterSubmissions(event.target.value);
        console.log()
    };
    const filterSubmissions = (filter) => {
        console.log(filter)
        let filteredSubmissions = [...submissions];
        switch (filter) {
            case "all":
                filteredSubmissions = submissions;
                break;
            case "approved":
                filteredSubmissions = filteredSubmissions.filter(submission => submission.publishStatusId === 1 && submission.coordinatorStatus === false);
                break;
            case "not commented":
                filteredSubmissions = filteredSubmissions.filter(submission => submission.coordinatorComment === null && submission.publishStatusId === 3);
                break;
            case "commented":
                filteredSubmissions = filteredSubmissions.filter(submission => submission.coordinatorComment !== null && submission.publishStatusId === 3);
                break;
            case "reject":
                filteredSubmissions = filteredSubmissions.filter(submission => submission.publishStatusId === 2);
                break;
            case 'guest approved':
                filteredSubmissions = filteredSubmissions.filter(submission => submission.publishStatusId === 1 && submission.coordinatorStatus === true);
                break;
            default:
                filteredSubmissions = submissions;
                break;
        }
        setFilteredSubmissions(filteredSubmissions);
    };

    const handleCheckboxChange = (submissionId, checked) => {
        if (checked) {
            setSelectedSubmissions(prevState => [...prevState, submissionId]);
        } else {
            setSelectedSubmissions(prevState => prevState.filter(id => id !== submissionId));
        }
    };
    const handleGuestUnApprove = async() => {
        try {
            setIsSubmitting(true)
            const FormData = require('form-data');
            const formData = new FormData();
            selectedSubmissions.forEach(articleId => {
                formData.append('articleIds', articleId);
            });
            // setIsSubmitting(true);
            const url = `${apis.article}UnapproveListArticlesForGuest`;

            const res = await authHeader().put(url, formData);
            if (res.status === 200) {
                setSelectedFilter("approved"); 
                await fetchSubmissions()
                filterSubmissions("approved")
                setIsSubmitting(false)
            } else {
                // setIsSubmitting(false);
                // setMessage(`An error occurred: ${res.data}`);
                setIsSubmitting(false)
            }
        } catch (error) {
            // setIsSubmitting(false);
            // setMessage(error.response.data);
            setIsSubmitting(false)
        }
        console.log("Guest approved submissions:", selectedSubmissions);
    };
    const handleGuestApprove = async() => {
        try {
            setIsSubmitting(true)
            const FormData = require('form-data');
            const formData = new FormData();
            selectedSubmissions.forEach(articleId => {
                formData.append('articleIds', articleId);
            });
            // setIsSubmitting(true);
            const url = `${apis.article}ApproveListArticlesForGuest`;

            const res = await authHeader().put(url, formData);
            if (res.status === 200) {
                setSelectedFilter("guest approved"); 
                await fetchSubmissions();
                filterSubmissions("guest approved")
                setIsSubmitting(false)
            } else {
                // setIsSubmitting(false);
                // setMessage(`An error occurred: ${res.data}`);
                setIsSubmitting(false)
            }
        } catch (error) {
            // setIsSubmitting(false);
            // setMessage(error.response.data);
            setIsSubmitting(false)
        }
    };
    return (
        <div className="submissions">
            <div className="postFilter">
                        <select value={selectedFilter} onChange={handleFilterChange} disabled={loading} className='filter'>
                        <option value="all">All</option>
                        <option value="approved">Approved</option>
                        <option value="guest approved">Guest Approved</option>
                        <option value="not commented">Not Commented</option>
                        <option value="commented">Commented</option>
                        <option value="reject">Reject</option>
                        </select>
            </div>
            <h1>Submissions</h1>
            {loading ? (
                <Box style={{ width: "100vh" }}>
                    {Array(10)
                    .fill()
                    .map((_, i) => (
                        <>
                        <Skeleton />
                        <Skeleton animation={i % 2 === 0 ? "wave" : false} />
                        </>
                    ))}
                </Box>
            ):(
                <div className="submissionsWrapper">
                    
                    {error ? (
                        <div>{error}</div>
                    ) : (
                    <>
                        {filteredSubmissions.map((submission) => (
                            <div key={submission.id}>
                                {(currentUser.roles.includes("Coordinator") && submission.publishStatusId === 1 
                                    && (selectedFilter === "approved" 
                                    || selectedFilter === "guest approved")) && (
                                    <label className='checkbox'>
                                        <input
                                            className='checkboxBtn'
                                            type="checkbox"
                                            onChange={(e) => handleCheckboxChange(submission.id, e.target.checked)}
                                        />
                                        {submission.title}
                                    </label>
                                )}
                                {/* {(currentUser.roles.includes("Coordinator") && submission.publishStatusId === 1 && selectedFilter === "guest approved") && (
                                    <label>
                                        <input
                                            type="checkbox"
                                            onChange={(e) => handleCheckboxChange(submission.id, e.target.checked)}
                                        />
                                        {submission.title}
                                    </label>
                                )} */}
                                <Submission submission={submission} currentUser={currentUser}/>
                            </div>
                        ))}
                    </>
                    )}
                    {selectedFilter === "approved" && (
                        <button className='guestBtn' onClick={handleGuestApprove} disabled={isSubmitting}>{isSubmitting? 'Loading...':"Confirm Guest Approve"}</button>
                    )}
                    {selectedFilter === "guest approved" && (
                        <button className='guestBtn' onClick={handleGuestUnApprove} disabled={isSubmitting}>{isSubmitting? 'Loading...':"Confirm Remove Guest Approve"}</button>
                    )}
                </div>
            )}
        </div>
    );
};

