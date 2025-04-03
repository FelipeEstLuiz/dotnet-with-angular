using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Extensions;
using Application.Infraestructure.Data.Repositories.Scripts;
using Dapper;
using System.Data;

namespace Application.Infraestructure.Data.Repositories;

public class UsuarioRepository(DatabaseConnection database) : IUsuarioRepository
{
    public async Task<Result<bool>> InsertAsync(Usuario request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = database.CreateConnection();
        await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            SqlUsuario.InsertUsuario,
            request,
            cancellationToken: cancellationToken
        ));

        return Result<bool>.Success(true);
    }

    public async Task<Result<Usuario?>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        DynamicParameters dynamicParameters = new();

        string query = SqlUsuario.SelectUsuario;

        query = query.MountEqual("Email", email, ref dynamicParameters);

        return await GetUsuarioAsync(
            query,
            dynamicParameters,
            cancellationToken: cancellationToken
        );
    }

    public async Task<Result<Usuario?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        DynamicParameters dynamicParameters = new();

        string query = SqlUsuario.SelectUsuario;

        query = query.MountEqual("Id", id, ref dynamicParameters);

        return await GetUsuarioAsync(
            query,
            dynamicParameters,
            cancellationToken: cancellationToken
        );
    }

    public async Task<Result<Usuario?>> GetUsuarioAsync(
        string query,
        DynamicParameters dynamicParameters,
        CancellationToken cancellationToken
    )
    {
        using IDbConnection connection = database.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Usuario?>(new CommandDefinition(
            query,
            dynamicParameters,
            cancellationToken: cancellationToken
        ));
    }

    public async Task<Result<IEnumerable<Usuario>>> GetAllAsync(CancellationToken cancellationToken)
    {
        using IDbConnection connection = database.CreateConnection();
        return Result<IEnumerable<Usuario>>.Success(await connection.QueryAsync<Usuario>(new CommandDefinition(
            SqlUsuario.SelectUsuario,
            cancellationToken: cancellationToken
        )));
    }
}
