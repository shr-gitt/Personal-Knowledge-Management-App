import { useState, useEffect } from "react"
import Button from "../Components/Button";
import Tags from "../Components/Tags";
import { useNavigate } from "react-router-dom";
import './CreateNotes.css'
import { CreateNote } from "../Service/noteService";

const CreateNotes = () =>{
    const [text, setText] = useState("");
    const [tags, setTags] = useState<string[]>([]);  // List of tags (as an array)
    const [tagInput, setTagInput] = useState("");  // Input field for new tag

    const title = text.split("\n")[0];
    const body = text.split("\n").slice(1).join("\n");

    const saveNote = async () => {
        if (!text.trim()) return;

        return await CreateNote({
            UserId: localStorage.getItem("username"),
            Title: title,
            Content: body,
            Tags: tags
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
            localStorage.setItem("draftNoteTags", tags);
        };
        window.addEventListener("beforeunload", handleUnload);
        return () => window.removeEventListener("beforeunload", handleUnload);
    }, [tags]);

    useEffect(() => {
        const handleUnload = () => {
            localStorage.setItem("draftNote", text);
        };
        window.addEventListener("beforeunload", handleUnload);
        return () => window.removeEventListener("beforeunload", handleUnload);
    }, [text]);

    // Function to handle adding new tag
    const handleAddTag = () => {
        if (tagInput.trim()) {
            setTags((prevTags) => [...prevTags, tagInput.trim()]);
            setTagInput("");  // Clear input after adding
        }
    };

    // Handle Enter key press to add tag
    const handleKeyPress = (e: React.KeyboardEvent) => {
        if (e.key === "Enter") {
            handleAddTag();
        }
    };

    // Proper async handler
    const handleSaveClick = async () => {
        await saveNote();
        navigate("/");
    };

    const navigate = useNavigate();

    return(<div className="page">
        <div className = "Tag">
            {/* Tag Input */}
            <input
                type="text"
                value={tagInput}
                onChange={(e) => setTagInput(e.target.value)}
                onKeyPress={handleKeyPress}
                className="tagBox"
                placeholder="Add a tag and press Enter."
            />
            {/* Add Tag Button */}
            <Button onClick={handleAddTag} color="primary">
                Add
            </Button>
            </div>


            {/* Display Tags */}
            <Tags tags={tags} />
            
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