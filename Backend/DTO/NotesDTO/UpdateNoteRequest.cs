using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.NotesDTO;

public class UpdateNoteRequest
{
    public required string id { get; set; }
    [Required(ErrorMessage = "UserId is required")]
    public required string UserId { get; set; }
    
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters")]
    public required string Title { get; set; }
    
    [StringLength(5000, ErrorMessage = "Description must be less than 5000 characters")]
    public string? Content { get; set; }
    public List<string>? Tags { get; set; }
}