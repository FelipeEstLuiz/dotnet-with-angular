using Application.Domain.Converter;

namespace Application.Core.DTO.Usuario;

public record UsuarioDto
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    [Newtonsoft.Json.JsonConverter(typeof(CustomLongDateTimeConverter))]
    public DateTime DataCadastro { get; private set; }

    public static UsuarioDto Map(Domain.Entities.Usuario usuario) => new()
    {
        Email = usuario.Email,
        Nome = usuario.User_Name,
        Id = usuario.Id,
        DataCadastro = usuario.Criado_Em
    };
}
