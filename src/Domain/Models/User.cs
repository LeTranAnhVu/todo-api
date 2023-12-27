namespace Domain.Models;

public class User : IAuditable
{
    public Guid Id { get; set; }
    public required string Oid { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Todo> Todos { get; } = new List<Todo>();

    public static User Create(string? oid, string? email)
    {
        if (string.IsNullOrEmpty(oid))
        {
            throw new ArgumentNullException(nameof(oid));
        }
        
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email));
        }
        
        return new User()
        {
            Oid = oid,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };
    }
}