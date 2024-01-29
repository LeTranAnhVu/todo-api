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
            return (false, "Occurred day is invalid for the once time todo");
        }

        if (occurredAt is not null)
        {
            if(StartedAt.ToUniversalTime().Date > occurredAt.Value.ToUniversalTime().Date)
            {
                return (false, "Occurred day should happen after start day"); 
            }

            if (EndedAt is not null && EndedAt.Value.ToUniversalTime().Date < occurredAt.Value.ToUniversalTime().Date)
            {
                return (false, "Occurred day should happen before end day"); 
            }
        }

        return (true, null);
    }
}