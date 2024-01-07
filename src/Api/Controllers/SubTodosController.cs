using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SubTodosController(ITodoService todoService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SubTodoDto>> CreateSubTodo(CreateSubTodoDto dto)
    {
        return Ok(await todoService.CreateSubTodoAsync(dto));
    }
        
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SubTodoDto>> UpdateSubTodo(Guid id, UpdateSubTodoDto dto)
    {
        return Ok(await todoService.UpdateSubTodoAsync(id, dto));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> UpdateSubTodo(Guid id)
    {
        await todoService.DeleteSubTodoAsync(id);
        return NoContent();
    }
}