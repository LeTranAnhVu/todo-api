using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

    public DbSet<Repeatable> Repeatables { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}