import { useState } from "react";
import Button from "../Components/Button"
import { useNavigate, Link } from 'react-router-dom';
import '../types/assets.d.ts'
import Logo from '../assets/logo.png';
import "./Login.css";
import {SignUp} from '../Service/authService';
import { SignUpRequest } from "../Dtos/Auth";

const Login = () =>{
    const navigate = useNavigate();

    const [formData, setFormData] = useState<SignUpRequest>({
        Username:"",
        Name:"",
        Phone:"",
        Email:"",
        Password:"",
        ConfirmPassword:"",
        Image:""
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

        // Optional: check passwords match
        if (formData.Password !== formData.ConfirmPassword) {
        alert("Passwords do not match!");
        return;
        }

        try {
            const payload = new FormData();
            payload.append("Username", formData.Username);
            payload.append("Name", formData.Name);
            payload.append("Phone", formData.Phone ?? "");
            payload.append("Email", formData.Email);
            payload.append("Password", formData.Password);
            payload.append("ConfirmPassword", formData.ConfirmPassword);

            //if (file) payload.append("Image", file);

            await SignUp(payload);
            navigate("/login");
        } catch (error) {
            console.error(error);
            alert("Registration failed");
        }
    };

    return  <div className="login-box">
        <img src={Logo} alt="App Logo" className='start-logo'/> 
        <h2>Welcome Back</h2>
        <form className="inputForm" onSubmit={handleSubmit}>
            <div className="inputs">
                <label className="labels">Full Name</label>
                <input type="text" name="Name" value={formData.Name} onChange={handleChange} />
            </div>
            <div className="inputs">
                <label className="labels">UserName</label>
                <input type="text" name="Username" value={formData.Username} onChange={handleChange} />
            </div>
            <div className="inputs">
                <label className="labels">Email Address</label>
                <input type="email" name="Email" value={formData.Email} onChange={handleChange} />
            </div>
            <div className="inputs">
                <label className="labels">Phone</label>
                <input type="tel" name="Phone" value={formData.Phone} onChange={handleChange} />
            </div>
            <div className="inputs">
                <label className="labels">Password</label>
                <input type="password" name="Password" value={formData.Password} onChange={handleChange} />
            </div>
            <div className="inputs">
                <label className="labels">Confirm Password</label>
                <input type="password" name="ConfirmPassword" value={formData.ConfirmPassword} onChange={handleChange} />
            </div>

            <Button type="submit" color='primary'>Create Account</Button>

        </form>

    </div>
}

export default Login