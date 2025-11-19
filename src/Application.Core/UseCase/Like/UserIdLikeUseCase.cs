using Application.Core.Model.Like;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Like;

public class UserIdLikeUseCase(ILikesRepository likesRepository) : IRequestHandler<UserIdLikeModel, Result<IReadOnlyList<int>>>
{
    public async Task<Result<IReadOnlyList<int>>> Handle(UserIdLikeModel request, CancellationToken cancellationToken = default)
        => Result.Success(await likesRepository.GetCurrentUserLikeIdAsync(request.UserId, cancellationToken));
}
