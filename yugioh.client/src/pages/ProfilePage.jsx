import { useEffect, useState } from 'react';
import '../index.css';

function ProfilePage() {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

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
                        <th>Profile</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Email:</td>
                        <td>{user.email}</td>
                    </tr>
                    <tr>
                        <td>First name:</td>
                        <td>{user.firstName}</td>
                    </tr>
                    <tr>
                        <td>Last name:</td>
                        <td>{user.lastName}</td>
                    </tr>
                    <tr>
                        <td>Address:</td>
                        <td>{user.address}</td>
                    </tr>
                    <tr>
                        <td>City:</td>
                        <td>{user.city}</td>
                    </tr>
                    <tr>
                        <td>Country:</td>
                        <td>{user.country}</td>
                    </tr>
                    <tr>
                        <td>Postal code:</td>
                        <td>{user.postalCode}</td>
                    </tr>
                    <tr>
                        <td>Phone number:</td>
                        <td>{user.phoneNumber}</td>
                    </tr>
                </tbody>
            </table>
            <button>Edit </button>
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