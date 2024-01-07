using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TodosController(ITodoService todoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TodoDto>>> GetAll()
    {
        return Ok(await todoService.GetAllTodosAsync());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoDto>> GetOne(Guid id)
    {
        return Ok(await todoService.GetTodoById(id));
    }
    
    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create(CreateTodoDto dto)
    {
        return Ok(await todoService.CreateAsync(dto));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TodoDto>> Update(Guid id, UpdateTodoDto dto)
    {
        return Ok(await todoService.UpdateTodoAsync(id, dto));
    }
    
    
    [HttpDelete("{id:guid}")]
    public Task Delete(Guid id)
    {
        return todoService.DeleteTodoAsync(id);
    }
}