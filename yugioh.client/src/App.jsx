import { useEffect, useState } from 'react';
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
        <div>
            <h1 id="tabelLabel">Yugioh Webshop</h1>
            <button>All cards</button>
            <button>Monster cards</button>
            <button>Spell/Trap cards</button>
            <button>Register</button>
            <button>Login</button>
            <button>Logout</button>
            <button>Cart</button>
            
        </div>
    );
    
}

export default App;