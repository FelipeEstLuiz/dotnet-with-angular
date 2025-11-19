using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UploadPhotoUserUseCase(IUserRepository userRepository, IPhotoService photoService) : IRequestHandler<PhotoUploadModel, Result<PhotoUserDto>>
{
    public async Task<Result<PhotoUserDto>> Handle(PhotoUploadModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.User? resultUser = await userRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);

        if (resultUser is null)
            return Result.Failure<PhotoUserDto>("User not found.", ResponseCodes.USER_NOT_FOUND);

        Result<Photo> result = await photoService.AddPhotoAsync(request.File);

        if (result.IsFailure)
            return Result.Failure<PhotoUserDto>(result.Errors);

        Photo photo = result.Data!;

        Domain.Entities.User user = resultUser!;

        if (user.Photos.Count == 0)
            user.ImageUrl = photo.Url;

        user.Photos.Add(photo);

        return await userRepository.SaveChangesAsync(cancellationToken)
            ? Result.Success(new PhotoUserDto(photo.Id, photo.Url, photo.PublicId, photo.UserId))
            : Result.Failure<PhotoUserDto>("Error adding user photo.");
    }
}
