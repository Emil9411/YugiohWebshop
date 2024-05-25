import { useState, useEffect } from 'react';
import Directions from './Directions';
import LevelRank from './LevelRank';
import Attributes from './Attributes';

import './Component.css';

function CardComponent({ cardData }) {
    const [card, setCard] = useState(cardData);
    const [cardBodyHidden, setCardBodyHidden] = useState(true);

    useEffect(() => {
        setCard(cardData);
    }, [cardData]);

    let cardImageUrl = "https://localhost:7114/YugiohPics/";
    if (card.frameType === "spell" || card.frameType === "trap" || card.frameType === "skill") {
        cardImageUrl += "SpellAndTrapCards/";
    } else {
        cardImageUrl += "MonsterCards/";
    }
    cardImageUrl += card.cardId + ".jpg";

    return (
        <div className="card">
            <div className="card-name">
                <h2>{card.name}</h2>
            </div>
            <div className="card-basics">
                <p>{card.type}</p>
                <img src={cardImageUrl} alt={card.name} />
                <br />
                <button onClick={() => setCardBodyHidden(!cardBodyHidden)}>
                    {cardBodyHidden ? 'Show details' : 'Hide details'}
                </button>
            </div>
            <div className={`card-body ${cardBodyHidden ? 'hidden' : ''}`}>
                <p>Card text: {card.description}</p>
                {card.attack ? <p style={{ color: "firebrick" }}>ATK: {card.attack}</p> : null}
                {card.defense ? <p style={{ color: "royalblue" }}>DEF: {card.defense}</p> : null}
                {card.level ? <LevelRank count={card.level} level={card.type.includes("XYZ") ? "rank" : "level" } /> : null}            
                {card.scale ? <p>Scale: {card.scale}</p> : null}
                {card.linkValue ? <p>Link Value: {card.linkValue}</p> : null}
                {card.linkMarkers ? <Directions directionsList={card.linkMarkers} /> : null}
                <p>Race: {card.race}</p>
                {card.attribute ? <Attributes attribute={card.attribute} /> : null}
                {card.archetype ? <p>Archetype: {card.archetype}</p> : null}
                <a href={card.ygoProDeckUrl}>YGOPRODeck link</a>
                <p>Price: {card.price} $</p>
            </div>
        </div>
    );
}

export default CardComponent;
