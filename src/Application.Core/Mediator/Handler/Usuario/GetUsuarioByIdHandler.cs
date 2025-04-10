using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Query.Usuario;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Handler.Usuario;

public class GetUsuarioByIdHandler(IUsuarioRepository usuarioRepository)
    : IRequestHandler<GetUsuarioByIdQuery, Result<UsuarioDto?>>
{
    public async Task<Result<UsuarioDto?>> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken)
    {
        Result<Domain.Entities.Usuario?> usuario = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario.IsSuccess && usuario.Data is not null)
            return UsuarioDto.Map(usuario.Data);
        else if (usuario.IsFailure)
            return Result<UsuarioDto?>.Failure(usuario.Errors);

        return Result<UsuarioDto?>.Failure("Usuário não encontrado.", Domain.Enums.ResponseCodes.NOT_FOUND);
    }
}
