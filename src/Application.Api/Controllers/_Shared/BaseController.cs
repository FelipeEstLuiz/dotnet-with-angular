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
        if (result.IsFailure)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;

            switch (result.ResponseCode)
            {
                case ResponseCodes.USER_NOT_HAVE_PERMISSION:
                    httpStatusCode = HttpStatusCode.Forbidden;
                    break;
                case ResponseCodes.UNAUTHORIZED:
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    break;
                case ResponseCodes.BAD_REQUEST:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
            }

            return ErrorResponse(httpStatusCode, result.Errors);
        }

        return result.IsSuccess
            ? StatusCode((int)statusCode, new Response(result.Data, protocol: _protocol.ToString()))
            : ErrorResponse(HttpStatusCode.BadRequest, result.Errors);
    }

    protected IActionResult ErrorResponse(HttpStatusCode statusCode, IEnumerable<string> errors)
    {
        Response response = new(_protocol.ToString());
        response.AddError(errors);
        return StatusCode((int)statusCode, response);
    }
}
