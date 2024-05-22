import './Component.css';

function Directions(props) {
    const directions = props.directionsList.split(",");

    const directionsImages = {
        "Top": "public/direction-arrows/up.png",
        "Bottom": "public/direction-arrows/down.png",
        "Left": "public/direction-arrows/left.png",
        "Right": "public/direction-arrows/right.png",
        "Bottom-Left": "public/direction-arrows/left-down.png",
        "Bottom-Right": "public/direction-arrows/right-down.png",
        "Top-Left": "public/direction-arrows/left-up.png",
        "Top-Right": "public/direction-arrows/right-up.png"
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