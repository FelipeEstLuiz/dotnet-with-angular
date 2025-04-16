using Application.Api.Controllers._Shared;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.Usuario;
using Application.Core.Model;
using Application.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Usuario")]
public class UsuarioController(CommunicationProtocol protocol, RequestDispatcher dispatcher) 
    : BaseAuthorizationController(protocol)
{
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsuarioDto>>))]
    public async Task<IActionResult> GetAllAsync() => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetAllUsuarioModel, Result<IEnumerable<UsuarioDto>>>(new GetAllUsuarioModel())
    );

    [HttpGet("{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsuarioDto>>))]
    public async Task<IActionResult> GetByIdAsync(Guid id) => HandlerResponse(
        HttpStatusCode.OK, 
        await dispatcher.Dispatch<GetUsuarioByIdModel, Result<UsuarioDto?>>(new GetUsuarioByIdModel(id))
    );
}
