using Application.Api.Controllers._Shared;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "User")]
public class UserController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseAuthorizationController(protocol)
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetAllAsync() => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetAllUserModel, Result<IEnumerable<UserDto>>>(new GetAllUserModel())
    );

    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetByIdAsync(int id) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetUserByIdModel, Result<UserDto?>>(new GetUserByIdModel(id))
    );

    [HttpGet("{userName}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetByUserNameAsync(string userName) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetUserByUserNameModel, Result<UserDto?>>(new GetUserByUserNameModel(userName))
    );

    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<string>))]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateUserModel request)
    {
        request.Id = id;
        return HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<UpdateUserModel, Result<string>>(request)
        );
    }
}
