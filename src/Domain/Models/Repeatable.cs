namespace Domain.Models;

public class Repeatable
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; }
    public RepeatableType Type { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public static Repeatable Create(RepeatableType type, DateOnly startDate, DateOnly? endDate = null)
    {
        return new Repeatable()
        {
            Type = type,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public (bool IsValid, string? Reason) IsValidOccurredDate(DateOnly? occurDate)
    {
        if (occurDate is null && Type != RepeatableType.Once)
        {
            return (false, "Occurred time is required");
        }
        
        if (occurDate is not null 
            && Type == RepeatableType.Once 
            && StartDate != occurDate.Value)
        {
            return (false, "Occurred day is invalid for the once time todo");
        }

        if (occurDate is not null)
        {
            if(StartDate > occurDate.Value)
            {
                return (false, "Occurred day should happen after start day"); 
            }

            if (EndDate is not null && EndDate.Value < occurDate.Value)
            {
                return (false, "Occurred day should happen before end day"); 
            }
        }

        return (true, null);
    }
}