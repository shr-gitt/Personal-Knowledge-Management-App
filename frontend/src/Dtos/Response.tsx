export interface Response {
    success: boolean,
    message: string,
    data: string;
}

export interface GraphResponse {
    success: boolean,
    message: string,
    data: {nodes: [], links:[]};
}