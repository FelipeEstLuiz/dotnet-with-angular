namespace Application.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }

    public static Usuario Create(string nome, string email, string password) => new()
    {
        Email = email,
        Nome = nome,
        Senha = password
    };
}
