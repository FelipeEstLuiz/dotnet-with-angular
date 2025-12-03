using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.Admin;
using Application.Core.DTO.User;
using Application.Core.Model.Admin;
using Application.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "Admin")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class AdminController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseApplicationController(protocol)
{
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<UsersRolesDto>>))]
    public async Task<IActionResult> GetUsersWithRoles() => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetUsersRolesModel, Result<IEnumerable<UsersRolesDto>>>(new GetUsersRolesModel())
    );

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{userId}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<string>>))]
    public async Task<IActionResult> EditRoles(string userId, [FromQuery] string roles) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<EditUserRolesModel, Result<IEnumerable<string>>>(new EditUserRolesModel(userId, roles))
    );

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<PhotoUserDto>>))]
    public async Task<IActionResult> GetPhotoForModeration() => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<GetPhotoForModerateModel, Result<IEnumerable<PhotoUserDto>>>(new GetPhotoForModerateModel())
    );

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("reject-photo/{photoId}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> RejectPhoto(int photoId) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<ModeratePhotoRejectModel, Result<bool>>(new ModeratePhotoRejectModel(photoId))
    );

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("approve-photo/{photoId}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> ApprovePhoto(int photoId) => HandlerResponse(
        HttpStatusCode.OK,
        await dispatcher.Dispatch<ModeratePhotoApproveModel, Result<bool>>(new ModeratePhotoApproveModel(photoId))
    );
}
