using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public class UpdateProfile
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    [Phone]
    public required string Phone { get; set; }
    
    public IFormFile? Image { get; set; }
}