import { useEffect, useState } from 'react';
import { DateRangePicker  } from 'react-date-range';
import 'react-date-range/dist/styles.css';
import 'react-date-range/dist/theme/default.css';
import './event.css'; 
import { format } from "date-fns";
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
    MenuItem,
} from "@mui/material";
import { Form, Formik } from "formik";
import * as yup from "yup";
const initialValues = {
    eventName: "",
};
const userSchema = yup.object().shape({
    // eventName: yup.string().required("required"),
});
const facultyOptions = [
    {
        id: 1,
        name: 'Marketing',
    },
    {
        id: 2,
        name: 'IT',
    }
]
const EventForm = ({
    handleCloseDialog,
    event,
    // reFetch,
    handleDefaultCloseEditDialog,
}) => {
    console.log(event)
    const isEdit = event? true: false;
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const isNonMobile = useMediaQuery("(min-width:60vh)");
    initialValues.eventName = event ? event.eventName : "";
    const [faculty, setFaculty] = useState('');
    
    
    console.log(faculty)
    const [openDate, setOpenDate] = useState(false);
    const [dateRange, setDateRange] = useState([
        {
        startDate: new Date(),
        endDate: new Date(),
        key: 'selection',
        },
    ]);

    useEffect(() => {
        if(event){
            setDateRange([{
                startDate: new Date(event.startDate),
                endDate: new Date(event.endDate),
                key: 'selection',
            }])
            setFaculty(event.faculty); 
        }
    }, [event]);

    const handleFacultyChange = (e) => {
        setFaculty(e.target.value);
    };

    const handleSubmit = (e) => {
        setIsSubmitting(true)
        e.preventDefault();
        console.log("Event Name:", e.eventName);
        console.log("Faculty:", faculty);
        console.log("Date Range:", dateRange);
        setIsSubmitting(false);
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
                    label="Event Name"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.eventName}
                    name="firstName"
                    error={!!touched.eventName && !!errors.eventName}
                    helperText={touched.eventName && errors.eventName}
                    sx={{ gridColumn: "span 2" }}
                    />
                    <TextField
                        fullWidth
                        variant="filled"
                        select
                        label="Faculty"
                        onBlur={handleBlur}
                        onChange={handleFacultyChange}
                        value={faculty}
                        name="faculty"
                        sx={{ minWidth: 80 , gridColumn: 'span 2 '}}
                        >
                        <MenuItem value="" disabled>
                            <em>Select a faculty</em>
                        </MenuItem>
                        {facultyOptions.map((faculty) => (
                            <MenuItem key={faculty} value={faculty.name}>
                            {faculty.name}
                            </MenuItem>
                        ))}
                    </TextField>
                    <div className="dateRangeContainer">
                        <div className="dateRangeTitle">
                            <label htmlFor="">Date Range:</label>
                        </div>
                        <span
                            onClick={() => setOpenDate(!openDate)}
                            className="dateRangeText"
                            >{`${format(dateRange[0].startDate, "MM/dd/yyyy")} to ${format(
                                dateRange[0].endDate,
                                "MM/dd/yyyy"
                            )}`}</span>
                        {openDate && (
                            <DateRangePicker
                            editableDateInputs={true}
                            onChange={(item) => setDateRange([item.selection])}
                            moveRangeOnFirstSelection={false}
                            ranges={dateRange}
                            className="datePicker"
                            // minDate={new Date()}
                            />
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

export default EventForm;
