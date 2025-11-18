import './App.css'
import {BrowserRouter as Router,Routes,Route} from 'react-router-dom'
import Login from './Pages/Login'
import Layout from './Components/Layout'

function App() {
  return(
    <Router>
      <Layout>
        <Routes>
          <Route path="/login" element={<Login />} />
        </Routes>
      </Layout>
    </Router>
  )
}

export default App