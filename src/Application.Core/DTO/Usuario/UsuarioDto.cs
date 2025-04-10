namespace Application.Core.DTO.Usuario;

public record UsuarioDto : ComumDto
{
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public static UsuarioDto Map(Domain.Entities.Usuario usuario) => new()
    {
        Email = usuario.Email,
        Nome = usuario.UserName,
        Id = usuario.Id,
        DataCadastro = usuario.CriadoEm
    };
}
