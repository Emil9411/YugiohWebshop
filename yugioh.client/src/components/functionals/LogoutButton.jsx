import { useNavigate, useLocation } from 'react-router-dom';
import swal from 'sweetalert';

function LogoutButton() {
    const navigate = useNavigate();
    const location = useLocation();

    async function handleLogout() {
        try {
            const response = await fetch("api/Auth/logout", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error("Logout failed");
            } else {
                if (location.pathname !== "/") {
                    swal({
                        title: "Logout Successful",
                        text: "You have successfully logged out",
                        icon: "success",
                        button: "OK"
                    }).then(() => {
                        navigate("/");
                        window.location.reload();
                    })
                } else {
                    swal({
                        title: "Logout Successful",
                        text: "You have successfully logged out",
                        icon: "success",
                        button: "OK"
                    }).then(() => {
                        window.location.reload();
                    })
                }
            }
        } catch (error) {
            console.error(error);
        }
    }
    return (
        <>
            <button onClick={handleLogout}>Logout</button>
        </>
    );
}

export default LogoutButton;