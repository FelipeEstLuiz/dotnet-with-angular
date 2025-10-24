using Application.Api.Util;
using Application.Core.DTO.User;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.Controllers._Shared;

[ApiController]
[Route("api/app/v{version:apiVersion}/[controller]")]
[ApiExplorerSettings(GroupName = "Application")]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResponseError))]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ResponseError))]
[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ResponseError))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ResponseError))]
[ProducesResponseType((int)HttpStatusCode.Forbidden, Type = typeof(ResponseError))]
public class BaseApplicationController(CommunicationProtocol protocol) : BaseController(protocol)
{
}
