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
    public async Task AddAsync(User user, CancellationToken cancellationToken) => await context.Users.AddAsync(user, cancellationToken);

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken) > 0;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await context
            .Users
            .Where(x => x.Email.ToLower() == email.ToLower())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);


    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => await context
            .Users
            .Where(x => x.UserName.ToLower() == name.ToLower())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) => await context
        .Users
        .Include(x => x.Photos)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    public async Task<IEnumerable<Photo>?> GetByPhotoIdAsync(int id, CancellationToken cancellationToken)
        => await context
            .Users
            .Where(x => x.Id == id)
            .SelectMany(x => x.Photos)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<Result<List<User>>> GetAllAsync(
        UserParams userParams,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            IQueryable<User> query = context.Users.AsQueryable();

            query = query.Where(x => x.Id != userParams.CurrentUserId);

            if (!string.IsNullOrWhiteSpace(userParams.Gender))
                query = query.Where(x => x.Gender.ToLower() == userParams.Gender.ToLower());

            DateOnly minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            DateOnly maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            return await query.ApplyQueryOptionsAsync(userParams, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error to find users: {Message}", ex.Message);
            return Result<List<User>>.Failure("Error to find users");
        }
    }
}
