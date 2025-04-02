using Application.Core.Mediator.Command.Usuario;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Domain.Util;
using MediatR;

namespace Application.Core.Mediator.Handler.Usuario;

public class CadastrarUsuarioHandler(IUsuarioRepository usuarioRepository)
    : IRequestHandler<CadastrarUsuarioCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        CadastrarUsuarioCommand request,
        CancellationToken cancellationToken
    ) => await usuarioRepository.InsertAsync(Domain.Entities.Usuario.Create(
        nome: request.Nome,
        email: request.Email,
        password: PasswordHasher.HashPassword(request.Senha)
    ), cancellationToken);
}
