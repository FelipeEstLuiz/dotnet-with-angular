using Application.Core.Model;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UpdateUserUseCase(
    IUserRepository userRepository
) : IRequestHandler<UpdateUserModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<Domain.Entities.User?> resultUser = await userRepository.GetByIdAsync(
            request.Id,
            cancellationToken
        );

        if (resultUser.IsSuccess && resultUser.Data is null)
            return Result<bool>.Failure("User not found.", ResponseCodes.USER_NOT_FOUND);
        else if (resultUser.IsFailure)
            return resultUser.SetResult<bool>();

        Domain.Entities.User user = resultUser.Data!;

        user.SetName(request.Name);
        user.SetCountry(request.Country);
        user.SetCity(request.City);
        user.SetIntroduction(request.Introduction);
        user.SetInterests(request.Interests);
        user.SetLookingFor(request.LookingFor);

        return await userRepository.SaveChangesAsync(cancellationToken);
    }
}
