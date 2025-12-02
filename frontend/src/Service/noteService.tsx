import { CreateNoteRequest, Response } from "../Dtos/Notes";
import apis from '../Config/api';

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