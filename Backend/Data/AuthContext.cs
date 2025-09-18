using Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Data;

public class AuthContext
{
    private readonly IMongoDatabase _db;
    public IMongoCollection<ApplicationUser> Users;

    public AuthContext(IOptions<MongoDbSettings> settings, IMongoClient mongoClient)
    {
        _db = mongoClient.GetDatabase(settings.Value.DatabaseName);
        Users = _db.GetCollection<ApplicationUser>("PKM_User");
    }
}