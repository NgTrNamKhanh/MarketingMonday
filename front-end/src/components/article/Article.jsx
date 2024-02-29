import { AttachFile, PermMedia } from "@mui/icons-material";
import React, { useState } from "react";
import "./article.css";
import TermsAndConditions from "../termsAndConditions/TermsAndConditions";

export default function Article() {
    const [selectedPhotos, setSelectedPhotos] = useState([]);
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [tncOpen, setTnCOpen] = useState(false);

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
                <div className="articleTop">
                    <img
                        src="https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX"
                        className="articleProfilePicture"
                    />
                    <textarea placeholder="Write your article?" className="articleInput"></textarea>
                </div>
                <hr className="articleHr" />
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

                <div className="articleBottom">
                    <div className="articleOptions">
                        <div className="articleOption">
                            <label htmlFor="photoInput">
                                <PermMedia htmlColor="tomato" className="articleIcon" />
                                <span className="articleOptionText">Photo</span>
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
                            <label htmlFor="fileInput">
                                <AttachFile htmlColor="blue" className="articleIcon" />
                                <span className="articleOptionText">File</span>
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

                    

                    <button onClick={handleOpenTnCDialog} className="articleButton">Submit</button>
                </div>
            </div>
        </div>
    );
}
