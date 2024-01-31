namespace Domain.Models;

public class TodoStatus : IAuditable
{
    public Guid Id { get; set; }
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateOnly OccurDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Complete(bool isCompleted)
    {
        CompletedAt = isCompleted ? DateTime.UtcNow : null;
        IsCompleted = isCompleted;
    }

    public static TodoStatus Create(Guid todoId, DateOnly occurDate)
    {
        return new TodoStatus()
        {
            TodoId = todoId,
            OccurDate = occurDate
        };
    }
}