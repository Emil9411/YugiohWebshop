import { useEffect, useState } from 'react';
import './index.css';
import CardComponent from './components/CardComponent';

function App() {

    const testCardObject = {
        id: 1,
        cardId: 89631139,
        name: 'Blue-Eyes White Dragon',
        type: 'Normal Monster',
        frameType: 'normal',
        desc: 'This legendary dragon is a powerful engine of destruction. Virtually invincible, very few have faced this awesome creature and lived to tell the tale.',
        atk: 3000,
        def: 2500,
        level: 8,
        race: 'Dragon',
        attribute: 'LIGHT',
        archetype: 'Blue-Eyes',
        price: 0.35,
        ygouProDeckUrl: 'https://ygoprodeck.com/card/blue-eyes-white-dragon-7485'
    };


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
                    <button>All cards</button>
                    <button>Monster cards</button>
                    <button>Spell/Trap cards</button>
                    <button>Register</button>
                    <button>Login</button>
                    <button>Logout</button>
                    <button>Cart</button>
                </div>
            </div>
            <CardComponent cardData={testCardObject} />
        </div>

    );

}

export default App;