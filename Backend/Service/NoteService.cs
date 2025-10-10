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

    public async Task<ServiceResponse<List<Note>>> GetAllNotes()
    {
        try
        {
            var notes = await _notes.Find(_ => true).ToListAsync();

            _logger.LogInformation("Notes found");
            return new ServiceResponse<List<Note>> { Success = true, Message = "Notes found", Data = notes };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new ServiceResponse<List<Note>> { Success = false, Message = ex.Message };
        }
    }

    public async Task<ServiceResponse<Note>> GetNote(string id)
    {
        try
        {
            var note = await _notes.Find(n => n.Id == id).FirstOrDefaultAsync();
            
            _logger.LogInformation("Note found");
            return new ServiceResponse<Note> {Success = true, Message = "Note found", Data = note};
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new ServiceResponse<Note> { Success = false, Message = ex.Message };
        }
    }

    public async Task<ServiceResponse<Note>> CreateNote(CreateNoteRequest noteDto)
    {
        if (string.IsNullOrWhiteSpace(noteDto.Title))
        {
            _logger.LogError("Attempted to create a note without title");
            return new ServiceResponse<Note> { Success = false, Message = "Title cannot be empty.", Data = default};
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
            return new ServiceResponse<Note> { Success = true, Message = "Note created", Data = note };
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB error while creating note for user {UserId}", noteDto.UserId);
            return new ServiceResponse<Note>
            {
                Success = false,
                Message = "Database error occurred while creating note."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating note for user {UserId}", noteDto.UserId);
            return new ServiceResponse<Note> { Success = false, Message = "An error occurred while creating note." };
        }
    }

    public async Task<ServiceResponse<Note>> DeleteNote(string id)
    {
        if (String.IsNullOrEmpty(id))
            return new ServiceResponse<Note> { Success = false, Message = "Note Id is null" };

        try
        {
            var deletedNote = await _notes.FindOneAndDeleteAsync(note => note.Id == id);

            if (deletedNote == null)
            {
                _logger.LogError("Note with id {id} not found for deletion", id);
                return new ServiceResponse<Note> { Success = false, Message = "Note not found." };
            }

            _logger.LogInformation("Deleted note titled {Title}", deletedNote.Title);
            return new ServiceResponse<Note> { Success = true, Message = "Note deleted.", Data = deletedNote };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Note deletion failed");
            return new ServiceResponse<Note> { Success = false, Message = "Note deletion failed." };
        }
    }

    public async Task<ServiceResponse<Note>> UpdateNote(UpdateNoteRequest noteDto)
    {
        if (string.IsNullOrEmpty(noteDto.id))
            return new ServiceResponse<Note> { Success = false, Message = "Cannot find note" };

        try
        {
            var update = Builders<Note>.Update
                .Set(n => n.Title, noteDto.Title)
                .Set(n => n.Content, noteDto.Content)
                .Set(n => n.Tags, noteDto.Tags)
                .Set(n => n.LastModified, DateTime.UtcNow);

            var result = await _notes.FindOneAndUpdateAsync(
                n => n.Id == noteDto.id && n.UserId == noteDto.UserId,
                update);

            if (result == null)
                return new ServiceResponse<Note> { Success = false, Message = "Note not found or not updated." };
            
            _logger.LogInformation("Note found for user {userId} and updated", noteDto.UserId);
            return new ServiceResponse<Note> { Success = true, Message = "Note updated", Data = result };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Note update failed");
            return new ServiceResponse<Note> { Success = false, Message = "Note update failed." };
        }
    }
}