using Application.Api.Controllers._Shared;
using Application.Core.Mediator.Command.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Usuario")]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(Response))]
public class UsuarioController(CommunicationProtocol protocol, IMediator mediator) : BaseApplicationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioCommand request)
        => HandlerResponse(HttpStatusCode.Created, await mediator.Send(request));
}
