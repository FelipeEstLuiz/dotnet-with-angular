using Application.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Api.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker presenceTracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        string userId = GetUserId();
        await presenceTracker.UserConnected(userId, Context.ConnectionId);
        await Clients.Others.SendAsync("UserOnline", userId);

        IEnumerable<string> currentUsers = await presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string userId = GetUserId();
        await presenceTracker.UserDisconnected(userId, Context.ConnectionId);
        await Clients.Others.SendAsync("UserOffline", userId);

        IEnumerable<string> currentUsers = await presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);

        await base.OnDisconnectedAsync(exception);
    }

    private string GetUserId() => Context.User?.GetUserId() ?? throw new HubException("Cannot get user id");
}
