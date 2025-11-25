import Button from "../Components/Button"
import { useNavigate, Link } from 'react-router-dom';
import Logo from '../assets/logo.png';
import "./Login.css";

const Login = () =>{
    const navigate = useNavigate();

    return  <div className="layout">
        <img src={Logo} alt="App Logo" className='logo'/> 
        <h2>Welcome Back</h2>
        <form className="inputForm">
            <div className="inputs">
                <label className="labels">Full Name</label>
                <input type="text" name="Full Name" />
            </div>
            <div className="inputs">
                <label className="labels">UserName</label>
                <input type="text" name="UserName" />
            </div>
            <div className="inputs">
                <label className="labels">Email Address</label>
                <input type="email" name="Email Address" />
            </div>
            <div className="inputs">
                <label className="labels">Password</label>
                <input type="password" name="Password" />
            </div>
            <div className="inputs">
                <label className="labels">Confirm Password</label>
                <input type="password" name="Confirm Password" />
            </div>
        </form>

        <Button onClick={() => navigate("/")} color='primary'>Create Account</Button>
    </div>
}

export default Login