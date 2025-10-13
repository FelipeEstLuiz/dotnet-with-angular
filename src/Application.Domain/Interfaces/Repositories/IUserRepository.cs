using Application.Domain.Entities;
using Application.Domain.Model;
using Application.Domain.VO;

namespace Application.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<bool>> InsertAsync(User user, CancellationToken cancellationToken);
    Task<Result<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<UserVo?>> GetUserVoByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<User?>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<UserVo?>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<Result<List<UserVo>>> GetAllAsync(
        QueryOptions? options = null,
        CancellationToken cancellationToken = default
    );
}
