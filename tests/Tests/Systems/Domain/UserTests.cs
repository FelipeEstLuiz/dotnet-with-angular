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
            usuarioMock.FullName,
            usuarioMock.Email,
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
        Assert.NotEqual(default, usuario.DateOfBirth);
    }
}
