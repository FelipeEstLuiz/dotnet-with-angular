using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;

namespace Application.Core.UseCase.User;

public class GetUserByIdUseCase(IUserRepository usuarioRepository)
    : IRequestHandler<GetUserByIdModel, Result<UserDto?>>
{
    public async Task<Result<UserDto?>> Handle(GetUserByIdModel request, CancellationToken cancellationToken)
    {
        Result<UserVo?> usuario = await usuarioRepository.GetUserVoByIdAsync(request.Id, cancellationToken);

        if (usuario.IsSuccess && usuario.Data is not null)
            return UserDto.Map(usuario.Data);
        else if (usuario.IsFailure)
            return Result<UserDto?>.Failure(usuario.Errors);

        return Result<UserDto?>.Failure("Usuário não encontrado.", Domain.Enums.ResponseCodes.NOT_FOUND);
    }
}
