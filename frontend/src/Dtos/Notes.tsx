export interface CreateNoteRequest {
    UserId : string;
    Title: string;
    Content: string;
    Tags?: string[];
}

export interface DeleteNoteRequest {
    id : string;
}