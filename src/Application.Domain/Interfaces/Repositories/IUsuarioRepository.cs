using Application.Domain.Entities;
using Application.Domain.Model;

namespace Application.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Result<bool>> InsertAsync(Usuario request, CancellationToken cancellationToken);
}
