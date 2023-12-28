using System.Linq.Expressions;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ITodoService
{
    public Task<List<TodoDto>> GetAllForOwnerAsync();
    public Task<TodoDto> CreateAsync(CreateTodoDto dto);
    public Task<TodoDto> UpdateFromOwnerAsync(Guid id, UpdateTodoDto dto);
    public Task DeleteFromOwnerAsync(Guid id);
    public Task<TodoDto> GetOneForOwnerById(Guid id);
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

        Repeatable? repeatable = Repeatable.Create(dto.RepeatableType);
        var todo = Todo.Create(userContext.Id, dto.Name, repeatable);
        todo.SubTodos = subTodos;
        context.Todos.Add(todo);
        await context.SaveChangesAsync();

        return mapper.Map<TodoDto>(todo);
    }

    public async Task<TodoDto> UpdateFromOwnerAsync(Guid id, UpdateTodoDto dto)
    {
        var todo = await InternalGetOneById(id, userContext.Id);

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ApplicationValidationException("Name is required.");
        }

        if (todo.Name == dto.Name)
        {
            return mapper.Map<TodoDto>(todo); 
        }
        
        // Check duplicated
        var existingTodo = await context.Todos.FirstOrDefaultAsync(td => td.Name == dto.Name && td.UserId == userContext.Id && td.ParentId == todo.ParentId);
        if (existingTodo is not null)
        {
            throw new ApplicationValidationException("The new name have been used by another todo.");
        }
        
        todo.Name = dto.Name;
        await context.SaveChangesAsync();
        return mapper.Map<TodoDto>(todo);
    }

    public async Task DeleteFromOwnerAsync(Guid id)
    {
        var todo = await InternalGetOneById(id, userContext.Id);
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();
    }

    public async Task<TodoDto> GetOneForOwnerById(Guid id)
    {
        var todo = await InternalGetOneById(id, userContext.Id);

        return mapper.Map<TodoDto>(todo);
    }

    private async Task<Todo> InternalGetOneById(Guid id, Guid? ownerId)
    {
        Todo? todo;
        if (ownerId is not null)
        {
            todo = await context.Todos.FirstOrDefaultAsync(td => td.Id == id && td.UserId == ownerId);
        }
        else
        {
            todo = await context.Todos.FirstOrDefaultAsync(td => td.Id == id);
        }

        if (todo is null)
        {
            throw new EntityNotFoundException(nameof(Todo));
        }

        return todo;
    }
}