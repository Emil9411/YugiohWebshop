import React, { useState, useEffect } from 'react';
import swal from 'sweetalert';
import '../index.css';

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
                                <h3>First name</h3>
                            </th>
                            <th>
                                <h3>Last name</h3>
                            </th>
                            <th>
                                <h3>Country</h3>
                            </th>
                            <th>
                                <h3>City</h3>
                            </th>
                            <th>
                                <h3>Address</h3>
                            </th>
                            <th>
                                <h3>Postal code</h3>
                            </th>
                            <th>
                                <h3>Phone number</h3>
                            </th>
                            <th colSpan="2">
                                <h3>Actions</h3>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {userList.map((user, i) => (
                            <tr key={i}>
                                <td>{user.username}</td>
                                <td>{user.email}</td>
                                <td>{user.firstName}</td>
                                <td>{user.lastName}</td>
                                <td>{user.country}</td>
                                <td>{user.city}</td>
                                <td>{user.address}</td>
                                <td>{user.postalCode}</td>
                                <td>{user.phoneNumber}</td>
                                <td>
                                    <button>Edit</button>
                                </td>
                                <td>
                                    <button>Delete</button>
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