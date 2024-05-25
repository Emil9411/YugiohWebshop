import { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import Pagination from '../components/Pagination';
import SelectFilter from '../components/SelectFilter';
import SearchBar from '../components/SearchBar';
import '../index.css';
import '../components/Component.css';

function AllCardsPage() {
    const [cards, setCards] = useState([]);
    const [filteredCards, setFilteredCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [typeList, setTypeList] = useState([]);
    const [archetypeList, setArchetypeList] = useState([]);
    const [attributeList, setAttributeList] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedType, setSelectedType] = useState('All');
    const [currentCards, setCurrentCards] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const cardsPerPage = 80;
    const maxPageButtons = 10;

    async function fetchCards() {
        try {
            const types = [];
            const archetypes = [];
            const attributes = [];
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
                if (card.archetype && !archetypes.includes(card.archetype)) {
                    archetypes.push(card.archetype);
                }
                if (card.attribute && !attributes.includes(card.attribute)) {
                    attributes.push(card.attribute);
                }
            });
            types.sort();
            setTypeList(['All', ...types])
            setArchetypeList(['All', ...archetypes]);
            setAttributeList(['ALL', ...attributes]);
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

    useEffect(() => {
        let filtered = cards;
        if (selectedType !== 'All') {
            filtered = filtered.filter((card) => card.type === selectedType);
        }
        if (searchTerm) {
            filtered = filtered.filter((card) => card.name.toLowerCase().includes(searchTerm.toLowerCase()));
        }
        setFilteredCards(filtered);
        setCurrentPage(1);
    }, [selectedType, searchTerm, cards]);

    const pagesCount = Math.ceil(filteredCards.length / cardsPerPage);

    function handlePageChange(pageNumber) {
        if (pageNumber > 0 && pageNumber <= pagesCount) {
            setCurrentPage(pageNumber);
            window.scrollTo(0, 0);
        }
    }

    function handleFilterChange(event) {
        setSelectedType(event.target.value);
    }

    function handleSearchChange(event) {
        setSearchTerm(event.target.value);
    }

    function handleClearFilter() {
        setSelectedType('All');
    }

    function handleClearSearch() {
        setSearchTerm('');
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
            <table>
                <thead>
                    <tr>
                        <th>
                            <h3>Filter by Type:</h3>
                        </th>
                        <th>
                            <SelectFilter optionsList={typeList} handleChange={handleFilterChange} />
                        </th>
                        <th>
                            <button onClick={handleClearFilter}>Clear filter</button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <h3>Search by Name:</h3>
                        </td>
                        <td>
                            <SearchBar value={searchTerm} onChange={handleSearchChange} placeholder="Search by name" />
                        </td>
                        <td>
                            <button onClick={handleClearSearch}>Clear search</button>
                        </td>
                    </tr>
                </tbody>
            </table>
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
