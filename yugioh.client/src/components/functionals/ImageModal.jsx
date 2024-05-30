import React, { useRef } from 'react';

function ImageModal({ imageUrl, onClose }) {
    const modalRef = useRef(null);

    const handleClickOutside = (event) => {
        if (modalRef.current && !modalRef.current.contains(event.target)) {
            onClose(); // Close the modal when clicked outside
        }
    };

    return (
        <div className="modal" onClick={handleClickOutside}>
            <div className="modal-content" onClick={handleClickOutside}>
                <img src={imageUrl} alt="Card" className="modal-image" ref={modalRef} />
            </div>
        </div>
    );
}

export default ImageModal;