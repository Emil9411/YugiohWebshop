function SelectFilter({ optionsList, handleChange, selectedValue }) {
    return (
        <select value={selectedValue} onChange={handleChange}>
            {optionsList.map((option, index) => {
                return <option key={index} value={option}>{option}</option>
            })}
        </select>
    );
}

export default SelectFilter;
