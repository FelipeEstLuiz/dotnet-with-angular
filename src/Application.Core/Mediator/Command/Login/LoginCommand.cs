using Application.Core.DTO.Usuario;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Command.Login;

public record LoginCommand : IRequest<Result<UsuarioDto?>>
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}
