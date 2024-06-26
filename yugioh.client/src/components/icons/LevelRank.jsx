function LevelRank({ count, level }) {
    const levelImages = {
        "level": "/stars/level.png",
        "rank": "/stars/rank.png"
    };

    const imageUrl = levelImages[level];

    return (
        <>
            <p>Level/Rank:</p>
            <div className="level-rank-images">
                {Array.from({ length: count }).map((_, index) => (
                    <img key={index} src={imageUrl} alt={`${level} ${index + 1}`} />
                ))}
            </div>
        </>
    );
}

export default LevelRank;
