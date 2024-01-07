namespace Domain.Models;

public class Todo : IAuditable
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    /// <summary>
    /// Whether the todo is sub todo and has it own parent todo.
    /// </summary>
    public Guid? ParentId { get; set; }
    public Todo? Parent { get; set; }
    public ICollection<Todo> SubTodos { get; set; } = new List<Todo>();
    public required Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Repeatable? Repeatable { get; set; } 

    public User User { get; set; }

    public static Todo Create(Guid userId, string name, Repeatable? repeatable = null, Guid? parentId = null)
    {
        return new Todo()
        {
            UserId = userId,
            Name = name,
            Repeatable = repeatable,
            ParentId = parentId
        };
    }
}