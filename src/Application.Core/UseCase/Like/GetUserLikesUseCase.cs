using Application.Core.DTO.User;
using Application.Core.Model.Like;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Like;

public class GetUserLikesUseCase(IUnitOfWork unitOfWork) : IRequestHandler<GetUserLikesModel, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(GetUserLikesModel request, CancellationToken cancellationToken = default)
    {
        UserLikeParams userLikeParams = new()
        {
            UserId = request.UserId,
            Predicate = request.Predicate,
            OrderAsc = request.OrderAsc,
            OrderBy = request.OrderBy,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return (await unitOfWork.LikesRepository.GetUserLikesAsync(userLikeParams, cancellationToken))
            .Map(result => result!.Select(UserDto.Map));
    }
}
