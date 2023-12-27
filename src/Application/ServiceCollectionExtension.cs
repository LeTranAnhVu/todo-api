using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}