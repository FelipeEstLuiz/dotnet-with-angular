using Application.Core.DTO.User;
using Application.Core.Model.Admin;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Admin;

public class GetPhotoForModerateUseCase(IUnitOfWork unitOfWork) : IRequestHandler<GetPhotoForModerateModel, Result<IEnumerable<PhotoUserDto>>>
{
    public async Task<Result<IEnumerable<PhotoUserDto>>> Handle(GetPhotoForModerateModel request, CancellationToken cancellationToken = default)
    {
        IEnumerable<Photo> photos = await unitOfWork.PhotoRepository.GetUnapprovedPhotos(cancellationToken);

        return Result<IEnumerable<PhotoUserDto>>.Success(photos.Select(x => new PhotoUserDto(
            x.Id,
            x.Url,
            x.PublicId,
            x.UserId,
            x.IsMain,
            x.IsApproved,
            x.User?.FullName ?? x.User?.UserName
        )));
    }
}
