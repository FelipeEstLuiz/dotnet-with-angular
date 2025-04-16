using Application.Api.Controllers._Shared;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Account")]
public class AccountController(CommunicationProtocol protocol, RequestDispatcher dispatcher) 
    : BaseApplicationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<LoginDto>))]
    public async Task<IActionResult> InsertUsuarioAsync([FromBody] CadastrarUsuarioModel request) 
        => HandlerResponse(
            HttpStatusCode.Created, 
            await dispatcher.Dispatch<CadastrarUsuarioModel, Result<LoginDto>>(request)
        );

    [HttpPost("Login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<LoginDto?>))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel request) 
        => HandlerResponse(HttpStatusCode.OK, await dispatcher.Dispatch<LoginModel, Result<LoginDto>>(request));
}
