import "../authentication.css";

const LoginBox = () => {

    return (
        <div className="login">
            <div className="loginWrapper">
                <div className="loginPicture">
                    <img src={process.env.PUBLIC_URL + '/pictures/login.jpg'} alt="My Image" />
                </div>
                <div className="loginFormWrapper">
                    <form className="loginForm">
                        <div className="loginTitle">
                            <h1>Login</h1>
                            <span>Log into CMUniversity</span>
                        </div>
                        <div className="loginField">
                            <label>Email</label>
                            <input type="text" className="loginInput"/>
                        </div>
                        <div className="loginField">
                            <label>Password</label>
                            <input type="password" className="loginInput"/>
                            <span className="passwordAdd">Use 8 or more characters with a mix of letters, numbers & symbols</span>
                        </div>
                        <div className="checkBox">
                            <input type="checkbox" name="tnd" id="tnd" className="checkBoxInput"/>
                            <label for="tnd">Agree to our <a href="">Terms of Use</a> and <a href="">Privacy Policy</a></label>
                        </div>
                        <button className="button loginButton">
                            <span className="loginButtonText">Log In</span>
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default LoginBox;
