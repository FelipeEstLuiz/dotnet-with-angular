using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class GetAllUserUseCase(IUserRepository userRepository)
    : IRequestHandler<GetAllUserModel, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(
        GetAllUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<List<Domain.Entities.User>> users = await userRepository.GetAllAsync(
            options: request,
            cancellationToken: cancellationToken
        );

        return users.IsFailure
            ? Result<IEnumerable<UserDto>>.Failure(users.Errors)
            : Result<IEnumerable<UserDto>>.Success(
                users.Data!.Select(UserDto.Map),
                users.TotalItems,
                users.CurrentPage,
                users.TotalPages,
                users.PageSize
            );
    }
}
