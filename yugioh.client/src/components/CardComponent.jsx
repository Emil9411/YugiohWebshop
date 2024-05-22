import { useState } from 'react';
import './CardComponent.css';

function CardComponent(props) {
    const [card, setCard] = useState(props.cardData)
    const [cardBodyHidden, setCardBodyHidden] = useState(true)

    let cardImageUrl = "https://localhost:7114/YugiohPics/"
    if (card.frameType === "spell" || card.frameType === "trap" || card.frameType === "skill") {
        cardImageUrl += "SpellAndTrapCards/"
    } else {
        cardImageUrl += "MonsterCards/"
    }
    cardImageUrl += card.cardId + ".jpg"

    return (
        <div
            className="card"
            style={{
                marginTop: window.innerWidth > 768 ? '200px' : (cardBodyHidden ? '100px' : '20px')
            }}
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
                <p>Card text: {card.desc}</p>
                <p>ATK: {card.atk}</p>
                <p>DEF: {card.def ? card.def : null}</p>
                <p>Level: {card.level ? card.level : null}</p>
                <p>Race: {card.race}</p>
                <p>Attribute: {card.attribute}</p>
                <p>Archetype: {card.archetype ? card.archetype : null}</p>
                <a href={card.ygouProDeckUrl}>Ygoprodeck link</a>
                <p>Price: {card.price} $</p>
            </div>
        </div>
    );
}

export default CardComponent;