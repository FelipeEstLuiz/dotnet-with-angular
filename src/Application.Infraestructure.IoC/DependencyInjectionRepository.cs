using Application.Domain.Interfaces.Repositories;
using Application.Infraestructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionRepository
{
    internal static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        return services;
    }
}
