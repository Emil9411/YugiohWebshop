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
                setUsers(true);
            } else {
                throw new Error("User list not fetched");
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

    async function changePersonalInfo(email, updatedInfo) {
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
                    fetchUsers();
                });
            }
        } catch (error) {
            console.error(error);
        }
    }

    function handleEditPersonalInfo(index) {
        const userToEdit = userList[index];
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

                // Client-side validation
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
                    changePersonalInfo(userToEdit.email, updatedInfo);
                }
            }
        });
    }

    async function deleteUser(email) {
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
                    fetchUsers();
                });
            }
        } catch (error) {
            console.error(error);
        }
    }


    function handleDeleteUser(index) {
        const userToDelete = userList[index];
        swal({
            title: 'Are you sure?',
            text: 'User will be deleted',
            icon: 'warning',
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                deleteUser(userToDelete.email);
            }
        });
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
            //need an edit, a delete and a cancel button
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
                                    <button>Find user by email</button>
                                </td>
                                <td>
                                    <button>Add new admin</button>
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
            ) : null}
        </div>
    );
}

export default AdminPage;