using Application.Core.DTO.User;
using Application.Core.Model.User;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.UseCase.User;

public class RefreshTokenUseCase(UserManager<Domain.Entities.User> userManager, ITokenService tokenService) : IRequestHandler<RefreshTokenModel, Result<UserLoginDto>>
{
    public async Task<Result<UserLoginDto>> Handle(RefreshTokenModel request, CancellationToken cancellationToken = default)
    {
#pragma warning disable CS8625
        if (string.IsNullOrEmpty(request.RefreshToken)) return Result<UserLoginDto>.Success(null, responseCode: ResponseCodes.NO_CONTENT);
#pragma warning restore CS8625

        Domain.Entities.User? user = await userManager
            .Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken && x.RefreshTokenExpiry > DateTime.UtcNow, cancellationToken: cancellationToken);

        if (user is null) return Result<UserLoginDto>.Failure("UNAUTHORIZED", ResponseCodes.UNAUTHORIZED);

        string token = await tokenService.GerarToken(user);
        string refreshToken = tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);
        return Result.Success(new UserLoginDto(LoginDto.Map(user, token), refreshToken));
    }
}
