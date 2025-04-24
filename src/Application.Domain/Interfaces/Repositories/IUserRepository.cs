using Application.Domain.Entities;
using Application.Domain.Model;

namespace Application.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<bool>> InsertAsync(User request, CancellationToken cancellationToken);
    Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<User?>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<User?>> GetByIdAsync(int userId, CancellationToken cancellationToken);
    Task<Result<List<User>>> GetAllAsync(
        QueryOptions? options = null,
        CancellationToken cancellationToken = default
    );
}
