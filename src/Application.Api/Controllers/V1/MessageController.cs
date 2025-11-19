using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.Message;
using Application.Core.Model.Message;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "Message")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class MessageController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseAuthorizationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<MessageDto>))]
    public async Task<IActionResult> CreateMessageAsync(CreateMessageModel request)
    {
        request.UserId = User.GetUserId();
        return HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<CreateMessageModel, Result<MessageDto>>(request)
        );
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseServerSide<IEnumerable<MessageDto>>))]
    public async Task<IActionResult> GetMessagesAsync([FromQuery] GetMessageModel request)
    {
        request.UserId = User.GetUserId();
        return HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<GetMessageModel, Result<IEnumerable<MessageDto>>>(request)
        );
    }

    [HttpGet("thread/{recipientId}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<MessageDto>>))]
    public async Task<IActionResult> GetMessageThreadAsync(int recipientId)
        => HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<GetMessageThreadModel, Result<IEnumerable<MessageDto>>>(new GetMessageThreadModel(User.GetUserId(), recipientId))
        );

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<MessageDto>>))]
    public async Task<IActionResult> DeleteGetMessageThreadAsync(Guid id)
        => HandlerResponse(
            HttpStatusCode.OK,
            await dispatcher.Dispatch<DeleteMessageModel, Result<bool>>(new DeleteMessageModel(User.GetUserId(), id))
        );
}
