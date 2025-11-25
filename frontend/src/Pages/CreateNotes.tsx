import { useState } from "react"
import Button from "../Components/Button";
import { useNavigate } from "react-router-dom";
import './CreateNotes.css'

const CreateNotes = () =>{
    const [text, setText] = useState("");

    const title = text.split("\n")[0];
    const body = text.split("\n").slice(1).join("\n");

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
            <Button onClick={() => navigate("/")} color="primary">Save</Button>
        </div>
    </div>)
}

export default CreateNotes