using Application.Api.Controllers._Shared;
using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Command.Usuario;
using Application.Core.Mediator.Query.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Usuario")]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Response))]
public class UsuarioController(CommunicationProtocol protocol, IMediator mediator) : BaseApplicationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> InsertUsuarioAsync([FromBody] CadastrarUsuarioCommand request)
        => HandlerResponse(HttpStatusCode.Created, await mediator.Send(request));

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsuarioDto>>))]
    public async Task<IActionResult> GetAllAsync()
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new GetAllUsuarioQuery()));

    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsuarioDto>>))]
    public async Task<IActionResult> GetByIdAsync(int id)
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new GetUsuarioByIdQuery(id)));
}
