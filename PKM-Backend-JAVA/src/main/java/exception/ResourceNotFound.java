package exception;

public class ResourceNotFound extends RuntimeException{
    public ResourceNotFound(String resource){
        super(resource + " not found");
    }
}
