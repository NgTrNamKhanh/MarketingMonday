import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    TextField,
    Typography,
    colors,
    useMediaQuery,
} from "@mui/material";
import { Form, Formik } from "formik";
import { useEffect, useState } from "react";
import * as yup from "yup";
import apis from "../../../services/apis.service";
import authHeader from "../../../services/auth.header";
import "./account.css"
// import authHeader from "../../services/auth-header";
const initialValues = {
    password: "",
    confirm_password: "",
};



const PasswordForm = ({
    email,
    handleClose,
}) => {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const isNonMobile = useMediaQuery("(min-width:60vh)");

    const userSchema = yup.object().shape({
        password:  yup.string().required("This field must not be empty"),
        confirm_password: 
                yup
                .string()
                .required("This field must not be empty")
                .oneOf([yup.ref("password"), null], "Confirm password does not match"),
    });

    const handleSubmit = async (values) => {
        try {
            setIsSubmitting(true);
                const url = apis.user+"changePassword"
                const res = await authHeader().put(url, {
                    params: { email: email , password: values.password},
                    withCredentials: true,
                });
                if (res.status === 200) {
                    setIsSubmitting(false);
                    setMessage("Account edited successfully.");
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

    return (
        <Dialog open={true} onClose={handleClose}>
        <DialogTitle>Change password</DialogTitle>
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
                        type="password"
                        label="Password"
                        onBlur={handleBlur}
                        onChange={handleChange}
                        value={values.password}
                        name="password"
                        error={!!touched.password && !!errors.password}
                        helperText={
                        touched.password && (
                            <span className="validation-error">{errors.password}</span>
                        )
                        }
                        sx={{ gridColumn: "span 2" }}
                    />
                    <TextField
                        fullWidth
                        variant="filled"
                        type="password"
                        label="Confirm Password"
                        onBlur={handleBlur}
                        onChange={handleChange}
                        value={values.confirm_password}
                        name="confirm_password"
                        error={!!touched.confirm_password && !!errors.confirm_password}
                        helperText={
                        touched.confirm_password && (
                            <span className="validation-error">
                            {errors.confirm_password}
                            </span>
                        )
                        }
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
                    {isSubmitting ? <span>Loading...</span> : 'Change'}
                    </Button>
                </DialogActions>
                </Form>
            )}
            </Formik>
        </DialogContent>
        </Dialog>
    );
};
export default PasswordForm;
