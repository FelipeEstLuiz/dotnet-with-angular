using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Core.UseCase.Login;
using Application.Core.UseCase.Usuario;
using Application.Core.Validator;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionUseCase
{
    internal static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<CadastrarUsuarioModel, Result<LoginDto>>, CadastrarUsuarioUseCase>();
        services.AddScoped<IRequestHandler<LoginModel, Result<LoginDto?>>, LoginUseCase>();
        services.AddScoped<IRequestHandler<GetAllUsuarioModel, Result<IEnumerable<UsuarioDto>>>, GetAllUsuarioUseCase>();
        services.AddScoped<IRequestHandler<GetUsuarioByIdModel, Result<UsuarioDto?>>, GetUsuarioByIdUseCase>();

        services.AddValidators();

        return services;
    }

    internal static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CadastrarUsuarioValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();

        return services;
    }
}
