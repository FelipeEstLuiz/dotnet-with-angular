using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Dapper;
using System.Data;

namespace Application.Infraestructure.Data.Repositories;

public class UsuarioRepository(DatabaseConnection database) : IUsuarioRepository
{
    public async Task<Result<bool>> InsertAsync(Usuario request, CancellationToken cancellationToken)
    {
        string query = @"
            INSERT INTO usuario (Nome, Email, Senha)
            VALUES (@Nome, @Email, @Senha);
        ";

        using IDbConnection connection = database.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            query,
            request,
            cancellationToken: cancellationToken
        )) > 0;
    }
}
