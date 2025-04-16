using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Usuario;

public class GetAllUsuarioUseCase(IUsuarioRepository usuarioRepository)
    : IRequestHandler<GetAllUsuarioModel, Result<IEnumerable<UsuarioDto>>>
{
    public async Task<Result<IEnumerable<UsuarioDto>>> Handle(
        GetAllUsuarioModel request,
        CancellationToken cancellationToken
    )
    {
        Result<List<Domain.Entities.Usuario>> usuarios = await usuarioRepository.GetAllAsync(
            options: request,
            cancellationToken: cancellationToken
        );
        return usuarios.SetResult(data => data.Select(x => UsuarioDto.Map(x)));
    }
}
