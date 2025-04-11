using Application.Domain.Entities;

namespace Tests.Domain;

public class UsuarioTests
{
    [Fact]
    public void Create_DeveRetornarUsuarioComDadosCorretos()
    {
        string nome = "joao";
        string email = "joao@email.com";

        Usuario usuario = Usuario.Create(nome, email);

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal(nome, usuario.UserName);
        Assert.Equal(nome.ToUpperInvariant(), usuario.NormalizedUserName);
        Assert.Equal(email, usuario.Email);
        Assert.Equal(email.ToUpperInvariant(), usuario.NormalizedEmail);
        Assert.False(usuario.EmailConfirmed);
        Assert.NotNull(usuario.SecurityStamp);
        Assert.NotNull(usuario.ConcurrencyStamp);
        Assert.NotEqual(default, usuario.CriadoEm);
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
        Usuario usuario = Usuario.Create("ana", "ana@email.com");
        string senha = "hash123";

        usuario.SetPassword(senha);

        Assert.Equal(senha, usuario.PasswordHash);
    }
}
