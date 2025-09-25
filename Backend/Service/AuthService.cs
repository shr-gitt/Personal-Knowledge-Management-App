using Backend.DTO;
using Backend.Models;
using Backend.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Backend.Service;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AuthService> _logger;
    private readonly UserRepository _userRepository;
    private readonly UploadImageService _uploadImageService;

    public AuthService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender, 
        ILogger<AuthService> logger,
        UserRepository userRepository,
        UploadImageService uploadImageService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _userRepository = userRepository;
        _uploadImageService = uploadImageService;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        => await _userRepository.GetAllAsync();
    
    public async Task<ApplicationUser> GetUserByUsername(string username)
        => await _userManager.FindByNameAsync(username);
    
    public async Task<AuthResponse> SignUpAccount(SignUpRequest model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Email is already in use."
            };
        }
        
        string imagePath = "";
        if (model.Image != null)
        {
            try
            {
                imagePath = await _uploadImageService.UploadImage(model.Image);
                _logger.LogInformation($"Upload image called {imagePath}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to upload image: {ex.Message}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Error uploading image. Please try again later."
                };
            }            
        }
        
        var user = new ApplicationUser
        {
            UserName = model.Username,
            Name = model.Name,
            PhoneNumber = model.Phone,
            Email = model.Email,
            Image = imagePath
        };
        
        try
        {
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                return new AuthResponse
                {
                    Success = true,
                    Message = "Sign-up successful."
                };
            }

            // If user creation fails, return the errors
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse
            {
                Success = false,
                Message = "Sign-up failed.",
                Data = errors
            };        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign-up");
            return new AuthResponse { Success = false, Message = $"Sign-up failed: {ex.Message}" };
        }
    }

    public async Task<AuthResponse> SignInAccount(SignInRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            _logger.LogWarning("User not found");
            return new AuthResponse { Success = false, Message = "Invalid email or password." };
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            _logger.LogInformation("User signed in successfully.");
            return new AuthResponse { Success = true, Message = "Sign-in successful." };
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return new AuthResponse { Success = false, Message = "Account is locked. Please try again later." };
        }

        if (result.IsNotAllowed)
        {
            _logger.LogWarning("User not allowed to sign in.");
            return new AuthResponse { Success = false, Message = "Sign-in not allowed. Please confirm your email or contact support." };
        }

        if (result.RequiresTwoFactor)
        {
            _logger.LogInformation("User requires two-factor authentication.");
            return new AuthResponse { Success = false, Message = "Two-factor authentication is required.", RequiresTwoFactor = true};
        }

        _logger.LogWarning("Invalid login attempt.");
        return new AuthResponse { Success = false, Message = "Invalid email or password." };
    }

    public async Task<AuthResponse> SignOutAccount()
    {
        try
        {
            await _signInManager.SignOutAsync();

            // Optionally clear other schemes
            //await _httpContextAccessor.HttpContext!.SignOutAsync(IdentityConstants.ExternalScheme);
            //await _httpContextAccessor.HttpContext!.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

            _logger.LogInformation("User signed out successfully.");
            return new AuthResponse { Success = true, Message = "Sign Out successful." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign out.");
            return new AuthResponse { Success = false, Message = "Error occurred during sign out." };
        }
    }
}