using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Extensions;
using Application.Infraestructure.Data.Repositories.Scripts;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Infraestructure.Data.Repositories;

public class UsuarioRepository(IDatabaseConnection database, ILogger<UsuarioRepository> logger) : IUsuarioRepository
{
    public async Task<Result<bool>> InsertAsync(Usuario request, CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection connection = database.CreateConnection();
            await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(
                SqlUsuario.InsertUsuario,
                request,
                cancellationToken: cancellationToken
            ));

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inserir usuário: {Message}", ex.Message);
            return Result<bool>.Failure("Erro ao inserir usuário");
        }
    }

    public async Task<Result<Usuario?>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            DynamicParameters dynamicParameters = new();

            string query = SqlUsuario.SelectUsuario;

            query = query.MountEqual("email", email, ref dynamicParameters);

            return await GetUsuarioAsync(
                query,
                dynamicParameters,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por email: email informado: {email}, erro: {Message}", email, ex.Message);
            return Result<Usuario?>.Failure("Erro ao obter usuário");
        }
    }

    public async Task<Result<Usuario?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            DynamicParameters dynamicParameters = new();

            string query = SqlUsuario.SelectUsuario;

            query = query.MountEqual("id", id, ref dynamicParameters);

            return await GetUsuarioAsync(
                query,
                dynamicParameters,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por id: id informado: {id}, erro: {Message}", id, ex.Message);
            return Result<Usuario?>.Failure("Erro ao obter usuário");
        }
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
        try
        {
            using IDbConnection connection = database.CreateConnection();
            return Result<IEnumerable<Usuario>>.Success(await connection.QueryAsync<Usuario>(new CommandDefinition(
                SqlUsuario.SelectUsuario,
                cancellationToken: cancellationToken
            )));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuarios: {Message}", ex.Message);
            return Result<IEnumerable<Usuario>>.Failure("Erro ao obter usuários");
        }
    }
}
