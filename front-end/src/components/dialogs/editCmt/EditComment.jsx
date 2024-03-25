import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography, colors, useMediaQuery } from '@mui/material'
import { Form, Formik } from 'formik';
import React, { useState } from 'react'
import * as yup from "yup";
import authHeader from '../../../services/auth.header';
import apis from '../../../services/apis.service';
const initialValues = {
    comment: "",
};
export default function EditComment({open, handleClose, comment}) {
    const [loading, setLoading] = useState(false);
    initialValues.comment = comment ? comment.content : "";
    const userSchema = yup.object().shape({
        comment: yup.string().required("required"),
    });
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const handleSubmit = async (values) => {
        const editComment = {
            content: values.comment,
            userId: comment.userId,
            articleId: comment.articleId,
        };
        console.log(editComment);
        try {
            
            setIsSubmitting(true);
            
                const url = apis.comment;
                const res = await authHeader().put(url, editComment);
                if (res.status === 200) {
                    // const updatedData = await reFetch();
                    setIsSubmitting(false);
                    setMessage("Comment edit successfully.");
                    handleClose();
                } else {
                    setIsSubmitting(false);
                    setMessage(`An error occurred: ${res.data}`);
                }
        
        } catch (error) {
        setIsSubmitting(false);
        setMessage(error.response.data);
        }
    };
    const isNonMobile = useMediaQuery("(min-width:60vh)");
    return (
    <Dialog open={open} onClose={handleClose}>
    <DialogTitle ><h2>Edit comment</h2></DialogTitle>
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
                    "& > div": { gridColumn: isNonMobile ? undefined : "span 4" },
                    }}
                >
                    <TextField
                    fullWidth
                    variant="filled"
                    type="text"
                    label="Comment"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.comment}
                    name="comment"
                    error={!!touched.comment && !!errors.comment}
                    helperText={touched.comment && errors.comment}
                    sx={{ gridColumn: "span 2" }}
                    />
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
                        {isSubmitting ? <span>Loading...</span> : "Edit"}
                    </Button>
                </DialogActions>
                </Form>
            )}
            </Formik>
        </DialogContent>
</Dialog>
  )
}
