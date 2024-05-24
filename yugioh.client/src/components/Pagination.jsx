import React from 'react';

function Pagination({ currentPage, pagesCount, handlePageChange }) {
    const renderPageButtons = () => {
        const buttons = [];
        const maxPageButtons = 10; // Maximum number of page buttons to display
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

    return (
        <div className="pagination">
            <button onClick={() => { handlePageChange(1); }} disabled={currentPage === 1}>First</button>
            <button onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>Previous</button>
            {renderPageButtons()}
            <button onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === pagesCount}>Next</button>
            <button onClick={() => { handlePageChange(pagesCount); }} disabled={currentPage === pagesCount}>Last</button>
        </div>
    );
}

export default Pagination;
