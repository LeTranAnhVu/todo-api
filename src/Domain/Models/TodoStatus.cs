namespace Domain.Models;

public class TodoStatus : IAuditable
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Complete()
    {
        CompletedAt = DateTime.UtcNow;
        IsCompleted = true;
    }
    
    public void Incomplete()
    {
        CompletedAt = null;
        IsCompleted = false;
    }
}