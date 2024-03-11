import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import authService from "../../../services/auth.service";
import "../authentication.css";

const LoginBox = ({setCurrentUser}) => {
    const navigator = useNavigate();
    // State variables for email and password inputs
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [checked, setChecked] = useState(false);
    // Handle form submission
    const handleSubmit = async (event) => {

        event.preventDefault();
        if (!checked) {
            setError("Please agree to out terms and conditions")
        }else{
            if (!email || !password) {
                setError("Please fill in all fields");
                return;
            }else {
                try {
                    const userData = await authService.login(email, password);
                    console.log("Logged in successfully:", userData);
                    setCurrentUser(userData)
                    navigator("/");
                } catch (error) {
                    console.log(error)
                    setError(error);
                }
            }
        }
    };
    const handleCheckboxChange = (event) => {
        setChecked(event.target.checked);
    };
    return (
        <div className="login">
            <div className="loginWrapper">
                <div className="loginPicture">
                    <img src={process.env.PUBLIC_URL + '/pictures/login.jpg'} alt="My Image" />
                </div>
                <div className="loginFormWrapper">
                    <form className="loginForm" onSubmit={handleSubmit}>
                        <div className="loginTitle">
                            <h1>Login</h1>
                            <span>Log into CMUniversity</span>
                        </div>
                        
                        <div className="loginField">
                            <label>Email</label>
                            <input
                                type="text"
                                className="loginInput"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </div>
                        <div className="loginField">
                            <label>Password</label>
                            <input
                                type="password"
                                className="loginInput"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                            <span className="passwordAdd">Use 8 or more characters with a mix of letters, numbers & symbols</span>
                        </div>
                        <div className="checkBox">
                            <input type="checkbox" name="tnd" id="tnd" className="checkBoxInput" onChange={handleCheckboxChange}/>
                            <label htmlFor="tnd">Agree to our <a href="">Terms of Use</a> and <a href="">Privacy Policy</a></label>
                        </div>
                        <div className="error">{error}</div>
                        <button type="submit" className="button loginButton">
                            <span className="loginButtonText">Log In</span>
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default LoginBox;
