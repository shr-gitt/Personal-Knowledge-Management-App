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
        {notes.map(note => (
                <NotesCard
                    key={note.id}
                    notes={[
                        {
                            id: note.id,
                            title: note.title,
                            content: note.content,
                            updatedAt: note.lastModified
                        }
                    ]}
                />
            ))}
        </div>
}

export default Home