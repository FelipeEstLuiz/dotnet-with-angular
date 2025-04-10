using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Command.Usuario;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.Mediator.Handler.Usuario;

public class CadastrarUsuarioHandler(
    IUsuarioRepository usuarioRepository,
    ITokenService tokenService
) : IRequestHandler<CadastrarUsuarioCommand, Result<LoginDto>>
{
    public async Task<Result<LoginDto>> Handle(
        CadastrarUsuarioCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<Domain.Entities.Usuario?> resultUsuario = await usuarioRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        if (resultUsuario.IsSuccess && resultUsuario.Data is not null)
            return Result<LoginDto>.Failure("E-mail já cadastrado");
        else if (resultUsuario.IsFailure)
            return Result<LoginDto>.Failure(resultUsuario.Errors);

        Domain.Entities.Usuario usuario = Domain.Entities.Usuario.Create(
            nome: request.Nome,
            email: request.Email
        );

        PasswordHasher<Domain.Entities.Usuario> hasher = new();

        usuario.SetPassword(hasher.HashPassword(usuario, request.Senha));

        Result<bool> resultInsert = await usuarioRepository.InsertAsync(usuario, cancellationToken);

        return resultInsert.IsSuccess
            ? Result<LoginDto>.Success(new LoginDto(usuario.UserName, usuario.Email, await tokenService.GerarToken(usuario)))
            : Result<LoginDto>.Failure(resultInsert.Errors);
    }
}
