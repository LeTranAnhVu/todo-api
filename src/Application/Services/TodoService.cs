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
    public Task<TodoDto> UpdateTodoAsync(Guid id, UpdateTodoDto dto);
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

        var startDate = dto.StartDate;
        var endDate = dto.EndDate;

        if (endDate.HasValue && endDate < startDate)
        {
            throw new ApplicationValidationException("End date must be after the start date");
        }

        // Validate the sub todo
        var duplicateSubTodo = dto.SubTodos?.GroupBy(stodo => stodo.Name).Where(g => g.Count() > 1);
        if (duplicateSubTodo?.Any() == true)
        {
            throw new ApplicationValidationException("Duplicated sub todo");
        }

        // var subTodos = dto.SubTodos?.Select((sTodoDto) =>
        // {
        //     // valida the repeatable type
        //     if (sTodoDto.RepeatableType != RepeatableType.Once && sTodoDto.RepeatableType != parentRepeatableType)
        //     {
        //         throw new ApplicationValidationException(
        //             $"Cannot set '{sTodoDto.RepeatableType}''s  sub todo '{sTodoDto.Name}' when the todo '{dto.Name}' is '{dto.RepeatableType}' ");
        //     }
        //
        //     Repeatable sRepeatable = Repeatable.Create(
        //         sTodoDto.RepeatableType ?? parentRepeatableType,
        //         startDate,
        //         endDate);
        //
        //     return Todo.Create(userContext.Id, sTodoDto.Name, sRepeatable);
        // }).ToList() ?? new List<Todo>();

        Repeatable repeatable = Repeatable.Create(
            parentRepeatableType,
            startDate,
            endDate);

        var todo = Todo.Create(userContext.Id, dto.Name, repeatable);

        todo.SubTodos = dto.SubTodos?.Select(std => CreateSubTodo(todo, std)).ToList() ?? new List<Todo>();
        
        context.Todos.Add(todo);
        await context.SaveChangesAsync();

        return mapper.Map<TodoDto>(todo);
    }

    public async Task<TodoDto> UpdateTodoAsync(Guid id, UpdateTodoDto dto)
    {
        var todo = await InternalGetOneById(id, userContext.Id);

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ApplicationValidationException("Name is required.");
        }

        if (todo.ParentId is not null)
        {
            throw new ApplicationValidationException("Cannot update sub todo");
        }

        // Check duplicated
        var duplicatedTodo = await context.Todos.FirstOrDefaultAsync(td =>
            td.Name == dto.Name
            && td.Id != id
            && td.UserId == userContext.Id
            && td.ParentId == todo.ParentId);

        if (duplicatedTodo is not null)
        {
            throw new ApplicationValidationException("The new name have been used by another todo.");
        }

        // Validate the sub todos
        if (dto.SubTodos is not null)
        {
            // All sub todo should not have duplicated name
            var dupUpdatedSubTodos = dto.SubTodos.GroupBy(s => s.Name).Where(g => g.Count() > 1);
            if (dupUpdatedSubTodos.Any())
            {
                throw new ApplicationValidationException("Duplicated sub todos input");
            }

            // Find the deleted sub todos
            var notFoundSubs = todo.SubTodos.Where(std => dto.SubTodos.All(dstd => dstd.Id != std.Id));
            context.Todos.RemoveRange(notFoundSubs);

            // Find the updated sub todos
            foreach (var subTodo in todo.SubTodos)
            {
                var updatedSubTodoDto = dto.SubTodos.FirstOrDefault(dstd => dstd.Id == subTodo.Id
                                                                            && dstd.Name != subTodo.Name);
                if (updatedSubTodoDto is { })
                {
                    subTodo.Name = updatedSubTodoDto.Name;
                }
            }

            // Find the new sub todos
            var newSubTodos = dto.SubTodos
                .Where(stdDto => todo.SubTodos.All(std => std.Id != stdDto.Id))
                .Select(stdDto => CreateSubTodo(todo, stdDto));
            context.Todos.AddRange(newSubTodos);
        }

        if (todo.Name != dto.Name)
        {
            todo.Name = dto.Name;
        }

        await context.SaveChangesAsync();
        return mapper.Map<TodoDto>(todo);
    }

    private Todo CreateSubTodo(Todo parent, UpsertNestedSubTodoDto dto)
    {
        var repeatableType = dto.RepeatableType ?? RepeatableType.Once;

        if (repeatableType != RepeatableType.Once && repeatableType != parent.Repeatable.Type)
        {
            throw new ApplicationValidationException("Invalid repeatable type");
        }

        if (dto.EndDate.HasValue 
            && !dto.StartDate.HasValue)
        {
            throw new ApplicationValidationException("StartDate required");
        }

        if (dto.EndDate.HasValue 
            && dto.StartDate.HasValue 
            && dto.StartDate.Value > dto.EndDate.Value)
        {
            throw new ApplicationValidationException("StartDate cannot be later than the EndDate");
        }

        var startDate = dto.StartDate ?? parent.Repeatable.StartDate;
        var endDate = dto.EndDate ?? parent.Repeatable.EndDate;

        if (startDate < parent.Repeatable.StartDate ||
            (parent.Repeatable.EndDate.HasValue 
             && startDate > parent.Repeatable.EndDate))
        {
            throw new ApplicationValidationException("StartDate of sub todo should be in range of its todo");
        }

        if (endDate < parent.Repeatable.StartDate ||
            (parent.Repeatable.EndDate.HasValue 
             && endDate > parent.Repeatable.EndDate))
        {
            throw new ApplicationValidationException("EndDate of sub todo should be in range of its todo");
        }

        Repeatable sRepeatable = Repeatable.Create(repeatableType, startDate, endDate);
        return Todo.Create(userContext.Id, dto.Name, sRepeatable, parent.Id);
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