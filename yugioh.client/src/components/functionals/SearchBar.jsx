function SearchBar({ value, onChange, placeholder }) {
    return (
        <input
            type="text"
            value={value}
            onChange={onChange}
            placeholder={placeholder || "Search"}
        />
    );
}

export default SearchBar;
