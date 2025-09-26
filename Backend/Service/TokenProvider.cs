using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Backend.Service;

public class TokenProvider<ApplicationUser> : IUserTwoFactorTokenProvider<ApplicationUser> where ApplicationUser : class
{
    public async Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();
        
        var expirationTime = DateTime.UtcNow.AddMinutes(5);
        var claim = new Claim(purpose, $"{code}|{expirationTime:0}");
        
        var claims = await manager.GetClaimsAsync(user);
        var existingClaim = claims.FirstOrDefault(x => x.Type == purpose);
        if (existingClaim != null)
        {
            await manager.RemoveClaimAsync(user, existingClaim);
        }
     
        await manager.AddClaimAsync(user, claim);

        return code;
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var claims = await manager.GetClaimsAsync(user);
        var existingClaim = claims.FirstOrDefault(x => x.Type == purpose);
        if (existingClaim == null)
            return false;
        
        var parts = existingClaim.Value.Split('|');
        if (parts.Length != 2)
            return false;
        
        var storedCode = parts[0];
        if (storedCode != token)
            return false;
        
        return true;
    }

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        return Task.FromResult(true);
    }
}