using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ITodoStatusService
{
    public Task<TodoStatusDto> CreateTodoStatusAsync(UpsertTodoStatusDto dto);
    public Task<TodoStatusDto> UpdateTodoStatusAsync(Guid id, UpsertTodoStatusDto dto);
    public Task<List<TodoStatusDto>> GetTodoStatusesAsync(List<Guid>? todoIds);
}

public class TodoStatusService(
    IApplicationDbContext context,
    IUserContextAccessor userContext,
    IMapper mapper) : ITodoStatusService
{
    public async Task<TodoStatusDto> CreateTodoStatusAsync(UpsertTodoStatusDto dto)
    {
        // Check the existing todo
        var ownerId = userContext.Id;
        var todo = await context.Todos
            .Include(td => td.Repeatable)
            .FirstOrDefaultAsync(td =>
                td.Id == dto.TodoId
                && td.UserId == ownerId);
        
        if (todo is null)
        {
            throw new ApplicationValidationException("Invalid todo id");
        }
        
        // Can be null if repeatable type is once.
        (bool isValidOccuredAt, string errorMessage) = todo.Repeatable.IsValidOccurredDate(dto.OccuredAt);
        if(!isValidOccuredAt)
        {
            throw new ApplicationValidationException(errorMessage);
        }
        
        var status = TodoStatus.Create(todo.Id, dto.OccuredAt ?? DateTime.UtcNow);
        status.Complete(dto.IsCompleted);
        context.TodoStatuses.Add(status);
        await context.SaveChangesAsync();
        return mapper.Map<TodoStatusDto>(status);
    }

    public async Task<TodoStatusDto> UpdateTodoStatusAsync(Guid id, UpsertTodoStatusDto dto)
    {
        var status = context.TodoStatuses.FirstOrDefault(stt => stt.Id == id && stt.TodoId == dto.TodoId);
        if (status is null)
        {
            throw new EntityNotFoundException("Todo status");
        }
        
        status.Complete(dto.IsCompleted);
        await context.SaveChangesAsync();
        return mapper.Map<TodoStatusDto>(status);
    }

    public Task<List<TodoStatusDto>> GetTodoStatusesAsync(List<Guid>? todoIds)
    {
        var query = context.TodoStatuses.AsNoTracking().AsQueryable();
        if (todoIds?.Any() == true)
        {
            query = query.Where(stt => todoIds.Contains(stt.TodoId));
        }
        return mapper.ProjectTo<TodoStatusDto>(query)
            .ToListAsync();
    }
}