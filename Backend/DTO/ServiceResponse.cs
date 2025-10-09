namespace Backend.DTO;

public class ServiceResponse : ApiResponse<string>
{
    public bool RequiresTwoFactor { get; set; } = false;
}