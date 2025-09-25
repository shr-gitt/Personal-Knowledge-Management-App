using Backend.DTO;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authservice;
    public AuthController(AuthService authservice)
    {
        _authservice = authservice;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _authservice.GetAllUsers();
        return Ok(users);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _authservice.GetUserByUsername(username);
        return Ok(user);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>),200)]
    [ProducesResponseType(typeof(ApiResponse<string>),400)]
    public async Task<IActionResult> Register([FromForm] SignUpRequest model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Model is invalid",
                Data = default
            });

        var result = await _authservice.SignUpAccount(model);
        if (result.Success)
        {
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "SignUp Successful",
                Data = default
            });
        }
        
        return BadRequest(new ApiResponse<string>
        {
            Success = false,
            Message = "Sign-up failed. Please try again later.",
            Data = result.Data
        });
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> LogIn(SignInRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Model is invalid",
                Data = default
            });
        }

        var result = await _authservice.SignInAccount(model);
        
        if (result.RequiresTwoFactor)
        {
            
            return Ok(new ApiResponse<string>
            {
                Success = false,
                Message = "Two-factor authentication is required. Please verify your identity.",
                Data = null
            }); 
        }
        
        if (result.Success)
        {
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Sign In Successful",
                Data = default
            });
        }

        return Unauthorized(new ApiResponse<string>
        {
            Success = false,
            Message = "Sign In Failed",
            Data = default
        });
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> LogOut()
    {
        var result = await _authservice.SignOutAccount();
        
        if (result.Success){
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Sign Out Successful",
                Data = default
            });
        }

        return BadRequest(new ApiResponse<string>
        {
            Success = false,
            Message = "Sign Out Failed",
            Data = default
        });
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateProfile model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Model is invalid",
                Data = default
            });

        var result = await _authservice.UpdateAccount(model);
        if (result.Success)
        {
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Update User Successful",
                Data = default
            });
        }
        
        return BadRequest(new ApiResponse<string>
        {
            Success = false,
            Message = "User update failed. Please try again later.",
            Data = result.Data
        }); 
    }
}