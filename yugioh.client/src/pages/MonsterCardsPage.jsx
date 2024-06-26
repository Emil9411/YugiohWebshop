import { useState, useEffect } from 'react';
import CardComponent from '../components/CardComponent';
import Pagination from '../components/functionals/Pagination';
import SelectFilter from '../components/functionals/SelectFilter';
import SearchBar from '../components/functionals/SearchBar';
import '../index.css';
import '../components/Component.css';

function MonsterCardsPage() {
    const [cards, setCards] = useState([]);
    const [filteredCards, setFilteredCards] = useState([]);

    const [loading, setLoading] = useState(true);

    const [typeList, setTypeList] = useState([]);
    const [filteredTypeList, setFilteredTypeList] = useState([]);
    const [selectedType, setSelectedType] = useState('All');

    const [archetypeList, setArchetypeList] = useState([]);
    const [filteredArchetypeList, setFilteredArchetypeList] = useState([]);
    const [selectedArchetype, setSelectedArchetype] = useState('All');

    const [attributeList, setAttributeList] = useState([]);
    const [filteredAttributeList, setFilteredAttributeList] = useState([]);
    const [selectedAttribute, setSelectedAttribute] = useState('ALL');

    const [raceList, setRaceList] = useState([]);
    const [filteredRaceList, setFilteredRaceList] = useState([]);
    const [selectedRace, setSelectedRace] = useState('All');

    const [levelList, setLevelList] = useState([]);
    const [filteredLevelList, setFilteredLevelList] = useState([]);
    const [selectedLevel, setSelectedLevel] = useState('All');

    const [minAttack, setMinAttack] = useState(null);
    const [maxAttack, setMaxAttack] = useState(null);

    const [minDefense, setMinDefense] = useState(null);
    const [maxDefense, setMaxDefense] = useState(null);

    const [searchTerm, setSearchTerm] = useState('');

    const [currentCards, setCurrentCards] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);

    const cardsPerPage = 80;
    const maxPageButtons = 10;

    async function fetchCards() {
        try {
            const types = [];
            const archetypes = [];
            const attributes = [];
            const races = [];
            const levels = [];
            const response = await fetch('/api/Card/allmonstercards');
            const array = await response.json();
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
                if (card.race && !races.includes(card.race)) {
                    races.push(card.race);
                }
                if (card.level && !levels.includes(card.level)) {
                    levels.push(card.level);
                }
            });
            types.sort();
            archetypes.sort();
            attributes.sort();
            races.sort();
            levels.sort((a, b) => a - b);
            setTypeList(['All', ...types]);
            setFilteredTypeList(['All', ...types]);
            setArchetypeList(['All', ...archetypes]);
            setFilteredArchetypeList(['All', ...archetypes]);
            setAttributeList(['ALL', ...attributes]);
            setFilteredAttributeList(['ALL', ...attributes]);
            setRaceList(['All', ...races]);
            setFilteredRaceList(['All', ...races]);
            setLevelList(['All', ...levels]);
            setFilteredLevelList(['All', ...levels]);
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
        let filteredTypes = typeList;
        let filteredArchetypes = archetypeList;
        let filteredAttributes = attributeList;
        let filteredRaces = raceList;
        let filteredLevels = levelList;
        if (selectedType !== 'All') {
            filtered = filtered.filter((card) => card.type === selectedType);
        }
        if (selectedArchetype !== 'All') {
            filtered = filtered.filter((card) => card.archetype === selectedArchetype);
        }
        if (selectedAttribute !== 'ALL') {
            filtered = filtered.filter((card) => card.attribute === selectedAttribute);
        }
        if (selectedRace !== 'All') {
            filtered = filtered.filter((card) => card.race === selectedRace);
        }
        if (selectedLevel != 'All') {
            filtered = filtered.filter((card) => card.level == selectedLevel);
        }
        if (minAttack) {
            filtered = filtered.filter((card) => card.attack >= minAttack);
        }
        if (maxAttack) {
            filtered = filtered.filter((card) => card.attack <= maxAttack);
        }
        if (minDefense) {
            filtered = filtered.filter((card) => card.defense >= minDefense);
        }
        if (maxDefense) {
            filtered = filtered.filter((card) => card.defense <= maxDefense);
        }
        if (searchTerm) {
            filtered = filtered.filter((card) => card.name.toLowerCase().includes(searchTerm.toLowerCase()));
        }
        setFilteredCards(filtered);
        filteredTypes = ['All'].concat([...new Set(filtered.map((card) => card.type))].filter(Boolean).sort());
        filteredArchetypes = ['All'].concat([...new Set(filtered.map((card) => card.archetype).filter(Boolean))].sort());
        filteredAttributes = ['ALL'].concat([...new Set(filtered.map((card) => card.attribute).filter(Boolean))].sort());
        filteredRaces = ['All'].concat([...new Set(filtered.map((card) => card.race).filter(Boolean))].sort());
        filteredLevels = ['All'].concat([...new Set(filtered.map((card) => card.level).filter(Boolean))].sort((a, b) => a - b));
        setFilteredTypeList(filteredTypes);
        setFilteredArchetypeList(filteredArchetypes);
        setFilteredAttributeList(filteredAttributes);
        setFilteredRaceList(filteredRaces);
        setFilteredLevelList(filteredLevels);
        setCurrentPage(1);
    }, [selectedType, searchTerm, cards, selectedArchetype, typeList, archetypeList, selectedAttribute, attributeList, selectedRace, raceList, selectedLevel, levelList, minAttack, maxAttack, minDefense, maxDefense]);


    const pagesCount = Math.ceil(filteredCards.length / cardsPerPage);

    function handlePageChange(pageNumber) {
        if (pageNumber > 0 && pageNumber <= pagesCount) {
            setCurrentPage(pageNumber);
            window.scrollTo(0, 0);
        }
    }

    function handleTypeFilterChange(event) {
        setSelectedType(event.target.value);
    }

    function handleArchetypeFilterChange(event) {
        setSelectedArchetype(event.target.value);
    }

    function handleAttributeFilterChange(event) {
        setSelectedAttribute(event.target.value);
    }

    function handleRaceFilterChange(event) {
        setSelectedRace(event.target.value);
    }

    function handleLevelFilterChange(event) {
        setSelectedLevel(event.target.value);
    }

    function handleMinAttackChange(event) {
        setMinAttack(event.target.value);
    }

    function handleMaxAttackChange(event) {
        setMaxAttack(event.target.value);
    }

    function handleSearchChange(event) {
        setSearchTerm(event.target.value);
    }

    function handleMinDefenseChange(event) {
        setMinDefense(event.target.value);
    }

    function handleMaxDefenseChange(event) {
        setMaxDefense(event.target.value);
    }

    function handleClearFilters() {
        setSelectedType(typeList[0]);
        setSelectedArchetype(archetypeList[0]);
        setSelectedAttribute(attributeList[0]);
        setSelectedRace(raceList[0]);
        setSelectedLevel(levelList[0]);
        setMinAttack('');
        setMaxAttack('');
        setMinDefense('');
        setMaxDefense('');
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
            <h1>Monster Cards</h1>
            <br />
            <Pagination currentPage={currentPage} pagesCount={pagesCount} handlePageChange={handlePageChange} maxPageButtons={maxPageButtons} />
            <br />
            <table>
                <thead>
                    <tr>
                        <th>
                            <h3>Type:</h3>
                        </th>
                        <th>
                            <h3>Archetype:</h3>
                        </th>
                        <th>
                            <h3>Attribute:</h3>
                        </th>
                        <th>
                            <h3>Race:</h3>
                        </th>
                        <th>
                            <h3>Level/Rank:</h3>
                        </th>
                        <th>
                            <h3>Search by Name:</h3>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <SelectFilter optionsList={filteredTypeList} handleChange={handleTypeFilterChange} selectedValue={selectedType} />
                        </td>
                        <td>
                            <SelectFilter optionsList={filteredArchetypeList} handleChange={handleArchetypeFilterChange} selectedValue={selectedArchetype} />
                        </td>
                        <td>
                            <SelectFilter optionsList={filteredAttributeList} handleChange={handleAttributeFilterChange} selectedValue={selectedAttribute} />
                        </td>
                        <td>
                            <SelectFilter optionsList={filteredRaceList} handleChange={handleRaceFilterChange} selectedValue={selectedRace} />
                        </td>
                        <td>
                            <SelectFilter optionsList={filteredLevelList} handleChange={handleLevelFilterChange} selectedValue={selectedLevel} />
                        </td>
                        <td>
                            <SearchBar value={searchTerm} onChange={handleSearchChange} placeholder="Search by name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Minimum ATK:</h3>
                        </td>
                        <td>
                            <h3>Maximum ATK:</h3>
                        </td>
                        <td>
                            <h3>Minimum DEF:</h3>
                        </td>
                        <td>
                            <h3>Maximum DEF:</h3>
                        </td>
                        <td>
                        </td>
                        <td>
                            <button onClick={handleClearSearch}>Clear search</button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="number" value={minAttack || ''} onChange={handleMinAttackChange} />
                        </td>
                        <td>
                            <input type="number" value={maxAttack || ''} onChange={handleMaxAttackChange} />
                        </td>
                        <td>
                            <input type="number" value={minDefense || ''} onChange={handleMinDefenseChange} />
                        </td>
                        <td>
                            <input type="number" value={maxDefense || ''} onChange={handleMaxDefenseChange} />
                        </td>
                        <td colSpan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colSpan="5">
                            <button onClick={handleClearFilters}>Clear filters</button>
                        </td>
                        <td>
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

export default MonsterCardsPage;