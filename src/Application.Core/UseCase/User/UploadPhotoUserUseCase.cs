using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UploadPhotoUserUseCase(IUserRepository usuarioRepository, IPhotoService photoService) : IRequestHandler<PhotoUploadModel, Result<PhotoUserDto>>
{
    public async Task<Result<PhotoUserDto>> Handle(PhotoUploadModel request, CancellationToken cancellationToken = default)
    {
        Result<Domain.Entities.User?> resultUsuario = await usuarioRepository.GetByNameAsync(request.UserName, cancellationToken: cancellationToken);

        if (resultUsuario.IsSuccess && resultUsuario.Data is null)
            return Result<PhotoUserDto>.Failure("User not found.");
        else if (resultUsuario.IsFailure)
            return resultUsuario.SetResult<PhotoUserDto>();

        Result<Photo> result = await photoService.AddPhotoAsync(request.File);

        if (result.IsFailure)
            return Result<PhotoUserDto>.Failure(result.Errors);

        Photo photo = result.Data!;

        Domain.Entities.User user = resultUsuario.Data!;

        user.Photos.Add(photo);

        Result<bool> updateResult = await usuarioRepository.UpdateAsync(user, cancellationToken);

        return updateResult.IsSuccess
            ? (Result<PhotoUserDto>)new PhotoUserDto(photo.Id, photo.Url, photo.IsMain, photo.PublicId, photo.UserId)
            : Result<PhotoUserDto>.Failure("Error adding user photo.");
    }
}
