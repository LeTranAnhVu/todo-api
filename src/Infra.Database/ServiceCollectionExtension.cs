using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseInfraDb(this IServiceCollection services, InfraDbOptions infraDbOptions)
    {
        // services.AddScoped<>()
        services.AddDbContext<ApplicationDbContext>((sp, builder) =>
            {
                builder.UseNpgsql(infraDbOptions.DbConnectionString)
                    .UseSnakeCaseNamingConvention();
            }
        );

        return services;
    }
}

public class InfraDbOptions
{
    /// <summary>
    /// Database connection string
    /// </summary>
    public required string DbConnectionString { get; set; }

    public bool SeedData { get; set; } = false;
}