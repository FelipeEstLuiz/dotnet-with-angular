using Application.Core.Model.Like;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Like;

public class UserIdLikeUseCase(IUnitOfWork unitOfWork) : IRequestHandler<UserIdLikeModel, Result<IReadOnlyList<string>>>
{
    public async Task<Result<IReadOnlyList<string>>> Handle(UserIdLikeModel request, CancellationToken cancellationToken = default)
        => Result.Success(await unitOfWork.LikesRepository.GetCurrentUserLikeIdAsync(request.UserId, cancellationToken));
}
