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
        onLogin();
        navigate("/");
    }

    const [formData, setFormData] = useState<SignInRequest>({            
            Email:"",
            Password:"",
            RememberMe:""
        })
    
        // Handle input changes
        const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
            const { name, value } = e.target;
            setFormData(prev => ({
            ...prev,
            [name]: value
            }));
        };
    
        // Handle form submit
        const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault(); // prevent page reload
    
            try {
                const payload = {
                    Email: formData.Email,
                    Password: formData.Password,
                    RememberMe: formData.RememberMe === "on"
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
                <input type="checkbox" name="RememberMe" value={formData.RememberMe} />
                <Button type="submit" color='primary'>Login</Button>
            </form>

            <div className="signIn">
                <label className="labels">New User?</label>
                <Button onClick={() => navigate("/SignUp")} color='link'>Sign In</Button>
            </div>
        </div>
    </div>
}

export default Login