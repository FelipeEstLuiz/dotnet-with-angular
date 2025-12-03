using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UpdatePhotoMainUseCase(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePhotoMainModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdatePhotoMainModel request,
        CancellationToken cancellationToken = default
    )
    {
        Domain.Entities.User? resultUser = await unitOfWork.UserRepository.GetByIdAsync(
            request.UserId,
            cancellationToken
        );

        if (resultUser is null)
            return Result.IsFailure("User not found.", ResponseCodes.USER_NOT_FOUND);

        Domain.Entities.User user = resultUser!;

        Photo? photo = user.Photos.SingleOrDefault(x => x.Id == request.PhotoId);

        if (photo is null)
            return Result.IsFailure("Photo not found.");
        else if (photo.IsMain)
            return Result.Success(true);

        Photo? currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain is not null) currentMain.IsMain = false;
        photo.IsMain = true;
        user.ImageUrl = photo.Url;

        return Result.Try(await unitOfWork.SaveAllChangesAsync(cancellationToken), "Error to set main photo.");
    }
}
