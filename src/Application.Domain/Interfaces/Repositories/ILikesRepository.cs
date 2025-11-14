using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Repositories;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUserLikesAsync(string predicate, int userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<int>> GetCurrentUserLikeIdAsync(int userId, CancellationToken cancellationToken);
    void Delete(UserLike userLike);
    Task AddAsync(UserLike userLike, CancellationToken cancellationToken);
    Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken);
}
