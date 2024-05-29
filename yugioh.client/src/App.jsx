import { useEffect, useState } from 'react';
import { Outlet, Link, useLocation } from "react-router-dom";
import './index.css';
function App() {
    const [user, setUser] = useState(null);
    const location = useLocation();

    const updateBackgroundClass = () => {
        const width = window.innerWidth;
        const root = document.documentElement;

        if (width > 1100) {
            root.classList.add('wide-bg');
            root.classList.remove('narrow-bg');
        } else {
            root.classList.add('narrow-bg');
            root.classList.remove('wide-bg');
        }
    };

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
                } else {
                    throw new Error("User not authenticated");
                }
            } catch (error) {
                console.error(error);
            }
        }
        fetchUser();
    }, [location.pathname]);




    useEffect(() => {
        updateBackgroundClass(); // Set initial background class
        window.addEventListener('resize', updateBackgroundClass); // Update background class on resize

        return () => {
            window.removeEventListener('resize', updateBackgroundClass); // Clean up the event listener on unmount
        };
    }, []);

    return (
        <div className="App">
            <div className="header">
                <div className="header-title">
                    <h1>Yugioh Webshop</h1>
                </div>
                <div className="header-buttons">
                    <Link to="/">
                        <button disabled={location.pathname === '/'}>Home</button>
                    </Link>
                    <Link to="/all">
                        <button disabled={location.pathname === '/all'}>All cards</button>
                    </Link>
                    {user === null ? (
                        <>
                            <Link to="/registration">
                                <button disabled={location.pathname === '/registration'}>Register</button>
                            </Link>
                            <Link to="/login">
                                <button disabled={location.pathname === '/login'}>Login</button>
                            </Link>
                        </>
                    ) : !user.username.includes("admin") ? (
                        <>
                            <Link to="/monsters">
                                <button disabled={location.pathname === '/monsters'}>Monster cards</button>
                            </Link>
                            <Link to="/spells">
                                <button disabled={location.pathname === '/spells'}>Spell/Trap cards</button>
                            </Link>
                            <button>Profile</button>
                            <button>Logout</button>
                            <button>Cart</button>
                        </>
                    ) : (
                        <>
                            <Link to="/monsters">
                                <button disabled={location.pathname === '/monsters'}>Monster cards</button>
                            </Link>
                            <Link to="/spells">
                                <button disabled={location.pathname === '/spells'}>Spell/Trap cards</button>
                            </Link>
                            <button>Admin</button>
                            <button>Logout</button>
                        </>
                    )}
                </div>
            </div>
            <Outlet />
        </div>

    );

}

export default App;