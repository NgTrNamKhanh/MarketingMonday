import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import authService from "../../../services/auth.service";
import "../authentication.css";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { ScaleLoader } from "react-spinners";

const LoginBox = ({setCurrentUser}) => {
    const navigator = useNavigate();
    // State variables for email and password inputs
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [checked, setChecked] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    // Handle form submission
    const handleSubmit = async (event) => {
        setIsSubmitting(true)
        event.preventDefault();
        if (!checked) {
            setError("Please agree to out terms and conditions")
            setIsSubmitting(false)
        }else{
            if (!email || !password) {
                setError("Please fill in all fields");
                setIsSubmitting(false)
            }else {
                try {
                    const userData = await authService.login(email, password);
                    console.log("Logged in successfully:", userData);
                    setCurrentUser(userData)
                    if(userData.roles.includes('Admin')){
                        navigator("/dashboard");
                    }else if (userData.roles.includes('Coordinator')){
                        navigator(`/submissions/${userData.facultyId}`);
                    }else if (userData.roles.includes('Manager')){
                        navigator("/dashboard");
                    }else if(userData.roles.includes('Student')){
                        navigator(`/feed/${userData.facultyId}`);
                    }else if (userData.roles.includes('Guest')){
                        navigator(`/feed/${userData.facultyId}`);
                    }
                    setIsSubmitting(false)
                } catch (error) {
                    console.log(error)
                    setError(error.response.data);
                    setIsSubmitting(false)
                }
            }
        }
    };
    const handleCheckboxChange = (event) => {
        setChecked(event.target.checked);
    };
    const togglePasswordVisibility = () => {
        setShowPassword(!showPassword);
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
                        <div className="passwordField">
                            <div className="passwordFieldTop">
                                <label>Password</label>
                                <div className="passwordFieldToogle">
                                {showPassword? 
                                    <div className="togglePasswordButton"  onClick={togglePasswordVisibility}>
                                        <VisibilityOff /> 
                                        <span>Hide</span>
                                        
                                    </div>
                                    : 
                                    <div className="togglePasswordButton"  onClick={togglePasswordVisibility}>
                                        <Visibility /> 
                                        <span>Show</span>
                                    </div>
                                }
                                </div>
                            </div>
                            <input
                                type={showPassword ? "text" : "password"}
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
                        {error && (
                            <div className="error">{error}</div>
                        )}
                        <button type="submit" className="button loginButton" disabled={isSubmitting}>
                            <span className="loginButtonText">{isSubmitting? <ScaleLoader/> : 'Log In'}</span>
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default LoginBox;
