import Button from "../Components/Button"

interface Props{
    onLogin : () => void;
}

const Home = () =>{
    return  <Button onClick={() => console.log('Login Clicked')} color='secondary'>Login</Button>
}

export default Home