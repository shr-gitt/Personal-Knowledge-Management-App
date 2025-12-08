import "./NotesCard.css"

interface Note {
    id: number;
    title: string;
    content: string;
    updatedAt: string;
}

interface Props {
    notes: Note[];
}

const NotesCard= ({notes}:Props)=>{
    return <div>
        {notes.map((note) =>(
            <div key={note.id} className="note-card">
                <div className="title-bar">
                    <h5 className="title">{note.title}</h5>
                    <span className="dates">{note.lastModified}</span>
                </div>
                <p className="information">{note.content}</p>
            </div>
        ))}
    </div>
}

export default NotesCard;