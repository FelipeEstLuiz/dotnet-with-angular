namespace Application.Infraestructure.Data.Repositories.Scripts;

internal static class SqlUsuario
{
    internal static string InsertUsuario = @"
        INSERT INTO usuario (Nome, Email, Senha)
        VALUES (@Nome, @Email, @Senha);
    ";

    internal static string SelectUsuario = @"
        SELECT
            Id,
            Nome,
            Email,
            Senha,
            Ativo,
            DataCadastro,
            DataAtualizacao
        FROM usuario
        WHERE 1 = 1
    ";
}
