using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class LikesRepository(ApplicationDbContext context) : ILikesRepository
{
    public void Delete(UserLike userLike) => context.Remove(userLike);

    public async Task<IReadOnlyList<int>> GetCurrentUserLikeIdAsync(int userId, CancellationToken cancellationToken)
        => await context
            .Likes
            .Where(x => x.SourceUserId == userId)
            .Select(x => x.TargetUserId)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId, CancellationToken cancellationToken)
        => await context
            .Likes
            .FindAsync([sourceUserId, targetUserId], cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<User>> GetUserLikesAsync(string predicate, int userId, CancellationToken cancellationToken)
    {
        IQueryable<UserLike> query = context.Likes.AsQueryable();

        switch (predicate)
        {
            case "liked":
                return await query
                    .Where(x => x.SourceUserId == userId)
                    .Select(x => x.TargetUser)
                    .ToListAsync(cancellationToken: cancellationToken);
            case "likedBy":
                return await query
                    .Where(x => x.TargetUserId == userId)
                    .Select(x => x.SourceUser)
                    .ToListAsync(cancellationToken: cancellationToken);
            default:
                IReadOnlyList<int> likeIds = await GetCurrentUserLikeIdAsync(userId, cancellationToken);
                return await query
                    .Where(x => x.TargetUserId == userId && likeIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser)
                    .ToListAsync(cancellationToken: cancellationToken);

        }
    }

    public async Task AddAsync(UserLike userLike, CancellationToken cancellationToken) => await context.Likes.AddAsync(userLike, cancellationToken);

    public async Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken) > 0;
}
