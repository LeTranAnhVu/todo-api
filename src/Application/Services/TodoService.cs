using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ITodoService
{
    public Task<List<TodoDto>> GetAllTodosAsync();
    public Task<TodoDto> CreateAsync(CreateTodoDto dto);
    public Task<SubTodoDto> CreateSubTodoAsync(CreateSubTodoDto dto);
    public Task<TodoDto> UpdateTodoAsync(Guid id, UpdateTodoDto dto);
    public Task<SubTodoDto> UpdateSubTodoAsync(Guid id, UpdateSubTodoDto dto);
    public Task DeleteSubTodoAsync(Guid id);
    public Task DeleteTodoAsync(Guid id);
    public Task<TodoDto> GetTodoById(Guid id);
}

public class TodoService(IApplicationDbContext context, IUserContextAccessor userContext, IMapper mapper) : ITodoService
{
    public async Task<List<TodoDto>> GetAllTodosAsync()
    {
        Guid userId = userContext.Id;
        var todos = await context.Todos
            .Where(td => td.UserId == userId && td.ParentId == null)
            .Include(td => td.Repeatable)
            .Include(td => td.SubTodos)
            .ThenInclude(subTodo => subTodo.Repeatable)
            .OrderByDescending(todo => todo.CreatedAt)
            .ToListAsync();
        return mapper.Map<List<TodoDto>>(todos);
    }

    public async Task<TodoDto> CreateAsync(CreateTodoDto dto)
    {
        // Validate the todo
        var isExisting = await context.Todos.AnyAsync(td => td.Name == dto.Name);
        if (isExisting)
        {
            throw new ApplicationValidationException("Name is used in another todo");
        }

        var parentRepeatableType = dto.RepeatableType ?? RepeatableType.Once;

        // Validate the sub todo
        var duplicateSubTodo = dto.SubTodos?.GroupBy(stodo => stodo.Name).Where(g => g.Count() > 1);
        if (duplicateSubTodo?.Any() == true)
        {
            throw new ApplicationValidationException("Duplicated sub todo");
        }

        var subTodos = dto.SubTodos?.Select((sTodoDto) =>
        {
            Repeatable? sRepeatable = Repeatable.Create(sTodoDto.RepeatableType ?? parentRepeatableType);
            return Todo.Create(userContext.Id, sTodoDto.Name, sRepeatable);
        }).ToList() ?? new List<Todo>();

        Repeatable? repeatable = Repeatable.Create(parentRepeatableType);
        var todo = Todo.Create(userContext.Id, dto.Name, repeatable);
        todo.SubTodos = subTodos;
        context.Todos.Add(todo);
        await context.SaveChangesAsync();

        return mapper.Map<TodoDto>(todo);
    }

    public async Task<SubTodoDto> CreateSubTodoAsync(CreateSubTodoDto dto)
    {
        var parent = await InternalGetOneById(dto.ParentId, userContext.Id);

        var isDuplicated = parent.SubTodos.Any(std => std.Name == dto.Name);
        if (isDuplicated)
        {
            throw new ApplicationValidationException("Duplicated sub todo");
        }

        Repeatable? sRepeatable = Repeatable.Create(dto.RepeatableType ?? RepeatableType.Once);
        var subTodo = Todo.Create(userContext.Id, dto.Name, sRepeatable, parent.Id);
        context.Todos.Add(subTodo);
        await context.SaveChangesAsync();
        return mapper.Map<SubTodoDto>(subTodo);
    }

    public async Task<TodoDto> UpdateTodoAsync(Guid id, UpdateTodoDto dto)
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
        var existingTodo = await context.Todos.FirstOrDefaultAsync(td =>
            td.Name == dto.Name
            && td.UserId == userContext.Id
            && td.ParentId == todo.ParentId);

        if (existingTodo is not null)
        {
            throw new ApplicationValidationException("The new name have been used by another todo.");
        }

        todo.Name = dto.Name;
        await context.SaveChangesAsync();
        return mapper.Map<TodoDto>(todo);
    }

    public async Task<SubTodoDto> UpdateSubTodoAsync(Guid id, UpdateSubTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ApplicationValidationException("Name is invalid");
        }

        var parent = await InternalGetOneById(dto.ParentId, userContext.Id);
        var isDuplicated = parent.SubTodos.Any(std => std.Name == dto.Name && std.Id != id);
        if (isDuplicated)
        {
            throw new ApplicationValidationException("Name is used in another sub todo");
        }

        var subTodo = parent.SubTodos.FirstOrDefault(std => std.Id == id);
        if (subTodo is null)
        {
            throw new EntityNotFoundException(nameof(subTodo));
        }

        subTodo.Name = dto.Name;

        await context.SaveChangesAsync();
        return mapper.Map<SubTodoDto>(subTodo);
    }

    public async Task DeleteSubTodoAsync(Guid id)
    {
        var subTodo = await InternalGetOneById(id, userContext.Id);
        context.Todos.Remove(subTodo);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTodoAsync(Guid id)
    {
        var todo = await InternalGetOneById(id, userContext.Id);
        context.Todos.RemoveRange(todo.SubTodos);
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();
    }

    public async Task<TodoDto> GetTodoById(Guid id)
    {
        var todo = await InternalGetOneById(id, userContext.Id);

        return mapper.Map<TodoDto>(todo);
    }

    private async Task<Todo> InternalGetOneById(Guid id, Guid? ownerId)
    {
        Todo? todo;
        if (ownerId is not null)
        {
            todo = await context.Todos
                .Include(td => td.Repeatable)
                .Include(td => td.SubTodos)
                .ThenInclude(std => std.Repeatable)
                .FirstOrDefaultAsync(td =>
                    td.Id == id
                    && td.UserId == ownerId);
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