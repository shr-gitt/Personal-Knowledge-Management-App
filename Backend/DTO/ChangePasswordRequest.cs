using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public class ChangePasswordRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}