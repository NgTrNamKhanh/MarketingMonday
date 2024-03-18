import React, { useState, useEffect, useRef } from 'react';
import Submission from '../../components/submission/Submission';
import "./submissions.css"
import { useParams } from 'react-router-dom';
import authHeader from '../../services/auth.header';
import apis from '../../services/apis.service';
import useFetch from '../../hooks/useFetch'
const submissions = [
    {
        id: 1,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: "user1",
        date: "February 20, 2024",
        title: "Submission Title 1",
        content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        submissionimgs: [
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
            "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
            "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX"
        ],
        likes: 10,
        dislikes: 2,
        commentsCount: 5,
        files: [
            // {
            //     name: 'tut3-RADconcepts.doc',
            //     lastModified: 1706591705000,
            //     lastModifiedDate: new Date('Tue Jan 30 2024 12:15:05 GMT+0700 (Indochina Time)'),
            //     size: 30208,
            //     type: 'application/msword',
            //     webkitRelativePath: ''
            // }
        ]
    },
    {
        id: 2,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        username: "user2",
        date: "February 19, 2024",
        title: "Submission Title 2",
        content: "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
        submissionimgs: [
            "https://m.media-amazon.com/images/M/MV5BNmNkNWU5NzUtNmVkNS00ZDE2LTg0NjgtNTIxNWYxOWIyM2FlXkEyXkFqcGdeQWFkcmllY2xh._V1_.jpg",
            "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX"
        ],
        likes: 20,
        dislikes: 1,
        commentsCount: 8,
        files: [
            // Add file objects here if needed
        ]
    }
    // Add more submissions as needed
];

export default function Submissions () {
    const { facultyId } = useParams();
    const [submissions, setSubmissions] = useState([]);
    const  [filteredSubmissions, setFilteredSubmissions] = useState([]);
    const [selectedFilter, setSelectedFilter] = useState("all");
    const [error, setError] = useState()

    useEffect(()=>{
        const fetchSubmissions = async () => {
            if (facultyId) {
                try {
                    setError(null)
                    const response = await authHeader().get(apis.article + "faculty/" + facultyId);
                    setSubmissions(response.data)
                    setFilteredSubmissions(response.data);

                }catch (error) {
                    setSubmissions([])
                    setFilteredSubmissions([])
                    setError(error.response.data)
                    console.error(error.response.data);
                }
            }
        }
        fetchSubmissions();
    },[facultyId])

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
            default:
                filteredSubmissions = submissions;
                break;
        }
        setFilteredSubmissions(filteredSubmissions);
    };

    return (
        <div className="submissions">
            <div className="submissionsWrapper">
            <div className="postFilter">
                <select value={selectedFilter} onChange={handleFilterChange}>
                <option value="all">All</option>
                <option value="approved">Approved</option>
                <option value="not commented">Not Commented</option>
                <option value="commented">Commented</option>
                <option value="reject">Reject</option>
                </select>
            </div>
            <h1>Submissions</h1>
            {error ? (
                <div>{error}</div>
            ) : (
                filteredSubmissions.map((submission) => (
                    <Submission key={submission.id} submission={submission} />
                ))
            )}
            </div>
        </div>
    );
};

