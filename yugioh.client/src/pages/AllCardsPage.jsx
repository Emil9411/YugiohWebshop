import { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import '../index.css';
import '../components/Component.css';

function AllCardsPage() {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);

    const [currentCards, setCurrentCards] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const cardsPerPage = 80;
    const maxPageButtons = 10; // Maximum number of page buttons to display

    // Fetch cards from the API
    async function fetchCards() {
        try {
            const response = await fetch('https://localhost:7114/api/Card/allcards');
            const data = await response.json();
            const array = data.monsterCards.concat(data.spellAndTrapCards);
            array.sort((a, b) => a.name.localeCompare(b.name));
            setCards(array);
            setLoading(false);
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        fetchCards();
    }, []);

    useEffect(() => {
        const indexOfLastCard = currentPage * cardsPerPage;
        const indexOfFirstCard = indexOfLastCard - cardsPerPage;
        setCurrentCards(cards.slice(indexOfFirstCard, indexOfLastCard));
    }, [currentPage, cards]);

    const pagesCount = Math.ceil(cards.length / cardsPerPage);

    const handlePageChange = (pageNumber) => {
        if (pageNumber > 0 && pageNumber <= pagesCount) {
            setCurrentPage(pageNumber);
            window.scrollTo(0, 0); // Scroll to the top of the page
        }
    };


    const renderPageButtons = () => {
        const buttons = [];
        const middleIndex = Math.floor(maxPageButtons / 2);
        let startPage = currentPage - middleIndex;
        let endPage = currentPage + middleIndex;

        if (startPage < 1) {
            startPage = 1;
            endPage = Math.min(startPage + maxPageButtons - 1, pagesCount);
        } else if (endPage > pagesCount) {
            endPage = pagesCount;
            startPage = Math.max(endPage - maxPageButtons + 1, 1);
        }

        for (let i = startPage; i <= endPage; i++) {
            buttons.push(
                <button
                    key={i}
                    onClick={() => handlePageChange(i)}
                    className={i === currentPage ? 'active' : ''}
                    disabled={i === currentPage}
                >
                    {i}
                </button>
            );
        }

        return buttons;
    };

    if (loading) {
        return (
            <>
                <br />
                <div className="spinner"></div>
            </>);
    }

    return (
        <>
            <br />
            <h1>All Cards</h1>
            <br />
            <div className="pagination">
                <button onClick={() => { setCurrentPage(1); window.scrollTo(0, 0); }} disabled={currentPage === 1}>First</button>
                <button onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>Previous</button>
                {renderPageButtons()}
                <button onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === pagesCount}>Next</button>
                <button onClick={() => { setCurrentPage(pagesCount); window.scrollTo(0, 0); }} disabled={currentPage === pagesCount}>Last</button>
            </div><div className="card-container">
                {currentCards.map((card, i) => (
                    <div key={i} className="card-wrapper">
                        <CardComponent cardData={card} />
                    </div>
                ))}
                <div className="pagination">
                    <button onClick={() => { setCurrentPage(1); window.scrollTo(0, 0); }} disabled={currentPage === 1}>First</button>
                    <button onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>Previous</button>
                    {renderPageButtons()}
                    <button onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === pagesCount}>Next</button>
                    <button onClick={() => { setCurrentPage(pagesCount); window.scrollTo(0, 0); }} disabled={currentPage === pagesCount}>Last</button>
                </div>
            </div>
        </>
    );
}

export default AllCardsPage;
