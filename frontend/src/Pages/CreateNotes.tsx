import { useState, useEffect } from "react"
import Button from "../Components/Button";
import { useNavigate } from "react-router-dom";
import './CreateNotes.css'
import { CreateNote } from "../Service/noteService";

const CreateNotes = () =>{
    const [text, setText] = useState("");

    const title = text.split("\n")[0];
    const body = text.split("\n").slice(1).join("\n");

    const saveNote = async () => {
        if (!text.trim()) return;

        return await CreateNote({
            UserId: localStorage.getItem("username"),
            Title: title,
            Content: body
        });
    };

    // Auto-save on navigation (component unmount)
    useEffect(() => {
        return () => {
            saveNote(); // not awaited, but triggers
        };
    }, []);

    // Save to localStorage on tab close (backend won't work here)
    useEffect(() => {
        const handleUnload = () => {
            localStorage.setItem("draftNote", text);
        };
        window.addEventListener("beforeunload", handleUnload);
        return () => window.removeEventListener("beforeunload", handleUnload);
    }, [text]);

    // Proper async handler
    const handleSaveClick = async () => {
        await saveNote();
        navigate("/");
    };

    const navigate = useNavigate();

    return(<div className="page">
        <textarea
            value = {text}
            onChange={(e)=>setText(e.target.value)}
            rows={10}
            className="noteBox"
            placeholder="Write new note here."
        />
        <div className="saveButton">
            <Button onClick={handleSaveClick} color="primary">Save</Button>
        </div>
    </div>)
}

export default CreateNotes