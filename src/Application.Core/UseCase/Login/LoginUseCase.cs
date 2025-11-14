using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.Login;

public class LoginUseCase(
    IUserRepository userRepository,
    ITokenService tokenService
) : IRequestHandler<LoginModel, Result<LoginDto?>>
{
    public async Task<Result<LoginDto?>> Handle(LoginModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.User? resultUser = await userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        return await ValidarPasswordAsync(resultUser, request.Password);
    }

    private async Task<Result<LoginDto?>> ValidarPasswordAsync(Domain.Entities.User? user, string senha)
    {
        if (user is null)
            return Result<LoginDto?>.Failure("Invalid user", Domain.Enums.ResponseCodes.USER_NOT_FOUND);

        PasswordVerificationResult resultado = new PasswordHasher<Domain.Entities.User>().VerifyHashedPassword(
            user,
            user.PasswordHash,
            senha
        );

        return resultado == PasswordVerificationResult.Failed
            ? Result<LoginDto?>.Failure("Invalid password", Domain.Enums.ResponseCodes.UNAUTHORIZED)
            : Result<LoginDto?>.Success(new LoginDto(
                user.Id,
                user.UserName,
                user.Email,
                await tokenService.GerarToken(user),
                user.ImageUrl
            ));
    }
}
