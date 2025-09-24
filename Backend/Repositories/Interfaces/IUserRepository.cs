using Backend.Models;

namespace Backend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser> GetAsync(string username);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task CreateAsync(ApplicationUser user);
    Task UpdateAsync(ApplicationUser user);
    Task DeleteAsync(ApplicationUser user);
}