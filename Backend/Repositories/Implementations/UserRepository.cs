using Backend.Models;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories.Implementations;

public class UserRepository:IUserRepository
{
    public async Task<ApplicationUser> GetAsync(string userId)
    {
        return null;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return null;
    }
    public async Task CreateAsync(ApplicationUser user){}
    public async Task UpdateAsync(ApplicationUser user){}
    public async Task DeleteAsync(ApplicationUser user){}
}