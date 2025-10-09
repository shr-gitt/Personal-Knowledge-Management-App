using Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Data;

public class NoteContext
{
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<Note> _notes;

    public NoteContext(IOptions<MongoDbSettings> options, IMongoClient mongoClient)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _db = database;
        _notes = database.GetCollection<Note>("Notes");
    }
    
    public IMongoCollection<Note> Notes 
        => _notes;
}