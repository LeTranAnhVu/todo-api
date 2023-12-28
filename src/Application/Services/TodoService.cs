using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ITodoService
{
    public Task<List<TodoDto>> GetAllForOwnerAsync();
    public Task<TodoDto> CreateAsync(CreateTodoDto dto);
}

public class TodoService(IApplicationDbContext context, IUserContextAccessor userContext, IMapper mapper) : ITodoService
{
    public async Task<List<TodoDto>> GetAllForOwnerAsync()
    {
        Guid userId = userContext.Id;
        var todos = await context.Todos
            .Where(td => td.UserId == userId && td.ParentId == null)
            .Include(td => td.Repeatable)
            .Include(td => td.SubTodos)
            .ToListAsync();
        return mapper.Map<List<TodoDto>>(todos);
    }

    public async Task<TodoDto> CreateAsync(CreateTodoDto dto)
    {
        var subTodos = dto.SubTodos?.Select((sTodoDto) =>
        {
            Repeatable? sRepeatable = Repeatable.Create(sTodoDto.RepeatableType);
            return Todo.Create(userContext.Id, sTodoDto.Name, sRepeatable);
        }).ToList() ?? new List<Todo>();

        Repeatable? repeatable =  Repeatable.Create(dto.RepeatableType);
        var todo = Todo.Create(userContext.Id, dto.Name, repeatable);
        todo.SubTodos = subTodos;
        context.Todos.Add(todo);
        await context.SaveChangesAsync();

        return mapper.Map<TodoDto>(todo);
    }
}