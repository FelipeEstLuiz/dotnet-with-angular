using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Core.UseCase.Login;
using Application.Core.UseCase.User;
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
        services.AddScoped<IRequestHandler<InsertUserModel, Result<LoginDto>>, InsertUserUseCase>();
        services.AddScoped<IRequestHandler<LoginModel, Result<LoginDto?>>, LoginUseCase>();
        services.AddScoped<IRequestHandler<GetAllUserModel, Result<IEnumerable<UserDto>>>, GetAllUserUseCase>();
        services.AddScoped<IRequestHandler<GetUserByUserNameModel, Result<UserDto?>>, GetUserByUserNameUseCase>();
        services.AddScoped<IRequestHandler<GetUserByIdModel, Result<UserDto?>>, GetUserByIdUseCase>();
        services.AddScoped<IRequestHandler<UpdateUserModel, Result<string>>, UpdateUserUseCase>();

        services.AddValidators();

        return services;
    }

    internal static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<InsertUserValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();

        return services;
    }
}
