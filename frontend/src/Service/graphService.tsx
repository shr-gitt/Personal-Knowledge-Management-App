import apis from '../Config/api'
import { GraphResponse} from '../Dtos/Response';

export async function fetchUserGraph() {
    const username = localStorage.getItem('username')!;
    
    // Construct the URL with the query parameter 'username'
    const url = `${apis.graph.fetchUserGraph}?username=${encodeURIComponent(username)}`;

    const response = await fetch(url, {
        method: 'GET',  // Use GET for fetching data
        headers: {
            'Content-Type': 'application/json',  // Content type for the request
        },
    });

    // Log the response status and body for debugging
    console.log('Response Status:', response.status);
    //const textResponse = await response.text();  // Read as text first
    //console.log('Response Body:', textResponse);  // Log the raw response body

    if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const body: GraphResponse = await response.json();  // Parse the response body as JSON once
    console.log(body);

    if (!body.success) {
        console.log(body.message);
        throw new Error(body.message || "Graph fetch failed");
    }

    return body.data;  // Return the data from the response
}
