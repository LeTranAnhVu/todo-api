using Domain.Models;

namespace Application.DTOs;

public class TodoDto
{
    public required string Name { get; set; }
    public ICollection<TodoDto>? SubTodos { get; set; }
    public RepeatableType? RepeatableType { get; set; }
    public DateTime? RepeatableStartedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}