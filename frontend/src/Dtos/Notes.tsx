export interface CreateNoteRequest {
    UserId : string;
    Title: string;
    Content: string;
    Tags: string[] || Null;
}