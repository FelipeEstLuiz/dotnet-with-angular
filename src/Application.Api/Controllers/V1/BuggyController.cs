using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Domain.Enums;
using Application.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "Buggy")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class BuggyController(CommunicationProtocol protocol) : BaseApplicationController(protocol)
{
    [HttpGet("auth")]
    public IActionResult GetUnauthorized() => GetError(HttpStatusCode.Unauthorized, ResponseCodes.UNAUTHORIZED);

    [HttpGet("not-found")]
    public IActionResult GetNotFound() => GetError(HttpStatusCode.NotFound, ResponseCodes.NOT_FOUND);

    [HttpGet("server-error")]
    public IActionResult GetServerError() => GetError(HttpStatusCode.InternalServerError, ResponseCodes.SERVER_ERROR);

    [HttpGet("forbidden")]
    public IActionResult GetForbidden() => GetError(HttpStatusCode.Forbidden, ResponseCodes.USER_NOT_HAVE_PERMISSION);

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest() => GetError(HttpStatusCode.BadRequest, ResponseCodes.BAD_REQUEST);

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-secret")]
    public IActionResult GetAdminSecret() => HandlerResponse(
        HttpStatusCode.OK,
        Result<string>.Success("Only admins should see this")
    );

    private IActionResult GetError(HttpStatusCode httpStatusCode, ResponseCodes responseCodes)
       => HandlerResponse(httpStatusCode, Result<string>.Failure(["Bug test response API", httpStatusCode.ToString()], responseCodes));
}
