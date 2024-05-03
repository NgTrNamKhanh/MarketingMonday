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
    firstName: "",
    lastName: "",
    password: "",
    confirm_password: "",
    email: "",
    phoneNumber: "",
    role: "",
    faculty: "",
};

const phoneRegExp =
/^((\+[1-9]{1,4}[ -]?)|(\([0-9]{2,3}\)[ -]?)|([0-9]{2,4})[ -]?)*?[0-9]{3,4}[ -]?[0-9]{3,4}$/;


const AccountForm = ({
    handleCloseDialog,
    account,
    open,
    reFetch,
    handleDefaultCloseEditDialog,
    facultyOptions,
    roleOptions
}) => {
    const [isEdit, setIsEdit] = useState(false);
    const [faculty, setFaculty] = useState("");
    const [role, setRole] = useState("");
    useEffect(() => {
        if (account) {
            setIsEdit(true);
            initialValues.firstName = account.firstName || "";
            initialValues.lastName = account.lastName || "";
            initialValues.email = account.email || "";
            initialValues.phoneNumber = account.phoneNumber || "";
            setFaculty(account.facultyId || "");
            setRole(account.role[0] || "");
        } else {
            setIsEdit(false);
        }
    }, [account]);
    // const [loading, setLoading] = useState(true);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const isNonMobile = useMediaQuery("(min-width:60vh)");
    // const [selectedImage, setSelectedImage] = useState(null);
    // const handleImageChange = (e) => {
    //     setSelectedImage(e.target.files[0]);
    // };
    // const handleRemovePhoto = () =>{
    //     setSelectedImage(null)
    // }
    const userSchema = yup.object().shape({
        firstName: yup.string().required("required"),
        lastName: yup.string().required("required"),
        password: isEdit 
            ? yup.string() 
            : yup.string()
                .required("This field must not be empty")
                .matches(
                    /^(?=.*[!@#$%^&*])(?=.*[A-Z])(?=.*\d).*$/,
                    "Password must contain at least one symbol, one capital letter, and one number"
                ).min(6, "Password must be at least 6 characters long"),
                confirm_password: isEdit 
                ? yup.string() 
                : yup.string()
                .required("This field must not be empty")
                .oneOf([yup.ref("password"), null], "Confirm password does not match"),
        phoneNumber: yup.string().required("required").matches(phoneRegExp, "Phone number is not valid"),
        email: yup.string().required("required").email("Please enter an email"),
    });
    
    

    const handleSubmit = async (values) => {
        const FormData = require('form-data');
        const formData = new FormData();
        formData.append('FirstName', values.firstName);
        formData.append('LastName', values.lastName);
        formData.append('Email', values.email);
        formData.append('PhoneNumber', values.phoneNumber); 
        formData.append('Password', values.password);
        formData.append('FacultyId', faculty );
        formData.append('Role', role);
        // if(selectedImage){
        //     formData.append("AvatarImageFile", selectedImage);
        // }
        try {
            setIsSubmitting(true);
            if(!isEdit){
                const url = apis.admin+"createAccount";
                const res = await authHeader().post(url, formData, {});
                if (res.status === 200) {
                    const updatedData = await reFetch();
                    localStorage.setItem("accounts", JSON.stringify(updatedData));
                    setIsSubmitting(false);
                    setMessage("Account added successfully.");
                    handleCloseDialog();
                } else {
                    setIsSubmitting(false);
                    setMessage(`An error occurred: ${res.data}`);
                }
            }else{
                const url = apis.admin+"account"
                const res = await authHeader().put(url, formData, {
                    params: { userId: account.id },
                    // headers: authHeader(),
                    withCredentials: true,
                });
                if (res.status === 200) {
                    const updatedData = await reFetch();
                    localStorage.setItem("accounts", JSON.stringify(updatedData));
                    setIsSubmitting(false);
                    setMessage("Account edited successfully.");
                    handleCloseDialog();
                } else {
                    setIsSubmitting(false);
                    setMessage(`An error occurred: ${res.data}`);
                }
            }
        
        } catch (error) {
        setIsSubmitting(false);
        setMessage(error.response.data);
        }
    };
    const onChangeFaculty = (e) => {
        setFaculty(e.target.value);
    };
    const onChangeRole = (e) => {
        setRole(e.target.value);
    };
    console.log(roleOptions)
    return (
        <Dialog open={open} onClose={handleDefaultCloseEditDialog}>
        <DialogTitle>{isEdit ? 'Edit Account' : 'Add Account'}</DialogTitle>
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
                    {/* <div className="photoInput">
                        <h2 className="photoInputLabel">Avatar</h2>
                        {selectedImage && (
                            <div className="photoInputContainer">
                                <img src={URL.createObjectURL(selectedImage)} className="photoInputImg"/>
                                <a href={URL.createObjectURL(selectedImage)} className="photoInputLink" target="_blank" rel="noopener noreferrer">{selectedImage.name}</a>
                                <button className="removeButton" onClick={() => handleRemovePhoto()}>X</button>
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
                            sx={{ gridColumn: "span 2" }}
                            />
                        </div>
                        )}
                    </div> */}
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
                    <TextField
                    fullWidth
                    variant="filled"
                    type="text"
                    label="Mobile"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.phoneNumber}
                    name="phoneNumber"
                    error={!!touched.phoneNumber && !!errors.phoneNumber}
                    helperText={touched.phoneNumber && errors.phoneNumber}
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
                    <FormControl sx={{  gridColumn: "span 2"  }}>
                        <InputLabel>Role</InputLabel>
                        <Select
                            value={role}
                            onChange={(e) => onChangeRole(e)}
                            autoWidth
                            label="role"
                            required
                        >
                        <MenuItem
                                value=""
                                disabled
                                // aria-required={!!formErrors.faculty}
                            >
                                <em>Select an Role</em>
                            </MenuItem>
                            {roleOptions.map((role) => (
                                <MenuItem key={role.id} value={role.name}>
                                    {role.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                    <FormControl sx={{  gridColumn: "span 2"  }}>
                    <InputLabel>Faculty</InputLabel>
                        <Select
                            value={faculty}
                            onChange={(e) => onChangeFaculty(e)}
                            autoWidth
                            label="faculty"
                            required
                        >
                            <MenuItem
                                value=""
                                disabled
                                // aria-required={!!formErrors.faculty}
                            >
                                <em>Select an faculty</em>
                            </MenuItem>
                            {facultyOptions.map((fa) => (
                                <MenuItem key={fa.id} value={fa.id}>
                                {fa.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                    
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
                    {isSubmitting ? <span>Loading...</span> : (isEdit ? 'Edit' : 'Add')}
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
