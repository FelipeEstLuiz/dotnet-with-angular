using Application.Core.DTO.Usuario;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Query.Usuario;

public record GetAllUsuarioQuery : IRequest<Result<IEnumerable<UsuarioDto>>>;
