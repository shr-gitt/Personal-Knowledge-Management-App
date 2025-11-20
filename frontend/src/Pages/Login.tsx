import Button from "../Components/Button"
import { useNavigate } from "react-router-dom";

interface Props{
    onLogin : () => void;
}

const Login = ({onLogin}:Props) =>{
    const navigate = useNavigate();

    const handleLogin = () =>{
        onLogin();
        navigate("/");
    }

    return  <Button onClick={handleLogin} color='secondary'>Login</Button>
}

export default Login