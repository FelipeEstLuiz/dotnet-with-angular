using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Core.Model.User;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "Account")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class AccountController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseApplicationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<LoginDto>))]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Response<LoginDto>))]
    public async Task<IActionResult> InsertUsuarioAsync([FromBody] InsertUserModel request)
        => HandlerResponse(
            HttpStatusCode.Created,
            await dispatcher.Dispatch<InsertUserModel, Result<LoginDto>>(request)
        );

    [HttpPost("Login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<LoginDto>))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel request)
        => HandlerResponse(HttpStatusCode.OK, await dispatcher.Dispatch<LoginModel, Result<LoginDto>>(request));
}
