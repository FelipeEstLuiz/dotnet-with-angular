using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Core.UseCase.User;
using Application.Core.Validator;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests.Systems.Core;

public class RequestDispatcherTests
{
    [Fact]
    public async Task Should_Return_Success_When_Valid()
    {
        Faker<InsertUserModel> faker = new Faker<InsertUserModel>()
           .RuleFor(cmd => cmd.Name, f => f.Name.FullName())
           .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
           .RuleFor(cmd => cmd.Password, f => "asd123@#02Fel")
           .RuleFor(cmd => cmd.PasswordConfirmed, (f, cmd) => cmd.Password);

        InsertUserModel command = faker.Generate();

        ITokenService tokenServiceMock = Substitute.For<ITokenService>();
        
        tokenServiceMock
            .GerarToken(Arg.Any<User>())
            .Returns(Task.FromResult("token"));

        IUserRepository usuarioRepositoryMock = Substitute.For<IUserRepository>();

        usuarioRepositoryMock
           .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
           .Returns(Task.FromResult(Result<User?>.Success(null)));

        usuarioRepositoryMock
            .InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result<bool>.Success(true)));

        ServiceCollection services = new();
        services.AddScoped<RequestDispatcher>();
        services.AddScoped<IRequestHandler<InsertUserModel, Result<LoginDto>>, InsertUserUseCase>();
        services.AddValidatorsFromAssemblyContaining<InsertUserValidator>();
        services.AddScoped(_ => usuarioRepositoryMock);
        services.AddScoped(_ => tokenServiceMock);

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        RequestDispatcher dispatcher = serviceProvider.GetRequiredService<RequestDispatcher>();

        Result<LoginDto> result = await dispatcher.Dispatch<InsertUserModel, Result<LoginDto>>(command);

        Assert.True(result.IsSuccess);
    }
}
