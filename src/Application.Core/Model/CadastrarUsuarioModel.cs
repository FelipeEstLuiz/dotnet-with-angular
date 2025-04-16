namespace Application.Core.Model;

public record CadastrarUsuarioModel
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string SenhaConfirmacao { get; set; } = null!;
}
