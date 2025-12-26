using Application.Domain.Entities;
using Tests.Fixtures;

namespace Tests.Systems.Domain;

public class UserTests
{
    [Fact]
    public void Create_DeveRetornarUsuarioComDadosCorretos()
    {
        User usuarioMock = GenerateUser.Created();

        User usuario = User.Create(
            usuarioMock.FullName,
            usuarioMock.Email!,
            usuarioMock.Gender,
            usuarioMock.DateOfBirth,
            usuarioMock.Introduction,
            usuarioMock.Interests,
            usuarioMock.LookingFor,
            usuarioMock.City,
            usuarioMock.Country
        );

        Assert.Equal(usuarioMock.FullName, usuario.FullName);
        Assert.Equal(usuarioMock.UserName, usuario.UserName);
        Assert.Equal(usuarioMock.Country, usuario.Country);
        Assert.Equal(usuarioMock.City, usuario.City);
        Assert.Equal(usuarioMock.Gender, usuario.Gender);
        Assert.Equal(usuarioMock.Interests, usuario.Interests);
        Assert.Equal(usuarioMock.Introduction, usuario.Introduction);
        Assert.Equal(usuarioMock.DateOfBirth, usuario.DateOfBirth);
        Assert.Equal(usuarioMock.LookingFor, usuario.LookingFor);
        Assert.Equal(usuarioMock.Email, usuario.Email);
        Assert.Equal(usuarioMock.NormalizedEmail, usuario.NormalizedEmail);
    }
}
