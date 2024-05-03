import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography, colors } from '@mui/material'
import { Form, Formik } from 'formik'
import React, { useState } from 'react'
import apis from '../../../services/apis.service';
import authHeader from '../../../services/auth.header';
import * as yup from "yup";
import { jwtDecode } from 'jwt-decode';
import Cookies from 'universal-cookie';
const initialValues = {
    
};
export default function AvatarForm({userId, handleClose}) {
    const userSchema = yup.object().shape({
    });
    const [selectedImage, setSelectedImage] = useState();
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const handleImageChange = (e) => {
        setSelectedImage(e.target.files[0])
    }
    const handleRemovePhoto = () => {
        setSelectedImage(null)
    }
    const handleSubmit = async() => {
        const FormData = require('form-data');
        const formData = new FormData();
        formData.append('avatarImage', selectedImage);
        try {
            setIsSubmitting(true);
            const url = apis.user+"ChangeAvatar"
            const res = await authHeader().put(url, formData, {
                params: { userId: userId },
                withCredentials: true,
            });
            if (res.status === 200) {
                const cookies = new Cookies();
                const user = cookies.get('CMU-user'); 
            
                if (user) {
            
                    if (user && user.avatar) {
                        user.avatar = res.data; 
                        cookies.remove('CMU-user'); 
                        const decodedToken = jwtDecode(user.jwt_token);
                        cookies.set("CMU-user", JSON.stringify(user), {
                            path: "/",
                            expires: new Date(decodedToken.exp * 1000)
                        });
                        window.location.reload();
                    }
                }
            
                setIsSubmitting(false);
                setMessage("Account edited successfully.");
                handleClose();
            }
            else {
                setIsSubmitting(false);
                console.log(res.data)
                setMessage(`An error occurred: ${res.data}`);
            }
        }catch (error) {
            console.log(error)
            setIsSubmitting(false);
            setMessage(error.response.data);
            }
    }
    
    return (
    <Dialog open={true} onClose={handleClose}>
        <DialogTitle>Change your avatar</DialogTitle>
        <DialogContent>
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
                <Box
                    display="grid"
                    gap="4vh"
                    gridTemplateColumns="repeat(4, minmax(0, 1fr))"
                    sx={{
                    "& > div": { gridColumn: "span 4" },
                    }}
                >
                    <div className="photoInput">
                        <h2 className="photoInputLabel">Avatar</h2>
                        {selectedImage && (
                            <div className="photoInputContainer">
                                <img src={URL.createObjectURL(selectedImage)} className="photoInputImg"/>
                                <a href={URL.createObjectURL(selectedImage)} disabled={isSubmitting} className="photoInputLink" target="_blank" rel="noopener noreferrer">{selectedImage.name}</a>
                                <button disabled={isSubmitting} className="removeButton" onClick={() => handleRemovePhoto()}>X</button>
                            </div>
                        )}
                        {!selectedImage && (
                            <div className="avatar-upload-form">
                            <input
                            type="file"
                            className="avatar-input"
                            accept="image/*"
                            onBlur={handleBlur}
                            onChange={handleImageChange}
                            disabled={isSubmitting}
                            sx={{ gridColumn: "span 2" }}
                            />
                        </div>
                        )}
                    </div>
                    
                </Box>

                <DialogActions>
                    <Typography
                    sx={{ color: colors.deepOrange[700], fontSize: "1.6vh" }}
                    >
                    {message}
                    </Typography>
                    <Button
                        onClick={handleClose}
                        disabled={isSubmitting}
                    >
                    Cancel
                    </Button>
                    <Button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? <span>Loading...</span> : 'Change'}
                    </Button>
                </DialogActions>
                </Form>
            )}
            </Formik>
        </DialogContent>
        </Dialog>
  )
}
