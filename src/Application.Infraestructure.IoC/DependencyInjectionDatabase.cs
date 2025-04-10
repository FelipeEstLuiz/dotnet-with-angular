using Application.Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionDatabase
{
    internal static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        return services;
    }
}
