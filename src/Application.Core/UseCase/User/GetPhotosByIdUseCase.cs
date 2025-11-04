using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class GetPhotosByIdUseCase(IUserRepository userRepository)
    : IRequestHandler<GetUserPhotoByIdModel, Result<IEnumerable<PhotoUserDto>?>>
{
    public async Task<Result<IEnumerable<PhotoUserDto>?>> Handle(
        GetUserPhotoByIdModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<IEnumerable<Photo>?> result = await userRepository.GetByPhotoIdAsync(request.Id, cancellationToken);

        return result.IsSuccess
            ? Result<IEnumerable<PhotoUserDto>?>.Success(result.Data?.Select(x => new PhotoUserDto(
                x.Id,
                x.Url,
                x.PublicId,
                x.UserId
            )))
            : Result<IEnumerable<PhotoUserDto>?>.Failure(result.Errors);
    }
}
