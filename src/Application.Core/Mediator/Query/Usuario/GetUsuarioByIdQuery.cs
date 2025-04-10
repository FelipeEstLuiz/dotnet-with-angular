using Application.Core.DTO.Usuario;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Query.Usuario;

public class GetUsuarioByIdQuery(Guid Id) : IRequest<Result<UsuarioDto?>>
{
    public Guid Id { get; } = Id;
}
