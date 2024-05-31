import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../index.css';
import swal from 'sweetalert';

function ProfilePage() {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    const navigate = useNavigate();

    useEffect(() => {
        async function fetchUser() {
            try {
                const response = await fetch('/api/Auth/whoami', {
                    method: 'GET',
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
                if (response.ok) {
                    const data = await response.json();
                    const userData = await fetchUserData(data.email);
                    setUser(userData);
                    setLoading(false);
                } else {
                    throw new Error('User not authenticated');
                }
            } catch (error) {
                console.error(error);
            }
        }
        fetchUser();
    }, []);

    async function fetchUserData(email) {
        try {
            const response = await fetch(`/api/User/getuserbyemail/${email}`, {
                method: 'GET',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            const data = await response.json();
            if (response.ok) {
                return data;
            } else {
                throw new Error('User not found');
            }
        } catch (error) {
            console.error(error);
        }
    }

    async function changeEmail(newEmail) {
        try {
            const response = await fetch('/api/Auth/changeemail', {
                method: 'PATCH',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email: user.email, newEmail }),
            });
            if (response.ok) {
                const logoutResponse = await fetch('/api/Auth/logout', {
                    method: 'POST',
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
                if (logoutResponse.ok) {
                    swal({
                        title: 'Email changed',
                        text: 'Please log in again',
                        icon: 'success',
                        button: 'OK',
                    }).then(() => {
                        navigate('/');
                        window.location.reload();
                    });
                } else {
                    throw new Error('Logout failed');
                }
            }
        } catch (error) {
            console.error(error);
        }
    }

    function handleChangeEmail() {
        swal({
            text: 'Enter new email',
            content: {
                element: 'input',
                attributes: {
                    type: 'email',
                    placeholder: 'New email',
                },
            },
            buttons: true,
        }).then((value) => {
            if (value) {
                swal({
                    title: 'Are you sure?',
                    text: `Do you really want to change your email to ${value}?`,
                    icon: 'warning',
                    buttons: true,
                    dangerMode: true,
                }).then((willChange) => {
                    if (willChange) {
                        changeEmail(value);
                    }
                });
            }
        });
    }

    async function changePassword(oldPassword, newPassword) {
        try {
            const response = await fetch('/api/Auth/changepassword', {
                method: 'PATCH',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email: user.email, oldPassword, newPassword }),
            });
            if (response.ok) {
                const logoutResponse = await fetch('/api/Auth/logout', {
                    method: 'POST',
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
                if (logoutResponse.ok) {
                    swal({
                        title: 'Password changed',
                        text: 'Please log in again',
                        icon: 'success',
                        button: 'OK',
                    }).then(() => {
                        navigate('/');
                        window.location.reload();
                    });
                } else {
                    throw new Error('Logout failed');
                }
            }
        } catch (error) {
            console.error(error);
        }
    }

    function handleChangePassword() {
        swal({
            text: 'Enter old password',
            content: {
                element: 'input',
                attributes: {
                    type: 'password',
                    placeholder: 'Old password',
                },
            },
            buttons: true,
        }).then((oldPassword) => {
            if (oldPassword) {
                swal({
                    text: 'Enter new password',
                    content: {
                        element: 'input',
                        attributes: {
                            type: 'password',
                            placeholder: 'New password',
                        },
                    },
                    buttons: true,
                }).then((newPassword) => {
                    if (newPassword) {
                        swal({
                            title: 'Are you sure?',
                            text: 'Do you really want to change your password?',
                            icon: 'warning',
                            buttons: true,
                            dangerMode: true,
                        }).then((willChange) => {
                            if (willChange) {
                                changePassword(oldPassword, newPassword);
                            }
                        });
                    }
                });
            }
        });
    }

    async function changePersonalInfo(updatedInfo) {
        try {
            const response = await fetch('/api/User/updateuser', {
                method: 'PATCH',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: user.email,
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
                    window.location.reload();
                });
            }
        } catch (error) {
            console.error(error);
        }
    }

    function handleEditPersonalInfo() {
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
                    <input type="text" id="firstName" name="firstName" value="${user.firstName ? user.firstName : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="lastName">Last name:</label>
                    </td>
                    <td>
                    <input type="text" id="lastName" name="lastName" value="${user.lastName ? user.lastName : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="address">Address:</label>
                    </td>
                    <td>
                    <input type="text" id="address" name="address" value="${user.address ? user.address : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="city">City:</label>
                    </td>
                    <td>
                    <input type="text" id="city" name="city" value="${user.city ? user.city : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="country">Country:</label>
                    </td>
                    <td>
                    <input type="text" id="country" name="country" value="${user.country ? user.country : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="postalCode">Postal code:</label>
                    </td>
                    <td>
                    <input type="text" id="postalCode" name="postalCode" value="${user.postalCode ? user.postalCode : ""}" required>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <label for="phoneNumber">Phone number:</label>
                    </td>
                    <td>
                    <input type="text" id="phoneNumber" name="phoneNumber" value="${user.phoneNumber ? user.phoneNumber : ""}" required>
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
                            handleEditPersonalInfo();
                        });
                        //isValid = false;
                        //break;
                    }
                    updatedInfo[fieldName] = fieldValue;
                }

                if (isValid) {
                    changePersonalInfo(updatedInfo);
                } 
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
        <div className="profile">
            <table>
                <thead>
                    <tr>
                        <th colSpan="3" style={{ textAlign: 'center' }}>Personal information of {user.userName}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Email:</td>
                        <td colSpan="2">{user.email}</td>
                    </tr>
                    <tr>
                        <td>First name:</td>
                        <td colSpan="2">{user.firstName}</td>
                    </tr>
                    <tr>
                        <td>Last name:</td>
                        <td colSpan="2">{user.lastName}</td>
                    </tr>
                    <tr>
                        <td>Address:</td>
                        <td colSpan="2">{user.address}</td>
                    </tr>
                    <tr>
                        <td>City:</td>
                        <td colSpan="2">{user.city}</td>
                    </tr>
                    <tr>
                        <td>Country:</td>
                        <td colSpan="2">{user.country}</td>
                    </tr>
                    <tr>
                        <td>Postal code:</td>
                        <td colSpan="2">{user.postalCode}</td>
                    </tr>
                    <tr>
                        <td>Phone number:</td>
                        <td colSpan="2">{user.phoneNumber}</td>
                    </tr>
                    <tr>
                        <td colSpan="3" style={{ height: '20px' }}></td>
                    </tr>
                    <tr>
                        <td style={{ textAlign: 'center' }}>
                            <button onClick={handleEditPersonalInfo}>
                                Edit personal info
                            </button>
                        </td>
                        <td style={{ textAlign: 'center' }}>
                            <button onClick={handleChangeEmail}>
                                Change email
                            </button>
                        </td>
                        <td style={{ textAlign: 'center' }}>
                            <button onClick={handleChangePassword}>
                                Change password
                            </button>
                        </td>
                    </tr>
                    <tr>
                        <td colSpan="3" style={{ height: '10px' }}></td>
                    </tr>
                </tbody>
            </table>
            <table>
                <thead>
                    <tr>
                        <th>Order ID</th>
                        <th>Order date</th>
                        <th>Order status</th>
                        <th>Order total</th>
                    </tr>
                </thead>
            </table>
        </div>
    );
}

export default ProfilePage;
