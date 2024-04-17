import React, { useEffect, useState } from "react";
import "./accounts.css";
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import { Link } from "react-router-dom";
import { Delete, EditOutlined, Visibility } from "@mui/icons-material";
import { Box, Skeleton, useTheme } from "@mui/material";
import AccountForm from "../../../components/forms/account/AccountForm";
import DeleteConfirm from "../../../components/dialogs/deleteConfirm/DeleteConfirm";
import useFetch from "../../../hooks/useFetch";
import apis from "../../../services/apis.service";
import authService from "../../../services/auth.service";
import authHeader from "../../../services/auth.header";


const Accounts = () => {
    const {data, loading ,error, reFetch} = useFetch(apis.admin+"accounts") 
    const [filteredData, setFilteredData] = useState([]);
    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [selectedAccount, setSelectedAccount] = useState(null);

    const user = authService.getCurrentUser();
    
    const filterRefetch = async () => {
        const refetchData = await reFetch();
        setFilteredData(refetchData);
        // setFilteredData(data);
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

    // Close the delete dialog
    const handleCloseDeleteDialog = () => {
        setToastMessage("Organisation deleted successfully");
        setDeleteDialogOpen(false);
        setSelectedAccount(null);
        setShowToast(true);
    };
    const handleDelete = async () =>{
        if (selectedAccount) {
            const email = selectedAccount.email;
            const url = `${apis.admin}${email}`;
            console.log(email)
            
            try {
                // Send a DELETE request to the server
                const res = await authHeader().delete(url);
        
                // Check if the delete was successful
                if (res.status === 200) {
                    const updatedData = await filterRefetch();
                    localStorage.setItem("users", JSON.stringify(updatedData));
                    handleCloseDeleteDialog();
                    setToastMessage("User deleted successfully");
                    setShowToast(true);
                } else {
                    
                }
            } catch (err) {
                console.error(err);
                setToastMessage("A problem occur");
                setShowToast(true);
            }
        }
    }
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
            flex: 3,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "phoneNumber",
            headerName: "Phone Number",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
        },
        {
            field: "facultyId",
            headerName: "Faculty",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
            valueGetter: (params) => {
                const facultyId = params.row.facultyId;
                const faculty = facultyOptions.find((faculty) => faculty.id === facultyId);
                return faculty ? faculty.name : "";
            },
        },
        {
            field: "roleId",
            headerName: "Role",
            flex: 2,
            headerClassName: "header-text",
            cellClassName: "data-cell",
            valueGetter: (params) => {
                const roleName = params.row.role[0];
                const role = roleOptions.find((role) => role.name === roleName);
                return role ? role.name : "";
            },
        },
        {
            field: "actions",
            headerName: "Actions",
            flex: 2,
            cellClassName: "data-cell",
            renderCell: ({ row }) => {
                // Serialize and encode the row object
                return (
                    <Box p="1vh" display="flex" justifyContent="center">
                        <Link
                            style={{ marginRight: "2vh" }} 
                            to={`/account/${row.id}`}
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
                );
            },
            headerClassName: "header-text",
        },
    ];
    useEffect(() => {
        if (showToast) {
            setTimeout(() => {
            setShowToast(false);
            }, 7000);
        }
    }, [showToast]);
    const [facultyOptions, setFacultyOptions] = useState([]);
    const [roleOptions, setRoleOptions] = useState([]);
    const fetchRolesAndFaculties = async () => {
        try {
            const rolesResponse = await authHeader().get(apis.role, {
                withCredentials: true,
            });
            const facultiesResponse = await authHeader().get(
                apis.faculty
            );
            localStorage.setItem("faculties", JSON.stringify(facultiesResponse.data));
            localStorage.setItem("roles", JSON.stringify(rolesResponse.data));
            setFacultyOptions(facultiesResponse.data);
            setRoleOptions(rolesResponse.data.filter((role) => role.name !== 'Admin'));

        } catch (error) {
            console.error("Error fetching roles and faculties:", error);
            // setMessage("Error fetching roles and faculties");
        }
    } ;
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
                    setRoleOptions(rolesFromStorage.filter((role) => role.name !== 'Admin'));
                } else {
                    await fetchRolesAndFaculties();
                }
            } catch (error) {
                console.error("Error initializing data:", error);
                // setMessage("Error initializing data");
            }
        
        }
        initializeData();
    }, []);
    useEffect(() => {
        setFilteredData(data.filter((userData) => userData.id !== user.id));
    }, [data, user.id, facultyOptions, roleOptions]);
    return (
        <div className="account">
            <h1 className="title">Accounts</h1>
            {loading ? (
                <Box style={{ width: "100vh" }}>
                {Array(10)
                .fill()
                .map((_, i) => (
                    <>
                    <Skeleton />
                    <Skeleton animation={i % 2 === 0 ? "wave" : false} />
                    </>
                ))}
            </Box>
            ):(
                <>
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
                    rows={filteredData}
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
                <DeleteConfirm
                    open={deleteDialogOpen}
                    handleClose={handleCloseDeleteDialog}
                    handleConfirm={handleDelete}
                />
                {editDialogOpen && (
                    <AccountForm
                        handleCloseDialog={handleCloseEditDialog}
                        handleDefaultCloseEditDialog={handleDefaultCloseEditDialog}
                        account={selectedAccount}
                        reFetch={reFetch}
                        facultyOptions ={facultyOptions}
                        roleOptions={roleOptions}
                    />
                )}
                </div>
            {/* </Box> */}
                </>
            )}
            
        </div>
    );
    
};

export default Accounts;
