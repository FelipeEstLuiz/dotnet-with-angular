using Application.Core.DTO.User;
using Application.Core.Model.Like;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Like;

public class GetUserLikesUseCase(ILikesRepository likesRepository) : IRequestHandler<GetUserLikesModel, Result<IEnumerable<UserDto>>>
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

        Result<List<Domain.Entities.User>> result = await likesRepository.GetUserLikesAsync(userLikeParams, cancellationToken);
        return result.IsFailure
            ? Result<IEnumerable<UserDto>>.Failure(result.Errors)
            : Result<IEnumerable<UserDto>>.Success(
                result.Data!.Select(UserDto.Map),
                result.TotalItems,
                result.CurrentPage,
                result.TotalPages,
                result.PageSize
            );
    }
}
