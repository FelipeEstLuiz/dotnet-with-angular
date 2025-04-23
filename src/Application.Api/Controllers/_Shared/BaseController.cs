using Application.Domain.Enums;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers._Shared;

[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
[SwaggerResponse(200, Type = typeof(Response))]
[SwaggerResponse(400, Type = typeof(Response))]
[SwaggerResponse(401, Type = typeof(Response))]
[SwaggerResponse(403, Type = typeof(Response))]
public class BaseController(CommunicationProtocol protocol) : ControllerBase
{
    protected readonly CommunicationProtocol _protocol = protocol;

    protected IActionResult HandlerResponse<T>(HttpStatusCode statusCode, Result<T> result)
    {
        if (result.IsSuccess)
            return StatusCode((int)statusCode, _Shared.Response.ResponseSuccess(
                result.Data,
                protocol: _protocol.ToString(),
                statusCode: statusCode
            ));

        HttpStatusCode httpStatusCode = result.ResponseCode switch
        {
            ResponseCodes.USER_NOT_HAVE_PERMISSION => HttpStatusCode.Forbidden,
            ResponseCodes.UNAUTHORIZED => HttpStatusCode.Unauthorized,
            ResponseCodes.NOT_FOUND => HttpStatusCode.NotFound,
            _ => HttpStatusCode.BadRequest,
        };

        return StatusCode((int)statusCode, _Shared.Response.Failure(
            _protocol.ToString(),
            result.Errors,
            statusCode: httpStatusCode
        ));
    }
}
