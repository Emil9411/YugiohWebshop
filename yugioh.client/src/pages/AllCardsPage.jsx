import { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import Pagination from '../components/Pagination';
import SelectFilter from '../components/SelectFilter';
import '../index.css';
import '../components/Component.css';

function AllCardsPage() {
    const [cards, setCards] = useState([]);
    const [filteredCards, setFilteredCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [typeList, setTypeList] = useState([]);

    const [currentCards, setCurrentCards] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const cardsPerPage = 80;
    const maxPageButtons = 10;

    async function fetchCards() {
        try {
            const types = [];
            const response = await fetch('https://localhost:7114/api/Card/allcards');
            const data = await response.json();
            const array = data.monsterCards.concat(data.spellAndTrapCards);
            array.sort((a, b) => a.name.localeCompare(b.name));
            setCards(array);
            setFilteredCards(array);
            array.map((card) => {
                if (!types.includes(card.type)) {
                    types.push(card.type);
                }
            });
            types.sort();
            setTypeList(['All', ...types])
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
        setCurrentCards(filteredCards.slice(indexOfFirstCard, indexOfLastCard));
    }, [currentPage, filteredCards]);

    const pagesCount = Math.ceil(filteredCards.length / cardsPerPage);

    function handlePageChange(pageNumber) {
        if (pageNumber > 0 && pageNumber <= pagesCount) {
            setCurrentPage(pageNumber);
            window.scrollTo(0, 0);
        }
    };

    function handleFilterChange(event) {
        const selectedType = event.target.value;
        if (selectedType === 'All') {
            setFilteredCards(cards);
        } else {
            const filtered = cards.filter((card) => card.type === selectedType);
            setFilteredCards(filtered);
        }
        setCurrentPage(1);
    }

    if (loading) {
        return (
            <>
                <br />
                <div className="spinner"></div>
            </>
        );
    }

    return (
        <>
            <br />
            <h1>All Cards</h1>
            <br />
            <Pagination currentPage={currentPage} pagesCount={pagesCount} handlePageChange={handlePageChange} maxPageButtons={maxPageButtons} />
            <br />
            <SelectFilter optionsList={typeList} handleChange={handleFilterChange} />
            <div className="card-container">
                {currentCards.map((card, i) => (
                    <div key={i} className="card-wrapper">
                        <CardComponent cardData={card} />
                    </div>
                ))}
            </div>
            <br />
            <Pagination currentPage={currentPage} pagesCount={pagesCount} handlePageChange={handlePageChange} maxPageButtons={maxPageButtons} />
        </>
    );
}

export default AllCardsPage;
