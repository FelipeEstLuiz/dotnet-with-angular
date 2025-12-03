using Application.Core.Model.Admin;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using CloudinaryDotNet.Actions;

namespace Application.Core.UseCase.Admin;

public class ModeratePhotoRejectUseCase(IUnitOfWork unitOfWork, IPhotoService photoService) : IRequestHandler<ModeratePhotoRejectModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(ModeratePhotoRejectModel request, CancellationToken cancellationToken = default)
    {
        Photo? photo = await unitOfWork.PhotoRepository.GetPhotoById(request.PhotoId, cancellationToken);

        if (photo is not null)
        {
            if (!string.IsNullOrWhiteSpace(photo.PublicId))
            {
                Result<DeletionResult> result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.IsSuccess && result.Data is not null && result.Data.Result == "ok")
                    unitOfWork.PhotoRepository.RemovePhoto(photo);
            }
            else
                unitOfWork.PhotoRepository.RemovePhoto(photo);
        }

        await unitOfWork.SaveAllChangesAsync(cancellationToken);

        return Result.IsSuccess();
    }
}
