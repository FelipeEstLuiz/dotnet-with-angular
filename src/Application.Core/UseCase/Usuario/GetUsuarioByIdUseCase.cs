using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Usuario;

public class GetUsuarioByIdUseCase(IUsuarioRepository usuarioRepository)
    : IRequestHandler<GetUsuarioByIdModel, Result<UsuarioDto?>>
{
    public async Task<Result<UsuarioDto?>> Handle(GetUsuarioByIdModel request, CancellationToken cancellationToken)
    {
        Result<Domain.Entities.Usuario?> usuario = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario.IsSuccess && usuario.Data is not null)
            return UsuarioDto.Map(usuario.Data);
        else if (usuario.IsFailure)
            return Result<UsuarioDto?>.Failure(usuario.Errors);

        return Result<UsuarioDto?>.Failure("Usuário não encontrado.", Domain.Enums.ResponseCodes.NOT_FOUND);
    }
}
