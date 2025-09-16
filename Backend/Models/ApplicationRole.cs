using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;

namespace Backend.Models
{
    public class ApplicationRole : MongoIdentityRole<ObjectId>
    {
        public const string Administrator = "Administrator";
        public const string User = "User";
    }
}