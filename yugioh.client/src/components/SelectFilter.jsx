function SelectFilter({ optionsList, handleChange }) {
    return (
        <select onChange={handleChange}>
            {optionsList.map((option, index) => {
                return <option key={index} value={option}>{option}</option>
            })}
        </select>
    );
}

export default SelectFilter;