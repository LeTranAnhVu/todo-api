using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationProfile));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITodoService, TodoService>();
        return services;
    }
}