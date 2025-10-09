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
        try
        {
            if (String.IsNullOrEmpty(noteDto.Title))
                throw new Exception("Title cannot be empty");

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

            return new ServiceResponse {Success = true, Message = "Note created"};
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ServiceResponse {Success = false, Message = ex.Message};
        }
    }
}