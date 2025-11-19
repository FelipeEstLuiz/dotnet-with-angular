using Application.Core.DTO.User;
using Application.Core.Model.User;
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
        IEnumerable<Photo>? photos = await userRepository.GetByPhotoIdAsync(request.Id, cancellationToken);

        return Result.Success(photos?.Select(x => new PhotoUserDto(
            x.Id,
            x.Url,
            x.PublicId,
            x.UserId
        )));
    }
}
