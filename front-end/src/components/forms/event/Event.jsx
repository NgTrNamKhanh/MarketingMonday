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
    FormControl,
    InputLabel,
    Select,
} from "@mui/material";
import { Form, Formik } from "formik";
import * as yup from "yup";
import apis from '../../../services/apis.service';
import axios from 'axios';
const initialValues = {
    eventName: "",
};
const userSchema = yup.object().shape({
    eventName: yup.string().required("required"),
});
const EventForm = ({
    handleCloseDialog,
    event,
    reFetch,
    handleDefaultCloseEditDialog,
    facultyOptions,
}) => {
    const isEdit = event? true: false;
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [message, setMessage] = useState("");
    const isNonMobile = useMediaQuery("(min-width:60vh)");
    initialValues.eventName = event ? event.eventName : "";
    const [faculty, setFaculty] = useState(event ? event.facultyId : "");
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
        }
    }, [event]);

    const handleFacultyChange = (e) => {
        setFaculty(e.target.value);
    };

    const handleSubmit = async (values) => {
        const eventSubmit = {
            eventName: values.eventName,
            startDate: dateRange[0].startDate,
            endDate: dateRange[0].endDate,
            facultyId: faculty
        };

        try {
            
            setIsSubmitting(true);
            
            if(!isEdit){
                const url = apis.event;
                const res = await axios.post(url, eventSubmit, {
                    // headers: authHeader(),
                    withCredentials: true,
                });
                if (res.status === 200) {
                    const updatedData = await reFetch();
                    localStorage.setItem("events", JSON.stringify(updatedData));
                    setIsSubmitting(false);
                    setMessage("Event added successfully.");
                    handleCloseDialog();
                } else {
                    setIsSubmitting(false);
                    setMessage(`An error occurred: ${res.data}`);
                }
            }else{
                const url = `${apis.event}${event.id}`
                const res = await axios.put(url, eventSubmit, {
                    // headers: authHeader(),
                    withCredentials: true,
                });
                if (res.status === 200) {
                    const updatedData = await reFetch();
                    localStorage.setItem("events", JSON.stringify(updatedData));
                    setIsSubmitting(false);
                    setMessage("Event edited successfully.");
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
    return (
        <Dialog open={true} onClose={handleDefaultCloseEditDialog}>
        <DialogTitle>{isEdit ? 'Edit Event' : 'Add Event'}</DialogTitle>
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
                    name="eventName"
                    error={!!touched.eventName && !!errors.eventName}
                    helperText={touched.eventName && errors.eventName}
                    sx={{ gridColumn: "span 2" }}
                    />
                    <FormControl sx={{  gridColumn: "span 2"  }}>
                    <InputLabel>Faculty</InputLabel>
                        <Select
                            value={faculty}
                            onChange={(e) => handleFacultyChange(e)}
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

export default EventForm;
