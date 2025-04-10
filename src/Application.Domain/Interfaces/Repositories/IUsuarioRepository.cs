using Application.Domain.Entities;
using Application.Domain.Model;

namespace Application.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Result<bool>> InsertAsync(Usuario request, CancellationToken cancellationToken);
    Task<Result<Usuario?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<Usuario?>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Usuario>>> GetAllAsync(CancellationToken cancellationToken);
}
