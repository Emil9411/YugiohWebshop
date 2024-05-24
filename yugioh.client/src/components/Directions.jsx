import './Component.css';

function Directions({ directionsList }) {
    const directions = directionsList.split(",");

    const directionsImages = {
        "Top": "/direction-arrows/up.png",
        "Bottom": "/direction-arrows/down.png",
        "Left": "/direction-arrows/left.png",
        "Right": "/direction-arrows/right.png",
        "Bottom-Left": "/direction-arrows/left-down.png",
        "Bottom-Right": "/direction-arrows/right-down.png",
        "Top-Left": "/direction-arrows/left-up.png",
        "Top-Right": "/direction-arrows/right-up.png"
    }

    return (
        <>
            <p>Link Markers: </p>
            <div className="directions">
            {directions.map((direction, i) => (
                <div key={i} className="direction-wrapper">
                    <img src={directionsImages[direction]} alt={direction} />
                </div>
            ))}
            </div>
        </>

    );
}

export default Directions;