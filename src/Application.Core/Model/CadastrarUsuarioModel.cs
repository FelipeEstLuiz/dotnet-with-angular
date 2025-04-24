using Application.Domain.Entities;

namespace Application.Core.Model;

public record CadastrarUsuarioModel
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string SenhaConfirmacao { get; set; } = null!;
    public DateOnly DataNascimento { get; set; }
    public string? Introducao { get; set; }
    public string Genero { get; set; } = null!;
    public string KnowAs { get; set; } = null!;

    public User MapUsuario() => User.Create(
        name: Nome,
        email: Email,
        knowAs: KnowAs,
        gender: Genero,
        introduction: Introducao,
        dateOfBirth: DataNascimento
    );
}
