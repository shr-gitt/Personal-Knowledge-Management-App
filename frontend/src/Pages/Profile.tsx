import { useNavigate } from "react-router-dom"
import "./Profile.css"

const Profile = () =>{
    const navigate = useNavigate();
    return  <div>
        <br/>
        <h3 className="information">Username: Username</h3>
        <h3 className="information">Name: Name</h3>
        <h3 className="information">Email Address: Email</h3>
        <div onClick={()=>navigate("/EditInfo")} className="button">
            Edit Information
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/ChangePassword")} className="button">
            Change Password
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/VerifyEmail")} className="button">
            Verify Account
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/TwoFA")} className="button">
            Two-Factor Authentication
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/DeleteAccount")} className="button">
            Delete Account
            <span className="chevron">›</span>
        </div>
        <div onClick={()=>navigate("/login")} className="button">
            Log Out
            <span className="chevron">›</span>
        </div>
    </div>
}

export default Profile