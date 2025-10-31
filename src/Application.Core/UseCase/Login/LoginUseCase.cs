using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.Login;

public class LoginUseCase(
    IUserRepository usuarioRepository,
    ITokenService tokenService
) : IRequestHandler<LoginModel, Result<LoginDto?>>
{
    public async Task<Result<LoginDto?>> Handle(LoginModel request, CancellationToken cancellationToken = default)
    {
        Result<Domain.Entities.User?> resultUsuario = await usuarioRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        if (resultUsuario.IsSuccess)
        {
            Domain.Entities.User? usuario = resultUsuario.Data;

            if (usuario is null)
                return Result<LoginDto?>.Failure("Invalid user", Domain.Enums.ResponseCodes.USER_NOT_FOUND);

            Result<LoginDto?> resultLogin = await ValidarPasswordAsync(usuario, request.Password);

            if (resultLogin.IsSuccess)
            {
                usuario.UpdateLastActive();
                await usuarioRepository.UpdateAsync(usuario, cancellationToken: cancellationToken);
            }

            return resultLogin;
        }

        return Result<LoginDto?>.Failure(resultUsuario.Errors);
    }

    private async Task<Result<LoginDto?>> ValidarPasswordAsync(Domain.Entities.User usuario, string senha)
    {
        PasswordVerificationResult resultado = new PasswordHasher<Domain.Entities.User>().VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            senha
        );

        return resultado == PasswordVerificationResult.Failed
            ? Result<LoginDto?>.Failure("Invalid password", Domain.Enums.ResponseCodes.UNAUTHORIZED)
            : Result<LoginDto?>.Success(new LoginDto(
                usuario.Id,
                usuario.UserName,
                usuario.Email,
                await tokenService.GerarToken(usuario),
                usuario.Photos?.FirstOrDefault(x => x.IsMain)?.Url
            ));
    }
}
