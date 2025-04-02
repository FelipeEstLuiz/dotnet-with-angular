using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionUseCase
{
    internal static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        return services;
    }
}
