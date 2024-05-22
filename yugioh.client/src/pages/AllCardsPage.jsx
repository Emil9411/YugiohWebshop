import React, { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import '../index.css';
import '../components/Component.css';

function AllCardsPage() {
    const testCardObject = {
        "attack": 0,
        "defense": null,
        "level": null,
        "attribute": "WIND",
        "scale": null,
        "linkValue": 4,
        "linkMarkers": "Bottom-Left,Bottom,Bottom-Right,Top",
        "id": 69516,
        "cardId": 4280258,
        "name": "Apollousa, Bow of the Goddess",
        "type": "Link Monster",
        "frameType": "link",
        "race": "Fairy",
        "archetype": null,
        "description": "2+ monsters with different names, except Tokens\r\nYou can only control 1 \"Apollousa, Bow of the Goddess\". The original ATK of this card becomes 800 x the number of Link Materials used for its Link Summon. Once per Chain, when your opponent activates a monster effect (Quick Effect): You can make this card lose exactly 800 ATK, and if you do, negate the activation.",
        "ygoProDeckUrl": "https://ygoprodeck.com/card/apollousa-bow-of-the-goddess-10242",
        "imageUrl": "https://images.ygoprodeck.com/images/cards/4280258.jpg",
        "price": "4.02"
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
