import React, { useState } from "react";
import "./accounts.css";
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import { Link } from "react-router-dom";
import { Delete, EditOutlined, Visibility } from "@mui/icons-material";
import { Box, useTheme } from "@mui/material";
import AccountForm from "../../../components/forms/account/AccountForm";

const data = [
    {
        id: 1,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        firstName: "user1",
        lastName: "lmao",
        email: "user1@gmail.com",
        contact_number: "123456789",
        faculty: "Marketing",
        role: 'user',
        
    },
    {
        id: 2,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        firstName: "user2",
        lastName: 'lmao',
        email: "user2@gmail.com",
        contact_number: "123456789",
        faculty: "Marketing",
        role: 'user',
        
    },
    {
        id: 3,
        img: "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQo19mduM602yfQenqFCY0mcAVU-KFkgrnBJJ4O8F4gIM_SZIVX",
        firstName: "user3",
        lastName: 'lmao',
        email: "user3@gmail.com",
        contact_number: "123456789",
        faculty: "Marketing",
        role: 'user',
        
    },
];

const Accounts = () => {

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [detailDialogOpen, setDetailDialogOpen] = useState(false);
    const [selectedAccount, setSelectedAccount] = useState(null);

    const handleOpenDetailsDialog = (acc) => {
        setSelectedAccount(acc);
        setDetailDialogOpen(true);
    };

    const handleCloseDetailsDialog = () => {
        setDetailDialogOpen(false);
        setSelectedAccount(null);
    };

    const handleOpenEditDialog = (acc) => {
        setSelectedAccount(acc);
        setEditDialogOpen(true);
    };

    const handleCloseEditDialog = () => {
        setToastMessage("Organisation updated successfully");
        setEditDialogOpen(false);
        setSelectedAccount(null);
        setShowToast(true);
    };

    const handleDefaultCloseEditDialog = () => {
    setEditDialogOpen(false);
    };

    // Open the delete dialog
    const handleOpenDeleteDialog = (acc) => {
        setSelectedAccount(acc);
    setDeleteDialogOpen(true);
    };

    const handleDefaultCloseDeleteDialog = () => {
        setDeleteDialogOpen(false);
    };

    // Close the delete dialog
    const handleCloseDeleteDialog = () => {
        setToastMessage("Organisation deleted successfully");
        setDeleteDialogOpen(false);
        setSelectedAccount(null);
        setShowToast(true);
    };
    const columns = [
        {
            field: "firstName",
            headerName: "First Name",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "lastName",
            headerName: "Last Name",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "email",
            headerName: "Email",
            flex: 1,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "contact_number",
            headerName: "Contact Number",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "faculty",
            headerName: "Faculty",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "role",  
            headerName: "Role",
            flex: 2,
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
        <div className="account">
            <h1 className="title">Accounts</h1>
            <button className="addButton" onClick={() => handleOpenEditDialog()}>Add Account</button>
            {/* <Box m="2vh">
                <h1 className="title">
                    Accounts
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
                <div className="accountTable">
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
                />
                <OrgDetails
                    openDialog={detailDialogOpen}
                    handleCloseDialog={handleCloseDetailsDialog}
                    selectedOrgDetails={selectedOrgDetails}
                /> */}
                {editDialogOpen && (
                    <AccountForm
                        handleCloseDialog={handleCloseEditDialog}
                        handleDefaultCloseEditDialog={handleDefaultCloseEditDialog}
                        account={selectedAccount}
                        // reFetch={reFetch}
                    />
                )}
                </div>
            {/* </Box> */}
        </div>
    );
};

export default Accounts;
