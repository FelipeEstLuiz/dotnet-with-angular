using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.Login;

public class LoginUseCase(
    UserManager<Domain.Entities.User> userManager,
    ITokenService tokenService
) : IRequestHandler<LoginModel, Result<UserLoginDto>>
{
    public async Task<Result<UserLoginDto>> Handle(LoginModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.User? resultUser = await userManager.FindByEmailAsync(request.Email);

        return resultUser is not null
            ? await ValidarPasswordAsync(resultUser, request.Password)
            : Result.Failure<UserLoginDto>("Invalid user", Domain.Enums.ResponseCodes.USER_NOT_FOUND);
    }

    private async Task<Result<UserLoginDto>> ValidarPasswordAsync(Domain.Entities.User user, string password)
    {
        if (await userManager.CheckPasswordAsync(user, password))
        {
            string token = await tokenService.GerarToken(user);
            string refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return Result.Success(new UserLoginDto(LoginDto.Map(user, token), refreshToken));
        }

        return Result.Failure<UserLoginDto>("Invalid password", Domain.Enums.ResponseCodes.UNAUTHORIZED);
    }
}
