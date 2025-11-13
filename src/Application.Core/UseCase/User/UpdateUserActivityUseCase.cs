using Application.Core.Model;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UpdateUserActivityUseCase(IUserRepository userRepository) : IRequestHandler<UpdateUserActivityModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateUserActivityModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<Domain.Entities.User?> resultUser = await userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken
        );

        if (resultUser.IsSuccess && resultUser.Data is null)
            return Result<bool>.Failure("User not found.", ResponseCodes.USER_NOT_FOUND);
        else if (resultUser.IsFailure)
            return resultUser.SetResult<bool>();

        Domain.Entities.User user = resultUser.Data!;

        user.UpdateLastActive();

        return await userRepository.SaveChangesAsync(cancellationToken)
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("Error to set last activity.");
    }
}
