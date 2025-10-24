using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class GetAllUserUseCase(IUserRepository usuarioRepository)
    : IRequestHandler<GetAllUserModel, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(
        GetAllUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<List<Domain.Entities.User>> usuarios = await usuarioRepository.GetAllAsync(
            options: request,
            cancellationToken: cancellationToken
        );
        return usuarios.SetResult(data => data.Select(x => UserDto.Map(x)));
    }
}
