using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.Controllers.V1;

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
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateUserModel request)
    {
        request.Id = id;
        request.NameToken = User.GetUserName();
        return HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<UpdateUserModel, Result<bool>>(request)
        );
    }

    [HttpPost("add-photo")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<PhotoUserDto>))]
    public async Task<IActionResult> AddPhoto(IFormFile file) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<PhotoUploadModel, Result<PhotoUserDto>>(new PhotoUploadModel(file, User.GetUserName()))
    );
}
