import './App.css';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import SignUp from './Pages/SignUp'
import Login from './Pages/Login';
import Home from './Pages/Home';
import CreateNotes from './Pages/CreateNotes';
import GraphView from './Pages/GraphView';
import Trash from './Pages/Trash';
import Settings from './Pages/Settings';
import Profile from './Pages/Profile';
import Layout from './Components/Layout';
import { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  return (
    <Router>
  <Routes>
    {/* Public route */}
    <Route 
      path="/login" 
      element={<Login onLogin={() => setIsLoggedIn(true)} />} 
    />

    <Route 
      path="/signup" 
      element={<SignUp />} 
    />

    {/* Private route wrapped by Layout */}
    <Route 
      element={<Layout />}  // Layout wraps all private pages
    >
      <Route 
        path="/" 
        element={isLoggedIn ? <Home /> : <Navigate to="/login" />} 
      />  
      <Route 
        path="/create_notes" 
        element={isLoggedIn ? <CreateNotes /> : <Navigate to="/login" />} 
      />
      <Route 
        path="/graph_view" 
        element={isLoggedIn ? <GraphView /> : <Navigate to="/login" />} 
      />
      <Route 
        path="/trash" 
        element={isLoggedIn ? <Trash /> : <Navigate to="/login" />} 
      />  
      <Route 
        path="/settings" 
        element={isLoggedIn ? <Settings /> : <Navigate to="/login" />} 
      />
      <Route 
        path="/profile" 
        element={isLoggedIn ? <Profile /> : <Navigate to="/login" />} 
      />
    </Route>
  </Routes>
</Router>

  );
}

export default App;
