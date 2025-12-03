import {useState} from "react";
import Button from "../Components/Button"
import { useNavigate, Link } from 'react-router-dom';
import Logo from '../assets/logo.png';
import "./Login.css";
import {SignIn} from "../Service/authService";

interface Props{
    onLogin : () => void;
}

const Login = ({onLogin}:Props) =>{
    const navigate = useNavigate();

    const handleLogin = (username: string) =>{
        localStorage.setItem("username",username);
        localStorage.setItem("isLoggedIn", true);
        onLogin();
        navigate("/");
    }

    const [formData, setFormData] = useState<SignInRequest>({            
            Email:"",
            Password:"",
            RememberMe:false
        })
    
        // Handle input changes
        const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
            const { name, value, type, checked } = e.target;
            setFormData(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value
            }));
        };
    
        // Handle form submit
        const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault(); // prevent page reload
    
            try {
                const payload = {
                    Email: formData.Email,
                    Password: formData.Password,
                    RememberMe: formData.RememberMe
                };
        
                const username = await SignIn(payload);
                handleLogin(username);
            } catch (error) {
                console.error(error);
                alert("Login failed");
            }
        };

    return  <div className="login-page">
        
        <div className="login-box">
            <img src={Logo} alt="App Logo" className='start-logo'/> 

            <h2>Welcome Back</h2>
            <form className="inputForm" onSubmit={handleSubmit}>
                <div className="inputs">
                    <label className="labels">Email Address</label>
                    <input type="email" name="Email" value={formData.Email} onChange={handleChange}/>
                </div>
                <div className="inputs">
                    <label className="labels">Password</label>
                    <input type="password" name="Password" value={formData.Password} onChange={handleChange}/>
                </div>
                
                <Button type="submit" color='primary'>Login</Button>
                <div className="signIn">
                    <label className="remember-me">
                        <input
                            type="checkbox"
                            name="RememberMe"
                            checked={formData.RememberMe}
                            onChange={handleChange}
                        />
                        Remember Me
                    </label>

                    <label className="new-user">
                        New User?
                        <Button onClick={() => navigate("/SignUp")} color='link'>Sign Up</Button>
                    </label>
                </div>
            </form>

            
        </div>
    </div>
}

export default Login