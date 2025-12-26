using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model.Like;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "Like")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class LikeController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseAuthorizationController(protocol)
{
    [HttpPost("{targetUserId}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> ToggleLikeAsync(string targetUserId) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<ToggleLikeModel, Result<bool>>(new ToggleLikeModel(targetUserId, User.GetUserId()))
    );

    [HttpGet("list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IReadOnlyList<string>>))]
    public async Task<IActionResult> GetCurrentUserLikeIdsAsync() => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<UserIdLikeModel, Result<IReadOnlyList<string>>>(new UserIdLikeModel(User.GetUserId()))
    );

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseServerSide<IEnumerable<UserDto>>))]
    public async Task<IActionResult> GetUserLikesAsync([FromQuery] GetUserLikesModel request)
    {
        request.UserId = User.GetUserId();
        return HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<GetUserLikesModel, Result<IEnumerable<UserDto>>>(request)
        );
    }
}
