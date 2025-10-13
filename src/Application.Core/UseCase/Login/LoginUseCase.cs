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

        return resultUsuario.IsSuccess
            ? await ValidarPasswordAsync(resultUsuario.Data, request.Password)
            : Result<LoginDto?>.Failure(resultUsuario.Errors);
    }

    private async Task<Result<LoginDto?>> ValidarPasswordAsync(Domain.Entities.User? usuario, string senha)
    {
        if (usuario is null)
            return Result<LoginDto?>.Failure("Usuário inválida", Domain.Enums.ResponseCodes.USER_NOT_FOUND);

        PasswordVerificationResult resultado = new PasswordHasher<Domain.Entities.User>().VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            senha
        );

        return resultado == PasswordVerificationResult.Failed
            ? Result<LoginDto?>.Failure("Senha inválida", Domain.Enums.ResponseCodes.UNAUTHORIZED)
            : Result<LoginDto?>.Success(new LoginDto(
                usuario.Id,
                usuario.UserName,
                usuario.Email,
                await tokenService.GerarToken(usuario)
            ));
    }
}
