using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Backend.DTO.NotesDTO;

public class CreateNote
{
    public required string UserId { get; set; }
    
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters")]
    public required string Title { get; set; }
    
    [StringLength(5000, ErrorMessage = "Description must be less than 5000 characters")]
    public string? Content { get; set; }
    public List<string>? Tags { get; set; }
}