namespace Domain.Models;

public class Repeatable
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; }
    public RepeatableType Type { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public static Repeatable Create(RepeatableType type, DateTime? startedAt = null, DateTime? endedAt = null)
    {
        return new Repeatable()
        {
            Type = type,
            StartedAt = startedAt ?? DateTime.UtcNow,
            EndedAt = endedAt
        };
    }

    public (bool IsValid, string? Reason) IsValidOccurredDate(DateTime? occurredAt)
    {
        if (occurredAt is null && Type != RepeatableType.Once)
        {
            return (false, "Occurred time is required");
        }
        
        if (occurredAt is not null 
            && Type == RepeatableType.Once 
            && StartedAt.ToUniversalTime().Date != occurredAt.Value.ToUniversalTime().Date)
        {
            return (false, "Occurred time is invalid for the once time todo");
        }

        return (true, null);
    }
}