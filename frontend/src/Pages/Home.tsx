import NotesCard from "../Components/NotesCard"
import "./Home.css"

const Home = () =>{
    return  <div className="pageContainer">
        <h1>Your Notes</h1>
        <input type="search" placeholder="Search" className="searchBar"/>

        <NotesCard notes={[
                {
                    id: 1,
                    title: "Project Ideas",
                    content: "Explore AI note-tagging, build dashboard layout.",
                    updatedAt: "2025-01-12 10:32"
                },
                {
                    id: 2,
                    title: "Shopping List",
                    content: "Milk, bread, eggs.",
                    updatedAt: "2025-01-14 08:10"
                }
            ]}/>
        </div>
}

export default Home