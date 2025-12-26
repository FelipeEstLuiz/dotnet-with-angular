using Application.Core.Common.Dispatcher;
using Application.Core.DTO.Message;
using Application.Core.Model.Message;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Api.SignalR;

[Authorize]
public class MessageHub(RequestDispatcher dispatcher, IHubContext<PresenceHub> presenceHub) : Hub
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();
        string otherUser = httpContext?.Request.Query["userId"].ToString() ?? throw new HubException("Other user not found");
        string groupName = GetGroupName(GetUserId(), otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await AddToGroupAsync(groupName);

        Result<IEnumerable<MessageDto>> result = await dispatcher.Dispatch<GetMessageThreadModel, Result<IEnumerable<MessageDto>>>(
            new GetMessageThreadModel(GetUserId(), otherUser)
        );

        if (result.IsFailure)
            throw new HubException(string.Join(", ", result.Errors));

        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", result.Data);
    }

    public async Task SendMessage(CreateMessageModel request)
    {
        request.UserId = GetUserId();

        string groupName = GetGroupName(request.UserId, request.RecipientId);

        Result<GroupDto> responseGroup = await dispatcher.Dispatch<GetGroupModel, Result<GroupDto>>(new GetGroupModel(groupName));

        if (responseGroup.IsFailure)
            throw new HubException(string.Join(", ", responseGroup.Errors));

        GroupDto? group = responseGroup.Data;

        bool userInGroup = group is not null && group.Connections.Any(x => x.UserId == request.RecipientId);

        if (userInGroup)
            request.DateRead = DateTime.UtcNow;

        Result<MessageDto> result = await dispatcher.Dispatch<CreateMessageModel, Result<MessageDto>>(request);

        if (result.IsFailure)
            throw new HubException(string.Join(", ", result.Errors));
        else if (result.Data is null)
            return;

        MessageDto message = result.Data!;

        await Clients.Group(groupName).SendAsync("NewMessage", message);
        List<string> connections = await PresenceTracker.GetConnectionsForUser(request.RecipientId);

        if (connections is not null && connections.Count > 0 && !userInGroup)
        {
            await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", message);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _ = await dispatcher.Dispatch<RemoveGroupModel, Result<bool>>(new RemoveGroupModel(Context.ConnectionId));
        await base.OnDisconnectedAsync(exception);
    }

    private async Task<bool> AddToGroupAsync(string groupName)
    {
        AddGroupModel request = new(groupName, Context.ConnectionId, GetUserId());

        Result<bool> result = await dispatcher.Dispatch<AddGroupModel, Result<bool>>(request);

        return result.IsSuccess ? result.Data : throw new HubException(string.Join(", ", result.Errors));
    }

    private static string GetGroupName(string caller, string other)
    {
        bool stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    private string GetUserId() => Context.User?.GetUserId() ?? throw new HubException("Cannot get user id");
}
