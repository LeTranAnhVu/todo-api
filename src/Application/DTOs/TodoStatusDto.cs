using Domain.Models;

namespace Application.DTOs;

public class TodoStatusDto
{
    public Guid Id { get; set; }
    public string TodoName { get; set; } = String.Empty;
    public Guid TodoId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime StartedAt { get; set; }
}