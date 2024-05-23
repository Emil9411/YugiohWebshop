import { useState, useEffect } from 'react';
import './Component.css';
import Directions from './Directions';

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
        <div
            className="card"
        >
            <div className="card-header">
                <h2>{card.name}</h2>
                <p>{card.type}</p>
            </div>
            <img src={cardImageUrl} alt={card.name} />
            <br />
            <button onClick={() => setCardBodyHidden(!cardBodyHidden)}>
                {cardBodyHidden ? 'Show details' : 'Hide details'}
            </button>
            <div className={`card-body ${cardBodyHidden ? 'hidden' : ''}`}>
                <p>Card text: {card.description}</p>
                {card.attack ? <p>ATK: {card.attack}</p> : null}
                {card.defense ? <p>DEF: {card.defense}</p> : null}
                {card.level ? <p>Level: {card.level}</p> : null}
                {card.scale ? <p>Scale: {card.scale}</p> : null}
                {card.linkValue ? <p>Link Value: {card.linkValue}</p> : null}
                {card.linkMarkers ? <Directions directionsList={card.linkMarkers} /> : null}
                <p>Race: {card.race}</p>
                <p>Attribute: {card.attribute}</p>
                {card.archetype ? <p>Archetype: {card.archetype}</p> : null}
                <a href={card.ygouProDeckUrl}>Ygoprodeck link</a>
                <p>Price: {card.price} $</p>
            </div>
        </div>
    );
}

export default CardComponent;
