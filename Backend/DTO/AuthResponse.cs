namespace Backend.DTO;

public class AuthResponse : ApiResponse<string>
{
    public bool RequiresTwoFactor { get; set; } = false;
}