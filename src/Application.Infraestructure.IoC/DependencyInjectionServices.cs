using Application.Core.Services;
using Application.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionServices
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

        return services;
    }
}
