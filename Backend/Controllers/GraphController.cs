using Backend.DTO;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class GraphController : ControllerBase
{
    private readonly GraphService _graphService;

    public GraphController(GraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserGraph(string username)
    {
        var result = await _graphService.GetUserGraph(username);
        if (result.Success)
            return Ok(new ApiResponse<object>{Success = true, Message = "Fetch User Graph Success", Data = result.Data});
        
        return BadRequest(new ApiResponse<object>{Success = false, Message = result.Message});
    }
}   