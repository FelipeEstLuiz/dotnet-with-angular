using Application.Domain.Entities;
using Bogus;

namespace Tests.Domain;

public class UsuarioTests
{
    [Fact]
    public void Create_DeveRetornarUsuarioComDadosCorretos()
    {
        string nome = "joao";
        string email = "joao@email.com";

        Faker<User> faker = new Faker<User>()
            .RuleFor(cmd => cmd.Id, f => f.Random.Guid())
            .RuleFor(cmd => cmd.UserName, f => nome)
            .RuleFor(cmd => cmd.Email, f => email)
            .RuleFor(u => u.DateOfBirth, f =>
            {
                DateTime date = f.Date.Past(50, DateTime.Today.AddYears(-18));
                return DateOnly.FromDateTime(date);
            })
            .RuleFor(u => u.Introduction, f => f.Lorem.Sentence())
            .RuleFor(u => u.Gender, f => f.PickRandom("Masculino", "Feminino", "Outro"))
            .RuleFor(u => u.KnowAs, f => f.Name.FirstName())
            .RuleFor(u => u.Created, f => f.Date.Past(1, DateTime.UtcNow));

        User usuarioMock = faker.Generate();

        User usuario = User.Create(
            usuarioMock.UserName,
            usuarioMock.Email,
            usuarioMock.KnowAs,
            usuarioMock.Gender,
            usuarioMock.Introduction,
            usuarioMock.DateOfBirth
        );

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal(nome, usuario.UserName);
        Assert.Equal(nome.ToUpperInvariant(), usuario.NormalizedUserName);
        Assert.Equal(email, usuario.Email);
        Assert.Equal(email.ToUpperInvariant(), usuario.NormalizedEmail);
        Assert.False(usuario.EmailConfirmed);
        Assert.NotNull(usuario.SecurityStamp);
        Assert.NotNull(usuario.ConcurrencyStamp);
        Assert.NotEqual(default, usuario.Created);
        Assert.NotEqual(default, usuario.DateOfBirth);
        Assert.Null(usuario.PhoneNumber);
        Assert.False(usuario.PhoneNumberConfirmed);
        Assert.False(usuario.TwoFactorEnabled);
        Assert.Null(usuario.LockoutEnd);
        Assert.False(usuario.LockoutEnabled);
        Assert.Equal(default, usuario.AccessFailedCount);
    }

    [Fact]
    public void SetPassword_DeveAtribuirPasswordHash()
    {
        Faker<User> faker = new Faker<User>()
            .RuleFor(cmd => cmd.Id, f => f.Random.Guid())
            .RuleFor(cmd => cmd.UserName, f => f.Person.FullName)
            .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
            .RuleFor(u => u.DateOfBirth, f =>
            {
                DateTime date = f.Date.Past(50, DateTime.Today.AddYears(-18));
                return DateOnly.FromDateTime(date);
            })
            .RuleFor(u => u.Introduction, f => f.Lorem.Sentence())
            .RuleFor(u => u.Gender, f => f.PickRandom("Masculino", "Feminino", "Outro"))
            .RuleFor(u => u.KnowAs, f => f.Name.FirstName())
            .RuleFor(u => u.Created, f => f.Date.Past(1, DateTime.UtcNow));

        User usuarioMock = faker.Generate();

        User usuario = User.Create(
            usuarioMock.UserName,
            usuarioMock.Email,
            usuarioMock.KnowAs,
            usuarioMock.Gender,
            usuarioMock.Introduction,
            usuarioMock.DateOfBirth
        );
        string senha = "hash123";

        usuario.SetPassword(senha);

        Assert.Equal(senha, usuario.PasswordHash);
    }
}
