using Application.Core.Model.Like;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Like;

public class ToggleLikeUseCase(IUnitOfWork unitOfWork) : IRequestHandler<ToggleLikeModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(ToggleLikeModel request, CancellationToken cancellationToken = default)
    {
        if (request.SourceUserId == request.TargetUserId)
            return Result<bool>.Failure("You cannot like yourself");

        UserLike? existingLike = await unitOfWork.LikesRepository.GetUserLikeAsync(request.SourceUserId, request.TargetUserId, cancellationToken);

        if (existingLike is null)
        {
            UserLike like = new()
            {
                TargetUserId = request.TargetUserId,
                SourceUserId = request.SourceUserId
            };

            await unitOfWork.LikesRepository.AddAsync(like, cancellationToken);
        }
        else
            unitOfWork.LikesRepository.Delete(existingLike);

        return Result.Try(await unitOfWork.SaveAllChangesAsync(cancellationToken), "Failed to update like");
    }
}
