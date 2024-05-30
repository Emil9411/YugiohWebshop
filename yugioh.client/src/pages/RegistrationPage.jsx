import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "../index.css";
import swal from 'sweetalert';

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
            swal({
                title: "Registration Failed",
                text: "Passwords do not match",
                icon: "error",
                button: "OK"
            });
            return;
        }
        if (!username || !email || !password) {
            swal({
                title: "Registration Failed",
                text: "Please fill out all fields",
                icon: "error",
                button: "OK"
            });
            return;
        }
        if (username.toLowerCase().includes("admin")) {
            swal({
                title: "Registration Failed",
                text: "Username cannot contain 'admin'",
                icon: "error",
                button: "OK"
            });
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
            const data = await response.json();

            if (response.ok) {
                swal({
                    title: "Registration Successful",
                    text: "You have successfully registered",
                    icon: "success",
                    button: "OK"
                }).then(() => {
                    navigate("/login");
                })
            } else {
                swal({
                    title: "Registration Failed",
                    text: data.token,
                    icon: "error",
                    button: "OK"
                });
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
                                {formSubmitted && username === "" ? <p style={{ color: "red" }}>Username is required</p> : formSubmitted && username.toLowerCase().includes("admin") ? <p style={{ color: "red" }}> Username cannot contain &quot;admin&quot;</p> : <p>Username:</p>}
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