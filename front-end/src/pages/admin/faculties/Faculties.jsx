import React, { useState } from "react";
import "./faculties.css";
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import { Link } from "react-router-dom";
import { Delete, EditOutlined, Visibility } from "@mui/icons-material";
import { Box, useTheme } from "@mui/material";

const data = [
    {
        id: 1,
        name: "Marketing",
    },
    {
        id: 2,
        name: "Design",
    },
    {
        id: 3,
        name: "Information Technology",
    },
];

const Faculties = () => {

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [detailDialogOpen, setDetailDialogOpen] = useState(false);
    const [selectedOrgDetails, setSelectedOrgDetails] = useState(null);

    const handleOpenDetailsDialog = (org) => {
        setSelectedOrgDetails(org);
        setDetailDialogOpen(true);
    };

    const handleCloseDetailsDialog = () => {
        setDetailDialogOpen(false);
        setSelectedOrgDetails(null);
    };

    const handleOpenEditDialog = (org) => {
        setSelectedOrgDetails(org);
        setEditDialogOpen(true);
    };

    const handleCloseEditDialog = () => {
        setToastMessage("Organisation updated successfully");
        setEditDialogOpen(false);
        setSelectedOrgDetails(null);
        setShowToast(true);
    };

    const handleDefaultCloseEditDialog = () => {
    setEditDialogOpen(false);
    };

    // Open the delete dialog
    const handleOpenDeleteDialog = (org) => {
    setSelectedOrgDetails(org);
    setDeleteDialogOpen(true);
    };

    const handleDefaultCloseDeleteDialog = () => {
    setDeleteDialogOpen(false);
    };

    // Close the delete dialog
    const handleCloseDeleteDialog = () => {
    setToastMessage("Organisation deleted successfully");
    setDeleteDialogOpen(false);
    setSelectedOrgDetails(null);
    setShowToast(true);
    };
    const columns = [
        {
            field: "id",
            headerName: "ID",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "name",
            headerName: "Name",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "actions",
            headerName: "Actions",
            flex: 1,
            cellClassName: "data-cell",
            renderCell: ({ row }) => (
            <Box p="1vh" display="flex" justifyContent="center">
                <Link
                onClick={() => handleOpenDetailsDialog(row)}
                style={{ marginRight: "2vh" }}
                >
                <Visibility />
                </Link>
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
        <div className="faculty">
            {/* <Box m="2vh">
                <h1 className="title">
                    facultys
                </h1>
                {/* <NotificationToast
                    show={showToast}
                    onClose={() => setShowToast(false)}
                    message={toastMessage}
                /> */}
                {/* <Box
                    m="4vh 0 0 0"
                    height="75vh"
                    sx={{
                    "& .MuiDataGrid-root": {
                        border: "none",
                    },
                    "& .MuiDataGrid-cell": {
                        borderBottom: "none",
                    },
                    "& .name-column--cell": {
                        color: colors.greenAccent[300],
                    },
                    "& .MuiDataGrid-columnHeaders": {
                        backgroundColor: colors.blueAccent[700],
                        borderBottom: "none",
                    },
                    "& .MuiDataGrid-virtualScroller": {
                        backgroundColor: colors.primary[400],
                    },
                    "& .MuiDataGrid-footerContainer": {
                        borderTop: "none",
                        backgroundColor: colors.blueAccent[700],
                    },
                    "& .MuiDataGrid-toolbarContainer .MuiButton-text": {
                        color: `${colors.grey[100]} !important`,
                    },
                    "& .css-hia42h-MuiCircularProgress-root": {
                        color: `${colors.grey[100]} !important`,
                    },
                    }}
                > */}
                <div className="facultyWrapper">
                    <DataGrid
                    rows={data}
                    columns={columns}
                    // loading={loading}
                    checkboxSelection
                    disableRowSelectionOnClick
                    components={{ Toolbar: GridToolbar }}
                    />
                {/* </Box> */}
                {/* <CustomDialog
                    open={deleteDialogOpen}
                    handleClose={handleDefaultCloseDeleteDialog}
                    handleConfirm={handleDelete}
                    title="Delete Organization"
                    description="Are you sure you want to delete this organization?"
                />
                <OrgDetails
                    openDialog={detailDialogOpen}
                    handleCloseDialog={handleCloseDetailsDialog}
                    selectedOrgDetails={selectedOrgDetails}
                /> */}
                {/* {editDialogOpen && (
                    <OrgUpdateForm
                    handleCloseDialog={handleCloseEditDialog}
                    handleDefaultCloseEditDialog={handleDefaultCloseEditDialog}
                    org={selectedOrgDetails}
                    reFetch={reFetch}
                    />
                )} */}
                </div>
            {/* </Box> */}
        </div>
    );
};

export default Faculties;
