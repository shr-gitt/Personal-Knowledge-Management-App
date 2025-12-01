import apis from '../Config/api';
import { SignUpRequest, Response } from '../Dtos/Auth';

export async function SignUp(data: SignUpRequest): Promise<Response> {
    const response = await fetch(apis.auth.register, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    // Parse the response body as JSON
    const body: Response = await response.json();

    // Now check the backend's success flag
    if (!body.success) {
        throw new Error(body.message);
    }

    return body; // entire ApiResponse object
}