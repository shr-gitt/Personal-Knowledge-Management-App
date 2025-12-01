import apis from '../Config/api';
import { SignUpRequest, Response } from '../Dtos/Auth';

export async function SignUp(data: FormData): Promise<Response> {
    console.log("FormData contents:");
    for (const [key, value] of data.entries()) {
        console.log(key, value);
    }
    const response = await fetch(apis.auth.register, {
        method: "POST",
        body: data,
    });

    console.log(response);

    // Parse the response body as JSON
    const body: Response = await response.json();

    console.log(body);
    console.error(body);

    // Now check the backend's success flag
    
    const Data = body.Data ?? null;

    if (!body.success) {
        console.log(body.message);
        throw new Error(body.errors
                ? Object.values(body.errors).flat().join("\n")
                : body.message || "Registration failed");
    }

    // Return shape expected by frontend
    return body;

}