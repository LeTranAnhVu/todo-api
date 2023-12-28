using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TodosController: ControllerBase
{
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly ITodoService _todoService;

    public TodosController(IUserContextAccessor userContextAccessor, ITodoService todoService)
    {
        _userContextAccessor = userContextAccessor;
        _todoService = todoService;
    }
    
    [HttpGet]
    public Task<List<TodoDto>> GetAll()
    {
        return _todoService.GetAllForOwnerAsync();
    }
    
    [HttpGet("{id}")]
    public Task<TodoDto> GetOne(Guid id)
    {
        return _todoService.GetOneForOwnerById(id);
    }
    
    [HttpPost]
    public Task<TodoDto> Create(CreateTodoDto dto)
    {
        return _todoService.CreateAsync(dto);
    }
    
    [HttpPut("{id}")]
    public Task<TodoDto> Update(Guid id, UpdateTodoDto dto)
    {
        return _todoService.UpdateFromOwnerAsync(id, dto);
    }
    
    
    [HttpDelete("{id}")]
    public Task Delete(Guid id)
    {
        return _todoService.DeleteFromOwnerAsync(id);
    }
}