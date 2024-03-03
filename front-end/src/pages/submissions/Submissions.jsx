import React, { useState, useEffect } from 'react';
import Submission from '../../components/submission/Submission';
import "./submissions.css"

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
    useEffect(() => {
        // Fetch submissions data from API or any data source
        // Update submissions state with fetched data
    }, []);

    const handleComment = (submissionId) => {
        // Implement comment functionality
    };

    const handleVerify = (submissionId) => {
        // Implement verify functionality
    };

    return (
        <div className="submissions">
            <div className="submissionsWrapper">
                <h1>Submissions</h1>
                {submissions.map((submission) => (
                    <Submission
                        key={submission.id}
                        submission={submission}
                        onComment={handleComment}
                        onVerify={handleVerify}
                    />
                ))}
            </div>
        </div>
    );
};

