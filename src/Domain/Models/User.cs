namespace Domain.Models;

public class User : IAuditable
{
    public required string Id { get; set; }
    public required string Oid { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Todo> Todos { get; } = new List<Todo>();
}