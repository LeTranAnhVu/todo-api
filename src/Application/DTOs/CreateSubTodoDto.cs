using Domain.Models;

namespace Application.DTOs;

public class CreateSubTodoDto
{
    public required Guid ParentId { get; set; }
    public required string Name { get; set; }
    public RepeatableType? RepeatableType { get; set; } 
}
