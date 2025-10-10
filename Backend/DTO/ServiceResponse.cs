namespace Backend.DTO;

public class ServiceResponse<T> : ApiResponse<T>
{
    public bool RequiresTwoFactor { get; set; } = false;
}