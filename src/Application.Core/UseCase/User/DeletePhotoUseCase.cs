using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using CloudinaryDotNet.Actions;

namespace Application.Core.UseCase.User;

public class DeletePhotoUseCase(IUserRepository userRepository, IPhotoService photoService)
    : IRequestHandler<DeletePhotoModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePhotoModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.User? resultUser = await userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken
        );

        if (resultUser is null)
            return Result<bool>.Failure("User not found.", ResponseCodes.USER_NOT_FOUND);

        Domain.Entities.User user = resultUser;

        Photo? photo = user.Photos.SingleOrDefault(x => x.Id == request.PhotoId);

        if (photo is null)
            return Result<bool>.Failure("Photo not found.");
        else if (user.ImageUrl == photo.Url)
            return Result<bool>.Failure("The main photo cannot be removed.");

        if (!string.IsNullOrWhiteSpace(photo.PublicId))
        {
            Result<DeletionResult> resultDeletePhotoPublic = await photoService.DeletePhotoAsync(photo.PublicId);

            if (resultDeletePhotoPublic.IsFailure)
                return Result<bool>.Failure(resultDeletePhotoPublic.Errors);
            else if (resultDeletePhotoPublic.Data is not null && resultDeletePhotoPublic.Data.Error is not null)
                return Result<bool>.Failure(resultDeletePhotoPublic.Data.Error.Message);
        }

        user.Photos.Remove(photo);

        return await userRepository.SaveChangesAsync(cancellationToken)
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("Error to remove photo.");
    }
}
