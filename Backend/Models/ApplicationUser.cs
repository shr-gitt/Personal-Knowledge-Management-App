using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;

namespace Backend.Models
{
    public class ApplicationUser : MongoIdentityUser<ObjectId>
    {
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}