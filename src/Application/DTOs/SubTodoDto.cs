using Domain.Models;

namespace Application.DTOs;

public class SubTodoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid ParentId { get; set; }
    public RepeatableType RepeatableType { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}