import NotesCard from "../Components/NotesCard"
import "./Home.css"
import { FetchAllNotes } from "../Service/noteService"
import { useState,useEffect } from 'react';

const Home = () =>{
    const [notes, setNotes] = useState([]);

    useEffect(() => {
        const loadNotes = async () => {
            try {
                const data = await FetchAllNotes();
                setNotes(data);
            } catch (err) {
                console.error(err);
            }
        };

        loadNotes();
    }, []);

    return  <div className="pageContainer">
        <h1>Your Notes</h1>
        <input type="search" placeholder="Search" className="searchBar"/>
            <NotesCard notes= {notes}/>
        </div>
}

export default Home