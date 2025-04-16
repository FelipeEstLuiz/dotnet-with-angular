using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Infraestructure.Data.Repositories;

public class UsuarioRepository(ApplicationDbContext context, ILogger<UsuarioRepository> logger) : IUsuarioRepository
{
    private readonly DbSet<Usuario> _dbSet = context.Set<Usuario>();

    public async Task<Result<bool>> InsertAsync(Usuario request, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddAsync(request, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inserir usuário: {Message}", ex.Message);
            return Result<bool>.Failure("Erro ao inserir usuario");
        }
    }

    public async Task<Result<Usuario?>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(
                x => x.Email.ToLower() == email.ToLower(),
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por email: email informado: {email}, erro: {Message}", email, ex.Message);
            return Result<Usuario?>.Failure("Erro ao obter usuario");
        }
    }

    public async Task<Result<Usuario?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por id: id informado: {id}, erro: {Message}", id, ex.Message);
            return Result<Usuario?>.Failure("Erro ao obter usuario");
        }
    }

    public async Task<Result<List<Usuario>>> GetAllAsync(
        QueryOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await _dbSet.AsNoTracking().ApplyQueryOptionsAsync(options, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuarios: {Message}", ex.Message);
            return Result<List<Usuario>>.Failure("Erro ao obter usuarios");
        }
    }
}
