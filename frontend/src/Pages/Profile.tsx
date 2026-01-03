import { useNavigate } from "react-router-dom";
import "./Profile.css";
import { Logout } from "../Service/authService";
import {
  User,
  Shield,
  Key,
  Trash2,
  LogOut,
  CheckCircle,
  ChevronRight,
} from "lucide-react";

interface SectionProps {
  title: string;
  children: React.ReactNode;
  danger?: boolean;
}

interface RowProps {
  icon: React.ReactNode;
  title: string;
  description: string;
  click?: string;
  onClick?: () => void;
  danger?: boolean;
}

const Profile = () => {
  const handleLogout = async () => {
    try {
      await Logout();
      localStorage.removeItem("isLoggedIn");
      localStorage.removeItem("username");
      navigate("/login");
    } catch (error) {
      console.error(error);
      alert("Logout failed");
    }
  };

  const navigate = useNavigate();

  const Section = ({ title, children, danger }: SectionProps) => (
    <div className={`section ${danger ? "danger" : ""}`}>
      <h3>{title}</h3>
      <div className="section-card">{children}</div>
    </div>
  );

  const Row = ({
    icon,
    title,
    description,
    click,
    onClick,
    danger,
  }: RowProps) => (
    <div
      onClick={onClick ? onClick : () => click && navigate(click)}
      className={`row ${danger ? "row-danger" : ""}`}
    >
      <div className="row-left">
        <span className="row-icon">{icon}</span>
        <div className="row-group">
          <div>
            <p className="row-title">{title}</p>
            <p className="row-desc">{description}</p>
          </div>
          <ChevronRight className="chevron" />
        </div>
      </div>
    </div>
  );

  return (
    <div>
      <div className="profile-card">
        <div className="avatar">M</div>
        <div className="profile-info">
          <h2>Mandip Shrestha</h2>
          <p className="username">@mandip</p>
          <p className="email">mandip@email.com</p>
        </div>
      </div>

      <Section title="Account">
        <Row
          icon={<User />}
          title="Edit Profile"
          description="Update your name and email"
          click="/EditInfo"
        />
        <Row
          icon={<CheckCircle />}
          title="Verify Account"
          description="Confirm your email address"
          click="/VerifyEmail"
        />
      </Section>
      <Section title="Security">
        <Row
          icon={<Key />}
          title="Change Password"
          description="Update your login credentials"
          click="/ChangePassword"
        />
        <Row
          icon={<Shield />}
          title="Two-Factor Authentication"
          description="Add an extra layer of security"
          click="/TwoFA"
        />
      </Section>

      <Section title="Danger Zone" danger>
        <Row
          icon={<Trash2 />}
          title="Delete Account"
          description="Permanently remove your account"
          click="/DeleteAccount"
          danger
        />
        <Row
          icon={<LogOut />}
          title="Log Out"
          description="Sign out from this device"
          onClick={handleLogout}
          danger
        />
      </Section>
    </div>
  );
};

export default Profile;