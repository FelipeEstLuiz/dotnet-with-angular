using Application.Core.DTO.User;
using Application.Core.Model.User;
using Application.Core.UseCase.User;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Bogus;
using NSubstitute;

namespace Tests.Systems.Core;

public class CadastrarUsuarioHandlerTests
{
    public readonly InsertUserUseCase _cadastrarUsuarioHandler;
    public readonly IUserRepository _usuarioRepositoryMock;
    public readonly ITokenService _tokenServiceMock;

    public CadastrarUsuarioHandlerTests()
    {
        _usuarioRepositoryMock = Substitute.For<IUserRepository>();
        _tokenServiceMock = Substitute.For<ITokenService>();
        _cadastrarUsuarioHandler = new(_usuarioRepositoryMock, _tokenServiceMock);
    }

    [Fact]
    public async Task Handle_Deve_Inserir_Usuario_Se_Email_Nao_Existir()
    {
        InsertUserModel command = Generate();

        _usuarioRepositoryMock
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)null));

        _usuarioRepositoryMock
            .AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result<bool>.Success(true)));

        _usuarioRepositoryMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        _tokenServiceMock
            .GerarToken(Arg.Any<User>())
            .Returns(Task.FromResult("token"));

        Result<LoginDto> result = await _cadastrarUsuarioHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("token", result.Data?.Token);
    }

    [Fact]
    public async Task Handle_Deve_Retornar_Erro_Se_Email_Ja_Cadastrado()
    {
        InsertUserModel command = Generate();

        User usuarioMock = command.MapUsuario();

        _usuarioRepositoryMock
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)usuarioMock));

        Result<LoginDto> result = await _cadastrarUsuarioHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("E-mail already exists", result.Errors);
    }

    private static InsertUserModel Generate() => new Faker<InsertUserModel>()
        .RuleFor(cmd => cmd.Name, f => f.Name.FullName())
        .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
        .RuleFor(cmd => cmd.Password, f => f.Internet.Password(8))
        .RuleFor(cmd => cmd.PasswordConfirmed, (f, cmd) => cmd.Password)
        .RuleFor(u => u.DateOfBirth, f =>
        {
            DateTime date = f.Date.Past(50, DateTime.Today.AddYears(-18));
            return DateOnly.FromDateTime(date);
        })
        .RuleFor(u => u.Introduction, f => f.Lorem.Sentence())
        .RuleFor(u => u.Gender, f => f.PickRandom("Masculino", "Feminino", "Outro"))
        .RuleFor(u => u.KnowAs, f => f.Name.FirstName())
        .Generate();
}
