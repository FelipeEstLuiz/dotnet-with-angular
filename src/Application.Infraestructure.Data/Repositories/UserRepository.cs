using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Domain.VO;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Infraestructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger) : IUserRepository
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
            logger.LogError(ex, "Erro ao inserir usuário: {Message}", ex.Message);
            return Result<bool>.Failure("Erro ao inserir usuario");
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
            logger.LogError(ex, "Erro ao atualizar usuário: {Message}", ex.Message);
            return Result<bool>.Failure("Erro ao atualizar usuario");
        }
    }

    public async Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por email: {Email}, erro: {Message}", email, ex.Message);
            return Result<User?>.Failure("Erro ao obter usuario");
        }
    }

    public async Task<Result<UserVo?>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.UserName.ToLower() == name.ToLower())
                .Select(x => new UserVo()
                {
                    City = x.City,
                    Country = x.Country,
                    Created = x.Created,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    Gender = x.Gender,
                    Id = x.Id,
                    Interests = x.Interests,
                    Introduction = x.Introduction,
                    LastActive = x.LastActive,
                    KnowAs = x.KnowAs,
                    LookingFor = x.LookingFor,
                    Name = x.UserName,
                    Photos = x.Photos.Select(p => new PhotoVo()
                    {
                        Id = p.Id,
                        IsMain = p.IsMain,
                        Url = p.Url
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por name: name informado: {name}, erro: {Message}", name, ex.Message);
            return Result<UserVo?>.Failure("Erro ao obter usuario");
        }
    }

    public async Task<Result<UserVo?>> GetUserVoByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new UserVo()
                {
                    City = x.City,
                    Country = x.Country,
                    Created = x.Created,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    Gender = x.Gender,
                    Id = x.Id,
                    Interests = x.Interests,
                    Introduction = x.Introduction,
                    KnowAs = x.KnowAs,
                    LookingFor = x.LookingFor,
                    Name = x.UserName,
                    Photos = x.Photos.Select(p => new PhotoVo()
                    {
                        Id = p.Id,
                        IsMain = p.IsMain,
                        Url = p.Url
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por id: id informado: {id}, erro: {Message}", id, ex.Message);
            return Result<UserVo?>.Failure("Erro ao obter usuario");
        }
    }

    public async Task<Result<User?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuario por id: id informado: {id}, erro: {Message}", id, ex.Message);
            return Result<User?>.Failure("Erro ao obter usuario");
        }
    }

    public async Task<Result<List<UserVo>>> GetAllAsync(
        QueryOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await _dbSet
            .AsNoTracking()
            .Select(x => new UserVo()
            {
                City = x.City,
                Country = x.Country,
                Created = x.Created,
                DateOfBirth = x.DateOfBirth,
                Email = x.Email,
                Gender = x.Gender,
                Id = x.Id,
                Interests = x.Interests,
                Introduction = x.Introduction,
                KnowAs = x.KnowAs,
                LookingFor = x.LookingFor,
                Name = x.UserName,
                Photos = x.Photos.Select(p => new PhotoVo()
                {
                    Id = p.Id,
                    IsMain = p.IsMain,
                    Url = p.Url
                }).ToList()
            })
            .ApplyQueryOptionsAsync(options, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro obter usuarios: {Message}", ex.Message);
            return Result<List<UserVo>>.Failure("Erro ao obter usuarios");
        }
    }
}
