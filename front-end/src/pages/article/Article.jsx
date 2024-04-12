import { AttachFile, PermMedia } from "@mui/icons-material";
import React, { useEffect, useState } from "react";
import "./article.css";
import TermsAndConditions from "../../components/dialogs/termsAndConditions/TermsAndConditions";
import authHeader from "../../services/auth.header";
import apis from "../../services/apis.service";
import { Form, Formik } from "formik";
import { Button, TextField, Checkbox, FormControlLabel } from "@mui/material";
import * as yup from "yup";
import authService from "../../services/auth.service";
import { useNavigate } from "react-router-dom";
import useFetch from "../../hooks/useFetch";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
const termsAndConditionsText = (
    <ul>
        <li>
            <strong>Acceptance of Terms:</strong> By accessing or using our services, you agree to be bound by these Terms and Conditions. If you disagree with any part of the terms, then you may not access the services.
        </li>
        <li>
            <strong>Content:</strong> All content provided through our services is for informational purposes only. We do not guarantee the accuracy, completeness, or usefulness of any information provided.
        </li>
        <li>
            <strong>User Conduct:</strong> You agree not to use the services for any unlawful purposes or to engage in any activity that disrupts the services or interferes with other users' ability to use the services.
        </li>
        <li>
            <strong>Privacy Policy:</strong> Our Privacy Policy outlines how we collect, use, and disclose your personal information. By using our services, you consent to the collection and use of your information as outlined in the Privacy Policy.
        </li>
        <li>
            <strong>Intellectual Property:</strong> All intellectual property rights related to the services are owned by us or our licensors. You may not use, reproduce, or distribute any content from the services without our permission.
        </li>
        <li>
            <strong>Limitation of Liability:</strong> We are not liable for any damages or losses arising from your use of the services or reliance on any information provided. In no event shall we be liable for any indirect, incidental, special, or consequential damages.
        </li>
        <li>
            <strong>Changes to Terms:</strong> We reserve the right to modify or update these Terms and Conditions at any time. Any changes will be effective immediately upon posting. It is your responsibility to review the Terms and Conditions periodically for changes.
        </li>
    </ul>
);
const headerText = (
    <>
    <p>Thank you for submitting this articles, your work is a great asset to the school!</p>
    <p>Before this article can be submitted for the Coodinator to see, there are a few Terms and Conditions:</p>
    </>
);
const initialValues = {
    title: "",
    description: "",
};
const userSchema = yup.object().shape({
    title: yup.string().required("required"),
    description: yup.string().required("required"),
});
export default function Article() {
    const navigator = useNavigate();
    const [selectedPhotos, setSelectedPhotos] = useState([]);
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const [tncOpen, setTnCOpen] = useState(false);
    const [startDate, setStartDate] = useState(); 
    const [endDate, setEndDate] = useState(); 
    const [remainingTime, setRemainingTime] = useState(0);

    const [currentUser, setCurrentUser] = useState(null);
    const [event, setEvent] = useState();

    const [isAnonymous, setIsAnonymous] = useState(false);
    useEffect(() => {
        const user = authService.getCurrentUser();
        if (user) {
            setCurrentUser(user);
        }
    }, []);
    useEffect(() => {
        const fetchEvent = async () => {
            if (currentUser) {
                try {
                    const response = await authHeader().get(apis.event + "student/" + currentUser.id);
                    setEvent(response.data);
                    setStartDate(response.data.startDate)
                    setEndDate(response.data.endDate)
                } catch (error) {
                    console.error("Error fetching event data:", error);
                }
            }
        };
        fetchEvent();
    }, [ currentUser]);
    useEffect(() => {
        const calculateRemainingTime = () => {
            const endDateConverted = new Date(endDate); 
            const currentDate = new Date();
            const timeDifference = endDateConverted.getTime() - currentDate.getTime();
            if (timeDifference >= 0) {
                // Calculate remaining time components
                const days = Math.floor(timeDifference / (1000 * 60 * 60 * 24));
                const hours = Math.floor((timeDifference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                const minutes = Math.floor((timeDifference % (1000 * 60 * 60)) / (1000 * 60));
                const seconds = Math.floor((timeDifference % (1000 * 60)) / 1000);
        
                // Format the remaining time into a readable string
                const remainingTimeStr = `${days} days, ${hours} hours, ${minutes} minutes, ${seconds} seconds`;
                setRemainingTime(remainingTimeStr);
            } else {
            setRemainingTime('Expired');
            }
        }
        calculateRemainingTime();

        // Update remaining time every second
        const interval = setInterval(calculateRemainingTime, 1000);
        return () => clearInterval(interval);
    }, [endDate]);

    const handleCloseTnCDialog = () => {
        setTnCOpen(false);
    };
    const handleOpenTnCDialog = () => {
        setTnCOpen(true);
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
    const handleSubmit = async(values) =>{
        setIsSubmitting(true);
        if(selectedPhotos.length === 0){
            setMessage("Please select a photo")
            setIsSubmitting(false)
            return
        }else if (selectedFiles.length === 0){
            setMessage("Please select a file")
            setIsSubmitting(false)
            return
        }else{
            const FormData = require('form-data');
            const formData = new FormData();
            formData.append('Title', values.title);
            formData.append('Description', values.description);
            formData.append('Date', new Date().toISOString()); 
            formData.append('FacultyId', currentUser.facultyId);
            formData.append('EventId',event.id );
            formData.append('StudentId', currentUser.id);
            formData.append('IsAnonymous', isAnonymous); 
            selectedPhotos.forEach((photo, index) => {
                formData.append(`ImageFiles`, photo, photo.name);
            });
            
            // Append document files
            selectedFiles.forEach((file, index) => {
                formData.append(`DocFiles`, file, file.name);
            });
            const url =  apis.article+"student"
    
            try{
                const res = await authHeader().post(url, formData);
                if (res.status === 200) {
                    setMessage("Account edited successfully.");
                    navigator("/");
                } else {
                    setIsSubmitting(false);
                    setMessage(`An error occurred: ${res.data}`);
                }
            }catch (error) {
                setIsSubmitting(false);
                console.log(error)
                setMessage(error.response.data);
            }
        }
    };

    return (
        <div className="article">
            <div className="articleWrapper">
                <h1>Submission</h1>
                <hr className="articleHr" />
                
                <div className="dateSection">
                    {startDate && endDate ? (
                        <>
                            <p>Event Name: {event.eventName}</p>
                            <p>Opened: {new Date(startDate).toLocaleDateString()} {new Date(startDate).toLocaleTimeString()}</p>
                            <p>Due: {new Date(endDate).toLocaleDateString()} {new Date(endDate).toLocaleTimeString()}</p>
                            {remainingTime === 'Expired' ? (
                                <p>This article is expired</p>
                            ) : (
                                <p>Time Remaining: {remainingTime}</p>
                            )}
                        </>
                    ) : (
                        <p>Loading...</p>
                    )}
                </div>
                <h2 className="createSubmission">Create a submission</h2>
                <Formik
                    onSubmit={handleSubmit}
                    initialValues={initialValues}
                    validationSchema={userSchema}
                >
                {({
                    values,
                    errors,
                    touched,
                    handleBlur,
                    handleChange,
                    handleSubmit,
                }) => (
                    <Form onSubmit={handleSubmit}>
                    {/* <Box
                        display="grid"
                        gap="4vh"
                        gridTemplateColumns="repeat(4, minmax(0, 1fr))"
                        sx={{
                        "& > div": { gridColumn: isNonMobile ? undefined : "span 4" },
                        }}
                    > */}
                    <div className="articleContent">
                        {/* <input type="text" className="articleTitle" placeholder="Title"/> */}
                        
                        <TextField
                            fullWidth
                            variant="filled"
                            type="text"
                            label="Title"
                            onBlur={handleBlur}
                            onChange={handleChange}
                            value={values.title}
                            name="title"
                            error={!!touched.title && !!errors.title}
                            helperText={touched.title && errors.title}
                            className="articleTitle"
                        />
                    <br />
                    {/* <textarea placeholder="Write your article?" className="articleInput"></textarea> */}
                    <CKEditor
                            editor={ClassicEditor}
                            data={values.description}
                            value={values.description}
                            onReady={(editor) => {
                                console.log('CKEditor React Component is ready to use!', editor);
                            }}
                            name="description"
                            helperText={touched.description && errors.description}
                            error={!!touched.description && !!errors.description}
                            onChange={(event, editor) => {
                                const data = editor.getData();
                                handleChange({ target: { name: 'description', value: data } });
                                console.log(data);
                            }}
                            className="articleTextEditer"
                    />
                    {!!touched.description && !!errors.description && (
                        <div style={{ color: 'red' }}>{errors.description}</div>
                    )}
                    {/* <TextField
                        fullWidth
                        variant="filled"
                        type="text"
                        label="Write your content!"
                        onBlur={handleBlur}
                        onChange={handleChange}
                        value={values.description}
                        name="description"
                        error={!!touched.description && !!errors.description}
                        helperText={touched.description && errors.description}
                        multiline
                        rows={9}
                        className="articleInput"
                        // sx={{ gridColumn: "span 2" }}
                    /> */}
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
                                termsAndConditionsText = {termsAndConditionsText}
                                headerText = {headerText}
                            />
                        </div>
                    </div>
                    <div className="articleError">
                        <span>{message}</span>
                    </div>
                    <div className="buttonContainer">
                        <FormControlLabel
                            control={<Checkbox checked={isAnonymous} onChange={(e) => setIsAnonymous(e.target.checked)} />}
                            label="Submit as Anonymous"
                        />
                        <Button type="submit" disabled={isSubmitting || remainingTime === 'Expired'}>
                            {isSubmitting ? <span>Loading...</span> : "Submit"}
                        </Button>
                    </div>
                    {/* </Box> */}
                </Form>
            )}
            </Formik>
            </div>
        </div>
    );
}
