using Domain.Models;

namespace Application.DTOs;

public class UpdateSubTodoDto
{
    public required Guid ParentId { get; set; }
    public required string Name { get; set; }
}
