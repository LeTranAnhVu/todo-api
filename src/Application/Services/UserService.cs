using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IUserService
{
    public Task<User?> FindByOidAsync(string oid);
    public Task<User> CreateUserAsync(User user);
}

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<User?> FindByOidAsync(string oid) =>
        _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Oid == oid);

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}