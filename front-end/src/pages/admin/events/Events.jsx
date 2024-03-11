import EventForm from '../../../components/forms/event/Event';
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import './events.css'
import { Delete, EditOutlined, Visibility } from '@mui/icons-material';
import { useState } from 'react';
import { Box, useTheme } from "@mui/material";
import { Link } from 'react-router-dom';
const data = [
    {
        id: 1,
        eventName: 'Marketing Essential',
        startDate: "February 20, 2024",
        endDate: 'February 29, 2024',
        faculty: "Marketing"
    },
    {
        id: 2,
        eventName: 'Loser Fest',
        startDate: "February 20, 2024",
        endDate: 'February 29, 2024',
        faculty: "IT"
    },
    {
        id: 3,
        eventName: 'Ass>Boob',
        startDate: "May 20, 2024",
        endDate: 'May 29, 2024',
        faculty: "Marketing"
    },
];
const Events = () => {
    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
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
    const handleOpenDeleteDialog = (acc) => {
        selectedEvent(acc);
        setDeleteDialogOpen(true);
    };
    const handleDefaultCloseEditDialog = () => {
        setEditDialogOpen(false);
        };

    const handleCloseDeleteDialog = () => {
        setToastMessage("Event deleted successfully");
        setDeleteDialogOpen(false);
        selectedEvent(null);
        setShowToast(true);
    };
    const columns = [
        {
            field: "eventName",
            headerName: "eventName",
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
        },
        {
            field: "endDate",
            headerName: "End Date",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "actions",
            headerName: "Actions",
            flex: 2,
            cellClassName: "data-cell",
            renderCell: ({ row }) => (
            <Box p="1vh" display="flex" justifyContent="center">
                <Link
                onClick={() => handleOpenEditDialog(row)}
                style={{ marginRight: "2vh" }}
                >
                <EditOutlined />
                </Link>
                <Link>
                <Delete onClick={() => handleOpenDeleteDialog(row)} />
                </Link>
            </Box>
            ),
            headerClassName: "header-text",
        },
        ];
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
                    // loading={loading}
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
                        // reFetch={reFetch}
                    />
                )}
                </div>
            </div>
        </div>
    )
}
export default Events;