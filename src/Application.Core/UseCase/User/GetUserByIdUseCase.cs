using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class GetUserByIdUseCase(IUserRepository usuarioRepository)
    : IRequestHandler<GetUserByIdModel, Result<UserDto?>>
{
    public async Task<Result<UserDto?>> Handle(GetUserByIdModel request, CancellationToken cancellationToken = default)
    {
        Result<Domain.Entities.User?> usuario = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario.IsSuccess && usuario.Data is not null)
            return UserDto.Map(usuario.Data);
        else if (usuario.IsFailure)
            return Result<UserDto?>.Failure(usuario.Errors);

        return Result<UserDto?>.Failure("User not found.", ResponseCodes.NOT_FOUND);
    }
}
