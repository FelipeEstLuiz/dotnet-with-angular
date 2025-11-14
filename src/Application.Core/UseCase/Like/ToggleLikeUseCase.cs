using Application.Core.Model.Like;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Like;

public class ToggleLikeUseCase(ILikesRepository likesRepository) : IRequestHandler<ToggleLikeModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(ToggleLikeModel request, CancellationToken cancellationToken = default)
    {
        if (request.SourceUserId == request.TargetUserId)
            return Result<bool>.Failure("You cannot like yourself");

        UserLike? existingLike = await likesRepository.GetUserLikeAsync(request.SourceUserId, request.TargetUserId, cancellationToken);

        if (existingLike is null)
        {
            UserLike like = new()
            {
                TargetUserId = request.TargetUserId,
                SourceUserId = request.SourceUserId
            };

            await likesRepository.AddAsync(like, cancellationToken);
        }
        else
            likesRepository.Delete(existingLike);

        return await likesRepository.SaveAllChangesAsync(cancellationToken)
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("Failed to update like");
    }
}
