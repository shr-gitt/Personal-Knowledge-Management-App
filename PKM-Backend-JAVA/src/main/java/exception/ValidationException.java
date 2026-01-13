package exception;

public class Validation extends RuntimeException{
    public Validation(String message){
        super(message);
    }
}
