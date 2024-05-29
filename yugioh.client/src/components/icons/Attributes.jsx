function Attributes({ attribute }) {
    const attributesImages = {
        "SPELL": "/attributes/spell.png",
        "TRAP": "/attributes/trap.png",
        "EARTH": "/attributes/earth.png",
        "WATER": "/attributes/water.png",
        "FIRE": "/attributes/fire.png",
        "WIND": "/attributes/wind.png",
        "LIGHT": "/attributes/light.png",
        "DARK": "/attributes/dark.png",
        "DIVINE": "/attributes/divine.png"
    }
    return (
        <>
            <p>Attributes:</p>
            <div className="attributes-images">
                <img src={attributesImages[attribute]} alt={attribute} />
                {attribute}
            </div>
        </>
    );
}
export default Attributes;