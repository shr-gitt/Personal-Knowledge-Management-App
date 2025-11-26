import { useNavigate } from "react-router-dom"
import "./Profile.css"

const Profile = () =>{
    const navigate = useNavigate();
    return  <div>
        <h3 className="information">Username: Username</h3>
        <h3 className="information">Name: Name</h3>
        <h3 className="information">Email Address: Email</h3>
        <div onClick={()=>navigate("/")} className="button">
            Edit Information
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/")} className="button">
            Change Password
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/")} className="button">
            Verify Account
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/")} className="button">
            Two-Factor Authentication
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/")} className="button">
            Delete Account
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/")} className="button">
            Log Out
            <span className="chevron">›</span>
        </div>
    </div>
}

export default Profile