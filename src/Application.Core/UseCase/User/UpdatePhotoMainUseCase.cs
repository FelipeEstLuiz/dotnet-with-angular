using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UpdatePhotoMainUseCase(IUserRepository userRepository)
    : IRequestHandler<UpdatePhotoMainModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdatePhotoMainModel request,
        CancellationToken cancellationToken = default
    )
    {
        Domain.Entities.User? resultUser = await userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken
        );

        if (resultUser is null)
            return Result<bool>.Failure("User not found.", ResponseCodes.USER_NOT_FOUND);

        Domain.Entities.User user = resultUser!;

        Photo? photo = user.Photos.SingleOrDefault(x => x.Id == request.PhotoId);

        if (photo is null)
            return Result<bool>.Failure("Photo not found.");
        else if (user.ImageUrl == photo.Url)
            return Result<bool>.Success(true);

        user.ImageUrl = photo.Url;

        return await userRepository.SaveChangesAsync(cancellationToken)
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("Error to set main photo.");
    }
}
