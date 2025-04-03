using Application.Core.DTO.Usuario;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Query.Usuario;

public class GetUsuarioByIdQuery(int Id) : IRequest<Result<UsuarioDto?>>
{
    public int Id { get; } = Id;
}
