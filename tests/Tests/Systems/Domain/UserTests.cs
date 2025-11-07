using Application.Domain.Entities;
using Tests.Fixtures;

namespace Tests.Systems.Domain;

public class UserTests
{
    [Fact]
    public void Create_DeveRetornarUsuarioComDadosCorretos()
    {
        string nome = "joao";
        string email = "joao@email.com";

        User usuarioMock = GenerateUser.Created();

        usuarioMock.UserName = nome;
        usuarioMock.Email = email;

        User usuario = User.Create(
            usuarioMock.UserName,
            usuarioMock.Email,
            usuarioMock.KnowAs,
            usuarioMock.Gender,
            usuarioMock.DateOfBirth,
            usuarioMock.Introduction,
            usuarioMock.Interests,
            usuarioMock.LookingFor,
            usuarioMock.City,
            usuarioMock.Country
        );

        Assert.Equal(default, usuario.Id);
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
        User usuarioMock = GenerateUser.Created();

        User usuario = User.Create(
            usuarioMock.UserName,
            usuarioMock.Email,
            usuarioMock.KnowAs,
            usuarioMock.Gender,
            usuarioMock.DateOfBirth,
            usuarioMock.Introduction,
            usuarioMock.Interests,
            usuarioMock.LookingFor,
            usuarioMock.City,
            usuarioMock.Country
        );
        string senha = "hash123";

        usuario.SetPassword(senha);

        Assert.Equal(senha, usuario.PasswordHash);
    }
}
