import LongMenu from "./Menu";
import "./NotesCard.css";

interface Note {
  id: number;
  title: string;
  content: string;
  lastModified: string;
}

interface Props {
  notes: Note[];
}

const NotesCard = ({ notes }: Props) => {
  return (
    <div>
      {notes.map((note) => {
        // Parse the ISO date string into a Date object
        const dateObj = new Date(note.lastModified);

        // Format the date and time separately
        const date = dateObj.toLocaleDateString(); // e.g., "12/2/2025"
        const time = dateObj.toLocaleTimeString([], {
          hour: "2-digit",
          minute: "2-digit",
        }); // e.g., "06:39 AM"
        return (
          <div key={note.id} className="note-card">
            <div className="title-bar">
              <h5 className="title">{note.title}</h5>
              <div className="group">
              <span className="date-time">
                <div>{date}</div> 
                <div>{time}</div> {" "}
              </span>
              <LongMenu noteId={note.id.toString()} />
              </div>
            </div>
            <p className="information">{note.content}</p>
          </div>
        );
      })}
    </div>
  );
};

export default NotesCard;
