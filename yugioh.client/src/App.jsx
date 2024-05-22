import { useEffect, useState } from 'react';
import { Outlet, Link, useLocation } from "react-router-dom";
import './index.css';
function App() {

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
                        <button>Home</button>
                    </Link>
                    <Link to="/all">
                        <button>All cards</button>
                    </Link>
                    <button>Monster cards</button>
                    <button>Spell/Trap cards</button>
                    <button>Register</button>
                    <button>Login</button>
                    <button>Logout</button>
                    <button>Cart</button>
                </div>
            </div>
            <Outlet />
        </div>

    );

}

export default App;