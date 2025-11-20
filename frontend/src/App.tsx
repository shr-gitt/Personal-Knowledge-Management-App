import './App.css';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './Pages/Login';
import Home from './Pages/Home';
import Layout from './Components/Layout';
import { useState } from 'react';

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

    {/* Private route wrapped by Layout */}
    <Route 
      element={<Layout children={undefined} />}  // Layout wraps all private pages
    >
      <Route 
        path="/" 
        element={isLoggedIn ? <Home /> : <Navigate to="/login" />} 
      />
      {/* Add more private pages here */}
    </Route>
  </Routes>
</Router>

  );
}

export default App;
