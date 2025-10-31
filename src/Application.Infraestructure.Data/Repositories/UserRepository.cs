using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context, IAppLogger<UserRepository> logger) : IUserRepository
{
    private readonly DbSet<User> _dbSet = context.Set<User>();

    public async Task<Result<bool>> InsertAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Insert user error: {Message}", ex.Message);
            return Result<bool>.Failure("Insert user error");
        }
    }

    public async Task<Result<bool>> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Update(user);
            await context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Update user error: {Message}", ex.Message);
            return Result<bool>.Failure("Update user error");
        }
    }

    public async Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Photos)
                .Where(x => x.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error to find user by email: {Email}, error: {Message}", email, ex.Message);
            return Result<User?>.Failure("Error to find user");
        }
    }

    public async Task<Result<User?>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UserName.ToLower() == name.ToLower())
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error to find user by name: {Name}, error: {Message}", name, ex.Message);
            return Result<User?>.Failure("Error to find user");
        }
    }

    public async Task<Result<User?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error to find user by id: {Id}, error: {Message}", id, ex.Message);
            return Result<User?>.Failure("Error to find user");
        }
    }

    public async Task<Result<IEnumerable<Photo>?>> GetByPhotoIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Photos)
                .Where(x => x.Id == id)
                .SelectMany(x => x.Photos)
                .ToListAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error to find photo user by id: {Id}, error: {Message}", id, ex.Message);
            return Result<IEnumerable<Photo>?>.Failure("Error to find photo user");
        }
    }

    public async Task<Result<List<User>>> GetAllAsync(
        QueryOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Photos)
                .ApplyQueryOptionsAsync(options, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error to find users: {Message}", ex.Message);
            return Result<List<User>>.Failure("Error to find users");
        }
    }
}
