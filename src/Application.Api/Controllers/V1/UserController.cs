using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Core.Model.User;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "User")]
public class UserController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseAuthorizationController(protocol)
{
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseServerSide<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllUserModel request)
    {
        request.CurrentUserId = User.GetUserId();
        return HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<GetAllUserModel, Result<IEnumerable<UserDto>>>(request)
        );
    }

    [HttpGet("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetByIdAsync(int id) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetUserByIdModel, Result<UserDto?>>(new GetUserByIdModel(id))
    );

    [HttpGet("{userName}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetByUserNameAsync(string userName) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetUserByUserNameModel, Result<UserDto>>(new GetUserByUserNameModel(userName))
    );

    [HttpGet("{id:int}/photos")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<IEnumerable<PhotoUserDto>?>>))]
    public async Task<IActionResult> GetPhotosByIdAsync(int id) => HandlerResponse(
       HttpStatusCode.OK,
       await dispatcher.Dispatch<GetUserPhotoByIdModel, Result<IEnumerable<PhotoUserDto>?>>(new GetUserPhotoByIdModel(id))
   );

    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateUserModel request)
    {
        int userId = User.GetUserId();

        if (id != userId)
            return HandlerResponse(
                HttpStatusCode.BadRequest,
                Result<bool>.Failure("Invalid member to Update", Domain.Enums.ResponseCodes.USER_NOT_FOUND)
            );

        request.Id = id;

        return HandlerResponse(
            HttpStatusCode.NoContent,
            await dispatcher.Dispatch<UpdateUserModel, Result<bool>>(request)
        );
    }

    [HttpPost("add-photo")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<PhotoUserDto>))]
    public async Task<IActionResult> AddPhoto(IFormFile file) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<PhotoUploadModel, Result<PhotoUserDto>>(new PhotoUploadModel(User.GetUserId(), file))
    );

    [HttpPut("set-main-photo/{photoId}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public async Task<IActionResult> SetMainPhotoAsync(int photoId) => HandlerResponse(
        HttpStatusCode.NoContent,
        await dispatcher.Dispatch<UpdatePhotoMainModel, Result<bool>>(new UpdatePhotoMainModel(User.GetUserId(), photoId))
    );

    [HttpDelete("photo/{photoId}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public async Task<IActionResult> DeletePhotoAsync(int photoId) => HandlerResponse(
       HttpStatusCode.NoContent,
       await dispatcher.Dispatch<DeletePhotoModel, Result<bool>>(new DeletePhotoModel(User.GetUserId(), photoId))
   );
}
