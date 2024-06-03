import React, { useState, useEffect } from 'react';
import swal from 'sweetalert';
import '../index.css';
import './admin.css';

function AdminPage() {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    const [showDatabaseOperations, setShowDatabaseOperations] = useState(false);
    const [showUserOperations, setShowUserOperations] = useState(false);
    const [showCartOperations, setShowCartOperations] = useState(false);

    const [userList, setUserList] = useState([]);
    const [users, setUsers] = useState(false);

    const [userToFind, setUserToFind] = useState(null);
    const [userSearch, setUserSearch] = useState(false);
    const [addAdmin, setAddAdmin] = useState(false);

    useEffect(() => {
        async function fetchUser() {
            try {
                const response = await fetch("api/Auth/whoami", {
                    method: "GET",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (response.ok) {
                    const data = await response.json();
                    setUser(data);
                    setLoading(false);
                } else {
                    throw new Error("User not authenticated");
                }
            } catch (error) {
                console.error(error);
            }
        }
        fetchUser();
    }, []);

    async function fetchUsers() {
        try {
            const response = await fetch("api/User/getusers", {
                method: "GET",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });
            if (response.ok) {
                const data = await response.json();
                setUserList(data);
                setUserSearch(false);
                setUsers(true);
            } else {
                throw new Error("User list not fetched");
            }
        } catch (error) {
            console.error(error);
        }
    }

    async function findUserByEmail(email) {
        try {
            const response = await fetch(`api/User/getuserbyemail/${email}`, {
                method: "GET",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });
            if (response.ok) {
                const data = await response.json();
                setUsers(false);
                swal({
                    text: "User details",
                    content: {
                        element: "div",
                        attributes: {
                            innerHTML: `
                            <table>
                            <tbody>
                            <tr>
                            <td>
                            <label for="firstName">First name:</label>
                            </td>
                            <td>
                            <p>${data.firstName ? data.firstName : "N/A"}</p>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <label for="lastName">Last name:</label>
                            </td>
                            <td>
                            <p>${data.lastName ? data.lastName : "N/A"}</p>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <label for="address">Address:</label>
                            </td>
                            <td>
                            <p>${data.address ? data.address : "N/A"}</p>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <label for="city">City:</label>
                            </td>
                            <td>
                            <p>${data.city ? data.city : "N/A"}</p>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <label for="country">Country:</label>
                            </td>
                            <td>
                            <p>${data.country ? data.country : "N/A"}</p>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <label for="postalCode">Postal code:</label>
                            </td>
                            <td>
                            <p>${data.postalCode ? data.postalCode : "N/A"}</p>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <label for="phoneNumber">Phone number:</label>
                            </td>
                            <td>
                            <p>${data.phoneNumber ? data.phoneNumber : "N/A"}</p>
                            </td>
                            </tr>
                            </tbody>
                            </table>
                            `,
                        },
                    },
                    buttons: {
                        edit: {
                            text: "Edit",
                            value: "edit",
                            visible: true,
                            className: "edit-button",
                        },
                        delete: {
                            text: "Delete",
                            value: "delete",
                            visible: true,
                            className: "delete-button",
                        },
                        cancel: {
                            text: "Cancel",
                            value: "cancel",
                            visible: true,
                            className: "cancel-button",
                        },
                    },
                }).then((value) => {
                    switch (value) {
                        case "edit":
                            handleEditPersonalInfo(email);
                            break;
                        case "delete":
                            handleDeleteUser(email);
                            break;
                        default:
                            break;
                    }
                });
            } else {
                swal({
                    title: 'Error',
                    text: 'User not found',
                    icon: 'error',
                });
                throw new Error("User not found");
            }
        } catch (error) {
            console.error(error);
        }
    }


    function handleFillDatabase() {
        swal({
            title: "Are you sure?",
            text: "Database will be filled with data from the API",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willFill) => {
            if (willFill) {
                setLoading(true);
                fetch("api/Card/filldatabase", {
                    method: "GET",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json",
                    },
                }).then((response) => {
                    setLoading(false);
                    if (response.ok) {
                        swal("Database filled", {
                            icon: "success",
                        });
                    } else {
                        swal("Database not filled", {
                            icon: "error",
                        });
                    }
                });
            }
        });
    }

    function handleUpdateDatabase() {
        swal({
            title: "Are you sure?",
            text: "Database will be updated with data from the API",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willUpdate) => {
            if (willUpdate) {
                setLoading(true);
                fetch("api/Card/updatedatabase", {
                    method: "PATCH",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json",
                    },
                }).then((response) => {
                    setLoading(false);
                    if (response.ok) {
                        swal("Database updated", {
                            icon: "success",
                        });
                    }
                    else {
                        swal("Database not updated", {
                            icon: "error",
                        });
                    }
                });
            }
        });
    }

    function handleClearDatabase() {
        swal({
            title: "Are you sure?",
            text: "Database will be cleared",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willClear) => {
            if (willClear) {
                setLoading(true);
                fetch("api/Card/cleandatabase", {
                    method: "DELETE",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json",
                    },
                }).then((response) => {
                    setLoading(false);
                    if (response.ok) {
                        swal("Database cleared", {
                            icon: "success",
                        });
                    } else {
                        swal("Database not cleared", {
                            icon: "error",
                        });
                    }
                });
            }
        });
    }

    async function changePersonalInfo(email, updatedInfo, individualUser) {
        try {
            const response = await fetch('/api/User/updateuser', {
                method: 'PATCH',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: email,
                    firstName: updatedInfo.firstName,
                    lastName: updatedInfo.lastName,
                    address: updatedInfo.address,
                    city: updatedInfo.city,
                    country: updatedInfo.country,
                    postalCode: updatedInfo.postalCode,
                    phoneNumber: updatedInfo.phoneNumber,
                }),
            });
            if (response.ok) {
                swal({
                    title: 'Personal info updated',
                    text: 'Your personal information has been updated',
                    icon: 'success',
                    button: 'OK',
                }).then(() => {
                    if (!individualUser) {
                        fetchUsers();
                    } else {
                        findUserByEmail(email);
                    }
                });
            }
        } catch (error) {
            console.error(error);
        }
    }

    async function handleEditPersonalInfo(index) {
        let userToEdit;
        let individualUser;
        if (typeof index === 'string') {
            individualUser = true;
            try {
                const response = await fetch(`api/User/getuserbyemail/${index}`, {
                    method: "GET",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (response.ok) {
                    userToEdit = await response.json();
                } else {
                    swal({
                        title: 'Error',
                        text: 'User not found',
                        icon: 'error',
                    });
                    throw new Error("User not found");
                }
            } catch (error) {
                console.error(error);
            }
        } else {
            individualUser = false;
            userToEdit = userList[index];
        }
        swal({
            text: 'Edit personal information',
            content: {
                element: 'div',
                attributes: {
                    innerHTML: `
                    <form id="editPersonalInfoForm">
                    <table>
                    <tbody>
                    <tr>
                    <td>
                    <label for="firstName">First name:</label>
                    </td>
                    <td>
                    <input type="text" id="firstName" name="firstName" value="${userToEdit.firstName ? userToEdit.firstName : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="lastName">Last name:</label>
                    </td>
                    <td>
                    <input type="text" id="lastName" name="lastName" value="${userToEdit.lastName ? userToEdit.lastName : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="address">Address:</label>
                    </td>
                    <td>
                    <input type="text" id="address" name="address" value="${userToEdit.address ? userToEdit.address : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="city">City:</label>
                    </td>
                    <td>
                    <input type="text" id="city" name="city" value="${userToEdit.city ? userToEdit.city : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="country">Country:</label>
                    </td>
                    <td>
                    <input type="text" id="country" name="country" value="${userToEdit.country ? userToEdit.country : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="postalCode">Postal code:</label>
                    </td>
                    <td>
                    <input type="text" id="postalCode" name="postalCode" value="${userToEdit.postalCode ? userToEdit.postalCode : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="phoneNumber">Phone number:</label>
                    </td>
                    <td>
                    <input type="text" id="phoneNumber" name="phoneNumber" value="${userToEdit.phoneNumber ? userToEdit.phoneNumber : ""}" required>
                    </td>
                    </tr>
                    </tbody>
                    </table>
                    </form>
                    `,
                },
            },
            buttons: true,
        }).then((willEdit) => {
            if (willEdit) {
                const form = document.getElementById('editPersonalInfoForm');
                const formData = new FormData(form);
                const updatedInfo = {};
                let isValid = true;
                for (const pair of formData.entries()) {
                    const [fieldName, fieldValue] = pair;
                    if (!fieldValue.trim()) {
                        const necessaryField = fieldName === 'firstName' ? 'First name' : fieldName === 'lastName' ? 'Last name' : fieldName === 'address' ? 'Address' : fieldName === 'city' ? 'City' : fieldName === 'country' ? 'Country' : fieldName === 'postalCode' ? 'Postal code' : 'Phone number';
                        swal({
                            title: 'Error',
                            text: `${necessaryField} is required`,
                            icon: 'error',
                        }).then(() => {
                            handleEditPersonalInfo(index);
                        });
                        isValid = false;
                        break;
                    }
                    updatedInfo[fieldName] = fieldValue;
                }
                if (isValid) {
                    changePersonalInfo(userToEdit.email, updatedInfo, individualUser);
                }
            }
        });
    }

    async function deleteUser(email, individualUser) {
        try {
            const response = await fetch(`/api/User/deleteuseradmin/${email}`, {
                method: 'DELETE',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.ok) {
                swal({
                    title: 'User deleted',
                    text: 'User has been deleted',
                    icon: 'success',
                    button: 'OK',
                }).then(() => {
                    if (!individualUser) {
                        fetchUsers();
                    } else {
                        document.getElementById('userToFind').value = '';
                        setUsers(false);
                        setUserSearch(true);
                    }
                });
            }
        } catch (error) {
            console.error(error);
        }
    }


    function handleDeleteUser(index) {
        let individualUser;
        if (typeof index !== 'number') {
            individualUser = true;
            swal({
                title: 'Are you sure?',
                text: 'User will be deleted',
                icon: 'warning',
                buttons: true,
                dangerMode: true,
            }).then((willDelete) => {
                if (willDelete) {
                    deleteUser(index, individualUser);
                }
            });
        } else {
            const userToDelete = userList[index];
            individualUser = false;
            swal({
                title: 'Are you sure?',
                text: 'User will be deleted',
                icon: 'warning',
                buttons: true,
                dangerMode: true,
            }).then((willDelete) => {
                if (willDelete) {
                    deleteUser(userToDelete.email, individualUser);
                }
            });
        }
    }

    function viewUserDetails(index) {
        const userToView = userList[index];
        swal({
            text: 'User details',
            content: {
                element: 'div',
                attributes: {
                    innerHTML: `
                    <table>
                    <tbody>
                    <tr>
                    <td>
                    <label for="firstName">First name:</label>
                    </td>
                    <td>
                    <p>${userToView.firstName ? userToView.firstName : 'N/A'}</p>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="lastName">Last name:</label>
                    </td>
                    <td>
                    <p>${userToView.lastName ? userToView.lastName : 'N/A'}</p>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="address">Address:</label>
                    </td>
                    <td>
                    <p>${userToView.address ? userToView.address : 'N/A'}</p>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="city">City:</label>
                    </td>
                    <td>
                    <p>${userToView.city ? userToView.city : 'N/A'}</p>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="country">Country:</label>
                    </td>
                    <td>
                    <p>${userToView.country ? userToView.country : 'N/A'}</p>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="postalCode">Postal code:</label>
                    </td>
                    <td>
                    <p>${userToView.postalCode ? userToView.postalCode : 'N/A'}</p>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="phoneNumber">Phone number:</label>
                    </td>
                    <td>
                    <p>${userToView.phoneNumber ? userToView.phoneNumber : 'N/A'}</p>
                    </td>
                    </tr>
                    </tbody>
                    </table>
                    `,
                },
            },
            buttons: {
                edit: {
                    text: 'Edit',
                    value: 'edit',
                    visible: true,
                    className: 'edit-button',
                },
                delete: {
                    text: 'Delete',
                    value: 'delete',
                    visible: true,
                    className: 'delete-button',
                },
                cancel: {
                    text: 'Cancel',
                    value: 'cancel',
                    visible: true,
                    className: 'cancel-button',
                },
            },
        }).then((value) => {
            switch (value) {
                case 'edit':
                    handleEditPersonalInfo(index);
                    break;
                case 'delete':
                    handleDeleteUser(index);
                    break;
                default:
                    break;
            }
        });
    }

    function handleFindUserByEmail() {
        const email = userToFind;
        if (!email.trim()) {
            swal({
                title: 'Error',
                text: 'Email is required',
                icon: 'error',
            });
        } else {
            findUserByEmail(email);
        }
    }

    async function handleAddNewAdmin(adminToAdd) {
        try {
            const response = await fetch('/api/User/addadminuser', {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: adminToAdd.email,
                    username: adminToAdd.username,
                    password: adminToAdd.password
                }),
            });
            if (response.ok) {
                swal({
                    title: 'Admin added',
                    text: 'New admin has been added',
                    icon: 'success',
                }).then(() => {
                    setAddAdmin(false);
                });
            }
        } catch (error) {
            console.error(error);
        }
    }

    function addNewAdmin() {
        setAddAdmin(true);
        swal({
            text: 'Add new admin',
            content: {
                element: 'div',
                attributes: {
                    innerHTML: `
                    <form id="addNewAdminForm">
                    <table>
                    <tbody>
                    <tr>
                    <td>
                    <label for="email">Email:</label>
                    </td>
                    <td>
                    <input type="email" id="email" name="email" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="username">Username:</label>
                    </td>
                    <td>
                    <input type="text" id="username" name="username" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="password">Password:</label>
                    </td>
                    <td>
                    <input type="password" id="password" name="password" required>
                    </td>
                    </tr>
                    </tbody>
                    </table>
                    </form>
                    `,
                },
            },
            buttons: true,
        }).then((willAdd) => {
            if (willAdd) {
                const form = document.getElementById('addNewAdminForm');
                const formData = new FormData(form);
                const adminToAdd = {};
                let isValid = true;
                for (const pair of formData.entries()) {
                    const [fieldName, fieldValue] = pair;
                    if (!fieldValue.trim()) {
                        const necessaryField = fieldName === 'email' ? 'Email' : fieldName === 'username' ? 'Username' : 'Password';
                        swal({
                            title: 'Error',
                            text: `${necessaryField} is required`,
                            icon: 'error',
                        }).then(() => {
                            addNewAdmin();
                        });
                        isValid = false;
                        break;
                    }
                    adminToAdd[fieldName] = fieldValue;
                }
                if (isValid) {
                    handleAddNewAdmin(adminToAdd);
                }
            } else {
                setAddAdmin(false);
            }
        });
    }

    if (loading) {
        return (
            <>
                <br />
                <div className="spinner"></div>
            </>
        );
    }

    return (
        <div className="admin-page">
            <h2>Welcome {user.username}</h2>
            <table className="admin-operations">
                <tbody>
                    <tr>
                        <td>
                            <button onClick={() => {
                                setShowDatabaseOperations(true);
                                setShowUserOperations(false);
                                setShowCartOperations(false);
                                setUserSearch(false);
                                setUsers(false);
                            }} disabled={showDatabaseOperations}>Database Operations</button>
                        </td>
                        <td>
                            <button onClick={() => {
                                setShowUserOperations(true);
                                setShowDatabaseOperations(false);
                                setShowCartOperations(false);
                            }} disabled={showUserOperations}>User Operations</button>
                        </td>
                        <td>
                            <button onClick={() => {
                                setShowCartOperations(true);
                                setShowDatabaseOperations(false);
                                setShowUserOperations(false);
                                setUserSearch(false);
                                setUsers(false);
                            }} disabled={showCartOperations}>Cart Operations</button>
                        </td>
                    </tr>
                    {showDatabaseOperations ? (
                        <>
                            <tr>
                                <td>
                                    <button onClick={handleFillDatabase}>Fill Database</button>
                                </td>
                                <td>
                                    <button onClick={handleUpdateDatabase}>Update Database</button>
                                </td>
                                <td>
                                    <button onClick={handleClearDatabase}>Clear Database</button>
                                </td>
                            </tr>
                        </>
                    ) : showUserOperations ? (
                        <>
                            <tr>
                                <td>
                                    <button onClick={fetchUsers} disabled={users}>Get user list</button>
                                </td>
                                <td>
                                    <button onClick={() => {
                                        setUserSearch(true);
                                        setUsers(false);
                                    }} disabled={userSearch}>Find user by email</button>
                                </td>
                                <td>
                                    <button onClick={() => {
                                        setUsers(false);
                                        setUserSearch(false);
                                        addNewAdmin();
                                    }} disabled={addAdmin}>Add new admin</button>
                                </td>
                            </tr>
                        </>
                    ) : null}
                </tbody>
            </table>
            {users ? (
                <table className="user-list">
                    <thead>
                        <tr>
                            <th>
                                <h3>Username</h3>
                            </th>
                            <th>
                                <h3>Email</h3>
                            </th>
                            <th>
                                <h3>Details</h3>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {userList.map((user, i) => (
                            <tr key={i}>
                                <td>{user.userName}</td>
                                <td>{user.email}</td>
                                <td>
                                    <button onClick={() => viewUserDetails(i)}>View details</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : userSearch ? (
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <h2>Search user by email:</h2>
                            </td>
                            <td>
                                <input type="email" required onChange={(e) => setUserToFind(e.target.value)} id="userToFind" placeholder="Enter email"></input>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <button onClick={handleFindUserByEmail}>Find user</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            ) : null}
        </div>
    );
}

export default AdminPage;