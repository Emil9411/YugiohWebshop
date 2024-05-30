import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "../index.css";
import swal from 'sweetalert';

function LoginPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [formSubmitted, setFormSubmitted] = useState(false);
    const navigate = useNavigate();

    async function handleSubmit(event) {
        event.preventDefault();
        setFormSubmitted(true);
        if (!email || !password) {
            return;
        }
        try {
            const response = await fetch("api/Auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email,
                    password
                }),
            });

            if (response.ok) {
                swal({
                    title: "Login Successful",
                    text: "You have successfully logged in",
                    icon: "success",
                    button: "OK"
                }).then(() => {
                    navigate("/");
                })
            } else {
                swal({
                    title: "Login Failed",
                    text: "Please check your email and password",
                    icon: "error",
                    button: "OK"
                });
                throw new Error("Login failed");
            }
        } catch (error) {
            console.error(error);
        }
    }
    return (
        <div className="login-page">
            <h2>Login</h2>
            <form onSubmit={handleSubmit}>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                {formSubmitted && email === "" ? <p style={{ color: "red" }}>Email is required</p> : <p>Email:</p>}
                            </td>
                            <td>
                                <input
                                    type="text"
                                    value={email}
                                    onChange={(event) => setEmail(event.target.value)}
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
                                    onChange={(event) => setPassword(event.target.value)}
                                />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <button onClick={() => navigate("/")} type="button">Cancel</button>
                            </td>
                            <td>
                                <button type="submit">Login</button>
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

export default LoginPage;