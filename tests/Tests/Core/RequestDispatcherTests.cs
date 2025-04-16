using Application.Core.Common.Dispatcher;
using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Core.UseCase.Usuario;
using Application.Core.Validator;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests.Core;

public class RequestDispatcherTests
{
    [Fact]
    public async Task Should_Return_Success_When_Valid()
    {
        Faker<CadastrarUsuarioModel> faker = new Faker<CadastrarUsuarioModel>()
           .RuleFor(cmd => cmd.Nome, f => f.Name.FullName())
           .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
           .RuleFor(cmd => cmd.Senha, f => "asd123@#02Fel")
           .RuleFor(cmd => cmd.SenhaConfirmacao, (f, cmd) => cmd.Senha);

        CadastrarUsuarioModel command = faker.Generate();

        ITokenService tokenServiceMock = Substitute.For<ITokenService>();
        
        tokenServiceMock
            .GerarToken(Arg.Any<Usuario>())
            .Returns(Task.FromResult("token"));

        IUsuarioRepository usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();

        usuarioRepositoryMock
           .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
           .Returns(Task.FromResult(Result<Usuario?>.Success(null)));

        usuarioRepositoryMock
            .InsertAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result<bool>.Success(true)));

        ServiceCollection services = new();
        services.AddScoped<RequestDispatcher>();
        services.AddScoped<IRequestHandler<CadastrarUsuarioModel, Result<LoginDto>>, CadastrarUsuarioUseCase>();
        services.AddValidatorsFromAssemblyContaining<CadastrarUsuarioValidator>();
        services.AddScoped(_ => usuarioRepositoryMock);
        services.AddScoped(_ => tokenServiceMock);

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        RequestDispatcher dispatcher = serviceProvider.GetRequiredService<RequestDispatcher>();

        Result<LoginDto> result = await dispatcher.Dispatch<CadastrarUsuarioModel, Result<LoginDto>>(command);

        Assert.True(result.IsSuccess);
    }
}
