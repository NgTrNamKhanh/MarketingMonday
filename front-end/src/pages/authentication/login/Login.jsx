import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import authService from "../../../services/auth.service";
import "../authentication.css";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { ScaleLoader } from "react-spinners";
import DialogDefault from "../../../components/dialogs/dialog/DialogDefault"
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
    const [showTermsDialog, setShowTermsDialog] = useState(false);
    const termsHeaderText = (
        <>
            <p>By using our services, you agree to abide by the following terms and conditions:</p>
        </>
    );
    const termsContent = (
        <div>
            <ul>
                <li> You must be 13 years or older to use this service.</li>
                <li> You agree not to misuse or interfere with the services provided.</li>
                <li> We reserve the right to suspend or terminate your account if you violate any of these terms.</li>
            </ul>
        </div>
    );
    const openTermsDialog = () => {
        setShowTermsDialog(true);
    };

    const closeTermsDialog = () => {
        setShowTermsDialog(false);
    };
    const [showPrivacyDialog, setShowPrivacyDialog] = useState(false);
    const privacyHeaderText = (
        <>
            <p>Your privacy is important to us. This Privacy Policy explains how we collect, use, disclose, and safeguard
            your information when you use our services. Please read this Privacy Policy carefully.</p>
        </>
    );
    const privacyContent = (
        <div>
            <ul>
                <li>
                    <strong>Information We Collect</strong>
                    <p>
                        We collect personal information such as your name, email address, and contact information when you register
                        for an account. We also collect usage data and device information when you interact with our services.
                    </p>
                </li>
                <li>
                    <strong>How We Use Your Information</strong>
                    <p>
                        We use your information to provide and improve our services, communicate with you, and personalize your
                        experience. We may also use your information for research and analysis purposes.
                    </p>
                </li>
                <li>
                    <strong>Sharing of Your Information</strong>
                    <p>
                        We may share your information with third-party service providers who assist us in providing our services.
                        We may also disclose your information in response to legal requests or to protect our rights or the rights of
                        others.
                    </p>
                </li>
                <li>
                    <strong>Security of Your Information</strong>
                    <p>
                        We take reasonable measures to protect your information from unauthorized access, use, or disclosure. However,
                        no method of transmission over the Internet or electronic storage is 100% secure.
                    </p>
                </li>
                <li>
                    <strong>Changes to This Privacy Policy</strong>
                    <p>
                        We may update this Privacy Policy from time to time. We will notify you of any changes by posting the new
                        Privacy Policy on this page.
                    </p>
                </li>
            </ul>
        </div>
    );
    
    const openPrivacyDialog = () => {
        setShowPrivacyDialog(true);
    };

    const closePrivacyDialog = () => {
        setShowPrivacyDialog(false);
    };



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
                    navigator("/");
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
                            <label htmlFor="tnd">
                                Agree to our{" "}
                                <a href="#" onClick={openTermsDialog}>
                                    Terms of Use
                                </a>{" "}
                                and{" "}
                                <a href="#" onClick={openPrivacyDialog}>
                                    Privacy Policy
                                </a>
                            </label>
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
            <DialogDefault
                open={showTermsDialog}
                handleClose={closeTermsDialog}
                contentText={termsContent}
                headerText={termsHeaderText}
                title="Terms of Use"
            />
            <DialogDefault
                open={showPrivacyDialog}
                handleClose={closePrivacyDialog}
                contentText={privacyContent}
                headerText={privacyHeaderText}
                title="Privacy Policy"
            />
        </div>
    );
};

export default LoginBox;
