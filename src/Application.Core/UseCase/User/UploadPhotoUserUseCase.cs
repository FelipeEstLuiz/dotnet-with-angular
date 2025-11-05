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
        Result<Domain.Entities.User?> resultUser = await userRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);

        if (resultUser.IsSuccess && resultUser.Data is null)
            return Result<PhotoUserDto>.Failure("User not found.", ResponseCodes.USER_NOT_FOUND);
        else if (resultUser.IsFailure)
            return resultUser.SetResult<PhotoUserDto>();

        Result<Photo> result = await photoService.AddPhotoAsync(request.File);

        if (result.IsFailure)
            return Result<PhotoUserDto>.Failure(result.Errors);

        Photo photo = result.Data!;

        Domain.Entities.User user = resultUser.Data!;

        user.Photos.Add(photo);

        Result<bool> updateResult = await userRepository.UpdateAsync(user, cancellationToken);

        return updateResult.IsSuccess
            ? (Result<PhotoUserDto>)new PhotoUserDto(photo.Id, photo.Url, photo.PublicId, photo.UserId)
            : Result<PhotoUserDto>.Failure("Error adding user photo.");
    }
}
