using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;

namespace Application.Services;

public interface ITodoStatusService
{
    public Task<TodoStatusDto> CreateTodoStatusAsync(UpsertTodoStatusDto dto);
    public Task<TodoStatusDto> UpdateTodoStatusAsync(Guid id, UpsertTodoStatusDto dto);
}

public class TodoStatusService(
    IApplicationDbContext context,
    IUserContextAccessor userContext,
    IMapper mapper,
    ITodoService todoService) : ITodoStatusService
{
    public async Task<TodoStatusDto> CreateTodoStatusAsync(UpsertTodoStatusDto dto)
    {
        // Check the existing todo
        var todo = await todoService.GetTodoById(dto.TodoId);
        var status = TodoStatus.Create(todo.Id);
        status.Complete(dto.IsCompleted);
        context.TodoStatuses.Add(status);
        await context.SaveChangesAsync();
        return mapper.Map<TodoStatusDto>(status);
    }

    public async Task<TodoStatusDto> UpdateTodoStatusAsync(Guid id, UpsertTodoStatusDto dto)
    {
        var todo = await todoService.GetTodoById(dto.TodoId);
        var status = context.TodoStatuses.FirstOrDefault(stt => stt.Id == id && todo.Id == dto.TodoId);
        if (status is null)
        {
            throw new EntityNotFoundException("Todo status");
        }
        
        status.Complete(dto.IsCompleted);
        await context.SaveChangesAsync();
        return mapper.Map<TodoStatusDto>(status);
    }
}