using Application.Core.DTO.User;
using Application.Core.Model.User;
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
        UserParams userParams = new()
        {
            CurrentUserId = request.CurrentUserId,
            Gender = request.Gender,
            OrderAsc = request.OrderAsc,
            OrderBy = request.OrderBy,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            MinAge = request.MinAge,
            MaxAge = request.MaxAge
        };

        return (await userRepository.GetAllAsync(userParams, cancellationToken: cancellationToken))
            .Map(result => result!.Select(UserDto.Map));
    }
}
