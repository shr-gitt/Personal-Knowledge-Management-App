import { Link } from 'react-router-dom';
import { FaHome, FaPlus, FaChartLine, FaUser } from "react-icons/fa";
import "./Navbar.css";
import Logo from "../assets/logo.png"

function Navbar() {
    return <div>
        <nav className="sidebar">
            <Link to="/" className='item'><FaHome /><span>Home</span></Link>
            <Link to="/create_notes" className='item'><FaPlus /><span>Create Notes</span></Link>
            <Link to ="/graph_view" className='item'><FaChartLine /><span>Graph View</span></Link>
            
            <div className='bottom'>
                <Link to="/profile" className='item'><FaUser /><span>Profile</span></Link>
            </div> 
        </nav>
        <img src={Logo} alt="App Logo" className='nav-logo'/> 
    </div>
}

export default Navbar