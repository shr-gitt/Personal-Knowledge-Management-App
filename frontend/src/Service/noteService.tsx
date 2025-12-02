import { CreateNoteRequest, Response } from "../Dtos/Notes";
import apis from '../Config/api';

export async function FetchAllNotes(){
    const response = await fetch(apis.notes.getAll)
    const body: Response = await response.json();

    console.log(body);

    if(body.success){
        console.log(`fetch all posts success:`,body.data);
        return body.data;
    }else{
        throw new Error(body.errors
            ? Object.values(body.errors).flat().join("\n")
            : body.message || "Fetch all Notes failed");
    }

    return body.data;
}

export async function CreateNote(data: CreateNoteRequest): Promise<string>{
    console.log(`Create Note data is:`,data);

    const response = await fetch(apis.notes.create, {
        method:"POST",
        body: JSON.stringify(data),
        headers: {"Content-Type":"application/json"}
    })

    console.log(response);

    const body: Response = await response.json();

    console.log(body);

    if(!body.success) {
        console.log(body.message);
        throw new Error(body.errors
            ? Object.values(body.errors).flat().join("\n")
            : body.message || "Create Note failed");
    }

    return body.data;
}