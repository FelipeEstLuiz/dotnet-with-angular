using Application.Api.Controllers._Shared;
using Application.Core.DTO.Usuario;
using Application.Core.Mediator.Command.Login;
using Application.Core.Mediator.Command.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Account")]
public class AccountController(CommunicationProtocol protocol, IMediator mediator) : BaseApplicationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> InsertUsuarioAsync([FromBody] CadastrarUsuarioCommand request)
       => HandlerResponse(HttpStatusCode.Created, await mediator.Send(request));

    [HttpPost("Login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<UsuarioDto?>))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand request)
      => HandlerResponse(HttpStatusCode.OK, await mediator.Send(request));
}
