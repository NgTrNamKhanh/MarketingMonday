import React, { useState } from "react";
import "../authentication.css";

const LoginBox = () => {
    const [registerAsGuest, setRegisterAsGuest] = useState(false);

    console.log(registerAsGuest)

    return (
        <div className="container">
            <div className="screen">
                {registerAsGuest ? (
                    <div className="screen__content">
                        <form className="login">
                            <div className="login__field">
                                <select className="login__input">
                                    <option value="">Select Facility</option>
                                    <option value="">Facility 1</option>
                                    <option value="">Facility 2</option>
                                </select>
                            </div>
                            <button className="button login__submit">
                                <span className="button__text">Register as Guest</span>
                                <i className="button__icon fas fa-chevron-right"></i>
                            </button>
                        </form>
                        <div className="registerSection">
                            <span>Have an account already?</span>
                            <span className="authenticationLink" onClick={() => setRegisterAsGuest(false)}>Login now</span>
                        </div>
                    </div>
                ) : (
                    <div className="screen__content">
                        <form className="login">
                            <div className="login__field">
                                <i className="login__icon fas fa-user"></i>
                                <input type="text" className="login__input" placeholder="User name / Email"/>
                            </div>
                            <div className="login__field">
                                <i className="login__icon fas fa-lock"></i>
                                <input type="password" className="login__input" placeholder="Password"/>
                            </div>
                            <button className="button login__submit">
                                <span className="button__text">Log In Now</span>
                                <i className="button__icon fas fa-chevron-right"></i>
                            </button>
                        </form>
                        <div className="registerSection">
                            <span>Don't have an account?</span><a href="/register">Register now</a>
                        </div>
                        <div className="registerSection">
                            <span>Want to log in as guest?</span>
                            <span className="authenticationLink" onClick={() => setRegisterAsGuest(true)}>Click here</span>
                        </div>
                    </div>
                )}

                <div className="screen__background">
                    <span className="screen__background__shape screen__background__shape4"></span>
                    <span className="screen__background__shape screen__background__shape3"></span>
                    <span className="screen__background__shape screen__background__shape2"></span>
                    <span className="screen__background__shape screen__background__shape1"></span>
                </div>
            </div>
        </div>
    );
};

export default LoginBox;
