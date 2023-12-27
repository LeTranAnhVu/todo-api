namespace Domain.Models;

public class Repeatable
{
    public string Id { get; set; }
    public required string TodoId { get; set; }
    public Todo Todo { get; set; }
    public RepeatableType Type { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}