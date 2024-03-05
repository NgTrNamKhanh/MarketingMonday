import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
    Typography,
    colors,
    useMediaQuery,
} from "@mui/material";
import axios from "axios";
import { Form, Formik } from "formik";
import { useState } from "react";
import * as yup from "yup";
// import authHeader from "../../services/auth-header";
const initialValues = {
    firstName: "",
    lastName: "",
    email: "",
    contactNumber: "",
};
const phoneRegExp =
/^((\+[1-9]{1,4}[ -]?)|(\([0-9]{2,3}\)[ -]?)|([0-9]{2,4})[ -]?)*?[0-9]{3,4}[ -]?[0-9]{3,4}$/;
const userSchema = yup.object().shape({
    firstName: yup.string().required("required"),
    lastName: yup.string().required("required"),
    contactNumber: yup.string().matches(phoneRegExp, "Phone number is not valid"),
    email: yup.string().email("Please enter an email"),
});

const AccountForm = ({
    handleCloseDialog,
    account,
    // reFetch,
    handleDefaultCloseEditDialog,
}) => {
    initialValues.firstName = account.firstName || "";
    initialValues.lastName = account.lastName || "";
    initialValues.email = account.email || "";
    initialValues.contactNumber = account.contact_number || "";
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const isNonMobile = useMediaQuery("(min-width:60vh)");

    const handleSubmit = async (values) => {
        const account = {
            firstName: values.firstName,
            lastName: values.lastName,
            email: values.email,
            contactNumber: values.contactNumber,
        };
        console.log(account);

        try {
        setIsSubmitting(true);
        // const url = "http://localhost:8449/api/organisation/organisation";
        // const res = await axios.put(url, account, {
        //     headers: authHeader(),
        //     withCredentials: true,
        // });
        // if (res.status === 200) {
        //     const updatedData = await reFetch();
        //     localStorage.setItem("accounts", JSON.stringify(updatedData));
        //     setIsSubmitting(false);
        //     setMessage("Account update successfully.");
        //     handleCloseDialog();
        // } else {
        //     setIsSubmitting(false);
        //     setMessage(`An error occurred: ${res.data}`);
        // }
        } catch (error) {
        setIsSubmitting(false);
        setMessage(error.response.data);
        }
    };

    return (
        <Dialog open={true} onClose={handleDefaultCloseEditDialog}>
        <DialogTitle>Edit Account</DialogTitle>
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
                    label="First Name"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.firstName}
                    name="firstName"
                    error={!!touched.firstName && !!errors.firstName}
                    helperText={touched.firstName && errors.firstName}
                    sx={{ gridColumn: "span 2" }}
                    />
                    <TextField
                    fullWidth
                    variant="filled"
                    type="text"
                    label="Last Name"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.lastName}
                    name="lastName"
                    error={!!touched.lastName && !!errors.lastName}
                    helperText={touched.lastName && errors.lastName}
                    sx={{ gridColumn: "span 2" }}
                    />
                    <TextField
                    fullWidth
                    variant="filled"
                    type="text"
                    label="Mobile"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.contactNumber}
                    name="contactNumber"
                    error={!!touched.contactNumber && !!errors.contactNumber}
                    helperText={touched.contactNumber && errors.contactNumber}
                    sx={{ gridColumn: "span 2" }}
                    />
                    
                    <TextField
                    fullWidth
                    variant="filled"
                    type="text"
                    label="Email"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.email}
                    name="email"
                    error={!!touched.email && !!errors.email}
                    helperText={touched.email && errors.email}
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
                        onClick={handleDefaultCloseEditDialog}
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
    );
};

export default AccountForm;
