using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;

namespace Application.Core.UseCase.User;
public class GetUserByUserNameUseCase(IUserRepository usuarioRepository)
    : IRequestHandler<GetUserByUserNameModel, Result<UserDto?>>
{
    public async Task<Result<UserDto?>> Handle(GetUserByUserNameModel request, CancellationToken cancellationToken = default)
    {
        Result<UserVo?> usuario = await usuarioRepository.GetByNameAsync(request.UserName, cancellationToken);

        if (usuario.IsSuccess && usuario.Data is not null)
            return UserDto.Map(usuario.Data);
        else if (usuario.IsFailure)
            return Result<UserDto?>.Failure(usuario.Errors);

        return Result<UserDto?>.Failure("Usuario nao encontrado.", Domain.Enums.ResponseCodes.NOT_FOUND);
    }
}