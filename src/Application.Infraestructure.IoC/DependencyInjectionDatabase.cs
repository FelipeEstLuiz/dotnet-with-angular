using Application.Domain.Interfaces.Repositories;
using Application.Infraestructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionDatabase
{
    internal static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

        return services;
    }
}
