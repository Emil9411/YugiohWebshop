import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "../index.css";

function RegistrationPage() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [passwordConfirmation, setPasswordConfirmation] = useState("");
    const [formSubmitted, setFormSubmitted] = useState(false);
    const navigate = useNavigate();

    async function handleSubmit(event) {
        event.preventDefault();
        setFormSubmitted(true);
        if (password !== passwordConfirmation) {
            return;
        }
        if (!username || !email || !password) {
            return;
        }
        try {
            const response = await fetch("api/Auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email,
                    username,
                    password
                }),
            });

            if (response.ok) {
                navigate("/login");
            } else {
                throw new Error("Registration failed");
            }
        } catch (error) {
            console.error(error);
        }
    }
    return (
        <div className="registration-page">
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                {formSubmitted && username === "" ? <p style={{ color: "red" }}>Username is required</p> : <p>Username:</p>}
                            </td>
                            <td>
                                <input
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                {formSubmitted && email === "" ? <p style={{ color: "red" }}>Email is required</p> : <p>Email:</p>}
                            </td>
                            <td>
                                <input
                                    type="email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                {formSubmitted && password === "" ? <p style={{ color: "red" }}>Password is required</p> : <p>Password:</p>}
                            </td>
                            <td>
                                <input
                                    type="password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                {formSubmitted && passwordConfirmation === "" ? <p style={{ color: "red" }}>Password confirmation is required</p> : <p>Password confirmation:</p>}
                            </td>
                            <td>
                                <input
                                    type="password"
                                    value={passwordConfirmation}
                                    onChange={(e) => setPasswordConfirmation(e.target.value)}
                                />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <button onClick={() => navigate("/")} type="button">Cancel</button>
                            </td>
                            <td>
                                <button type="submit">Register</button>
                            </td>
                        </tr>
                        <tr rowSpan="2">
                        <td colSpan="2"></td>
                        </tr>
                    </tbody>
                </table>
            </form>
        </div>
    );
}

export default RegistrationPage;