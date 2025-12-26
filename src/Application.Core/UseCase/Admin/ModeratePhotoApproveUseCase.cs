using Application.Core.Model.Admin;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Admin;

public class ModeratePhotoApproveUseCase(IUnitOfWork unitOfWork) : IRequestHandler<ModeratePhotoApproveModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(ModeratePhotoApproveModel request, CancellationToken cancellationToken = default)
    {
        Photo? photo = await unitOfWork.PhotoRepository.GetPhotoById(request.PhotoId, cancellationToken);

        if (photo is null)
            return Result<bool>.Failure("Photo not found", Domain.Enums.ResponseCodes.NOT_FOUND);

        photo.IsApproved = true;

        Domain.Entities.User? user = await unitOfWork.UserRepository.GetUserByPhotoId(request.PhotoId, cancellationToken);

        if (user is null)
            return Result<bool>.Failure("User not found", Domain.Enums.ResponseCodes.NOT_FOUND);

        if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

        await unitOfWork.SaveAllChangesAsync(cancellationToken);

        return Result.IsSuccess();
    }
}
