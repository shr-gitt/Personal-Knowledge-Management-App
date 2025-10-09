using Backend.Data;
using Backend.DTO;
using Backend.DTO.NotesDTO;
using Backend.Models;
using MongoDB.Driver;

namespace Backend.Service;

public class NoteService
{
    private readonly IMongoCollection<Note> _notes;
    private readonly ILogger<NoteService> _logger;

    public NoteService(NoteContext noteContext,  ILogger<NoteService> logger)
    {
        _notes = noteContext.Notes;
        _logger = logger;
    }
    
    public async Task<ServiceResponse> CreateNote(CreateNote noteDto)
    {
        if (string.IsNullOrWhiteSpace(noteDto.Title))
        {
            _logger.LogError("Attempted to create a note without title");
            return new ServiceResponse { Success = false, Message = "Title cannot be empty." };
        }
        
        try
        {
            var note = new Note
            {
                UserId = noteDto.UserId,
                Title = noteDto.Title,
                Content = noteDto.Content,
                Tags = noteDto.Tags,
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };

            await _notes.InsertOneAsync(note);

            _logger.LogInformation("Note successfully created for user {userId}", note.UserId);
            return new ServiceResponse { Success = true, Message = "Note created" };
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB error while creating note for user {UserId}", noteDto.UserId);
            return new ServiceResponse
            {
                Success = false,
                Message = "Database error occurred while creating note."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating note for user {UserId}", noteDto.UserId);
            return new ServiceResponse { Success = false, Message = "An error occurred while creating note." };
        }
    }
}