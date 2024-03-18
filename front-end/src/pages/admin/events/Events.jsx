import EventForm from '../../../components/forms/event/Event';
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import './events.css'
import { Delete, EditOutlined, Visibility } from '@mui/icons-material';
import { useEffect, useState } from 'react';
import { Box, useTheme } from "@mui/material";
import { Link } from 'react-router-dom';
import useFetch from '../../../hooks/useFetch';
import apis from '../../../services/apis.service';
import authHeader from '../../../services/auth.header';
// const data = [
//     {
//         id: 1,
//         eventName: 'Marketing Essential',
//         startDate: "February 20, 2024",
//         endDate: 'February 29, 2024',
//         faculty: "Marketing"
//     },
//     {
//         id: 2,
//         eventName: 'Loser Fest',
//         startDate: "February 20, 2024",
//         endDate: 'February 29, 2024',
//         faculty: "IT"
//     },
//     {
//         id: 3,
//         eventName: 'Ass>Boob',
//         startDate: "May 20, 2024",
//         endDate: 'May 29, 2024',
//         faculty: "Marketing"
//     },
// ];
const Events = () => {
    const {data, loading, error, reFetch} = useFetch(apis.event) 
    console.log(data)
    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    // const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const handleOpenEditDialog = (event) => {
        setSelectedEvent(event);
        setEditDialogOpen(true);
    };

    const handleCloseEditDialog = () => {
        setToastMessage("Event updated successfully");
        setEditDialogOpen(false);
        setSelectedEvent(null);
        setShowToast(true);
    };
    // const handleOpenDeleteDialog = (acc) => {
    //     selectedEvent(acc);
    //     setDeleteDialogOpen(true);
    // };
    const handleDefaultCloseEditDialog = () => {
        setEditDialogOpen(false);
        };

    // const handleCloseDeleteDialog = () => {
    //     setToastMessage("Event deleted successfully");
    //     setDeleteDialogOpen(false);
    //     selectedEvent(null);
    //     setShowToast(true);
    // };
    const columns = [
        {
            field: "eventName",
            headerName: "Event Name",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "startDate",
            headerName: "Start Date",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
            renderCell: (params) => (
                <span className="data-cell">
                    {new Date(params.row.startDate).toLocaleDateString()} {new Date(params.row.startDate).toLocaleTimeString()}
                </span>
            ),
        },
        {
            field: "endDate",
            headerName: "End Date",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
            renderCell: (params) => (
                <span className="data-cell">
                    {new Date(params.row.endDate).toLocaleDateString()} {new Date(params.row.endDate).toLocaleTimeString()}
                </span>
            ),
        },
        
        {
            field: "facultyId",
            headerName: "Faculty",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
            valueGetter: (params) => {
                const facultyId = params.row.facultyId;
                const faculty = facultyOptions.find((faculty) => faculty.id === facultyId);
                return faculty ? faculty.name : "";
            },
        },
        {
            field: "actions",
            headerName: "Actions",
            flex: 1,
            cellClassName: "data-cell",
            renderCell: ({ row }) => (
            <Box p="1vh" display="flex" justifyContent="center">
                <Link
                onClick={() => handleOpenEditDialog(row)}
                style={{ marginRight: "2vh" }}
                >
                <EditOutlined />
                </Link>
                {/* <Link>
                <Delete onClick={() => handleOpenDeleteDialog(row)} />
                </Link> */}
            </Box>
            ),
            headerClassName: "header-text",
        },
    ];
    const [facultyOptions, setFacultyOptions] = useState([]);
    const fetchFaculties = async () => {
        // setLoading(true);
        try {

            const facultiesResponse = await authHeader().get(
                apis.faculty
            );
            localStorage.setItem("faculties", JSON.stringify(facultiesResponse.data)
            );
            // setLoading(false);
        } catch (error) {
            console.error("Error fetching faculties:", error);
            // setMessage("Error fetching roles and faculties");
            // setLoading(false);
        }
    } ;
    useEffect(() => {
        const initializeData = async () => {
            const faLocal = localStorage.getItem("faculties");
            try {
                if (
                    faLocal &&
                    JSON.parse(faLocal).length !== 0
                ) {
                    const facultiesFromStorage = JSON.parse(faLocal);
                    setFacultyOptions(facultiesFromStorage);
                } else {
                    await fetchFaculties();
                }
            } catch (error) {
                console.error("Error initializing data:", error);
                // setMessage("Error initializing data");
            }
        
        }
        initializeData();
    }, []);
    return (
        <div className="events">
            <div className="eventsWrapper">
                <div className="eventsTitle">
                    <h1>Events</h1>
                </div>
                <button className="addButton" onClick={() => handleOpenEditDialog()}>Add Event</button>
                <div className="eventsTable">
                    <DataGrid
                    rows={data}
                    columns={columns}
                    loading={loading}
                    checkboxSelection
                    disableRowSelectionOnClick
                    components={{ Toolbar: GridToolbar }}
                    sx={{
                        "& .MuiDataGrid-root": {
                            border: "none",
                        },
                        "& .MuiDataGrid-cell": {
                            borderBottom: "none",
                        },
                        "& .name-column--cell": {
                            color: 'white',
                        },
                        "& .MuiDataGrid-columnHeaders": {
                            backgroundColor: '#5D54A4',
                            borderBottom: "none",
                        },
                        "& .MuiDataGrid-virtualScroller": {
                            backgroundColor: 'white',
                        },
                        "& .MuiDataGrid-footerContainer": {
                            borderTop: "none",
                            backgroundColor: '#5D54A4',
                        },
                        "& .MuiDataGrid-toolbarContainer .MuiButton-text": {
                            color: `black !important`,
                        },
                        "& .css-hia42h-MuiCircularProgress-root": {
                            color: `black !important`,
                        },
                    }}

                    />
                {/* </Box> */}
                {/* <CustomDialog
                    open={deleteDialogOpen}
                    handleClose={handleDefaultCloseDeleteDialog}
                    handleConfirm={handleDelete}
                    title="Delete Organization"
                    description="Are you sure you want to delete this organization?"
                />*/}
                {editDialogOpen && (
                    <EventForm
                        handleCloseDialog={handleCloseEditDialog}
                        handleDefaultCloseEditDialog={handleDefaultCloseEditDialog}
                        event={selectedEvent}
                        reFetch={reFetch}
                        facultyOptions={facultyOptions}
                    />
                )}
                </div>
            </div>
        </div>
    )
}
export default Events;