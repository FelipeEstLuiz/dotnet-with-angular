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
        IReadOnlyList<Domain.Entities.User> users = await likesRepository.GetUserLikesAsync(request.Predicate, request.UserId, cancellationToken);
        return Result<IEnumerable<UserDto>>.Success(users.Select(UserDto.Map));
    }
}
