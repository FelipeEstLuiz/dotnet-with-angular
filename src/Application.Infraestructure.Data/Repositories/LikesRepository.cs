using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class LikesRepository(ApplicationDbContext context) : ILikesRepository
{
    public void Delete(UserLike userLike) => context.Remove(userLike);

    public async Task<IReadOnlyList<string>> GetCurrentUserLikeIdAsync(string userId, CancellationToken cancellationToken)
        => await context
            .Likes
            .Where(x => x.SourceUserId == userId)
            .Select(x => x.TargetUserId)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<UserLike?> GetUserLikeAsync(string sourceUserId, string targetUserId, CancellationToken cancellationToken)
        => await context
            .Likes
            .FindAsync([sourceUserId, targetUserId], cancellationToken: cancellationToken);

    public async Task<Result<List<User>>> GetUserLikesAsync(UserLikeParams userLikeParams, CancellationToken cancellationToken)
    {
        IQueryable<UserLike> query = context.Likes.AsQueryable();
        IQueryable<User> result;

        switch (userLikeParams.Predicate)
        {
            case "liked":
                result = query
                    .Where(x => x.SourceUserId == userLikeParams.UserId)
                    .Include(x => x.TargetUser.Photos)
                    .Select(x => x.TargetUser);
                break;
            case "likedBy":
                result = query
                    .Where(x => x.TargetUserId == userLikeParams.UserId)
                    .Include(x => x.SourceUser.Photos)
                    .Select(x => x.SourceUser);
                break;
            default:
                IReadOnlyList<string> likeIds = await GetCurrentUserLikeIdAsync(userLikeParams.UserId, cancellationToken);
                result = query
                    .Where(x => x.TargetUserId == userLikeParams.UserId && likeIds.Contains(x.SourceUserId))
                    .Include(x => x.SourceUser.Photos)
                    .Select(x => x.SourceUser);
                break;

        }

        return await result.ApplyQueryOptionsAsync(userLikeParams, cancellationToken: cancellationToken);
    }

    public async Task AddAsync(UserLike userLike, CancellationToken cancellationToken) => await context.Likes.AddAsync(userLike, cancellationToken);
}
