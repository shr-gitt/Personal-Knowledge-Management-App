import { Link } from 'react-router-dom';
import { FaHome, FaSearch, FaPlus, FaChartLine, FaTrash, FaCog, FaUser } from "react-icons/fa";
import "./Navbar.css";

function Navbar() {
    return (
    <nav className="sidebar">
        <Link to="/" className='item'><FaHome /><span>Home</span></Link>
        <Link to="/create_notes" className='item'><FaPlus /><span>Create Notes</span></Link>
        <Link to ="/graph_view" className='item'><FaChartLine /><span>Graph View</span></Link>
        <Link to="/trash" className='item'><FaTrash /><span>Trash</span></Link>
        
        <div className='bottom'>
            <Link to="/settings" className='item'><FaCog /><span>Settings</span></Link>
            <Link to="/profile" className='item'><FaUser /><span>Profile</span></Link>
        </div> 
    </nav>
    );
}

export default Navbar