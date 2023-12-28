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
    
    [HttpPost]
    public Task<TodoDto> Create(CreateTodoDto dto)
    {
        return _todoService.CreateAsync(dto);
    }
}