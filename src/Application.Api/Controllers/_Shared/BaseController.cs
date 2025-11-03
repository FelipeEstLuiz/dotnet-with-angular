using Application.Api.Util;
using Application.Domain.Enums;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Application.Api.Controllers._Shared;

[SwaggerResponse(200, Type = typeof(Response))]
public class BaseController(CommunicationProtocol protocol) : ControllerBase
{
    protected readonly CommunicationProtocol _protocol = protocol;

    protected IActionResult HandlerResponse<T>(HttpStatusCode statusCode, Result<T> result)
    {
        Response? response;

        if (result.IsSuccess)
        {
            response = statusCode != HttpStatusCode.NoContent ? Util.Response.ResponseSuccess(
                result.Data,
                protocol: _protocol.ToString(),
                statusCode: statusCode
            ) : null;
        }
        else
        {
            statusCode = result.ResponseCode switch
            {
                ResponseCodes.USER_NOT_HAVE_PERMISSION => HttpStatusCode.Forbidden,
                ResponseCodes.UNAUTHORIZED => HttpStatusCode.Unauthorized,
                ResponseCodes.NOT_FOUND => HttpStatusCode.NotFound,
                ResponseCodes.SERVER_ERROR => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.BadRequest,
            };

            response = Util.Response.Failure(
                _protocol.ToString(),
                result.Errors,
                statusCode: statusCode
            );
        }

        return StatusCode((int)statusCode, response);
    }
}
