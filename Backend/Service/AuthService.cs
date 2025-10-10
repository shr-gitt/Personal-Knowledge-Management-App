using Backend.DTO;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Backend.Service;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AuthService> _logger;
    private readonly UploadImageService _uploadImageService;
    private readonly CustomUserManager _customUserManager;
    private readonly TokenProvider<ApplicationUser> _tokenProvider;

    public AuthService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender, 
        ILogger<AuthService> logger,
        UploadImageService uploadImageService,
        CustomUserManager customUserManager,
        TokenProvider<ApplicationUser> tokenProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _uploadImageService = uploadImageService;
        _customUserManager = customUserManager;
        _tokenProvider = tokenProvider;
    }

    public Task<IEnumerable<ApplicationUser>> GetAllUsers()
        => null;
        //=> await _userManager.GetUserAsync();
    
    public async Task<ApplicationUser> GetUserByUsername(string username)
        => await _userManager.FindByNameAsync(username);
    
    public async Task<ServiceResponse<ApplicationUser>> SignUpAccount(SignUpRequest model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return new ServiceResponse<ApplicationUser>
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
                return new ServiceResponse<ApplicationUser>
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
                return new ServiceResponse<ApplicationUser>
                {
                    Success = true,
                    Message = "Sign-up successful.",
                    Data = user
                };
            }

            // If user creation fails, return the errors
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ServiceResponse<ApplicationUser>
            {
                Success = false,
                Message = "Sign-up failed.",
                //Data = errors
            };        
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign-up");
            return new ServiceResponse<ApplicationUser> { Success = false, Message = $"Sign-up failed: {ex.Message}" };
        }
    }

    public async Task<ServiceResponse<ApplicationUser>> SignInAccount(SignInRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            _logger.LogWarning("User not found");
            return new ServiceResponse<ApplicationUser> { Success = false, Message = "Invalid email or password." };
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        
        if (result.RequiresTwoFactor)
        {
            _logger.LogInformation("User requires two-factor authentication.");

            var code = await _customUserManager.GenerateTwoFactorTokenAsync(user, "CustomTokenProvider");

            try
            {
                await _emailSender.SendEmailAsync(
                    user.Name,
                    user.Email!, 
                    "Login Attempt",
                    $"Your code for logging in is:<br><strong>{code}</strong><br>" +
                    $"This code is valid for 5 minutes from now i.e. upto <br><strong>{DateTime.Now}</strong></br> UTC. Copy this code into the app to login.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send two factor login email to {Email}", user.Email);
                return new ServiceResponse<ApplicationUser> { Success = false, Message = "Failed to send email for Two-factor authentication. Please try again later." };
            }

            return new ServiceResponse<ApplicationUser> { Success = false, Message = "Two-factor Authentication required." };
        }
        
        if (result.Succeeded)
        {
            _logger.LogInformation("User signed in successfully.");
            return new ServiceResponse<ApplicationUser> { Success = true, Message = "Sign-in successful.", Data = user };
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return new ServiceResponse<ApplicationUser> { Success = false, Message = "Account is locked. Please try again later." };
        }

        if (result.IsNotAllowed)
        {
            _logger.LogWarning("User not allowed to sign in.");
            return new ServiceResponse<ApplicationUser> { Success = false, Message = "Sign-in not allowed. Please confirm your email or contact support." };
        }

        _logger.LogWarning("Invalid login attempt.");
        return new ServiceResponse<ApplicationUser> { Success = false, Message = "Invalid email or password." };
    }

    public async Task<ServiceResponse<string>> SignOutAccount()
    {
        try
        {
            await _signInManager.SignOutAsync();

            // Optionally clear other schemes
            //await _httpContextAccessor.HttpContext!.SignOutAsync(IdentityConstants.ExternalScheme);
            //await _httpContextAccessor.HttpContext!.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

            _logger.LogInformation("User signed out successfully.");
            return new ServiceResponse<string> { Success = true, Message = "Sign Out successful." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign out.");
            return new ServiceResponse<string> { Success = false, Message = "Error occurred during sign out." };
        }
    }

    public async Task<ServiceResponse<ApplicationUser>> UpdateAccount(UpdateProfile model)
    {
        //var existingUser = await _userManager.FindByNameAsync(model.Username);
        
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
                return new ServiceResponse<ApplicationUser>
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
            Image = imagePath
        };
        
        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User updated.");
                return new ServiceResponse<ApplicationUser>
                {
                    Success = true,
                    Message = "User update successful.",
                    Data = user
                };
            }

            // If user creation fails, return the errors
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ServiceResponse<ApplicationUser>
            {
                Success = false,
                Message = "User update failed.",
                //Data = errors
            };        
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user update.");
            return new ServiceResponse<ApplicationUser> { Success = false, Message = $"User update failed: {ex.Message}" };
        }
    }

    public async Task<ServiceResponse<string>> DeleteAccount(SignInRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("User not found.");
                return new ServiceResponse<string> { Success = false, Message = "User not found." };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
         
            if (result.Succeeded)
            {
                _logger.LogInformation("User deleted successfully.");
                return new ServiceResponse<string> { Success = true, Message = "Account deletion successful." };
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return new ServiceResponse<string> { Success = false, Message = "Account is locked. Please try again later." };
            }

            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User not allowed to sign in.");
                return new ServiceResponse<string> { Success = false, Message = "Sign-in not allowed. Please confirm your email or contact support." };
            }

            _logger.LogWarning("Invalid login attempt.");
            return new ServiceResponse<string> { Success = false, Message = "Invalid email or password." };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot delete account");
            return new ServiceResponse<string> {Success = false, Message = $"User account deletion failed: {e.Message}"};
        }
    }

    public async Task<ServiceResponse<string>> ForgotPassword(ForgotPasswordRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new ServiceResponse<string> { Success = false, Message = "User not found." };

            _logger.LogInformation("Forgot password called.");

            var code = await _customUserManager.GeneratePasswordResetTokenAsync(user);

            try
            {
                await _emailSender.SendEmailAsync(
                    user.Name,
                    user.Email!,
                    "Reset Password",
                    $"Your code for resetting password is:<br><strong>{code}</strong><br>" +
                    $"This code is valid for 5 minutes from now i.e. upto <br><strong>{DateTime.Now}</strong></br> UTC. Copy this code into the app to login.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reset password email to {Email}", user.Email);
                return new ServiceResponse<string>
                    { Success = false, Message = "Failed to send email for reset password. Please try again later." };
            }

            return new ServiceResponse<string> { Success = true, Message = "Reset password success." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot forgot password");
            return new ServiceResponse<string> { Success = false, Message = $"Forgot password failed: {ex.Message}" };
        }
    }

    public async Task<ServiceResponse<string>> ResetPassword(ResetPasswordRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ServiceResponse<string> { Success = false, Message = "User not found." };
            
            var result = await _userManager.ResetPasswordAsync(user, model.ResetCode, model.NewPassword);
            if (result.Succeeded)
                return new ServiceResponse<string> { Success = true, Message = "Reset Password success." };
            
            return new ServiceResponse<string> { Success = false, Message = "Reset password failed." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Cannot reset password");
            return new ServiceResponse<string> { Success = false, Message = $"Reset password failed: {ex.Message}" };
        }
    }

    public async Task<ServiceResponse<string>> ChangePassword(ChangePasswordRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "User not found.",
                };

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Password Changed successfully."
                };

            return new ServiceResponse<string>
            {
                Success = false,
                Message = "Password Changed failed."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error changing password");
            return new ServiceResponse<string> { Success = false, Message = $"Error changing password: {ex.Message}" };
        }
    }
}