using System.Globalization;

namespace Application.Core.DTO.Usuario;

public record UsuarioDto : ComumDto
{
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string DataNascimento { get; private set; } = null!;
    public int Idade { get; private set; }

    public static UsuarioDto Map(Domain.Entities.User usuario) => new()
    {
        Email = usuario.Email,
        Nome = usuario.UserName,
        Id = usuario.Id,
        DataCadastro = usuario.Created,
        DataNascimento = usuario.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        Idade = usuario.GetAge()
    };
}
