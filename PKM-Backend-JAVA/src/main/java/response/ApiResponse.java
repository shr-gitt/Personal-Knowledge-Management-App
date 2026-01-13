package response;

public class ApiResponse<T> {
    private boolean success;
    private String message;
    private T data;
    private ApiResponse(boolean success, String message, T data) {
        this.success = success;
        this.message = message;
        this.data = data;
    }

    public ApiResponse() {
    }

    public static <T> ApiResponse<T> success(String message,T data){
        return new ApiResponse<>(true, message, data);
    }

    public static <T> ApiResponse<T> failure(String message){
        return new ApiResponse<>(false, message, null);
    }
}
