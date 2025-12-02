namespace Application.Domain.Entities;

public class Connection(string connectionId, string userId)
{
    public string ConnectionId { get; set; } = connectionId;
    public string UserId { get; set; } = userId;

    // nav property
    public string GroupName { get; set; } = null!;
    public Group Group { get; set; } = null!;
}
