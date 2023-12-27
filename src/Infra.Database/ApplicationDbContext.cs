using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Database;

public class ApplicationDbContext: DbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<Repeatable> Repeatables { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Since EF Core automatically pluralize all the name of table as default e.g user to users when it compose the SQL scripts
        // This setting would help to prevent it, so that it keep the table has it was.
        configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(user => user.Todos)
            .WithOne(todo=> todo.User)
            .HasForeignKey(todo => todo.UserId)
            .IsRequired();

        modelBuilder.Entity<Todo>()
            .HasMany(todo => todo.SubTodos)
            .WithOne(todo => todo.Parent)
            .HasForeignKey(todo => todo.ParentId)
            .IsRequired(false);

        modelBuilder.Entity<Todo>()
            .HasOne(todo => todo.Repeatable)
            .WithOne(re => re.Todo)
            .HasForeignKey<Repeatable>(re => re.TodoId)
            .IsRequired();
        
        modelBuilder.Entity<Todo>()
            .HasIndex(todo => new {todo.UserId, todo.Name, todo.ParentId })
            .IsUnique();

        modelBuilder.Entity<Repeatable>()
            .HasIndex(re => new {re.Type, re.TodoId })
            .IsUnique();

    }
}