namespace Application.Interfaces;

public interface IUserContextAccessor
{
    public Guid? Id { get; }
    public string? Oid { get; }
    public string? Email { get; }
}