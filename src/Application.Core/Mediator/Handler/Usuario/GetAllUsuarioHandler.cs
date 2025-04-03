using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Query.Usuario;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Handler.Usuario;

public class GetAllUsuarioHandler(IUsuarioRepository usuarioRepository)
    : IRequestHandler<GetAllUsuarioQuery, Result<IEnumerable<UsuarioDto>>>
{
    public async Task<Result<IEnumerable<UsuarioDto>>> Handle(
        GetAllUsuarioQuery request,
        CancellationToken cancellationToken
    )
    {
        Result<IEnumerable<Domain.Entities.Usuario>> usuarios = await usuarioRepository.GetAllAsync(cancellationToken);
        return usuarios.SetResult(data => data.Select(x => UsuarioDto.Map(x)));
    }
}
