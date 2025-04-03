using Application.Domain.Converter;

namespace Application.Core.DTO.Usuario;

public record UsuarioDto
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public bool Ativo { get; private set; }

    [Newtonsoft.Json.JsonConverter(typeof(CustomLongDateTimeConverter))]
    public DateTime DataCadastro { get; private set; }

    [Newtonsoft.Json.JsonConverter(typeof(CustomLongDateTimeConverter))]
    public DateTime DataAtualizacao { get; private set; }

    public static UsuarioDto Map(Domain.Entities.Usuario usuario) => new()
    {
        Email = usuario.Email,
        Nome = usuario.Nome,
        Id = usuario.Id,
        Ativo = usuario.Ativo,
        DataAtualizacao = usuario.DataAtualizacao,
        DataCadastro = usuario.DataCadastro
    };
}
