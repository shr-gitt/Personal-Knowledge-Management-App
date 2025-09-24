using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public class SignUpRequest
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    [Phone]
    public required string Phone { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(25, MinimumLength = 7, ErrorMessage = "Password must be between 7 and 25 characters")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    public IFormFile? Image { get; set; }
}