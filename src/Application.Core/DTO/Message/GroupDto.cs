namespace Application.Core.DTO.Message;

public class GroupDto
{
    public required string Name { get; set; }
    public IEnumerable<ConnectionDto> Connections { get; set; } = [];
}

public class ConnectionDto(string connectionId, string userId)
{
    public string ConnectionId { get; set; } = connectionId;
    public string UserId { get; set; } = userId;
}
