using Domain.Models;

namespace Application.DTOs;

public class CreateTodoDto
{
    public required string Name { get; set; }
    public ICollection<CreateTodoDto>? SubTodos { get; set; }
    public RepeatableType? RepeatableType { get; set; } 
}