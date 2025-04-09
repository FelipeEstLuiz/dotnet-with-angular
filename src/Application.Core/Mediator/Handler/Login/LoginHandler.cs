using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Command.Login;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.Mediator.Handler.Login;

public class LoginHandler(IUsuarioRepository usuarioRepository) : IRequestHandler<LoginCommand, Result<UsuarioDto?>>
{
    public async Task<Result<UsuarioDto?>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Result<Domain.Entities.Usuario?> resultUsuario = await usuarioRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        return resultUsuario.IsSuccess
            ? resultUsuario.Data is null
                ? Result<UsuarioDto?>.Failure("Usuário inválida", Domain.Enums.ResponseCodes.USER_NOT_FOUND)
                : ValidarPasswordAsync(resultUsuario.Data, request.Senha)
            : Result<UsuarioDto?>.Failure(resultUsuario.Errors);
    }

    private static Result<UsuarioDto?> ValidarPasswordAsync(Domain.Entities.Usuario usuario, string senha)
    {
        PasswordVerificationResult resultado = new PasswordHasher<Domain.Entities.Usuario>().VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            senha
        );

        return resultado == PasswordVerificationResult.Failed
            ? Result<UsuarioDto?>.Failure("Senha inválida", Domain.Enums.ResponseCodes.UNAUTHORIZED)
            : UsuarioDto.Map(usuario);
    }
}
