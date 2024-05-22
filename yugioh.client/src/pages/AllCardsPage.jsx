import React, { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import '../index.css';
import '../components/CardComponent.css';

function AllCardsPage() {
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
    const [cards, setCards] = useState([testCardObject, testCardObject, testCardObject, testCardObject, testCardObject]);

    return (
        <div className="card-container">
            {cards.map((card, i) => (
                <div key={i} className="card-wrapper">
                    <CardComponent cardData={card} />
                </div>
            ))}
        </div>
    );
}

export default AllCardsPage;
