﻿using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseInfraDb(this IServiceCollection services, InfraDbOptions infraDbOptions)
    {
        // EF core interceptors
        services.AddScoped<AuditableInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, builder) =>
            {
                var auditInterceptor = sp.GetService<AuditableInterceptor>() ?? throw new NullReferenceException(nameof(AuditableInterceptor));
                builder.UseNpgsql(infraDbOptions.DbConnectionString)
                    .AddInterceptors(auditInterceptor)
                    .UseSnakeCaseNamingConvention();
            }
        );

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

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