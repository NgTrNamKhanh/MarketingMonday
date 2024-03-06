import { AttachFile, PermMedia } from "@mui/icons-material";
import React, { useEffect, useState } from "react";
import "./article.css";
import TermsAndConditions from "../termsAndConditions/TermsAndConditions";

export default function Article() {
    const [selectedPhotos, setSelectedPhotos] = useState([]);
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [tncOpen, setTnCOpen] = useState(false);
    const [startDate, setStartDate] = useState(new Date()); 
    const [endDate, setEndDate] = useState(new Date(2024, 2, 4)); 
    const [remainingTime, setRemainingTime] = useState(0);

    useEffect(() => {
        // Calculate remaining time when startDate or endDate changes
        const intervalId = setInterval(() => {
            const currentTime = new Date();
            const diff = endDate.getTime() - currentTime.getTime();
            setRemainingTime(diff > 0 ? diff : 0);
        }, 1000);

        return () => clearInterval(intervalId);
    }, [startDate, endDate]);

    const handleCloseTnCDialog = () => {
        setTnCOpen(false);
    };
    const handleOpenTnCDialog = () => {
        setTnCOpen(true);
    };
    const handleSubmit = () =>{
        console.log("Hi")
    };

    const handlePhotoUpload = (event) => {
        const files = Array.from(event.target.files);
        files.forEach(file => {
            if (!selectedPhotos.some(photo => photo.name === file.name)) {
                setSelectedPhotos([...selectedPhotos, file]);
            }
        });
    };

    const handleFileUpload = (event) => {
        const files = Array.from(event.target.files);
        files.forEach(file => {
            if (!selectedFiles.some(selectedFile => selectedFile.name === file.name)) {
                setSelectedFiles([...selectedFiles, file]);
            }
        });
        console.log(files[0])
    };
    const handleRemovePhoto = (index) => {
        const updatedPhotos = [...selectedPhotos];
        updatedPhotos.splice(index, 1);
        setSelectedPhotos(updatedPhotos);
    };
    
    const handleRemoveFile = (index) => {
        const updatedFiles = [...selectedFiles];
        updatedFiles.splice(index, 1);
        setSelectedFiles(updatedFiles);
    };


    return (
        <div className="article">
            <div className="articleWrapper">
                <h1>Submission</h1>
                <hr className="articleHr" />
                <div className="dateSection">
                    <p>Opened: {startDate.toLocaleDateString()}</p>
                    <p>Due: {endDate.toLocaleDateString()}</p>
                    <p>Time Remaining: {Math.floor(remainingTime / (1000 * 60 * 60 * 24))} days</p>
                </div>
                <h2 className="createSubmission">Create a submission</h2>
                <div className="articleContent">
                    <input type="text" className="articleTitle" placeholder="Title"/>
                    <br />
                    <textarea placeholder="Write your article?" className="articleInput"></textarea>
                </div>
                <div className="articleBottom">
                    <div className="selectedItems">
                        {selectedPhotos.map((photo, index) => (
                            <div key={index} className="itemContainer">
                                <img src={URL.createObjectURL(photo)} alt={`Uploaded photo ${index}`} className="itemContainerImg"/>
                                <a href={URL.createObjectURL(photo)} className="fileLink" target="_blank" rel="noopener noreferrer">{photo.name}</a>
                                <button className="removeButton" onClick={() => handleRemovePhoto(index)}>X</button>
                            </div>
                        ))}
                        {selectedFiles.map((file, index) => (
                            <div key={index} className="itemContainer">
                                <a href={URL.createObjectURL(file)} className="fileLink" target="_blank" rel="noopener noreferrer">{file.name}</a>
                                <button className="removeButton" onClick={() => handleRemoveFile(index)}>X</button>
                            </div>
                        ))}
                    </div>
                    <div className="articleOptions">
                        <div className="articleOption">
                            <label htmlFor="photoInput" className="fileInput">
                                <PermMedia htmlColor="tomato" className="articleIcon" />
                            </label>
                            <input
                                id="photoInput"
                                type="file"
                                accept=".jpg, .jpeg, .png"
                                style={{ display: "none" }}
                                onChange={handlePhotoUpload}
                                multiple
                            />
                        </div>
                        <div className="articleOption">
                            <label htmlFor="fileInput" className="fileInput">
                                <AttachFile htmlColor="blue" className="articleIcon" />
                            </label>
                            <input
                                id="fileInput"
                                type="file"
                                accept=".docx, .doc"
                                style={{ display: "none" }}
                                onChange={handleFileUpload}
                                multiple
                            />
                        </div>
                        <TermsAndConditions
                            open = {tncOpen}
                            handleClose = {handleCloseTnCDialog}
                            handleConfirm = {handleSubmit}
                        />
                    </div>
                </div>
                <div className="buttonContainer">
                    <button onClick={handleOpenTnCDialog} className="articleButton">Submit</button>
                </div>
            </div>
        </div>
    );
}
