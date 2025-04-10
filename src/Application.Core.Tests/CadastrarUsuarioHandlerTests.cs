using Application.Core.Mediator.Command.Usuario;
using Application.Core.Mediator.Handler.Usuario;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.Util;
using Bogus;
using NSubstitute;

namespace Application.Core.Tests;

public class CadastrarUsuarioHandlerTests
{
    public readonly CadastrarUsuarioHandler _cadastrarUsuarioHandler;
    public readonly IUsuarioRepository _usuarioRepositoryMock;
    public readonly ITokenService _tokenServiceMock;

    public CadastrarUsuarioHandlerTests()
    {
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();
        _tokenServiceMock = Substitute.For<ITokenService>();
        _cadastrarUsuarioHandler = new(_usuarioRepositoryMock, _tokenServiceMock);
    }

    [Fact]
    public async Task Handle_Deve_Inserir_Usuario_Se_Email_Nao_Existir()
    {
        Faker<CadastrarUsuarioCommand> faker = new Faker<CadastrarUsuarioCommand>()
            .RuleFor(cmd => cmd.Nome, f => f.Name.FullName())
            .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
            .RuleFor(cmd => cmd.Senha, f => f.Internet.Password(8))
            .RuleFor(cmd => cmd.SenhaConfirmacao, (f, cmd) => cmd.Senha);

        CadastrarUsuarioCommand command = faker.Generate();

        _usuarioRepositoryMock
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result<Usuario?>.Success(null)));

        _usuarioRepositoryMock
            .InsertAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result<bool>.Success(true)));

        _tokenServiceMock
            .GerarToken(Arg.Any<Usuario>())
            .Returns(Task.FromResult("token"));

        Result<DTO.Usuario.LoginDto> result = await _cadastrarUsuarioHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data?.Token == "token");
    }

    [Fact]
    public async Task Handle_Deve_Retornar_Erro_Se_Email_Ja_Cadastrado()
    {
        Faker<CadastrarUsuarioCommand> faker = new Faker<CadastrarUsuarioCommand>()
            .RuleFor(cmd => cmd.Nome, f => f.Name.FullName())
            .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
            .RuleFor(cmd => cmd.Senha, f => f.Internet.Password(8))
            .RuleFor(cmd => cmd.SenhaConfirmacao, (f, cmd) => cmd.Senha);

        CadastrarUsuarioCommand command = faker.Generate();

        Usuario usuarioMock = Usuario.Create(
            command.Nome,
            command.Email
        );

        _usuarioRepositoryMock
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result<Usuario?>.Success(usuarioMock)));

        Result<DTO.Usuario.LoginDto> result = await _cadastrarUsuarioHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("E-mail ja cadastrado", result.Errors);
    }    
}