import React, { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import '../index.css';
import '../components/Component.css';

function AllCardsPage() {
    const [cards, setCards] = useState([]);

    async function fetchCards() {
        try {
            const response = await fetch('https://localhost:7114/api/Card/allcards');
            const data = await response.json();
            const array = data.monsterCards.concat(data.spellAndTrapCards);
            setCards(array);
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        fetchCards();
    }, []);

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
