using Application.Core.DTO.User;
using Application.Core.Model.User;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.User;

public class InsertUserUseCase(
    UserManager<Domain.Entities.User> userManager,
    ITokenService tokenService
) : IRequestHandler<InsertUserModel, Result<UserLoginDto>>
{
    public async Task<Result<UserLoginDto>> Handle(
        InsertUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Domain.Entities.User user = request.MapUsuario();

        IdentityResult result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Member");
            string token = await tokenService.GerarToken(user);
            string refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);
            return Result.Success(new UserLoginDto(LoginDto.Map(user, token), refreshToken));
        }

        return Result.Failure<UserLoginDto>(result.Errors.Select(x => x.Description));
    }
}
