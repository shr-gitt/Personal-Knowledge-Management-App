export interface SignUpRequest {
    Username: string;
    Name: string;
    Phone?: string; // optional
    Email: string;
    Password: string;
    ConfirmPassword: string;
    Image?: string; // optional
}

export interface Response {
    success: boolean,
    message: string,
    data: string;
}
