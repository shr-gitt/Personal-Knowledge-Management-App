export interface SignUpRequest {
    Username: string;
    Name: string;
    Phone?: string; // optional
    Email: string;
    Password: string;
    ConfirmPassword: string;
    Image?: File| string; // optional
}

export interface SignInRequest {
    Email: string;
    Password: string;
    RememberMe: boolean;
}

export interface Response {
    Success: boolean,
    Message: string,
    Data: string;
}
