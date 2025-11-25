using Application.Core.Model.User;
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
        Domain.Entities.User? resultUser = await userRepository.GetByIdAsync(
            request.Id!,
            cancellationToken
        );

        if (resultUser is null)
            return Result.IsFailure("User not found.", ResponseCodes.USER_NOT_FOUND);

        Domain.Entities.User user = resultUser!;

        user.SetName(request.FullName);
        user.SetCountry(request.Country);
        user.SetCity(request.City);
        user.SetIntroduction(request.Introduction);
        user.SetInterests(request.Interests);
        user.SetLookingFor(request.LookingFor);

        return await userRepository.SaveChangesAsync(cancellationToken);
    }
}
