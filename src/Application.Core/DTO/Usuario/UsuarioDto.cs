using Application.Domain.Converter;
using Application.Domain.Extensions;

namespace Application.Core.DTO.Usuario;

public record UsuarioDto : ComumDto
{
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    [Newtonsoft.Json.JsonConverter(typeof(CustomDateTimeConverter))]
    public DateOnly DataNascimento { get; private set; }

    public int Idade => DataNascimento.CalcularIdade();

    public static UsuarioDto Map(Domain.Entities.User usuario) => new()
    {
        Email = usuario.Email,
        Nome = usuario.UserName,
        Id = usuario.Id,
        DataCadastro = usuario.Created,
        DataNascimento = usuario.DateOfBirth
    };
}
