using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Backend.Models;

public class Note
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }
    public required string UserId { get; set; }
    
    public required string Title { get; set; }
    public string? Content { get; set; }
    public List<string>? Tags { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsArchived { get; set; }
}