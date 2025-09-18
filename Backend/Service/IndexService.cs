using Backend.Models;
using MongoDB.Driver;

namespace Backend.Service;

public class IndexService
{
    private readonly IMongoCollection<ApplicationUser> _users;

    public IndexService(IMongoDatabase database)
    {
        _users = database.GetCollection<ApplicationUser>("PKM_User");
    }

    public void CreateIndexes()
    {
        // Get existing indexes
        var indexes = _users.Indexes.List().ToList();

        // Check if NormalizedUserName index exists
        bool usernameIndexExists = indexes.Any(i => i["name"] == "NormalizedUserName_1");

        if (usernameIndexExists)
        {
            throw new InvalidOperationException("Index 'NormalizedUserName_1' already exists.");
        }
        else{
            var usernameIndex = Builders<ApplicationUser>.IndexKeys.Ascending(u => u.NormalizedUserName);
            var indexOptions = new CreateIndexOptions { Unique = true }; 
            var usernameIndexModel = new CreateIndexModel<ApplicationUser>(usernameIndex, indexOptions);
            _users.Indexes.CreateOne(usernameIndexModel);
        }

        // Check if Email index exists
        bool emailIndexExists = indexes.Any(i => i["name"] == "Email_1");
        
        if (emailIndexExists)
        {
            throw new InvalidOperationException("Index 'Email_1' already exists.");
        }
        else{
            var emailIndex = Builders<ApplicationUser>.IndexKeys.Ascending(u => u.Email);
            var emailIndexModel = new CreateIndexModel<ApplicationUser>(emailIndex);
            _users.Indexes.CreateOne(emailIndexModel);
        }
    }
}