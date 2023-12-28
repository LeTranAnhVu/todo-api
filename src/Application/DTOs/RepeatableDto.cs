
using Domain.Models;

namespace Application.DTOs;

public class RepeatableDto
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public RepeatableType Type { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}