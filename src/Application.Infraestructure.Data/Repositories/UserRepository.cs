using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken) => await context.Users.AddAsync(user, cancellationToken);

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken) => await context
        .Users
        .IgnoreQueryFilters()
        .Where(x => x.UserName!.ToLower() == name.ToLower() || x.FullName.ToLower() == name.ToLower())
        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken) => await context
        .Users
        .Include(x => x.Photos)
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    public async Task<IEnumerable<Photo>?> GetByPhotoIdAsync(string id, CancellationToken cancellationToken)
        => await context
            .Users
            .IgnoreQueryFilters()
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
            IQueryable<User> query = context
                .Users
                .Include(x => x.Photos)
                .AsQueryable();

            query = query.Where(x => x.Id != userParams.CurrentUserId);

            if (!string.IsNullOrWhiteSpace(userParams.Gender))
                query = query.Where(x => x.Gender.ToLower() == userParams.Gender.ToLower());

            DateOnly minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            DateOnly maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            return await query.ApplyQueryOptionsAsync(userParams, cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            return Result<List<User>>.Failure("Error to find users");
        }
    }

    public async Task<User?> GetUserByPhotoId(int photoId, CancellationToken cancellationToken) => await context
        .Users
        .Include(p => p.Photos)
        .IgnoreQueryFilters()
        .Where(p => p.Photos.Any(p => p.Id == photoId))
        .FirstOrDefaultAsync(cancellationToken);
}
