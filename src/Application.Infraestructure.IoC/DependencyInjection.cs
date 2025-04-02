using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        services.AddDatabase(connectionString);
        services.AddRepository();
        services.AddUseCase();
        services.AddServices();

        return services;
    }
}
