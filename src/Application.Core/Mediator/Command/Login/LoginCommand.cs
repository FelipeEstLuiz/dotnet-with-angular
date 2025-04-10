using Application.Core.DTO.Usuario;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Command.Login;

public record LoginCommand : IRequest<Result<LoginDto?>>
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}
