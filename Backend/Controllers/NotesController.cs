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
                Message = "Note created",
            });
        
        return BadRequest(new ApiResponse<string>{Success = false, Message = "Note could not be created"});
    }
}