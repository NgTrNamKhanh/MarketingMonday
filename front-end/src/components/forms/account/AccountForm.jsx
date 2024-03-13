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
import axios from "axios";
import { Form, Formik } from "formik";
import { useEffect, useState } from "react";
import * as yup from "yup";
import apis from "../../../services/apis.service";
// import authHeader from "../../services/auth-header";
const initialValues = {
    firstName: "",
    lastName: "",
    password: "",
    confirm_password: "",
    email: "",
    contactNumber: "",
    role: "",
    faculty: "",
};

const phoneRegExp =
/^((\+[1-9]{1,4}[ -]?)|(\([0-9]{2,3}\)[ -]?)|([0-9]{2,4})[ -]?)*?[0-9]{3,4}[ -]?[0-9]{3,4}$/;


const AccountForm = ({
    handleCloseDialog,
    account,
    reFetch,
    handleDefaultCloseEditDialog,
}) => {
    const isEdit = account? true: false;
    initialValues.firstName = account ? account.firstName : "";
    initialValues.lastName = account ? account.lastName : "";
    initialValues.email = account ? account.email : "";
    initialValues.contactNumber = account ? account.contact_number : "";
    const [faculty, setFaculty] = useState(account ? account.faculty : "");
    const [facultyOptions, setFacultyOptions] = useState([]);
    const [role, setRole] = useState(account ? account.role : "");
    const [roleOptions, setRoleOptions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const isNonMobile = useMediaQuery("(min-width:60vh)");


    const userSchema = yup.object().shape({
        firstName: yup.string().required("required"),
        lastName: yup.string().required("required"),
        password: isEdit ? yup.string() : yup.string().required("This field must not be empty"),
        confirm_password: isEdit 
            ? yup.string() 
            : yup
                .string()
                .required("This field must not be empty")
                .oneOf([yup.ref("password"), null], "Confirm password does not match"),
        contactNumber: yup.string().required("required").matches(phoneRegExp, "Phone number is not valid"),
        email: yup.string().required("required").email("Please enter an email"),
    });


    const fetchRolesAndFaculties = async () => {
        setLoading(true);
        try {
            const rolesResponse = await axios.get(apis.role, {
                // headers: authHeader(),
                withCredentials: true,
            });
            const facultiesResponse = await axios.get(
                apis.faculty
            );
            console.log(facultiesResponse)
            console.log(rolesResponse)

        
            setFacultyOptions(facultiesResponse.data);
            localStorage.setItem("faculties", JSON.stringify(facultiesResponse.data)
            );
            setRoleOptions(rolesResponse.data);
            localStorage.setItem("roles", JSON.stringify(rolesResponse.data));
            setLoading(false);
            } catch (error) {
            console.error("Error fetching roles and faculties:", error);
            setMessage("Error fetching roles and faculties");
            setLoading(false);
            }
        } ;
        console.log(facultyOptions)
    useEffect(() => {
        const initializeData = async () => {
            const faLocal = localStorage.getItem("faculties");
            const roleLocal = localStorage.getItem("roles");
            try {
                if (
                    faLocal &&
                    JSON.parse(faLocal).length !== 0 &&
                    roleLocal &&
                    JSON.parse(roleLocal).length !== 0
                ) {
                    const facultiesFromStorage = JSON.parse(faLocal);
                const rolesFromStorage = JSON.parse(roleLocal);
                    setFacultyOptions(facultiesFromStorage);
                    setRoleOptions(rolesFromStorage);
                    setLoading(false);
                } else {
                    await fetchRolesAndFaculties();
                }
            } catch (error) {
                console.error("Error initializing data:", error);
                setMessage("Error initializing data");
            }
        
        }
        initializeData();
    }, []);
    const handleSubmit = async (values) => {
        const account = {
            firstName: values.firstName,
            lastName: values.lastName,
            email: values.email,
            contactNumber: values.contactNumber,
            password: values.password,
            role: role,
            facultyId: faculty
        };
        console.log(account);

        try {
        setIsSubmitting(true);
        const url = apis.admin+"createAccount";
        const res = await axios.post(url, account, {
            // headers: authHeader(),
            withCredentials: true,
        });
        if (res.status === 200) {
            const updatedData = await reFetch();
            localStorage.setItem("accounts", JSON.stringify(updatedData));
            setIsSubmitting(false);
            setMessage("Account update successfully.");
            handleCloseDialog();
        } else {
            setIsSubmitting(false);
            setMessage(`An error occurred: ${res.data}`);
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
    return (
        <Dialog open={true} onClose={handleDefaultCloseEditDialog}>
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
                                <MenuItem key={fa.facultyId} value={fa.facultyId}>
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
