using Application.Domain.Entities;
using Application.Domain.Model;

namespace Application.Domain.Interfaces.Repositories;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLikeAsync(string sourceUserId, string targetUserId, CancellationToken cancellationToken);
    Task<Result<List<User>>> GetUserLikesAsync(UserLikeParams userLikeParams, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetCurrentUserLikeIdAsync(string userId, CancellationToken cancellationToken);
    void Delete(UserLike userLike);
    Task AddAsync(UserLike userLike, CancellationToken cancellationToken);
}
