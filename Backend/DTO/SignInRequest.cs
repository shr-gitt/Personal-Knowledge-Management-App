using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public class SignInRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(25, MinimumLength = 7, ErrorMessage = "Password must be between 7 and 25 characters")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public bool RememberMe { get; set; } = true;
}