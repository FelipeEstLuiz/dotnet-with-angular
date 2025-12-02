using System.Collections.Concurrent;

namespace Application.Api.SignalR;

public class PresenceTracker
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> OnlineUsers = new();

#pragma warning disable S2325
    public Task UserConnected(string userId, string connectionId)
    {
        ConcurrentDictionary<string, byte> connections = OnlineUsers.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());
        connections.TryAdd(connectionId, 0);
        return Task.CompletedTask;
    }

    public Task UserDisconnected(string userId, string connectionId)
    {
        if (OnlineUsers.TryGetValue(userId, out ConcurrentDictionary<string, byte>? connections))
        {
            connections.TryRemove(connectionId, out _);

            if (connections.IsEmpty)
                OnlineUsers.TryRemove(userId, out _);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<string>> GetOnlineUsers()
    {
        IEnumerable<string> onlineUsers = OnlineUsers.Keys.OrderBy(k => k);
        return Task.FromResult(onlineUsers);
    }

    public static Task<List<string>> GetConnectionsForUser(string userId)
    {
        return OnlineUsers.TryGetValue(userId, out ConcurrentDictionary<string, byte>? connections)
            ? Task.FromResult(connections.Keys.ToList())
            : Task.FromResult(new List<string>());
    }
#pragma warning restore S2325
}
