using Application.Api.Controllers._Shared;
using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Query.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Usuario")]
public class UsuarioController(CommunicationProtocol protocol, IMediator mediator) : BaseApplicationController(protocol)
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsuarioDto>>))]
    public async Task<IActionResult> GetAllAsync()
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new GetAllUsuarioQuery()));

    [HttpGet("{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsuarioDto>>))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new GetUsuarioByIdQuery(id)));
}
