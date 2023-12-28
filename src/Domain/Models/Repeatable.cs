namespace Domain.Models;

public class Repeatable
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; }
    public RepeatableType Type { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public static Repeatable? Create(RepeatableType? type, DateTime? startedAt = null, DateTime? endedAt = null)
    {
        if (type is { } t)
        {
            return new Repeatable()
            {
                Type = t,
                StartedAt = startedAt ?? DateTime.UtcNow,
                EndedAt = endedAt
            };
        }

        return null;
    }
}