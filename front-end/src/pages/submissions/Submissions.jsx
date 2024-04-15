import React, { useState, useEffect, useRef } from 'react';
import Submission from '../../components/submission/Submission';
import "./submissions.css"
import { useParams } from 'react-router-dom';
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import authService from '../../services/auth.service';
import Skeleton from "@mui/material/Skeleton";
import { Box} from "@mui/material";
export default function Submissions () {
    const { facultyId } = useParams();
    const [submissions, setSubmissions] = useState([]);
    const  [filteredSubmissions, setFilteredSubmissions] = useState([]);
    const [selectedFilter, setSelectedFilter] = useState("all");
    const [error, setError] = useState()
    const [currentUser, setCurrentUser] = useState(null);
    const [loading, setLoading] = useState(false);
    const [selectedSubmissions, setSelectedSubmissions] = useState([]);
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
    useEffect(()=>{
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
        fetchSubmissions();
    },[facultyId,currentUser])

    const handleFilterChange = (event) => {
        setSelectedFilter(event.target.value);
        // Call function to sort posts based on selected filter
        filterSubmissions(event.target.value);
    };
    const filterSubmissions = (filter) => {
        let filteredSubmissions = [...submissions];
        switch (filter) {
            case "all":
                filteredSubmissions = submissions;
                break;
            case "approved":
                filteredSubmissions = filteredSubmissions.filter(submission => submission.publishStatusId === 1);
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
    const handleGuestApprove = async() => {
        try {
            const FormData = require('form-data');
            const formData = new FormData();
            selectedSubmissions.forEach(articleId => {
                formData.append('articleIds', articleId);
            });
            // setIsSubmitting(true);
            const url = `${apis.article}updateListCoordinatorStatus`;

            const res = await authHeader().put(url, formData);
            if (res.status === 200) {

                // localStorage.setItem("accounts", JSON.stringify(updatedData));
                // setIsSubmitting(false);
                // setMessage("Account edited successfully.");
            } else {
                // setIsSubmitting(false);
                // setMessage(`An error occurred: ${res.data}`);
            }
        } catch (error) {
            // setIsSubmitting(false);
            // setMessage(error.response.data);
        }
        console.log("Guest approved submissions:", selectedSubmissions);
    };
    return (
        <div className="submissions">
            <div className="postFilter">
                        <select value={selectedFilter} onChange={handleFilterChange}>
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
                                {currentUser.roles.includes("Coordinator") && submission.publishStatusId === 1 && (
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={submission.publishStatusId === 1 && submission.coordinatorStatus === true}
                                            onChange={(e) => handleCheckboxChange(submission.id, e.target.checked)}
                                        />
                                        {submission.title}
                                    </label>
                                )}
                                <Submission submission={submission} currentUser={currentUser}/>
                            </div>
                        ))}
                    </>
                    )}
                    <button onClick={handleGuestApprove}>Guest Approve Selected</button>
                </div>
            )}
        </div>
    );
};

