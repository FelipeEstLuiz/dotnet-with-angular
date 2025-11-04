using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class GetUserByUserNameUseCase(IUserRepository userRepository)
    : IRequestHandler<GetUserByUserNameModel, Result<UserDto?>>
{
    public async Task<Result<UserDto?>> Handle(GetUserByUserNameModel request, CancellationToken cancellationToken = default)
    {
        Result<Domain.Entities.User?> user = await userRepository.GetByNameAsync(request.UserName, cancellationToken);

        if (user.IsSuccess && user.Data is not null)
            return UserDto.Map(user.Data);
        else if (user.IsFailure)
            return Result<UserDto?>.Failure(user.Errors);

        return Result<UserDto?>.Failure("User not found.", ResponseCodes.NOT_FOUND);
    }
}