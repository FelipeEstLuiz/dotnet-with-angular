using Application.Core.Mediator.Command.Usuario;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.Mediator.Handler.Usuario;

public class CadastrarUsuarioHandler(IUsuarioRepository usuarioRepository)
    : IRequestHandler<CadastrarUsuarioCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        CadastrarUsuarioCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<Domain.Entities.Usuario?> resultUsuario = await usuarioRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        if (resultUsuario.IsSuccess && resultUsuario.Data is not null)
            return Result<bool>.Failure("E-mail já cadastrado");
        else if (resultUsuario.IsFailure)
            return Result<bool>.Failure(resultUsuario.Errors);

        Domain.Entities.Usuario usuario = Domain.Entities.Usuario.Create(
            nome: request.Nome,
            email: request.Email
        );

        PasswordHasher<Domain.Entities.Usuario> hasher = new();

        usuario.SetPassword(hasher.HashPassword(usuario, request.Senha));

        return await usuarioRepository.InsertAsync(usuario, cancellationToken);
    }
}
