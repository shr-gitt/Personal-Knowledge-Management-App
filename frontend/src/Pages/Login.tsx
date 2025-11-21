import Button from "../Components/Button"
import { useNavigate, Link } from 'react-router-dom';
import Logo from '../assets/logo.png';
import "./Login.css";

interface Props{
    onLogin : () => void;
}

const Login = ({onLogin}:Props) =>{
    const navigate = useNavigate();

    const handleLogin = () =>{
        onLogin();
        navigate("/");
    }

    return  <div className="layout">
        <img src={Logo} alt="App Logo" className='logo'/> 
        <h2>Welcome Back</h2>
        <form className="inputForm">
            <div className="inputs">
                <label className="labels">Email Address</label>
                <input type="email" name="Email Address" />
            </div>
            <div className="inputs">
                <label className="labels">Password</label>
                <input type="password" name="Password" />
            </div>
        </form>

        <Button onClick={handleLogin} color='primary'>Login</Button>
        <div className="signIn">
            <label className="labels">New User?</label>
            <Button onClick={handleLogin} color='link'>Sign In</Button>
        </div>
    </div>
}

export default Login