namespace Domain.Models;

public class Todo : IAuditable
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    /// <summary>
    /// Whether the todo is sub todo and has it own parent todo.
    /// </summary>
    public string? ParentId { get; set; }
    public Todo? Parent { get; set; }
    public ICollection<Todo> SubTodos { get; } = new List<Todo>();
    public required string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Repeatable? Repeatable { get; set; } 

    public User User { get; set; }
}