using Backend.DTO;
using Backend.DTO.NotesDTO;
using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotesController :  ControllerBase
{
    private readonly NoteService _noteservice;
    
    public NotesController(NoteService noteservice)
    {
        _noteservice = noteservice;
    }
/*
    [HttpGet]
    public async Task<IActionResult> GetAllNotes()
    {
        var result = await _noteservice.GetAllNotes();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes([FromQuery] int page)
    {
        var result = await _noteservice.GetNote();
        return Ok(result);
    }*/

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNote([FromBody] CreateNote model)
    {
        if(!ModelState.IsValid)
            return BadRequest(new ApiResponse<string>
            {
                Success = false, 
                Message = "Model is invalid", 
                Data = default
            });

        var result = await _noteservice.CreateNote(model);

        if (result.Success)
            return Ok(new ApiResponse<string>
            {
                Success = true, 
                Message = result.Message,
            });
        
        return BadRequest(new ApiResponse<string>{Success = false, Message = result.Message});
    }
    
    [HttpPost]
    [Authorize]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<string>),200)]
    [ProducesResponseType(typeof(ApiResponse<string>),400)]
    public async Task<IActionResult> DeleteNote(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<string> { Success = false, Message = "Model is invalid" });

        var result = await _noteservice.DeleteNote(id);

        if (result.Success)
            return Ok(new ApiResponse<string> { Success = true, Message = result.Message });

        return BadRequest(new ApiResponse<string> { Success = false, Message = result.Message });
    }
}