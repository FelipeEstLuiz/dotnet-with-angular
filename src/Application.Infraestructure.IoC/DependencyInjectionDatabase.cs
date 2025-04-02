using Application.Infraestructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionDatabase
{
    internal static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString)
    {
        services.AddSingleton(new DatabaseConnection(connectionString));
        return services;
    }
}
