using Application.Domain.Entities;
using Application.Domain.Model;

namespace Application.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Photo>?> GetByPhotoIdAsync(string id, CancellationToken cancellationToken);
    Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<Result<List<User>>> GetAllAsync(
        UserParams userParams,
        CancellationToken cancellationToken = default
    );
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
