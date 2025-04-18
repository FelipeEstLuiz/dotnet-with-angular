﻿using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.Login;

public class LoginUseCase(
    IUsuarioRepository usuarioRepository,
    ITokenService tokenService
) : IRequestHandler<LoginModel, Result<LoginDto?>>
{
    public async Task<Result<LoginDto?>> Handle(LoginModel request, CancellationToken cancellationToken)
    {
        Result<Domain.Entities.Usuario?> resultUsuario = await usuarioRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        return resultUsuario.IsSuccess
            ? resultUsuario.Data is null
                ? Result<LoginDto?>.Failure("Usuário inválida", Domain.Enums.ResponseCodes.USER_NOT_FOUND)
                : await ValidarPasswordAsync(resultUsuario.Data, request.Senha)
            : Result<LoginDto?>.Failure(resultUsuario.Errors);
    }

    private async Task<Result<LoginDto?>> ValidarPasswordAsync(Domain.Entities.Usuario usuario, string senha)
    {
        PasswordVerificationResult resultado = new PasswordHasher<Domain.Entities.Usuario>().VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            senha
        );

        return resultado == PasswordVerificationResult.Failed
            ? Result<LoginDto?>.Failure("Senha inválida", Domain.Enums.ResponseCodes.UNAUTHORIZED)
            : Result<LoginDto?>.Success(new LoginDto(
                usuario.UserName,
                usuario.Email,
                await tokenService.GerarToken(usuario)
            ));
    }
}
