using Application.Core.DTO.User;
using Application.Core.Model.User;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class GetUserByUserNameUseCase(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserByUserNameModel, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByUserNameModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.User? user = await unitOfWork.UserRepository.GetByNameAsync(request.UserName, cancellationToken);
        return user is not null ? Result.Success(UserDto.Map(user)) : Result.Failure<UserDto>("User not found.", ResponseCodes.NOT_FOUND);
    }
}